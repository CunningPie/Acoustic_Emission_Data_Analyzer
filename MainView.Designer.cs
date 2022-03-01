namespace AEDataAnalyzer
{
    partial class MainView
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_FindWaves = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Plot = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Correlation = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Show_Waves = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_Create_Super_Waves = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tool_SW_Set_Correlation = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Tools_STF = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Tools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File_Open});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(44, 24);
            this.Menu_File.Text = "File";
            // 
            // Menu_File_Open
            // 
            this.Menu_File_Open.Name = "Menu_File_Open";
            this.Menu_File_Open.Size = new System.Drawing.Size(120, 26);
            this.Menu_File_Open.Text = "Open";
            this.Menu_File_Open.Click += new System.EventHandler(this.Menu_File_Open_Click);
            // 
            // Menu_Tools
            // 
            this.Menu_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Tools_FindWaves,
            this.Menu_Tools_Plot,
            this.Menu_Tools_Correlation,
            this.Menu_Tools_Show_Waves,
            this.Menu_Tools_Create_Super_Waves,
            this.Menu_Tool_SW_Set_Correlation,
            this.Menu_Tools_STF});
            this.Menu_Tools.Name = "Menu_Tools";
            this.Menu_Tools.Size = new System.Drawing.Size(56, 24);
            this.Menu_Tools.Text = "Tools";
            // 
            // Menu_Tools_FindWaves
            // 
            this.Menu_Tools_FindWaves.Enabled = false;
            this.Menu_Tools_FindWaves.Name = "Menu_Tools_FindWaves";
            this.Menu_Tools_FindWaves.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_FindWaves.Text = "Wave Searching";
            this.Menu_Tools_FindWaves.Click += new System.EventHandler(this.Menu_Tools_FindWaves_Click);
            // 
            // Menu_Tools_Plot
            // 
            this.Menu_Tools_Plot.Enabled = false;
            this.Menu_Tools_Plot.Name = "Menu_Tools_Plot";
            this.Menu_Tools_Plot.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_Plot.Text = "Plot";
            this.Menu_Tools_Plot.Click += new System.EventHandler(this.Menu_Tools_Plot_Click);
            // 
            // Menu_Tools_Correlation
            // 
            this.Menu_Tools_Correlation.Enabled = false;
            this.Menu_Tools_Correlation.Name = "Menu_Tools_Correlation";
            this.Menu_Tools_Correlation.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_Correlation.Text = "Correlation";
            this.Menu_Tools_Correlation.Click += new System.EventHandler(this.Menu_Tools_Correlation_Click);
            // 
            // Menu_Tools_Show_Waves
            // 
            this.Menu_Tools_Show_Waves.Enabled = false;
            this.Menu_Tools_Show_Waves.Name = "Menu_Tools_Show_Waves";
            this.Menu_Tools_Show_Waves.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_Show_Waves.Text = "Show Waves ";
            this.Menu_Tools_Show_Waves.Click += new System.EventHandler(this.Menu_Tools_Show_Waves_Click);
            // 
            // Menu_Tools_Create_Super_Waves
            // 
            this.Menu_Tools_Create_Super_Waves.Enabled = false;
            this.Menu_Tools_Create_Super_Waves.Name = "Menu_Tools_Create_Super_Waves";
            this.Menu_Tools_Create_Super_Waves.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_Create_Super_Waves.Text = "Create Super Waves";
            this.Menu_Tools_Create_Super_Waves.Click += new System.EventHandler(this.Menu_Tools_Create_Super_Waves_Click);
            // 
            // Menu_Tool_SW_Set_Correlation
            // 
            this.Menu_Tool_SW_Set_Correlation.Name = "Menu_Tool_SW_Set_Correlation";
            this.Menu_Tool_SW_Set_Correlation.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tool_SW_Set_Correlation.Text = "SW Set Correlation";
            this.Menu_Tool_SW_Set_Correlation.Click += new System.EventHandler(this.Menu_Tool_SW_Set_Correlation_Click);
            // 
            // Menu_Tools_STF
            // 
            this.Menu_Tools_STF.Name = "Menu_Tools_STF";
            this.Menu_Tools_STF.Size = new System.Drawing.Size(216, 26);
            this.Menu_Tools_STF.Text = "Save TXT file";
            this.Menu_Tools_STF.Click += new System.EventHandler(this.Menu_Tools_STF_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainView";
            this.Text = "Анализ данных";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_File_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_FindWaves;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Plot;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Correlation;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Show_Waves;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_Create_Super_Waves;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tool_SW_Set_Correlation;
        private System.Windows.Forms.ToolStripMenuItem Menu_Tools_STF;
    }
}

