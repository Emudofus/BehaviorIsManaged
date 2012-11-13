// Type: System.Enum
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System
{
  [ComVisible(true)]
  [Serializable]
  public abstract class Enum : ValueType, IComparable, IFormattable, IConvertible
  {
    private static readonly char[] enumSeperatorCharArray = new char[1]
    {
      ','
    };
    private static Hashtable fieldInfoHash = Hashtable.Synchronized(new Hashtable());
    private const string enumSeperator = ", ";
    private const int maxHashElements = 100;

    static Enum()
    {
    }

    [SecuritySafeCritical]
    private static Enum.HashEntry GetHashEntry(RuntimeType enumType)
    {
      Enum.HashEntry hashEntry = (Enum.HashEntry) Enum.fieldInfoHash[(object) enumType];
      if (hashEntry == null)
      {
        if (Enum.fieldInfoHash.Count > 100)
          Enum.fieldInfoHash.Clear();
        ulong[] o1 = (ulong[]) null;
        string[] o2 = (string[]) null;
        Enum.GetEnumValues(enumType.GetTypeHandleInternal(), JitHelpers.GetObjectHandleOnStack<ulong[]>(ref o1), JitHelpers.GetObjectHandleOnStack<string[]>(ref o2));
        hashEntry = new Enum.HashEntry(o2, o1);
        Enum.fieldInfoHash[(object) enumType] = (object) hashEntry;
      }
      return hashEntry;
    }

    private static string InternalFormattedHexString(object value)
    {
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.SByte:
          return ((byte) (sbyte) value).ToString("X2", (IFormatProvider) null);
        case TypeCode.Byte:
          return ((byte) value).ToString("X2", (IFormatProvider) null);
        case TypeCode.Int16:
          return ((ushort) (short) value).ToString("X4", (IFormatProvider) null);
        case TypeCode.UInt16:
          return ((ushort) value).ToString("X4", (IFormatProvider) null);
        case TypeCode.Int32:
          return ((uint) (int) value).ToString("X8", (IFormatProvider) null);
        case TypeCode.UInt32:
          return ((uint) value).ToString("X8", (IFormatProvider) null);
        case TypeCode.Int64:
          return ((ulong) (long) value).ToString("X16", (IFormatProvider) null);
        case TypeCode.UInt64:
          return ((ulong) value).ToString("X16", (IFormatProvider) null);
        default:
          throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
      }
    }

    private static string InternalFormat(RuntimeType eT, object value)
    {
      if (!eT.IsDefined(typeof (FlagsAttribute), false))
        return Enum.GetName((Type) eT, value) ?? value.ToString();
      else
        return Enum.InternalFlagsFormat(eT, value);
    }

    private static string InternalFlagsFormat(RuntimeType eT, object value)
    {
      ulong num1 = Enum.ToUInt64(value);
      Enum.HashEntry hashEntry = Enum.GetHashEntry(eT);
      string[] strArray = hashEntry.names;
      ulong[] numArray = hashEntry.values;
      int index = numArray.Length - 1;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      ulong num2 = num1;
      for (; index >= 0 && (index != 0 || (long) numArray[index] != 0L); --index)
      {
        if (((long) num1 & (long) numArray[index]) == (long) numArray[index])
        {
          num1 -= numArray[index];
          if (!flag)
            stringBuilder.Insert(0, ", ");
          stringBuilder.Insert(0, strArray[index]);
          flag = false;
        }
      }
      if ((long) num1 != 0L)
        return value.ToString();
      if ((long) num2 != 0L)
        return ((object) stringBuilder).ToString();
      if (numArray.Length > 0 && (long) numArray[0] == 0L)
        return strArray[0];
      else
        return "0";
    }

    internal static ulong ToUInt64(object value)
    {
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.SByte:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
          return (ulong) Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);
        case TypeCode.Byte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
          return Convert.ToUInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);
        default:
          throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
      }
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static int InternalCompareTo(object o1, object o2);

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static RuntimeType InternalGetUnderlyingType(RuntimeType enumType);

    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static void GetEnumValues(RuntimeTypeHandle enumType, ObjectHandleOnStack values, ObjectHandleOnStack names);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static object InternalBoxEnum(RuntimeType enumType, long value);

    [SecuritySafeCritical]
    public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
    {
      return Enum.TryParse<TEnum>(value, false, out result);
    }

    [SecuritySafeCritical]
    public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
    {
      result = default (TEnum);
      Enum.EnumResult parseResult = new Enum.EnumResult();
      parseResult.Init(false);
      bool flag;
      if (flag = Enum.TryParseEnum(typeof (TEnum), value, ignoreCase, ref parseResult))
        result = (TEnum) parseResult.parsedEnum;
      return flag;
    }

    [ComVisible(true)]
    public static object Parse(Type enumType, string value)
    {
      return Enum.Parse(enumType, value, false);
    }

    [ComVisible(true)]
    public static object Parse(Type enumType, string value, bool ignoreCase)
    {
      Enum.EnumResult parseResult = new Enum.EnumResult();
      parseResult.Init(true);
      if (Enum.TryParseEnum(enumType, value, ignoreCase, ref parseResult))
        return parseResult.parsedEnum;
      else
        throw parseResult.GetEnumParseException();
    }

    [SecuritySafeCritical]
    private static bool TryParseEnum(Type enumType, string value, bool ignoreCase, ref Enum.EnumResult parseResult)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      if (value == null)
      {
        parseResult.SetFailure(Enum.ParseFailureKind.ArgumentNull, "value");
        return false;
      }
      else
      {
        value = value.Trim();
        if (value.Length == 0)
        {
          parseResult.SetFailure(Enum.ParseFailureKind.Argument, "Arg_MustContainEnumInfo", (object) null);
          return false;
        }
        else
        {
          ulong num1 = 0UL;
          if (char.IsDigit(value[0]) || (int) value[0] == 45 || (int) value[0] == 43)
          {
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            try
            {
              object obj = Convert.ChangeType((object) value, underlyingType, (IFormatProvider) CultureInfo.InvariantCulture);
              parseResult.parsedEnum = Enum.ToObject(enumType, obj);
              return true;
            }
            catch (FormatException ex)
            {
            }
            catch (Exception ex)
            {
              if (parseResult.canThrow)
              {
                throw;
              }
              else
              {
                parseResult.SetFailure(ex);
                return false;
              }
            }
          }
          string[] strArray1 = value.Split(Enum.enumSeperatorCharArray);
          Enum.HashEntry hashEntry = Enum.GetHashEntry(enumType1);
          string[] strArray2 = hashEntry.names;
          for (int index1 = 0; index1 < strArray1.Length; ++index1)
          {
            strArray1[index1] = strArray1[index1].Trim();
            bool flag = false;
            for (int index2 = 0; index2 < strArray2.Length; ++index2)
            {
              if (ignoreCase)
              {
                if (string.Compare(strArray2[index2], strArray1[index1], StringComparison.OrdinalIgnoreCase) != 0)
                  continue;
              }
              else if (!strArray2[index2].Equals(strArray1[index1]))
                continue;
              ulong num2 = hashEntry.values[index2];
              num1 |= num2;
              flag = true;
              break;
            }
            if (!flag)
            {
              parseResult.SetFailure(Enum.ParseFailureKind.ArgumentWithParameter, "Arg_EnumValueNotFound", (object) value);
              return false;
            }
          }
          try
          {
            parseResult.parsedEnum = Enum.ToObject(enumType, num1);
            return true;
          }
          catch (Exception ex)
          {
            if (parseResult.canThrow)
            {
              throw;
            }
            else
            {
              parseResult.SetFailure(ex);
              return false;
            }
          }
        }
      }
    }

    [SecuritySafeCritical]
    [ComVisible(true)]
    public static Type GetUnderlyingType(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      else
        return enumType.GetEnumUnderlyingType();
    }

    [ComVisible(true)]
    public static Array GetValues(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      else
        return enumType.GetEnumValues();
    }

    internal static ulong[] InternalGetValues(RuntimeType enumType)
    {
      return Enum.GetHashEntry(enumType).values;
    }

    [ComVisible(true)]
    public static string GetName(Type enumType, object value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      else
        return enumType.GetEnumName(value);
    }

    [ComVisible(true)]
    public static string[] GetNames(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      else
        return enumType.GetEnumNames();
    }

    internal static string[] InternalGetNames(RuntimeType enumType)
    {
      return Enum.GetHashEntry(enumType).names;
    }

    [ComVisible(true)]
    public static object ToObject(Type enumType, object value)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.SByte:
          return Enum.ToObject(enumType, (sbyte) value);
        case TypeCode.Byte:
          return Enum.ToObject(enumType, (byte) value);
        case TypeCode.Int16:
          return Enum.ToObject(enumType, (short) value);
        case TypeCode.UInt16:
          return Enum.ToObject(enumType, (ushort) value);
        case TypeCode.Int32:
          return Enum.ToObject(enumType, (int) value);
        case TypeCode.UInt32:
          return Enum.ToObject(enumType, (uint) value);
        case TypeCode.Int64:
          return Enum.ToObject(enumType, (long) value);
        case TypeCode.UInt64:
          return Enum.ToObject(enumType, (ulong) value);
        default:
          throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnumBaseTypeOrEnum"), "value");
      }
    }

    [ComVisible(true)]
    public static bool IsDefined(Type enumType, object value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      else
        return enumType.IsEnumDefined(value);
    }

    [ComVisible(true)]
    public static string Format(Type enumType, object value, string format)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      if (value == null)
        throw new ArgumentNullException("value");
      if (format == null)
        throw new ArgumentNullException("format");
      RuntimeType eT = enumType as RuntimeType;
      if (eT == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      Type type = value.GetType();
      Type underlyingType = Enum.GetUnderlyingType(enumType);
      if (type.IsEnum)
      {
        Enum.GetUnderlyingType(type);
        if (!type.IsEquivalentTo(enumType))
          throw new ArgumentException(Environment.GetResourceString("Arg_EnumAndObjectMustBeSameType", (object) type.ToString(), (object) enumType.ToString()));
        else
          value = ((Enum) value).GetValue();
      }
      else if (type != underlyingType)
        throw new ArgumentException(Environment.GetResourceString("Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType", (object) type.ToString(), (object) underlyingType.ToString()));
      if (format.Length != 1)
        throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
      switch (format[0])
      {
        case 'D':
        case 'd':
          return value.ToString();
        case 'X':
        case 'x':
          return Enum.InternalFormattedHexString(value);
        case 'G':
        case 'g':
          return Enum.InternalFormat(eT, value);
        case 'F':
        case 'f':
          return Enum.InternalFlagsFormat(eT, value);
        default:
          throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
      }
    }

    [SecuritySafeCritical]
    internal object GetValue()
    {
      return this.InternalGetValue();
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private object InternalGetValue();

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public override bool Equals(object obj);

    public override int GetHashCode()
    {
      return this.GetValue().GetHashCode();
    }

    public override string ToString()
    {
      return Enum.InternalFormat((RuntimeType) this.GetType(), this.GetValue());
    }

    [Obsolete("The provider argument is not used. Please use ToString(String).")]
    public string ToString(string format, IFormatProvider provider)
    {
      return this.ToString(format);
    }

    [SecuritySafeCritical]
    public int CompareTo(object target)
    {
      if (this == null)
        throw new NullReferenceException();
      int num = Enum.InternalCompareTo((object) this, target);
      if (num < 2)
        return num;
      if (num != 2)
        throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
      Type type = this.GetType();
      throw new ArgumentException(Environment.GetResourceString("Arg_EnumAndObjectMustBeSameType", (object) target.GetType().ToString(), (object) type.ToString()));
    }

    [SecuritySafeCritical]
    public string ToString(string format)
    {
      if (format == null || format.Length == 0)
        format = "G";
      if (string.Compare(format, "G", StringComparison.OrdinalIgnoreCase) == 0)
        return base.ToString();
      if (string.Compare(format, "D", StringComparison.OrdinalIgnoreCase) == 0)
        return this.GetValue().ToString();
      if (string.Compare(format, "X", StringComparison.OrdinalIgnoreCase) == 0)
        return Enum.InternalFormattedHexString(this.GetValue());
      if (string.Compare(format, "F", StringComparison.OrdinalIgnoreCase) == 0)
        return Enum.InternalFlagsFormat((RuntimeType) this.GetType(), this.GetValue());
      else
        throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
    }

    [Obsolete("The provider argument is not used. Please use ToString().")]
    public string ToString(IFormatProvider provider)
    {
      return base.ToString();
    }

    public bool HasFlag(Enum flag)
    {
      if (!this.GetType().IsEquivalentTo(flag.GetType()))
      {
        throw new ArgumentException(Environment.GetResourceString("Argument_EnumTypeDoesNotMatch", (object) flag.GetType(), (object) this.GetType()));
      }
      else
      {
        ulong num = Enum.ToUInt64(flag.GetValue());
        return ((long) Enum.ToUInt64(this.GetValue()) & (long) num) == (long) num;
      }
    }

    public TypeCode GetTypeCode()
    {
      Type underlyingType = Enum.GetUnderlyingType(this.GetType());
      if (underlyingType == typeof (int))
        return TypeCode.Int32;
      if (underlyingType == typeof (sbyte))
        return TypeCode.SByte;
      if (underlyingType == typeof (short))
        return TypeCode.Int16;
      if (underlyingType == typeof (long))
        return TypeCode.Int64;
      if (underlyingType == typeof (uint))
        return TypeCode.UInt32;
      if (underlyingType == typeof (byte))
        return TypeCode.Byte;
      if (underlyingType == typeof (ushort))
        return TypeCode.UInt16;
      if (underlyingType == typeof (ulong))
        return TypeCode.UInt64;
      else
        throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return Convert.ToBoolean(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return Convert.ToChar(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return Convert.ToSByte(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      return Convert.ToByte(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return Convert.ToInt16(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return Convert.ToUInt16(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return Convert.ToInt32(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return Convert.ToUInt32(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return Convert.ToInt64(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return Convert.ToUInt64(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return Convert.ToSingle(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return Convert.ToDouble(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    Decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return Convert.ToDecimal(this.GetValue(), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", (object) "Enum", (object) "DateTime"));
    }

    object IConvertible.ToType(Type type, IFormatProvider provider)
    {
      return Convert.DefaultToType((IConvertible) this, type, provider);
    }

    [ComVisible(true)]
    [SecuritySafeCritical]
    [CLSCompliant(false)]
    public static object ToObject(Type enumType, sbyte value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [ComVisible(true)]
    [SecuritySafeCritical]
    public static object ToObject(Type enumType, short value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [ComVisible(true)]
    [SecuritySafeCritical]
    public static object ToObject(Type enumType, int value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [SecuritySafeCritical]
    [ComVisible(true)]
    public static object ToObject(Type enumType, byte value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [CLSCompliant(false)]
    [ComVisible(true)]
    [SecuritySafeCritical]
    public static object ToObject(Type enumType, ushort value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [SecuritySafeCritical]
    [CLSCompliant(false)]
    [ComVisible(true)]
    public static object ToObject(Type enumType, uint value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    [ComVisible(true)]
    [SecuritySafeCritical]
    public static object ToObject(Type enumType, long value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, value);
    }

    [CLSCompliant(false)]
    [ComVisible(true)]
    [SecuritySafeCritical]
    public static object ToObject(Type enumType, ulong value)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException("enumType");
      if (!enumType.IsEnum)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
      RuntimeType enumType1 = enumType as RuntimeType;
      if (enumType1 == (RuntimeType) null)
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
      else
        return Enum.InternalBoxEnum(enumType1, (long) value);
    }

    private enum ParseFailureKind
    {
      None,
      Argument,
      ArgumentNull,
      ArgumentWithParameter,
      UnhandledException,
    }

    private struct EnumResult
    {
      internal object parsedEnum;
      internal bool canThrow;
      internal Enum.ParseFailureKind m_failure;
      internal string m_failureMessageID;
      internal string m_failureParameter;
      internal object m_failureMessageFormatArgument;
      internal Exception m_innerException;

      internal void Init(bool canMethodThrow)
      {
        this.parsedEnum = (object) 0;
        this.canThrow = canMethodThrow;
      }

      internal void SetFailure(Exception unhandledException)
      {
        this.m_failure = Enum.ParseFailureKind.UnhandledException;
        this.m_innerException = unhandledException;
      }

      internal void SetFailure(Enum.ParseFailureKind failure, string failureParameter)
      {
        this.m_failure = failure;
        this.m_failureParameter = failureParameter;
        if (this.canThrow)
          throw this.GetEnumParseException();
      }

      internal void SetFailure(Enum.ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument)
      {
        this.m_failure = failure;
        this.m_failureMessageID = failureMessageID;
        this.m_failureMessageFormatArgument = failureMessageFormatArgument;
        if (this.canThrow)
          throw this.GetEnumParseException();
      }

      internal Exception GetEnumParseException()
      {
        switch (this.m_failure)
        {
          case Enum.ParseFailureKind.Argument:
            return (Exception) new ArgumentException(Environment.GetResourceString(this.m_failureMessageID));
          case Enum.ParseFailureKind.ArgumentNull:
            return (Exception) new ArgumentNullException(this.m_failureParameter);
          case Enum.ParseFailureKind.ArgumentWithParameter:
            return (Exception) new ArgumentException(Environment.GetResourceString(this.m_failureMessageID, new object[1]
            {
              this.m_failureMessageFormatArgument
            }));
          case Enum.ParseFailureKind.UnhandledException:
            return this.m_innerException;
          default:
            return (Exception) new ArgumentException(Environment.GetResourceString("Arg_EnumValueNotFound"));
        }
      }
    }

    private class HashEntry
    {
      public string[] names;
      public ulong[] values;

      public HashEntry(string[] names, ulong[] values)
      {
        this.names = names;
        this.values = values;
      }
    }
  }
}
