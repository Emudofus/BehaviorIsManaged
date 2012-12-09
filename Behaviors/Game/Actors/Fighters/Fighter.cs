#region License GNU GPL
// Fighter.cs
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
using BiM.Behaviors.Game.Actions;
using BiM.Behaviors.Game.Actors.Interfaces;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Core.Collections;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors.Fighters
{
  public abstract class Fighter : ContextActor, INamed
  {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private ObservableCollectionMT<Fighter> m_summons = new ObservableCollectionMT<Fighter>();
    private ReadOnlyObservableCollectionMT<Fighter> m_readOnlySummons;

    protected Fighter()
    {
      CastsHistory = new SpellCastHistory(this);
      m_readOnlySummons = new ReadOnlyObservableCollectionMT<Fighter>(m_summons);
    }

    public delegate void TurnHandler(Fighter fighter);

    public event TurnHandler SequenceEnded;

    internal void NotifySequenceEnded()
    {
      if (SequenceEnded != null) SequenceEnded(this);
    }


    public event TurnHandler TurnStarted;

    internal void NotifyTurnStarted()
    {
      if (TurnStarted != null) TurnStarted(this);
    }

    public event TurnHandler TurnEnded;

    internal void NotifyTurnEnded()
    {
      if (IsMoving())
        NotifyStopMoving(false);

      TurnHandler handler = TurnEnded;
      if (handler != null) handler(this);
    }

    public delegate void SpellCastHandler(Fighter fighter, SpellCast cast);
    public event SpellCastHandler SpellCasted;

    internal void NotifySpellCasted(SpellCast cast)
    {
      CastsHistory.AddSpellCast(cast);

      SpellCastHandler handler = SpellCasted;
      if (handler != null) handler(this, cast);
    }

    private bool m_isReady;

    public Fight Fight
    {
      get;
      protected set;
    }

    public FightTeam Team
    {
      get;
      protected set;
    }

    public virtual bool IsAlive
    {
      get;
      set;
    }

    public bool IsReady
    {
      get { return m_isReady; }
      set
      {
        m_isReady = value;
        Action<Fighter, bool> evnt = ReadyStateChanged;

        if (evnt != null)
          evnt(this, value);
      }
    }

    public virtual string Name
    {
      get;
      protected set;
    }

    public virtual int Level
    {
      get;
      protected set;
    }

    public virtual IMinimalStats Stats
    {
      get;
      protected set;
    }

    public SpellCastHistory CastsHistory
    {
      get;
      protected set;
    }

    public override Map Map
    {
      get
      {
        return Fight.Map;
      }
      protected set
      {
      }
    }

    public override IMapContext Context
    {
      get { return Fight; }
      protected set
      {
      }
    }

    public virtual bool Summoned
    {
      get;
      protected set;
    }

    public virtual Fighter Summoner
    {
      get;
      protected set;
    }

    public ReadOnlyObservableCollectionMT<Fighter> Summons
    {
      get { return m_readOnlySummons; }
    }

    /// <summary>
    /// Returns true whenever it's fighter's turn
    /// </summary>
    /// <returns></returns>
    public virtual bool IsPlaying()
    {
      return this == Fight.TimeLine.CurrentPlayer;
    }

    /// <summary>
    /// Returns true whenever the fighter is able to play (i.g not dead)
    /// </summary>
    /// <returns></returns>
    public virtual bool CanPlay()
    {
      return IsAlive;
    }

    public int GetRealSpellRange(SpellLevel spell)
    {
      var range = (int)(spell.rangeCanBeBoosted ? Stats.Range + spell.range : spell.range);

      if (range < spell.minRange)
        return (int)spell.minRange;

      return range;
    }

    public bool IsInSpellRange(Cell cell, SpellLevel spell)
    {
      var range = GetRealSpellRange(spell);
      var dist = Cell.ManhattanDistanceTo(cell);

      if (!(dist >= spell.minRange && dist <= range)) return false;
      return IsInLineIfNeeded(cell, spell);
    }

    private bool IsInLineIfNeeded(Cell cell, SpellLevel spell)
    {
      if (!spell.castInLine) return true;
      return Cell.X == cell.X || Cell.Y == cell.Y;
    }

    public bool AddSummon(Fighter fighter)
    {
      if (!fighter.Summoned || fighter.Summoner != this)
        return false;

      m_summons.Add(fighter);
      return true;
    }

    public bool RemoveSummon(Fighter fighter)
    {
      return m_summons.Remove(fighter);
    }

    public FightTeam GetOpposedTeam()
    {
      return Fight.GetTeam(Team.Id == FightTeamColor.Blue ? FightTeamColor.Red : FightTeamColor.Blue);
    }

    public event Action<Fighter, bool> ReadyStateChanged;

    public virtual void Update(GameFightFighterInformations informations)
    {
      IsAlive = informations.alive;

      Stats.Update(informations.stats);

      Update(informations.disposition);

    }

    public override string ToString()
    {
      return String.Format("{0} (lv {1} {2})", Name, Level, Team.TeamType == TeamTypeEnum.TEAM_TYPE_PLAYER ? "friend" : "foe");
    }

    internal void UpdateHP(Protocol.Messages.GameActionFightLifePointsLostMessage message)
    {
      //if (this is PlayedFighter)
      //  logger.Debug("HP of {0} : {1} => {2}", Name, Stats.Health, Stats.Health - message.loss);
      Stats.UpdateHP(-message.loss);
    }

    internal void UpdateHP(Protocol.Messages.GameActionFightLifePointsGainMessage message)
    {
      //if (this is PlayedFighter)
      //  logger.Debug("HP of {0} : {1} => {2}", Name, Stats.Health, Stats.Health + message.delta);
      Stats.UpdateHP(message.delta);
    }

    internal void Update(Protocol.Messages.GameActionFightPointsVariationMessage message)
    {
      switch ((ActionIdEnum)(message.actionId))
      {
        case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER:
          //if (this is PlayedFighter)
          //  (this as PlayedFighter).Character.SendMessage(String.Format("{3} => AP of {0} : {1} => {2}", Name, Stats.CurrentAP, Stats.CurrentAP + message.delta, (ActionIdEnum)(message.actionId)));
          Stats.UpdateAP(message.delta);
          break;
        case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_DEBOOST_ACTION_POINTS:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_STEAL:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_BOOST_ACTION_POINTS:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_USE:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_WIN:
          goto case ActionIdEnum.ACTION_CHARACTER_ACTION_POINTS_LOST_CASTER;

        case ActionIdEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_MOVEMENT_POINTS_STEAL:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_MOVEMENT_POINTS_WIN:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_DEBOOST_MOVEMENT_POINTS:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_BOOST_MOVEMENT_POINTS:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_MOVEMENT_POINTS_USE:
          goto case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER;
        case ActionIdEnum.ACTION_CHARACTER_MOVEMEMT_POINTS_LOST_CASTER:
          //if (this is PlayedFighter)
          //  (this as PlayedFighter).Character.SendMessage(String.Format("{3} => MP of {0} : {1} => {2}", Name, Stats.CurrentMP, Stats.CurrentMP + message.delta, (ActionIdEnum)(message.actionId)));
          Stats.UpdateMP(message.delta);
          break;
      }
    }
  }
}