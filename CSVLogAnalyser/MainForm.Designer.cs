namespace CSVLogAnalyser
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.openFileDialogCSV = new System.Windows.Forms.OpenFileDialog();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.imageListTree = new System.Windows.Forms.ImageList(this.components);
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.labelSearchCount = new System.Windows.Forms.Label();
      this.buttonSearchCount = new System.Windows.Forms.Button();
      this.labelFill = new System.Windows.Forms.Label();
      this.buttonFill = new System.Windows.Forms.Button();
      this.buttonSerchNext = new System.Windows.Forms.Button();
      this.buttonSerchPrevious = new System.Windows.Forms.Button();
      this.textBoxSearchText = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.labelRecap2 = new System.Windows.Forms.Label();
      this.labelRecap = new System.Windows.Forms.Label();
      this.buttonBrowse = new System.Windows.Forms.Button();
      this.textBoxFilename = new System.Windows.Forms.TextBox();
      this.buttonCopyClibboard = new System.Windows.Forms.Button();
      this.labelCopy = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // openFileDialogCSV
      // 
      this.openFileDialogCSV.DefaultExt = "csv";
      this.openFileDialogCSV.FileName = "*";
      this.openFileDialogCSV.Filter = "CSV files|*.csv";
      this.openFileDialogCSV.ReadOnlyChecked = true;
      this.openFileDialogCSV.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.ImageIndex = 5;
      this.treeView1.ImageList = this.imageListTree;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageIndex = 5;
      this.treeView1.Size = new System.Drawing.Size(406, 516);
      this.treeView1.TabIndex = 0;
      // 
      // imageListTree
      // 
      this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
      this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListTree.Images.SetKeyName(0, "server.png");
      this.imageListTree.Images.SetKeyName(1, "workplace.png");
      this.imageListTree.Images.SetKeyName(2, "robot.png");
      this.imageListTree.Images.SetKeyName(3, "about.png");
      this.imageListTree.Images.SetKeyName(4, "help2.png");
      this.imageListTree.Images.SetKeyName(5, "bullet_triangle_green.png");
      this.imageListTree.Images.SetKeyName(6, "knight2.png");
      this.imageListTree.Images.SetKeyName(7, "skull.png");
      this.imageListTree.Images.SetKeyName(8, "yinyang.png");
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.treeView1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.labelCopy);
      this.splitContainer1.Panel2.Controls.Add(this.buttonCopyClibboard);
      this.splitContainer1.Panel2.Controls.Add(this.labelSearchCount);
      this.splitContainer1.Panel2.Controls.Add(this.buttonSearchCount);
      this.splitContainer1.Panel2.Controls.Add(this.labelFill);
      this.splitContainer1.Panel2.Controls.Add(this.buttonFill);
      this.splitContainer1.Panel2.Controls.Add(this.buttonSerchNext);
      this.splitContainer1.Panel2.Controls.Add(this.buttonSerchPrevious);
      this.splitContainer1.Panel2.Controls.Add(this.textBoxSearchText);
      this.splitContainer1.Panel2.Controls.Add(this.label1);
      this.splitContainer1.Panel2.Controls.Add(this.labelRecap2);
      this.splitContainer1.Panel2.Controls.Add(this.labelRecap);
      this.splitContainer1.Panel2.Controls.Add(this.buttonBrowse);
      this.splitContainer1.Panel2.Controls.Add(this.textBoxFilename);
      this.splitContainer1.Size = new System.Drawing.Size(702, 516);
      this.splitContainer1.SplitterDistance = 406;
      this.splitContainer1.TabIndex = 1;
      // 
      // labelSearchCount
      // 
      this.labelSearchCount.AutoSize = true;
      this.labelSearchCount.Location = new System.Drawing.Point(5, 298);
      this.labelSearchCount.Name = "labelSearchCount";
      this.labelSearchCount.Size = new System.Drawing.Size(0, 13);
      this.labelSearchCount.TabIndex = 12;
      // 
      // buttonSearchCount
      // 
      this.buttonSearchCount.Location = new System.Drawing.Point(118, 246);
      this.buttonSearchCount.Name = "buttonSearchCount";
      this.buttonSearchCount.Size = new System.Drawing.Size(27, 23);
      this.buttonSearchCount.TabIndex = 11;
      this.buttonSearchCount.Text = "*";
      this.buttonSearchCount.UseVisualStyleBackColor = true;
      this.buttonSearchCount.Click += new System.EventHandler(this.buttonSearchCount_Click);
      // 
      // labelFill
      // 
      this.labelFill.AutoSize = true;
      this.labelFill.Location = new System.Drawing.Point(15, 159);
      this.labelFill.Name = "labelFill";
      this.labelFill.Size = new System.Drawing.Size(193, 13);
      this.labelFill.TabIndex = 10;
      this.labelFill.Text = "--------------------------------------------------------------";
      this.labelFill.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // buttonFill
      // 
      this.buttonFill.Location = new System.Drawing.Point(86, 129);
      this.buttonFill.Name = "buttonFill";
      this.buttonFill.Size = new System.Drawing.Size(56, 23);
      this.buttonFill.TabIndex = 9;
      this.buttonFill.Text = "Fill";
      this.buttonFill.UseVisualStyleBackColor = true;
      this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
      // 
      // buttonSerchNext
      // 
      this.buttonSerchNext.Location = new System.Drawing.Point(166, 246);
      this.buttonSerchNext.Name = "buttonSerchNext";
      this.buttonSerchNext.Size = new System.Drawing.Size(24, 23);
      this.buttonSerchNext.TabIndex = 8;
      this.buttonSerchNext.Text = ">";
      this.buttonSerchNext.UseVisualStyleBackColor = true;
      this.buttonSerchNext.Click += new System.EventHandler(this.buttonSearchNext_Click);
      // 
      // buttonSerchPrevious
      // 
      this.buttonSerchPrevious.Location = new System.Drawing.Point(73, 246);
      this.buttonSerchPrevious.Name = "buttonSerchPrevious";
      this.buttonSerchPrevious.Size = new System.Drawing.Size(24, 23);
      this.buttonSerchPrevious.TabIndex = 7;
      this.buttonSerchPrevious.Text = "<";
      this.buttonSerchPrevious.UseVisualStyleBackColor = true;
      this.buttonSerchPrevious.Click += new System.EventHandler(this.buttonSearchPrevious_Click);
      // 
      // textBoxSearchText
      // 
      this.textBoxSearchText.Location = new System.Drawing.Point(3, 220);
      this.textBoxSearchText.Name = "textBoxSearchText";
      this.textBoxSearchText.Size = new System.Drawing.Size(286, 20);
      this.textBoxSearchText.TabIndex = 6;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(2, 204);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Search string";
      // 
      // labelRecap2
      // 
      this.labelRecap2.AutoSize = true;
      this.labelRecap2.Location = new System.Drawing.Point(12, 104);
      this.labelRecap2.Name = "labelRecap2";
      this.labelRecap2.Size = new System.Drawing.Size(35, 13);
      this.labelRecap2.TabIndex = 4;
      this.labelRecap2.Text = "label1";
      // 
      // labelRecap
      // 
      this.labelRecap.AutoSize = true;
      this.labelRecap.Location = new System.Drawing.Point(12, 78);
      this.labelRecap.Name = "labelRecap";
      this.labelRecap.Size = new System.Drawing.Size(35, 13);
      this.labelRecap.TabIndex = 3;
      this.labelRecap.Text = "label1";
      // 
      // buttonBrowse
      // 
      this.buttonBrowse.Location = new System.Drawing.Point(12, 36);
      this.buttonBrowse.Name = "buttonBrowse";
      this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
      this.buttonBrowse.TabIndex = 2;
      this.buttonBrowse.Text = "Browse...";
      this.buttonBrowse.UseVisualStyleBackColor = true;
      this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
      // 
      // textBoxFilename
      // 
      this.textBoxFilename.Location = new System.Drawing.Point(12, 12);
      this.textBoxFilename.Name = "textBoxFilename";
      this.textBoxFilename.ReadOnly = true;
      this.textBoxFilename.Size = new System.Drawing.Size(268, 20);
      this.textBoxFilename.TabIndex = 0;
      // 
      // buttonCopyClibboard
      // 
      this.buttonCopyClibboard.Location = new System.Drawing.Point(22, 346);
      this.buttonCopyClibboard.Name = "buttonCopyClibboard";
      this.buttonCopyClibboard.Size = new System.Drawing.Size(140, 23);
      this.buttonCopyClibboard.TabIndex = 13;
      this.buttonCopyClibboard.Text = "Copy Code to ClipBoard";
      this.buttonCopyClibboard.UseVisualStyleBackColor = true;
      this.buttonCopyClibboard.Click += new System.EventHandler(this.buttonCopyClibboard_Click);
      // 
      // labelCopy
      // 
      this.labelCopy.AutoSize = true;
      this.labelCopy.Location = new System.Drawing.Point(11, 376);
      this.labelCopy.Name = "labelCopy";
      this.labelCopy.Size = new System.Drawing.Size(0, 13);
      this.labelCopy.TabIndex = 14;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(702, 516);
      this.Controls.Add(this.splitContainer1);
      this.Name = "MainForm";
      this.Text = "CVS Logs analyser";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog openFileDialogCSV;
    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.ImageList imageListTree;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Label labelRecap;
    private System.Windows.Forms.Button buttonBrowse;
    private System.Windows.Forms.TextBox textBoxFilename;
    private System.Windows.Forms.Label labelRecap2;
    private System.Windows.Forms.Button buttonSerchNext;
    private System.Windows.Forms.Button buttonSerchPrevious;
    private System.Windows.Forms.TextBox textBoxSearchText;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelFill;
    private System.Windows.Forms.Button buttonFill;
    private System.Windows.Forms.Label labelSearchCount;
    private System.Windows.Forms.Button buttonSearchCount;
    private System.Windows.Forms.Button buttonCopyClibboard;
    private System.Windows.Forms.Label labelCopy;
  }
}

