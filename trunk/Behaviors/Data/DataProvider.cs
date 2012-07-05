using BiM.Core.Reflection;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Data
{
    public class DataProvider : Singleton<DataProvider>
    {
        public T GetObjectData<T>(int id)
            where T : class, IDataObject
        {
            return null;
        }

        public T GetObjectDataOrDefault<T>(int id)
            where T : class, IDataObject
        {
            return null;
        }
    }
}