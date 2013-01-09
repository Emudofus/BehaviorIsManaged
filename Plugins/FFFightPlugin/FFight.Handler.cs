using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors;
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
          Bot.CallDelayed(1000, () => Bot.SendToServer(new PartyAcceptInvitationMessage(message.partyId)));
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

    [MessageHandler(typeof(PartyUpdateLightMessage))]
    public void HandlePartyUpdateLightMessage(Bot bot, PartyUpdateLightMessage message)
    {
      var member = Party.Find(mem => mem.id == message.id);
      if (member != null)
      {
        member.lifePoints = message.lifePoints;
        member.maxLifePoints = message.maxLifePoints;
      }
    }

    #endregion Party

    short LastLeaderCell;
    [MessageHandler(typeof(GameMapMovementMessage))]
    public void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
    {
      if (message.actorId == PartyLeaderId && Character.Id != message.actorId)
      {
        //LastLeaderCell = message.keyMovements.Last();
        //foreach (FFight ffight in GetOtherFFights().Where(plug => Party != null && Party.Any(member => member.id == plug.Character.Id)))
        if (message.keyMovements.Length > 0)
          Character.Move(message.keyMovements.Last(), null, 1, true);
      }
    }

    [MessageHandler(typeof(ChangeMapMessage))]
    public void HandleChangeMapMessage(Bot bot, ChangeMapMessage message)
    {
      if (Character.Id == PartyLeaderId) // I'm the leader => ask all other characters to follow me on the new map
      {
        LastLeaderCell = Character.Cell.Id; // Select all ffight plugins within the party
        foreach (FFight ffight in GetOtherFFights().Where(plug => Party != null && Party.Any(member => member.id == plug.Character.Id)))
          ffight.Character.ChangeMap(Character.Map.Id, Character.Cell.Id, message.mapId, 10);
      }
    }

    [MessageHandler(typeof(CharacterExperienceGainMessage))]
    public void HandleCharacterExperienceGainMessage(Bot bot, CharacterExperienceGainMessage message)
    {
      if (bot == null || bot.Character == null || bot.Character.Fight == null)
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
      logger.Debug(string.Format("{0} is dead (FF)", fighter));
      fighter.IsAlive = false;
    }
  }
}
