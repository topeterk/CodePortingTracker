namespace CodePortingTracker
{
    partial class ProjectForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.SrcPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrcFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Completion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinesDone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFilesToProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowDrop = true;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SrcPath,
            this.SrcFile,
            this.Completion,
            this.LineCount,
            this.LinesDone});
            this.dgv.Location = new System.Drawing.Point(12, 27);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(601, 389);
            this.dgv.TabIndex = 0;
            this.dgv.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseDoubleClick);
            this.dgv.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgv_UserDeletedRow);
            this.dgv.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_DragDrop);
            this.dgv.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_DragEnter);
            // 
            // SrcPath
            // 
            this.SrcPath.HeaderText = "Path";
            this.SrcPath.MinimumWidth = 10;
            this.SrcPath.Name = "SrcPath";
            this.SrcPath.ReadOnly = true;
            this.SrcPath.ToolTipText = "Path to the source file";
            this.SrcPath.Width = 40;
            // 
            // SrcFile
            // 
            this.SrcFile.HeaderText = "File";
            this.SrcFile.MinimumWidth = 10;
            this.SrcFile.Name = "SrcFile";
            this.SrcFile.ReadOnly = true;
            this.SrcFile.ToolTipText = "Name of the source file";
            this.SrcFile.Width = 300;
            // 
            // Completion
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Completion.DefaultCellStyle = dataGridViewCellStyle1;
            this.Completion.HeaderText = "Done";
            this.Completion.MinimumWidth = 10;
            this.Completion.Name = "Completion";
            this.Completion.ReadOnly = true;
            this.Completion.ToolTipText = "Percentage of how many lines of code are marked as done";
            this.Completion.Width = 50;
            // 
            // LineCount
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.LineCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.LineCount.HeaderText = "Lines";
            this.LineCount.MinimumWidth = 10;
            this.LineCount.Name = "LineCount";
            this.LineCount.ReadOnly = true;
            this.LineCount.ToolTipText = "Amount of lines in the source file";
            this.LineCount.Width = 70;
            // 
            // LinesDone
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.LinesDone.DefaultCellStyle = dataGridViewCellStyle3;
            this.LinesDone.HeaderText = "Completed";
            this.LinesDone.MinimumWidth = 10;
            this.LinesDone.Name = "LinesDone";
            this.LinesDone.ReadOnly = true;
            this.LinesDone.ToolTipText = "Amount of lines marked as done";
            this.LinesDone.Width = 70;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(625, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToProjectToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addFilesToProjectToolStripMenuItem
            // 
            this.addFilesToProjectToolStripMenuItem.Name = "addFilesToProjectToolStripMenuItem";
            this.addFilesToProjectToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.addFilesToProjectToolStripMenuItem.Text = "Add files to project";
            this.addFilesToProjectToolStripMenuItem.Click += new System.EventHandler(this.addFilesToProjectToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 428);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "ProjectForm";
            this.Text = "Project";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.ProjectForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFilesToProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinesDone;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Completion;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrcPath;
    }
}