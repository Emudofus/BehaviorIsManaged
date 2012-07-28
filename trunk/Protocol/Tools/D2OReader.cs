using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using BiM.Core.Extensions;
using BiM.Core.IO;
using BiM.Protocol.Data;

namespace BiM.Protocol.Tools
{
    public class D2OReader
    {
        private const int NullIdentifier = unchecked((int) 0xAAAAAAAA);

        /// <summary>
        /// Contains all assembly where the reader search d2o class
        /// </summary>
        public static List<Assembly> ClassesContainers = new List<Assembly>
            {
                typeof (Breed).Assembly
            };

        private static readonly Dictionary<Type, Func<object[], object>> objectCreators =
            new Dictionary<Type, Func<object[], object>>();

        private readonly string m_filePath;


        private int m_classcount;
        private Dictionary<int, D2OClassDefinition> m_classes;
        private int m_headeroffset;
        private Dictionary<int, int> m_indextable = new Dictionary<int, int>();
        private int m_indextablelen;
        private BigEndianReader m_reader;

        /// <summary>
        ///   Create and initialise a new D2o file
        /// </summary>
        /// <param name = "filePath">Path of the file</param>
        public D2OReader(string filePath)
            : this(new MemoryStream(File.ReadAllBytes(filePath)))
        {
            m_filePath = filePath;
        }

        public D2OReader(Stream stream)
        {
            Initialize(stream);
        }

        public string FilePath
        {
            get
            {
                return m_filePath;
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        public int IndexCount
        {
            get
            {
                return m_indextable.Count;
            }
        }

        public Dictionary<int, D2OClassDefinition> Classes
        {
            get
            {
                return m_classes;
            }
        }

        public Dictionary<int, int> Indexes
        {
            get
            {
                return m_indextable;
            }
        }

        private void Initialize(Stream stream)
        {
            m_reader = new BigEndianReader(stream);

            string header = m_reader.ReadUTFBytes(3);

            if (header != "D2O")
            {
                throw new Exception("Header doesn't equal the string \'D2O\' : Corrupted file");
            }

            ReadIndexTable();
            ReadClassesTable();
        }

        private void ReadIndexTable()
        {
            m_headeroffset = m_reader.ReadInt();
            m_reader.Seek(m_headeroffset, SeekOrigin.Begin); // place the reader at the beginning of the indextable
            m_indextablelen = m_reader.ReadInt();

            // init table index
            m_indextable = new Dictionary<int, int>(m_indextablelen/8);
            for (int i = 0; i < m_indextablelen; i += 8)
            {
                m_indextable.Add(m_reader.ReadInt(), m_reader.ReadInt());
            }
        }

        private void ReadClassesTable()
        {
            m_classcount = m_reader.ReadInt();
            m_classes = new Dictionary<int, D2OClassDefinition>(m_classcount);
            for (int i = 0; i < m_classcount; i++)
            {
                int classId = m_reader.ReadInt();

                string classMembername = m_reader.ReadUTF();
                string classPackagename = m_reader.ReadUTF();

                Type classType = FindType(classMembername);

                int fieldscount = m_reader.ReadInt();
                var fields = new List<D2OFieldDefinition>(fieldscount);
                for (int l = 0; l < fieldscount; l++)
                {
                    string fieldname = m_reader.ReadUTF();
                    var fieldtype = (D2OFieldType) m_reader.ReadInt();

                    var vectorTypes = new List<Tuple<D2OFieldType, string>>();
                    if (fieldtype == D2OFieldType.List)
                    {
                        addVectorType:

                        string name = m_reader.ReadUTF();
                        var id = (D2OFieldType) m_reader.ReadInt();
                        vectorTypes.Add(Tuple.Create(id, name));

                        if (id == D2OFieldType.List)
                            goto addVectorType;
                    }

                    FieldInfo field = classType.GetField(fieldname);

                    if (field == null)
                        throw new Exception(string.Format("Missing field '{0}' in class '{1}'", fieldname, classType.Name));

                    fields.Add(new D2OFieldDefinition(fieldname, fieldtype, field, m_reader.BaseStream.Position,
                                                      vectorTypes.ToArray()));
                }

                m_classes.Add(classId,
                              new D2OClassDefinition(classId, classMembername, classPackagename, classType, fields,
                                                     m_reader.BaseStream.Position));
            }
        }

        private static Type FindType(string className)
        {
            IEnumerable<Type> correspondantTypes = from asm in ClassesContainers
                                                   let types = asm.GetTypes()
                                                   from type in types
                                                   where type.Name.Equals(className, StringComparison.InvariantCulture) && type.HasInterface(typeof(IDataObject))
                                                   select type;

            return correspondantTypes.Single();
        }

        private bool IsTypeDefined(Type type)
        {
            return m_classes.Count(entry => entry.Value.ClassType == type) > 0;
        }

        /// <summary>
        ///   Get all objects that corresponding to T associated to his index
        /// </summary>
        /// <typeparam name = "T">Corresponding class</typeparam>
        /// <param name = "allownulled">True to adding null instead of throwing an exception</param>
        /// <returns></returns>
        public Dictionary<int, T> ReadObjects<T>(bool allownulled = false)
            where T : class, IDataObject
        {
            if (!IsTypeDefined(typeof (T)))
                throw new Exception("The file doesn't contain this class");

            var result = new Dictionary<int, T>(m_indextable.Count);

            using (BigEndianReader reader = CloneReader())
            {
                foreach (var index in m_indextable)
                {
                    reader.Seek(index.Value, SeekOrigin.Begin);

                    int classid = reader.ReadInt();

                    if (m_classes[classid].ClassType == typeof (T) ||
                        m_classes[classid].ClassType.IsSubclassOf(typeof (T)))
                    {
                        try
                        {
                            result.Add(index.Key, BuildObject(m_classes[classid], reader)as T);
                        }
                        catch
                        {
                            if (allownulled)
                                result.Add(index.Key, default(T));
                            else
                                throw;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///   Get all objects associated to his index
        /// </summary>
        /// <param name = "allownulled">True to adding null instead of throwing an exception</param>
        /// <returns></returns>
        public Dictionary<int, object> ReadObjects(bool allownulled = false)
        {
            var result = new Dictionary<int, object>(m_indextable.Count);

            using (BigEndianReader reader = CloneReader())
            {
                foreach (var index in m_indextable)
                {
                    reader.Seek(index.Value, SeekOrigin.Begin);

                    try
                    {
                        result.Add(index.Key, ReadObject(index.Key, reader));
                    }
                    catch
                    {
                        if (allownulled)
                            result.Add(index.Key, null);
                        else
                            throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///   Get an object from his index
        /// </summary>
        /// <param name="cloneReader">When sets to true it copies the reader to have a thread safe method</param>
        /// <returns></returns>
        public object ReadObject(int index, bool cloneReader = false)
        {
            if (cloneReader)
            {
                using (BigEndianReader reader = CloneReader())
                {
                    return ReadObject(index, reader);
                }
            }

            return ReadObject(index, m_reader);
        }

        private object ReadObject(int index, BigEndianReader reader)
        {
            int offset = m_indextable[index];
            reader.Seek(offset, SeekOrigin.Begin);

            int classid = reader.ReadInt();

            object result = BuildObject(m_classes[classid], reader);

            return result;
        }

        private object BuildObject(D2OClassDefinition classDefinition, BigEndianReader reader)
        {
            if (!objectCreators.ContainsKey(classDefinition.ClassType))
            {
                Func<object[], object> creator = CreateObjectBuilder(classDefinition.ClassType,
                                                                     classDefinition.Fields.Select(
                                                                         entry => entry.Value.FieldInfo).ToArray());

                objectCreators.Add(classDefinition.ClassType, creator);
            }

            var values = new List<object>();
            foreach (D2OFieldDefinition field in classDefinition.Fields.Values)
            {
                object fieldValue = ReadField(reader, field, field.TypeId);

                if (field.FieldType.IsAssignableFrom(fieldValue.GetType()))
                    values.Add(fieldValue);
                else if (fieldValue is IConvertible &&
                         field.FieldType.GetInterface("IConvertible") != null)
                {
                    try
                    {
                        if (fieldValue is int && ((int)fieldValue) < 0 && field.FieldType == typeof(uint))
                            values.Add(unchecked ((uint)((int)fieldValue)));
                        else
                            values.Add(Convert.ChangeType(fieldValue, field.FieldType));
                    }
                    catch
                    {
                        throw new Exception(string.Format("Field '{0}.{1}' with value {2} is not of type '{3}'", classDefinition.Name,
                                                          field.Name, fieldValue, fieldValue.GetType()));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Field '{0}.{1}' with value {2} is not of type '{3}'", classDefinition.Name,
                                                      field.Name, fieldValue, fieldValue.GetType()));
                }
            }

            return objectCreators[classDefinition.ClassType](values.ToArray());
        }

        public T ReadObject<T>(int index)
            where T : class, IDataObject
        {
            using (BigEndianReader reader = CloneReader())
            {
                return ReadObject<T>(index, reader);
            }
        }

        private T ReadObject<T>(int index, BigEndianReader reader)
            where T : class, IDataObject
        {
            if (!IsTypeDefined(typeof (T)))
                throw new Exception("The file doesn't contain this class");

            int offset = m_indextable[index];
            reader.Seek(offset, SeekOrigin.Begin);

            int classid = reader.ReadInt();

            if (m_classes[classid].ClassType != typeof (T) && !m_classes[classid].ClassType.IsSubclassOf(typeof (T)))
                throw new Exception(string.Format("Wrong type, try to read object with {1} instead of {0}",
                                                  typeof (T).Name, m_classes[classid].ClassType.Name));

            return BuildObject(m_classes[classid], reader) as T;
        }

        public Dictionary<int, D2OClassDefinition> GetObjectsClasses()
        {
            return m_indextable.ToDictionary(index => index.Key, index => GetObjectClass(index.Key));
        }


        /// <summary>
        /// Get the class corresponding to the object at the given index
        /// </summary>
        public D2OClassDefinition GetObjectClass(int index)
        {
            int offset = m_indextable[index];
            m_reader.Seek(offset, SeekOrigin.Begin);

            int classid = m_reader.ReadInt();

            return m_classes[classid];
        }

        private object ReadField(BigEndianReader reader, D2OFieldDefinition field, D2OFieldType typeId,
                                 int vectorDimension = 0)
        {
            switch (typeId)
            {
                case D2OFieldType.Int:
                    return ReadFieldInt(reader);
                case D2OFieldType.Bool:
                    return ReadFieldBool(reader);
                case D2OFieldType.String:
                    return ReadFieldUTF(reader);
                case D2OFieldType.Double:
                    return ReadFieldDouble(reader);
                case D2OFieldType.I18N:
                    return ReadFieldI18n(reader);
                case D2OFieldType.UInt:
                    return ReadFieldUInt(reader);
                case D2OFieldType.List:
                    return ReadFieldVector(reader, field, vectorDimension);
                default:
                    return ReadFieldObject(reader);
            }
        }


        private object ReadFieldVector(BigEndianReader reader, D2OFieldDefinition field, int vectorDimension)
        {
            int count = reader.ReadInt();

            Type vectorType = field.FieldType;
            for (int i = 0; i < vectorDimension; i++)
            {
                vectorType = vectorType.GetGenericArguments()[0];
            }

            if (!objectCreators.ContainsKey(vectorType))
            {
                Func<object[], object> creator = CreateObjectBuilder(vectorType, new FieldInfo[0]);

                objectCreators.Add(vectorType, creator);
            }

            var result = objectCreators[vectorType](new object[0]) as IList;

            for (int i = 0; i < count; i++)
            {
                vectorDimension++;
                // i didn't found a way to have thez correct dimension so i just add "- 1"
                result.Add(ReadField(reader, field, field.VectorTypes[vectorDimension - 1].Item1, vectorDimension));
                vectorDimension--;
            }

            return result;
        }

        private object ReadFieldObject(BigEndianReader reader)
        {
            int classid = reader.ReadInt();

            if (classid == NullIdentifier)
                return null;

            if (Classes.Keys.Contains(classid))
                return BuildObject(Classes[classid], reader);

            return null;
        }

        private static int ReadFieldInt(BigEndianReader reader)
        {
            return reader.ReadInt();
        }

        private static uint ReadFieldUInt(BigEndianReader reader)
        {
            return reader.ReadUInt();
        }

        private static bool ReadFieldBool(BigEndianReader reader)
        {
            return reader.ReadByte() != 0;
        }

        private static string ReadFieldUTF(BigEndianReader reader)
        {
            return reader.ReadUTF();
        }

        private static double ReadFieldDouble(BigEndianReader reader)
        {
            return reader.ReadDouble();
        }

        private static int ReadFieldI18n(BigEndianReader reader)
        {
            return reader.ReadInt();
        }

        internal BigEndianReader CloneReader()
        {
            if (m_reader.BaseStream.Position > 0)
                m_reader.Seek(0, SeekOrigin.Begin);

            Stream streamClone = new MemoryStream();
            m_reader.BaseStream.CopyTo(streamClone);

            return new BigEndianReader(streamClone);
        }

        public void Close()
        {
            m_reader.Dispose();
        }

        private static Func<object[], object> CreateObjectBuilder(Type classType, params FieldInfo[] fields)
        {
            IEnumerable<Type> fieldsType = from entry in fields
                                           select entry.FieldType;

            var method = new DynamicMethod(Guid.NewGuid().ToString("N"), typeof (object),
                                           new[] {typeof (object[])}.ToArray());

            ILGenerator ilGenerator = method.GetILGenerator();

            ilGenerator.DeclareLocal(classType);
            ilGenerator.DeclareLocal(classType);

            ilGenerator.Emit(OpCodes.Newobj, classType.GetConstructor(Type.EmptyTypes));
            ilGenerator.Emit(OpCodes.Stloc_0);
            for (int i = 0; i < fields.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);

                if (fields[i].FieldType.IsClass)
                    ilGenerator.Emit(OpCodes.Castclass, fields[i].FieldType);
                else
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, fields[i].FieldType);
                }

                ilGenerator.Emit(OpCodes.Stfld, fields[i]);
            }

            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ret);

            return
                (Func<object[], object>)
                method.CreateDelegate(Expression.GetFuncType(new[] {typeof (object[]), typeof (object)}.ToArray()));
        }
    }
}