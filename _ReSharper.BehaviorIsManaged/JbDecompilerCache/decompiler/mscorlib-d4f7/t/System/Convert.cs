// Type: System.Convert
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System
{
  public static class Convert
  {
    internal static readonly RuntimeType[] ConvertTypes = new RuntimeType[19]
    {
      (RuntimeType) typeof (Empty),
      (RuntimeType) typeof (object),
      (RuntimeType) typeof (DBNull),
      (RuntimeType) typeof (bool),
      (RuntimeType) typeof (char),
      (RuntimeType) typeof (sbyte),
      (RuntimeType) typeof (byte),
      (RuntimeType) typeof (short),
      (RuntimeType) typeof (ushort),
      (RuntimeType) typeof (int),
      (RuntimeType) typeof (uint),
      (RuntimeType) typeof (long),
      (RuntimeType) typeof (ulong),
      (RuntimeType) typeof (float),
      (RuntimeType) typeof (double),
      (RuntimeType) typeof (Decimal),
      (RuntimeType) typeof (DateTime),
      (RuntimeType) typeof (object),
      (RuntimeType) typeof (string)
    };
    private static readonly RuntimeType EnumType = (RuntimeType) typeof (Enum);
    internal static readonly char[] base64Table = new char[65]
    {
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      '+',
      '/',
      '='
    };
    public static readonly object DBNull = (object) DBNull.Value;

    static Convert()
    {
    }

    public static TypeCode GetTypeCode(object value)
    {
      if (value == null)
        return TypeCode.Empty;
      IConvertible convertible = value as IConvertible;
      if (convertible != null)
        return convertible.GetTypeCode();
      else
        return TypeCode.Object;
    }

    public static bool IsDBNull(object value)
    {
      if (value == DBNull.Value)
        return true;
      IConvertible convertible = value as IConvertible;
      if (convertible == null)
        return false;
      else
        return convertible.GetTypeCode() == TypeCode.DBNull;
    }

    public static object ChangeType(object value, TypeCode typeCode)
    {
      return Convert.ChangeType(value, typeCode, (IFormatProvider) Thread.CurrentThread.CurrentCulture);
    }

    public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
    {
      if (value == null && (typeCode == TypeCode.Empty || typeCode == TypeCode.String || typeCode == TypeCode.Object))
        return (object) null;
      IConvertible convertible = value as IConvertible;
      if (convertible == null)
        throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
      switch (typeCode)
      {
        case TypeCode.Empty:
          throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
        case TypeCode.Object:
          return value;
        case TypeCode.DBNull:
          throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
        case TypeCode.Boolean:
          return (object) (bool) (convertible.ToBoolean(provider) ? 1 : 0);
        case TypeCode.Char:
          return (object) convertible.ToChar(provider);
        case TypeCode.SByte:
          return (object) convertible.ToSByte(provider);
        case TypeCode.Byte:
          return (object) convertible.ToByte(provider);
        case TypeCode.Int16:
          return (object) convertible.ToInt16(provider);
        case TypeCode.UInt16:
          return (object) convertible.ToUInt16(provider);
        case TypeCode.Int32:
          return (object) convertible.ToInt32(provider);
        case TypeCode.UInt32:
          return (object) convertible.ToUInt32(provider);
        case TypeCode.Int64:
          return (object) convertible.ToInt64(provider);
        case TypeCode.UInt64:
          return (object) convertible.ToUInt64(provider);
        case TypeCode.Single:
          return (object) convertible.ToSingle(provider);
        case TypeCode.Double:
          return (object) convertible.ToDouble(provider);
        case TypeCode.Decimal:
          return (object) convertible.ToDecimal(provider);
        case TypeCode.DateTime:
          return (object) convertible.ToDateTime(provider);
        case TypeCode.String:
          return (object) convertible.ToString(provider);
        default:
          throw new ArgumentException(Environment.GetResourceString("Arg_UnknownTypeCode"));
      }
    }

    internal static object DefaultToType(IConvertible value, Type targetType, IFormatProvider provider)
    {
      if (targetType == (Type) null)
        throw new ArgumentNullException("targetType");
      RuntimeType runtimeType = targetType as RuntimeType;
      if (runtimeType != (RuntimeType) null)
      {
        if (value.GetType() == targetType)
          return (object) value;
        if (runtimeType == Convert.ConvertTypes[3])
          return (object) (bool) (value.ToBoolean(provider) ? 1 : 0);
        if (runtimeType == Convert.ConvertTypes[4])
          return (object) value.ToChar(provider);
        if (runtimeType == Convert.ConvertTypes[5])
          return (object) value.ToSByte(provider);
        if (runtimeType == Convert.ConvertTypes[6])
          return (object) value.ToByte(provider);
        if (runtimeType == Convert.ConvertTypes[7])
          return (object) value.ToInt16(provider);
        if (runtimeType == Convert.ConvertTypes[8])
          return (object) value.ToUInt16(provider);
        if (runtimeType == Convert.ConvertTypes[9])
          return (object) value.ToInt32(provider);
        if (runtimeType == Convert.ConvertTypes[10])
          return (object) value.ToUInt32(provider);
        if (runtimeType == Convert.ConvertTypes[11])
          return (object) value.ToInt64(provider);
        if (runtimeType == Convert.ConvertTypes[12])
          return (object) value.ToUInt64(provider);
        if (runtimeType == Convert.ConvertTypes[13])
          return (object) value.ToSingle(provider);
        if (runtimeType == Convert.ConvertTypes[14])
          return (object) value.ToDouble(provider);
        if (runtimeType == Convert.ConvertTypes[15])
          return (object) value.ToDecimal(provider);
        if (runtimeType == Convert.ConvertTypes[16])
          return (object) value.ToDateTime(provider);
        if (runtimeType == Convert.ConvertTypes[18])
          return (object) value.ToString(provider);
        if (runtimeType == Convert.ConvertTypes[1])
          return (object) value;
        if (runtimeType == Convert.EnumType)
          return (object) (Enum) value;
        if (runtimeType == Convert.ConvertTypes[2])
          throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
        if (runtimeType == Convert.ConvertTypes[0])
          throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
      }
      throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", (object) value.GetType().FullName, (object) targetType.FullName));
    }

    public static object ChangeType(object value, Type conversionType)
    {
      return Convert.ChangeType(value, conversionType, (IFormatProvider) Thread.CurrentThread.CurrentCulture);
    }

    public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
    {
      if (conversionType == (Type) null)
        throw new ArgumentNullException("conversionType");
      if (value == null)
      {
        if (conversionType.IsValueType)
          throw new InvalidCastException(Environment.GetResourceString("InvalidCast_CannotCastNullToValueType"));
        else
          return (object) null;
      }
      else
      {
        IConvertible convertible = value as IConvertible;
        if (convertible == null)
        {
          if (value.GetType() == conversionType)
            return value;
          else
            throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
        }
        else
        {
          RuntimeType runtimeType = conversionType as RuntimeType;
          if (runtimeType == Convert.ConvertTypes[3])
            return (object) (bool) (convertible.ToBoolean(provider) ? 1 : 0);
          if (runtimeType == Convert.ConvertTypes[4])
            return (object) convertible.ToChar(provider);
          if (runtimeType == Convert.ConvertTypes[5])
            return (object) convertible.ToSByte(provider);
          if (runtimeType == Convert.ConvertTypes[6])
            return (object) convertible.ToByte(provider);
          if (runtimeType == Convert.ConvertTypes[7])
            return (object) convertible.ToInt16(provider);
          if (runtimeType == Convert.ConvertTypes[8])
            return (object) convertible.ToUInt16(provider);
          if (runtimeType == Convert.ConvertTypes[9])
            return (object) convertible.ToInt32(provider);
          if (runtimeType == Convert.ConvertTypes[10])
            return (object) convertible.ToUInt32(provider);
          if (runtimeType == Convert.ConvertTypes[11])
            return (object) convertible.ToInt64(provider);
          if (runtimeType == Convert.ConvertTypes[12])
            return (object) convertible.ToUInt64(provider);
          if (runtimeType == Convert.ConvertTypes[13])
            return (object) convertible.ToSingle(provider);
          if (runtimeType == Convert.ConvertTypes[14])
            return (object) convertible.ToDouble(provider);
          if (runtimeType == Convert.ConvertTypes[15])
            return (object) convertible.ToDecimal(provider);
          if (runtimeType == Convert.ConvertTypes[16])
            return (object) convertible.ToDateTime(provider);
          if (runtimeType == Convert.ConvertTypes[18])
            return (object) convertible.ToString(provider);
          if (runtimeType == Convert.ConvertTypes[1])
            return value;
          else
            return convertible.ToType(conversionType, provider);
        }
      }
    }

    public static bool ToBoolean(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToBoolean((IFormatProvider) null);
      else
        return false;
    }

    public static bool ToBoolean(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToBoolean(provider);
      else
        return false;
    }

    public static bool ToBoolean(bool value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static bool ToBoolean(sbyte value)
    {
      return (int) value != 0;
    }

    public static bool ToBoolean(char value)
    {
      return ((IConvertible) value).ToBoolean((IFormatProvider) null);
    }

    public static bool ToBoolean(byte value)
    {
      return (int) value != 0;
    }

    public static bool ToBoolean(short value)
    {
      return (int) value != 0;
    }

    [CLSCompliant(false)]
    public static bool ToBoolean(ushort value)
    {
      return (int) value != 0;
    }

    public static bool ToBoolean(int value)
    {
      return value != 0;
    }

    [CLSCompliant(false)]
    public static bool ToBoolean(uint value)
    {
      return (int) value != 0;
    }

    public static bool ToBoolean(long value)
    {
      return value != 0L;
    }

    [CLSCompliant(false)]
    public static bool ToBoolean(ulong value)
    {
      return (long) value != 0L;
    }

    public static bool ToBoolean(string value)
    {
      if (value == null)
        return false;
      else
        return bool.Parse(value);
    }

    public static bool ToBoolean(string value, IFormatProvider provider)
    {
      if (value == null)
        return false;
      else
        return bool.Parse(value);
    }

    public static bool ToBoolean(float value)
    {
      return (double) value != 0.0;
    }

    public static bool ToBoolean(double value)
    {
      return value != 0.0;
    }

    public static bool ToBoolean(Decimal value)
    {
      return value != new Decimal(0);
    }

    public static bool ToBoolean(DateTime value)
    {
      return ((IConvertible) value).ToBoolean((IFormatProvider) null);
    }

    public static char ToChar(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToChar((IFormatProvider) null);
      else
        return char.MinValue;
    }

    public static char ToChar(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToChar(provider);
      else
        return char.MinValue;
    }

    public static char ToChar(bool value)
    {
      return ((IConvertible) (bool) (value ? 1 : 0)).ToChar((IFormatProvider) null);
    }

    public static char ToChar(char value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static char ToChar(sbyte value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    public static char ToChar(byte value)
    {
      return (char) value;
    }

    public static char ToChar(short value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    [CLSCompliant(false)]
    public static char ToChar(ushort value)
    {
      return (char) value;
    }

    public static char ToChar(int value)
    {
      if (value < 0 || value > (int) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    [CLSCompliant(false)]
    public static char ToChar(uint value)
    {
      if (value > (uint) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    public static char ToChar(long value)
    {
      if (value < 0L || value > (long) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    [CLSCompliant(false)]
    public static char ToChar(ulong value)
    {
      if (value > (ulong) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
      else
        return (char) value;
    }

    public static char ToChar(string value)
    {
      return Convert.ToChar(value, (IFormatProvider) null);
    }

    public static char ToChar(string value, IFormatProvider provider)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (value.Length != 1)
        throw new FormatException(Environment.GetResourceString("Format_NeedSingleChar"));
      else
        return value[0];
    }

    public static char ToChar(float value)
    {
      return ((IConvertible) value).ToChar((IFormatProvider) null);
    }

    public static char ToChar(double value)
    {
      return ((IConvertible) value).ToChar((IFormatProvider) null);
    }

    public static char ToChar(Decimal value)
    {
      return ((IConvertible) value).ToChar((IFormatProvider) null);
    }

    public static char ToChar(DateTime value)
    {
      return ((IConvertible) value).ToChar((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToSByte((IFormatProvider) null);
      else
        return (sbyte) 0;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToSByte(provider);
      else
        return (sbyte) 0;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(bool value)
    {
      if (!value)
        return (sbyte) 0;
      else
        return (sbyte) 1;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(sbyte value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(char value)
    {
      if ((int) value > (int) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(byte value)
    {
      if ((int) value > (int) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(short value)
    {
      if ((int) value < (int) sbyte.MinValue || (int) value > (int) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(ushort value)
    {
      if ((int) value > (int) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(int value)
    {
      if (value < (int) sbyte.MinValue || value > (int) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(uint value)
    {
      if ((long) value > (long) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(long value)
    {
      if (value < (long) sbyte.MinValue || value > (long) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(ulong value)
    {
      if (value > (ulong) sbyte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
      else
        return (sbyte) value;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(float value)
    {
      return Convert.ToSByte((double) value);
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(double value)
    {
      return Convert.ToSByte(Convert.ToInt32(value));
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(Decimal value)
    {
      return Decimal.ToSByte(Decimal.Round(value, 0));
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(string value)
    {
      if (value == null)
        return (sbyte) 0;
      else
        return sbyte.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(string value, IFormatProvider provider)
    {
      return sbyte.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(DateTime value)
    {
      return ((IConvertible) value).ToSByte((IFormatProvider) null);
    }

    public static byte ToByte(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToByte((IFormatProvider) null);
      else
        return (byte) 0;
    }

    public static byte ToByte(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToByte(provider);
      else
        return (byte) 0;
    }

    public static byte ToByte(bool value)
    {
      if (!value)
        return (byte) 0;
      else
        return (byte) 1;
    }

    public static byte ToByte(byte value)
    {
      return value;
    }

    public static byte ToByte(char value)
    {
      if ((int) value > (int) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    [CLSCompliant(false)]
    public static byte ToByte(sbyte value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    public static byte ToByte(short value)
    {
      if ((int) value < 0 || (int) value > (int) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    [CLSCompliant(false)]
    public static byte ToByte(ushort value)
    {
      if ((int) value > (int) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    public static byte ToByte(int value)
    {
      if (value < 0 || value > (int) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    [CLSCompliant(false)]
    public static byte ToByte(uint value)
    {
      if (value > (uint) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    public static byte ToByte(long value)
    {
      if (value < 0L || value > (long) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    [CLSCompliant(false)]
    public static byte ToByte(ulong value)
    {
      if (value > (ulong) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) value;
    }

    public static byte ToByte(float value)
    {
      return Convert.ToByte((double) value);
    }

    public static byte ToByte(double value)
    {
      return Convert.ToByte(Convert.ToInt32(value));
    }

    public static byte ToByte(Decimal value)
    {
      return Decimal.ToByte(Decimal.Round(value, 0));
    }

    public static byte ToByte(string value)
    {
      if (value == null)
        return (byte) 0;
      else
        return byte.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static byte ToByte(string value, IFormatProvider provider)
    {
      if (value == null)
        return (byte) 0;
      else
        return byte.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    public static byte ToByte(DateTime value)
    {
      return ((IConvertible) value).ToByte((IFormatProvider) null);
    }

    public static short ToInt16(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToInt16((IFormatProvider) null);
      else
        return (short) 0;
    }

    public static short ToInt16(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToInt16(provider);
      else
        return (short) 0;
    }

    public static short ToInt16(bool value)
    {
      if (!value)
        return (short) 0;
      else
        return (short) 1;
    }

    public static short ToInt16(char value)
    {
      if ((int) value > (int) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    [CLSCompliant(false)]
    public static short ToInt16(sbyte value)
    {
      return (short) value;
    }

    public static short ToInt16(byte value)
    {
      return (short) value;
    }

    [CLSCompliant(false)]
    public static short ToInt16(ushort value)
    {
      if ((int) value > (int) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    public static short ToInt16(int value)
    {
      if (value < (int) short.MinValue || value > (int) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    [CLSCompliant(false)]
    public static short ToInt16(uint value)
    {
      if ((long) value > (long) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    public static short ToInt16(short value)
    {
      return value;
    }

    public static short ToInt16(long value)
    {
      if (value < (long) short.MinValue || value > (long) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    [CLSCompliant(false)]
    public static short ToInt16(ulong value)
    {
      if (value > (ulong) short.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
      else
        return (short) value;
    }

    public static short ToInt16(float value)
    {
      return Convert.ToInt16((double) value);
    }

    public static short ToInt16(double value)
    {
      return Convert.ToInt16(Convert.ToInt32(value));
    }

    public static short ToInt16(Decimal value)
    {
      return Decimal.ToInt16(Decimal.Round(value, 0));
    }

    public static short ToInt16(string value)
    {
      if (value == null)
        return (short) 0;
      else
        return short.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static short ToInt16(string value, IFormatProvider provider)
    {
      if (value == null)
        return (short) 0;
      else
        return short.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    public static short ToInt16(DateTime value)
    {
      return ((IConvertible) value).ToInt16((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt16((IFormatProvider) null);
      else
        return (ushort) 0;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt16(provider);
      else
        return (ushort) 0;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(bool value)
    {
      if (!value)
        return (ushort) 0;
      else
        return (ushort) 1;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(char value)
    {
      return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(sbyte value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(byte value)
    {
      return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(short value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(int value)
    {
      if (value < 0 || value > (int) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(ushort value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(uint value)
    {
      if (value > (uint) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(long value)
    {
      if (value < 0L || value > (long) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(ulong value)
    {
      if (value > (ulong) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) value;
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(float value)
    {
      return Convert.ToUInt16((double) value);
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(double value)
    {
      return Convert.ToUInt16(Convert.ToInt32(value));
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(Decimal value)
    {
      return Decimal.ToUInt16(Decimal.Round(value, 0));
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(string value)
    {
      if (value == null)
        return (ushort) 0;
      else
        return ushort.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(string value, IFormatProvider provider)
    {
      if (value == null)
        return (ushort) 0;
      else
        return ushort.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(DateTime value)
    {
      return ((IConvertible) value).ToUInt16((IFormatProvider) null);
    }

    public static int ToInt32(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToInt32((IFormatProvider) null);
      else
        return 0;
    }

    public static int ToInt32(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToInt32(provider);
      else
        return 0;
    }

    public static int ToInt32(bool value)
    {
      if (!value)
        return 0;
      else
        return 1;
    }

    public static int ToInt32(char value)
    {
      return (int) value;
    }

    [CLSCompliant(false)]
    public static int ToInt32(sbyte value)
    {
      return (int) value;
    }

    public static int ToInt32(byte value)
    {
      return (int) value;
    }

    public static int ToInt32(short value)
    {
      return (int) value;
    }

    [CLSCompliant(false)]
    public static int ToInt32(ushort value)
    {
      return (int) value;
    }

    [CLSCompliant(false)]
    public static int ToInt32(uint value)
    {
      if (value > (uint) int.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
      else
        return (int) value;
    }

    public static int ToInt32(int value)
    {
      return value;
    }

    public static int ToInt32(long value)
    {
      if (value < (long) int.MinValue || value > (long) int.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
      else
        return (int) value;
    }

    [CLSCompliant(false)]
    public static int ToInt32(ulong value)
    {
      if (value > (ulong) int.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
      else
        return (int) value;
    }

    public static int ToInt32(float value)
    {
      return Convert.ToInt32((double) value);
    }

    public static int ToInt32(double value)
    {
      if (value >= 0.0)
      {
        if (value < 2147483647.5)
        {
          int num1 = (int) value;
          double num2 = value - (double) num1;
          if (num2 > 0.5 || num2 == 0.5 && (num1 & 1) != 0)
            ++num1;
          return num1;
        }
      }
      else if (value >= -2147483648.5)
      {
        int num1 = (int) value;
        double num2 = value - (double) num1;
        if (num2 < -0.5 || num2 == -0.5 && (num1 & 1) != 0)
          --num1;
        return num1;
      }
      throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
    }

    [SecuritySafeCritical]
    public static int ToInt32(Decimal value)
    {
      return Decimal.FCallToInt32(value);
    }

    public static int ToInt32(string value)
    {
      if (value == null)
        return 0;
      else
        return int.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static int ToInt32(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0;
      else
        return int.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    public static int ToInt32(DateTime value)
    {
      return ((IConvertible) value).ToInt32((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt32((IFormatProvider) null);
      else
        return 0U;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt32(provider);
      else
        return 0U;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(bool value)
    {
      if (!value)
        return 0U;
      else
        return 1U;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(char value)
    {
      return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(sbyte value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      else
        return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(byte value)
    {
      return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(short value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      else
        return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(ushort value)
    {
      return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(int value)
    {
      if (value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      else
        return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(uint value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(long value)
    {
      if (value < 0L || value > (long) uint.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      else
        return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(ulong value)
    {
      if (value > (ulong) uint.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      else
        return (uint) value;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(float value)
    {
      return Convert.ToUInt32((double) value);
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(double value)
    {
      if (value < -0.5 || value >= 4294967295.5)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
      uint num1 = (uint) value;
      double num2 = value - (double) num1;
      if (num2 > 0.5 || num2 == 0.5 && ((int) num1 & 1) != 0)
        ++num1;
      return num1;
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(Decimal value)
    {
      return Decimal.ToUInt32(Decimal.Round(value, 0));
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(string value)
    {
      if (value == null)
        return 0U;
      else
        return uint.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0U;
      else
        return uint.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(DateTime value)
    {
      return ((IConvertible) value).ToUInt32((IFormatProvider) null);
    }

    public static long ToInt64(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToInt64((IFormatProvider) null);
      else
        return 0L;
    }

    public static long ToInt64(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToInt64(provider);
      else
        return 0L;
    }

    public static long ToInt64(bool value)
    {
      return value ? 1L : 0L;
    }

    public static long ToInt64(char value)
    {
      return (long) value;
    }

    [CLSCompliant(false)]
    public static long ToInt64(sbyte value)
    {
      return (long) value;
    }

    public static long ToInt64(byte value)
    {
      return (long) value;
    }

    public static long ToInt64(short value)
    {
      return (long) value;
    }

    [CLSCompliant(false)]
    public static long ToInt64(ushort value)
    {
      return (long) value;
    }

    public static long ToInt64(int value)
    {
      return (long) value;
    }

    [CLSCompliant(false)]
    public static long ToInt64(uint value)
    {
      return (long) value;
    }

    [CLSCompliant(false)]
    public static long ToInt64(ulong value)
    {
      if (value > 9223372036854775807UL)
        throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
      else
        return (long) value;
    }

    public static long ToInt64(long value)
    {
      return value;
    }

    public static long ToInt64(float value)
    {
      return Convert.ToInt64((double) value);
    }

    [SecuritySafeCritical]
    public static long ToInt64(double value)
    {
      return checked ((long) Math.Round(value));
    }

    public static long ToInt64(Decimal value)
    {
      return Decimal.ToInt64(Decimal.Round(value, 0));
    }

    public static long ToInt64(string value)
    {
      if (value == null)
        return 0L;
      else
        return long.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static long ToInt64(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0L;
      else
        return long.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    public static long ToInt64(DateTime value)
    {
      return ((IConvertible) value).ToInt64((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt64((IFormatProvider) null);
      else
        return 0UL;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToUInt64(provider);
      else
        return 0UL;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(bool value)
    {
      if (!value)
        return 0UL;
      else
        return 1UL;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(char value)
    {
      return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(sbyte value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
      else
        return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(byte value)
    {
      return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(short value)
    {
      if ((int) value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
      else
        return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(ushort value)
    {
      return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(int value)
    {
      if (value < 0)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
      else
        return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(uint value)
    {
      return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(long value)
    {
      if (value < 0L)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
      else
        return (ulong) value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(ulong value)
    {
      return value;
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(float value)
    {
      return Convert.ToUInt64((double) value);
    }

    [SecuritySafeCritical]
    [CLSCompliant(false)]
    public static ulong ToUInt64(double value)
    {
      return checked ((ulong) Math.Round(value));
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(Decimal value)
    {
      return Decimal.ToUInt64(Decimal.Round(value, 0));
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(string value)
    {
      if (value == null)
        return 0UL;
      else
        return ulong.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0UL;
      else
        return ulong.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, provider);
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(DateTime value)
    {
      return ((IConvertible) value).ToUInt64((IFormatProvider) null);
    }

    public static float ToSingle(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToSingle((IFormatProvider) null);
      else
        return 0.0f;
    }

    public static float ToSingle(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToSingle(provider);
      else
        return 0.0f;
    }

    [CLSCompliant(false)]
    public static float ToSingle(sbyte value)
    {
      return (float) value;
    }

    public static float ToSingle(byte value)
    {
      return (float) value;
    }

    public static float ToSingle(char value)
    {
      return ((IConvertible) value).ToSingle((IFormatProvider) null);
    }

    public static float ToSingle(short value)
    {
      return (float) value;
    }

    [CLSCompliant(false)]
    public static float ToSingle(ushort value)
    {
      return (float) value;
    }

    public static float ToSingle(int value)
    {
      return (float) value;
    }

    [CLSCompliant(false)]
    public static float ToSingle(uint value)
    {
      return (float) value;
    }

    public static float ToSingle(long value)
    {
      return (float) value;
    }

    [CLSCompliant(false)]
    public static float ToSingle(ulong value)
    {
      return (float) value;
    }

    public static float ToSingle(float value)
    {
      return value;
    }

    public static float ToSingle(double value)
    {
      return (float) value;
    }

    public static float ToSingle(Decimal value)
    {
      return (float) value;
    }

    public static float ToSingle(string value)
    {
      if (value == null)
        return 0.0f;
      else
        return float.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static float ToSingle(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0.0f;
      else
        return float.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
    }

    public static float ToSingle(bool value)
    {
      return value ? 1f : 0.0f;
    }

    public static float ToSingle(DateTime value)
    {
      return ((IConvertible) value).ToSingle((IFormatProvider) null);
    }

    public static double ToDouble(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToDouble((IFormatProvider) null);
      else
        return 0.0;
    }

    public static double ToDouble(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToDouble(provider);
      else
        return 0.0;
    }

    [CLSCompliant(false)]
    public static double ToDouble(sbyte value)
    {
      return (double) value;
    }

    public static double ToDouble(byte value)
    {
      return (double) value;
    }

    public static double ToDouble(short value)
    {
      return (double) value;
    }

    public static double ToDouble(char value)
    {
      return ((IConvertible) value).ToDouble((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static double ToDouble(ushort value)
    {
      return (double) value;
    }

    public static double ToDouble(int value)
    {
      return (double) value;
    }

    [CLSCompliant(false)]
    public static double ToDouble(uint value)
    {
      return (double) value;
    }

    public static double ToDouble(long value)
    {
      return (double) value;
    }

    [CLSCompliant(false)]
    public static double ToDouble(ulong value)
    {
      return (double) value;
    }

    public static double ToDouble(float value)
    {
      return (double) value;
    }

    public static double ToDouble(double value)
    {
      return value;
    }

    public static double ToDouble(Decimal value)
    {
      return (double) value;
    }

    public static double ToDouble(string value)
    {
      if (value == null)
        return 0.0;
      else
        return double.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static double ToDouble(string value, IFormatProvider provider)
    {
      if (value == null)
        return 0.0;
      else
        return double.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
    }

    public static double ToDouble(bool value)
    {
      return value ? 1.0 : 0.0;
    }

    public static double ToDouble(DateTime value)
    {
      return ((IConvertible) value).ToDouble((IFormatProvider) null);
    }

    public static Decimal ToDecimal(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToDecimal((IFormatProvider) null);
      else
        return new Decimal(0);
    }

    public static Decimal ToDecimal(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToDecimal(provider);
      else
        return new Decimal(0);
    }

    [CLSCompliant(false)]
    public static Decimal ToDecimal(sbyte value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(byte value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(char value)
    {
      return ((IConvertible) value).ToDecimal((IFormatProvider) null);
    }

    public static Decimal ToDecimal(short value)
    {
      return (Decimal) value;
    }

    [CLSCompliant(false)]
    public static Decimal ToDecimal(ushort value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(int value)
    {
      return (Decimal) value;
    }

    [CLSCompliant(false)]
    public static Decimal ToDecimal(uint value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(long value)
    {
      return (Decimal) value;
    }

    [CLSCompliant(false)]
    public static Decimal ToDecimal(ulong value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(float value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(double value)
    {
      return (Decimal) value;
    }

    public static Decimal ToDecimal(string value)
    {
      if (value == null)
        return new Decimal(0);
      else
        return Decimal.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static Decimal ToDecimal(string value, IFormatProvider provider)
    {
      if (value == null)
        return new Decimal(0);
      else
        return Decimal.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, provider);
    }

    public static Decimal ToDecimal(Decimal value)
    {
      return value;
    }

    public static Decimal ToDecimal(bool value)
    {
      return (Decimal) (value ? 1 : 0);
    }

    public static Decimal ToDecimal(DateTime value)
    {
      return ((IConvertible) value).ToDecimal((IFormatProvider) null);
    }

    public static DateTime ToDateTime(DateTime value)
    {
      return value;
    }

    public static DateTime ToDateTime(object value)
    {
      if (value != null)
        return ((IConvertible) value).ToDateTime((IFormatProvider) null);
      else
        return DateTime.MinValue;
    }

    public static DateTime ToDateTime(object value, IFormatProvider provider)
    {
      if (value != null)
        return ((IConvertible) value).ToDateTime(provider);
      else
        return DateTime.MinValue;
    }

    public static DateTime ToDateTime(string value)
    {
      if (value == null)
        return new DateTime(0L);
      else
        return DateTime.Parse(value, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static DateTime ToDateTime(string value, IFormatProvider provider)
    {
      if (value == null)
        return new DateTime(0L);
      else
        return DateTime.Parse(value, provider);
    }

    [CLSCompliant(false)]
    public static DateTime ToDateTime(sbyte value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(byte value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(short value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static DateTime ToDateTime(ushort value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(int value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static DateTime ToDateTime(uint value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(long value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    [CLSCompliant(false)]
    public static DateTime ToDateTime(ulong value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(bool value)
    {
      return ((IConvertible) (bool) (value ? 1 : 0)).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(char value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(float value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(double value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static DateTime ToDateTime(Decimal value)
    {
      return ((IConvertible) value).ToDateTime((IFormatProvider) null);
    }

    public static string ToString(object value)
    {
      return Convert.ToString(value, (IFormatProvider) null);
    }

    public static string ToString(object value, IFormatProvider provider)
    {
      IConvertible convertible = value as IConvertible;
      if (convertible != null)
        return convertible.ToString(provider);
      IFormattable formattable = value as IFormattable;
      if (formattable != null)
        return formattable.ToString((string) null, provider);
      if (value != null)
        return value.ToString();
      else
        return string.Empty;
    }

    public static string ToString(bool value)
    {
      return value.ToString();
    }

    public static string ToString(bool value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(char value)
    {
      return char.ToString(value);
    }

    public static string ToString(char value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    [CLSCompliant(false)]
    public static string ToString(sbyte value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(sbyte value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(byte value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(byte value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(short value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(short value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    [CLSCompliant(false)]
    public static string ToString(ushort value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(ushort value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(int value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(int value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    [CLSCompliant(false)]
    public static string ToString(uint value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(uint value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(long value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(long value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    [CLSCompliant(false)]
    public static string ToString(ulong value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(ulong value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(float value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(float value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(double value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(double value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(Decimal value)
    {
      return value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public static string ToString(Decimal value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(DateTime value)
    {
      return value.ToString();
    }

    public static string ToString(DateTime value, IFormatProvider provider)
    {
      return value.ToString(provider);
    }

    public static string ToString(string value)
    {
      return value;
    }

    public static string ToString(string value, IFormatProvider provider)
    {
      return value;
    }

    public static byte ToByte(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      int num = ParseNumbers.StringToInt(value, fromBase, 4608);
      if (num < 0 || num > (int) byte.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
      else
        return (byte) num;
    }

    [CLSCompliant(false)]
    public static sbyte ToSByte(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      int num = ParseNumbers.StringToInt(value, fromBase, 5120);
      if (fromBase != 10 && num <= (int) byte.MaxValue || num >= (int) sbyte.MinValue && num <= (int) sbyte.MaxValue)
        return (sbyte) num;
      else
        throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
    }

    public static short ToInt16(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      int num = ParseNumbers.StringToInt(value, fromBase, 6144);
      if (fromBase != 10 && num <= (int) ushort.MaxValue || num >= (int) short.MinValue && num <= (int) short.MaxValue)
        return (short) num;
      else
        throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      int num = ParseNumbers.StringToInt(value, fromBase, 4608);
      if (num < 0 || num > (int) ushort.MaxValue)
        throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
      else
        return (ushort) num;
    }

    public static int ToInt32(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.StringToInt(value, fromBase, 4096);
    }

    [CLSCompliant(false)]
    public static uint ToUInt32(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return (uint) ParseNumbers.StringToInt(value, fromBase, 4608);
    }

    public static long ToInt64(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.StringToLong(value, fromBase, 4096);
    }

    [CLSCompliant(false)]
    public static ulong ToUInt64(string value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && (fromBase != 10 && fromBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return (ulong) ParseNumbers.StringToLong(value, fromBase, 4608);
    }

    [SecuritySafeCritical]
    public static string ToString(byte value, int toBase)
    {
      if (toBase != 2 && toBase != 8 && (toBase != 10 && toBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.IntToString((int) value, toBase, -1, ' ', 64);
    }

    [SecuritySafeCritical]
    public static string ToString(short value, int toBase)
    {
      if (toBase != 2 && toBase != 8 && (toBase != 10 && toBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.IntToString((int) value, toBase, -1, ' ', 128);
    }

    [SecuritySafeCritical]
    public static string ToString(int value, int toBase)
    {
      if (toBase != 2 && toBase != 8 && (toBase != 10 && toBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.IntToString(value, toBase, -1, ' ', 0);
    }

    [SecuritySafeCritical]
    public static string ToString(long value, int toBase)
    {
      if (toBase != 2 && toBase != 8 && (toBase != 10 && toBase != 16))
        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
      else
        return ParseNumbers.LongToString(value, toBase, -1, ' ', 0);
    }

    public static string ToBase64String(byte[] inArray)
    {
      if (inArray == null)
        throw new ArgumentNullException("inArray");
      else
        return Convert.ToBase64String(inArray, 0, inArray.Length, Base64FormattingOptions.None);
    }

    [ComVisible(false)]
    public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
    {
      if (inArray == null)
        throw new ArgumentNullException("inArray");
      else
        return Convert.ToBase64String(inArray, 0, inArray.Length, options);
    }

    public static string ToBase64String(byte[] inArray, int offset, int length)
    {
      return Convert.ToBase64String(inArray, offset, length, Base64FormattingOptions.None);
    }

    [ComVisible(false)]
    [SecuritySafeCritical]
    public static unsafe string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
    {
      if (inArray == null)
        throw new ArgumentNullException("inArray");
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
      if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
      {
        throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[1]
        {
          (object) options
        }));
      }
      else
      {
        int length1 = inArray.Length;
        if (offset > length1 - length)
          throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
        if (length1 == 0)
          return string.Empty;
        bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
        string str = string.FastAllocateString(Convert.CalculateOutputLength(length, insertLineBreaks));
        fixed (char* outChars = str)
          fixed (byte* inData = inArray)
          {
            Convert.ConvertToBase64Array(outChars, inData, offset, length, insertLineBreaks);
            return str;
          }
      }
    }

    public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
    {
      return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, Base64FormattingOptions.None);
    }

    [SecuritySafeCritical]
    [ComVisible(false)]
    public static unsafe int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
    {
      if (inArray == null)
        throw new ArgumentNullException("inArray");
      if (outArray == null)
        throw new ArgumentNullException("outArray");
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (offsetIn < 0)
        throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
      if (offsetOut < 0)
        throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
      if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
      {
        throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[1]
        {
          (object) options
        }));
      }
      else
      {
        int length1 = inArray.Length;
        if (offsetIn > length1 - length)
          throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
        if (length1 == 0)
          return 0;
        bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
        int length2 = outArray.Length;
        int num1 = Convert.CalculateOutputLength(length, insertLineBreaks);
        if (offsetOut > length2 - num1)
          throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_OffsetOut"));
        int num2;
        fixed (char* outChars = &outArray[offsetOut])
          fixed (byte* inData = inArray)
            num2 = Convert.ConvertToBase64Array(outChars, inData, offsetIn, length, insertLineBreaks);
        return num2;
      }
    }

    [SecurityCritical]
    private static unsafe int ConvertToBase64Array(char* outChars, byte* inData, int offset, int length, bool insertLineBreaks)
    {
      int num1 = length % 3;
      int num2 = offset + (length - num1);
      int index1 = 0;
      int num3 = 0;
      fixed (char* chPtr1 = Convert.base64Table)
      {
        int index2 = offset;
        while (index2 < num2)
        {
          if (insertLineBreaks)
          {
            if (num3 == 76)
            {
              char* chPtr2 = outChars;
              int num4 = index1;
              int num5 = 1;
              int num6 = num4 + num5;
              IntPtr num7 = (IntPtr) num4 * 2;
              *(short*) ((IntPtr) chPtr2 + num7) = (short) 13;
              char* chPtr3 = outChars;
              int num8 = num6;
              int num9 = 1;
              index1 = num8 + num9;
              IntPtr num10 = (IntPtr) num8 * 2;
              *(short*) ((IntPtr) chPtr3 + num10) = (short) 10;
              num3 = 0;
            }
            num3 += 4;
          }
          outChars[index1] = chPtr1[((int) inData[index2] & 252) >> 2];
          outChars[index1 + 1] = chPtr1[((int) inData[index2] & 3) << 4 | ((int) inData[index2 + 1] & 240) >> 4];
          outChars[index1 + 2] = chPtr1[((int) inData[index2 + 1] & 15) << 2 | ((int) inData[index2 + 2] & 192) >> 6];
          outChars[index1 + 3] = chPtr1[(int) inData[index2 + 2] & 63];
          index1 += 4;
          index2 += 3;
        }
        int index3 = num2;
        if (insertLineBreaks && num1 != 0 && num3 == 76)
        {
          char* chPtr2 = outChars;
          int num4 = index1;
          int num5 = 1;
          int num6 = num4 + num5;
          IntPtr num7 = (IntPtr) num4 * 2;
          *(short*) ((IntPtr) chPtr2 + num7) = (short) 13;
          char* chPtr3 = outChars;
          int num8 = num6;
          int num9 = 1;
          index1 = num8 + num9;
          IntPtr num10 = (IntPtr) num8 * 2;
          *(short*) ((IntPtr) chPtr3 + num10) = (short) 10;
        }
        switch (num1)
        {
          case 1:
            outChars[index1] = chPtr1[((int) inData[index3] & 252) >> 2];
            outChars[index1 + 1] = chPtr1[((int) inData[index3] & 3) << 4];
            outChars[index1 + 2] = chPtr1[64];
            outChars[index1 + 3] = chPtr1[64];
            index1 += 4;
            break;
          case 2:
            outChars[index1] = chPtr1[((int) inData[index3] & 252) >> 2];
            outChars[index1 + 1] = chPtr1[((int) inData[index3] & 3) << 4 | ((int) inData[index3 + 1] & 240) >> 4];
            outChars[index1 + 2] = chPtr1[((int) inData[index3 + 1] & 15) << 2];
            outChars[index1 + 3] = chPtr1[64];
            index1 += 4;
            break;
        }
      }
      return index1;
    }

    private static int CalculateOutputLength(int inputLength, bool insertLineBreaks)
    {
      int num1 = inputLength / 3 * 4 + (inputLength % 3 != 0 ? 4 : 0);
      if (num1 == 0 || !insertLineBreaks)
        return num1;
      int num2 = num1 / 76;
      if (num1 % 76 == 0)
        --num2;
      num1 += num2 * 2;
      return num1;
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static byte[] FromBase64String(string s);

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static byte[] FromBase64CharArray(char[] inArray, int offset, int length);
  }
}
