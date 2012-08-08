using System;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Data
{
    public interface IDataSource
    {
        T ReadObject<T>(params object[] keys) where T : class, IDataObject;
        bool DoesHandleType(Type type);
    }
}