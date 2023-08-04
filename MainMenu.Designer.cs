namespace LPR381_Project
{
    partial class MainMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCutting = new System.Windows.Forms.Button();
            this.rtbResults = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnCutting
            // 
            this.btnCutting.ForeColor = System.Drawing.Color.Black;
            this.btnCutting.Location = new System.Drawing.Point(28, 123);
            this.btnCutting.Name = "btnCutting";
            this.btnCutting.Size = new System.Drawing.Size(95, 42);
            this.btnCutting.TabIndex = 0;
            this.btnCutting.Text = "Cutting Plane";
            this.btnCutting.UseVisualStyleBackColor = true;
            this.btnCutting.Click += new System.EventHandler(this.btnCutting_click);
            // 
            // rtbResults
            // 
            this.rtbResults.Location = new System.Drawing.Point(225, 123);
            this.rtbResults.Name = "rtbResults";
            this.rtbResults.Size = new System.Drawing.Size(827, 355);
            this.rtbResults.TabIndex = 1;
            this.rtbResults.Text = "\n";
            // 
            // MainMenu
            // 
            this.AllowResize = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1180, 632);
            this.Controls.Add(this.rtbResults);
            this.Controls.Add(this.btnCutting);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style = MetroSet_UI.Enums.Style.Dark;
            this.Text = "Main Menu";
            this.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.ThemeName = "MetroDark";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCutting;
        private RichTextBox rtbResults;
    }
}