namespace FuseboxFreedom {
    partial class GUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_paths = new System.Windows.Forms.DataGridView();
            this.pb_converting = new System.Windows.Forms.ProgressBar();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_output = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txt_root = new System.Windows.Forms.ToolStripTextBox();
            this.btn_process = new System.Windows.Forms.Button();
            this.chk_watch = new System.Windows.Forms.CheckBox();
            this.fd_load = new System.Windows.Forms.OpenFileDialog();
            this.fd_save = new System.Windows.Forms.SaveFileDialog();
            this.fd_folder = new System.Windows.Forms.FolderBrowserDialog();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewImageColumn();
            this.btn_load = new System.Windows.Forms.ToolStripSplitButton();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.ProgressHideTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_paths)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_paths, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.pb_converting, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txt_input, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_output, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_process, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chk_watch, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(740, 462);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgv_paths
            // 
            this.dgv_paths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_paths.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Path,
            this.Status});
            this.tableLayoutPanel1.SetColumnSpan(this.dgv_paths, 2);
            this.dgv_paths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_paths.Location = new System.Drawing.Point(3, 114);
            this.dgv_paths.Name = "dgv_paths";
            this.dgv_paths.Size = new System.Drawing.Size(734, 345);
            this.dgv_paths.TabIndex = 6;
            this.dgv_paths.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_paths_CellClick);
            this.dgv_paths.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_paths_CellEndEdit);
            // 
            // pb_converting
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pb_converting, 2);
            this.pb_converting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pb_converting.Location = new System.Drawing.Point(3, 94);
            this.pb_converting.Name = "pb_converting";
            this.pb_converting.Size = new System.Drawing.Size(734, 14);
            this.pb_converting.TabIndex = 0;
            // 
            // txt_input
            // 
            this.txt_input.Dock = System.Windows.Forms.DockStyle.Top;
            this.txt_input.Location = new System.Drawing.Point(3, 39);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(364, 20);
            this.txt_input.TabIndex = 1;
            this.txt_input.TextChanged += new System.EventHandler(this.txt_input_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Circuit File";
            // 
            // txt_output
            // 
            this.txt_output.Dock = System.Windows.Forms.DockStyle.Top;
            this.txt_output.Location = new System.Drawing.Point(373, 39);
            this.txt_output.Name = "txt_output";
            this.txt_output.Size = new System.Drawing.Size(364, 20);
            this.txt_output.TabIndex = 2;
            this.txt_output.TextChanged += new System.EventHandler(this.txt_output_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(373, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output File";
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 2);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_load,
            this.btn_save,
            this.toolStripLabel1,
            this.txt_root,
            this.toolStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(740, 23);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(75, 15);
            this.toolStripLabel1.Text = "Project Root:";
            // 
            // txt_root
            // 
            this.txt_root.Name = "txt_root";
            this.txt_root.Size = new System.Drawing.Size(300, 23);
            this.txt_root.TextChanged += new System.EventHandler(this.txt_root_TextChanged);
            // 
            // btn_process
            // 
            this.btn_process.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_process.Location = new System.Drawing.Point(662, 65);
            this.btn_process.Name = "btn_process";
            this.btn_process.Size = new System.Drawing.Size(75, 23);
            this.btn_process.TabIndex = 7;
            this.btn_process.Text = "Process";
            this.btn_process.UseVisualStyleBackColor = true;
            this.btn_process.Click += new System.EventHandler(this.btn_process_Click);
            // 
            // chk_watch
            // 
            this.chk_watch.AutoSize = true;
            this.chk_watch.Location = new System.Drawing.Point(3, 65);
            this.chk_watch.Name = "chk_watch";
            this.chk_watch.Size = new System.Drawing.Size(117, 17);
            this.chk_watch.TabIndex = 5;
            this.chk_watch.Text = "Watch for changes";
            this.chk_watch.UseVisualStyleBackColor = true;
            this.chk_watch.CheckedChanged += new System.EventHandler(this.chk_watch_CheckedChanged);
            // 
            // fd_load
            // 
            this.fd_load.DefaultExt = "fbf.settings";
            this.fd_load.Filter = "FBF Settings|*.fbf.settings";
            // 
            // fd_save
            // 
            this.fd_save.DefaultExt = "fbf.settings";
            this.fd_save.Filter = "FBF Settings|*.fbf.settings";
            // 
            // Path
            // 
            this.Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Path.DataPropertyName = "path";
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Status.DataPropertyName = "status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Status.Width = 62;
            // 
            // btn_load
            // 
            this.btn_load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_load.Image = ((System.Drawing.Image)(resources.GetObject("btn_load.Image")));
            this.btn_load.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(89, 19);
            this.btn_load.Text = "Load Project";
            this.btn_load.ButtonClick += new System.EventHandler(this.btn_load_ButtonClick);
            // 
            // btn_save
            // 
            this.btn_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 19);
            this.btn_save.Text = "Save Project";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::FuseboxFreedom.Properties.Resources.folder;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // ProgressHideTimer
            // 
            this.ProgressHideTimer.Interval = 1000;
            this.ProgressHideTimer.Tick += new System.EventHandler(this.ProgressHideTimer_Tick);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 462);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.Text = "FuseboxFreedom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_paths)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar pb_converting;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_watch;
        private System.Windows.Forms.DataGridView dgv_paths;
        private System.Windows.Forms.Button btn_process;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton btn_load;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txt_root;
        private System.Windows.Forms.OpenFileDialog fd_load;
        private System.Windows.Forms.SaveFileDialog fd_save;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.FolderBrowserDialog fd_folder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewImageColumn Status;
        private System.Windows.Forms.Timer ProgressHideTimer;
    }
}