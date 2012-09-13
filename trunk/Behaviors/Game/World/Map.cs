using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Config;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Tools.Dlm;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.World
{
    public class Map : IContext, IMapDataProvider
    {
        [Configurable("MapDecryptionKey", "The decryption key used by default")]
        public static string GenericDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        public const int ElevationTolerance = 11;
        public const uint Width = 14;
        public const uint Height = 20;

        public const uint MapSize = Width*Height*2;
        private readonly List<RolePlayActor> m_actors = new List<RolePlayActor>();
        private readonly DlmMap m_map;

        public Map(int id)
            : this(id, GenericDecryptionKey)
        {
        }

        public Map(int id, string decryptionKey)
        {
            // decryption key not used ? oO
            m_map = DataProvider.Instance.Get<DlmMap>(id, GenericDecryptionKey);
            IEnumerable<Cell> cells = m_map.Cells.Select(entry => new Cell(this, entry));

            Cells = new CellList(cells.ToArray());
        }


        public ReadOnlyCollection<RolePlayActor> Actors
        {
            get { return m_actors.AsReadOnly(); }
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

        public bool UsingNewMovementSystem
        {
            get { return m_map.UsingNewMovementSystem; }
        }

        #endregion

        #region IContext Members

        public ContextActor[] GetActors(Cell cell)
        {
            return Actors.Where(entry => entry.Position != null && entry.Position.Cell == cell).Select(entry => (ContextActor)entry).ToArray();
        }

        #endregion

        #region IMapDataProvider Members

        public CellList Cells
        {
            get;
            private set;
        }

        public bool CellMarked(Cell cell)
        {
            return false;
        }

        #endregion

        public void Tick(int dt)
        {
            foreach (var actor in Actors)
            {
                actor.Tick(dt);
            }
        }

        public RolePlayActor GetActor(int id)
        {
            return m_actors.FirstOrDefault(entry => entry.Id == id);
        }

        public void Update(MapComplementaryInformationsDataMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            SubArea = new SubArea(message.subAreaId)
                          {AlignmentSide = (AlignmentSideEnum) message.subareaAlignmentSide};

            Bot bot = BotManager.Instance.GetCurrentBot();
            m_actors.Clear();
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

            Obstacles = message.obstacles;
        }

        public void AddActor(RolePlayActor actor)
        {
            m_actors.Add(actor);
        }

        public void AddActor(GameRolePlayActorInformations actor)
        {
            m_actors.Add(CreateRolePlayActor(actor));
        }

        public bool RemoveActor(RolePlayActor actor)
        {
            return m_actors.Remove(actor);
        }

        public bool RemoveActor(int id)
        {
            return m_actors.RemoveAll(entry => entry.Id == id) > 0;
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

        public bool IsActor(Cell cell)
        {
            return Actors.Any(entry => entry.Position != null && entry.Position.Cell == cell);
        }

        public bool IsCellMarked(Cell cell)
        {
            // todo
            return false;
        }

        public object[] GetMarks(Cell cell)
        {
            return new object[0];
        }

        public bool IsCellWalkable(Cell cell, bool throughEntities = false, Cell previousCell = null)
        {
            if (!cell.Walkable)
                return false;

            if (cell.NonWalkableDuringRP)
                return false;

            // compare the floors
            if (UsingNewMovementSystem && previousCell != null)
            {
                var floorDiff = Math.Abs(cell.Floor) - Math.Abs(previousCell.Floor);

                if (cell.MoveZone != previousCell.MoveZone || cell.MoveZone == previousCell.MoveZone && cell.MoveZone == 0 && floorDiff > Map.ElevationTolerance)
                    return false;
            }

            // todo : LoS

            return true;
        }
    }
}