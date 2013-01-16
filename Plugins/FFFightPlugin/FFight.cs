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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.Spells;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;
using BiM.Core.Messages;
using BiM.Core.Threading;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace zFFFightPlugin
{
  internal static class WelcomeMessageRegister
  {
    static void SetFrame(Bot bot, Mode mode)
    {
      if (bot.HasFrame<FFight>())
      {
        bot.Character.SendInformation("Set existing FFight to {0} mode", mode);
        bot.GetFrame<FFight>().Mode = mode;
      }
      else
        if (bot.AddFrame(new FFight(bot, mode)))
          bot.Character.SendInformation("Experimental AI fight started in {0} mode", mode);
        else
          bot.Character.SendInformation("Failed to start a new FFight frame !");

    }

    [MessageHandler(typeof(ChatClientMultiMessage))]
    public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
    {
      if (message.content == ".help")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        bot.Character.SendInformation(".dump spells");
        bot.Character.SendInformation(".dump all");
        bot.Character.SendInformation(".FF ? => Show all plugins running");
        bot.Character.SendInformation(".FF on or .FF auto => Starts experimental AI fight in automatic mode");
        bot.Character.SendInformation(".FF fol => Put the experimental AI fight in follower mode");
        bot.Character.SendInformation(".FF gat => Put the experimental AI fight in gathering mode (not implemented yet)");
        bot.Character.SendInformation(".FF off or .FF man => Disable experimental AI fight (manual mode)");
        bot.Character.SendInformation(".FF stats => gives some stats");
        bot.Character.SendInformation(".FF all => Starts experimental AI fight on all Bots");
        bot.Character.SendInformation(".FF /all => Stops experimental AI fight on all Bots");
        bot.Character.SendInformation("message <Level> => Filters the messages received from the bot to the Dofus client. <Level> is a bit field (4 bits, so values range from 0 to 7)");

      }
      else if (message.content == ".dump all")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        XmlDumper.DumpAll();
      }
      else if (message.content == ".dump spells")
      {
        message.BlockNetworkSend();// do not send this message to the server                
        XmlDumper.SpellsDumper("_Spells.xml");
      }

      if (message.content.StartsWith(".FF"))
      {
        message.BlockNetworkSend();// do not send this message to the server                
        if (message.content == ".FF ?")
        {
          int BotNo = 0;
          int FrameNo = 0;
          foreach (Bot subBot in BotManager.Instance.Bots)
          {
            BotNo++;
            foreach (IFrame frame in subBot.Frames)
            {
              FrameNo++;
              bot.Character.SendInformation("Bot {0} ({3}) Frame {1} : {2}", BotNo, FrameNo, frame.GetType().Name, subBot.Character);
            }
          }

        }
        else
          if (message.content == ".FF all")
          {
            bot.Character.SendInformation("Experimental AI fight started for all played characters (set to follower mode for non-leaders of parties)");
            foreach (Bot subBot in BotManager.Instance.Bots)
            {
              if (subBot.AddFrame(new FFight(subBot)))
              {
                subBot.Character.SendInformation("Experimental AI fight started");
                bot.Character.SendInformation("FF started for {0}", bot.Character);
              }
              else
              {
                subBot.Character.SendInformation("Can't start FF");
                bot.Character.SendInformation("Can't start FF for {0}", bot.Character);
              }

            }
          }
          else if (message.content == ".FF /all")
          {
            bot.Character.SendInformation("Experimental AI fight stopped for all played characters (set to manual mode)");
            foreach (Bot subBot in BotManager.Instance.Bots)
            {
              if (subBot.RemoveFrame<FFight>())
              {
                subBot.Character.SendInformation("Experimental AI fight stopped");
                bot.Character.SendInformation("FF stopped for {0}", bot.Character);
              }
              else
              {
                subBot.Character.SendInformation("Failed to stop Experimental AI fight. Probably not running ?");
                bot.Character.SendInformation("Can't stop FF for {0}", bot.Character);
              }
            }
          }
          else if (message.content.StartsWith(".FF fol", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Follower);
          else if (message.content.StartsWith(".FF gat", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Ressources);
          else if (message.content.StartsWith(".FF auto", StringComparison.InvariantCultureIgnoreCase) || message.content.StartsWith(".FF on", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.AutomaticFight);
          else if (message.content.StartsWith(".FF man", StringComparison.InvariantCultureIgnoreCase) || message.content.StartsWith(".FF off", StringComparison.InvariantCultureIgnoreCase))
            SetFrame(bot, Mode.Manual);
          else if (message.content == ".FF stats")
          {
            if (!bot.HasFrame<FFight>())
            {
              bot.Character.SendInformation("Experimental AI fight is NOT running");
            }
            else
            {
              FFight fightBot = bot.GetFrame<FFight>();
              bot.Character.SendInformation("Experimental AI fight IS running in mode {0}", fightBot.Mode);
              fightBot.Dump();
            }
          }
      }

      PlayedCharacter PC = bot.Character;
      if (message.content == "?")
      {
        message.BlockNetworkSend();// do not send this message to the server
        PC.SendInformation(String.Format("Position : NF{0} - F{1}", PC.Cell, PC.Fighter != null ? PC.Fighter.Cell.ToString() : "N/A"));
        /*PC.ResetCellsHighlight();
        if (PC.Fighter != null)
        {
            PC.HighlightCells(PC.Fight.BlueTeam.FightersAlive.Select(fighter => fighter.Cell), Color.Blue);
            PC.HighlightCells(PC.Fight.RedTeam.FightersAlive.Select(fighter => fighter.Cell), Color.Red);
            PC.HighlightCell(PC.Fighter.Cell, Color.Pink);
        }
        else
            PC.HighlightCell(PC.Cell, Color.Pink);*/
      }
      if (message.content.StartsWith("message"))
      {
        message.BlockNetworkSend();// do not send this message to the server
        string sdbgLevel = message.content.Replace("message", "").Trim();
        PlayedCharacter.MessageLevel dbgLevel = PC.InformationLevel;
        if (PlayedCharacter.MessageLevel.TryParse(sdbgLevel, out dbgLevel))
        {
          PC.SendMessage(String.Format("MessageLevel was {0}, it is now {1}", PC.InformationLevel, dbgLevel));
          PC.InformationLevel = dbgLevel;
        }
        else
          PC.SendMessage(String.Format("MessageLevel is {0}", PC.InformationLevel));
      }

    }
  }

  public enum Mode { AutomaticFight, Follower, Ressources, Manual }
  internal partial class FFight : Frame<FFight>
  {
    //private WindowDetector wndProcessor;
    private static int FFightCount = 0;

        public int Id { get; private set; }
        public int CharacterId { get { if (Character != null) return Character.Id; return 0; } }

    #region Setting stats
    private Stopwatch sitTimer;
    private Stopwatch fightTimer;
    private Stopwatch botTimer;
    private int startingElapsSecondes;
    #endregion Setting stats

    private SimplerTimer _checkTimer;
    private bool _sit = false;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    //private ContextActor.MoveStopHandler _stopMovingDelegate;
    private Fighter _currentTarget;
    private LOSMap _losMapF, _losMapRP;
    private BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder _pathFinder;
    private string _lastPathDetail;
    private SpellTarget spellTarget;
    private bool isMoving;
    FFSettings settings;
    public Mode Mode { get; set; }
    public bool LeaderMode { get; set; }
    public FFight LeaderBot { get; set; }

    public int? PartyLeaderId { get; private set; }
    public List<PartyMemberInformations> Party { get; set; }
    public int PartyId { get; private set; }

        public PlayedCharacter Character { get { return Bot.Character; } }
    public PlayedFighter Fighter { get; private set; }

    public FFight(Bot bot, Mode mode = Mode.AutomaticFight)
      : base(bot)
    {
            Id = ++FFightCount;
            if (Character == null)
      {
        OnDetached();
        return;
      }
      Mode = mode;
      #region Settings and logs
      Directory.CreateDirectory("Spells");
      bot.CallDelayed(5000, () => File.WriteAllText("spells/" + Character.Name + ".spells.txt", Character.SpellsBook.GetFullDetail()));
      settings = Bot.Settings.GetOrAddEntry<FFSettings>();
      if (settings.Restarts == 0)
        settings.Init(bot.Character);
      Party = null;
      botTimer = new Stopwatch();
      botTimer.Start();
      startingElapsSecondes = settings.BotElapsedSeconds;
      fightTimer = new Stopwatch();
      sitTimer = new Stopwatch();
      settings.Restarts++;
      #endregion

      isMoving = false;
      Character.FightJoined += OnFightJoined;
      Character.FightLeft += OnFightLeft;
      Character.MapJoined += OnMapJoined;
      _losMapRP = new LOSMap(bot.Character.Map);

      TeamMeUp();
      //bot.CallDelayed(2000, () => wndProcessor = bot.GetFrame<WindowDetector>());
            if (Character.IsFighting()) // reconnect
                OnFightJoined(Character, bot.Character.Fight);
            else if (Character.Map != null)
                OnMapJoined(Character, Character.Map);
    }

    /// <summary>
    /// Get All other FFight plugins within the same server
    /// </summary>
    /// <returns></returns>
        IEnumerable<FFight> GetOtherFFights(bool withinSameParty = false, bool withinSameMap = false)
    {
            if (Character != null)
      foreach (Bot subBot in BotManager.Instance.Bots)
                    if (subBot != Bot && subBot.Character != null && subBot.ClientInformations.SelectedServer.id == Bot.ClientInformations.SelectedServer.id && subBot.Character.Id != CharacterId)
          if (subBot.HasFrame<FFight>())
            if (!withinSameParty || (Party != null && Party.Any(member => member.id == subBot.Character.Id)))
                                if (!withinSameMap || subBot.Character.Map.Id == Character.Map.Id)
              yield return subBot.GetFrame<FFight>();
    }

    private void TeamMeUp()
    {
      // Looking for other bots on the same server
      foreach (FFight ffight in GetOtherFFights()) // The first one we find on the same server, and not gathered mode
        if (ffight.Mode != Mode.Ressources)
        {
          LeaderBot = ffight;
          ffight.Bot.SendToServer(new PartyInvitationRequestMessage(Character.Name));
          return;
        }
    }


    private void OnMapJoined(PlayedCharacter character, Map map)
    {

      _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(map, false);
      _losMapRP = new LOSMap(map);
      if (_checkTimer != null)
      {
        _checkTimer.Dispose();
        _checkTimer = null;
      }
            _checkTimer = Bot.CallPeriodically(4 * 1000, CheckMonsters);
    }

    public void Sit()
    {
      if (!_sit)
      {
        sitTimer.Restart();

        Bot.SendToServer(new EmotePlayRequestMessage(1));
        //Bot.Character.Say("/sit");

        _sit = true;

                Character.StartMoving += StandUp;

      }
    }

    private void StandUp(ContextActor sender, MovementBehavior path)
    {
      Debug.Assert(sitTimer.IsRunning);
      sitTimer.Stop();
      settings.BotHealingElapsedSeconds += (int)(sitTimer.ElapsedMilliseconds / 1000);

      _sit = false;
            Character.StartMoving -= StandUp;
    }

    int MapMoveFailedCount = 0;
    Stopwatch waitingOtherTimer = new Stopwatch();
    private void CheckMonsters()
    {
      //startingElapsSecondes = settings.BotElapsedSeconds;
      settings.BotElapsedSeconds = startingElapsSecondes + (int)(botTimer.ElapsedMilliseconds / 1000);
      settings.LastActivityDate = DateTime.Now;
      Bot.SaveSettings();
            if (Character.Stats.Health * 10 < Character.Stats.MaxHealth * 9) // under 90% full health
      {
        if (!_sit)
        {
                    Character.SendWarning("Character health too low : {0}/{1} => sit", Character.Stats.Health, Character.Stats.MaxHealth);

          Bot.CallDelayed(500, Sit);
        }

        return;
      }
      if (Mode != Mode.AutomaticFight && Mode != Mode.Ressources)
        return; // 

      if (settings.MaxPower < 0.1)
        settings.MaxPower = 0.99;
      int level = (int)(Character.Level * settings.MaxPower);

            if (PartyLeaderId == CharacterId) // Leader mode : wait all members of the party are in the same map
      {
        if (!Party.All(member => Character.Map.Actors.Any(actor => actor.Id == member.id)))
        {
          if (!waitingOtherTimer.IsRunning)
            waitingOtherTimer.Start();
          if (waitingOtherTimer.ElapsedMilliseconds > 60000) // Over 1 minute
          {
            Character.SendInformation("Leader : waiting for over 1 minute for other members of the party {0} => come back on previous map", string.Join(",", Party.Where(member => !Character.Map.Actors.Any(actor => actor.Id == member.id)).Select(member => string.Format("{0} ({1}/{2})", member.name, member.lifePoints, member.maxLifePoints))));
            if (Character.PreviousMap.HasValue)
              Character.ChangeMap(Character.Map.Id, Character.Cell.Id, Character.PreviousMap.Value, 10);
            waitingOtherTimer.Reset();
          }
          else
            Character.SendInformation("Leader : waiting for other members of the party {0}", string.Join(",", Party.Where(member => !Character.Map.Actors.Any(actor => actor.Id == member.id)).Select(member => string.Format("{0} ({1}/{2})", member.name, member.lifePoints, member.maxLifePoints))));
          return;
        }
        level = Party.Sum(member => member.level);
        Character.SendInformation("Leader : looking for monsters around team level = {0}", level);
      }
      waitingOtherTimer.Reset();
      int levelMax = Math.Max(Math.Min(level + 10, level * 2), (int)(level * 1.2));
      int levelMin = level / 2;

      var reachableCells = _pathFinder.FindConnectedCells(Character.Cell, false, false);
      var monster = Character.Map.Actors.OfType<GroupMonster>()
          .Where(x => x.Level <= levelMax && x.Level >= levelMin && (reachableCells.Any(cell => cell.Id == x.Cell.Id)))
          .OrderBy(x => Math.Abs(x.Level - level)).FirstOrDefault(); // As close as possible from character's level

      if (monster != null && MapMoveFailedCount < 5)
      {
        //Bot.Character.SendMessage(String.Format("Trying to start a fight with group lv {0}, stCell {1}, leader {2} lv {3}", monster.Level, monster.Cell, monster.Leader.Name, monster.Leader.Grade.grade));
        if (!Character.TryStartFightWith(monster, _pathFinder))
          MapMoveFailedCount++;
        else
          MapMoveFailedCount = 0;
        //Move(Bot.Character.Cell, monster.Cell, false, -1);
      }
      else
        if (!Character.IsMoving())
          Bot.CallDelayed(500, () => ChangeMap());
    }

    public void ChangeMap(MapNeighbour neighbour = MapNeighbour.Any)
    {
            MapNeighbour choosenNeighbour = Character.ChangeMap(neighbour);
      if (choosenNeighbour == MapNeighbour.None) return; // Failed
      MapMoveFailedCount = 0;
            if (otherFFights != null && Character != null && Character.Movement != null && choosenNeighbour != MapNeighbour.None)
        foreach (FFight ffight in otherFFights)
                    ffight.ComeOnMyMap(Character.Movement.EndCell.Id, Character.NextMap, choosenNeighbour);
    }

    List<Bot> otherBots;
    List<FFight> otherFFights;

    public bool JoinAsFollower(Fight otherFight)
    {
        return false;
    }

        public void ComeOnMyMap(int srcMap, short destCell, int dstMap)
        {
            Character.SendInformation("Leader call me on the map {0}, through cell {1}. I'm on cell {2} in map {3}", dstMap, destCell, Character.Cell, Character.Map);
            Bot.CallDelayed(1000 + 500 * Id, () => Character.ChangeMap(srcMap, destCell, dstMap, 10));
        }

    public bool ComeOnMyMap(short cellId, int? mapId, MapNeighbour neighbour)
    {
      return false;
    }

    private void OnFightJoined(PlayedCharacter character, Fight fight)
    {
      otherBots = new List<Bot>();
      otherFFights = new List<FFight>();
      Fighter = character.Fighter;


      fightTimer.Restart();
      settings.FightStarted++;
      settings.LastFightDate = DateTime.Now;
      if (_checkTimer != null)
      {
        _checkTimer.Dispose();
        _checkTimer = null;
      }
      _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(fight, true);
      _losMapF = new LOSMap(fight);

      Fighter = character.Fighter;
      Fighter.TurnStarted += OnTurnStarted;
      fight.StateChanged += OnStateChanged;
      //Fighter.SequenceEnded += OnSequenceEnd;
      Fighter.Acknowledge += OnAcknowledgement;
    }

    private void OnStateChanged(Fight fight, FightPhase phase)
    {
      if (phase == FightPhase.Placement)
        Bot.CallDelayed(2000, Placement);
    }

    void FindBestAttack()
    {
      bestAttackSpell = Fighter.FindMostEfficientAttackSpell();
      if (bestAttackSpell == null)
        bestDistance = 1;
      bestDistance = Fighter.GetRealSpellRange(bestAttackSpell.LevelTemplate);
    }

    void Placement()
    {
      FindBestAttack();
      if (Mode == Mode.Manual) return;
      int delay = 50;
            if (PartyLeaderId != null && PartyLeaderId != CharacterId)
      {
        if (Character.Breed.Id == (int)BreedEnum.Eniripsa)
          delay = 6000;
        delay = 2000;
      }

      if (settings.IsInvoker)
        Bot.CallDelayed(delay, FindFarPlacement);
      else if (settings.IsHealer)
        Bot.CallDelayed(delay, PlaceCloseToFriendsButFarFromEnemies);
      else
        Bot.CallDelayed(delay, FindOptimalPlacement);
    }

    private void OnFightLeft(PlayedCharacter character, Fight fight)
    {
      Debug.Assert(character == Character);
      Character.ResetCellsHighlight();
      Debug.Assert(fightTimer.IsRunning);
      fightTimer.Stop();
      settings.BotFightingElapsedSecond += (int)(fightTimer.ElapsedMilliseconds / 1000);

      _lastPathDetail = null;
      _losMapF = null;

      if (Fighter != null)
      {
        //Fighter.SequenceEnded -= OnSequenceEnd;
        Fighter.Acknowledge -= OnAcknowledgement;
        Fighter.TurnStarted -= OnTurnStarted;
      }
      /*if (_stopMovingDelegate != null)
      {
          if (Fighter != null) Fighter.StopMoving -= _stopMovingDelegate;
          _stopMovingDelegate = null;
      }*/
      fight.StateChanged -= OnStateChanged;
      Fighter.Dispose();
      Fighter = null;
      _pathFinder = new BiM.Behaviors.Game.World.Pathfinding.FFPathFinding.PathFinder(character.Map, false);
    }

    //bool DoNotMoveAgain;
    int bestDistance = 4;
    Spell bestAttackSpell;
    int _IAStep;

    private void OnTurnStarted(Fighter fighter)
    {
      //if (wndProcessor!=null) 
      //    wndProcessor.CloseWindow(50+50*FFightNb);
      Debug.Assert(fighter == Fighter);
      if (Mode == Mode.Manual) return;
      _losMapF.UpdateTargetCell(null, true, true);
      //DoNotMoveAgain = false;            
      Bot.CallDelayed(500, StartAI);
      _attackDone = false;
      _IAStep = 0;
      spellTarget = null;
      isMoving = false;
    }

    bool _attackDone = false;


    private void StartAI()
    {
            Character.ResetCellsHighlight();
      isMoving = false;
      spellTarget = null;
      _lastPathDetail = null;
      _lastActionDesc = string.Empty;

      if (Fighter == null) return;

      _currentTarget = GetNearestEnemy(); // Better nearest here than weakest. Even if the weakest should be the nearest, it's not a sure thing. 

      if (_currentTarget == null)
      {
                Character.SendWarning("No target => no attack");
                //PassTurn();
                //return;
      }

            Character.SendDebug("StartAI AP:{0}, MP:{1}, Pos:{2}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, Fighter.Cell);


      //_isLastAction = false;
      // Don't use invocs against mutants
      while (true)
      {
        switch (_IAStep++)
        {
          case 0:
            if (settings.IsInvoker && Fighter.Fight.BlueTeam.TeamType != BiM.Protocol.Enums.TeamTypeEnum.TEAM_TYPE_MUTANT && Fighter.Fight.RedTeam.TeamType != BiM.Protocol.Enums.TeamTypeEnum.TEAM_TYPE_MUTANT)
            {
              if (Fighter.CanSummon())
              {
                Fighter.Character.SendDebug("Trying to cast an invoc {0}/{1}", Fighter.SummonedCount, Fighter.PCStats.SummonLimit);
                if (CastInvocation())
                {
                  return;
                }
              }
              Fighter.Character.SendDebug("Can't invoc a creature yet. {0}/{1}", Fighter.SummonedCount, Fighter.PCStats.SummonLimit);
              /*if (_IAStep == 1 && Cast(Spell.SpellCategory.Buff))
              {
                  _IAStep = 2;
                  return; // trying to improve invoc count                
              }*/
            }
            //else
            //    if (settings.IsHealer)
            //        if (Cast(Spell.SpellCategory.Healing))
            //            return;
            break;
          case 1:
            if (settings.FavoredBoostSpells != null && settings.FavoredBoostSpells.Count > 0 && Cast(settings.FavoredBoostSpells)) return;
            break;
          case 2:
                        if (_currentTarget == null) // no enemy can be seen
                        {
                            if (Cast(Spell.SpellCategory.Buff | Spell.SpellCategory.Healing))
                                return;
                        }
                        else
            if (Cast(Spell.SpellCategory.All ^ Spell.SpellCategory.Invocation))
            {
              if (spellTarget != null && spellTarget.Spell != null)
                if ((spellTarget.Spell.Categories & Spell.SpellCategory.Damages) > 0) _attackDone = true;
              return;
            }
            break;
          case 3:
                        if (!settings.IsInvoker && Fighter.CanSummon() && CastInvocation()) return;
            break;
                    case 4: // No spell can be cast
            if (Fighter.Stats.CurrentMP <= 0)
            {
              Bot.CallDelayed(250, PassTurn);
              return;
            }
            break;
                    case 5: // No other spells can be cast (out of AP) and at least one attack succeeded => move away and pass the turn
            if (settings.IsInvoker || _attackDone)
            {
                            Character.SendDebug("StartAI No spell castable, moving away {0}pm", Fighter.Stats.CurrentMP);
                            if (_currentTarget != null)
              MoveFar(false);
              return;
            }
            break;
                    case 6: // If no spell is at range, then try to come closer (best spell distance) and try again. No need to be cautious here : we can't cast anything else
                        if (_currentTarget != null)
            MoveNear();
            return;
          default:
            return;
        }
      }
    }

    private void DisplaySpellAvailability()
    {
            foreach (Spell spell in Character.SpellsBook.Spells)
      {
        if (!spell.IsAvailable(Fighter.Id, null))
                    Character.SendDebug("{0} is not available : {1}", spell, spell.AvailabilityExplainString(Fighter.Id));
      }
    }

    private string _explain;

    bool lastActionFailed;
    // It's valid here to have uninitialized spellTarget => returns false quietly in that case. 
    bool TryCast(string explain)
    {
      lastActionFailed = false;
      _explain = explain;
      if (spellTarget == null || spellTarget.Spell == null || spellTarget.FromCell == null || spellTarget.TargetCell == null || spellTarget.Efficiency <= 0 || spellTarget.cast == true)
        return false;
      ComeAtSpellRangeThenCast();
      return !lastActionFailed;
    }

    void ComeAtSpellRangeThenCast()
    {
      lastActionFailed = false;

      isMoving = false;
      if (spellTarget == null || spellTarget.Spell == null || spellTarget.FromCell == null || spellTarget.TargetCell == null || spellTarget.cast == true)
      {
                Character.SendError("ComeAtSpellRangeThenCast <{0}> can't be used : spellTarget is not properly initialized (or already cast) => PassTurn", _explain);
        //PassTurn();//PassTurn();
        lastActionFailed = true;
        return;
      }
      if (Fighter.Cell.Id == spellTarget.FromCell.Id) // No Need to move
      {
        lastActionFailed = !CastSpell();
        return;
      }

      if (!Fighter.Move(spellTarget.FromCell, _pathFinder))
      {
        //DoNotMoveAgain = true;
        Debug.Assert(Fighter.Cell.Id != spellTarget.FromCell.Id);
                Character.SendError("Failed to move from {0} to {1} to cast {2} => pass", Fighter.Cell, spellTarget.FromCell, spellTarget.Spell);
        //PassTurn();
        lastActionFailed = true;
        return;
      }
      else
      {
        if (Fighter.Movement == null)
        {
          _lastPathDetail = "???";
                    Character.SendWarning("Failed to move to cast {0} ?", spellTarget.Spell);
        }
        else
        {
          isMoving = true;
          _lastPathDetail = Fighter.MovementPath.ToString();
                    Character.SendDebug("Moving to cast {0} : pos {1} => {2} [{3}]", spellTarget.Spell, Fighter.Cell, spellTarget.FromCell, Fighter.Movement.MovementPath.ToString());
        }
        _lastActionDesc = "MoveToCast";
        return;
      }
    }

    bool CastSpell()
    {
      if (Fighter.CastSpell(spellTarget.Spell, spellTarget.TargetCell))
      {
                Character.SendInformation("Casting {0} Spell {1} (cat {2}) on [{3}] {4} (search time : {5}ms)", _explain, spellTarget.Spell, spellTarget.Spell.Categories, string.Join<Fighter>(",", Fighter.Fight.GetActorsOnCell(spellTarget.TargetCell)), spellTarget.TargetCell, spellTarget.TimeSpan.TotalMilliseconds);
        _lastActionDesc = String.Format("Casting {0} spell {1}", _explain, spellTarget.Spell);
        //spellTarget = null;
        return true;
      }
            Character.SendError("Failed to cast {0} Spell {1} (cat {2}) on [{3}] {4}", _explain, spellTarget.Spell, spellTarget.Spell.Categories, string.Join<Fighter>(",", Fighter.Fight.GetActorsOnCell(spellTarget.TargetCell)), spellTarget.TargetCell);
      //spellTarget = null;
      //PassTurn();
      return false;
    }

    Cell GetRandomSurroundingFreeCell(Cell center, Cell target, Spell spell)
    {
      int distanceMax = Fighter.GetRealSpellRange(spell.LevelTemplate);
      Lozenge shape = new Lozenge(0, (byte)distanceMax);
      //Bot.Character.SendWarning("Fight.AliveActors:{0} - Fighter.Map.Actors:{1}", string.Join(",", Fighter.Fight.Map.Actors), string.Join(",", Fighter.Map.Actors));
      //Fighter.Fight.AliveActorsCells.Except(Fighter.Map.Actors.Select(actor => actor.Cell));
      IEnumerable<Cell> cells = shape.GetCells(center, Fighter.Map)/*.Except(Fighter.Fight.AliveActorsCells)*/.Where(cell => cell.Walkable && !cell.NonWalkableDuringFight && !Fighter.Fight.IsActorOnCell(cell));
      // Restrict selection as needed for InLine or InDiagonal constrains
      if (spell.LevelTemplate.castInLine || spell.LevelTemplate.castInDiagonal)
        cells = cells.Where(cell => spell.LevelTemplate.castInLine && (center.X == cell.X || center.Y == cell.Y) || spell.LevelTemplate.castInDiagonal && (Math.Abs(center.X - cell.X) == Math.Abs(center.Y - cell.Y)));
      // Restrict selection as needed for LoS constrain
      if (spell.LevelTemplate.castTestLos)
      {
        _losMapF.UpdateTargetCell(Fighter.Cell, true, true);
        cells = _losMapF.GetCellsSeenByTarget(cells);
      }
      //Bot.Character.HighlightCells(cells, Color.AntiqueWhite);
      // Keep the stCell the closest from target
      if (target != null)
        return cells.OrderBy(cell => cell.ManhattanDistanceTo(target)).FirstOrDefault();
      else
        return cells.GetRandom();
    }

    bool CastInvocation()
    {
      foreach (Spell spell in Fighter.GetOrderListOfInvocationSpells())
      {
                Cell dest = GetRandomSurroundingFreeCell(Fighter.Cell, _currentTarget == null ? null : _currentTarget.Cell, spell);
        if (dest != null)
        {
          spellTarget = new SpellTarget(1, Fighter.Cell, dest, spell);
        }
        else
        {
                    Cell whereToMove = Fighter.GetPossibleMoves(true, false, _pathFinder).FirstOrDefault(cell => GetRandomSurroundingFreeCell(cell, _currentTarget == null ? null : _currentTarget.Cell, spell) != null);
          if (whereToMove == null) continue;
          spellTarget = new SpellTarget(1, whereToMove, dest, spell);
        }
        return TryCast("invocation");
      }
      return false;
    }


    bool Cast(Spell.SpellCategory category, bool withWeapon = true)
    {
      if (Fighter.Stats.CurrentAP <= 0) return false;
            spellTarget = Character.SpellsBook.FindBestUsage(Fighter, category, withWeapon);
      return TryCast(category.ToString());
    }

    bool Cast(List<int> spellIds)
    {

      if (Fighter.Stats.CurrentAP <= 0) return false;
            spellTarget = Character.SpellsBook.FindBestUsage(Fighter, spellIds);
      return TryCast(string.Format("Spells {0}", string.Join(",", spellIds)));
    }


    //private bool _isLastAction;
    private void PassTurn()
    {
      //Bot.Character.SendMessage("PassTurn", Color.Pink);
      if (Fighter != null)
      {
        Fighter.PassTurn();

      }
    }

    string _lastActionDesc;
    //private void OnSequenceEnd(Fighter fighter)
    //{
    //Bot.Character.SendMessage(String.Format("OnSequenceEnd AP:{0}, MP:{1}, LastAction:{2} ({3})", Bot.Character.Stats.CurrentAP, Bot.Character.Stats.CurrentMP, _isLastAction, _lastActionDesc), Color.Pink);
    //_lastActionDesc = "OnSequenceEnd";
    //if (!_isLastAction)
    //  Bot.CallDelayed(300, StartAI);
    //else
    //  Bot.CallDelayed(300, PassTurn);
    //}

    private void OnAcknowledgement(bool failed)
    {
      if (Mode == Mode.Manual) return;
      if (_IAStep == 0)
      {
                Character.SendWarning("OnAcknowledgement before any action on the turn => ignored. AP:{0}, MP:{1}. LastAction: {2}, Actors {3} on Cells {4}. Last path {5}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc, string.Join(",", Fighter.Fight.Actors), string.Join(",", Fighter.Fight.Actors.Select(actor => actor.Cell)), _lastPathDetail);
        return;
      }

      bool NoSpellTarget = spellTarget == null || spellTarget.Spell == null || spellTarget.cast == true;


      if (failed == true)
      {
        if (!isMoving && NoSpellTarget)
        {
                    Character.SendError("OnAcknowledgement [{6} - {7}] : Failed, but it is neither a move or a spell ! - {2} FAILED. AP:{0}, MP:{1}. Actors {3} on Cells {4}. Last path {5}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc, string.Join(",", Fighter.Fight.Actors), string.Join(",", Fighter.Fight.Actors.Select(actor => actor.Cell)), _lastPathDetail, Id, Character);
          settings.UnvalidAckFailed++;
        }
        else
          if (isMoving)
          {
            settings.FMovesFailed++;
                        Character.SendError("OnAcknowledgement [{6} - {7}] : {2} FAILED. AP:{0}, MP:{1}. Actors {3} on Cells {4}. Last path {5}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc, string.Join(",", Fighter.Fight.Actors), string.Join(",", Fighter.Fight.Actors.Select(actor => actor.Cell)), _lastPathDetail, Id, Character);
          }
          else
          {
            settings.SpellsFailed++;
            Cell cellTarget = spellTarget.TargetCell;
            Fighter[] targets = cellTarget == null ? new Fighter[0] : Fighter.Fight.GetActorsOnCell(spellTarget.TargetCell);
                        Character.SendError("OnAcknowledgement [{9}{10}] : {2} FAILED. AP : {0}, MP : {1}. {3} casted on {4} on cell {5} : {6} -  Actors {7} on Cells {8}", Character.Stats.CurrentAP, Character.Stats.CurrentMP,
                                _lastActionDesc, spellTarget.Spell, string.Join<Fighter>(",", targets), cellTarget, spellTarget.Spell.AvailabilityExplainString(targets.Select(fighter => fighter.Id).FirstOrDefault()), string.Join(",", Fighter.Fight.Actors), string.Join(",", Fighter.Fight.Actors.Select(actor => actor.Cell)), Id, Character);
            spellTarget.cast = true;
          }
      }
      else
      {
        if (!isMoving && NoSpellTarget)
        {
                    Character.SendError("OnAcknowledgement [{6} - {7}] : Succeeded, but it is neither a move or a spell ! - {2} SUCCESS. AP:{0}, MP:{1}. Actors {3} on Cells {4}. Last path {5}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc, string.Join(",", Fighter.Fight.Actors), string.Join(",", Fighter.Fight.Actors.Select(actor => actor.Cell)), _lastPathDetail, Id, Character);
          settings.UnvalidAckSucceeded++;
        }
        else
          if (isMoving)
          {
            settings.FMovesSucceded++;
                        Character.SendDebug("Success on Moving \"{2}\"  - OnAcknowledgement AP:{0}, MP:{1}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc);
          }
          else
          {
            settings.SpellsSucceded++;
                        Character.SendDebug("Success on Cast \"{2}\"  - OnAcknowledgement AP:{0}, MP:{1}", Character.Stats.CurrentAP, Character.Stats.CurrentMP, _lastActionDesc);
            spellTarget.cast = true;
          }

        //if (_lastSpellCasted != null)
        //{
        //    Bot.Character.SendDebug("Availability of {0} after casted : {1}", _lastSpellCasted, _lastSpellCasted.AvailabilityExplainString(_lastSpellTarget));
        //}
      }
      _lastActionDesc = "OnAcknowledgement for " + _lastActionDesc;
      if (isMoving && !NoSpellTarget) // Move to reach casting position
        if (!failed)
          Bot.CallDelayed(300, ComeAtSpellRangeThenCast);
        else  // Move failed
          Bot.CallDelayed(300, StartAI); // continue after last failed operation. 
      else
        if (!NoSpellTarget) // spell cast
        {
          if (!failed) // If spell cast succeedeed, then restart AI from start. Otherwise, continue after last failed operation. 
            _IAStep = 0;
          Bot.CallDelayed(300, StartAI);
        }
        else
          if (isMoving) // Move without casting => pass turn in all cases
            Bot.CallDelayed(300, PassTurn);
          else
            Bot.CallDelayed(300, PassTurn);
    }

    private void ReadyToStartFight()
    {
      if (Fighter == null)
        if (Character.Fighter == null)
          return;
        else
          Fighter = Character.Fighter;
            if (PartyLeaderId == CharacterId && Fighter.Fight.Phase == FightPhase.Placement)
      {
        foreach (var member in Party)
        {
          if (!Fighter.Team.FightersAlive.Any(fighter => fighter.Id == member.id))
          {
            Character.SendInformation("Leader : Waiting for {0} to come in fight", member.name);
            Bot.CallDelayed(500, ReadyToStartFight);
            return;
          }
        }
      }
      Character.SendInformation("I'm Ready to start");
      Bot.CallDelayed(500, () => Bot.SendToServer(new GameFightReadyMessage(true)));
    }

    private void FindFarPlacement()
    {
      var fighterCells = Fighter.AvailablePlacementCells.ToArray();
      var enemyCells = Fighter.EnemiesCells.ToArray();
      if ((fighterCells.Length > 0) && (enemyCells.Length > 0))
      {
        Cell farCell = fighterCells.OrderBy(placementCell => enemyCells.Min(cell => cell.ManhattanDistanceTo(placementCell))).LastOrDefault();
                Character.Fighter.ChangePrePlacement(farCell);
      }
      ReadyToStartFight();

    }

    private void FindOptimalPlacement()
    {
      var fighterCells = Fighter.AvailablePlacementCells.ToArray();
      var enemyCells = Fighter.EnemiesCells.ToArray();
      if ((fighterCells.Length > 0) && (enemyCells.Length > 0))
      {
                spellTarget = Character.SpellsBook.FindBestUsage(Fighter, Spell.SpellCategory.Damages, true, fighterCells.OrderByDescending(cell => enemyCells.Min(ennemyCell => cell.ManhattanDistanceTo(ennemyCell))));
        if (spellTarget != null && spellTarget.Efficiency > 0)
        {
          Character.SendDebug("FindOptimalPlacement : bestSpell: {0}, efficiency : {1}, posCell {2}, targetCell {3}", spellTarget.Spell, spellTarget.Efficiency, spellTarget.FromCell, spellTarget.TargetCell);
          Character.Fighter.ChangePrePlacement(spellTarget.FromCell);
        }
      }
      ReadyToStartFight();
    }

    private Fighter FindWeakestEnemy()
    {
      return Fighter.GetOpposedTeam().FightersAlive.OrderBy(x => x.Level).FirstOrDefault();
    }

    /// <summary>
    /// Initial placement : find the position the closest to the weakest ennemy
    /// </summary>
    private void PlaceToWeakestEnemy()
    {
      var enemy = FindWeakestEnemy();
      if (enemy == null)
      {
                Character.SendError("PlaceToWeakestEnemy : no enemy left ?");
        return;
      }

      var cell = Fighter.AvailablePlacementCells.OrderBy(x => x.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
      Fighter.ChangePrePlacement(cell);
      ReadyToStartFight();
    }

    public void JoinFight(PartyMemberInFightMessage message, int nbTry)
    {
      if (nbTry <= 0)
      {
        Character.SendWarning("Can't join {0}'s fight, no try left !", message.memberName);
        return;
      }
      if (Character.IsFighting())
      {
        Character.SendWarning("Can't join {0}'s fight, as I'm already fighting ! Trying again later.", message.memberName);
        Bot.CallDelayed(4000, () => JoinFight(message, nbTry - 2));
        return;
      }
      if (Character.Map.Id != message.fightMap.mapId)
      {
        Character.SendWarning("Can't join {4}'s fight, as I'm not on the same map ({0} vs {1} [{2},{3}]) ! Trying again later.", Character.Map, message.fightMap.mapId, message.fightMap.worldX, message.fightMap.worldY, message.memberName);
        Bot.CallDelayed(2000, () => JoinFight(message, nbTry - 1));
        return;
      }
      Character.SendInformation("Joining {0}'s fight !", message.memberName);
      if (Character.IsMoving())
        Character.CancelMove(false);
      if (nbTry == 14)
        Bot.CallDelayed(2500, () => Bot.SendToServer(new GameFightJoinRequestMessage(message.memberId, message.fightId)));
      else
        Bot.SendToServer(new GameFightJoinRequestMessage(message.memberId, message.fightId));
    }

    private void PlaceCloseToFriendsButFarFromEnemies()
    {
      var fighterCells = Fighter.AvailablePlacementCells.ToArray();
      var friendCells = Fighter.FriendCells.ToArray();
      var enemyCells = Fighter.EnemiesCells.ToArray();
      if (fighterCells.Length > 0 && friendCells.Length > 0 && enemyCells.Length > 0)
      {
        Cell bestPos = Fighter.AvailablePlacementCells.OrderBy(pos => (5 * friendCells.Sum(friendCell => friendCell.ManhattanDistanceTo(pos))) / (friendCells.Length + 1) - enemyCells.Sum(enemyCell => enemyCell.ManhattanDistanceTo(pos)) / (enemyCells.Length + 1)).FirstOrDefault();
        Fighter.ChangePrePlacement(bestPos);
      }

      ReadyToStartFight();
    }

    /// <summary>
    /// Select the best starting position, close to the weakest monster, in proper distance for best spell, 
    /// and as far as possible from other monsters. 
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="InLine"></param>
    private void PlaceAtDistanceFromWeakestEnemy(int distance, bool InLine)
    {
      // TODO : also consider LOS      

      Fighter weakestEnemy = FindWeakestEnemy();
      if (weakestEnemy == null)
      {
        logger.Warn("PlaceAtDistanceFromWeakestEnemy : weakestEnemy is null");
        return;
      }
      // find the cells under distance from weakestEnemy, and - if needed - in line
      Cell[] startingSet = Fighter.AvailablePlacementCells.Where(cell => ((cell.ManhattanDistanceTo(weakestEnemy.Cell) <= distance) && (!InLine || cell.X == weakestEnemy.Cell.X || cell.Y == weakestEnemy.Cell.Y))).ToArray();
      if (startingSet.Length == 0)
      {
        logger.Debug("No cell at range => PlaceToWeakestEnemy");
        PlaceToWeakestEnemy();
        return;
      }
      logger.Debug("Placement of {0} vs {1} (cell {2}) - max Distance {4} - InLine {5} - choices : {3}", Fighter.Name, weakestEnemy.ToString(), weakestEnemy.Cell, string.Join<Cell>(",", startingSet), distance, InLine);

      Cell[] finalSet = startingSet;
      if (finalSet.Length > 1 && Fighter.GetOpposedTeam().FightersAlive.Length > 1)
      {
        // remove all cells where another enemy is closer
        foreach (Fighter otherEnnemy in Fighter.GetOpposedTeam().FightersAlive)
          if (otherEnnemy != weakestEnemy)
            finalSet = finalSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) >= x.ManhattanDistanceTo(weakestEnemy.Cell)).ToArray();
        logger.Debug("Rule 1 : choices {0}", string.Join<Cell>(",", finalSet));

        // if none, then we only remove cells where we are in contact of any other ennemy 
        if (startingSet.Length == 0)
        {
          finalSet = startingSet;
          foreach (Fighter otherEnnemy in Fighter.GetOpposedTeam().FightersAlive)
            if (otherEnnemy != weakestEnemy)
              finalSet = finalSet.Where(x => x.ManhattanDistanceTo(otherEnnemy.Cell) > 1).ToArray();
          logger.Debug("Rule 2 : choices {0}", string.Join<Cell>(",", finalSet));
        }

        // if still none, just keep all cells, ignoring other enemies
        if (finalSet.Length == 0)
        {
          finalSet = startingSet;
          logger.Debug("Rule 3 (full set) : choices {0}", string.Join<Cell>(",", finalSet));
        }
      }
      // Find a stCell as far as possible from weakest ennemy, but not over distance
      var bestCell = finalSet.OrderBy(x => x.ManhattanDistanceTo(weakestEnemy.Cell)).LastOrDefault();

      // If none under distance, then the closest
      if (bestCell == null)
      {
        logger.Debug("No cell at range => PlaceToWeakestEnemy");
        PlaceToWeakestEnemy();
        return;
      }
      //logger.Debug("Cell selected : {0}, distance {1}", bestCell, bestCell.ManhattanDistanceTo(weakestEnemy.Cell));
      Fighter.ChangePrePlacement(bestCell);
    }


    private Cell GetCellAtSpellRange(Fighter fighter, uint minRange, int maxDistanceWished, bool inLine, bool inDiagonal, bool needLOSCheck, bool cautious, bool getOnlyExactResult)
    {
      Cell dest = null;
      IEnumerable<Cell> allMoves = Fighter.GetPossibleMoves(cautious);
      //Bot.Character.ResetCellsHighlight();
      //Bot.Character.HighlightCells(allMoves, Color.Orange);
      IEnumerable<Cell> selectedMoves = allMoves;
      if (needLOSCheck)
      {
        _losMapF.UpdateTargetCell(fighter.Cell, true, false);
        selectedMoves = _losMapF.GetCellsSeenByTarget(selectedMoves);
      }
      IEnumerable<Cell> bestSelectedMoves = selectedMoves;

      // Restrict selection as needed for InLine or InDiagonal constrains
      if (inLine || inDiagonal)
        bestSelectedMoves = bestSelectedMoves.Where(cell => inLine && (fighter.Cell.X == cell.X || fighter.Cell.Y == cell.Y) || inDiagonal && (Math.Abs(fighter.Cell.X - cell.X) == Math.Abs(fighter.Cell.Y - cell.Y)));

      // Keep only those cells that respect max distance to target
      bestSelectedMoves = bestSelectedMoves.Where(cell => cell.ManhattanDistanceTo(fighter.Cell) <= maxDistanceWished && cell.ManhattanDistanceTo(fighter.Cell) >= minRange);

      // As the cells are initialy sorted by distance from the starting position, just get the first one if any. 
      dest = bestSelectedMoves.FirstOrDefault();
      if (getOnlyExactResult || dest != null) return dest;

      // If no bestResult and no need to have a proper bestResult, come as close as possible to the target

      // Find the closest position that is equal or under maxDistanceWished of the target, if any
      dest = allMoves.FirstOrDefault(cell => cell.ManhattanDistanceTo(fighter.Cell) <= maxDistanceWished);
      if (dest != null) return dest;

      // as we are far from the target, use PathFinding to find the shortest path to reach the target
      BiM.Behaviors.Game.World.Pathfinding.Path path = ((IAdvancedPathFinder)_pathFinder).FindPath(Fighter.Cell, fighter.Cell, false, Fighter.Stats.CurrentMP);

      //Bot.Character.ResetCellsHighlight();
      //Bot.Character.HighlightCells(Bot.Character.Fight.GetTrappedCellIds(), Color.Brown);
      //Bot.Character.HighlightCells(path.Cells, Color.OrangeRed);                       

      return path.End;
    }


    private void MoveNear()
    {

      //Fighter fighter, uint minRange, int maxDistanceWished, bool inLine, bool inDiagonal, bool needLOSCheck, bool cautious;
      //bestAttackSpell
      //Bot.Character.SendMessage(String.Format("MoveNear {0} : mp = {1}, distanceWished = {2}, inLine = {3}, inDiagonal = {4}, LOS = {5}", fighter, mp, maxDistanceWished, inLine, inDiagonal, needLOSCheck), Color.Pink);
      //_stopMovingDelegate = (sender, behavior, canceled, refused) => OnStopMoving(behavior, canceled, refused);
      //Bot.Character.Fighter.StopMoving += _stopMovingDelegate;
      Fighter fighter = Fighter.GetOpposedTeam().FightersAlive.OrderBy(_fighter => _fighter.Stats.Health).FirstOrDefault();
      if (fighter == null)
      {
                Character.SendWarning("No enemy left => PassTurn");
        PassTurn();
      }

      if (bestAttackSpell == null) FindBestAttack();
      Cell dest = GetCellAtSpellRange(fighter, bestAttackSpell.LevelTemplate.minRange, bestDistance, bestAttackSpell.LevelTemplate.castInLine, bestAttackSpell.LevelTemplate.castInDiagonal, bestAttackSpell.LevelTemplate.castTestLos, false, false);

      if (dest == null)
      {
                Character.SendWarning("Can't Move near {0}", fighter);
        PassTurn();
        return;
      }
      //Bot.Character.SendMessage(res, Color.Pink);

      //Move(Fighter.Cell, dest, true, mp);
            if (!Fighter.Move(dest, _pathFinder, 0, false))
      {
                if (dest.Id == Fighter.Cell.Id)
                    Character.SendInformation("No move from {0} to {1}, to come close to {2} => pass", Fighter.Cell, dest, fighter);
                else
                    Character.SendError("Failed to move from {0} to {1}, to come close to {2} => pass", Fighter.Cell, dest, fighter);
        PassTurn();
      }
      else
      {
        {
          if (Fighter.Movement == null)
            _lastPathDetail = "???";
          else
            _lastPathDetail = Fighter.MovementPath.ToString();
          _lastActionDesc = "MoveNear";
          isMoving = true;
        }
      }
    }


    // Move away and pass turn
    private void MoveFar(bool cautious)
    {
      //try
      {
        //DoNotMoveAgain = true;
        //_isLastAction = true;
        IEnumerable<Cell> enemyCells = Fighter.GetOpposedTeam().FightersAlive.Select(fighter => fighter.Cell);

        /*Cell dest = GetPossibleMoves().Where(stCell => Fighter.Fight.IsCellWalkable(stCell, false, Fighter.Cell)).OrderByDescending(stCell => enemies.Min(ennCell => stCell.ManhattanDistanceTo(ennCell.Cell))).FirstOrDefault();
        // Do not move if the new position is not better than the old one. 
        if (enemies.Min(ennCell => dest.ManhattanDistanceTo(ennCell.Cell)) < enemies.Min(ennCell => Fighter.Cell.ManhattanDistanceTo(ennCell.Cell)))
        {                
            DoNotMoveAgain = true;
            return;
        }*/
        uint ActualDistanceFromEnnemies = enemyCells.Min(ennCell => Fighter.Cell.ManhattanDistanceTo(ennCell));
        //DoNotMoveAgain = true;
        //Bot.Character.ResetCellsHighlight();
        //Bot.Character.HighlightCells(Bot.Character.Fight.GetTrappedCellIds(), Color.Brown);
        /*Bot.Character.HighlightCells(_pathFinder.FindConnectedCells(
            Fighter.Cell, true, cautious,
            cell => enemyCells.Min(ennCell => cell.Cell.ManhattanDistanceTo(ennCell)) > ActualDistanceFromEnnemies,
            cell => enemyCells.Min(ennCell => (int)cell.Cell.ManhattanDistanceTo(ennCell)), Fighter.Stats.CurrentMP), Color.Yellow);
        Bot.Character.HighlightCells(enemyCells, Color.Red);
        Bot.Character.HighlightCell(Fighter.Cell, Color.Blue);*/
        if (Fighter.Stats.CurrentMP < 1)
        {
          PassTurn();
          return;
        }
        Cell dest = _pathFinder.FindConnectedCells(
            Fighter.Cell, true, cautious,
            cell => enemyCells.Min(ennCell => cell.Cell.ManhattanDistanceTo(ennCell)) > ActualDistanceFromEnnemies,
            cell => enemyCells.Min(ennCell => (int)cell.Cell.ManhattanDistanceTo(ennCell)), Fighter.Stats.CurrentMP).LastOrDefault();

        if (dest == null)
        {
                    Character.SendWarning("Can't find a path away from monsters", Fighter.Cell, dest, String.Join(",", enemyCells));

          //DoNotMoveAgain = true;
          PassTurn();
          return;
        }
        Debug.Assert((enemyCells.Min(ennCell => dest.ManhattanDistanceTo(ennCell)) > ActualDistanceFromEnnemies), "This move do not take the character away from monsters !");

        //Move(Fighter.Cell, dest, true, Fighter.Stats.CurrentMP);
                if (!Fighter.Move(dest, _pathFinder, 0, cautious))
        {
          //DoNotMoveAgain = true;
                    Character.SendError("Failed to move far from {0} to {1}, away from {2} => pass", Fighter.Cell, dest, String.Join(",", enemyCells));
          PassTurn();
          return;
        }
        else
        {
          if (Fighter.Movement == null)
            _lastPathDetail = "???";
          else
            _lastPathDetail = Fighter.MovementPath.ToString();
                    Character.SendDebug("Moving away from ({0}) : pos {1}({2}) => {3}({4}) => pass", String.Join(",", enemyCells), Fighter.Cell, ActualDistanceFromEnnemies, dest, enemyCells.Min(ennCell => dest.ManhattanDistanceTo(ennCell)));
          isMoving = true;
          _lastActionDesc = "MoveFar";
        }
      }
    }

    private Fighter GetNearestEnemy()
    {
      if (Fighter == null) return null;
      var enemyTeam = Fighter.GetOpposedTeam();
      return enemyTeam.FightersAlive.OrderBy(enemy => Fighter.Cell.ManhattanDistanceTo(enemy.Cell)).FirstOrDefault();
    }


    public override void OnAttached()
    {
      base.OnAttached();
    }

    public override void OnDetached()
    {
            if (Character != null)
      {
                Character.FightJoined -= OnFightJoined;
                Character.FightLeft -= OnFightLeft;
                Character.MapJoined -= OnMapJoined;
                Character.StartMoving -= StandUp;
      }

      if (Fighter != null)
      {
        Fighter.TurnStarted -= OnTurnStarted;
        if (Fighter.Fight != null)
          Fighter.Fight.StateChanged -= OnStateChanged;
        //Fighter.SequenceEnded -= OnSequenceEnd;
        Fighter.Acknowledge -= OnAcknowledgement;
        Fighter = null;
      }

      if (_checkTimer != null)
      {
        _checkTimer.Dispose();
        _checkTimer = null;
      }

      base.OnDetached();
    }

    internal void Dump()
    {
            Character.SendInformation("Current target : {0}", _currentTarget);
            Character.SendInformation("HP : {0}, AP : {1}, MP : {2}", Character.Stats.Health, Character.Stats.CurrentAP, Character.Stats.CurrentMP);
            Character.SendInformation("Sitting : {0}, Moving : {1}, Fighting : {2}", _sit, Fighter != null ? Fighter.IsMoving() : Character.IsMoving(), Character.IsFighting());
            Character.SendInformation("MapID : {0}, Cell : {1}", Fighter != null ? Fighter.Map.Id : Character.Map.Id, Fighter != null ? Fighter.Cell : Character.Cell);
    }


  }
}