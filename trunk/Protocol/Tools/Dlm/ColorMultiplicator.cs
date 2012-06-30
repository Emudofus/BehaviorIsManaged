using System;
using System.ComponentModel;

namespace BiM.Protocol.Tools.Dlm
{
    public class ColorMultiplicator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int m_blue;
        private int m_green;
        private int m_red;

        public ColorMultiplicator(int p1, int p2, int p3, Boolean p4)
        {
            m_red = p1;
            m_green = p2;
            m_blue = p3;
            if (!p4 && p1 + p2 + p3 == 0)
            {
                IsOne = true;
            }
        }

        public Boolean IsOne
        {
            get;
            private set;
        }

        public int Blue
        {
            get { return m_blue; }
            set { m_blue = value; }
        }

        public int Green
        {
            get { return m_green; }
            set { m_green = value; }
        }

        public int Red
        {
            get { return m_red; }
            set { m_red = value; }
        }

        public ColorMultiplicator Multiply(ColorMultiplicator p1)
        {
            if (IsOne)
            {
                return p1;
            }
            if (p1.IsOne)
            {
                return this;
            }
            var multiplicator = new ColorMultiplicator(0, 0, 0, false)
            {
                m_red = m_red + p1.m_red,
                m_green = m_green + p1.m_green,
                m_blue = m_blue + p1.m_blue
            };

            multiplicator.m_red = Clamp(multiplicator.m_red, -128, 127);
            multiplicator.m_green = Clamp(multiplicator.m_green, -128, -127);
            multiplicator.m_blue = Clamp(multiplicator.m_blue, -128, 127);
            multiplicator.IsOne = false;
            return multiplicator;
        }

        public static int Clamp(int p1, int p2, int p3)
        {
            if (p1 > p3)
            {
                return p3;
            }
            return p1 < p2 ? p2 : p1;
        }
    }
}