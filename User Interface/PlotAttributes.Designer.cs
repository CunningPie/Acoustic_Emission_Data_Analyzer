namespace AEDataAnalyzer.User_Interface
{
    partial class PlotAttributes
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
            this.Attributes = new System.Windows.Forms.GroupBox();
            this.Amplitude = new System.Windows.Forms.CheckBox();
            this.Energy = new System.Windows.Forms.CheckBox();
            this.Time = new System.Windows.Forms.CheckBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Attributes.SuspendLayout();
            this.SuspendLayout();
            // 
            // Attributes
            // 
            this.Attributes.Controls.Add(this.Time);
            this.Attributes.Controls.Add(this.Energy);
            this.Attributes.Controls.Add(this.Amplitude);
            this.Attributes.Location = new System.Drawing.Point(12, 12);
            this.Attributes.Name = "Attributes";
            this.Attributes.Size = new System.Drawing.Size(212, 117);
            this.Attributes.TabIndex = 0;
            this.Attributes.TabStop = false;
            this.Attributes.Text = "Attributes";
            // 
            // Amplitude
            // 
            this.Amplitude.AutoSize = true;
            this.Amplitude.Checked = true;
            this.Amplitude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Amplitude.Location = new System.Drawing.Point(7, 22);
            this.Amplitude.Name = "Amplitude";
            this.Amplitude.Size = new System.Drawing.Size(92, 21);
            this.Amplitude.TabIndex = 0;
            this.Amplitude.Text = "Amplitude";
            this.Amplitude.UseVisualStyleBackColor = true;
            // 
            // Energy
            // 
            this.Energy.AutoSize = true;
            this.Energy.Location = new System.Drawing.Point(7, 50);
            this.Energy.Name = "Energy";
            this.Energy.Size = new System.Drawing.Size(75, 21);
            this.Energy.TabIndex = 1;
            this.Energy.Text = "Energy";
            this.Energy.UseVisualStyleBackColor = true;
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.Location = new System.Drawing.Point(7, 78);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(61, 21);
            this.Time.TabIndex = 2;
            this.Time.Text = "Time";
            this.Time.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(12, 135);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(100, 25);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(124, 135);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 25);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // PlotAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 176);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Attributes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PlotAttributes";
            this.Text = "PlotAttributes";
            this.Attributes.ResumeLayout(false);
            this.Attributes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Attributes;
        private System.Windows.Forms.CheckBox Time;
        private System.Windows.Forms.CheckBox Energy;
        private System.Windows.Forms.CheckBox Amplitude;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}