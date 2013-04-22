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

        static Color GetMessageColor(string message, Color defaultColor)
        {
            switch (message)
            {
                case "GameActionFightNoSpellCastMessage":
                case "GameMapNoMovementMessage":
                    return Color.Red;
                case "BasicNoOperationMessage":
                case "TextInformationMessage":
                case "PartyUpdateLightMessage":
                    return Color.Gray;
                default:
                    return defaultColor;
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

        TreeMasterMessageItem _currentMasterMessageItem = null;
        TreeMasterMessageItem currentMasterMessageItem { get { EnsureCurrentMasterExists(); return _currentMasterMessageItem; } set { _currentMasterMessageItem = value; } }
        TreeMessageItem currentMessageItem = null;

        void EnsureCurrentMasterExists()
        {
            if (_currentMasterMessageItem == null)
                Main.Add(_currentMasterMessageItem = new TreeMasterMessageItem(treeView1, "", "Start", 0, 4));
        }

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
                string message = items.Length > 1 ? items[1] : "???";
                string masterName = null;
                int imageId = -1;
                bool endFlag;
                int masterRange = GetMasterRange(message, out imageId, out masterName, out endFlag);
                ListMessages.Add(currentMessageItem = new TreeMessageItem(timeStamp, message, GetImage(items.Length > 2 ? items[3] : "???", isComment), isComment));

                MessageCount++;

                if (masterRange >= 0) // It's a group start or end
                {
                    if (endFlag)
                        currentMasterMessageItem.Add(currentMessageItem);
                    Main.Add(currentMasterMessageItem = new TreeMasterMessageItem(treeView1, timeStamp, masterName, masterRange, imageId));
                }
                if (!endFlag)
                    currentMasterMessageItem.Add(currentMessageItem);
            }
            else
                if (isComment) // Just a comment
                {
                    currentMessageItem = new TreeMessageItem(null, line, GetImage(line, isComment), isComment);
                    ListMessages.Add(currentMessageItem);
                    if (currentMasterMessageItem == null)
                        Main.Add(currentMasterMessageItem = new TreeMasterMessageItem(treeView1, "", "Start", 0, GetImage(line, isComment)));
                    currentMasterMessageItem.Add(currentMessageItem);
                }
                else // A line within a message
                    if (currentMessageItem != null)
                        currentMessageItem.Add(new TreeItemLine(range, items[range], (items.Length == range + 2) ? items[range + 1] : null));
        }

        int LoadFile(string FileName)
        {

            textBoxFilename.Text = FileName;

            Stopwatch chrono = Stopwatch.StartNew();

            Main = new List<TreeMasterMessageItem>();
            ListMessages = new List<TreeMessageItem>();
            byte[] buf = File.ReadAllBytes(FileName);
            TimeSpan tLoad = chrono.Elapsed;
            MemoryStream stream = new MemoryStream(buf);
            //IEnumerable<string> data = File.ReadLines(FileName);
            MessageCount = 0;
            int lineCount = 0;
            string line;
            StreamReader reader = new StreamReader(stream);
            while ((line = reader.ReadLine()) != null)
            {
                lineCount++;
                AddLine(line);
            }
            TimeSpan tAddLine = chrono.Elapsed - tLoad;

            FillTree();
            TimeSpan tFillTree = chrono.Elapsed - tLoad - tAddLine;
            TimeSpan tFinal = chrono.Elapsed;
            labelRecap.Text = string.Format("{0} lines, {1} main nodes, {2} messages", lineCount, treeView1.Nodes.Count, MessageCount);
            labelRecap2.Text = string.Format("Total time {0} ({1}+{2}+{3})", tFinal.ToString(@"s\.fff"), tLoad.ToString(@"s\.fff"), tAddLine.ToString(@"s\.fff"), tFillTree.ToString(@"s\.fff"));
            return lineCount; // 3.6 - 8.8. - 0.04
        }
        int LoadFileIEnum(string FileName)
        {

            textBoxFilename.Text = FileName;

            Stopwatch chrono = Stopwatch.StartNew();

            Main = new List<TreeMasterMessageItem>();
            ListMessages = new List<TreeMessageItem>();
            //byte[] buf = File.ReadAllBytes(FileName);
            TimeSpan tLoad0 = chrono.Elapsed;
            IEnumerable<string> data = File.ReadLines(FileName);
            TimeSpan tLoad = chrono.Elapsed;
            MessageCount = 0;
            int lineCount = 0;
            foreach (string line in data)
            {
                lineCount++;
                AddLine(line);
            }
            TimeSpan tAddLine = chrono.Elapsed - tLoad;

            FillTree();
            TimeSpan tFillTree = chrono.Elapsed - tLoad - tAddLine;
            TimeSpan tFinal = chrono.Elapsed;
            labelRecap.Text = string.Format("{0} lines, {1} main nodes, {2} messages", lineCount, treeView1.Nodes.Count, MessageCount);
            labelRecap2.Text = string.Format("Total time {0} ({4} / {1}+{2}+{3})", tFinal.ToString(@"s\.fff"), tLoad.ToString(@"s\.fff"), tAddLine.ToString(@"s\.fff"), tFillTree.ToString(@"s\.fff"), tLoad0.ToString(@"s\.fff"));
            return lineCount; // 3.6 - 8.8. - 0.04
        }

        void FillTree()
        {
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            for (int i = 0; i < Main.Count; i++)
                if (Main[i].MasterLevel == 0)
                {

                    treeView1.Nodes.Add(ProcessList(i, true));

                }
            treeView1.EndUpdate();
        }

        public static List<TreeMasterMessageItem> Main = new List<TreeMasterMessageItem>();

        public class TreeMasterMessageItem
        {
            public TreeView Tree;
            public TreeNode CorrespondingNode { get; set; }
            public TreeMasterMessageItem(TreeView tree, string timeStamp, string masterName, int masterLevel, int imageId)
            {
                Tree = tree;
                MasterName = masterName;
                TimeStamp = timeStamp;
                ImageId = imageId;
                MasterLevel = masterLevel;
                MessageItems = new List<TreeMessageItem>();
                CorrespondingNode = null;
            }

            public int ImageId { get; private set; }
            public string TimeStamp { get; private set; }
            public string MasterName { get; private set; }
            public int MasterLevel { get; private set; } // 0 = root, 1...

            public void Add(TreeMessageItem newItem)
            {
                MessageItems.Add(newItem);
                newItem.Parent = this;
            }

            public List<TreeMessageItem> MessageItems { get; private set; }
            public override string ToString()
            {
                if ((MasterName == "Round" || MasterName == "Turn") && MessageItems.Count > 0 && MessageItems[0].ItemLines.Count > 0)
                    return string.Format("{0} - {1} ({2})", TimeStamp, MasterName, MessageItems[0].ItemLines[0].Value);
                else
                    return string.Format("{0} - {1}", TimeStamp, MasterName);
            }
            public int TextCount(string lowerStringToFind)
            {
                return FindText(lowerStringToFind).Count();
            }

            public IEnumerable<position> FindText(string lowerStringToFind)
            {
                if (MasterName.ToLowerInvariant().Contains(lowerStringToFind)) yield return new position(this);
                foreach (var item in MessageItems)
                    foreach (var pos in item.FindText(lowerStringToFind)) yield return pos;
            }

        }

        static TreeNode[] nodes = new TreeNode[15];
        public static List<TreeMessageItem> ListMessages = new List<TreeMessageItem>();
        public class TreeMessageItem
        {
            public TreeMasterMessageItem Parent { get; set; }

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

            public void Add(TreeItemLine itemLine)
            {
                ItemLines.Add(itemLine);
                itemLine.Parent = this;
            }

            public TreeNode BuildNode()
            {
                TreeNode newNode = new TreeNode(ToString(), ImageId, ImageId);
                newNode.Tag = this;
                newNode.ForeColor = GetMessageColor(MessageName, GetTextColor(ImageId));
                foreach (TreeItemLine itemLine in ItemLines)
                {
                    TreeNode childNode = new TreeNode(itemLine.ToString());
                    childNode.ForeColor = Color.Black;
                    childNode.Tag = itemLine;
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

            public int TextCount(string lowerStringToFind)
            {
                int count = 0;
                if (MessageName.ToLowerInvariant().Contains(lowerStringToFind)) count++;
                foreach (var item in ItemLines)
                    count += item.FindText(lowerStringToFind).Count();
                return count;
            }
            public IEnumerable<position> FindText(string lowerStringToFind)
            {
                if (MessageName.ToLowerInvariant().Contains(lowerStringToFind)) yield return new position(this);
                foreach (var item in ItemLines)
                    foreach (var pos in item.FindText(lowerStringToFind)) yield return pos;
            }
        }

        public class TreeItemLine
        {
            public TreeMessageItem Parent { get; set; }
            public TreeItemLine(int childLevel, string variable, string value = null)
            {
                ChildLevel = childLevel;
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

            public IEnumerable<position> FindText(string lowerStringToFind)
            {
                if (Variable.ToLowerInvariant().Contains(lowerStringToFind)) yield return new position(this);
                if (Value != null)
                    if (Value.ToLowerInvariant().Contains(lowerStringToFind)) yield return new position(this);
            }
        }

        static void FillNode(TreeNode node, int startIndex)
        {
            node.Nodes.AddRange(Main[startIndex].MessageItems.Select(item => item.BuildNode()).ToArray());
            int currentLevel = Main[startIndex].MasterLevel;
            for (int i = startIndex + 1; i < Main.Count; i++)
            {
                if (Main[i].MasterLevel == currentLevel) break;
                if (Main[i].MasterLevel == currentLevel + 1)
                    node.Nodes.Add(ProcessList(i, false));
            }
        }

        static void UpdateNode(TreeNode node, int? startIndex = null)
        {
            if (startIndex == null)
            {
                node.Nodes.Clear();
                startIndex = node.Tag as int?;
                if (!startIndex.HasValue)
                    return;
                node.Tag = Main[startIndex.Value];
            }
            FillNode(node, startIndex.Value);
        }

        static TreeNode ProcessList(int startIndex, bool dummy)
        {
            TreeMasterMessageItem masterItem = Main[startIndex];
            TreeNode node = new TreeNode(masterItem.ToString(), masterItem.ImageId, masterItem.ImageId);
            node.ForeColor = GetTextColor(masterItem.ImageId);
            masterItem.CorrespondingNode = node;
            if (dummy)
            {
                node.Tag = startIndex;
                node.Nodes.Add("dummy");
                return node;
            }
            node.Tag = Main[startIndex];
            FillNode(node, startIndex);
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

        private TreeNode FindTextRecursive(TreeNode ATreeNode, string AText)
        {
            foreach (TreeNode tn in ATreeNode.Nodes)
            {
                if (tn.Text.ToLowerInvariant().Contains(AText))
                    return tn;
                TreeNode result = FindTextRecursive(tn, AText);
                if (result != null) return result;
            }
            return null;
        }

        private static void Expand(TreeNode tn)
        {
            if (tn.Parent != null)
            {
                tn.Parent.Expand();
                if (tn.Parent.Parent != null)
                    Expand(tn.Parent);
            }
        }

        private void buttonSearchPrevious_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            TreeNode selNode = treeView1.SelectedNode ?? treeView1.TopNode;
            position refPos = new position(selNode);
            position foundPos = SearchAll(textBoxSearchText.Text.ToLowerInvariant()).LastOrDefault(pos => (pos as IComparable).CompareTo(refPos) < 0);
            FoundDisplay(foundPos);
            UseWaitCursor = false;
        }

        public class position : IComparable
        {
            int IComparable.CompareTo(object obj)
            {
                position compWith = obj as position;
                if (compWith != null)
                {
                    if (tmi == null || compWith.tmi == null)
                        throw new InvalidOperationException(string.Format("Can't compare two positions that do not define tmi"));
                    return ListMessages.IndexOf(tmi).CompareTo(ListMessages.IndexOf(compWith.tmi));
                }
                throw new InvalidOperationException(string.Format("Can't compare a {0} with a position", obj.GetType()));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <param name="expandIfNeeded"></param>
            public position(object context)
            {
                if (context is TreeNode)
                {
                    Node = context as TreeNode;
                    if (Node.Tag is int)
                        UpdateNode(Node, null);
                    context = Node.Tag;
                }
                if (context is TreeMasterMessageItem)
                    SetPosition(context as TreeMasterMessageItem);
                else
                    if (context is TreeMessageItem)
                        SetPosition(context as TreeMessageItem);
                    else
                        if (context is TreeItemLine)
                            SetPosition(context as TreeItemLine);
            }

            private void SetPosition(TreeMasterMessageItem context)
            {
                tmmi = context;
                tmi = context.MessageItems[0];
            }
            public void SetPosition(TreeMessageItem context)
            {
                tmmi = context.Parent;
                tmi = context;
            }
            public void SetPosition(TreeItemLine context)
            {
                til = context;
                tmi = context.Parent;
                tmmi = tmi.Parent;
            }

            public TreeMasterMessageItem tmmi;
            public TreeMessageItem tmi;
            public TreeItemLine til;
            private TreeNode _node;
            public TreeNode Node
            {
                get
                {
                    if (_node == null)
                    {
                        if (tmmi.CorrespondingNode == null)
                        {
                            int mainIndex = Main.IndexOf(tmmi);
                            TreeNode Rank0Master = null; // Find corresponding rank0 node
                            foreach (TreeNode node in tmmi.Tree.Nodes)
                                if (node.Tag is int)
                                    if ((node.Tag as int?).Value <= mainIndex)
                                        Rank0Master = node;
                                    else
                                        break;
                            if (Rank0Master == null) return null;
                            UpdateNode(Rank0Master, null);
                        }
                        Debug.Assert(tmmi.CorrespondingNode != null);
                        _node = tmmi.CorrespondingNode;
                        if (tmi != null)
                            foreach (TreeNode node in _node.Nodes)
                                if (node.Tag == tmi)
                                {
                                    _node = node;
                                    break;
                                }
                        if (til != null)
                            foreach (TreeNode node in _node.Nodes)
                                if (node.Tag == til)
                                {
                                    _node = node;
                                    break;
                                }
                    }
                    return _node;
                }
                set { _node = value; }
            }
        }
        TreeView MainTree { get { return treeView1; } }

        IEnumerable<position> SearchAll(string searchText)
        {
            foreach (var tmmi in Main)
                foreach (position pos in tmmi.FindText(searchText))
                    yield return pos;
        }

        private void FoundDisplay(position foundPos)
        {
            if (foundPos == null)
                label1.Text = string.Format("{0} not found", textBoxSearchText.Text);
            else
            {
                label1.Text = string.Format("{0} found in message {1}, on timeStamp {2}", textBoxSearchText.Text, foundPos.tmi.MessageName, foundPos.tmi.TimeStamp);
                TreeNode node = foundPos.Node;
                if (node == null)
                {
                    label1.Text = string.Format("(OOPS) {0} found in message {1}, on timeStamp {2}", textBoxSearchText.Text, foundPos.tmi.MessageName, foundPos.tmi.TimeStamp);
                    return;
                }
                treeView1.CollapseAll();
                foundPos.tmmi.CorrespondingNode.Expand();
                node.Expand();
                //Expand(node);
                treeView1.SelectedNode = node;
                treeView1.Focus();
            }
        }

        private void buttonSearchNext_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            TreeNode selNode = treeView1.SelectedNode ?? treeView1.TopNode;
            position refPos = new position(selNode);
            position foundPos = SearchAll(textBoxSearchText.Text.ToLowerInvariant()).FirstOrDefault(pos => (pos as IComparable).CompareTo(refPos) > 0);
            FoundDisplay(foundPos);
            UseWaitCursor = false;
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Stopwatch chrono = new Stopwatch();
            chrono.Start();
            List<TreeNode> nodesToUpdate = new List<TreeNode>();
            foreach (TreeNode node in treeView1.Nodes)
                if (node.Tag is int)
                    nodesToUpdate.Add(node);
            treeView1.BeginUpdate();
            foreach (TreeNode node in nodesToUpdate)
                UpdateNode(node, null);
            treeView1.EndUpdate();
            labelFill.Text = string.Format("{0} main nodes filled in {1}", nodesToUpdate.Count, chrono.Elapsed.ToString(@"s\.fff"));
            UseWaitCursor = false;
        }


        int recursiveCountInNode(TreeNodeCollection nodes, string text)
        {
            int count = 0;
            foreach (TreeNode node in nodes)
            {
                if (node.Text.Contains(text))
                    count++;
                if (node.Nodes != null)
                    count += recursiveCountInNode(node.Nodes, text);
            }
            return count;
        }

        private void buttonSearchCount_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            int count = 0;
            foreach (var master in Main)
                count += master.TextCount(textBoxSearchText.Text.ToLowerInvariant());
            labelSearchCount.Text = string.Format("\"{0}\" found in {1} nodes (search done in {2})", textBoxSearchText.Text, count, chrono.Elapsed.ToString(@"s\.fff"));
            UseWaitCursor = false;
        }


        private object FindText(object from, string textToFind)
        {
            return null;
        }

        private void buttonCopyClibboard_Click(object sender, EventArgs e)
        {
            TreeMessageItem message = GetSelectedMessage();
            if (message == null)
            {
                labelCopy.Text = "No message selected";
                return;
            }
            string messageStr = string.Format("[MessageHandler(typeof({0}))]\npublic static void On{0}(Bot bot, {0} message)\n\t{{\n\t\t//if (bot.Character != null) bot.Character.Process{0}(bot, message);\n\t}}\n\n", message.MessageName);
            System.Windows.Forms.Clipboard.SetText(messageStr);
            labelCopy.Text = string.Format("Code to handle the message {0} is in Clipboard", message.MessageName);
        }

        private TreeMessageItem GetSelectedMessage()
        {
            object Tag = treeView1.SelectedNode.Tag;
            if (Tag is int)
            {
                UpdateNode(treeView1.SelectedNode, null);
                Tag = treeView1.SelectedNode.Tag;
            }
            TreeMasterMessageItem tmmi = Tag as TreeMasterMessageItem;
            TreeMessageItem tmi = Tag as TreeMessageItem;
            TreeItemLine til = Tag as TreeItemLine;
            if (tmmi != null)
                tmi = tmmi.MessageItems[0];
            if (til != null)
                tmi = til.Parent;
            return tmi;
        }
    }
}
