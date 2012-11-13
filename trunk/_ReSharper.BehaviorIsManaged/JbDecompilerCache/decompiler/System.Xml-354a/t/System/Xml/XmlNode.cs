// Type: System.Xml.XmlNode
// Assembly: System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Xml.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  [DebuggerDisplay("{debuggerDisplayProxy}")]
  public abstract class XmlNode : ICloneable, IEnumerable, IXPathNavigable
  {
    internal XmlNode parentNode;

    public abstract string Name { get; }

    public virtual string Value
    {
      get
      {
        return (string) null;
      }
      set
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, Res.GetString("Xdom_Node_SetVal"), new object[1]
        {
          (object) ((object) this.NodeType).ToString()
        }));
      }
    }

    public abstract XmlNodeType NodeType { get; }

    public virtual XmlNode ParentNode
    {
      get
      {
        if (this.parentNode.NodeType != XmlNodeType.Document)
          return this.parentNode;
        XmlLinkedNode xmlLinkedNode1 = this.parentNode.FirstChild as XmlLinkedNode;
        if (xmlLinkedNode1 != null)
        {
          XmlLinkedNode xmlLinkedNode2 = xmlLinkedNode1;
          while (xmlLinkedNode2 != this)
          {
            xmlLinkedNode2 = xmlLinkedNode2.next;
            if (xmlLinkedNode2 == null || xmlLinkedNode2 == xmlLinkedNode1)
              goto label_7;
          }
          return this.parentNode;
        }
label_7:
        return (XmlNode) null;
      }
    }

    public virtual XmlNodeList ChildNodes
    {
      get
      {
        return (XmlNodeList) new XmlChildNodes(this);
      }
    }

    public virtual XmlNode PreviousSibling
    {
      get
      {
        return (XmlNode) null;
      }
    }

    public virtual XmlNode NextSibling
    {
      get
      {
        return (XmlNode) null;
      }
    }

    public virtual XmlAttributeCollection Attributes
    {
      get
      {
        return (XmlAttributeCollection) null;
      }
    }

    public virtual XmlDocument OwnerDocument
    {
      get
      {
        if (this.parentNode.NodeType == XmlNodeType.Document)
          return (XmlDocument) this.parentNode;
        else
          return this.parentNode.OwnerDocument;
      }
    }

    public virtual XmlNode FirstChild
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        XmlLinkedNode lastNode = this.LastNode;
        if (lastNode != null)
          return (XmlNode) lastNode.next;
        else
          return (XmlNode) null;
      }
    }

    public virtual XmlNode LastChild
    {
      get
      {
        return (XmlNode) this.LastNode;
      }
    }

    internal virtual bool IsContainer
    {
      get
      {
        return false;
      }
    }

    internal virtual XmlLinkedNode LastNode
    {
      get
      {
        return (XmlLinkedNode) null;
      }
      set
      {
      }
    }

    public virtual bool HasChildNodes
    {
      get
      {
        return this.LastNode != null;
      }
    }

    public virtual string NamespaceURI
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual string Prefix
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    public abstract string LocalName { get; }

    public virtual bool IsReadOnly
    {
      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")] get
      {
        XmlDocument ownerDocument = this.OwnerDocument;
        return XmlNode.HasReadOnlyParent(this);
      }
    }

    public virtual string InnerText
    {
      get
      {
        XmlNode firstChild = this.FirstChild;
        if (firstChild == null)
          return string.Empty;
        if (firstChild.NextSibling == null)
        {
          switch (firstChild.NodeType)
          {
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
            case XmlNodeType.Whitespace:
            case XmlNodeType.SignificantWhitespace:
              return firstChild.Value;
          }
        }
        StringBuilder builder = new StringBuilder();
        this.AppendChildText(builder);
        return ((object) builder).ToString();
      }
      set
      {
        XmlNode firstChild = this.FirstChild;
        if (firstChild != null && firstChild.NextSibling == null && firstChild.NodeType == XmlNodeType.Text)
        {
          firstChild.Value = value;
        }
        else
        {
          this.RemoveAll();
          this.AppendChild((XmlNode) this.OwnerDocument.CreateTextNode(value));
        }
      }
    }

    public virtual string OuterXml
    {
      get
      {
        StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        XmlDOMTextWriter xmlDomTextWriter = new XmlDOMTextWriter((TextWriter) stringWriter);
        try
        {
          this.WriteTo((XmlWriter) xmlDomTextWriter);
        }
        finally
        {
          xmlDomTextWriter.Close();
        }
        return stringWriter.ToString();
      }
    }

    public virtual string InnerXml
    {
      get
      {
        StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
        XmlDOMTextWriter xmlDomTextWriter = new XmlDOMTextWriter((TextWriter) stringWriter);
        try
        {
          this.WriteContentTo((XmlWriter) xmlDomTextWriter);
        }
        finally
        {
          xmlDomTextWriter.Close();
        }
        return stringWriter.ToString();
      }
      set
      {
        throw new InvalidOperationException(Res.GetString("Xdom_Set_InnerXml"));
      }
    }

    public virtual IXmlSchemaInfo SchemaInfo
    {
      get
      {
        return XmlDocument.NotKnownSchemaInfo;
      }
    }

    public virtual string BaseURI
    {
      get
      {
        for (XmlNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
        {
          switch (parentNode.NodeType)
          {
            case XmlNodeType.EntityReference:
              return ((XmlEntityReference) parentNode).ChildBaseURI;
            case XmlNodeType.Document:
            case XmlNodeType.Entity:
            case XmlNodeType.Attribute:
              return parentNode.BaseURI;
            default:
              goto default;
          }
        }
        return string.Empty;
      }
    }

    internal XmlDocument Document
    {
      get
      {
        if (this.NodeType == XmlNodeType.Document)
          return (XmlDocument) this;
        else
          return this.OwnerDocument;
      }
    }

    public virtual XmlElement this[string name]
    {
      get
      {
        for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == name)
            return (XmlElement) xmlNode;
        }
        return (XmlElement) null;
      }
    }

    public virtual XmlElement this[string localname, string ns]
    {
      get
      {
        for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.LocalName == localname && xmlNode.NamespaceURI == ns)
            return (XmlElement) xmlNode;
        }
        return (XmlElement) null;
      }
    }

    internal virtual XmlSpace XmlSpace
    {
      get
      {
        XmlNode xmlNode = this;
        do
        {
          XmlElement xmlElement = xmlNode as XmlElement;
          if (xmlElement != null && xmlElement.HasAttribute("xml:space"))
          {
            switch (XmlConvert.TrimString(xmlElement.GetAttribute("xml:space")))
            {
              case "default":
                return XmlSpace.Default;
              case "preserve":
                return XmlSpace.Preserve;
            }
          }
          xmlNode = xmlNode.ParentNode;
        }
        while (xmlNode != null);
        return XmlSpace.None;
      }
    }

    internal virtual string XmlLang
    {
      get
      {
        XmlNode xmlNode = this;
        do
        {
          XmlElement xmlElement = xmlNode as XmlElement;
          if (xmlElement != null && xmlElement.HasAttribute("xml:lang"))
            return xmlElement.GetAttribute("xml:lang");
          xmlNode = xmlNode.ParentNode;
        }
        while (xmlNode != null);
        return string.Empty;
      }
    }

    internal virtual XPathNodeType XPNodeType
    {
      get
      {
        return (XPathNodeType) -1;
      }
    }

    internal virtual string XPLocalName
    {
      get
      {
        return string.Empty;
      }
    }

    internal virtual bool IsText
    {
      get
      {
        return false;
      }
    }

    internal virtual XmlNode PreviousText
    {
      get
      {
        return (XmlNode) null;
      }
    }

    object debuggerDisplayProxy
    {
      private get
      {
        return (object) new DebuggerDisplayXmlNodeProxy(this);
      }
    }

    internal XmlNode()
    {
    }

    internal XmlNode(XmlDocument doc)
    {
      if (doc == null)
        throw new ArgumentException(Res.GetString("Xdom_Node_Null_Doc"));
      this.parentNode = (XmlNode) doc;
    }

    public virtual XPathNavigator CreateNavigator()
    {
      XmlDocument xmlDocument = this as XmlDocument;
      if (xmlDocument != null)
        return xmlDocument.CreateNavigator(this);
      else
        return this.OwnerDocument.CreateNavigator(this);
    }

    public XmlNode SelectSingleNode(string xpath)
    {
      XmlNodeList xmlNodeList = this.SelectNodes(xpath);
      if (xmlNodeList == null)
        return (XmlNode) null;
      else
        return xmlNodeList[0];
    }

    public XmlNode SelectSingleNode(string xpath, XmlNamespaceManager nsmgr)
    {
      XPathNavigator navigator = this.CreateNavigator();
      if (navigator == null)
        return (XmlNode) null;
      XPathExpression expr = navigator.Compile(xpath);
      expr.SetContext(nsmgr);
      return new XPathNodeList(navigator.Select(expr))[0];
    }

    public XmlNodeList SelectNodes(string xpath)
    {
      XPathNavigator navigator = this.CreateNavigator();
      if (navigator == null)
        return (XmlNodeList) null;
      else
        return (XmlNodeList) new XPathNodeList(navigator.Select(xpath));
    }

    public XmlNodeList SelectNodes(string xpath, XmlNamespaceManager nsmgr)
    {
      XPathNavigator navigator = this.CreateNavigator();
      if (navigator == null)
        return (XmlNodeList) null;
      XPathExpression expr = navigator.Compile(xpath);
      expr.SetContext(nsmgr);
      return (XmlNodeList) new XPathNodeList(navigator.Select(expr));
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    internal bool AncestorNode(XmlNode node)
    {
      for (XmlNode parentNode = this.ParentNode; parentNode != null && parentNode != this; parentNode = parentNode.ParentNode)
      {
        if (parentNode == node)
          return true;
      }
      return false;
    }

    internal bool IsConnected()
    {
      XmlNode parentNode = this.ParentNode;
      while (parentNode != null && parentNode.NodeType != XmlNodeType.Document)
        parentNode = parentNode.ParentNode;
      return parentNode != null;
    }

    public virtual XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
    {
      if (this == newChild || this.AncestorNode(newChild))
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Child"));
      if (refChild == null)
        return this.AppendChild(newChild);
      if (!this.IsContainer)
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Contain"));
      if (refChild.ParentNode != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Path"));
      if (newChild == refChild)
        return newChild;
      XmlDocument ownerDocument1 = newChild.OwnerDocument;
      XmlDocument ownerDocument2 = this.OwnerDocument;
      if (ownerDocument1 != null && ownerDocument1 != ownerDocument2 && ownerDocument1 != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Context"));
      if (!this.CanInsertBefore(newChild, refChild))
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Location"));
      if (newChild.ParentNode != null)
        newChild.ParentNode.RemoveChild(newChild);
      if (newChild.NodeType == XmlNodeType.DocumentFragment)
      {
        XmlNode firstChild = newChild.FirstChild;
        XmlNode xmlNode = firstChild;
        if (xmlNode != null)
        {
          newChild.RemoveChild(xmlNode);
          this.InsertBefore(xmlNode, refChild);
          this.InsertAfter(newChild, xmlNode);
        }
        return firstChild;
      }
      else
      {
        if (!(newChild is XmlLinkedNode) || !this.IsValidChildType(newChild.NodeType))
          throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_TypeConflict"));
        XmlLinkedNode xmlLinkedNode1 = (XmlLinkedNode) newChild;
        XmlLinkedNode xmlLinkedNode2 = (XmlLinkedNode) refChild;
        string str = newChild.Value;
        XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(newChild, newChild.ParentNode, this, str, str, XmlNodeChangedAction.Insert);
        if (eventArgs != null)
          this.BeforeEvent(eventArgs);
        if (xmlLinkedNode2 == this.FirstChild)
        {
          xmlLinkedNode1.next = xmlLinkedNode2;
          this.LastNode.next = xmlLinkedNode1;
          xmlLinkedNode1.SetParent(this);
          if (xmlLinkedNode1.IsText && xmlLinkedNode2.IsText)
            XmlNode.NestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode2);
        }
        else
        {
          XmlLinkedNode xmlLinkedNode3 = (XmlLinkedNode) xmlLinkedNode2.PreviousSibling;
          xmlLinkedNode1.next = xmlLinkedNode2;
          xmlLinkedNode3.next = xmlLinkedNode1;
          xmlLinkedNode1.SetParent(this);
          if (xmlLinkedNode3.IsText)
          {
            if (xmlLinkedNode1.IsText)
            {
              XmlNode.NestTextNodes((XmlNode) xmlLinkedNode3, (XmlNode) xmlLinkedNode1);
              if (xmlLinkedNode2.IsText)
                XmlNode.NestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode2);
            }
            else if (xmlLinkedNode2.IsText)
              XmlNode.UnnestTextNodes((XmlNode) xmlLinkedNode3, (XmlNode) xmlLinkedNode2);
          }
          else if (xmlLinkedNode1.IsText && xmlLinkedNode2.IsText)
            XmlNode.NestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode2);
        }
        if (eventArgs != null)
          this.AfterEvent(eventArgs);
        return (XmlNode) xmlLinkedNode1;
      }
    }

    public virtual XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
    {
      if (this == newChild || this.AncestorNode(newChild))
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Child"));
      if (refChild == null)
        return this.PrependChild(newChild);
      if (!this.IsContainer)
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Contain"));
      if (refChild.ParentNode != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Path"));
      if (newChild == refChild)
        return newChild;
      XmlDocument ownerDocument1 = newChild.OwnerDocument;
      XmlDocument ownerDocument2 = this.OwnerDocument;
      if (ownerDocument1 != null && ownerDocument1 != ownerDocument2 && ownerDocument1 != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Context"));
      if (!this.CanInsertAfter(newChild, refChild))
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Location"));
      if (newChild.ParentNode != null)
        newChild.ParentNode.RemoveChild(newChild);
      if (newChild.NodeType == XmlNodeType.DocumentFragment)
      {
        XmlNode refChild1 = refChild;
        XmlNode firstChild = newChild.FirstChild;
        for (XmlNode xmlNode = firstChild; xmlNode != null; {
          XmlNode nextSibling;
          xmlNode = nextSibling;
        }
        )
        {
          nextSibling = xmlNode.NextSibling;
          newChild.RemoveChild(xmlNode);
          this.InsertAfter(xmlNode, refChild1);
          refChild1 = xmlNode;
        }
        return firstChild;
      }
      else
      {
        if (!(newChild is XmlLinkedNode) || !this.IsValidChildType(newChild.NodeType))
          throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_TypeConflict"));
        XmlLinkedNode xmlLinkedNode1 = (XmlLinkedNode) newChild;
        XmlLinkedNode xmlLinkedNode2 = (XmlLinkedNode) refChild;
        string str = newChild.Value;
        XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(newChild, newChild.ParentNode, this, str, str, XmlNodeChangedAction.Insert);
        if (eventArgs != null)
          this.BeforeEvent(eventArgs);
        if (xmlLinkedNode2 == this.LastNode)
        {
          xmlLinkedNode1.next = xmlLinkedNode2.next;
          xmlLinkedNode2.next = xmlLinkedNode1;
          this.LastNode = xmlLinkedNode1;
          xmlLinkedNode1.SetParent(this);
          if (xmlLinkedNode2.IsText && xmlLinkedNode1.IsText)
            XmlNode.NestTextNodes((XmlNode) xmlLinkedNode2, (XmlNode) xmlLinkedNode1);
        }
        else
        {
          XmlLinkedNode xmlLinkedNode3 = xmlLinkedNode2.next;
          xmlLinkedNode1.next = xmlLinkedNode3;
          xmlLinkedNode2.next = xmlLinkedNode1;
          xmlLinkedNode1.SetParent(this);
          if (xmlLinkedNode2.IsText)
          {
            if (xmlLinkedNode1.IsText)
            {
              XmlNode.NestTextNodes((XmlNode) xmlLinkedNode2, (XmlNode) xmlLinkedNode1);
              if (xmlLinkedNode3.IsText)
                XmlNode.NestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode3);
            }
            else if (xmlLinkedNode3.IsText)
              XmlNode.UnnestTextNodes((XmlNode) xmlLinkedNode2, (XmlNode) xmlLinkedNode3);
          }
          else if (xmlLinkedNode1.IsText && xmlLinkedNode3.IsText)
            XmlNode.NestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode3);
        }
        if (eventArgs != null)
          this.AfterEvent(eventArgs);
        return (XmlNode) xmlLinkedNode1;
      }
    }

    public virtual XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
    {
      XmlNode nextSibling = oldChild.NextSibling;
      this.RemoveChild(oldChild);
      this.InsertBefore(newChild, nextSibling);
      return oldChild;
    }

    public virtual XmlNode RemoveChild(XmlNode oldChild)
    {
      if (!this.IsContainer)
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Remove_Contain"));
      if (oldChild.ParentNode != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Remove_Child"));
      XmlLinkedNode xmlLinkedNode1 = (XmlLinkedNode) oldChild;
      string str = xmlLinkedNode1.Value;
      XmlNodeChangedEventArgs eventArgs = this.GetEventArgs((XmlNode) xmlLinkedNode1, this, (XmlNode) null, str, str, XmlNodeChangedAction.Remove);
      if (eventArgs != null)
        this.BeforeEvent(eventArgs);
      XmlLinkedNode lastNode = this.LastNode;
      if (xmlLinkedNode1 == this.FirstChild)
      {
        if (xmlLinkedNode1 == lastNode)
        {
          this.LastNode = (XmlLinkedNode) null;
          xmlLinkedNode1.next = (XmlLinkedNode) null;
          xmlLinkedNode1.SetParent((XmlNode) null);
        }
        else
        {
          XmlLinkedNode xmlLinkedNode2 = xmlLinkedNode1.next;
          if (xmlLinkedNode2.IsText && xmlLinkedNode1.IsText)
            XmlNode.UnnestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode2);
          lastNode.next = xmlLinkedNode2;
          xmlLinkedNode1.next = (XmlLinkedNode) null;
          xmlLinkedNode1.SetParent((XmlNode) null);
        }
      }
      else if (xmlLinkedNode1 == lastNode)
      {
        XmlLinkedNode xmlLinkedNode2 = (XmlLinkedNode) xmlLinkedNode1.PreviousSibling;
        xmlLinkedNode2.next = xmlLinkedNode1.next;
        this.LastNode = xmlLinkedNode2;
        xmlLinkedNode1.next = (XmlLinkedNode) null;
        xmlLinkedNode1.SetParent((XmlNode) null);
      }
      else
      {
        XmlLinkedNode xmlLinkedNode2 = (XmlLinkedNode) xmlLinkedNode1.PreviousSibling;
        XmlLinkedNode xmlLinkedNode3 = xmlLinkedNode1.next;
        if (xmlLinkedNode3.IsText)
        {
          if (xmlLinkedNode2.IsText)
            XmlNode.NestTextNodes((XmlNode) xmlLinkedNode2, (XmlNode) xmlLinkedNode3);
          else if (xmlLinkedNode1.IsText)
            XmlNode.UnnestTextNodes((XmlNode) xmlLinkedNode1, (XmlNode) xmlLinkedNode3);
        }
        xmlLinkedNode2.next = xmlLinkedNode3;
        xmlLinkedNode1.next = (XmlLinkedNode) null;
        xmlLinkedNode1.SetParent((XmlNode) null);
      }
      if (eventArgs != null)
        this.AfterEvent(eventArgs);
      return oldChild;
    }

    public virtual XmlNode PrependChild(XmlNode newChild)
    {
      return this.InsertBefore(newChild, this.FirstChild);
    }

    public virtual XmlNode AppendChild(XmlNode newChild)
    {
      XmlDocument xmlDocument = this.OwnerDocument ?? this as XmlDocument;
      if (!this.IsContainer)
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Contain"));
      if (this == newChild || this.AncestorNode(newChild))
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Child"));
      if (newChild.ParentNode != null)
        newChild.ParentNode.RemoveChild(newChild);
      XmlDocument ownerDocument = newChild.OwnerDocument;
      if (ownerDocument != null && ownerDocument != xmlDocument && ownerDocument != this)
        throw new ArgumentException(Res.GetString("Xdom_Node_Insert_Context"));
      if (newChild.NodeType == XmlNodeType.DocumentFragment)
      {
        XmlNode firstChild = newChild.FirstChild;
        for (XmlNode xmlNode = firstChild; xmlNode != null; {
          XmlNode nextSibling;
          xmlNode = nextSibling;
        }
        )
        {
          nextSibling = xmlNode.NextSibling;
          newChild.RemoveChild(xmlNode);
          this.AppendChild(xmlNode);
        }
        return firstChild;
      }
      else
      {
        if (!(newChild is XmlLinkedNode) || !this.IsValidChildType(newChild.NodeType))
          throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_TypeConflict"));
        if (!this.CanInsertAfter(newChild, this.LastChild))
          throw new InvalidOperationException(Res.GetString("Xdom_Node_Insert_Location"));
        string str = newChild.Value;
        XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(newChild, newChild.ParentNode, this, str, str, XmlNodeChangedAction.Insert);
        if (eventArgs != null)
          this.BeforeEvent(eventArgs);
        XmlLinkedNode lastNode = this.LastNode;
        XmlLinkedNode xmlLinkedNode = (XmlLinkedNode) newChild;
        if (lastNode == null)
        {
          xmlLinkedNode.next = xmlLinkedNode;
          this.LastNode = xmlLinkedNode;
          xmlLinkedNode.SetParent(this);
        }
        else
        {
          xmlLinkedNode.next = lastNode.next;
          lastNode.next = xmlLinkedNode;
          this.LastNode = xmlLinkedNode;
          xmlLinkedNode.SetParent(this);
          if (lastNode.IsText && xmlLinkedNode.IsText)
            XmlNode.NestTextNodes((XmlNode) lastNode, (XmlNode) xmlLinkedNode);
        }
        if (eventArgs != null)
          this.AfterEvent(eventArgs);
        return (XmlNode) xmlLinkedNode;
      }
    }

    internal virtual XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
    {
      XmlNodeChangedEventArgs eventArgsForLoad = doc.GetInsertEventArgsForLoad(newChild, this);
      if (eventArgsForLoad != null)
        doc.BeforeEvent(eventArgsForLoad);
      XmlLinkedNode lastNode = this.LastNode;
      XmlLinkedNode xmlLinkedNode = (XmlLinkedNode) newChild;
      if (lastNode == null)
      {
        xmlLinkedNode.next = xmlLinkedNode;
        this.LastNode = xmlLinkedNode;
        xmlLinkedNode.SetParentForLoad(this);
      }
      else
      {
        xmlLinkedNode.next = lastNode.next;
        lastNode.next = xmlLinkedNode;
        this.LastNode = xmlLinkedNode;
        if (lastNode.IsText && xmlLinkedNode.IsText)
          XmlNode.NestTextNodes((XmlNode) lastNode, (XmlNode) xmlLinkedNode);
        else
          xmlLinkedNode.SetParentForLoad(this);
      }
      if (eventArgsForLoad != null)
        doc.AfterEvent(eventArgsForLoad);
      return (XmlNode) xmlLinkedNode;
    }

    internal virtual bool IsValidChildType(XmlNodeType type)
    {
      return false;
    }

    internal virtual bool CanInsertBefore(XmlNode newChild, XmlNode refChild)
    {
      return true;
    }

    internal virtual bool CanInsertAfter(XmlNode newChild, XmlNode refChild)
    {
      return true;
    }

    public abstract XmlNode CloneNode(bool deep);

    internal virtual void CopyChildren(XmlDocument doc, XmlNode container, bool deep)
    {
      for (XmlNode xmlNode = container.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        this.AppendChildForLoad(xmlNode.CloneNode(deep), doc);
    }

    public virtual void Normalize()
    {
      XmlNode xmlNode1 = (XmlNode) null;
      StringBuilder stringBuilder = new StringBuilder();
      for (XmlNode xmlNode2 = this.FirstChild; xmlNode2 != null; {
        XmlNode nextSibling;
        xmlNode2 = nextSibling;
      }
      )
      {
        nextSibling = xmlNode2.NextSibling;
        switch (xmlNode2.NodeType)
        {
          case XmlNodeType.Element:
            xmlNode2.Normalize();
            goto default;
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            stringBuilder.Append(xmlNode2.Value);
            if (this.NormalizeWinner(xmlNode1, xmlNode2) == xmlNode1)
            {
              this.RemoveChild(xmlNode2);
              break;
            }
            else
            {
              if (xmlNode1 != null)
                this.RemoveChild(xmlNode1);
              xmlNode1 = xmlNode2;
              break;
            }
          default:
            if (xmlNode1 != null)
            {
              xmlNode1.Value = ((object) stringBuilder).ToString();
              xmlNode1 = (XmlNode) null;
            }
            stringBuilder.Remove(0, stringBuilder.Length);
            break;
        }
      }
      if (xmlNode1 == null || stringBuilder.Length <= 0)
        return;
      xmlNode1.Value = ((object) stringBuilder).ToString();
    }

    private XmlNode NormalizeWinner(XmlNode firstNode, XmlNode secondNode)
    {
      if (firstNode == null)
        return secondNode;
      if (firstNode.NodeType == XmlNodeType.Text)
        return firstNode;
      if (secondNode.NodeType == XmlNodeType.Text)
        return secondNode;
      if (firstNode.NodeType == XmlNodeType.SignificantWhitespace)
        return firstNode;
      if (secondNode.NodeType == XmlNodeType.SignificantWhitespace)
        return secondNode;
      if (firstNode.NodeType == XmlNodeType.Whitespace)
        return firstNode;
      if (secondNode.NodeType == XmlNodeType.Whitespace)
        return secondNode;
      else
        return (XmlNode) null;
    }

    public virtual bool Supports(string feature, string version)
    {
      if (string.Compare("XML", feature, StringComparison.OrdinalIgnoreCase) == 0 && (version == null || version == "1.0" || version == "2.0"))
        return true;
      else
        return false;
    }

    internal static bool HasReadOnlyParent(XmlNode n)
    {
      while (n != null)
      {
        switch (n.NodeType)
        {
          case XmlNodeType.Attribute:
            n = (XmlNode) ((XmlAttribute) n).OwnerElement;
            continue;
          case XmlNodeType.EntityReference:
          case XmlNodeType.Entity:
            return true;
          default:
            n = n.ParentNode;
            continue;
        }
      }
      return false;
    }

    public virtual XmlNode Clone()
    {
      return this.CloneNode(true);
    }

    object ICloneable.Clone()
    {
      return (object) this.CloneNode(true);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new XmlChildEnumerator(this);
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new XmlChildEnumerator(this);
    }

    private void AppendChildText(StringBuilder builder)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.FirstChild == null)
        {
          if (xmlNode.NodeType == XmlNodeType.Text || xmlNode.NodeType == XmlNodeType.CDATA || (xmlNode.NodeType == XmlNodeType.Whitespace || xmlNode.NodeType == XmlNodeType.SignificantWhitespace))
            builder.Append(xmlNode.InnerText);
        }
        else
          xmlNode.AppendChildText(builder);
      }
    }

    public abstract void WriteTo(XmlWriter w);

    public abstract void WriteContentTo(XmlWriter w);

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public virtual void RemoveAll()
    {
      XmlNode oldChild = this.FirstChild;
      for (; oldChild != null; {
        XmlNode nextSibling;
        oldChild = nextSibling;
      }
      )
      {
        nextSibling = oldChild.NextSibling;
        this.RemoveChild(oldChild);
      }
    }

    public virtual string GetNamespaceOfPrefix(string prefix)
    {
      return this.GetNamespaceOfPrefixStrict(prefix) ?? string.Empty;
    }

    internal string GetNamespaceOfPrefixStrict(string prefix)
    {
      XmlDocument document = this.Document;
      if (document != null)
      {
        prefix = document.NameTable.Get(prefix);
        if (prefix == null)
          return (string) null;
        XmlNode xmlNode = this;
        while (xmlNode != null)
        {
          if (xmlNode.NodeType == XmlNodeType.Element)
          {
            XmlElement xmlElement = (XmlElement) xmlNode;
            if (xmlElement.HasAttributes)
            {
              XmlAttributeCollection attributes = xmlElement.Attributes;
              if (prefix.Length == 0)
              {
                for (int index = 0; index < attributes.Count; ++index)
                {
                  XmlAttribute xmlAttribute = attributes[index];
                  if (xmlAttribute.Prefix.Length == 0 && Ref.Equal(xmlAttribute.LocalName, document.strXmlns))
                    return xmlAttribute.Value;
                }
              }
              else
              {
                for (int index = 0; index < attributes.Count; ++index)
                {
                  XmlAttribute xmlAttribute = attributes[index];
                  if (Ref.Equal(xmlAttribute.Prefix, document.strXmlns))
                  {
                    if (Ref.Equal(xmlAttribute.LocalName, prefix))
                      return xmlAttribute.Value;
                  }
                  else if (Ref.Equal(xmlAttribute.Prefix, prefix))
                    return xmlAttribute.NamespaceURI;
                }
              }
            }
            if (Ref.Equal(xmlNode.Prefix, prefix))
              return xmlNode.NamespaceURI;
            xmlNode = xmlNode.ParentNode;
          }
          else
            xmlNode = xmlNode.NodeType != XmlNodeType.Attribute ? xmlNode.ParentNode : (XmlNode) ((XmlAttribute) xmlNode).OwnerElement;
        }
        if (Ref.Equal(document.strXml, prefix))
          return document.strReservedXml;
        if (Ref.Equal(document.strXmlns, prefix))
          return document.strReservedXmlns;
      }
      return (string) null;
    }

    public virtual string GetPrefixOfNamespace(string namespaceURI)
    {
      return this.GetPrefixOfNamespaceStrict(namespaceURI) ?? string.Empty;
    }

    internal string GetPrefixOfNamespaceStrict(string namespaceURI)
    {
      XmlDocument document = this.Document;
      if (document != null)
      {
        namespaceURI = document.NameTable.Add(namespaceURI);
        XmlNode xmlNode = this;
        while (xmlNode != null)
        {
          if (xmlNode.NodeType == XmlNodeType.Element)
          {
            XmlElement xmlElement = (XmlElement) xmlNode;
            if (xmlElement.HasAttributes)
            {
              XmlAttributeCollection attributes = xmlElement.Attributes;
              for (int index = 0; index < attributes.Count; ++index)
              {
                XmlAttribute xmlAttribute = attributes[index];
                if (xmlAttribute.Prefix.Length == 0)
                {
                  if (Ref.Equal(xmlAttribute.LocalName, document.strXmlns) && xmlAttribute.Value == namespaceURI)
                    return string.Empty;
                }
                else if (Ref.Equal(xmlAttribute.Prefix, document.strXmlns))
                {
                  if (xmlAttribute.Value == namespaceURI)
                    return xmlAttribute.LocalName;
                }
                else if (Ref.Equal(xmlAttribute.NamespaceURI, namespaceURI))
                  return xmlAttribute.Prefix;
              }
            }
            if (Ref.Equal(xmlNode.NamespaceURI, namespaceURI))
              return xmlNode.Prefix;
            xmlNode = xmlNode.ParentNode;
          }
          else
            xmlNode = xmlNode.NodeType != XmlNodeType.Attribute ? xmlNode.ParentNode : (XmlNode) ((XmlAttribute) xmlNode).OwnerElement;
        }
        if (Ref.Equal(document.strReservedXml, namespaceURI))
          return document.strXml;
        if (Ref.Equal(document.strReservedXmlns, namespaceURI))
          return document.strXmlns;
      }
      return (string) null;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    internal virtual void SetParent(XmlNode node)
    {
      if (node == null)
        this.parentNode = (XmlNode) this.OwnerDocument;
      else
        this.parentNode = node;
    }

    internal virtual void SetParentForLoad(XmlNode node)
    {
      this.parentNode = node;
    }

    internal static void SplitName(string name, out string prefix, out string localName)
    {
      int length = name.IndexOf(':');
      if (-1 == length || length == 0 || name.Length - 1 == length)
      {
        prefix = string.Empty;
        localName = name;
      }
      else
      {
        prefix = name.Substring(0, length);
        localName = name.Substring(length + 1);
      }
    }

    internal virtual XmlNode FindChild(XmlNodeType type)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.NodeType == type)
          return xmlNode;
      }
      return (XmlNode) null;
    }

    internal virtual XmlNodeChangedEventArgs GetEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
    {
      XmlDocument ownerDocument = this.OwnerDocument;
      if (ownerDocument == null)
        return (XmlNodeChangedEventArgs) null;
      if (!ownerDocument.IsLoading && (newParent != null && newParent.IsReadOnly || oldParent != null && oldParent.IsReadOnly))
        throw new InvalidOperationException(Res.GetString("Xdom_Node_Modify_ReadOnly"));
      else
        return ownerDocument.GetEventArgs(node, oldParent, newParent, oldValue, newValue, action);
    }

    internal virtual void BeforeEvent(XmlNodeChangedEventArgs args)
    {
      if (args == null)
        return;
      this.OwnerDocument.BeforeEvent(args);
    }

    internal virtual void AfterEvent(XmlNodeChangedEventArgs args)
    {
      if (args == null)
        return;
      this.OwnerDocument.AfterEvent(args);
    }

    internal virtual string GetXPAttribute(string localName, string namespaceURI)
    {
      return string.Empty;
    }

    internal static void NestTextNodes(XmlNode prevNode, XmlNode nextNode)
    {
      nextNode.parentNode = prevNode;
    }

    internal static void UnnestTextNodes(XmlNode prevNode, XmlNode nextNode)
    {
      nextNode.parentNode = prevNode.ParentNode;
    }
  }
}
