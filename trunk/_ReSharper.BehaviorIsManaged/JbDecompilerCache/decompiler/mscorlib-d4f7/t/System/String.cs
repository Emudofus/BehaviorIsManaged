// Type: System.String
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System
{
  [ComVisible(true)]
  [Serializable]
  public sealed class String : IComparable, ICloneable, IConvertible, IComparable<string>, IEnumerable<char>, IEnumerable, IEquatable<string>
  {
    public static readonly string Empty = "";
    private const int TrimHead = 0;
    private const int TrimTail = 1;
    private const int TrimBoth = 2;
    private const int charPtrAlignConst = 1;
    private const int alignConst = 3;
    [NonSerialized]
    private int m_stringLength;
    [ForceTokenStabilization]
    [NonSerialized]
    private char m_firstChar;

    internal char FirstChar
    {
      get
      {
        return this.m_firstChar;
      }
    }

    [IndexerName("Chars")]
    public char this[int index] { [SecuritySafeCritical, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int Length { [SecuritySafeCritical, MethodImpl(MethodImplOptions.InternalCall)] get; }

    static String()
    {
    }

    [CLSCompliant(false)]
    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(char* value);

    [SecurityCritical]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(char* value, int startIndex, int length);

    [CLSCompliant(false)]
    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(sbyte* value);

    [CLSCompliant(false)]
    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(sbyte* value, int startIndex, int length);

    [CLSCompliant(false)]
    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(sbyte* value, int startIndex, int length, Encoding enc);

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(char[] value, int startIndex, int length);

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(char[] value);

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public String(char c, int count);

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator ==(string a, string b)
    {
      return string.Equals(a, b);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator !=(string a, string b)
    {
      return !string.Equals(a, b);
    }

    public static string Join(string separator, params string[] value)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      else
        return string.Join(separator, value, 0, value.Length);
    }

    [ComVisible(false)]
    public static string Join(string separator, params object[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (values.Length == 0 || values[0] == null)
        return string.Empty;
      if (separator == null)
        separator = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = values[0].ToString();
      if (str1 != null)
        stringBuilder.Append(str1);
      for (int index = 1; index < values.Length; ++index)
      {
        stringBuilder.Append(separator);
        if (values[index] != null)
        {
          string str2 = values[index].ToString();
          if (str2 != null)
            stringBuilder.Append(str2);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    [ComVisible(false)]
    public static string Join<T>(string separator, IEnumerable<T> values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (separator == null)
        separator = string.Empty;
      using (IEnumerator<T> enumerator = values.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        if ((object) enumerator.Current != null)
        {
          string str = enumerator.Current.ToString();
          if (str != null)
            stringBuilder.Append(str);
        }
        while (enumerator.MoveNext())
        {
          stringBuilder.Append(separator);
          if ((object) enumerator.Current != null)
          {
            string str = enumerator.Current.ToString();
            if (str != null)
              stringBuilder.Append(str);
          }
        }
        return ((object) stringBuilder).ToString();
      }
    }

    [ComVisible(false)]
    public static string Join(string separator, IEnumerable<string> values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      if (separator == null)
        separator = string.Empty;
      using (IEnumerator<string> enumerator = values.GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        if (enumerator.Current != null)
          stringBuilder.Append(enumerator.Current);
        while (enumerator.MoveNext())
        {
          stringBuilder.Append(separator);
          if (enumerator.Current != null)
            stringBuilder.Append(enumerator.Current);
        }
        return ((object) stringBuilder).ToString();
      }
    }

    [SecuritySafeCritical]
    public static unsafe string Join(string separator, string[] value, int startIndex, int count)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
      if (startIndex > value.Length - count)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
      if (separator == null)
        separator = string.Empty;
      if (count == 0)
        return string.Empty;
      int num1 = 0;
      int num2 = startIndex + count - 1;
      for (int index = startIndex; index <= num2; ++index)
      {
        if (value[index] != null)
          num1 += value[index].Length;
      }
      int num3 = num1 + (count - 1) * separator.Length;
      if (num3 < 0 || num3 + 1 < 0)
        throw new OutOfMemoryException();
      if (num3 == 0)
        return string.Empty;
      string str = string.FastAllocateString(num3);
      fixed (char* buffer = &str.m_firstChar)
      {
        UnSafeCharBuffer unSafeCharBuffer = new UnSafeCharBuffer(buffer, num3);
        unSafeCharBuffer.AppendString(value[startIndex]);
        for (int index = startIndex + 1; index <= num2; ++index)
        {
          unSafeCharBuffer.AppendString(separator);
          unSafeCharBuffer.AppendString(value[index]);
        }
      }
      return str;
    }

    [SecuritySafeCritical]
    private static unsafe int CompareOrdinalIgnoreCaseHelper(string strA, string strB)
    {
      int num1 = Math.Min(strA.Length, strB.Length);
      fixed (char* chPtr1 = &strA.m_firstChar)
        fixed (char* chPtr2 = &strB.m_firstChar)
        {
          char* chPtr3 = chPtr1;
          char* chPtr4 = chPtr2;
          for (; num1 != 0; --num1)
          {
            int num2 = (int) *chPtr3;
            int num3 = (int) *chPtr4;
            if ((uint) (num2 - 97) <= 25U)
              num2 -= 32;
            if ((uint) (num3 - 97) <= 25U)
              num3 -= 32;
            if (num2 != num3)
              return num2 - num3;
            ++chPtr3;
            ++chPtr4;
          }
          return strA.Length - strB.Length;
        }
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static int nativeCompareOrdinalEx(string strA, int indexA, string strB, int indexB, int count);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static int nativeCompareOrdinalIgnoreCaseWC(string strA, char* strBChars);

    [SecuritySafeCritical]
    internal static unsafe string SmallCharToUpper(string strIn)
    {
      int length = strIn.Length;
      string str = string.FastAllocateString(length);
      fixed (char* chPtr1 = &strIn.m_firstChar)
        fixed (char* chPtr2 = &str.m_firstChar)
        {
          for (int index = 0; index < length; ++index)
          {
            int num = (int) chPtr1[index];
            if ((uint) (num - 97) <= 25U)
              num -= 32;
            chPtr2[index] = (char) num;
          }
        }
      return str;
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [SecuritySafeCritical]
    private static unsafe bool EqualsHelper(string strA, string strB)
    {
      int length = strA.Length;
      if (length != strB.Length)
        return false;
      fixed (char* chPtr1 = &strA.m_firstChar)
        fixed (char* chPtr2 = &strB.m_firstChar)
        {
          char* chPtr3 = chPtr1;
          char* chPtr4 = chPtr2;
          while (length >= 10 && (*(int*) chPtr3 == *(int*) chPtr4 && *(int*) (chPtr3 + 2) == *(int*) (chPtr4 + 2)) && (*(int*) (chPtr3 + 4) == *(int*) (chPtr4 + 4) && *(int*) (chPtr3 + 6) == *(int*) (chPtr4 + 6) && *(int*) (chPtr3 + 8) == *(int*) (chPtr4 + 8)))
          {
            chPtr3 += 10;
            chPtr4 += 10;
            length -= 10;
          }
          while (length > 0 && *(int*) chPtr3 == *(int*) chPtr4)
          {
            chPtr3 += 2;
            chPtr4 += 2;
            length -= 2;
          }
          return length <= 0;
        }
    }

    [SecuritySafeCritical]
    private static unsafe int CompareOrdinalHelper(string strA, string strB)
    {
      int num1 = Math.Min(strA.Length, strB.Length);
      int num2 = -1;
      fixed (char* chPtr1 = &strA.m_firstChar)
        fixed (char* chPtr2 = &strB.m_firstChar)
        {
          char* chPtr3 = chPtr1;
          char* chPtr4 = chPtr2;
          while (num1 >= 10)
          {
            if (*(int*) chPtr3 != *(int*) chPtr4)
            {
              num2 = 0;
              break;
            }
            else if (*(int*) (chPtr3 + 2) != *(int*) (chPtr4 + 2))
            {
              num2 = 2;
              break;
            }
            else if (*(int*) (chPtr3 + 4) != *(int*) (chPtr4 + 4))
            {
              num2 = 4;
              break;
            }
            else if (*(int*) (chPtr3 + 6) != *(int*) (chPtr4 + 6))
            {
              num2 = 6;
              break;
            }
            else if (*(int*) (chPtr3 + 8) != *(int*) (chPtr4 + 8))
            {
              num2 = 8;
              break;
            }
            else
            {
              chPtr3 += 10;
              chPtr4 += 10;
              num1 -= 10;
            }
          }
          if (num2 != -1)
          {
            char* chPtr5 = chPtr3 + num2;
            char* chPtr6 = chPtr4 + num2;
            int num3;
            if ((num3 = (int) *chPtr5 - (int) *chPtr6) != 0)
              return num3;
            else
              return (int) chPtr5[1] - (int) chPtr6[1];
          }
          else
          {
            while (num1 > 0 && *(int*) chPtr3 == *(int*) chPtr4)
            {
              chPtr3 += 2;
              chPtr4 += 2;
              num1 -= 2;
            }
            if (num1 <= 0)
              return strA.Length - strB.Length;
            int num3;
            if ((num3 = (int) *chPtr3 - (int) *chPtr4) != 0)
              return num3;
            else
              return (int) chPtr3[1] - (int) chPtr4[1];
          }
        }
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    public override bool Equals(object obj)
    {
      if (this == null)
        throw new NullReferenceException();
      string strB = obj as string;
      if (strB == null)
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      else
        return string.EqualsHelper(this, strB);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public bool Equals(string value)
    {
      if (this == null)
        throw new NullReferenceException();
      if (value == null)
        return false;
      if (object.ReferenceEquals((object) this, (object) value))
        return true;
      else
        return string.EqualsHelper(this, value);
    }

    [SecuritySafeCritical]
    public bool Equals(string value, StringComparison comparisonType)
    {
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (this == value)
        return true;
      if (value == null)
        return false;
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
        case StringComparison.Ordinal:
          return string.EqualsHelper(this, value);
        case StringComparison.OrdinalIgnoreCase:
          if (this.Length != value.Length)
            return false;
          if (this.IsAscii() && value.IsAscii())
            return string.CompareOrdinalIgnoreCaseHelper(this, value) == 0;
          else
            return TextInfo.CompareOrdinalIgnoreCase(this, value) == 0;
        default:
          throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool Equals(string a, string b)
    {
      if (a == b)
        return true;
      if (a == null || b == null)
        return false;
      else
        return string.EqualsHelper(a, b);
    }

    [SecuritySafeCritical]
    public static bool Equals(string a, string b, StringComparison comparisonType)
    {
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (a == b)
        return true;
      if (a == null || b == null)
        return false;
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
        case StringComparison.Ordinal:
          return string.EqualsHelper(a, b);
        case StringComparison.OrdinalIgnoreCase:
          if (a.Length != b.Length)
            return false;
          if (a.IsAscii() && b.IsAscii())
            return string.CompareOrdinalIgnoreCaseHelper(a, b) == 0;
          else
            return TextInfo.CompareOrdinalIgnoreCase(a, b) == 0;
        default:
          throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      }
    }

    [SecuritySafeCritical]
    public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
      if (destination == null)
        throw new ArgumentNullException("destination");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
      if (sourceIndex < 0)
        throw new ArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (count > this.Length - sourceIndex)
        throw new ArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
      if (destinationIndex > destination.Length - count || destinationIndex < 0)
        throw new ArgumentOutOfRangeException("destinationIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
      if (count <= 0)
        return;
      fixed (char* chPtr1 = &this.m_firstChar)
        fixed (char* chPtr2 = destination)
          string.wstrcpy(chPtr2 + destinationIndex, chPtr1 + sourceIndex, count);
    }

    [SecuritySafeCritical]
    public unsafe char[] ToCharArray()
    {
      int length = this.Length;
      char[] chArray = new char[length];
      if (length > 0)
      {
        fixed (char* smem = &this.m_firstChar)
          fixed (char* dmem = chArray)
            string.wstrcpyPtrAligned(dmem, smem, length);
      }
      return chArray;
    }

    [SecuritySafeCritical]
    public unsafe char[] ToCharArray(int startIndex, int length)
    {
      if (startIndex < 0 || startIndex > this.Length || startIndex > this.Length - length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      char[] chArray = new char[length];
      if (length > 0)
      {
        fixed (char* chPtr = &this.m_firstChar)
          fixed (char* dmem = chArray)
            string.wstrcpy(dmem, chPtr + startIndex, length);
      }
      return chArray;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool IsNullOrEmpty(string value)
    {
      if (value != null)
        return value.Length == 0;
      else
        return true;
    }

    public static bool IsNullOrWhiteSpace(string value)
    {
      if (value == null)
        return true;
      for (int index = 0; index < value.Length; ++index)
      {
        if (!char.IsWhiteSpace(value[index]))
          return false;
      }
      return true;
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [SecuritySafeCritical]
    public override unsafe int GetHashCode()
    {
      fixed (char* chPtr = this)
      {
        int num1 = 352654597;
        int num2 = num1;
        int* numPtr = (int*) chPtr;
        int length = this.Length;
        while (length > 0)
        {
          num1 = (num1 << 5) + num1 + (num1 >> 27) ^ *numPtr;
          if (length > 2)
          {
            num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
            numPtr += 2;
            length -= 4;
          }
          else
            break;
        }
        return num1 + num2 * 1566083941;
      }
    }

    public string[] Split(params char[] separator)
    {
      return this.SplitInternal(separator, int.MaxValue, StringSplitOptions.None);
    }

    public string[] Split(char[] separator, int count)
    {
      return this.SplitInternal(separator, count, StringSplitOptions.None);
    }

    [ComVisible(false)]
    public string[] Split(char[] separator, StringSplitOptions options)
    {
      return this.SplitInternal(separator, int.MaxValue, options);
    }

    [ComVisible(false)]
    public string[] Split(char[] separator, int count, StringSplitOptions options)
    {
      return this.SplitInternal(separator, count, options);
    }

    [ComVisible(false)]
    internal string[] SplitInternal(char[] separator, int count, StringSplitOptions options)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
      if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
      {
        throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[1]
        {
          (object) options
        }));
      }
      else
      {
        bool flag = options == StringSplitOptions.RemoveEmptyEntries;
        if (count == 0 || flag && this.Length == 0)
          return new string[0];
        int[] sepList = new int[this.Length];
        int numReplaces = this.MakeSeparatorList(separator, ref sepList);
        if (numReplaces == 0 || count == 1)
          return new string[1]
          {
            this
          };
        else if (flag)
          return this.InternalSplitOmitEmptyEntries(sepList, (int[]) null, numReplaces, count);
        else
          return this.InternalSplitKeepEmptyEntries(sepList, (int[]) null, numReplaces, count);
      }
    }

    [ComVisible(false)]
    public string[] Split(string[] separator, StringSplitOptions options)
    {
      return this.Split(separator, int.MaxValue, options);
    }

    [ComVisible(false)]
    public string[] Split(string[] separator, int count, StringSplitOptions options)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
      if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
      {
        throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[1]
        {
          (object) options
        }));
      }
      else
      {
        bool flag = options == StringSplitOptions.RemoveEmptyEntries;
        if (separator == null || separator.Length == 0)
          return this.SplitInternal((char[]) null, count, options);
        if (count == 0 || flag && this.Length == 0)
          return new string[0];
        int[] sepList = new int[this.Length];
        int[] lengthList = new int[this.Length];
        int numReplaces = this.MakeSeparatorList(separator, ref sepList, ref lengthList);
        if (numReplaces == 0 || count == 1)
          return new string[1]
          {
            this
          };
        else if (flag)
          return this.InternalSplitOmitEmptyEntries(sepList, lengthList, numReplaces, count);
        else
          return this.InternalSplitKeepEmptyEntries(sepList, lengthList, numReplaces, count);
      }
    }

    private string[] InternalSplitKeepEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
    {
      int startIndex = 0;
      int index1 = 0;
      --count;
      int num = numReplaces < count ? numReplaces : count;
      string[] strArray = new string[num + 1];
      for (int index2 = 0; index2 < num && startIndex < this.Length; ++index2)
      {
        strArray[index1++] = this.Substring(startIndex, sepList[index2] - startIndex);
        startIndex = sepList[index2] + (lengthList == null ? 1 : lengthList[index2]);
      }
      if (startIndex < this.Length && num >= 0)
        strArray[index1] = this.Substring(startIndex);
      else if (index1 == num)
        strArray[index1] = string.Empty;
      return strArray;
    }

    private string[] InternalSplitOmitEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
    {
      int length1 = numReplaces < count ? numReplaces + 1 : count;
      string[] strArray1 = new string[length1];
      int startIndex = 0;
      int length2 = 0;
      for (int index = 0; index < numReplaces && startIndex < this.Length; ++index)
      {
        if (sepList[index] - startIndex > 0)
          strArray1[length2++] = this.Substring(startIndex, sepList[index] - startIndex);
        startIndex = sepList[index] + (lengthList == null ? 1 : lengthList[index]);
        if (length2 == count - 1)
        {
          while (index < numReplaces - 1 && startIndex == sepList[++index])
            startIndex += lengthList == null ? 1 : lengthList[index];
          break;
        }
      }
      if (startIndex < this.Length)
        strArray1[length2++] = this.Substring(startIndex);
      string[] strArray2 = strArray1;
      if (length2 != length1)
      {
        strArray2 = new string[length2];
        for (int index = 0; index < length2; ++index)
          strArray2[index] = strArray1[index];
      }
      return strArray2;
    }

    [SecuritySafeCritical]
    private unsafe int MakeSeparatorList(char[] separator, ref int[] sepList)
    {
      int num1 = 0;
      if (separator == null || separator.Length == 0)
      {
        fixed (char* chPtr = &this.m_firstChar)
        {
          for (int index = 0; index < this.Length && num1 < sepList.Length; ++index)
          {
            if (char.IsWhiteSpace(chPtr[index]))
              sepList[num1++] = index;
          }
        }
      }
      else
      {
        int length1 = sepList.Length;
        int length2 = separator.Length;
        fixed (char* chPtr1 = &this.m_firstChar)
          fixed (char* chPtr2 = separator)
          {
            for (int index = 0; index < this.Length && num1 < length1; ++index)
            {
              char* chPtr3 = chPtr2;
              int num2 = 0;
              while (num2 < length2)
              {
                if ((int) chPtr1[index] == (int) *chPtr3)
                {
                  sepList[num1++] = index;
                  break;
                }
                else
                {
                  ++num2;
                  ++chPtr3;
                }
              }
            }
          }
      }
      return num1;
    }

    [SecuritySafeCritical]
    private unsafe int MakeSeparatorList(string[] separators, ref int[] sepList, ref int[] lengthList)
    {
      int index1 = 0;
      int length1 = sepList.Length;
      fixed (char* chPtr = &this.m_firstChar)
      {
        for (int indexA = 0; indexA < this.Length && index1 < length1; ++indexA)
        {
          for (int index2 = 0; index2 < separators.Length; ++index2)
          {
            string strB = separators[index2];
            if (!string.IsNullOrEmpty(strB))
            {
              int length2 = strB.Length;
              if ((int) chPtr[indexA] == (int) strB[0] && length2 <= this.Length - indexA && (length2 == 1 || string.CompareOrdinal(this, indexA, strB, 0, length2) == 0))
              {
                sepList[index1] = indexA;
                lengthList[index1] = length2;
                ++index1;
                indexA += length2 - 1;
                break;
              }
            }
          }
        }
      }
      return index1;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string Substring(int startIndex)
    {
      return this.Substring(startIndex, this.Length - startIndex);
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string Substring(int startIndex, int length)
    {
      return this.InternalSubStringWithChecks(startIndex, length, false);
    }

    [SecurityCritical]
    internal string InternalSubStringWithChecks(int startIndex, int length, bool fAlwaysCopy)
    {
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      if (startIndex > this.Length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndexLargerThanLength"));
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
      if (startIndex > this.Length - length)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_IndexLength"));
      if (length == 0)
        return string.Empty;
      else
        return this.InternalSubString(startIndex, length, fAlwaysCopy);
    }

    [SecurityCritical]
    private unsafe string InternalSubString(int startIndex, int length, bool fAlwaysCopy)
    {
      if (startIndex == 0 && length == this.Length && !fAlwaysCopy)
        return this;
      string str = string.FastAllocateString(length);
      fixed (char* dmem = &str.m_firstChar)
        fixed (char* chPtr = &this.m_firstChar)
          string.wstrcpy(dmem, chPtr + startIndex, length);
      return str;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string Trim(params char[] trimChars)
    {
      if (trimChars == null || trimChars.Length == 0)
        return this.TrimHelper(2);
      else
        return this.TrimHelper(trimChars, 2);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string TrimStart(params char[] trimChars)
    {
      if (trimChars == null || trimChars.Length == 0)
        return this.TrimHelper(0);
      else
        return this.TrimHelper(trimChars, 0);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string TrimEnd(params char[] trimChars)
    {
      if (trimChars == null || trimChars.Length == 0)
        return this.TrimHelper(1);
      else
        return this.TrimHelper(trimChars, 1);
    }

    [SecurityCritical]
    private static unsafe string CreateString(sbyte* value, int startIndex, int length, Encoding enc)
    {
      if (enc == null)
        return new string(value, startIndex, length);
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      if (value + startIndex < value)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
      byte[] numArray = new byte[length];
      try
      {
        Buffer.memcpy((byte*) value, startIndex, numArray, 0, length);
      }
      catch (NullReferenceException ex)
      {
        throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
      }
      return enc.GetString(numArray);
    }

    [SecurityCritical]
    internal static unsafe string CreateStringFromEncoding(byte* bytes, int byteLength, Encoding encoding)
    {
      int charCount = encoding.GetCharCount(bytes, byteLength, (DecoderNLS) null);
      if (charCount == 0)
        return string.Empty;
      string str = string.FastAllocateString(charCount);
      fixed (char* chars = &str.m_firstChar)
        encoding.GetChars(bytes, byteLength, chars, charCount, (DecoderNLS) null);
      return str;
    }

    [SecuritySafeCritical]
    internal unsafe byte[] ConvertToAnsi(int iMaxDBCSCharByteSize, bool fBestFit, bool fThrowOnUnmappableChar, out int cbLength)
    {
      int cbDestBuffer = (this.Length + 3) * iMaxDBCSCharByteSize;
      byte[] numArray = new byte[cbDestBuffer];
      uint flags = fBestFit ? 0U : 1024U;
      uint num = 0U;
      int index;
      fixed (byte* pbDestBuffer = numArray)
        fixed (char* pwzSource = &this.m_firstChar)
          index = Win32Native.WideCharToMultiByte(0U, flags, pwzSource, this.Length, pbDestBuffer, cbDestBuffer, IntPtr.Zero, fThrowOnUnmappableChar ? new IntPtr((void*) &num) : IntPtr.Zero);
      if ((int) num != 0)
        throw new ArgumentException(Environment.GetResourceString("Interop_Marshal_Unmappable_Char"));
      cbLength = index;
      numArray[index] = (byte) 0;
      return numArray;
    }

    public bool IsNormalized()
    {
      return this.IsNormalized(NormalizationForm.FormC);
    }

    [SecuritySafeCritical]
    public bool IsNormalized(NormalizationForm normalizationForm)
    {
      if (this.IsFastSort() && (normalizationForm == NormalizationForm.FormC || normalizationForm == NormalizationForm.FormKC || (normalizationForm == NormalizationForm.FormD || normalizationForm == NormalizationForm.FormKD)))
        return true;
      else
        return Normalization.IsNormalized(this, normalizationForm);
    }

    public string Normalize()
    {
      return this.Normalize(NormalizationForm.FormC);
    }

    [SecuritySafeCritical]
    public string Normalize(NormalizationForm normalizationForm)
    {
      if (this.IsAscii() && (normalizationForm == NormalizationForm.FormC || normalizationForm == NormalizationForm.FormKC || (normalizationForm == NormalizationForm.FormD || normalizationForm == NormalizationForm.FormKD)))
        return this;
      else
        return Normalization.Normalize(this, normalizationForm);
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static string FastAllocateString(int length);

    [SecuritySafeCritical]
    private static unsafe void FillStringChecked(string dest, int destPos, string src)
    {
      if (src.Length > dest.Length - destPos)
        throw new IndexOutOfRangeException();
      fixed (char* chPtr = &dest.m_firstChar)
        fixed (char* smem = &src.m_firstChar)
          string.wstrcpy(chPtr + destPos, smem, src.Length);
    }

    [SecurityCritical]
    private static unsafe void wstrcpyPtrAligned(char* dmem, char* smem, int charCount)
    {
      while (charCount >= 8)
      {
        *(int*) dmem = (int) *(uint*) smem;
        *(int*) (dmem + 2) = (int) *(uint*) (smem + 2);
        *(int*) (dmem + 4) = (int) *(uint*) (smem + 4);
        *(int*) (dmem + 6) = (int) *(uint*) (smem + 6);
        dmem += 8;
        smem += 8;
        charCount -= 8;
      }
      if ((charCount & 4) != 0)
      {
        *(int*) dmem = (int) *(uint*) smem;
        *(int*) (dmem + 2) = (int) *(uint*) (smem + 2);
        dmem += 4;
        smem += 4;
      }
      if ((charCount & 2) != 0)
      {
        *(int*) dmem = (int) *(uint*) smem;
        dmem += 2;
        smem += 2;
      }
      if ((charCount & 1) == 0)
        return;
      *dmem = *smem;
    }

    [SecurityCritical]
    internal static unsafe void wstrcpy(char* dmem, char* smem, int charCount)
    {
      if (charCount <= 0)
        return;
      if (((int) dmem & 2) != 0)
      {
        *dmem = *smem;
        ++dmem;
        ++smem;
        --charCount;
      }
      while (charCount >= 8)
      {
        *(int*) dmem = (int) *(uint*) smem;
        *(int*) (dmem + 2) = (int) *(uint*) (smem + 2);
        *(int*) (dmem + 4) = (int) *(uint*) (smem + 4);
        *(int*) (dmem + 6) = (int) *(uint*) (smem + 6);
        dmem += 8;
        smem += 8;
        charCount -= 8;
      }
      if ((charCount & 4) != 0)
      {
        *(int*) dmem = (int) *(uint*) smem;
        *(int*) (dmem + 2) = (int) *(uint*) (smem + 2);
        dmem += 4;
        smem += 4;
      }
      if ((charCount & 2) != 0)
      {
        *(int*) dmem = (int) *(uint*) smem;
        dmem += 2;
        smem += 2;
      }
      if ((charCount & 1) == 0)
        return;
      *dmem = *smem;
    }

    [SecuritySafeCritical]
    private unsafe string CtorCharArray(char[] value)
    {
      if (value == null || value.Length == 0)
        return string.Empty;
      string str = string.FastAllocateString(value.Length);
      fixed (char* dmem = str)
        fixed (char* smem = value)
          string.wstrcpyPtrAligned(dmem, smem, value.Length);
      return str;
    }

    [SecuritySafeCritical]
    private unsafe string CtorCharArrayStartLength(char[] value, int startIndex, int length)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
      if (startIndex > value.Length - length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (length <= 0)
        return string.Empty;
      string str = string.FastAllocateString(length);
      fixed (char* dmem = str)
        fixed (char* chPtr = value)
          string.wstrcpy(dmem, chPtr + startIndex, length);
      return str;
    }

    [SecuritySafeCritical]
    private unsafe string CtorCharCount(char c, int count)
    {
      if (count > 0)
      {
        string str = string.FastAllocateString(count);
        fixed (char* chPtr1 = str)
        {
          char* chPtr2;
          for (chPtr2 = chPtr1; ((int) (uint) chPtr2 & 3) != 0 && count > 0; --count)
            *chPtr2++ = c;
          uint num = (uint) c << 16 | (uint) c;
          if (count >= 4)
          {
            count -= 4;
            do
            {
              *(int*) chPtr2 = (int) num;
              *(int*) (chPtr2 + 2) = (int) num;
              chPtr2 += 4;
              count -= 4;
            }
            while (count >= 0);
          }
          if ((count & 2) != 0)
          {
            *(int*) chPtr2 = (int) num;
            chPtr2 += 2;
          }
          if ((count & 1) != 0)
            *chPtr2 = c;
        }
        return str;
      }
      else
      {
        if (count == 0)
          return string.Empty;
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", new object[1]
        {
          (object) "count"
        }));
      }
    }

    [ForceTokenStabilization]
    [SecurityCritical]
    private static unsafe int wcslen(char* ptr)
    {
      char* chPtr = ptr;
      while (((int) (uint) chPtr & 3) != 0 && (int) *chPtr != 0)
        ++chPtr;
      if ((int) *chPtr != 0)
      {
        while (((int) *chPtr & (int) chPtr[1]) != 0 || (int) *chPtr != 0 && (int) chPtr[1] != 0)
          chPtr += 2;
      }
      while ((int) *chPtr != 0)
        ++chPtr;
      return (int) (chPtr - ptr);
    }

    [SecurityCritical]
    private unsafe string CtorCharPtr(char* ptr)
    {
      if ((IntPtr) ptr == IntPtr.Zero)
        return string.Empty;
      if ((UIntPtr) ptr < UIntPtr(64000))
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeStringPtrNotAtom"));
      try
      {
        int num = string.wcslen(ptr);
        string str = string.FastAllocateString(num);
        fixed (char* dmem = str)
          string.wstrcpy(dmem, ptr, num);
        return str;
      }
      catch (NullReferenceException ex)
      {
        throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
      }
    }

    [ForceTokenStabilization]
    [SecurityCritical]
    private unsafe string CtorCharPtrStartLength(char* ptr, int startIndex, int length)
    {
      if (length < 0)
        throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      char* smem = ptr + startIndex;
      if (smem < ptr)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
      string str = string.FastAllocateString(length);
      try
      {
        fixed (char* dmem = str)
          string.wstrcpy(dmem, smem, length);
        return str;
      }
      catch (NullReferenceException ex)
      {
        throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int Compare(string strA, string strB)
    {
      return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int Compare(string strA, string strB, bool ignoreCase)
    {
      if (ignoreCase)
        return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
      else
        return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
    }

    [SecuritySafeCritical]
    public static int Compare(string strA, string strB, StringComparison comparisonType)
    {
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (strA == strB)
        return 0;
      if (strA == null)
        return -1;
      if (strB == null)
        return 1;
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
        case StringComparison.Ordinal:
          return string.CompareOrdinalHelper(strA, strB);
        case StringComparison.OrdinalIgnoreCase:
          if (strA.IsAscii() && strB.IsAscii())
            return string.CompareOrdinalIgnoreCaseHelper(strA, strB);
          else
            return TextInfo.CompareOrdinalIgnoreCase(strA, strB);
        default:
          throw new NotSupportedException(Environment.GetResourceString("NotSupported_StringComparison"));
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      else
        return culture.CompareInfo.Compare(strA, strB, options);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      if (ignoreCase)
        return culture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
      else
        return culture.CompareInfo.Compare(strA, strB, CompareOptions.None);
    }

    public static int Compare(string strA, int indexA, string strB, int indexB, int length)
    {
      int length1 = length;
      int length2 = length;
      if (strA != null && strA.Length - indexA < length1)
        length1 = strA.Length - indexA;
      if (strB != null && strB.Length - indexB < length2)
        length2 = strB.Length - indexB;
      return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, CompareOptions.None);
    }

    public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase)
    {
      int length1 = length;
      int length2 = length;
      if (strA != null && strA.Length - indexA < length1)
        length1 = strA.Length - indexA;
      if (strB != null && strB.Length - indexB < length2)
        length2 = strB.Length - indexB;
      if (ignoreCase)
        return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, CompareOptions.IgnoreCase);
      else
        return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, CompareOptions.None);
    }

    public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      int length1 = length;
      int length2 = length;
      if (strA != null && strA.Length - indexA < length1)
        length1 = strA.Length - indexA;
      if (strB != null && strB.Length - indexB < length2)
        length2 = strB.Length - indexB;
      if (ignoreCase)
        return culture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, CompareOptions.IgnoreCase);
      else
        return culture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, CompareOptions.None);
    }

    public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      int length1 = length;
      int length2 = length;
      if (strA != null && strA.Length - indexA < length1)
        length1 = strA.Length - indexA;
      if (strB != null && strB.Length - indexB < length2)
        length2 = strB.Length - indexB;
      return culture.CompareInfo.Compare(strA, indexA, length1, strB, indexB, length2, options);
    }

    [SecuritySafeCritical]
    public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
    {
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (strA == null || strB == null)
      {
        if (strA == strB)
          return 0;
        if (strA != null)
          return 1;
        else
          return -1;
      }
      else
      {
        if (length < 0)
          throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
        if (indexA < 0)
          throw new ArgumentOutOfRangeException("indexA", Environment.GetResourceString("ArgumentOutOfRange_Index"));
        if (indexB < 0)
          throw new ArgumentOutOfRangeException("indexB", Environment.GetResourceString("ArgumentOutOfRange_Index"));
        if (strA.Length - indexA < 0)
          throw new ArgumentOutOfRangeException("indexA", Environment.GetResourceString("ArgumentOutOfRange_Index"));
        if (strB.Length - indexB < 0)
          throw new ArgumentOutOfRangeException("indexB", Environment.GetResourceString("ArgumentOutOfRange_Index"));
        if (length == 0 || strA == strB && indexA == indexB)
          return 0;
        int num1 = length;
        int num2 = length;
        if (strA != null && strA.Length - indexA < num1)
          num1 = strA.Length - indexA;
        if (strB != null && strB.Length - indexB < num2)
          num2 = strB.Length - indexB;
        switch (comparisonType)
        {
          case StringComparison.CurrentCulture:
            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num1, strB, indexB, num2, CompareOptions.None);
          case StringComparison.CurrentCultureIgnoreCase:
            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num1, strB, indexB, num2, CompareOptions.IgnoreCase);
          case StringComparison.InvariantCulture:
            return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, num1, strB, indexB, num2, CompareOptions.None);
          case StringComparison.InvariantCultureIgnoreCase:
            return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, num1, strB, indexB, num2, CompareOptions.IgnoreCase);
          case StringComparison.Ordinal:
            return string.nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);
          case StringComparison.OrdinalIgnoreCase:
            return TextInfo.CompareOrdinalIgnoreCaseEx(strA, indexA, strB, indexB, num1, num2);
          default:
            throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"));
        }
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int CompareTo(object value)
    {
      if (value == null)
        return 1;
      if (!(value is string))
        throw new ArgumentException(Environment.GetResourceString("Arg_MustBeString"));
      else
        return string.Compare(this, (string) value, StringComparison.CurrentCulture);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int CompareTo(string strB)
    {
      if (strB == null)
        return 1;
      else
        return CultureInfo.CurrentCulture.CompareInfo.Compare(this, strB, CompareOptions.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int CompareOrdinal(string strA, string strB)
    {
      if (strA == strB)
        return 0;
      if (strA == null)
        return -1;
      if (strB == null)
        return 1;
      else
        return string.CompareOrdinalHelper(strA, strB);
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length)
    {
      if (strA != null && strB != null)
        return string.nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);
      if (strA == strB)
        return 0;
      if (strA != null)
        return 1;
      else
        return -1;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public bool Contains(string value)
    {
      return this.IndexOf(value, StringComparison.Ordinal) >= 0;
    }

    public bool EndsWith(string value)
    {
      return this.EndsWith(value, StringComparison.CurrentCulture);
    }

    [ComVisible(false)]
    [SecuritySafeCritical]
    public bool EndsWith(string value, StringComparison comparisonType)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (this == value || value.Length == 0)
        return true;
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
        case StringComparison.Ordinal:
          if (this.Length >= value.Length)
            return string.nativeCompareOrdinalEx(this, this.Length - value.Length, value, 0, value.Length) == 0;
          else
            return false;
        case StringComparison.OrdinalIgnoreCase:
          if (this.Length >= value.Length)
            return TextInfo.CompareOrdinalIgnoreCaseEx(this, this.Length - value.Length, value, 0, value.Length, value.Length) == 0;
          else
            return false;
        default:
          throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      }
    }

    public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (this == value)
        return true;
      else
        return (culture != null ? culture : CultureInfo.CurrentCulture).CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    internal bool EndsWith(char value)
    {
      int length = this.Length;
      if (length != 0 && (int) this[length - 1] == (int) value)
        return true;
      else
        return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOf(char value)
    {
      return this.IndexOf(value, 0, this.Length);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOf(char value, int startIndex)
    {
      return this.IndexOf(value, startIndex, this.Length - startIndex);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int IndexOf(char value, int startIndex, int count);

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOfAny(char[] anyOf)
    {
      return this.IndexOfAny(anyOf, 0, this.Length);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOfAny(char[] anyOf, int startIndex)
    {
      return this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int IndexOfAny(char[] anyOf, int startIndex, int count);

    public int IndexOf(string value)
    {
      return this.IndexOf(value, StringComparison.CurrentCulture);
    }

    public int IndexOf(string value, int startIndex)
    {
      return this.IndexOf(value, startIndex, StringComparison.CurrentCulture);
    }

    public int IndexOf(string value, int startIndex, int count)
    {
      if (startIndex < 0 || startIndex > this.Length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (count < 0 || count > this.Length - startIndex)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
      else
        return this.IndexOf(value, startIndex, count, StringComparison.CurrentCulture);
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOf(string value, StringComparison comparisonType)
    {
      return this.IndexOf(value, 0, this.Length, comparisonType);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int IndexOf(string value, int startIndex, StringComparison comparisonType)
    {
      return this.IndexOf(value, startIndex, this.Length - startIndex, comparisonType);
    }

    public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0 || startIndex > this.Length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
      if (count < 0 || startIndex > this.Length - count)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
        case StringComparison.Ordinal:
          return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.Ordinal);
        case StringComparison.OrdinalIgnoreCase:
          return TextInfo.IndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);
        default:
          throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public int LastIndexOf(char value)
    {
      return this.LastIndexOf(value, this.Length - 1, this.Length);
    }

    public int LastIndexOf(char value, int startIndex)
    {
      return this.LastIndexOf(value, startIndex, startIndex + 1);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int LastIndexOf(char value, int startIndex, int count);

    public int LastIndexOfAny(char[] anyOf)
    {
      return this.LastIndexOfAny(anyOf, this.Length - 1, this.Length);
    }

    public int LastIndexOfAny(char[] anyOf, int startIndex)
    {
      return this.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int LastIndexOfAny(char[] anyOf, int startIndex, int count);

    [SecuritySafeCritical]
    public int LastIndexOf(string value)
    {
      return this.LastIndexOf(value, this.Length - 1, this.Length, StringComparison.CurrentCulture);
    }

    public int LastIndexOf(string value, int startIndex)
    {
      return this.LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);
    }

    public int LastIndexOf(string value, int startIndex, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
      else
        return this.LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    [SecuritySafeCritical]
    public int LastIndexOf(string value, StringComparison comparisonType)
    {
      return this.LastIndexOf(value, this.Length - 1, this.Length, comparisonType);
    }

    public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
    {
      return this.LastIndexOf(value, startIndex, startIndex + 1, comparisonType);
    }

    public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (this.Length == 0 && (startIndex == -1 || startIndex == 0))
      {
        if (value.Length != 0)
          return -1;
        else
          return 0;
      }
      else
      {
        if (startIndex < 0 || startIndex > this.Length)
          throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
        if (startIndex == this.Length)
        {
          --startIndex;
          if (count > 0)
            --count;
          if (value.Length == 0 && count >= 0 && startIndex - count + 1 >= 0)
            return startIndex;
        }
        if (count < 0 || startIndex - count + 1 < 0)
          throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
        switch (comparisonType)
        {
          case StringComparison.CurrentCulture:
            return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
          case StringComparison.CurrentCultureIgnoreCase:
            return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
          case StringComparison.InvariantCulture:
            return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
          case StringComparison.InvariantCultureIgnoreCase:
            return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
          case StringComparison.Ordinal:
            return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.Ordinal);
          case StringComparison.OrdinalIgnoreCase:
            return TextInfo.LastIndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);
          default:
            throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
        }
      }
    }

    public string PadLeft(int totalWidth)
    {
      return this.PadHelper(totalWidth, ' ', false);
    }

    public string PadLeft(int totalWidth, char paddingChar)
    {
      return this.PadHelper(totalWidth, paddingChar, false);
    }

    public string PadRight(int totalWidth)
    {
      return this.PadHelper(totalWidth, ' ', true);
    }

    public string PadRight(int totalWidth, char paddingChar)
    {
      return this.PadHelper(totalWidth, paddingChar, true);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string PadHelper(int totalWidth, char paddingChar, bool isRightPadded);

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public bool StartsWith(string value)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      else
        return this.StartsWith(value, StringComparison.CurrentCulture);
    }

    [SecuritySafeCritical]
    [ComVisible(false)]
    public bool StartsWith(string value, StringComparison comparisonType)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
        throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      if (this == value || value.Length == 0)
        return true;
      switch (comparisonType)
      {
        case StringComparison.CurrentCulture:
          return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
        case StringComparison.CurrentCultureIgnoreCase:
          return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
        case StringComparison.InvariantCulture:
          return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
        case StringComparison.InvariantCultureIgnoreCase:
          return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
        case StringComparison.Ordinal:
          if (this.Length < value.Length)
            return false;
          else
            return string.nativeCompareOrdinalEx(this, 0, value, 0, value.Length) == 0;
        case StringComparison.OrdinalIgnoreCase:
          if (this.Length < value.Length)
            return false;
          else
            return TextInfo.CompareOrdinalIgnoreCaseEx(this, 0, value, 0, value.Length, value.Length) == 0;
        default:
          throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
      }
    }

    public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (this == value)
        return true;
      else
        return (culture != null ? culture : CultureInfo.CurrentCulture).CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
    }

    public string ToLower()
    {
      return this.ToLower(CultureInfo.CurrentCulture);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToLower(CultureInfo culture)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      else
        return culture.TextInfo.ToLower(this);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToLowerInvariant()
    {
      return this.ToLower(CultureInfo.InvariantCulture);
    }

    public string ToUpper()
    {
      return this.ToUpper(CultureInfo.CurrentCulture);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToUpper(CultureInfo culture)
    {
      if (culture == null)
        throw new ArgumentNullException("culture");
      else
        return culture.TextInfo.ToUpper(this);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToUpperInvariant()
    {
      return this.ToUpper(CultureInfo.InvariantCulture);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public override string ToString()
    {
      return this;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string ToString(IFormatProvider provider)
    {
      return this;
    }

    public object Clone()
    {
      return (object) this;
    }

    public string Trim()
    {
      return this.TrimHelper(2);
    }

    [SecuritySafeCritical]
    private string TrimHelper(int trimType)
    {
      int end = this.Length - 1;
      int start = 0;
      if (trimType != 1)
      {
        start = 0;
        while (start < this.Length && char.IsWhiteSpace(this[start]))
          ++start;
      }
      if (trimType != 0)
      {
        end = this.Length - 1;
        while (end >= start && char.IsWhiteSpace(this[end]))
          --end;
      }
      return this.CreateTrimmedString(start, end);
    }

    [SecuritySafeCritical]
    private string TrimHelper(char[] trimChars, int trimType)
    {
      int end = this.Length - 1;
      int start = 0;
      if (trimType != 1)
      {
        for (start = 0; start < this.Length; ++start)
        {
          char ch = this[start];
          int index = 0;
          while (index < trimChars.Length && (int) trimChars[index] != (int) ch)
            ++index;
          if (index == trimChars.Length)
            break;
        }
      }
      if (trimType != 0)
      {
        for (end = this.Length - 1; end >= start; --end)
        {
          char ch = this[end];
          int index = 0;
          while (index < trimChars.Length && (int) trimChars[index] != (int) ch)
            ++index;
          if (index == trimChars.Length)
            break;
        }
      }
      return this.CreateTrimmedString(start, end);
    }

    [SecurityCritical]
    private string CreateTrimmedString(int start, int end)
    {
      int length = end - start + 1;
      if (length == this.Length)
        return this;
      if (length == 0)
        return string.Empty;
      else
        return this.InternalSubString(start, length, false);
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string InsertInternal(int startIndex, string value);

    [SecuritySafeCritical]
    public string Insert(int startIndex, string value)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0 || startIndex > this.Length)
        throw new ArgumentOutOfRangeException("startIndex");
      else
        return this.InsertInternal(startIndex, value);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string ReplaceInternal(char oldChar, char newChar);

    [SecuritySafeCritical]
    public string Replace(char oldChar, char newChar)
    {
      return this.ReplaceInternal(oldChar, newChar);
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string ReplaceInternal(string oldValue, string newValue);

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public string Replace(string oldValue, string newValue)
    {
      if (oldValue == null)
        throw new ArgumentNullException("oldValue");
      else
        return this.ReplaceInternal(oldValue, newValue);
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string RemoveInternal(int startIndex, int count);

    [SecuritySafeCritical]
    public string Remove(int startIndex, int count)
    {
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      else
        return this.RemoveInternal(startIndex, count);
    }

    public string Remove(int startIndex)
    {
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
      if (startIndex >= this.Length)
        throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndexLessThanLength"));
      else
        return this.Substring(0, startIndex);
    }

    public static string Format(string format, object arg0)
    {
      if (format == null)
        throw new ArgumentNullException("format");
      return string.Format((IFormatProvider) null, format, new object[1]
      {
        arg0
      });
    }

    public static string Format(string format, object arg0, object arg1)
    {
      if (format == null)
        throw new ArgumentNullException("format");
      return string.Format((IFormatProvider) null, format, new object[2]
      {
        arg0,
        arg1
      });
    }

    public static string Format(string format, object arg0, object arg1, object arg2)
    {
      if (format == null)
        throw new ArgumentNullException("format");
      return string.Format((IFormatProvider) null, format, arg0, arg1, arg2);
    }

    public static string Format(string format, params object[] args)
    {
      if (format == null || args == null)
        throw new ArgumentNullException(format == null ? "format" : "args");
      else
        return string.Format((IFormatProvider) null, format, args);
    }

    [SecuritySafeCritical]
    public static string Format(IFormatProvider provider, string format, params object[] args)
    {
      if (format == null || args == null)
        throw new ArgumentNullException(format == null ? "format" : "args");
      StringBuilder stringBuilder = new StringBuilder(format.Length + args.Length * 8);
      stringBuilder.AppendFormat(provider, format, args);
      return ((object) stringBuilder).ToString();
    }

    [SecuritySafeCritical]
    public static unsafe string Copy(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      int length = str.Length;
      string str1 = string.FastAllocateString(length);
      fixed (char* dmem = &str1.m_firstChar)
        fixed (char* smem = &str.m_firstChar)
          string.wstrcpyPtrAligned(dmem, smem, length);
      return str1;
    }

    public static string Concat(object arg0)
    {
      if (arg0 == null)
        return string.Empty;
      else
        return arg0.ToString();
    }

    public static string Concat(object arg0, object arg1)
    {
      if (arg0 == null)
        arg0 = (object) string.Empty;
      if (arg1 == null)
        arg1 = (object) string.Empty;
      return arg0.ToString() + arg1.ToString();
    }

    public static string Concat(object arg0, object arg1, object arg2)
    {
      if (arg0 == null)
        arg0 = (object) string.Empty;
      if (arg1 == null)
        arg1 = (object) string.Empty;
      if (arg2 == null)
        arg2 = (object) string.Empty;
      return arg0.ToString() + arg1.ToString() + arg2.ToString();
    }

    [CLSCompliant(false)]
    [SecuritySafeCritical]
    public static string Concat(object arg0, object arg1, object arg2, object arg3)
    {
      ArgIterator argIterator = new ArgIterator(__arglist);
      int length = argIterator.GetRemainingCount() + 4;
      object[] objArray = new object[length];
      objArray[0] = arg0;
      objArray[1] = arg1;
      objArray[2] = arg2;
      objArray[3] = arg3;
      for (int index = 4; index < length; ++index)
        objArray[index] = TypedReference.ToObject(argIterator.GetNextArg());
      return string.Concat(objArray);
    }

    public static string Concat(params object[] args)
    {
      if (args == null)
        throw new ArgumentNullException("args");
      string[] values = new string[args.Length];
      int totalLength = 0;
      for (int index = 0; index < args.Length; ++index)
      {
        object obj = args[index];
        values[index] = obj == null ? string.Empty : obj.ToString();
        if (values[index] == null)
          values[index] = string.Empty;
        totalLength += values[index].Length;
        if (totalLength < 0)
          throw new OutOfMemoryException();
      }
      return string.ConcatArray(values, totalLength);
    }

    [ComVisible(false)]
    public static string Concat<T>(IEnumerable<T> values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      StringBuilder stringBuilder = new StringBuilder();
      using (IEnumerator<T> enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if ((object) enumerator.Current != null)
          {
            string str = enumerator.Current.ToString();
            if (str != null)
              stringBuilder.Append(str);
          }
        }
      }
      return ((object) stringBuilder).ToString();
    }

    [ComVisible(false)]
    public static string Concat(IEnumerable<string> values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      StringBuilder stringBuilder = new StringBuilder();
      using (IEnumerator<string> enumerator = values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current != null)
            stringBuilder.Append(enumerator.Current);
        }
      }
      return ((object) stringBuilder).ToString();
    }

    [SecuritySafeCritical]
    public static string Concat(string str0, string str1)
    {
      if (string.IsNullOrEmpty(str0))
      {
        if (string.IsNullOrEmpty(str1))
          return string.Empty;
        else
          return str1;
      }
      else
      {
        if (string.IsNullOrEmpty(str1))
          return str0;
        int length = str0.Length;
        string dest = string.FastAllocateString(length + str1.Length);
        string.FillStringChecked(dest, 0, str0);
        string.FillStringChecked(dest, length, str1);
        return dest;
      }
    }

    [SecuritySafeCritical]
    public static string Concat(string str0, string str1, string str2)
    {
      if (str0 == null && str1 == null && str2 == null)
        return string.Empty;
      if (str0 == null)
        str0 = string.Empty;
      if (str1 == null)
        str1 = string.Empty;
      if (str2 == null)
        str2 = string.Empty;
      string dest = string.FastAllocateString(str0.Length + str1.Length + str2.Length);
      string.FillStringChecked(dest, 0, str0);
      string.FillStringChecked(dest, str0.Length, str1);
      string.FillStringChecked(dest, str0.Length + str1.Length, str2);
      return dest;
    }

    [SecuritySafeCritical]
    public static string Concat(string str0, string str1, string str2, string str3)
    {
      if (str0 == null && str1 == null && (str2 == null && str3 == null))
        return string.Empty;
      if (str0 == null)
        str0 = string.Empty;
      if (str1 == null)
        str1 = string.Empty;
      if (str2 == null)
        str2 = string.Empty;
      if (str3 == null)
        str3 = string.Empty;
      string dest = string.FastAllocateString(str0.Length + str1.Length + str2.Length + str3.Length);
      string.FillStringChecked(dest, 0, str0);
      string.FillStringChecked(dest, str0.Length, str1);
      string.FillStringChecked(dest, str0.Length + str1.Length, str2);
      string.FillStringChecked(dest, str0.Length + str1.Length + str2.Length, str3);
      return dest;
    }

    [SecuritySafeCritical]
    private static string ConcatArray(string[] values, int totalLength)
    {
      string dest = string.FastAllocateString(totalLength);
      int destPos = 0;
      for (int index = 0; index < values.Length; ++index)
      {
        string.FillStringChecked(dest, destPos, values[index]);
        destPos += values[index].Length;
      }
      return dest;
    }

    public static string Concat(params string[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");
      int totalLength = 0;
      string[] values1 = new string[values.Length];
      for (int index = 0; index < values.Length; ++index)
      {
        string str = values[index];
        values1[index] = str == null ? string.Empty : str;
        totalLength += values1[index].Length;
        if (totalLength < 0)
          throw new OutOfMemoryException();
      }
      return string.ConcatArray(values1, totalLength);
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static string Intern(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      else
        return Thread.GetDomain().GetOrInternString(str);
    }

    [SecuritySafeCritical]
    public static string IsInterned(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      else
        return Thread.GetDomain().IsStringInterned(str);
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public TypeCode GetTypeCode()
    {
      return TypeCode.String;
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return Convert.ToBoolean(this, provider);
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return Convert.ToChar(this, provider);
    }

    [SecuritySafeCritical]
    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return Convert.ToSByte(this, provider);
    }

    [SecuritySafeCritical]
    byte IConvertible.ToByte(IFormatProvider provider)
    {
      return Convert.ToByte(this, provider);
    }

    [SecuritySafeCritical]
    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return Convert.ToInt16(this, provider);
    }

    [SecuritySafeCritical]
    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return Convert.ToUInt16(this, provider);
    }

    [SecuritySafeCritical]
    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return Convert.ToInt32(this, provider);
    }

    [SecuritySafeCritical]
    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return Convert.ToUInt32(this, provider);
    }

    [SecuritySafeCritical]
    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return Convert.ToInt64(this, provider);
    }

    [SecuritySafeCritical]
    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return Convert.ToUInt64(this, provider);
    }

    [SecuritySafeCritical]
    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return Convert.ToSingle(this, provider);
    }

    [SecuritySafeCritical]
    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return Convert.ToDouble(this, provider);
    }

    [SecuritySafeCritical]
    Decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return Convert.ToDecimal(this, provider);
    }

    [SecuritySafeCritical]
    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return Convert.ToDateTime(this, provider);
    }

    object IConvertible.ToType(Type type, IFormatProvider provider)
    {
      return Convert.DefaultToType((IConvertible) this, type, provider);
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool IsFastSort();

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool IsAscii();

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetTrailByte(byte data);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool TryGetTrailByte(out byte data);

    public CharEnumerator GetEnumerator()
    {
      return new CharEnumerator(this);
    }

    IEnumerator<char> IEnumerable<char>.GetEnumerator()
    {
      return (IEnumerator<char>) new CharEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new CharEnumerator(this);
    }

    [SecurityCritical]
    [ForceTokenStabilization]
    internal static unsafe void InternalCopy(string src, IntPtr dest, int len)
    {
      if (len == 0)
        return;
      fixed (char* chPtr = &src.m_firstChar)
        Buffer.memcpyimpl((byte*) chPtr, (byte*) dest.ToPointer(), len);
    }
  }
}
