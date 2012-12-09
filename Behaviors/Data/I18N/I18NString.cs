#region License GNU GPL
// I18NString.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.ComponentModel;
using BiM.Core.I18n;
using BiM.Protocol.Tools;

namespace BiM.Behaviors.Data
{
    public class I18NString : INotifyPropertyChanged
    {
        private readonly I18NDataManager m_manager;
        private bool m_shouldRefresh = true;

        public I18NString(int id, I18NDataManager manager)
        {
            m_manager = manager;
            Id = id;
        }

        public I18NString(string id, I18NDataManager manager)
        {
            m_manager = manager;
            IdString = id;
        }

        /// <summary>
        /// Used if IdString == null
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// If null, Id is used
        /// </summary>
        public string IdString
        {
            get;
            set;
        }

        private string m_text;

        public string Text
        {
            get {
                if (m_shouldRefresh)
                    Refresh();

                return m_text; }
        }

        private Languages m_language;

        /// <summary>
        /// Default if null
        /// </summary>
        public Languages Language
        {
            get { return m_language; }
            set
            {
                if (m_language != value)
                    Refresh();
                m_language = value; 
            }
        }

        /// <summary>
        /// Update the Text property
        /// </summary>
        public void Refresh()
        {
            if (IdString != null)
                m_text = I18NDataManager.Instance.ReadText(IdString, Language);
            else
                m_text = I18NDataManager.Instance.ReadText(Id, Language);

            m_shouldRefresh = false;
        }

        public static implicit operator string(I18NString instance)
        {
            return instance.Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}