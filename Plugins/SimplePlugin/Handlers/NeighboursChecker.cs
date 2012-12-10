#region License GNU GPL
// NeighboursChecker.cs
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

using BiM.Behaviors;
using BiM.Behaviors.Data.Maps;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Data;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public static class NeighboursCheckerRegister
    {
        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.AddFrame(new NeighboursChecker(bot));
        }
    }

    public class NeighboursChecker : Frame<NeighboursChecker>
    {
        private MapPositionData m_lastMapPos;
        private Path m_lastPath;

        public NeighboursChecker(Bot bot)
            : base(bot)
        {
            bot.Character.StopMoving += OnStopMoving;
        }

        private void OnStopMoving(ContextActor actor, MovementBehavior movement, bool canceled, bool refused)
        {
            m_lastPath = movement.MovementPath;
        }

        [MessageHandler(typeof (ChangeMapMessage))]
        public void HandleChangeMapMessage(Bot bot, ChangeMapMessage message)
        {
            MapPositionData mapPosition = MapsPositionManager.Instance.GetMapPosition(message.mapId);
            if (m_lastPath != null && m_lastMapPos != null)
            {
                MapNeighbour direction = Map.GetDirectionOfTransitionCell(m_lastPath.End);
                int? neighbour = m_lastMapPos.GetNeighbourId(direction);

                if (neighbour == null)
                    bot.Character.SendWarning("The actual map ({0}) is not the {1} neighbour of the previous map {2} (NOT FOUND)", message.mapId, direction, m_lastMapPos.MapId);
                else if (neighbour != message.mapId)
                    bot.Character.SendWarning("The actual map ({0}) is not the {1} neighbour of the previous map {2}" +
                                              "(MISMATCH attempt : {3})", message.mapId, direction, m_lastMapPos.MapId, neighbour);
                else
                {
                    bot.Character.SendDebug("You came from {0}", direction);
                }
            }

            m_lastMapPos = mapPosition;
        }


    }
}