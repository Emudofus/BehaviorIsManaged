#region License GNU GPL
// IconsManager.cs
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
using System.Collections.Generic;
using BiM.Core.Reflection;
using BiM.Protocol.Tools.D2p;

namespace BiM.Behaviors.Data
{
    public class IconsManager : Singleton<IconsManager>
    {
        private D2pFile m_d2PFile;

        public void Initialize(string path)
        {
            m_d2PFile = new D2pFile(path);
        }

        public Icon GetIcon(int id)
        {
            if (!m_d2PFile.Exists(id + ".png"))
                throw new ArgumentException(string.Format("Item icon {0} not found", id));

            var data = m_d2PFile.ReadFile(id + ".png");

            return new Icon(id, id + ".png", data);
        }

        public IEnumerable<Icon> EnumerateIcons()
        {
            foreach (var entry in m_d2PFile.Entries)
            {
                if (!entry.FullFileName.EndsWith(".png"))
                    continue;

                var data = m_d2PFile.ReadFile(entry);
                var id = int.Parse(entry.FileName.Replace(".png", ""));

                yield return new Icon(id, entry.FileName, data);
            }
        }
    }
}