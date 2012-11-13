// Type: System.Drawing.Point
// Assembly: System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Drawing.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;

namespace System.Drawing
{
  [TypeConverter(typeof (PointConverter))]
  [ComVisible(true)]
  [Serializable]
  public struct Point
  {
    public static readonly Point Empty = new Point();
    private int x;
    private int y;

    [Browsable(false)]
    public bool IsEmpty
    {
      get
      {
        if (this.x == 0)
          return this.y == 0;
        else
          return false;
      }
    }

    public int X
    {
      get
      {
        return this.x;
      }
      set
      {
        this.x = value;
      }
    }

    public int Y
    {
      get
      {
        return this.y;
      }
      set
      {
        this.y = value;
      }
    }

    static Point()
    {
    }

    public Point(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public Point(Size sz)
    {
      this.x = sz.Width;
      this.y = sz.Height;
    }

    public Point(int dw)
    {
      this.x = (int) (short) Point.LOWORD(dw);
      this.y = (int) (short) Point.HIWORD(dw);
    }

    public static implicit operator PointF(Point p)
    {
      return new PointF((float) p.X, (float) p.Y);
    }

    public static explicit operator Size(Point p)
    {
      return new Size(p.X, p.Y);
    }

    public static Point operator +(Point pt, Size sz)
    {
      return Point.Add(pt, sz);
    }

    public static Point operator -(Point pt, Size sz)
    {
      return Point.Subtract(pt, sz);
    }

    public static bool operator ==(Point left, Point right)
    {
      if (left.X == right.X)
        return left.Y == right.Y;
      else
        return false;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public static bool operator !=(Point left, Point right)
    {
      return !(left == right);
    }

    public static Point Add(Point pt, Size sz)
    {
      return new Point(pt.X + sz.Width, pt.Y + sz.Height);
    }

    public static Point Subtract(Point pt, Size sz)
    {
      return new Point(pt.X - sz.Width, pt.Y - sz.Height);
    }

    public static Point Ceiling(PointF value)
    {
      return new Point((int) Math.Ceiling((double) value.X), (int) Math.Ceiling((double) value.Y));
    }

    public static Point Truncate(PointF value)
    {
      return new Point((int) value.X, (int) value.Y);
    }

    public static Point Round(PointF value)
    {
      return new Point((int) Math.Round((double) value.X), (int) Math.Round((double) value.Y));
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Point))
        return false;
      Point point = (Point) obj;
      if (point.X == this.X)
        return point.Y == this.Y;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return this.x ^ this.y;
    }

    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    public void Offset(int dx, int dy)
    {
      this.X += dx;
      this.Y += dy;
    }

    public void Offset(Point p)
    {
      this.Offset(p.X, p.Y);
    }

    public override string ToString()
    {
      return "{X=" + this.X.ToString((IFormatProvider) CultureInfo.CurrentCulture) + ",Y=" + this.Y.ToString((IFormatProvider) CultureInfo.CurrentCulture) + "}";
    }

    private static int HIWORD(int n)
    {
      return n >> 16 & (int) ushort.MaxValue;
    }

    private static int LOWORD(int n)
    {
      return n & (int) ushort.MaxValue;
    }
  }
}
