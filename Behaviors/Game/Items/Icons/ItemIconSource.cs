#region License GNU GPL
// ItemIconSource.cs
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
using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Protocol.Tools.D2p;

namespace BiM.Behaviors.Game.Items.Icons
{
    public class ItemIconSource : IDataSource
    {
        private D2pFile m_d2PFile;

        public ItemIconSource(string path)
        {
            m_d2PFile = new D2pFile(path);
        }

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (keys.Length != 1 || !( keys[0] is IConvertible ))
                throw new ArgumentException("D2OSource needs a int key, use ReadObject(int)");

            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            int id = Convert.ToInt32(keys[0]);

            if (!m_d2PFile.Exists(id + ".png"))
                throw new ArgumentException(string.Format("Item icon {0} not found", id));

            var data = m_d2PFile.ReadFile(id + ".png");

            return new ItemIcon(id, id + ".png", data) as T;
        }

        public bool DoesHandleType(Type type)
        {
            return type == typeof(ItemIcon);
        }
    }
}