#region License GNU GPL
// AutoFight.cs
// 
// Copyright (C) 2012, 2013 - BehaviorIsManaged
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

// Author : FastFrench - antispam@laposte.net
#endregion
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace FFFightPlugin
{
  internal partial class FFight
  {

    [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
    public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
    {
      bot.AddFrame(new FFight(bot, Mode.Follower));
    }

    #region party processing

    /// <summary>
    /// New leader
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyLeaderUpdateMessage))]
    public void HandlePartyLeaderUpdateMessage(Bot bot, PartyLeaderUpdateMessage message)
    {
      if (Party == null) return;
      PartyLeaderId = message.partyLeaderId;
    }

    // Remove a Member from the party
    [MessageHandler(typeof(PartyMemberRemoveMessage))]
    public void HandlePartyMemberRemoveMessage(Bot bot, PartyMemberRemoveMessage message)
    {
      if (Party == null) return;
      Party = Party.Where(member => member.id != message.leavingPlayerId).ToList();
      if (message.leavingPlayerId == PartyLeaderId)
        PartyLeaderId = null;
    }

    /// <summary>
    /// Party deletion
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyDeletedMessage))]
    public void HandlePartyDeletedMessage(Bot bot, PartyDeletedMessage message)
    {
      Party = null;
      PartyLeaderId = null;
    }

    /// <summary>
    /// We just joined a party.
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyJoinMessage))]
    public void HandlePartyJoinMessage(Bot bot, PartyJoinMessage message)
    {
      Party = new List<PartyMemberInformations>();
      foreach (PartyMemberInformations member in message.members)
        Party.Add(member);
      PartyId = message.partyId;
      PartyLeaderId = message.partyLeaderId;
    }

    /// <summary>
    /// A new member just joined the party
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyNewMemberMessage))]
    public void HandlePartyNewMemberMessage(Bot bot, PartyNewMemberMessage message)
    {
      if (Party == null)
        Party = new List<PartyMemberInformations>();
      if (!Party.Any(member => member.id == message.memberInformations.id))
        Party.Add(message.memberInformations);
    }

    /// <summary>
    /// We received an invitation to join a party : if it comes from a character played by this plugin, then accept.
    /// Otherwise, do nothing : let the possibility to manually accept, or refuse. 
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyInvitationMessage))]
    public void HandlePartyInvitationMessage(Bot bot, PartyInvitationMessage message)
    {
      foreach (FFight ffight in GetOtherFFights())
        if (ffight.Character.Id == message.fromId)
                    Bot.SendToServer(new PartyAcceptInvitationMessage(message.partyId), 1000);
      //PartyRefuseInvitationMessage
    }


    /// <summary>
    /// If a fight is started with another member of the party on the same map, then join
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    [MessageHandler(typeof(PartyMemberInFightMessage))]
    public void HandlePartyMemberInFightMessage(Bot bot, PartyMemberInFightMessage message)
    {
      JoinFight(message, 14);
    }

    #endregion Party

        #region movement
    short LastLeaderCell;
    [MessageHandler(typeof(GameMapMovementMessage))]
    public void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
    {
      if (message.actorId == PartyLeaderId && Character.Id != message.actorId)
      {
        //foreach (FFight ffight in GetOtherFFights().Where(plug => Party != null && Party.Any(member => member.id == plug.Character.Id)))
        if (message.keyMovements.Length > 0)
                {
                    LastLeaderCell = message.keyMovements.Last();
                    Character.SendInformation("Follow leader in {0}s : {1} => 1 cell from {2}", (500.0m * Id) / 1000.0m, Character.Cell, Character.Map.Cells[message.keyMovements.Last()]);
                    Character.MoveIfNeededThenAction(message.keyMovements.Last(), null, 500 * Id, 1, true);                    
                }
      }
    }

    [MessageHandler(typeof(ChangeMapMessage))]
    public void HandleChangeMapMessage(Bot bot, ChangeMapMessage message)
    {
      if (Character.Id == PartyLeaderId) // I'm the leader => ask all other characters to follow me on the new map
      {
        LastLeaderCell = Character.Cell.Id; // Select all ffight plugins within the party
                int previousmapId = Character.Map.Id;
                foreach (FFight ffight in GetOtherFFights(true, true))
                    ffight.ComeOnMyMap(previousmapId, LastLeaderCell, message.mapId);
            }
        }

        [MessageHandler(typeof(PartyUpdateLightMessage))]
        public void HandlePartyUpdateLightMessage(Bot bot, PartyUpdateLightMessage message)
        {
            if (Party == null) return;
            var member = Party.Find(mem => mem.id == message.id);
            if (member != null)
            {
                member.lifePoints = message.lifePoints;
                member.maxLifePoints = message.maxLifePoints;
            }
        }

        [MessageHandler(typeof(InteractiveUseRequestMessage))]
        public void HandleInteractiveUseRequestMessage(Bot bot, InteractiveUseRequestMessage message)
        {
            if (PartyLeaderId == CharacterId)
                foreach (FFight ffight in GetOtherFFights(true, true)) // All member of the party on the same map
                {
                    //if (Character.Cell.Id != ffight.Character.Cell.Id)
                    ffight.Bot.Character.MoveIfNeededThenAction(Character.Cell.Id, () => ffight.Bot.SendToServer(new InteractiveUseRequestMessage(message.elemId, message.skillInstanceUid), 2500 + 500 * ffight.Id), 500 * ffight.Id, 0);
                    //ffight.Bot.SendToServer(new InteractiveUseRequestMessage(message.elemId, message.skillInstanceUid), 2500 + 500 * ffight.Id);
      }
    }


        [MessageHandler(typeof(TeleportRequestMessage))]
        public void HandleTeleportRequestMessage(Bot bot, TeleportRequestMessage message)
        {
            if (PartyLeaderId == CharacterId)
                foreach (FFight ffight in GetOtherFFights(true, true)) // All member of the party on the same map
                    ffight.Bot.SendToServer(new TeleportRequestMessage(message.teleporterType, message.mapId), 2500 + 500 * ffight.Id);
        }

        #endregion movement

    [MessageHandler(typeof(CharacterExperienceGainMessage))]
    public void HandleCharacterExperienceGainMessage(Bot bot, CharacterExperienceGainMessage message)
    {
            if (bot == null || bot.Character == null)
      {
        logger.Error("Fight is not properly initialized.");
        return; // Can't handle the message
      }
      settings.XPDone += (int)(message.experienceCharacter);
      if (message.experienceCharacter > 0 && bot.Character.Stats.Health > 0) settings.FightWin++; // Todo : use proper message, this way is wrong as one can get xp when losing fight. 
    }
    [MessageHandler(typeof(GameActionFightDeathMessage))]
    public void HandleGameActionFightDeathMessage(Bot bot, GameActionFightDeathMessage message)
    {
      if (bot == null || bot.Character == null || bot.Character.Fight == null)
      {
        logger.Error("Fight is not properly initialized.");
        return; // Can't handle the message
      }
      var fighter = Fighter.Fight.GetActor(message.targetId);
      if (fighter != null)
        if (fighter.Team.Id != Fighter.Team.Id)
          settings.MobKilled++;
        else
          if (fighter == Fighter)
            settings.FightLost++;
        }

        /// <summary>
        /// When the leader is NOT a FFbot, then try to guess in what map he moved
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        [MessageHandler(typeof(GameContextRemoveElementMessage))]
        public void HandleGameContextRemoveElementMessage(Bot bot, GameContextRemoveElementMessage message)
        {
            if (message.id == PartyLeaderId && CharacterId != PartyLeaderId)
            {
                foreach (FFight ffight in GetOtherFFights(true))
                    if (ffight.CharacterId == PartyLeaderId)
                        return; // we found a FFight playing the leader => we skip this message
                int NextMap = Character.GetMapLinkedToCell(LastLeaderCell);
                if (NextMap == -1)
                    logger.Error("Can't follow the leader, no map linked from last cell {0}.", LastLeaderCell);
                else
                    ComeOnMyMap(Character.Map.Id, LastLeaderCell, NextMap);
            }
    }
  }
}
