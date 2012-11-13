// Type: System.Xml.XmlTextWriter
// Assembly: System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Xml.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
  public class XmlTextWriter : XmlWriter
  {
    private static string[] stateName = new string[10]
    {
      "Start",
      "Prolog",
      "PostDTD",
      "Element",
      "Attribute",
      "Content",
      "AttrOnly",
      "Epilog",
      "Error",
      "Closed"
    };
    private static string[] tokenName = new string[14]
    {
      "PI",
      "Doctype",
      "Comment",
      "CData",
      "StartElement",
      "EndElement",
      "LongEndElement",
      "StartAttribute",
      "EndAttribute",
      "Content",
      "Base64",
      "RawData",
      "Whitespace",
      "Empty"
    };
    private static readonly XmlTextWriter.State[] stateTableDefault = new XmlTextWriter.State[104]
    {
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.AttrOnly,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Epilog
    };
    private static readonly XmlTextWriter.State[] stateTableDocument = new XmlTextWriter.State[104]
    {
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Element,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Prolog,
      XmlTextWriter.State.PostDTD,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Attribute,
      XmlTextWriter.State.Content,
      XmlTextWriter.State.Error,
      XmlTextWriter.State.Epilog
    };
    private XmlCharType xmlCharType = XmlCharType.Instance;
    private const int NamespaceStackInitialSize = 8;
    private const int MaxNamespacesWalkCount = 16;
    private TextWriter textWriter;
    private XmlTextEncoder xmlEncoder;
    private Encoding encoding;
    private Formatting formatting;
    private bool indented;
    private int indentation;
    private char indentChar;
    private XmlTextWriter.TagInfo[] stack;
    private int top;
    private XmlTextWriter.State[] stateTable;
    private XmlTextWriter.State currentState;
    private XmlTextWriter.Token lastToken;
    private XmlTextWriterBase64Encoder base64Encoder;
    private char quoteChar;
    private char curQuoteChar;
    private bool namespaces;
    private XmlTextWriter.SpecialAttr specialAttr;
    private string prefixForXmlNs;
    private bool flush;
    private XmlTextWriter.Namespace[] nsStack;
    private int nsTop;
    private Dictionary<string, int> nsHashtable;
    private bool useNsHashtable;

    public Stream BaseStream
    {
      get
      {
        StreamWriter streamWriter = this.textWriter as StreamWriter;
        if (streamWriter != null)
          return streamWriter.BaseStream;
        else
          return (Stream) null;
      }
    }

    public bool Namespaces
    {
      get
      {
        return this.namespaces;
      }
      set
      {
        if (this.currentState != XmlTextWriter.State.Start)
          throw new InvalidOperationException(Res.GetString("Xml_NotInWriteState"));
        this.namespaces = value;
      }
    }

    public Formatting Formatting
    {
      get
      {
        return this.formatting;
      }
      set
      {
        this.formatting = value;
        this.indented = value == Formatting.Indented;
      }
    }

    public int Indentation
    {
      get
      {
        return this.indentation;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException(Res.GetString("Xml_InvalidIndentation"));
        this.indentation = value;
      }
    }

    public char IndentChar
    {
      get
      {
        return this.indentChar;
      }
      set
      {
        this.indentChar = value;
      }
    }

    public char QuoteChar
    {
      get
      {
        return this.quoteChar;
      }
      set
      {
        if ((int) value != 34 && (int) value != 39)
          throw new ArgumentException(Res.GetString("Xml_InvalidQuote"));
        this.quoteChar = value;
        this.xmlEncoder.QuoteChar = value;
      }
    }

    public override WriteState WriteState
    {
      get
      {
        switch (this.currentState)
        {
          case XmlTextWriter.State.Start:
            return WriteState.Start;
          case XmlTextWriter.State.Prolog:
          case XmlTextWriter.State.PostDTD:
            return WriteState.Prolog;
          case XmlTextWriter.State.Element:
            return WriteState.Element;
          case XmlTextWriter.State.Attribute:
          case XmlTextWriter.State.AttrOnly:
            return WriteState.Attribute;
          case XmlTextWriter.State.Content:
          case XmlTextWriter.State.Epilog:
            return WriteState.Content;
          case XmlTextWriter.State.Error:
            return WriteState.Error;
          case XmlTextWriter.State.Closed:
            return WriteState.Closed;
          default:
            return WriteState.Error;
        }
      }
    }

    public override XmlSpace XmlSpace
    {
      get
      {
        for (int index = this.top; index > 0; --index)
        {
          XmlSpace xmlSpace = this.stack[index].xmlSpace;
          if (xmlSpace != XmlSpace.None)
            return xmlSpace;
        }
        return XmlSpace.None;
      }
    }

    public override string XmlLang
    {
      get
      {
        for (int index = this.top; index > 0; --index)
        {
          string str = this.stack[index].xmlLang;
          if (str != null)
            return str;
        }
        return (string) null;
      }
    }

    static XmlTextWriter()
    {
    }

    internal XmlTextWriter()
    {
      this.namespaces = true;
      this.formatting = Formatting.None;
      this.indentation = 2;
      this.indentChar = ' ';
      this.nsStack = new XmlTextWriter.Namespace[8];
      this.nsTop = -1;
      this.stack = new XmlTextWriter.TagInfo[10];
      this.top = 0;
      this.stack[this.top].Init(-1);
      this.quoteChar = '"';
      this.stateTable = XmlTextWriter.stateTableDefault;
      this.currentState = XmlTextWriter.State.Start;
      this.lastToken = XmlTextWriter.Token.Empty;
    }

    public XmlTextWriter(Stream w, Encoding encoding)
      : this()
    {
      this.encoding = encoding;
      this.textWriter = encoding == null ? (TextWriter) new StreamWriter(w) : (TextWriter) new StreamWriter(w, encoding);
      this.xmlEncoder = new XmlTextEncoder(this.textWriter);
      this.xmlEncoder.QuoteChar = this.quoteChar;
    }

    public XmlTextWriter(string filename, Encoding encoding)
      : this((Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), encoding)
    {
    }

    public XmlTextWriter(TextWriter w)
      : this()
    {
      this.textWriter = w;
      this.encoding = w.Encoding;
      this.xmlEncoder = new XmlTextEncoder(w);
      this.xmlEncoder.QuoteChar = this.quoteChar;
    }

    public override void WriteStartDocument()
    {
      this.StartDocument(-1);
    }

    public override void WriteStartDocument(bool standalone)
    {
      this.StartDocument(standalone ? 1 : 0);
    }

    public override void WriteEndDocument()
    {
      try
      {
        this.AutoCompleteAll();
        if (this.currentState != XmlTextWriter.State.Epilog)
        {
          if (this.currentState == XmlTextWriter.State.Closed)
            throw new ArgumentException(Res.GetString("Xml_ClosedOrError"));
          else
            throw new ArgumentException(Res.GetString("Xml_NoRoot"));
        }
        else
        {
          this.stateTable = XmlTextWriter.stateTableDefault;
          this.currentState = XmlTextWriter.State.Start;
          this.lastToken = XmlTextWriter.Token.Empty;
        }
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      try
      {
        this.ValidateName(name, false);
        this.AutoComplete(XmlTextWriter.Token.Doctype);
        this.textWriter.Write("<!DOCTYPE ");
        this.textWriter.Write(name);
        if (pubid != null)
        {
          this.textWriter.Write(" PUBLIC " + (object) this.quoteChar);
          this.textWriter.Write(pubid);
          this.textWriter.Write((string) (object) this.quoteChar + (object) " " + (string) (object) this.quoteChar);
          this.textWriter.Write(sysid);
          this.textWriter.Write(this.quoteChar);
        }
        else if (sysid != null)
        {
          this.textWriter.Write(" SYSTEM " + (object) this.quoteChar);
          this.textWriter.Write(sysid);
          this.textWriter.Write(this.quoteChar);
        }
        if (subset != null)
        {
          this.textWriter.Write("[");
          this.textWriter.Write(subset);
          this.textWriter.Write("]");
        }
        this.textWriter.Write('>');
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.StartElement);
        this.PushStack();
        this.textWriter.Write('<');
        if (this.namespaces)
        {
          this.stack[this.top].defaultNs = this.stack[this.top - 1].defaultNs;
          if (this.stack[this.top - 1].defaultNsState != XmlTextWriter.NamespaceState.Uninitialized)
            this.stack[this.top].defaultNsState = XmlTextWriter.NamespaceState.NotDeclaredButInScope;
          this.stack[this.top].mixed = this.stack[this.top - 1].mixed;
          if (ns == null)
          {
            if (prefix != null && prefix.Length != 0 && this.LookupNamespace(prefix) == -1)
              throw new ArgumentException(Res.GetString("Xml_UndefPrefix"));
          }
          else if (prefix == null)
          {
            string prefix1 = this.FindPrefix(ns);
            if (prefix1 != null)
              prefix = prefix1;
            else
              this.PushNamespace((string) null, ns, false);
          }
          else if (prefix.Length == 0)
          {
            this.PushNamespace((string) null, ns, false);
          }
          else
          {
            if (ns.Length == 0)
              prefix = (string) null;
            this.VerifyPrefixXml(prefix, ns);
            this.PushNamespace(prefix, ns, false);
          }
          this.stack[this.top].prefix = (string) null;
          if (prefix != null && prefix.Length != 0)
          {
            this.stack[this.top].prefix = prefix;
            this.textWriter.Write(prefix);
            this.textWriter.Write(':');
          }
        }
        else if (ns != null && ns.Length != 0 || prefix != null && prefix.Length != 0)
          throw new ArgumentException(Res.GetString("Xml_NoNamespaces"));
        this.stack[this.top].name = localName;
        this.textWriter.Write(localName);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteEndElement()
    {
      this.InternalWriteEndElement(false);
    }

    public override void WriteFullEndElement()
    {
      this.InternalWriteEndElement(true);
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.StartAttribute);
        this.specialAttr = XmlTextWriter.SpecialAttr.None;
        if (this.namespaces)
        {
          if (prefix != null && prefix.Length == 0)
            prefix = (string) null;
          if (ns == "http://www.w3.org/2000/xmlns/" && prefix == null && localName != "xmlns")
            prefix = "xmlns";
          if (prefix == "xml")
          {
            if (localName == "lang")
              this.specialAttr = XmlTextWriter.SpecialAttr.XmlLang;
            else if (localName == "space")
              this.specialAttr = XmlTextWriter.SpecialAttr.XmlSpace;
          }
          else if (prefix == "xmlns")
          {
            if ("http://www.w3.org/2000/xmlns/" != ns && ns != null)
              throw new ArgumentException(Res.GetString("Xml_XmlnsBelongsToReservedNs"));
            if (localName == null || localName.Length == 0)
            {
              localName = prefix;
              prefix = (string) null;
              this.prefixForXmlNs = (string) null;
            }
            else
              this.prefixForXmlNs = localName;
            this.specialAttr = XmlTextWriter.SpecialAttr.XmlNs;
          }
          else if (prefix == null && localName == "xmlns")
          {
            if ("http://www.w3.org/2000/xmlns/" != ns && ns != null)
              throw new ArgumentException(Res.GetString("Xml_XmlnsBelongsToReservedNs"));
            this.specialAttr = XmlTextWriter.SpecialAttr.XmlNs;
            this.prefixForXmlNs = (string) null;
          }
          else if (ns == null)
          {
            if (prefix != null && this.LookupNamespace(prefix) == -1)
              throw new ArgumentException(Res.GetString("Xml_UndefPrefix"));
          }
          else if (ns.Length == 0)
          {
            prefix = string.Empty;
          }
          else
          {
            this.VerifyPrefixXml(prefix, ns);
            if (prefix != null && this.LookupNamespaceInCurrentScope(prefix) != -1)
              prefix = (string) null;
            string prefix1 = this.FindPrefix(ns);
            if (prefix1 != null && (prefix == null || prefix == prefix1))
            {
              prefix = prefix1;
            }
            else
            {
              if (prefix == null)
                prefix = this.GeneratePrefix();
              this.PushNamespace(prefix, ns, false);
            }
          }
          if (prefix != null && prefix.Length != 0)
          {
            this.textWriter.Write(prefix);
            this.textWriter.Write(':');
          }
        }
        else
        {
          if (ns != null && ns.Length != 0 || prefix != null && prefix.Length != 0)
            throw new ArgumentException(Res.GetString("Xml_NoNamespaces"));
          if (localName == "xml:lang")
            this.specialAttr = XmlTextWriter.SpecialAttr.XmlLang;
          else if (localName == "xml:space")
            this.specialAttr = XmlTextWriter.SpecialAttr.XmlSpace;
        }
        this.xmlEncoder.StartAttribute(this.specialAttr != XmlTextWriter.SpecialAttr.None);
        this.textWriter.Write(localName);
        this.textWriter.Write('=');
        if ((int) this.curQuoteChar != (int) this.quoteChar)
        {
          this.curQuoteChar = this.quoteChar;
          this.xmlEncoder.QuoteChar = this.quoteChar;
        }
        this.textWriter.Write(this.curQuoteChar);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteEndAttribute()
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.EndAttribute);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteCData(string text)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.CData);
        if (text != null && text.IndexOf("]]>", StringComparison.Ordinal) >= 0)
          throw new ArgumentException(Res.GetString("Xml_InvalidCDataChars"));
        this.textWriter.Write("<![CDATA[");
        if (text != null)
          this.xmlEncoder.WriteRawWithSurrogateChecking(text);
        this.textWriter.Write("]]>");
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteComment(string text)
    {
      try
      {
        if (text != null && (text.IndexOf("--", StringComparison.Ordinal) >= 0 || text.Length != 0 && (int) text[text.Length - 1] == 45))
          throw new ArgumentException(Res.GetString("Xml_InvalidCommentChars"));
        this.AutoComplete(XmlTextWriter.Token.Comment);
        this.textWriter.Write("<!--");
        if (text != null)
          this.xmlEncoder.WriteRawWithSurrogateChecking(text);
        this.textWriter.Write("-->");
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      try
      {
        if (text != null && text.IndexOf("?>", StringComparison.Ordinal) >= 0)
          throw new ArgumentException(Res.GetString("Xml_InvalidPiChars"));
        if (string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) == 0 && this.stateTable == XmlTextWriter.stateTableDocument)
          throw new ArgumentException(Res.GetString("Xml_DupXmlDecl"));
        this.AutoComplete(XmlTextWriter.Token.PI);
        this.InternalWriteProcessingInstruction(name, text);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteEntityRef(string name)
    {
      try
      {
        this.ValidateName(name, false);
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.xmlEncoder.WriteEntityRef(name);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteCharEntity(char ch)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.xmlEncoder.WriteCharEntity(ch);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteWhitespace(string ws)
    {
      try
      {
        if (ws == null)
          ws = string.Empty;
        if (!this.xmlCharType.IsOnlyWhitespace(ws))
          throw new ArgumentException(Res.GetString("Xml_NonWhitespace"));
        this.AutoComplete(XmlTextWriter.Token.Whitespace);
        this.xmlEncoder.Write(ws);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteString(string text)
    {
      try
      {
        if (text == null || text.Length == 0)
          return;
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.xmlEncoder.Write(text);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.xmlEncoder.WriteSurrogateCharEntity(lowChar, highChar);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.xmlEncoder.Write(buffer, index, count);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.RawData);
        this.xmlEncoder.WriteRaw(buffer, index, count);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteRaw(string data)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.RawData);
        this.xmlEncoder.WriteRawWithSurrogateChecking(data);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      try
      {
        if (!this.flush)
          this.AutoComplete(XmlTextWriter.Token.Base64);
        this.flush = true;
        if (this.base64Encoder == null)
          this.base64Encoder = new XmlTextWriterBase64Encoder(this.xmlEncoder);
        this.base64Encoder.Encode(buffer, index, count);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteBinHex(byte[] buffer, int index, int count)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        BinHexEncoder.Encode(buffer, index, count, (XmlWriter) this);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void Close()
    {
      try
      {
        this.AutoCompleteAll();
      }
      catch
      {
      }
      finally
      {
        this.currentState = XmlTextWriter.State.Closed;
        this.textWriter.Close();
      }
    }

    public override void Flush()
    {
      this.textWriter.Flush();
    }

    public override void WriteName(string name)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        this.InternalWriteName(name, false);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override void WriteQualifiedName(string localName, string ns)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        if (this.namespaces)
        {
          if (ns != null && ns.Length != 0 && ns != this.stack[this.top].defaultNs)
          {
            string str = this.FindPrefix(ns);
            if (str == null)
            {
              if (this.currentState != XmlTextWriter.State.Attribute)
              {
                throw new ArgumentException(Res.GetString("Xml_UndefNamespace", new object[1]
                {
                  (object) ns
                }));
              }
              else
              {
                str = this.GeneratePrefix();
                this.PushNamespace(str, ns, false);
              }
            }
            if (str.Length != 0)
            {
              this.InternalWriteName(str, true);
              this.textWriter.Write(':');
            }
          }
        }
        else if (ns != null && ns.Length != 0)
          throw new ArgumentException(Res.GetString("Xml_NoNamespaces"));
        this.InternalWriteName(localName, true);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    public override string LookupPrefix(string ns)
    {
      if (ns == null || ns.Length == 0)
        throw new ArgumentException(Res.GetString("Xml_EmptyName"));
      string str = this.FindPrefix(ns);
      if (str == null && ns == this.stack[this.top].defaultNs)
        str = string.Empty;
      return str;
    }

    public override void WriteNmToken(string name)
    {
      try
      {
        this.AutoComplete(XmlTextWriter.Token.Content);
        if (name == null || name.Length == 0)
          throw new ArgumentException(Res.GetString("Xml_EmptyName"));
        if (!ValidateNames.IsNmtokenNoNamespaces(name))
          throw new ArgumentException(Res.GetString("Xml_InvalidNameChars", new object[1]
          {
            (object) name
          }));
        else
          this.textWriter.Write(name);
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    private void StartDocument(int standalone)
    {
      try
      {
        if (this.currentState != XmlTextWriter.State.Start)
          throw new InvalidOperationException(Res.GetString("Xml_NotTheFirst"));
        this.stateTable = XmlTextWriter.stateTableDocument;
        this.currentState = XmlTextWriter.State.Prolog;
        StringBuilder stringBuilder = new StringBuilder(128);
        stringBuilder.Append(string.Concat(new object[4]
        {
          (object) "version=",
          (object) this.quoteChar,
          (object) "1.0",
          (object) this.quoteChar
        }));
        if (this.encoding != null)
        {
          stringBuilder.Append(" encoding=");
          stringBuilder.Append(this.quoteChar);
          stringBuilder.Append(this.encoding.WebName);
          stringBuilder.Append(this.quoteChar);
        }
        if (standalone >= 0)
        {
          stringBuilder.Append(" standalone=");
          stringBuilder.Append(this.quoteChar);
          stringBuilder.Append(standalone == 0 ? "no" : "yes");
          stringBuilder.Append(this.quoteChar);
        }
        this.InternalWriteProcessingInstruction("xml", ((object) stringBuilder).ToString());
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    private void AutoComplete(XmlTextWriter.Token token)
    {
      if (this.currentState == XmlTextWriter.State.Closed)
        throw new InvalidOperationException(Res.GetString("Xml_Closed"));
      if (this.currentState == XmlTextWriter.State.Error)
      {
        throw new InvalidOperationException(Res.GetString("Xml_WrongToken", (object) XmlTextWriter.tokenName[(int) token], (object) XmlTextWriter.stateName[8]));
      }
      else
      {
        XmlTextWriter.State state = this.stateTable[(int) ((int) token * 8 + this.currentState)];
        if (state == XmlTextWriter.State.Error)
        {
          throw new InvalidOperationException(Res.GetString("Xml_WrongToken", (object) XmlTextWriter.tokenName[(int) token], (object) XmlTextWriter.stateName[(int) this.currentState]));
        }
        else
        {
          switch (token)
          {
            case XmlTextWriter.Token.PI:
            case XmlTextWriter.Token.Comment:
            case XmlTextWriter.Token.CData:
            case XmlTextWriter.Token.StartElement:
              if (this.currentState == XmlTextWriter.State.Attribute)
              {
                this.WriteEndAttributeQuote();
                this.WriteEndStartTag(false);
              }
              else if (this.currentState == XmlTextWriter.State.Element)
                this.WriteEndStartTag(false);
              if (token == XmlTextWriter.Token.CData)
              {
                this.stack[this.top].mixed = true;
                break;
              }
              else if (this.indented && this.currentState != XmlTextWriter.State.Start)
              {
                this.Indent(false);
                break;
              }
              else
                break;
            case XmlTextWriter.Token.Doctype:
              if (this.indented && this.currentState != XmlTextWriter.State.Start)
              {
                this.Indent(false);
                break;
              }
              else
                break;
            case XmlTextWriter.Token.EndElement:
            case XmlTextWriter.Token.LongEndElement:
              if (this.flush)
                this.FlushEncoders();
              if (this.currentState == XmlTextWriter.State.Attribute)
                this.WriteEndAttributeQuote();
              if (this.currentState == XmlTextWriter.State.Content)
                token = XmlTextWriter.Token.LongEndElement;
              else
                this.WriteEndStartTag(token == XmlTextWriter.Token.EndElement);
              if (XmlTextWriter.stateTableDocument == this.stateTable && this.top == 1)
              {
                state = XmlTextWriter.State.Epilog;
                break;
              }
              else
                break;
            case XmlTextWriter.Token.StartAttribute:
              if (this.flush)
                this.FlushEncoders();
              if (this.currentState == XmlTextWriter.State.Attribute)
              {
                this.WriteEndAttributeQuote();
                this.textWriter.Write(' ');
                break;
              }
              else if (this.currentState == XmlTextWriter.State.Element)
              {
                this.textWriter.Write(' ');
                break;
              }
              else
                break;
            case XmlTextWriter.Token.EndAttribute:
              if (this.flush)
                this.FlushEncoders();
              this.WriteEndAttributeQuote();
              break;
            case XmlTextWriter.Token.Content:
            case XmlTextWriter.Token.Base64:
            case XmlTextWriter.Token.RawData:
            case XmlTextWriter.Token.Whitespace:
              if (token != XmlTextWriter.Token.Base64 && this.flush)
                this.FlushEncoders();
              if (this.currentState == XmlTextWriter.State.Element && this.lastToken != XmlTextWriter.Token.Content)
                this.WriteEndStartTag(false);
              if (state == XmlTextWriter.State.Content)
              {
                this.stack[this.top].mixed = true;
                break;
              }
              else
                break;
            default:
              throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
          }
          this.currentState = state;
          this.lastToken = token;
        }
      }
    }

    private void AutoCompleteAll()
    {
      if (this.flush)
        this.FlushEncoders();
      while (this.top > 0)
        this.WriteEndElement();
    }

    private void InternalWriteEndElement(bool longFormat)
    {
      try
      {
        if (this.top <= 0)
          throw new InvalidOperationException(Res.GetString("Xml_NoStartTag"));
        this.AutoComplete(longFormat ? XmlTextWriter.Token.LongEndElement : XmlTextWriter.Token.EndElement);
        if (this.lastToken == XmlTextWriter.Token.LongEndElement)
        {
          if (this.indented)
            this.Indent(true);
          this.textWriter.Write('<');
          this.textWriter.Write('/');
          if (this.namespaces && this.stack[this.top].prefix != null)
          {
            this.textWriter.Write(this.stack[this.top].prefix);
            this.textWriter.Write(':');
          }
          this.textWriter.Write(this.stack[this.top].name);
          this.textWriter.Write('>');
        }
        int num = this.stack[this.top].prevNsTop;
        if (this.useNsHashtable && num < this.nsTop)
          this.PopNamespaces(num + 1, this.nsTop);
        this.nsTop = num;
        --this.top;
      }
      catch
      {
        this.currentState = XmlTextWriter.State.Error;
        throw;
      }
    }

    private void WriteEndStartTag(bool empty)
    {
      this.xmlEncoder.StartAttribute(false);
      for (int index = this.nsTop; index > this.stack[this.top].prevNsTop; --index)
      {
        if (!this.nsStack[index].declared)
        {
          this.textWriter.Write(" xmlns");
          this.textWriter.Write(':');
          this.textWriter.Write(this.nsStack[index].prefix);
          this.textWriter.Write('=');
          this.textWriter.Write(this.quoteChar);
          this.xmlEncoder.Write(this.nsStack[index].ns);
          this.textWriter.Write(this.quoteChar);
        }
      }
      if (this.stack[this.top].defaultNs != this.stack[this.top - 1].defaultNs && this.stack[this.top].defaultNsState == XmlTextWriter.NamespaceState.DeclaredButNotWrittenOut)
      {
        this.textWriter.Write(" xmlns");
        this.textWriter.Write('=');
        this.textWriter.Write(this.quoteChar);
        this.xmlEncoder.Write(this.stack[this.top].defaultNs);
        this.textWriter.Write(this.quoteChar);
        this.stack[this.top].defaultNsState = XmlTextWriter.NamespaceState.DeclaredAndWrittenOut;
      }
      this.xmlEncoder.EndAttribute();
      if (empty)
        this.textWriter.Write(" /");
      this.textWriter.Write('>');
    }

    private void WriteEndAttributeQuote()
    {
      if (this.specialAttr != XmlTextWriter.SpecialAttr.None)
        this.HandleSpecialAttribute();
      this.xmlEncoder.EndAttribute();
      this.textWriter.Write(this.curQuoteChar);
    }

    private void Indent(bool beforeEndElement)
    {
      if (this.top == 0)
      {
        this.textWriter.WriteLine();
      }
      else
      {
        if (this.stack[this.top].mixed)
          return;
        this.textWriter.WriteLine();
        for (int index = (beforeEndElement ? this.top - 1 : this.top) * this.indentation; index > 0; --index)
          this.textWriter.Write(this.indentChar);
      }
    }

    private void PushNamespace(string prefix, string ns, bool declared)
    {
      if ("http://www.w3.org/2000/xmlns/" == ns)
        throw new ArgumentException(Res.GetString("Xml_CanNotBindToReservedNamespace"));
      if (prefix == null)
      {
        switch (this.stack[this.top].defaultNsState)
        {
          case XmlTextWriter.NamespaceState.Uninitialized:
          case XmlTextWriter.NamespaceState.NotDeclaredButInScope:
            this.stack[this.top].defaultNs = ns;
            goto case 2;
          case XmlTextWriter.NamespaceState.DeclaredButNotWrittenOut:
            this.stack[this.top].defaultNsState = declared ? XmlTextWriter.NamespaceState.DeclaredAndWrittenOut : XmlTextWriter.NamespaceState.DeclaredButNotWrittenOut;
            break;
        }
      }
      else
      {
        if (prefix.Length != 0 && ns.Length == 0)
          throw new ArgumentException(Res.GetString("Xml_PrefixForEmptyNs"));
        int index = this.LookupNamespace(prefix);
        if (index != -1 && this.nsStack[index].ns == ns)
        {
          if (!declared)
            return;
          this.nsStack[index].declared = true;
        }
        else
        {
          if (declared && index != -1 && index > this.stack[this.top].prevNsTop)
            this.nsStack[index].declared = true;
          this.AddNamespace(prefix, ns, declared);
        }
      }
    }

    private void AddNamespace(string prefix, string ns, bool declared)
    {
      int index = ++this.nsTop;
      if (index == this.nsStack.Length)
      {
        XmlTextWriter.Namespace[] namespaceArray = new XmlTextWriter.Namespace[index * 2];
        Array.Copy((Array) this.nsStack, (Array) namespaceArray, index);
        this.nsStack = namespaceArray;
      }
      this.nsStack[index].Set(prefix, ns, declared);
      if (this.useNsHashtable)
      {
        this.AddToNamespaceHashtable(index);
      }
      else
      {
        if (index != 16)
          return;
        this.nsHashtable = new Dictionary<string, int>((IEqualityComparer<string>) new SecureStringHasher());
        for (int namespaceIndex = 0; namespaceIndex <= index; ++namespaceIndex)
          this.AddToNamespaceHashtable(namespaceIndex);
        this.useNsHashtable = true;
      }
    }

    private void AddToNamespaceHashtable(int namespaceIndex)
    {
      string key = this.nsStack[namespaceIndex].prefix;
      int num;
      if (this.nsHashtable.TryGetValue(key, out num))
        this.nsStack[namespaceIndex].prevNsIndex = num;
      this.nsHashtable[key] = namespaceIndex;
    }

    private void PopNamespaces(int indexFrom, int indexTo)
    {
      for (int index = indexTo; index >= indexFrom; --index)
      {
        if (this.nsStack[index].prevNsIndex == -1)
          this.nsHashtable.Remove(this.nsStack[index].prefix);
        else
          this.nsHashtable[this.nsStack[index].prefix] = this.nsStack[index].prevNsIndex;
      }
    }

    private string GeneratePrefix()
    {
      return "d" + this.top.ToString("d", (IFormatProvider) CultureInfo.InvariantCulture) + "p" + (this.stack[this.top].prefixCount++ + 1).ToString("d", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    private void InternalWriteProcessingInstruction(string name, string text)
    {
      this.textWriter.Write("<?");
      this.ValidateName(name, false);
      this.textWriter.Write(name);
      this.textWriter.Write(' ');
      if (text != null)
        this.xmlEncoder.WriteRawWithSurrogateChecking(text);
      this.textWriter.Write("?>");
    }

    private int LookupNamespace(string prefix)
    {
      if (this.useNsHashtable)
      {
        int num;
        if (this.nsHashtable.TryGetValue(prefix, out num))
          return num;
      }
      else
      {
        for (int index = this.nsTop; index >= 0; --index)
        {
          if (this.nsStack[index].prefix == prefix)
            return index;
        }
      }
      return -1;
    }

    private int LookupNamespaceInCurrentScope(string prefix)
    {
      if (this.useNsHashtable)
      {
        int num;
        if (this.nsHashtable.TryGetValue(prefix, out num) && num > this.stack[this.top].prevNsTop)
          return num;
      }
      else
      {
        for (int index = this.nsTop; index > this.stack[this.top].prevNsTop; --index)
        {
          if (this.nsStack[index].prefix == prefix)
            return index;
        }
      }
      return -1;
    }

    private string FindPrefix(string ns)
    {
      for (int index = this.nsTop; index >= 0; --index)
      {
        if (this.nsStack[index].ns == ns && this.LookupNamespace(this.nsStack[index].prefix) == index)
          return this.nsStack[index].prefix;
      }
      return (string) null;
    }

    private void InternalWriteName(string name, bool isNCName)
    {
      this.ValidateName(name, isNCName);
      this.textWriter.Write(name);
    }

    private void ValidateName(string name, bool isNCName)
    {
      if (name == null || name.Length == 0)
        throw new ArgumentException(Res.GetString("Xml_EmptyName"));
      int length = name.Length;
      if (this.namespaces)
      {
        int num = -1;
        for (int index = ValidateNames.ParseNCName(name); index != length; {
          int offset;
          index = offset + ValidateNames.ParseNmtoken(name, offset);
        }
        )
        {
          if ((int) name[index] == 58 && !isNCName && (num == -1 && index > 0) && index + 1 < length)
          {
            num = index;
            offset = index + 1;
          }
          else
            goto label_9;
        }
        return;
      }
      else if (ValidateNames.IsNameNoNamespaces(name))
        return;
label_9:
      throw new ArgumentException(Res.GetString("Xml_InvalidNameChars", new object[1]
      {
        (object) name
      }));
    }

    private void HandleSpecialAttribute()
    {
      string attributeValue = this.xmlEncoder.AttributeValue;
      switch (this.specialAttr)
      {
        case XmlTextWriter.SpecialAttr.XmlSpace:
          string str = XmlConvert.TrimString(attributeValue);
          if (str == "default")
          {
            this.stack[this.top].xmlSpace = XmlSpace.Default;
            break;
          }
          else if (str == "preserve")
          {
            this.stack[this.top].xmlSpace = XmlSpace.Preserve;
            break;
          }
          else
            throw new ArgumentException(Res.GetString("Xml_InvalidXmlSpace", new object[1]
            {
              (object) str
            }));
        case XmlTextWriter.SpecialAttr.XmlLang:
          this.stack[this.top].xmlLang = attributeValue;
          break;
        case XmlTextWriter.SpecialAttr.XmlNs:
          this.VerifyPrefixXml(this.prefixForXmlNs, attributeValue);
          this.PushNamespace(this.prefixForXmlNs, attributeValue, true);
          break;
      }
    }

    private void VerifyPrefixXml(string prefix, string ns)
    {
      if (prefix != null && prefix.Length == 3 && ((int) prefix[0] == 120 || (int) prefix[0] == 88) && (((int) prefix[1] == 109 || (int) prefix[1] == 77) && ((int) prefix[2] == 108 || (int) prefix[2] == 76)) && "http://www.w3.org/XML/1998/namespace" != ns)
        throw new ArgumentException(Res.GetString("Xml_InvalidPrefix"));
    }

    private void PushStack()
    {
      if (this.top == this.stack.Length - 1)
      {
        XmlTextWriter.TagInfo[] tagInfoArray = new XmlTextWriter.TagInfo[this.stack.Length + 10];
        if (this.top > 0)
          Array.Copy((Array) this.stack, (Array) tagInfoArray, this.top + 1);
        this.stack = tagInfoArray;
      }
      ++this.top;
      this.stack[this.top].Init(this.nsTop);
    }

    private void FlushEncoders()
    {
      if (this.base64Encoder != null)
        this.base64Encoder.Flush();
      this.flush = false;
    }

    private enum NamespaceState
    {
      Uninitialized,
      NotDeclaredButInScope,
      DeclaredButNotWrittenOut,
      DeclaredAndWrittenOut,
    }

    private struct TagInfo
    {
      internal string name;
      internal string prefix;
      internal string defaultNs;
      internal XmlTextWriter.NamespaceState defaultNsState;
      internal XmlSpace xmlSpace;
      internal string xmlLang;
      internal int prevNsTop;
      internal int prefixCount;
      internal bool mixed;

      internal void Init(int nsTop)
      {
        this.name = (string) null;
        this.defaultNs = string.Empty;
        this.defaultNsState = XmlTextWriter.NamespaceState.Uninitialized;
        this.xmlSpace = XmlSpace.None;
        this.xmlLang = (string) null;
        this.prevNsTop = nsTop;
        this.prefixCount = 0;
        this.mixed = false;
      }
    }

    private struct Namespace
    {
      internal string prefix;
      internal string ns;
      internal bool declared;
      internal int prevNsIndex;

      internal void Set(string prefix, string ns, bool declared)
      {
        this.prefix = prefix;
        this.ns = ns;
        this.declared = declared;
        this.prevNsIndex = -1;
      }
    }

    private enum SpecialAttr
    {
      None,
      XmlSpace,
      XmlLang,
      XmlNs,
    }

    private enum State
    {
      Start,
      Prolog,
      PostDTD,
      Element,
      Attribute,
      Content,
      AttrOnly,
      Epilog,
      Error,
      Closed,
    }

    private enum Token
    {
      PI,
      Doctype,
      Comment,
      CData,
      StartElement,
      EndElement,
      LongEndElement,
      StartAttribute,
      EndAttribute,
      Content,
      Base64,
      RawData,
      Whitespace,
      Empty,
    }
  }
}
