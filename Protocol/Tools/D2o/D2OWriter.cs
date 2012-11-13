#region License GNU GPL
// D2OWriter.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Tools
{
    public class D2OWriter : IDisposable
    {
        public string BakFilename
        {
            get;
            set;
        }

        public string Filename
        {
            get;
            set;
        }

        private const int NullIdentifier = unchecked((int)0xAAAAAAAA);

        private Dictionary<int, D2OClassDefinition> m_classes;
        private Dictionary<int, int> m_indexTable;
        private Dictionary<Type, int> m_allocatedClassId = new Dictionary<Type, int>();
        private Dictionary<int, object> m_objects = new Dictionary<int, object>();

        private object m_writingSync = new object();
        private bool m_writing;
        private bool m_needToBeSync;
        private BigEndianWriter m_writer;

        /// <summary>
        /// Create and flush and empty d2o file
        /// </summary>
        /// <param name="path"></param>
        public static void CreateEmptyFile(string path)
        {
            if (File.Exists(path))
                throw new Exception("File already exists, delete before overwrite");

            var writer = new BinaryWriter(File.OpenWrite(path));

            writer.Write("D2O");
            writer.Write((int)writer.BaseStream.Position + 4); // index table offset

            writer.Write(0); // index table len
            writer.Write(0); // class count

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Create a new instance of D2oWriter
        /// </summary>
        /// <param name="filename"></param>
        public D2OWriter(string filename)
        {
            Filename = filename;

            if (!File.Exists(filename))
                CreateWrite(filename);
            else
                OpenWrite();
        }

        private void CreateWrite(string filename)
        {
            m_writer = new BigEndianWriter(File.Create(filename));

            m_indexTable = new Dictionary<int, int>();
            m_classes = new Dictionary<int, D2OClassDefinition>();
            m_objects = new Dictionary<int, object>();
            m_allocatedClassId = new Dictionary<Type, int>();
        }

        private void OpenWrite()
        {
            m_writer = new BigEndianWriter(File.OpenWrite(Filename));

            ResetMembersByReading();
        }

        private void ResetMembersByReading()
        {
            var reader = new D2OReader(File.OpenRead(Filename));

            m_indexTable = reader.Indexes;
            m_classes = reader.Classes;
            m_allocatedClassId = m_classes.ToDictionary(entry => entry.Value.ClassType, entry => entry.Key);
            m_objects = reader.ReadObjects();

            reader.Close();
        }

        /// <summary>
        /// Start editing of the d2o file
        /// </summary>
        /// <param name="backupFile"></param>
        public void StartWriting(bool backupFile = true)
        {
            if (backupFile)
            {
                BakFilename = Filename + ".bak";
                File.Copy(Filename, BakFilename, true);
            }

            // overwrite existing file
            File.WriteAllBytes(Filename, new byte[0]);

            m_writing = true;
            lock (m_writingSync)
            {
                if (m_needToBeSync)
                {
                    ResetMembersByReading();
                }
            }
        }

        /// <summary>
        /// Stop editing the d2o file, flush the file and dispose ressources
        /// </summary>
        public void EndWriting()
        {
            lock (m_writingSync)
            {
                m_writer.Seek(0, SeekOrigin.Begin);

                m_writing = false;
                m_needToBeSync = false;

                WriteHeader();

                foreach (var obj in m_objects)
                {
                    if (!m_indexTable.ContainsKey(obj.Key))
                        m_indexTable.Add(obj.Key, (int)m_writer.BaseStream.Position);
                    else
                    {
                        m_indexTable[obj.Key] = (int)m_writer.BaseStream.Position;
                    }

                    WriteObject(obj.Value, obj.Value.GetType());
                }

                WriteIndexTable();
                WriteClassesDefinition();

                m_writer.Dispose();
            }
        }

        public void Dispose()
        {
            if (m_writing)
                EndWriting();
        }


        private void WriteHeader()
        {
            m_writer.WriteUTFBytes("D2O");
            m_writer.WriteInt(0); // allocate space to write the correct index table offset
        }

        private void WriteIndexTable()
        {
            int offset = (int)m_writer.BaseStream.Position;

            m_writer.Seek(3, SeekOrigin.Begin);
            m_writer.WriteInt(offset);

            m_writer.Seek(offset, SeekOrigin.Begin);
            m_writer.WriteInt(m_indexTable.Count * 8);

            foreach (var index in m_indexTable)
            {
                m_writer.WriteInt(index.Key);
                m_writer.WriteInt(index.Value);
            }
        }

        private void WriteClassesDefinition()
        {
            m_writer.WriteInt(m_classes.Count);

            foreach (var classDefinition in m_classes.Values)
            {
                classDefinition.Offset = (int)m_writer.BaseStream.Position;
                m_writer.WriteInt(classDefinition.Id);

                m_writer.WriteUTF(classDefinition.Name);
                m_writer.WriteUTF(classDefinition.PackageName);

                m_writer.WriteInt(classDefinition.Fields.Count);

                foreach (var field in classDefinition.Fields.Values)
                {
                    field.Offset = (int)m_writer.BaseStream.Position;
                    m_writer.WriteUTF(field.Name);
                    m_writer.WriteInt((int)field.TypeId);

                    foreach (var vectorType in field.VectorTypes)
                    {
                        m_writer.WriteUTF(vectorType.Item2);
                        m_writer.WriteInt((int)vectorType.Item1);
                    }
                }
            }
        }

        public void Write<T>(T obj)
        {
            Write(obj, m_objects.Count > 0 ? m_objects.Keys.Max() + 1 : 0);
        }

        public void Write<T>(T obj, int index)
        {
            if (!m_writing)
                StartWriting();

            lock (m_writingSync)
            {
                m_needToBeSync = true;

                if (!IsClassDeclared(typeof(T)))
                    // if the class is not allocated then the class is not defined
                    DefineClassDefinition(typeof(T));

                if (m_objects.ContainsKey(index))
                    m_objects[index] = obj;
                else
                    m_objects.Add(index, obj);
            }
        }

        public void Delete(int index)
        {
            lock (m_writingSync)
            {
                if (m_objects.ContainsKey(index))
                    m_objects.Remove(index);
            }
        }

        private bool IsClassDeclared(Type classType)
        {
            return m_allocatedClassId.ContainsKey(classType);
        }

        private int AllocateClassId(Type classType)
        {
            int id = m_allocatedClassId.Count > 0 ? m_allocatedClassId.Values.Max() + 1 : 0;
            AllocateClassId(classType, id);

            return id;
        }

        private void AllocateClassId(Type classType, int classId)
        {
            m_allocatedClassId.Add(classType, classId);
        }

        private void DefineClassDefinition(Type classType)
        {
            if (m_classes.Count(entry => entry.Value.ClassType == ( classType )) > 0) // already define
                return;

            AllocateClassId(classType);

            object[] attributes = classType.GetCustomAttributes(typeof(D2OClassAttribute), false);

            if (attributes.Length != 1)
                throw new Exception("The given class has no D2OClassAttribute attribute and cannot be wrote");

            string package = ( (D2OClassAttribute)attributes[0] ).PackageName;
            string name = !string.IsNullOrEmpty(((D2OClassAttribute) attributes[0]).Name)
                              ? ((D2OClassAttribute) attributes[0]).Name
                              : classType.Name;

            // add fields
            var fields = ( from field in classType.GetFields()
                           let attribute = (D2OFieldAttribute)field.GetCustomAttributes(typeof(D2OFieldAttribute), false).SingleOrDefault()
                           let fieldTypeId = GetIdByType(field.FieldType)
                           let vectorTypes = GetVectorTypes(field.FieldType)
                           let fieldName = attribute != null ? attribute.FieldName : field.Name
                           where field.GetCustomAttributes(typeof(D2OIgnore), false).Count() <= 0
                           select new D2OFieldDefinition(fieldName, fieldTypeId, field, -1, vectorTypes) );

            // add properties
            fields.Concat(from property in classType.GetProperties()
                          let attribute = (D2OFieldAttribute)property.GetCustomAttributes(typeof(D2OFieldAttribute), false).SingleOrDefault()
                          let fieldTypeId = GetIdByType(property.PropertyType)
                          let vectorTypes = GetVectorTypes(property.PropertyType)
                          let fieldName = attribute != null ? attribute.FieldName : property.Name
                          where property.GetCustomAttributes(typeof(D2OIgnore), false).Count() <= 0
                          select new D2OFieldDefinition(fieldName, fieldTypeId, property, -1, vectorTypes));

            m_classes.Add(m_allocatedClassId[classType],
                new D2OClassDefinition(m_allocatedClassId[classType], name, package, classType, fields, -1));

            DefineAllocatedTypes(); // build class definition of allocated types that aren't define
        }

        private void DefineAllocatedTypes()
        {
            foreach (var allocatedClass in m_allocatedClassId.Where(entry => !m_classes.ContainsKey(entry.Value)))
            {
                DefineClassDefinition(allocatedClass.Key);
            }
        }

        private D2OFieldType GetIdByType(Type fieldType)
        {
            if (fieldType == typeof(int))
                return D2OFieldType.Int;
            if (fieldType == typeof(bool))
                return D2OFieldType.Bool;
            if (fieldType == typeof(string))
                return D2OFieldType.String;
            if (fieldType == typeof(double))
                return D2OFieldType.Double;
            if (fieldType == typeof(int)) // that's useless, i know ...
                return D2OFieldType.I18N;
            if (fieldType == typeof(uint))
                return D2OFieldType.UInt;
            if (fieldType.GetGenericTypeDefinition() == typeof(List<>))
                return D2OFieldType.List;

            int classId;
            if (m_allocatedClassId.ContainsKey(fieldType))
            {
                classId = AllocateClassId(fieldType);

                m_allocatedClassId.Add(fieldType, classId);
            }

            classId = m_allocatedClassId[fieldType];

            return (D2OFieldType)classId;
        }

        private Tuple<D2OFieldType, string>[] GetVectorTypes(Type vectorType)
        {
            var ids = new List<Tuple<D2OFieldType, string>>();

            if (vectorType.IsGenericType)
            {
                Type currentGenericType = vectorType;
                Type[] genericArguments = currentGenericType.GetGenericArguments();

                while (genericArguments.Length > 0)
                {
                    ids.Add(Tuple.Create(GetIdByType(genericArguments[0]), genericArguments[0].Name));

                    currentGenericType = genericArguments[0];
                    genericArguments = currentGenericType.GetGenericArguments();
                }
            }

            return ids.ToArray();
        }

        private void WriteObject(object obj, Type type)
        {
            if (!m_allocatedClassId.ContainsKey(obj.GetType()))
                throw new Exception(string.Format("Unexpected object of type {0} (was not registered)", obj.GetType()));

            var @class = m_classes[m_allocatedClassId[type]];

            m_writer.WriteInt(@class.Id);

            foreach (var field in @class.Fields)
            {
                object fieldValue = field.Value.GetValue(obj);

                WriteField(m_writer, field.Value, fieldValue);
            }
        }

        private void WriteField(BigEndianWriter writer, D2OFieldDefinition field, dynamic obj, int vectorDimension = 0)
        {
            switch (field.TypeId)
            {
                case D2OFieldType.Int:
                    WriteFieldInt(writer, (int)obj);
                    break;
                case D2OFieldType.Bool:
                    WriteFieldBool(writer, obj);
                    break;
                case D2OFieldType.String:
                    WriteFieldUTF(writer, obj);
                    break;
                case D2OFieldType.Double:
                    WriteFieldDouble(writer, obj);
                    break;
                case D2OFieldType.I18N:
                    WriteFieldI18n(writer, obj);
                    break;
                case D2OFieldType.UInt:
                    WriteFieldUInt(writer, (uint)obj);
                    break;
                case D2OFieldType.List:
                    WriteFieldVector(writer, field, obj, vectorDimension);
                    break;
                default:
                    WriteFieldObject(writer, obj);
                    break;
            }
        }


        private void WriteFieldVector(BigEndianWriter writer, D2OFieldDefinition field, IList list, int vectorDimension)
        {
            writer.WriteInt(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                WriteField(writer, field, field.VectorTypes[vectorDimension], ++vectorDimension);
            }
        }

        private void WriteFieldObject(BigEndianWriter writer, object obj)
        {
            if (obj == null)
                writer.WriteInt(NullIdentifier);
            else
            {
                if (!m_allocatedClassId.ContainsKey(obj.GetType()))
                    throw new Exception(string.Format("Unexpected object of type {0} (was not registered)", obj.GetType()));

                int classid = m_allocatedClassId[obj.GetType()];
                writer.WriteInt(classid);

                WriteObject(obj, obj.GetType());
            }
        }

        private static void WriteFieldInt(BigEndianWriter writer, int value)
        {
            writer.WriteInt(value);
        }

        private static void WriteFieldUInt(BigEndianWriter writer, uint value)
        {
            writer.WriteUInt(value);
        }

        private static void WriteFieldBool(BigEndianWriter writer, bool value)
        {
            writer.WriteBoolean(value);
        }

        private static void WriteFieldUTF(BigEndianWriter writer, string value)
        {
            writer.WriteUTF(value);
        }

        private static void WriteFieldDouble(BigEndianWriter writer, double value)
        {
            writer.WriteDouble(value);
        }

        private static void WriteFieldI18n(BigEndianWriter writer, int value)
        {
            writer.WriteInt(value);
        }
    }
}