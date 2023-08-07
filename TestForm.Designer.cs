namespace LPR381_Project
{
    partial class TestForm
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
            this.mbtnSelectFile = new MetroSet_UI.Controls.MetroSetButton();
            this.rtbOutput = new MetroSet_UI.Controls.MetroSetRichTextBox();
            this.SuspendLayout();
            // 
            // mbtnSelectFile
            // 
            this.mbtnSelectFile.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.mbtnSelectFile.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.mbtnSelectFile.DisabledForeColor = System.Drawing.Color.Gray;
            this.mbtnSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mbtnSelectFile.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(207)))), ((int)(((byte)(255)))));
            this.mbtnSelectFile.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(207)))), ((int)(((byte)(255)))));
            this.mbtnSelectFile.HoverTextColor = System.Drawing.Color.White;
            this.mbtnSelectFile.IsDerivedStyle = true;
            this.mbtnSelectFile.Location = new System.Drawing.Point(15, 126);
            this.mbtnSelectFile.Name = "mbtnSelectFile";
            this.mbtnSelectFile.NormalBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.mbtnSelectFile.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.mbtnSelectFile.NormalTextColor = System.Drawing.Color.White;
            this.mbtnSelectFile.PressBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(147)))), ((int)(((byte)(195)))));
            this.mbtnSelectFile.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(147)))), ((int)(((byte)(195)))));
            this.mbtnSelectFile.PressTextColor = System.Drawing.Color.White;
            this.mbtnSelectFile.Size = new System.Drawing.Size(184, 43);
            this.mbtnSelectFile.Style = MetroSet_UI.Enums.Style.Light;
            this.mbtnSelectFile.StyleManager = null;
            this.mbtnSelectFile.TabIndex = 0;
            this.mbtnSelectFile.Text = "Select Text File";
            this.mbtnSelectFile.ThemeAuthor = "Narwin";
            this.mbtnSelectFile.ThemeName = "MetroLite";
            // 
            // rtbOutput
            // 
            this.rtbOutput.AutoWordSelection = false;
            this.rtbOutput.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.rtbOutput.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.rtbOutput.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.rtbOutput.DisabledForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.rtbOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbOutput.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.rtbOutput.IsDerivedStyle = true;
            this.rtbOutput.Lines = null;
            this.rtbOutput.Location = new System.Drawing.Point(309, 126);
            this.rtbOutput.MaxLength = 32767;
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = false;
            this.rtbOutput.Size = new System.Drawing.Size(856, 426);
            this.rtbOutput.Style = MetroSet_UI.Enums.Style.Light;
            this.rtbOutput.StyleManager = null;
            this.rtbOutput.TabIndex = 1;
            this.rtbOutput.ThemeAuthor = "Narwin";
            this.rtbOutput.ThemeName = "MetroLite";
            this.rtbOutput.WordWrap = true;
            // 
            // MainMenu
            // 
            this.AllowResize = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1180, 632);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.mbtnSelectFile);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style = MetroSet_UI.Enums.Style.Dark;
            this.Text = "Main Menu";
            this.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.ThemeName = "MetroDark";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroSet_UI.Controls.MetroSetButton mbtnSelectFile;
        private MetroSet_UI.Controls.MetroSetRichTextBox rtbOutput;
    }
}