#region License GNU GPL

// TransitionsManager.cs
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
using System.IO;
using System.Reflection;
using BiM.Core.Extensions;
using BiM.Core.Reflection;

namespace BiM.Behaviors.Game.World.MapTraveling.Transitions
{
    public class TransitionsManager : Singleton<TransitionsManager>
    {
        private readonly Dictionary<int, Func<SubMapTransition>> m_transitionsCreators = new Dictionary<int, Func<SubMapTransition>>();

        public void RegisterTransition(Type type)
        {
            var attr = type.GetCustomAttribute<TransitionAttribute>();
            if (!type.IsSubclassOf(typeof (SubMapTransition)) || attr == null)
                throw new Exception(string.Format("Type {0} is not valid for a transition, it must inherit SubMapTransition and have a TransitionAttribute attribute", type.Name));

            if (m_transitionsCreators.ContainsKey(attr.Identifier))
                throw new Exception(string.Format("Cannot register transition type {0} : Identifier {1} already used by transition class {2}",
                                                  type.Name, attr.Identifier, m_transitionsCreators[attr.Identifier]));

            ConstructorInfo ctor = type.GetConstructor(new Type[0]);
            var del = ctor.CreateDelegate<Func<SubMapTransition>>();

            m_transitionsCreators.Add(attr.Identifier, del);
        }

        public void SerializeTransition(BinaryWriter writer, SubMapTransition transition)
        {
            Type type = transition.GetType();
            var attr = type.GetCustomAttribute<TransitionAttribute>();

            if (attr == null)
                throw new Exception("TransitionAttribute not found");

            if (!m_transitionsCreators.ContainsKey(attr.Identifier))
                RegisterTransition(type);

            writer.Write(attr.Identifier);
            transition.Serialize(writer);
        }

        public SubMapTransition DeserializeTransition(BinaryReader reader)
        {
            int identifer = reader.ReadInt32();

            if (!m_transitionsCreators.ContainsKey(identifer))
                throw new Exception(string.Format("Transition with identifier {0} not found", identifer));

            SubMapTransition transition = m_transitionsCreators[identifer]();

            transition.Deserialize(reader);

            return transition;
        }
    }
}