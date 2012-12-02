﻿#region License GNU GPL
// Map.cs
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
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Interactives;
using BiM.Behaviors.Game.World.Areas;
using BiM.Behaviors.Game.World.Data;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Collections;
using BiM.Core.Config;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Tools.Dlm;
using BiM.Protocol.Types;
using NLog;
using Npc = BiM.Behaviors.Game.Actors.RolePlay.Npc;
using SubArea = BiM.Behaviors.Game.World.Areas.SubArea;

namespace BiM.Behaviors.Game.World
{
    public partial class Map : MapContext<RolePlayActor>
    {
        public const int ElevationTolerance = 11;
        public const uint Width = 14;
        public const uint Height = 20;

        public const uint MapSize = Width*Height*2;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Configurable("MapDecryptionKey", "The decryption key used by default")]
        public static string GenericDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        private readonly DlmMap m_map;
        private readonly MapPosition m_position;
        private readonly List<Tuple<uint, Cell>> m_elements = new List<Tuple<uint, Cell>>();
        private readonly ObservableCollectionMT<InteractiveObject> m_interactives;
        private readonly ReadOnlyObservableCollectionMT<InteractiveObject> m_readOnlyInteractives;

        // not used
        /*public static readonly Dictionary<MapNeighbour, int[]> MapChangeDatas = new Dictionary<MapNeighbour, int[]>
        {
            {MapNeighbour.Right, new [] {1, 2, 128}},
            {MapNeighbour.Left, new [] {8, 16, 32}},
            {MapNeighbour.Top, new [] {32, 64, 128}},
            {MapNeighbour.Bottom, new [] {2, 4, 8}},
        };*/

        public static readonly Dictionary<MapNeighbour, int> MapChangeDatas = new Dictionary<MapNeighbour, int>
        {
            {MapNeighbour.Right, 1},
            {MapNeighbour.Left, 16},
            {MapNeighbour.Top, 64},
            {MapNeighbour.Bottom, 4},
        };

        /// <summary>
        /// Create a Map instance only used to store datas (cells, properties ...)
        /// </summary>
        public static Map CreateDataMapInstance(MapData map)
        {
            return new Map(map);
        }

        public Map(int id)
            : this(id, GenericDecryptionKey)
        {
        }

        public Map(int id, string decryptionKey)
        {
            // decryption key not used ? oO
            m_map = DataProvider.Instance.Get<DlmMap>(id, GenericDecryptionKey);
            m_position = DataProvider.Instance.GetOrDefault<MapPosition>(id);
            IEnumerable<Cell> cells = m_map.Cells.Select(entry => new Cell(this, entry));
            m_cells = new CellList(cells.ToArray());

            InitializeElements();

            m_interactives = new ObservableCollectionMT<InteractiveObject>();
            m_readOnlyInteractives = new ReadOnlyObservableCollectionMT<InteractiveObject>(m_interactives);
        }

        private Map(MapData map)
        {
            m_position = DataProvider.Instance.GetOrDefault<MapPosition>(map.Id);
            IEnumerable<Cell> cells = map.Cells.Select(entry => new Cell(this, entry));
            m_cells = new CellList(cells.ToArray());
        }

        private void InitializeElements()
        {
            foreach (DlmLayer layer in m_map.Layers)
            {
                foreach (DlmCell cell in layer.Cells)
                {
                    foreach (DlmGraphicalElement element in cell.Elements.OfType<DlmGraphicalElement>())
                    {
                        m_elements.Add(Tuple.Create(element.Identifier, Cells[cell.Id]));
                    }
                }
            }
        }

        public ReadOnlyObservableCollectionMT<InteractiveObject> Interactives
        {
            get { return m_readOnlyInteractives; }
        }

        public SubArea SubArea
        {
            get;
            private set;
        }

        public MapObstacle[] Obstacles
        {
            get;
            private set;
        }

        private CellList m_cells;
        private string m_name;

        public override CellList Cells
        {
            get { return m_cells; }
        }

        public override void Tick(int dt)
        {
            foreach (RolePlayActor actor in Actors)
            {
                actor.Tick(dt);
            }
        }

        public bool CanStopOnCell(Cell cell)
        {
            return IsCellWalkable(cell) && !Interactives.Any(x => x.Cell == cell);
        }

        #region Update methods

        public void Update(MapComplementaryInformationsDataMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            SubArea = new SubArea(message.subAreaId)
                          {AlignmentSide = (AlignmentSideEnum) message.subareaAlignmentSide};

            Bot bot = BotManager.Instance.GetCurrentBot();
            ClearActors();

            foreach (GameRolePlayActorInformations actor in message.actors)
            {
                if (actor.contextualId == bot.Character.Id)
                {
                    bot.Character.Update(actor as GameRolePlayCharacterInformations);
                    AddActor(bot.Character);
                }
                else
                    AddActor(actor);
            }

            foreach (InteractiveElement element in message.interactiveElements)
            {
                AddInteractive(new InteractiveObject(this, element));
            }

            foreach (StatedElement element in message.statedElements)
            {
                Update(element);
            }

            Obstacles = message.obstacles;
        }

        public void Update(InteractiveElementUpdatedMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            InteractiveObject element = GetInteractive(message.interactiveElement.elementId);

            if (element != null)
                element.Update(message.interactiveElement);
            else
                AddInteractive(new InteractiveObject(this, message.interactiveElement));
        }

        public void Update(InteractiveMapUpdateMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            m_interactives.Clear();
            foreach (InteractiveElement element in message.interactiveElements)
            {
                AddInteractive(new InteractiveObject(this, element));
            }
        }

        public void Update(StatedElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            InteractiveObject interactive = GetInteractive(element.elementId);

            if (interactive != null)
            {
                interactive.Update(element);
            }
            else
                logger.Warn("Cannot found Interactive {0} associated to the StatedElement", element.elementId);
        }

        public void Update(StatedElementUpdatedMessage message)
        {
            Update(message.statedElement);
        }

        public void Update(StatedMapUpdateMessage message)
        {
            foreach (InteractiveObject interactive in m_interactives)
            {
                interactive.ResetState();
            }

            foreach (StatedElement element in message.statedElements)
            {
                Update(element);
            }
        }

        #endregion

        #region World Objects Management

        public InteractiveObject GetInteractive(int id)
        {
            InteractiveObject[] interactives = m_interactives.Where(entry => entry.Id == id).ToArray();

            if (interactives.Length > 1)
            {
                logger.Error("{0} interactives objects found with the same id {1} !", interactives.Length, id);
            }

            return interactives.FirstOrDefault();
        }

        public void AddInteractive(InteractiveObject interactive)
        {
            m_interactives.Add(interactive);

            Tuple<uint, Cell> element = m_elements.FirstOrDefault(x => x.Item1 == interactive.Id);
            if (element != null)
                interactive.DefinePosition(element.Item2);
        }

        public void AddActor(GameRolePlayActorInformations actor)
        {
            AddActor(CreateRolePlayActor(actor));
        }

        public RolePlayActor CreateRolePlayActor(GameRolePlayActorInformations actor)
        {
            if (actor is GameRolePlayCharacterInformations)
                return new Character(actor as GameRolePlayCharacterInformations, this);
            if (actor is GameRolePlayGroupMonsterInformations)
                return new GroupMonster(actor as GameRolePlayGroupMonsterInformations, this);
            if (actor is GameRolePlayMerchantWithGuildInformations)
                return new Merchant(actor as GameRolePlayMerchantWithGuildInformations, this);
            if (actor is GameRolePlayMerchantInformations)
                return new Merchant(actor as GameRolePlayMerchantInformations, this);
            if (actor is GameRolePlayMountInformations)
                return new MountActor(actor as GameRolePlayMountInformations, this);
            if (actor is GameRolePlayMutantInformations)
                return new Mutant(actor as GameRolePlayMutantInformations, this);
            if (actor is GameRolePlayNpcWithQuestInformations)
                return new Npc(actor as GameRolePlayNpcWithQuestInformations, this);
            if (actor is GameRolePlayNpcInformations)
                return new Npc(actor as GameRolePlayNpcInformations, this);
            if (actor is GameRolePlayTaxCollectorInformations)
                return new TaxCollector(actor as GameRolePlayTaxCollectorInformations, this);
            if (actor is GameRolePlayPrismInformations)
                return new Prism(actor as GameRolePlayPrismInformations, this);

            throw new Exception(string.Format("Actor {0} not handled", actor.GetType()));
        }

        #endregion

        #region Map Properties

        public int Id
        {
            get { return m_map.Id; }
        }

        public byte Version
        {
            get { return m_map.Version; }
        }

        public bool Encrypted
        {
            get { return m_map.Encrypted; }
        }

        public byte EncryptionVersion
        {
            get { return m_map.EncryptionVersion; }
        }

        public uint RelativeId
        {
            get { return m_map.RelativeId; }
        }

        public byte MapType
        {
            get { return m_map.MapType; }
        }

        public int SubAreaId
        {
            get { return m_map.SubAreaId; }
        }

        public int BottomNeighbourId
        {
            get { return m_map.BottomNeighbourId; }
        }

        public int LeftNeighbourId
        {
            get { return m_map.LeftNeighbourId; }
        }

        public int RightNeighbourId
        {
            get { return m_map.RightNeighbourId; }
        }

        public int ShadowBonusOnEntities
        {
            get { return m_map.ShadowBonusOnEntities; }
        }

        public Color BackgroundColor
        {
            get { return m_map.BackgroundColor; }
        }

        public ushort ZoomScale
        {
            get { return m_map.ZoomScale; }
        }

        public short ZoomOffsetX
        {
            get { return m_map.ZoomOffsetX; }
        }

        public short ZoomOffsetY
        {
            get { return m_map.ZoomOffsetY; }
        }

        public bool UseLowPassFilter
        {
            get { return m_map.UseLowPassFilter; }
        }

        public bool UseReverb
        {
            get { return m_map.UseReverb; }
        }

        public int PresetId
        {
            get { return m_map.PresetId; }
        }

        public DlmFixture[] BackgroudFixtures
        {
            get { return m_map.BackgroudFixtures; }
        }

        public int TopNeighbourId
        {
            get { return m_map.TopNeighbourId; }
        }

        public DlmFixture[] ForegroundFixtures
        {
            get { return m_map.ForegroundFixtures; }
        }

        public int GroundCRC
        {
            get { return m_map.GroundCRC; }
        }

        public DlmLayer[] Layers
        {
            get { return m_map.Layers; }
        }

        public override bool UsingNewMovementSystem
        {
            get { return m_map.UsingNewMovementSystem; }
        }

        #endregion

        #region Position

        public int PosX
        {
            get
            {
                return m_position != null ? m_position.posX : (Id & 0x3FE00) >> 9;
            }
        }

        public int PosY
        {
            get
            {
                return m_position != null ? m_position.posY : Id & 0x1FF;
            }
        }

        public bool Outdoor
        {
            get
            {
                return m_position != null && m_position.outdoor;
            }
        }

        public int Capabilities
        {
            get
            {
                return m_position != null ? m_position.capabilities : 0;
            }
        }

        public string Name
        {
            get { if (m_position == null)
                return string.Empty;
                
                return m_name ?? (m_name = DataProvider.Instance.Get<string>(m_position.nameId)); }
        }

        public int WorldMap
        {
            get
            {
                return m_position != null ? m_position.worldMap : -1;
            }
        }

        public bool HasProprityOnWorldmap
        {
            get
            {
                return m_position != null && m_position.hasPriorityOnWorldmap;
            }
        }

        #endregion
    }
}