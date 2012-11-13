// Type: System.Windows.Data.Binding
// Assembly: PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\WPF\PresentationFramework.dll

using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;

namespace System.Windows.Data
{
  public class Binding : BindingBase
  {
    public static readonly RoutedEvent SourceUpdatedEvent = EventManager.RegisterRoutedEvent("SourceUpdated", RoutingStrategy.Bubble, typeof (EventHandler<DataTransferEventArgs>), typeof (Binding));
    public static readonly RoutedEvent TargetUpdatedEvent = EventManager.RegisterRoutedEvent("TargetUpdated", RoutingStrategy.Bubble, typeof (EventHandler<DataTransferEventArgs>), typeof (Binding));
    public static readonly DependencyProperty XmlNamespaceManagerProperty = DependencyProperty.RegisterAttached("XmlNamespaceManager", typeof (object), typeof (Binding), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Binding.IsValidXmlNamespaceManager));
    public static readonly object DoNothing = (object) new NamedObject("Binding.DoNothing");
    private static readonly ObjectRef UnsetSource = (ObjectRef) new ExplicitObjectRef((object) null);
    private static readonly ObjectRef StaticSourceRef = (ObjectRef) new ExplicitObjectRef(BindingExpression.StaticSource);
    private ObjectRef _source = Binding.UnsetSource;
    public const string IndexerName = "Item[]";
    private Binding.SourceProperties _sourceInUse;
    private PropertyPath _ppath;
    private bool _isAsync;
    private bool _bindsDirectlyToSource;
    private bool _doesNotTransferDefaultValue;
    private int _attachedPropertiesInPath;

    public Collection<ValidationRule> ValidationRules
    {
      get
      {
        if (!this.HasValue(BindingBase.Feature.ValidationRules))
          this.SetValue(BindingBase.Feature.ValidationRules, (object) new ValidationRuleCollection());
        return (Collection<ValidationRule>) this.GetValue(BindingBase.Feature.ValidationRules, (object) null);
      }
    }

    [DefaultValue(false)]
    public bool ValidatesOnExceptions
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.ValidatesOnExceptions, value);
      }
    }

    [DefaultValue(false)]
    public bool ValidatesOnDataErrors
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.ValidatesOnDataErrors, value);
      }
    }

    [DefaultValue(true)]
    public bool ValidatesOnNotifyDataErrors
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors, value);
      }
    }

    public PropertyPath Path
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._ppath;
      }
      set
      {
        this.CheckSealed();
        this._ppath = value;
        this._attachedPropertiesInPath = -1;
        this.ClearFlag(BindingBase.BindingFlags.PathGeneratedInternally);
        if (this._ppath == null || !this._ppath.StartsWithStaticProperty)
          return;
        if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.StaticSource)
          this.SourceReference = Binding.StaticSourceRef;
        else
          throw new InvalidOperationException(System.Windows.SR.Get("BindingConflict", (object) Binding.SourceProperties.StaticSource, (object) this._sourceInUse));
      }
    }

    [DefaultValue(null)]
    public string XPath
    {
      get
      {
        return (string) this.GetValue(BindingBase.Feature.XPath, (object) null);
      }
      set
      {
        this.CheckSealed();
        this.SetValue(BindingBase.Feature.XPath, (object) value, (object) null);
      }
    }

    [DefaultValue(BindingMode.Default)]
    public BindingMode Mode
    {
      get
      {
        switch (this.GetFlagsWithinMask(BindingBase.BindingFlags.OneWay | BindingBase.BindingFlags.OneWayToSource | BindingBase.BindingFlags.PropDefault))
        {
          case BindingBase.BindingFlags.OneTime:
            return BindingMode.OneTime;
          case BindingBase.BindingFlags.OneWay:
            return BindingMode.OneWay;
          case BindingBase.BindingFlags.OneWayToSource:
            return BindingMode.OneWayToSource;
          case BindingBase.BindingFlags.OneWay | BindingBase.BindingFlags.OneWayToSource:
            return BindingMode.TwoWay;
          case BindingBase.BindingFlags.PropDefault:
            return BindingMode.Default;
          default:
            Invariant.Assert(false, "Unexpected BindingMode value");
            return BindingMode.TwoWay;
        }
      }
      set
      {
        this.CheckSealed();
        BindingBase.BindingFlags flags = BindingBase.FlagsFrom(value);
        if (flags == BindingBase.BindingFlags.IllegalInput)
          throw new InvalidEnumArgumentException("value", (int) value, typeof (BindingMode));
        this.ChangeFlagsWithinMask(BindingBase.BindingFlags.OneWay | BindingBase.BindingFlags.OneWayToSource | BindingBase.BindingFlags.PropDefault, flags);
      }
    }

    [DefaultValue(UpdateSourceTrigger.Default)]
    public UpdateSourceTrigger UpdateSourceTrigger
    {
      get
      {
        switch (this.GetFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault))
        {
          case BindingBase.BindingFlags.UpdateExplicitly:
            return UpdateSourceTrigger.Explicit;
          case BindingBase.BindingFlags.UpdateDefault:
            return UpdateSourceTrigger.Default;
          case BindingBase.BindingFlags.OneTime:
            return UpdateSourceTrigger.PropertyChanged;
          case BindingBase.BindingFlags.UpdateOnLostFocus:
            return UpdateSourceTrigger.LostFocus;
          default:
            Invariant.Assert(false, "Unexpected UpdateSourceTrigger value");
            return UpdateSourceTrigger.Default;
        }
      }
      set
      {
        this.CheckSealed();
        BindingBase.BindingFlags flags = BindingBase.FlagsFrom(value);
        if (flags == BindingBase.BindingFlags.IllegalInput)
          throw new InvalidEnumArgumentException("value", (int) value, typeof (UpdateSourceTrigger));
        this.ChangeFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault, flags);
      }
    }

    [DefaultValue(false)]
    public bool NotifyOnSourceUpdated
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated, value);
      }
    }

    [DefaultValue(false)]
    public bool NotifyOnTargetUpdated
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated, value);
      }
    }

    [DefaultValue(false)]
    public bool NotifyOnValidationError
    {
      get
      {
        return this.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
      }
      set
      {
        if (this.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError) == value)
          return;
        this.CheckSealed();
        this.ChangeFlag(BindingBase.BindingFlags.NotifyOnValidationError, value);
      }
    }

    [DefaultValue(null)]
    public IValueConverter Converter
    {
      get
      {
        return (IValueConverter) this.GetValue(BindingBase.Feature.Converter, (object) null);
      }
      set
      {
        this.CheckSealed();
        this.SetValue(BindingBase.Feature.Converter, (object) value, (object) null);
      }
    }

    [DefaultValue(null)]
    public object ConverterParameter
    {
      get
      {
        return this.GetValue(BindingBase.Feature.ConverterParameter, (object) null);
      }
      set
      {
        this.CheckSealed();
        this.SetValue(BindingBase.Feature.ConverterParameter, value, (object) null);
      }
    }

    [TypeConverter(typeof (CultureInfoIetfLanguageTagConverter))]
    [DefaultValue(null)]
    public CultureInfo ConverterCulture
    {
      get
      {
        return (CultureInfo) this.GetValue(BindingBase.Feature.Culture, (object) null);
      }
      set
      {
        this.CheckSealed();
        this.SetValue(BindingBase.Feature.Culture, (object) value, (object) null);
      }
    }

    public object Source
    {
      get
      {
        WeakReference<object> weakReference = (WeakReference<object>) this.GetValue(BindingBase.Feature.ObjectSource, (object) null);
        if (weakReference == null)
          return (object) null;
        object target;
        if (!weakReference.TryGetTarget(out target))
          return (object) null;
        else
          return target;
      }
      set
      {
        this.CheckSealed();
        if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.Source)
        {
          if (value != DependencyProperty.UnsetValue)
          {
            this.SetValue(BindingBase.Feature.ObjectSource, (object) new WeakReference<object>(value));
            this.SourceReference = (ObjectRef) new ExplicitObjectRef(value);
          }
          else
          {
            this.ClearValue(BindingBase.Feature.ObjectSource);
            this.SourceReference = (ObjectRef) null;
          }
        }
        else
          throw new InvalidOperationException(System.Windows.SR.Get("BindingConflict", (object) Binding.SourceProperties.Source, (object) this._sourceInUse));
      }
    }

    [DefaultValue(null)]
    public RelativeSource RelativeSource
    {
      get
      {
        return (RelativeSource) this.GetValue(BindingBase.Feature.RelativeSource, (object) null);
      }
      set
      {
        this.CheckSealed();
        if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.RelativeSource)
        {
          this.SetValue(BindingBase.Feature.RelativeSource, (object) value, (object) null);
          this.SourceReference = value != null ? (ObjectRef) new RelativeObjectRef(value) : (ObjectRef) null;
        }
        else
          throw new InvalidOperationException(System.Windows.SR.Get("BindingConflict", (object) Binding.SourceProperties.RelativeSource, (object) this._sourceInUse));
      }
    }

    [DefaultValue(null)]
    public string ElementName
    {
      get
      {
        return (string) this.GetValue(BindingBase.Feature.ElementSource, (object) null);
      }
      set
      {
        this.CheckSealed();
        if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.ElementName)
        {
          this.SetValue(BindingBase.Feature.ElementSource, (object) value, (object) null);
          this.SourceReference = value != null ? (ObjectRef) new ElementObjectRef(value) : (ObjectRef) null;
        }
        else
          throw new InvalidOperationException(System.Windows.SR.Get("BindingConflict", (object) Binding.SourceProperties.ElementName, (object) this._sourceInUse));
      }
    }

    [DefaultValue(false)]
    public bool IsAsync
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._isAsync;
      }
      set
      {
        this.CheckSealed();
        this._isAsync = value;
      }
    }

    [DefaultValue(null)]
    public object AsyncState
    {
      get
      {
        return this.GetValue(BindingBase.Feature.AsyncState, (object) null);
      }
      set
      {
        this.CheckSealed();
        this.SetValue(BindingBase.Feature.AsyncState, value, (object) null);
      }
    }

    [DefaultValue(false)]
    public bool BindsDirectlyToSource
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._bindsDirectlyToSource;
      }
      set
      {
        this.CheckSealed();
        this._bindsDirectlyToSource = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
    {
      get
      {
        return (UpdateSourceExceptionFilterCallback) this.GetValue(BindingBase.Feature.ExceptionFilterCallback, (object) null);
      }
      set
      {
        this.SetValue(BindingBase.Feature.ExceptionFilterCallback, (object) value, (object) null);
      }
    }

    internal override CultureInfo ConverterCultureInternal
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.ConverterCulture;
      }
    }

    internal ObjectRef SourceReference
    {
      get
      {
        if (this._source != Binding.UnsetSource)
          return this._source;
        else
          return (ObjectRef) null;
      }
      set
      {
        this.CheckSealed();
        this._source = value;
        this.DetermineSource();
      }
    }

    internal bool TreeContextIsRequired
    {
      get
      {
        if (this._attachedPropertiesInPath < 0)
          this._attachedPropertiesInPath = this._ppath == null ? 0 : this._ppath.ComputeUnresolvedAttachedPropertiesInPath();
        bool flag = this._attachedPropertiesInPath > 0;
        if (!flag && this.HasValue(BindingBase.Feature.XPath) && this.XPath.IndexOf(':') >= 0)
          flag = true;
        return flag;
      }
    }

    internal override Collection<ValidationRule> ValidationRulesInternal
    {
      get
      {
        return (Collection<ValidationRule>) this.GetValue(BindingBase.Feature.ValidationRules, (object) null);
      }
    }

    internal bool TransfersDefaultValue
    {
      get
      {
        return !this._doesNotTransferDefaultValue;
      }
      set
      {
        this.CheckSealed();
        this._doesNotTransferDefaultValue = !value;
      }
    }

    internal override bool ValidatesOnNotifyDataErrorsInternal
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.ValidatesOnNotifyDataErrors;
      }
    }

    static Binding()
    {
    }

    public Binding()
    {
    }

    public Binding(string path)
    {
      if (path == null)
        return;
      if (Dispatcher.CurrentDispatcher == null)
        throw new InvalidOperationException();
      this.Path = new PropertyPath(path, new object[0]);
    }

    public static void AddSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
    {
      UIElement.AddHandler(element, Binding.SourceUpdatedEvent, (Delegate) handler);
    }

    public static void RemoveSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
    {
      UIElement.RemoveHandler(element, Binding.SourceUpdatedEvent, (Delegate) handler);
    }

    public static void AddTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
    {
      UIElement.AddHandler(element, Binding.TargetUpdatedEvent, (Delegate) handler);
    }

    public static void RemoveTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
    {
      UIElement.RemoveHandler(element, Binding.TargetUpdatedEvent, (Delegate) handler);
    }

    public static XmlNamespaceManager GetXmlNamespaceManager(DependencyObject target)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      else
        return (XmlNamespaceManager) target.GetValue(Binding.XmlNamespaceManagerProperty);
    }

    public static void SetXmlNamespaceManager(DependencyObject target, XmlNamespaceManager value)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      target.SetValue(Binding.XmlNamespaceManagerProperty, (object) value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeValidationRules()
    {
      if (this.HasValue(BindingBase.Feature.ValidationRules))
        return this.ValidationRules.Count > 0;
      else
        return false;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializePath()
    {
      if (this._ppath != null)
        return !this.TestFlag(BindingBase.BindingFlags.PathGeneratedInternally);
      else
        return false;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeSource()
    {
      return false;
    }

    internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
    {
      return (BindingExpressionBase) BindingExpression.CreateBindingExpression(target, dp, this, owner);
    }

    internal override ValidationRule LookupValidationRule(Type type)
    {
      return BindingBase.LookupValidationRule(type, this.ValidationRulesInternal);
    }

    internal object DoFilterException(object bindExpr, Exception exception)
    {
      UpdateSourceExceptionFilterCallback exceptionFilterCallback = (UpdateSourceExceptionFilterCallback) this.GetValue(BindingBase.Feature.ExceptionFilterCallback, (object) null);
      if (exceptionFilterCallback != null)
        return exceptionFilterCallback(bindExpr, exception);
      else
        return (object) exception;
    }

    internal void UsePath(PropertyPath path)
    {
      this._ppath = path;
      this.SetFlag(BindingBase.BindingFlags.PathGeneratedInternally);
    }

    internal override BindingBase CreateClone()
    {
      return (BindingBase) new Binding();
    }

    internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
    {
      Binding binding = (Binding) baseClone;
      binding._ppath = this._ppath;
      this.CopyValue(BindingBase.Feature.XPath, (BindingBase) binding);
      binding._source = this._source;
      this.CopyValue(BindingBase.Feature.Culture, (BindingBase) binding);
      binding._isAsync = this._isAsync;
      this.CopyValue(BindingBase.Feature.AsyncState, (BindingBase) binding);
      binding._bindsDirectlyToSource = this._bindsDirectlyToSource;
      binding._doesNotTransferDefaultValue = this._doesNotTransferDefaultValue;
      this.CopyValue(BindingBase.Feature.ObjectSource, (BindingBase) binding);
      this.CopyValue(BindingBase.Feature.RelativeSource, (BindingBase) binding);
      this.CopyValue(BindingBase.Feature.Converter, (BindingBase) binding);
      this.CopyValue(BindingBase.Feature.ConverterParameter, (BindingBase) binding);
      binding._attachedPropertiesInPath = this._attachedPropertiesInPath;
      this.CopyValue(BindingBase.Feature.ValidationRules, (BindingBase) binding);
      base.InitializeClone(baseClone, mode);
    }

    private static bool IsValidXmlNamespaceManager(object value)
    {
      if (value != null)
        return SystemXmlHelper.IsXmlNamespaceManager(value);
      else
        return true;
    }

    private void DetermineSource()
    {
      this._sourceInUse = this._source == Binding.UnsetSource ? Binding.SourceProperties.None : (this.HasValue(BindingBase.Feature.RelativeSource) ? Binding.SourceProperties.RelativeSource : (this.HasValue(BindingBase.Feature.ElementSource) ? Binding.SourceProperties.ElementName : (this.HasValue(BindingBase.Feature.ObjectSource) ? Binding.SourceProperties.Source : (this._source == Binding.StaticSourceRef ? Binding.SourceProperties.StaticSource : Binding.SourceProperties.InternalSource))));
    }

    private enum SourceProperties : byte
    {
      None,
      RelativeSource,
      ElementName,
      Source,
      StaticSource,
      InternalSource,
    }
  }
}
