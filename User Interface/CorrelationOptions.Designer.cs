namespace AEDataAnalyzer
{
    partial class CorrelationOptions
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
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Options = new System.Windows.Forms.GroupBox();
            this.CountsDuration = new System.Windows.Forms.CheckBox();
            this.Time = new System.Windows.Forms.CheckBox();
            this.Energy = new System.Windows.Forms.CheckBox();
            this.Amplitude = new System.Windows.Forms.CheckBox();
            this.CorrelationType = new System.Windows.Forms.GroupBox();
            this.FechnerCoeff = new System.Windows.Forms.CheckBox();
            this.PearsonCoeff = new System.Windows.Forms.CheckBox();
            this.Operation = new System.Windows.Forms.GroupBox();
            this.Mult = new System.Windows.Forms.CheckBox();
            this.Sum = new System.Windows.Forms.CheckBox();
            this.Options.SuspendLayout();
            this.CorrelationType.SuspendLayout();
            this.Operation.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(286, 156);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(100, 26);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(392, 156);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 26);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Options
            // 
            this.Options.Controls.Add(this.CountsDuration);
            this.Options.Controls.Add(this.Time);
            this.Options.Controls.Add(this.Energy);
            this.Options.Controls.Add(this.Amplitude);
            this.Options.Location = new System.Drawing.Point(22, 13);
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(147, 137);
            this.Options.TabIndex = 3;
            this.Options.TabStop = false;
            this.Options.Text = "Options";
            // 
            // CountsDuration
            // 
            this.CountsDuration.AutoSize = true;
            this.CountsDuration.Location = new System.Drawing.Point(7, 106);
            this.CountsDuration.Name = "CountsDuration";
            this.CountsDuration.Size = new System.Drawing.Size(132, 21);
            this.CountsDuration.TabIndex = 3;
            this.CountsDuration.Text = "Counts/Duration";
            this.CountsDuration.UseVisualStyleBackColor = true;
            this.CountsDuration.CheckedChanged += new System.EventHandler(this.Counts_CheckedChanged);
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
            // CorrelationType
            // 
            this.CorrelationType.Controls.Add(this.FechnerCoeff);
            this.CorrelationType.Controls.Add(this.PearsonCoeff);
            this.CorrelationType.Location = new System.Drawing.Point(187, 13);
            this.CorrelationType.Name = "CorrelationType";
            this.CorrelationType.Size = new System.Drawing.Size(179, 137);
            this.CorrelationType.TabIndex = 4;
            this.CorrelationType.TabStop = false;
            this.CorrelationType.Text = "Correlation Type";
            // 
            // FechnerCoeff
            // 
            this.FechnerCoeff.AutoSize = true;
            this.FechnerCoeff.Location = new System.Drawing.Point(7, 50);
            this.FechnerCoeff.Name = "FechnerCoeff";
            this.FechnerCoeff.Size = new System.Drawing.Size(152, 21);
            this.FechnerCoeff.TabIndex = 1;
            this.FechnerCoeff.Text = "Fechner Coefficient";
            this.FechnerCoeff.UseVisualStyleBackColor = true;
            // 
            // PearsonCoeff
            // 
            this.PearsonCoeff.AutoSize = true;
            this.PearsonCoeff.Checked = true;
            this.PearsonCoeff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PearsonCoeff.Location = new System.Drawing.Point(6, 22);
            this.PearsonCoeff.Name = "PearsonCoeff";
            this.PearsonCoeff.Size = new System.Drawing.Size(153, 21);
            this.PearsonCoeff.TabIndex = 0;
            this.PearsonCoeff.Text = "Pearson Coefficient";
            this.PearsonCoeff.UseVisualStyleBackColor = true;
            // 
            // Operation
            // 
            this.Operation.Controls.Add(this.Mult);
            this.Operation.Controls.Add(this.Sum);
            this.Operation.Location = new System.Drawing.Point(373, 13);
            this.Operation.Name = "Operation";
            this.Operation.Size = new System.Drawing.Size(119, 137);
            this.Operation.TabIndex = 5;
            this.Operation.TabStop = false;
            this.Operation.Text = "Operation";
            // 
            // Mult
            // 
            this.Mult.AutoSize = true;
            this.Mult.Location = new System.Drawing.Point(7, 50);
            this.Mult.Name = "Mult";
            this.Mult.Size = new System.Drawing.Size(56, 21);
            this.Mult.TabIndex = 1;
            this.Mult.Text = "Mult";
            this.Mult.UseVisualStyleBackColor = true;
            this.Mult.CheckedChanged += new System.EventHandler(this.Mult_CheckedChanged);
            // 
            // Sum
            // 
            this.Sum.AutoSize = true;
            this.Sum.Checked = true;
            this.Sum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Sum.Location = new System.Drawing.Point(7, 22);
            this.Sum.Name = "Sum";
            this.Sum.Size = new System.Drawing.Size(58, 21);
            this.Sum.TabIndex = 0;
            this.Sum.Text = "Sum";
            this.Sum.UseVisualStyleBackColor = true;
            this.Sum.CheckedChanged += new System.EventHandler(this.Sum_CheckedChanged);
            // 
            // CorrelationOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 194);
            this.Controls.Add(this.Operation);
            this.Controls.Add(this.CorrelationType);
            this.Controls.Add(this.Options);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CorrelationOptions";
            this.Text = "CoefficientList";
            this.Options.ResumeLayout(false);
            this.Options.PerformLayout();
            this.CorrelationType.ResumeLayout(false);
            this.CorrelationType.PerformLayout();
            this.Operation.ResumeLayout(false);
            this.Operation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox Options;
        private System.Windows.Forms.CheckBox Time;
        private System.Windows.Forms.CheckBox Energy;
        private System.Windows.Forms.CheckBox Amplitude;
        private System.Windows.Forms.GroupBox CorrelationType;
        private System.Windows.Forms.CheckBox FechnerCoeff;
        private System.Windows.Forms.CheckBox PearsonCoeff;
        private System.Windows.Forms.GroupBox Operation;
        private System.Windows.Forms.CheckBox Mult;
        private System.Windows.Forms.CheckBox Sum;
        private System.Windows.Forms.CheckBox CountsDuration;
    }
}