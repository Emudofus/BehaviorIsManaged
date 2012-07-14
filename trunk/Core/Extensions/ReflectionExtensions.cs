using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace BiM.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type GetActionType(this MethodInfo method)
        {
            return Expression.GetActionType(method.GetParameters().Select(entry => entry.ParameterType).ToArray());
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.FindInterfaces(FilterByName, interfaceType).Length > 0;
        }

        private static bool FilterByName(Type typeObj, Object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider type)
            where T : Attribute
        {
            var attribs = type.GetCustomAttributes(typeof(T), false) as T[];
            return attribs;
        }

        public static T GetCustomAttribute<T>(this ICustomAttributeProvider type)
            where T : Attribute
        {
            return type.GetCustomAttributes<T>().FirstOrDefault();
        }

        public static bool IsDerivedFromGenericType(this Type type, Type genericType)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type == null)
            {
                return false;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            return IsDerivedFromGenericType(type.BaseType, genericType);
        }

        public static T CreateDelegate<T>(this ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToList();

            var lamba = Expression.Lambda<T>(Expression.New(ctor, parameters), parameters);

            return lamba.Compile();
        }

        public static Delegate CreateDelegate(this MethodInfo method, params Type[] delegParams)
        {
            var methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (delegParams.Length != methodParams.Length)
                throw new Exception("Method parameters count != delegParams.Length");

            var dynamicMethod = new DynamicMethod(string.Empty, null, new[] { typeof(object) }.Concat(delegParams).ToArray());
            var ilGenerator = dynamicMethod.GetILGenerator();

            if (!method.IsStatic)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(method.DeclaringType.IsClass ? OpCodes.Castclass : OpCodes.Unbox, method.DeclaringType);
            }

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].IsSubclassOf(delegParams[i]) || methodParams[i].HasInterface(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception(string.Format("Cannot cast {0} to {1}", methodParams[i].Name, delegParams[i].Name));
            }

            ilGenerator.Emit(OpCodes.Call, method);

            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(Expression.GetActionType(new[] {typeof(object)}.Concat(delegParams).ToArray()));
        }
    }
}