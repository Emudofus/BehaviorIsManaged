#region License GNU GPL
// ItemIcon.cs
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
using System.IO;
using System.Windows.Media.Imaging;

namespace BiM.Behaviors.Game.Items.Icons
{
    public class ItemIcon
    {
        public ItemIcon(int id, string name, byte[] data)
        {
            Id = id;
            Name = name;
            Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = new MemoryStream(data);
            Image.EndInit();
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public BitmapImage Image
        {
            get;
            private set;
        }
    }
}