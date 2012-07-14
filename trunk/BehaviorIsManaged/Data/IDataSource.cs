using System;
using BiM.Protocol.Data;

namespace BiM.Data
{
    public interface IDataSource
    {
        T ReadObject<T>(int id) where T : class, IDataObject;
        bool DoesHandleType(Type type);
    }
}