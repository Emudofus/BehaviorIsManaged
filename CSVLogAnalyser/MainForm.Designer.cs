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
            this.labelRecap = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.labelRecap2 = new System.Windows.Forms.Label();
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
            this.splitContainer1.Panel2.Controls.Add(this.labelRecap2);
            this.splitContainer1.Panel2.Controls.Add(this.labelRecap);
            this.splitContainer1.Panel2.Controls.Add(this.buttonBrowse);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFilename);
            this.splitContainer1.Size = new System.Drawing.Size(702, 516);
            this.splitContainer1.SplitterDistance = 406;
            this.splitContainer1.TabIndex = 1;
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
            // labelRecap2
            // 
            this.labelRecap2.AutoSize = true;
            this.labelRecap2.Location = new System.Drawing.Point(12, 104);
            this.labelRecap2.Name = "labelRecap2";
            this.labelRecap2.Size = new System.Drawing.Size(35, 13);
            this.labelRecap2.TabIndex = 4;
            this.labelRecap2.Text = "label1";
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
  }
}

