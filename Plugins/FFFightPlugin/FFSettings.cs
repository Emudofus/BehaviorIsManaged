#region License GNU GPL
// Settings.cs
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
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Settings;
using BiM.Protocol.Enums;

namespace zFFFightPlugin
{
  public class FFSettings : SettingsEntry
  {
    public FFSettings()
      : base()
    {
      /*FavoredAttackSpells = new List<uint>();
      FavoredBoostSpells = new List<uint>();
      FavoredCurseSpells = new List<uint>();
      FavoredHealSpells = new List<uint>();
      FavoredInvocSpells = new List<uint>();

      IsInvoker = false;
      IsHealer = false;*/
    }

    public void Init(PlayedCharacter character)
    {
      FavoredAttackSpells = new List<int>();
      FavoredBoostSpells = new List<int>();
      FavoredCurseSpells = new List<int>();
      FavoredHealSpells = new List<int>();
      FavoredInvocSpells = new List<int>();
      IsInvoker = false;
      IsHealer = false;
      // You can set here default preference for each breed   
      MaxPower = 1.0;
      switch ((BreedEnum)character.Breed.Id)
      {
        case BreedEnum.Cra:
          break;
        case BreedEnum.Ecaflip:
          break;
        case BreedEnum.Eniripsa:
          IsHealer = true;
          MaxPower = 0.8;
          FavoredBoostSpells = new List<int>() { 126 }; // Mot stimulant                   
          break;
        case BreedEnum.Enutrof:
          break;
        case BreedEnum.Feca:
          break;
        case BreedEnum.Iop:
          MaxPower = 1.5;
          break;
        case BreedEnum.Osamodas:
          FavoredBoostSpells = new List<int>() { 26 }; // Bénédiction animale
          IsInvoker = true;
          break;
        case BreedEnum.Pandawa:
          break;
        case BreedEnum.Roublard:
          break;
        case BreedEnum.Sacrieur:
          break;
        case BreedEnum.Sadida:
          break;
        case BreedEnum.Sram:
          break;
        case BreedEnum.Steamer:
          break;
        case BreedEnum.Xelor:
          break;
        case BreedEnum.Zobal:
          break;
        default:
          break;
      }
    }

    public override string EntryName
    {
      get
      {
        return "FFight";
      }
    }

    public double MaxPower { get; set; }

    public bool IsInvoker { get; set; }
    public bool IsHealer { get; set; }
    public List<int> FavoredBoostSpells { get; set; }
    public List<int> FavoredAttackSpells { get; set; }
    public List<int> FavoredCurseSpells { get; set; }
    public List<int> FavoredHealSpells { get; set; }
    public List<int> FavoredInvocSpells { get; set; }
    public int MobKilled { get; set; }
    public int FightStarted { get; set; }
    public int FightWin { get; set; }
    public int FightLost { get; set; }
    public int XPDone { get; set; }
    public int RPMovesSucceded { get; set; }
    public int RPMovesFailed { get; set; }
    public int FMovesSucceded { get; set; }
    public int FMovesFailed { get; set; }
    public int SpellsSucceded { get; set; }
    public int SpellsFailed { get; set; }
    public int UnvalidAckFailed { get; set; }
    public int UnvalidAckSucceeded { get; set; }

    public int Restarts { get; set; }
    public int BotElapsedSeconds { get; set; }
    public int BotFightingElapsedSecond { get; set; }
    public int BotHealingElapsedSeconds { get; set; }
    public DateTime LastFightDate { get; set; }
    public DateTime LastActivityDate { get; set; }
  }
}