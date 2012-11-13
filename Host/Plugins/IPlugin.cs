#region License GNU GPL
// IPlugin.cs
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

namespace BiM.Host.Plugins
{
    public static class PluginExtensions
    {
        public static string GetDefaultDescription(this IPlugin plugin)
        {
            return string.Format("'{0}' v{1} by {2}", plugin.Name, plugin.GetType().Assembly.GetName().Version, plugin.Author);
        }
    }

    public interface IPlugin
    {
        PluginContext Context
        {
            get;
        }

        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        string Author
        {
            get;
        }

        Version Version
        {
            get;
        }

        void Initialize();
        void Shutdown();

        void Dispose();
    }
}