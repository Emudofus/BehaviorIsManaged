#region License GNU GPL
// MainForm.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CSVLogAnalyser
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      treeView1.BeforeExpand += treeView1_BeforeExpand;
      string[] args = Environment.GetCommandLineArgs();
      if (args.Length > 1)
        LoadFile(args[1]);
      else
        openFileDialogCSV.ShowDialog(this);
      this.DragDrop += MainForm_DragDrop;
      this.DragOver += MainForm_DragOver;
      this.AllowDrop = true;
    }

    #region Drag&drap
    void MainForm_DragOver(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    void MainForm_DragDrop(object sender, DragEventArgs e)
    {
      try
      {
        Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

        if (a != null && a.Length > 0)
        {
          // Extract string from first array element
          // (ignore all files except first if number of files are dropped).
          string s = a.GetValue(0).ToString();

          LoadFile(s);
          Activate();        // in the case Explorer overlaps this form
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Error in DragDrop function: " + ex.Message);
      }
    }
    #endregion Drag&drap

    public static int MessageCount;
    int GetImage(string from, bool comment)
    {
      if (comment) return 3;

      switch (from)
      {
        case "Server":
          return 0;
        case "Client":
          return 1;
        case "Local":
          return 2;
        default:
          return 4;
      }
    }

    static Color GetTextColor(int ImageId)
    {
      switch (ImageId)
      {
        case 0: // Server
          return Color.Black;
        case 1: // Client
          return Color.Blue;
        case 2: // Bot
          return Color.Green;
        case 3: // Bot (informations)
          return Color.DarkOliveGreen;
        case 8: // Connexion
          return Color.DarkBlue;
        case 7: // Fight
          return Color.DarkRed;
        default:
          return Color.Black;
      }
    }

    private int GetMasterRange(string message, out int image, out string masterName, out bool endFlag)
    {
      endFlag = false;
      if (Main.Count == 0)
      {
        masterName = "Connexion";
        image = -1;
        return 0;
      }
      switch (message)
      {
        case "HelloConnectMessage":
        case "CharacterSelectedSuccessMessage":
          image = 8;
          masterName = "Connexion";
          return 0;
        case "GameFightStartingMessage":
          masterName = "Fight"; // image 7
          image = 7;
          return 0;
        case "GameFightEndMessage":
          masterName = "Non fight";
          endFlag = true; // End of current master section
          image = 8;
          return 0;
        case "GameFightNewRoundMessage":
          masterName = "Round";
          image = 7;
          return 1;
        case "GameFightTurnStartMessage":
          masterName = "Turn";
          image = 7;
          return 2;
        default:
          masterName = null;
          image = -1;
          return -1;
      }
    }

    TreeMasterMessageItem currentMasterMessageItem = null;
    TreeMessageItem currentMessageItem = null;

    void AddLine(string line)
    {
      string[] items = line.Split(';');
      if (items.Length == 0) return;
      int range = items.Count(item => String.IsNullOrEmpty(item));
      bool isComment = range == 0 && !char.IsDigit(items[0][0]);
      bool isSubMasterNode = range == 0 && !isComment;

      if (isSubMasterNode) // Start of a message
      {
        string timeStamp = items[0];
        string message = items[1];
        string masterName = null;
        int imageId = -1;
        bool endFlag;
        int masterRange = GetMasterRange(message, out imageId, out masterName, out endFlag);
        currentMessageItem = new TreeMessageItem(timeStamp, message, GetImage(items[3], isComment), isComment);
        MessageCount++;

        if (masterRange >= 0) // It's a group start or end
        {
          if (endFlag)
            currentMasterMessageItem.MessageItems.Add(currentMessageItem);
          Main.Add(currentMasterMessageItem = new TreeMasterMessageItem(timeStamp, masterName, masterRange, imageId));
        }
        if (!endFlag)
          currentMasterMessageItem.MessageItems.Add(currentMessageItem);
      }
      else
        if (isComment) // Just a comment
          currentMasterMessageItem.MessageItems.Add(new TreeMessageItem(null, line, GetImage(line, isComment), isComment));
        else // A line within a message
          if (currentMessageItem != null)
            currentMessageItem.ItemLines.Add(new TreeItemLine(range, items[range], (items.Length == range + 2) ? items[range + 1] : null));
    }

    int LoadFile(string FileName)
    {

      textBoxFilename.Text = openFileDialogCSV.FileName;

      Stopwatch chrono = new Stopwatch();
      chrono.Start();

      Main = new List<TreeMasterMessageItem>();
      string[] data = File.ReadAllLines(FileName);
      TimeSpan tLoad = chrono.Elapsed;
      MessageCount = 0;
      foreach (string line in data)
        AddLine(line);
      FillTree();
      TimeSpan tFinal = chrono.Elapsed;
      labelRecap.Text = string.Format("{0} lines, {1} main nodes, {2} messages", data.Length, treeView1.Nodes.Count, MessageCount);
      labelRecap2.Text = string.Format("loading time {0}, parsing time {1}", tLoad.ToString(@"s\.fff"), (tFinal - tLoad).ToString(@"s\.fff"));
      return data.Length;
    }

    void FillTree()
    {
      treeView1.BeginUpdate();
      treeView1.Nodes.Clear();
      for (int i = 0; i < Main.Count; i++)
        if (Main[i].MasterLevel == 0)
          treeView1.Nodes.Add(ProcessList(i, true));
      treeView1.EndUpdate();
    }

    static List<TreeMasterMessageItem> Main = new List<TreeMasterMessageItem>();

    public class TreeMasterMessageItem
    {
      public TreeMasterMessageItem(string timeStamp, string masterName, int masterLevel, int imageId)
      {
        MasterName = masterName;
        TimeStamp = timeStamp;
        ImageId = imageId;
        MasterLevel = masterLevel;
        MessageItems = new List<TreeMessageItem>();
      }
      public int ImageId { get; private set; }
      public string TimeStamp { get; private set; }
      public string MasterName { get; private set; }
      public int MasterLevel { get; private set; } // 0 = root, 1...
      public List<TreeMessageItem> MessageItems { get; private set; }
      public override string ToString()
      {
        if ((MasterName == "Round" || MasterName == "Turn") && MessageItems.Count > 0 && MessageItems[0].ItemLines.Count > 0)
          return string.Format("{0} - {1} ({2})", TimeStamp, MasterName, MessageItems[0].ItemLines[0].Value);
        else
          return string.Format("{0} - {1}", TimeStamp, MasterName);
      }
    }

    static TreeNode[] nodes = new TreeNode[15];

    public class TreeMessageItem
    {
      public TreeMessageItem(string timeStamp, string messageName, int imageId, bool isComment)
      {
        TimeStamp = timeStamp;
        MessageName = messageName;
        ImageId = imageId;
        ItemLines = new List<TreeItemLine>();
        IsComment = isComment;
      }
      public string TimeStamp;
      public string MessageName;
      public int ImageId;
      public List<TreeItemLine> ItemLines;
      bool IsComment;
      public override string ToString()
      {
        if (IsComment)
          return MessageName;
        else
          return TimeStamp + " - " + MessageName;
      }

      public TreeNode BuildNode()
      {
        TreeNode newNode = new TreeNode(ToString(), ImageId, ImageId);
        newNode.ForeColor = GetTextColor(ImageId);
        foreach (TreeItemLine itemLine in ItemLines)
        {
          TreeNode childNode = new TreeNode(itemLine.ToString());
          childNode.ForeColor = Color.Black;
          if (itemLine.ChildLevel <= 5)
            newNode.Nodes.Add(childNode);
          else
          {
            if (nodes[itemLine.ChildLevel - 1] != null)
              nodes[itemLine.ChildLevel - 1].Nodes.Add(childNode);
            else
              Debug.Assert(false);
          }
          nodes[itemLine.ChildLevel] = childNode;
        }
        return newNode;
      }
    }

    public class TreeItemLine
    {
      public TreeItemLine(int childLevel, string variable, string value = null)
      {
        childLevel = ChildLevel;
        Variable = variable;
        Value = value;
      }
      public int ChildLevel;
      public string Variable;
      public string Value;
      public override string ToString()
      {
        if (Value == null)
          return Variable;
        else
          return Variable + " = " + Value;
      }
    }

    TreeNode CreateDummyNode(int startIndex)
    {
      TreeNode node = new TreeNode(Main[startIndex].ToString(), Main[startIndex].ImageId, Main[startIndex].ImageId);
      node.ForeColor = GetTextColor(Main[startIndex].ImageId);
      node.Tag = startIndex;
      node.Nodes.Add("dummy");
      return node;
    }

    void UpdateNode(TreeNode node, int? startIndex = null)
    {
      if (startIndex == null)
      {
        node.Nodes.Clear();
        startIndex = node.Tag as int?;
        if (node.Tag == null)
          return;
        node.Tag = null;
      }

      node.Nodes.AddRange(Main[startIndex.Value].MessageItems.Select(item => item.BuildNode()).ToArray());
      int currentLevel = Main[startIndex.Value].MasterLevel;
      for (int i = startIndex.Value + 1; i < Main.Count; i++)
      {
        if (Main[i].MasterLevel == currentLevel) break;
        if (Main[i].MasterLevel == currentLevel + 1)
          node.Nodes.Add(ProcessList(i, false));
      }
    }

    TreeNode ProcessList(int startIndex, bool dummy)
    {
      TreeNode node = new TreeNode(Main[startIndex].ToString(), Main[startIndex].ImageId, Main[startIndex].ImageId);
      node.ForeColor = GetTextColor(Main[startIndex].ImageId);
      if (dummy)
      {
        node.Tag = startIndex;
        node.Nodes.Add("dummy");
        return node;
      }
      node.Nodes.AddRange(Main[startIndex].MessageItems.Select(item => item.BuildNode()).ToArray());
      int currentLevel = Main[startIndex].MasterLevel;
      for (int i = startIndex + 1; i < Main.Count; i++)
      {
        if (Main[i].MasterLevel == currentLevel) break;
        if (Main[i].MasterLevel == currentLevel + 1)
          node.Nodes.Add(ProcessList(i, false));
      }
      return node;
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      LoadFile(openFileDialogCSV.FileName);
    }

    void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      if (e.Node.Tag is int)
        UpdateNode(e.Node, null);
    }

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
      openFileDialogCSV.ShowDialog();
    }
  }
}
