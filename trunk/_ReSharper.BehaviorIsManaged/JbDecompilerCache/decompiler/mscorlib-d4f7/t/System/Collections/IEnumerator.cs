// Type: System.Collections.IEnumerator
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Runtime.InteropServices;

namespace System.Collections
{
  [ComVisible(true)]
  [Guid("496B0ABF-CDEE-11d3-88E8-00902754C43A")]
  [__DynamicallyInvokable]
  public interface IEnumerator
  {
    [__DynamicallyInvokable]
    object Current { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    bool MoveNext();

    [__DynamicallyInvokable]
    void Reset();
  }
}
