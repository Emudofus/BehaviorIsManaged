// Type: System.ComponentModel.TypeConverter
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
  [ComVisible(true)]
  [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
  public class TypeConverter
  {
    private static volatile bool useCompatibleTypeConversion = false;
    private static volatile bool firstLoadAppSetting = true;
    private static object loadAppSettingLock = new object();
    private const string s_UseCompatibleTypeConverterBehavior = "UseCompatibleTypeConverterBehavior";

    static bool UseCompatibleTypeConversion
    {
      private get
      {
        if (TypeConverter.firstLoadAppSetting)
        {
          lock (TypeConverter.loadAppSettingLock)
          {
            if (TypeConverter.firstLoadAppSetting)
            {
              string local_0 = ConfigurationManager.AppSettings["UseCompatibleTypeConverterBehavior"];
              try
              {
                if (!string.IsNullOrEmpty(local_0))
                  TypeConverter.useCompatibleTypeConversion = bool.Parse(local_0.Trim());
              }
              catch
              {
                TypeConverter.useCompatibleTypeConversion = false;
              }
              TypeConverter.firstLoadAppSetting = false;
            }
          }
        }
        return TypeConverter.useCompatibleTypeConversion;
      }
    }

    static TypeConverter()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public TypeConverter()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool CanConvertFrom(Type sourceType)
    {
      return this.CanConvertFrom((ITypeDescriptorContext) null, sourceType);
    }

    public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if (sourceType == typeof (InstanceDescriptor))
        return true;
      else
        return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool CanConvertTo(Type destinationType)
    {
      return this.CanConvertTo((ITypeDescriptorContext) null, destinationType);
    }

    public virtual bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      return destinationType == typeof (string);
    }

    public object ConvertFrom(object value)
    {
      return this.ConvertFrom((ITypeDescriptorContext) null, CultureInfo.CurrentCulture, value);
    }

    public virtual object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      InstanceDescriptor instanceDescriptor = value as InstanceDescriptor;
      if (instanceDescriptor != null)
        return instanceDescriptor.Invoke();
      else
        throw this.GetConvertFromException(value);
    }

    public object ConvertFromInvariantString(string text)
    {
      return this.ConvertFromString((ITypeDescriptorContext) null, CultureInfo.InvariantCulture, text);
    }

    public object ConvertFromInvariantString(ITypeDescriptorContext context, string text)
    {
      return this.ConvertFromString(context, CultureInfo.InvariantCulture, text);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object ConvertFromString(string text)
    {
      return this.ConvertFrom((ITypeDescriptorContext) null, (CultureInfo) null, (object) text);
    }

    public object ConvertFromString(ITypeDescriptorContext context, string text)
    {
      return this.ConvertFrom(context, CultureInfo.CurrentCulture, (object) text);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object ConvertFromString(ITypeDescriptorContext context, CultureInfo culture, string text)
    {
      return this.ConvertFrom(context, culture, (object) text);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object ConvertTo(object value, Type destinationType)
    {
      return this.ConvertTo((ITypeDescriptorContext) null, (CultureInfo) null, value, destinationType);
    }

    public virtual object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == (Type) null)
        throw new ArgumentNullException("destinationType");
      if (!(destinationType == typeof (string)))
        throw this.GetConvertToException(value, destinationType);
      if (value == null)
        return (object) string.Empty;
      if (culture != null && culture != CultureInfo.CurrentCulture)
      {
        IFormattable formattable = value as IFormattable;
        if (formattable != null)
          return (object) formattable.ToString((string) null, (IFormatProvider) culture);
      }
      return (object) value.ToString();
    }

    public string ConvertToInvariantString(object value)
    {
      return this.ConvertToString((ITypeDescriptorContext) null, CultureInfo.InvariantCulture, value);
    }

    public string ConvertToInvariantString(ITypeDescriptorContext context, object value)
    {
      return this.ConvertToString(context, CultureInfo.InvariantCulture, value);
    }

    public string ConvertToString(object value)
    {
      return (string) this.ConvertTo((ITypeDescriptorContext) null, CultureInfo.CurrentCulture, value, typeof (string));
    }

    public string ConvertToString(ITypeDescriptorContext context, object value)
    {
      return (string) this.ConvertTo(context, CultureInfo.CurrentCulture, value, typeof (string));
    }

    public string ConvertToString(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      return (string) this.ConvertTo(context, culture, value, typeof (string));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object CreateInstance(IDictionary propertyValues)
    {
      return this.CreateInstance((ITypeDescriptorContext) null, propertyValues);
    }

    public virtual object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    {
      return (object) null;
    }

    protected Exception GetConvertFromException(object value)
    {
      throw new NotSupportedException(System.SR.GetString("ConvertFromException", (object) this.GetType().Name, (object) (value != null ? value.GetType().FullName : System.SR.GetString("ToStringNull"))));
    }

    protected Exception GetConvertToException(object value, Type destinationType)
    {
      throw new NotSupportedException(System.SR.GetString("ConvertToException", (object) this.GetType().Name, (object) (value != null ? value.GetType().FullName : System.SR.GetString("ToStringNull")), (object) destinationType.FullName));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool GetCreateInstanceSupported()
    {
      return this.GetCreateInstanceSupported((ITypeDescriptorContext) null);
    }

    public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public PropertyDescriptorCollection GetProperties(object value)
    {
      return this.GetProperties((ITypeDescriptorContext) null, value);
    }

    public PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value)
    {
      return this.GetProperties(context, value, new Attribute[1]
      {
        (Attribute) BrowsableAttribute.Yes
      });
    }

    public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      return (PropertyDescriptorCollection) null;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool GetPropertiesSupported()
    {
      return this.GetPropertiesSupported((ITypeDescriptorContext) null);
    }

    public virtual bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ICollection GetStandardValues()
    {
      return (ICollection) this.GetStandardValues((ITypeDescriptorContext) null);
    }

    public virtual TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return (TypeConverter.StandardValuesCollection) null;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool GetStandardValuesExclusive()
    {
      return this.GetStandardValuesExclusive((ITypeDescriptorContext) null);
    }

    public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool GetStandardValuesSupported()
    {
      return this.GetStandardValuesSupported((ITypeDescriptorContext) null);
    }

    public virtual bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool IsValid(object value)
    {
      return this.IsValid((ITypeDescriptorContext) null, value);
    }

    public virtual bool IsValid(ITypeDescriptorContext context, object value)
    {
      if (TypeConverter.UseCompatibleTypeConversion)
        return true;
      bool flag = true;
      try
      {
        if (value == null || this.CanConvertFrom(context, value.GetType()))
          this.ConvertFrom(context, CultureInfo.InvariantCulture, value);
        else
          flag = false;
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names)
    {
      props.Sort(names);
      return props;
    }

    protected abstract class SimplePropertyDescriptor : PropertyDescriptor
    {
      private Type componentType;
      private Type propertyType;

      public override Type ComponentType
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.componentType;
        }
      }

      public override bool IsReadOnly
      {
        get
        {
          return this.Attributes.Contains((Attribute) ReadOnlyAttribute.Yes);
        }
      }

      public override Type PropertyType
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.propertyType;
        }
      }

      protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType)
        : this(componentType, name, propertyType, new Attribute[0])
      {
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType, Attribute[] attributes)
        : base(name, attributes)
      {
        this.componentType = componentType;
        this.propertyType = propertyType;
      }

      public override bool CanResetValue(object component)
      {
        DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute) this.Attributes[typeof (DefaultValueAttribute)];
        if (defaultValueAttribute == null)
          return false;
        else
          return defaultValueAttribute.Value.Equals(this.GetValue(component));
      }

      public override void ResetValue(object component)
      {
        DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute) this.Attributes[typeof (DefaultValueAttribute)];
        if (defaultValueAttribute == null)
          return;
        this.SetValue(component, defaultValueAttribute.Value);
      }

      public override bool ShouldSerializeValue(object component)
      {
        return false;
      }
    }

    public class StandardValuesCollection : ICollection, IEnumerable
    {
      private ICollection values;
      private Array valueArray;

      public int Count
      {
        get
        {
          if (this.valueArray != null)
            return this.valueArray.Length;
          else
            return this.values.Count;
        }
      }

      public object this[int index]
      {
        get
        {
          if (this.valueArray != null)
            return this.valueArray.GetValue(index);
          IList list = this.values as IList;
          if (list != null)
            return list[index];
          this.valueArray = (Array) new object[this.values.Count];
          this.values.CopyTo(this.valueArray, 0);
          return this.valueArray.GetValue(index);
        }
      }

      int ICollection.Count
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.Count;
        }
      }

      bool ICollection.IsSynchronized
      {
        get
        {
          return false;
        }
      }

      object ICollection.SyncRoot
      {
        get
        {
          return (object) null;
        }
      }

      public StandardValuesCollection(ICollection values)
      {
        if (values == null)
          values = (ICollection) new object[0];
        Array array = values as Array;
        if (array != null)
          this.valueArray = array;
        this.values = values;
      }

      public void CopyTo(Array array, int index)
      {
        this.values.CopyTo(array, index);
      }

      public IEnumerator GetEnumerator()
      {
        return this.values.GetEnumerator();
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      void ICollection.CopyTo(Array array, int index)
      {
        this.CopyTo(array, index);
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      IEnumerator IEnumerable.GetEnumerator()
      {
        return this.GetEnumerator();
      }
    }
  }
}
