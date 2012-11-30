using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
  public class MapTools
  {
    #region properties
    public MapDataManager mapMngr { get; private set; }
    public string LastError { get; private set; }
    public CellInfo[] cells { get; private set; }

    //fabrication d'une bitmap
    private Bitmap b;
    //création de l'outil de dessin
    private Graphics g;

    public TimeSpan OpenMapTimer { get; private set; }
    public TimeSpan PathFindingTimer { get; private set; }
    #endregion properties

    #region ctor
    public MapTools(string MapsPath = null)
    {
      //fabrication d'un bitmap
      b = new Bitmap(930, 742);
      //création de l'outil de dessin
      g = Graphics.FromImage(b);
      string FullPath = null;
      if (MapsPath != null)
      {
        FullPath = Path.Combine(Application.StartupPath, MapsPath);
        if (!Directory.Exists(FullPath))
          FullPath = null;
      }

      string RelPath = "/data/content/maps";
      for (int i = 0; i <= 5; i++)
      {
        FullPath = Application.StartupPath + RelPath;
        if (Directory.Exists(FullPath)) break;
        FullPath = null;
        RelPath = "/.." + RelPath;
      }
      if (FullPath == null)
        throw new FileNotFoundException("The directory with Map data can't be found");
      mapMngr = new MapDataManager(FullPath, false);
      mapMngr.statItem = new CellStats();
      OpenMap(null);
    }
    #endregion ctor

    #region Maps load
    public enum MapRelPos { Random, Current, Left, Right, Top, Bottom };
    public bool OpenMapRel(MapRelPos relPos)
    {
      switch (relPos)
      {
        case MapRelPos.Current:
          return OpenMap((int)mapMngr.MapData.MapId);
        case MapRelPos.Left:
          return OpenMap((int)mapMngr.MapData.GetNeighbourMapId(MapParser.Map.Direction.Left, false));
        case MapRelPos.Right:
          return OpenMap((int)mapMngr.MapData.GetNeighbourMapId(MapParser.Map.Direction.Right, false));
        case MapRelPos.Top:
          return OpenMap((int)mapMngr.MapData.GetNeighbourMapId(MapParser.Map.Direction.Top, false));
        case MapRelPos.Bottom:
          return OpenMap((int)mapMngr.MapData.GetNeighbourMapId(MapParser.Map.Direction.Bottom, false));
        default:
          return OpenMap(null);
      }
    }

    public bool CheckIfRelMapExist(MapRelPos relPos)
    {
      switch (relPos)
      {
        case MapRelPos.Current:
          return mapMngr.DoMapExist((int)mapMngr.MapData.MapId);
        case MapRelPos.Left:
          return mapMngr.DoMapExist((int)mapMngr.MapData.LeftNeighbourId);
        case MapRelPos.Right:
          return mapMngr.DoMapExist((int)mapMngr.MapData.RightNeigbourId);
        case MapRelPos.Top:
          return mapMngr.DoMapExist((int)mapMngr.MapData.TopNeighbourId);
        case MapRelPos.Bottom:
          return mapMngr.DoMapExist((int)mapMngr.MapData.BottomNeighbourId);
        default:
          return true;
      }
    }

    public bool OpenMap(int? MapId, bool OpenMessageBoxOnError = true)
    {
      mapMngr.statItem.Reset();
      Stopwatch st = new Stopwatch();
      st.Start();
      bool bSuccess = mapMngr.OpenMapWithCurrentMode(MapId);
      st.Stop();
      OpenMapTimer = st.Elapsed;
      if (bSuccess)
        PostMapLoad();
      else
      {
        LastError = mapMngr.LastError;
        if (OpenMessageBoxOnError)
          MessageBox.Show(LastError, "MapOpen");
      }
      cells = mapMngr.CellsInfos;
      return bSuccess;
    }

    void PostMapLoad()
    {
      //mapMngr.AnalyseMapData();
      PathFindingDone = false;
    }
    #endregion Maps load

    #region PathFinding
    public int StartingCell { get; private set; }
    public int ExitCell { get; private set; }
    public bool PathFindingDone { get; private set; }
    public int[] StartingCells { get; private set; }
    public int[] ExitCells { get; private set; }
    public int[] FullPath { get; private set; }
    public int[] FinalPath { get; private set; } // Maybe packed or not
    public int[] FinalPackedPath { get; private set; } // Maybe packed or not


    private int GetRandomCell(bool fightingMap, Random rnd, bool Walkable)
    {
      int Cell = CellInfo.CELL_ERROR;
      if (mapMngr.CurrentMapId == null) return CellInfo.CELL_ERROR;
      int StopFools = 1000;
      do
      {
        Cell = rnd.Next(560);
        if (IsValidWalkingCell(Cell, fightingMap) == Walkable) return Cell;
        if (StopFools-- == 0) return CellInfo.CELL_ERROR;
      } while (true);
    }

    private void RetrieveResults(Pathfinder pathFinder, int DistanceFromTarget, bool packed)
    {
      ExitCell = pathFinder.ExitCell;
      StartingCell = pathFinder.StartingCell;
      if (pathFinder.PathResult != null)
      {
        FullPath = pathFinder.PathResult.ToArray();
        FinalPath = pathFinder.GetLastPathUnpacked(DistanceFromTarget);
        if (packed) FinalPackedPath = pathFinder.PathPacker.PackPath(FinalPath);
        else FinalPackedPath = null;
      }
      else
      {
        FullPath = null;
        FinalPath = null;
        FinalPackedPath = null;
      }
    }

    /// <summary>
    /// Set new random Start and Exit points
    /// </summary>
    public bool SetRandomPath(bool fightingMap, int NbStartCells = 1, int NbExitCells = 1, bool ExitWalkable = true)
    {
      Random rnd = new Random();
      StartingCells = new int[NbStartCells];
      ExitCells = new int[NbExitCells];

      if (mapMngr.CurrentMapId == null) return false;
      for (int i = 0; i < NbStartCells; i++)
      {
        StartingCells[i] = GetRandomCell(fightingMap, rnd, true);
        if (StartingCells[i] == CellInfo.CELL_ERROR) return false;
      }
      for (int i = 0; i < NbExitCells; i++)
      {
        ExitCells[i] = GetRandomCell(fightingMap, rnd, ExitWalkable);
        if (ExitCells[i] == CellInfo.CELL_ERROR) return false;
      }
      return true;
    }

    /// <summary>
    /// Says if a CellId is valid for Pathfinding
    /// </summary>
    /// <param name="CellId"></param>
    /// <param name="iscombatMap"></param>
    /// <returns></returns>
    public bool IsValidWalkingCell(int CellId, bool iscombatMap)
    {
      if (CellId < 0 || CellId >= cells.Length) return false;
      if (!cells[CellId].isWalkable) return false;
      if (iscombatMap && !cells[CellId].isCombatWalkable)
        return false;
      return true;
    }

    /// <summary>
    /// Set starting cell for PathFinding. This is the cell where the PC stands.
    /// </summary>
    /// <param name="StartCell"></param>
    /// <param name="iscombatMap"></param>
    /// <returns></returns>
    public bool SetStartintCells(int[] StartCells)
    {
      // No data checking here
      StartingCells = StartCells;
      return true;
    }

    public bool SetStartintCell(int StartCell, bool iscombatMap)
    {
      if (!IsValidWalkingCell(StartCell, iscombatMap)) return false;
      StartingCells = new int[1] { StartCell };
      return true;
    }

    /// <summary>
    /// Set end cell for PathFinding. This is the target of the PC. 
    /// Depending on the parameters of the Pathfinding, the PC may want to come to a given distance from this. 
    /// </summary>
    /// <param name="StartCell"></param>
    /// <param name="iscombatMap"></param>
    /// <returns></returns>
    public bool SetEndCell(int EndCell, bool iscombatMap)
    {
      if (!IsValidWalkingCell(EndCell, iscombatMap)) return false;
      ExitCells = new int[1] { EndCell };
      return true;
    }

    /// <summary>
    /// Set end different possible exit cells for PathFinding. This can be possible targets for the PC. 
    /// Depending on the parameters of the Pathfinding, the PC may want to come to a given distance to one of them (the closest). 
    /// </summary>
    /// <param name="StartCell"></param>
    /// <param name="iscombatMap"></param>
    /// <returns></returns>
    public bool SetEndCells(int[] EndCells, bool iscombatMap)
    {
      ExitCells = EndCells;
      return true;
    }

    /// <summary>
    /// Return the "flight distance" between to cells. It gives a rought indication on how far they are, 
    /// without processing full PathFinding.
    /// </summary>
    /// <param name="StartCell"></param>
    /// <param name="EndCell"></param>
    /// <param name="iscombatMap"></param>
    /// <returns></returns>
    public int GetFlightDistance(int StartCell, int EndCell, bool iscombatMap)
    {
      return (int)Pathfinder.GetFlightDistance(cells[StartCell], cells[EndCell], iscombatMap);
    }


    public bool RunFindPath(bool isCombatMap, bool SelectFartherCells)
    {
      Pathfinder finder = new Pathfinder(cells, isCombatMap);
      Stopwatch st = new Stopwatch();
      st.Start();
      StartingCell = CellInfo.CELL_ERROR;
      ExitCell = CellInfo.CELL_ERROR;
      if (StartingCells == null || StartingCells.Length == 0 || ExitCells == null || ExitCells.Length == 0) return false;
      bool bFound = finder.FindPath(StartingCells, ExitCells);
      st.Stop();
      PathFindingTimer = st.Elapsed;
      RetrieveResults(finder, 0, true);
      PathFindingDone = true;
      return bFound;
    }


    /// <summary>
    /// if FishCells = null, then demo mode = random cells
    /// </summary>
    /// <param name="refCell"></param>
    /// <param name="FishCells"></param>
    /// <returns></returns>
    public int FindFish(int refCell, int[] FishCells)
    {
      if (FishCells == null)
      {
        if (!SetRandomPath(false, 1, 6, false)) return CellInfo.CELL_ERROR;
        FishCells = ExitCells;
        refCell = StartingCells[0];
      }
      else
      {
        ExitCells = FishCells;
        StartingCells = new int[] { refCell };
      }

      Pathfinder finder = new Pathfinder(cells, false);
      Stopwatch st = new Stopwatch();
      st.Start();
      int fish = finder.FindFishingSpot(refCell, FishCells, 3);
      st.Stop();
      PathFindingTimer = st.Elapsed;
      RetrieveResults(finder, 0, true);
      PathFindingDone = true;
      return fish;
    }


    #endregion PathFinding

    #region Map Drawing
    public Point GetCellPositionOnMap(int CellId)
    {
      if (CellId < 0 || CellId >= 560) return new Point();
      int i = CellId / 14;
      int j = CellId % 14;
      int x = i * 64 + 32;
      if ((j & 1) == 1) x += 32;
      int y = j * 18 + 18;
      return new Point(x, y);
    }

    public bool DrawCurrentMap(PictureBox pictureBox, bool isCombatMap, bool showAllCells, CellInfo.FieldNames SelectedInfo)
    {
      if (mapMngr.MapData == null) return false;
      g.Clear(Color.CadetBlue);
      g.SmoothingMode = SmoothingMode.HighQuality;
      Debug.Assert(mapMngr.MapData.Cells.Count == 560);
      int x = 0;
      int y = 0;
      //P.x = cellId % MAP_WIDTH + (cellId / MAP_WIDTH)/2
      //P.y = cellId % MAP_WIDTH - (cellId / MAP_WIDTH)/2
      for (int j = 0; j <= 39; j++)
        for (int i = 0; i <= 13; i++)
        {
          x = i * 64 + 32;
          if ((j & 1) == 1) x += 32;
          int cellId = j * 14 + i;
          y = j * 18 + 18;
          Color couleur = cells[cellId].color;
          Point center = new Point(x, y);
          //couleur = Color.Chartreuse;
          if ((/*!isCombatMap && */!cells[cellId].isWalkable) || (isCombatMap && !cells[cellId].isCombatWalkable))
            couleur = Color.Black;
          if (PathFindingDone)
          {
            //if (mapMngr.CellsInfos[cellId].isInPath1)
            // couleur = Color.Blue;
            //if ((cellId == StartingCell) || (cellId == ExitCell))
            //  couleur = Color.LightBlue;
          }
          if (mapMngr.CellsInfos[cellId].drawable || showAllCells)
          {

            DrawPolygon(center, couleur, cells[cellId].isInPath1, cells[cellId].isInPath2, (cellId == StartingCell), (cellId == ExitCell), StartingCells != null && StartingCells.Contains(cellId), ExitCells != null && ExitCells.Contains(cellId));
            if (SelectedInfo != CellInfo.FieldNames.Nothing)
            {
              center = new Point(center.X - 10, center.Y - 7);

              g.DrawString(cells[cellId].getValue(SelectedInfo), Control.DefaultFont, Brushes.AliceBlue, center);
            }
            //g.DrawString(CellsInfos[cellId].speed.ToString(), Font, Brushes.AliceBlue, center);
          }
        }
      //mise en picturebox
      //pictureBox.Invalidate();
      pictureBox.Image = (Image)b.Clone();
      return true;
    }

    private void DrawPolygon(Point center, Color color, bool isInPath1, bool isInPath2, bool isStartCell, bool isExitCell, bool isInStartCells, bool isInExitCells)
    {
      // Cellule dans la couleur demandée, bordure en blanc
      //   InPath1 : cercle plein bleu
      //   StartCell : triangle sup plain jaune
      //   isInStartCells : triangle sup vide jaune
      //   ExitCell : triangle inf plain jaune
      //   isInExitCells : triangle inf vide jaune

      SolidBrush brush = new SolidBrush(color);
      Pen penEndPoints = new Pen(Color.Yellow, 3);
      Brush brushEndPoints = new SolidBrush(Color.Yellow);
      SolidBrush brushInPath = new SolidBrush(Color.DarkBlue);
      Point[] points = new Point[4];
      points[0] = new Point(center.X, center.Y - 18);
      points[1] = new Point(center.X + 32, center.Y);
      points[2] = new Point(center.X, center.Y + 18);
      points[3] = new Point(center.X - 32, center.Y);
      // dessiner le polygone 
      g.FillPolygon(brush, points);
      g.DrawPolygon(Pens.White, points);
      if (PathFindingDone)
      {
        if (isInExitCells)
          g.DrawPolygon(penEndPoints, new Point[] { points[0], points[1], points[3] });
        if (isInStartCells)
          g.DrawPolygon(penEndPoints, new Point[] { points[1], points[2], points[3] });
        if (isExitCell)
          g.FillPolygon(brushEndPoints, new Point[] { points[0], points[1], points[3] });
        if (isStartCell)
          g.FillPolygon(brushEndPoints, new Point[] { points[1], points[2], points[3] });
        if (isInPath1)
          g.FillEllipse(brushInPath, center.X - 12, center.Y - 8, 24, 16);
      }

    }


    #endregion

  }
}
