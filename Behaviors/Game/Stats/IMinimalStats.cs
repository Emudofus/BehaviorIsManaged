#region License GNU GPL
// IMinimalStats.cs
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
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public interface IMinimalStats
    {
        int Initiative
        {
            get;
        }

        int Health
        {
            get;
        }

        int MaxHealth
        {
            get;
        }

        int MaxHealthBase
        {
            get;
        }

        int CurrentAP
        {
            get;
        }

        int CurrentMP
        {
            get;
        }

        int MaxAP
        {
            get;
        }

        int MaxMP
        {
            get;
        }

        int Range
        {
            get;
        }

        int PermanentDamagePercent
        {
            get;
        }

        int TackleBlock
        {
            get;
        }

        int TackleEvade
        {
            get;
        }

        int DodgeAPProbability
        {
            get;
        }

        int DodgeMPProbability
        {
            get;
        }

        int NeutralResistPercent
        {
            get;
        }

        int EarthResistPercent
        {
            get;
        }

        int WaterResistPercent
        {
            get;
        }

        int AirResistPercent
        {
            get;
        }

        int FireResistPercent
        {
            get;
        }

        int NeutralElementReduction
        {
            get;
        }

        int EarthElementReduction
        {
            get;
        }

        int WaterElementReduction
        {
            get;
        }

        int AirElementReduction
        {
            get;
        }

        int FireElementReduction
        {
            get;
        }
        void Update(GameFightMinimalStats stats);
    }
}