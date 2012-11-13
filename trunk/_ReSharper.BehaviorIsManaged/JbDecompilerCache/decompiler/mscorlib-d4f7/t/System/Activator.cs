// Type: System.Activator
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Configuration.Assemblies;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Security;
using System.Security.Policy;
using System.Threading;

namespace System
{
  [ClassInterface(ClassInterfaceType.None)]
  [ComVisible(true)]
  [ComDefaultInterface(typeof (_Activator))]
  public sealed class Activator : _Activator
  {
    internal const int LookupMask = 255;
    internal const BindingFlags ConLookup = 20;
    internal const BindingFlags ConstructorDefault = 532;

    private Activator()
    {
    }

    [SecuritySafeCritical]
    public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
    {
      return Activator.CreateInstance(type, bindingAttr, binder, args, culture, (object[]) null);
    }

    [SecuritySafeCritical]
    public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      if (type is TypeBuilder)
        throw new NotSupportedException(Environment.GetResourceString("NotSupported_CreateInstanceWithTypeBuilder"));
      if ((bindingAttr & (BindingFlags) 255) == BindingFlags.Default)
        bindingAttr |= BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
      if (activationAttributes != null && activationAttributes.Length > 0)
      {
        if (!type.IsMarshalByRef)
          throw new NotSupportedException(Environment.GetResourceString("NotSupported_ActivAttrOnNonMBR"));
        if (!type.IsContextful && (activationAttributes.Length > 1 || !(activationAttributes[0] is UrlAttribute)))
          throw new NotSupportedException(Environment.GetResourceString("NotSupported_NonUrlAttrOnMBR"));
      }
      RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
      if (runtimeType == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "type");
      else
        return runtimeType.CreateInstanceImpl(bindingAttr, binder, args, culture, activationAttributes);
    }

    [SecuritySafeCritical]
    public static object CreateInstance(Type type, params object[] args)
    {
      return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, args, (CultureInfo) null, (object[]) null);
    }

    [SecuritySafeCritical]
    public static object CreateInstance(Type type, object[] args, object[] activationAttributes)
    {
      return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, args, (CultureInfo) null, activationAttributes);
    }

    [SecuritySafeCritical]
    public static object CreateInstance(Type type)
    {
      return Activator.CreateInstance(type, false);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ObjectHandle CreateInstance(string assemblyName, string typeName)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null, (Evidence) null, ref stackMark);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, activationAttributes, (Evidence) null, ref stackMark);
    }

    public static object CreateInstance(Type type, bool nonPublic)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
      if (runtimeType == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "type");
      else
        return runtimeType.CreateInstanceDefaultCtor(!nonPublic);
    }

    [SecuritySafeCritical]
    public static T CreateInstance<T>()
    {
      RuntimeType runtimeType = typeof (T) as RuntimeType;
      if (runtimeType.HasElementType)
        throw new MissingMethodException(Environment.GetResourceString("Arg_NoDefCTor"));
      else
        return (T) runtimeType.CreateInstanceDefaultCtor(true, true, true, true);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
    {
      return Activator.CreateInstanceFrom(assemblyFile, typeName, (object[]) null);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
    {
      return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, activationAttributes);
    }

    [Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo, ref stackMark);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, (Evidence) null, ref stackMark);
    }

    [SecurityCritical]
    internal static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo, ref StackCrawlMark stackMark)
    {
      if (securityInfo != null && !AppDomain.CurrentDomain.IsLegacyCasPolicyEnabled)
        throw new NotSupportedException(Environment.GetResourceString("NotSupported_RequiresCasPolicyImplicit"));
      Assembly assembly = assemblyName != null ? (Assembly) RuntimeAssembly.InternalLoad(assemblyName, securityInfo, ref stackMark, false) : RuntimeAssembly.GetExecutingAssembly(ref stackMark);
      if (assembly == (Assembly) null)
        return (ObjectHandle) null;
      object instance = Activator.CreateInstance(assembly.GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
      if (instance == null)
        return (ObjectHandle) null;
      else
        return new ObjectHandle(instance);
    }

    [Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
    public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
    {
      if (securityInfo != null && !AppDomain.CurrentDomain.IsLegacyCasPolicyEnabled)
        throw new NotSupportedException(Environment.GetResourceString("NotSupported_RequiresCasPolicyImplicit"));
      else
        return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo);
    }

    public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
    {
      return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, (Evidence) null);
    }

    [SecuritySafeCritical]
    private static ObjectHandle CreateInstanceFromInternal(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
    {
      object instance = Activator.CreateInstance(Assembly.LoadFrom(assemblyFile, securityInfo).GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
      if (instance == null)
        return (ObjectHandle) null;
      else
        return new ObjectHandle(instance);
    }

    [SecurityCritical]
    public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      else
        return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName);
    }

    [SecurityCritical]
    [Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
    public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      if (securityAttributes != null && !AppDomain.CurrentDomain.IsLegacyCasPolicyEnabled)
        throw new NotSupportedException(Environment.GetResourceString("NotSupported_RequiresCasPolicyImplicit"));
      else
        return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
    }

    [SecurityCritical]
    public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      else
        return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, (Evidence) null);
    }

    [SecurityCritical]
    public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      else
        return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName);
    }

    [SecurityCritical]
    [Obsolete("Methods which use Evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
    public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      if (securityAttributes != null && !AppDomain.CurrentDomain.IsLegacyCasPolicyEnabled)
        throw new NotSupportedException(Environment.GetResourceString("NotSupported_RequiresCasPolicyImplicit"));
      else
        return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
    }

    [SecurityCritical]
    public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
    {
      if (domain == null)
        throw new ArgumentNullException("domain");
      else
        return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, (Evidence) null);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateInstance(ActivationContext activationContext)
    {
      return (AppDomain.CurrentDomain.DomainManager ?? new AppDomainManager()).ApplicationActivator.CreateInstance(activationContext);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
    {
      return (AppDomain.CurrentDomain.DomainManager ?? new AppDomainManager()).ApplicationActivator.CreateInstance(activationContext, activationCustomData);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
    {
      return Activator.CreateComInstanceFrom(assemblyName, typeName, (byte[]) null, AssemblyHashAlgorithm.None);
    }

    [SecuritySafeCritical]
    public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
    {
      Assembly assembly = Assembly.LoadFrom(assemblyName, hashValue, hashAlgorithm);
      Type type = assembly.GetType(typeName, true, false);
      object[] customAttributes = type.GetCustomAttributes(typeof (ComVisibleAttribute), false);
      if (customAttributes.Length > 0 && !((ComVisibleAttribute) customAttributes[0]).Value)
        throw new TypeLoadException(Environment.GetResourceString("Argument_TypeMustBeVisibleFromCom"));
      if (assembly == (Assembly) null)
        return (ObjectHandle) null;
      object instance = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null);
      if (instance == null)
        return (ObjectHandle) null;
      else
        return new ObjectHandle(instance);
    }

    [SecurityCritical]
    public static object GetObject(Type type, string url)
    {
      return Activator.GetObject(type, url, (object) null);
    }

    [SecurityCritical]
    public static object GetObject(Type type, string url, object state)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("type");
      else
        return RemotingServices.Connect(type, url, state);
    }

    [Conditional("_DEBUG")]
    private static void Log(bool test, string title, string success, string failure)
    {
      int num = test ? 1 : 0;
    }

    void _Activator.GetTypeInfoCount(out uint pcTInfo)
    {
      throw new NotImplementedException();
    }

    void _Activator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
    {
      throw new NotImplementedException();
    }

    void _Activator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
    {
      throw new NotImplementedException();
    }

    void _Activator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
    {
      throw new NotImplementedException();
    }
  }
}
