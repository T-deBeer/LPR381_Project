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
            mbtnSelectFile = new MetroSet_UI.Controls.MetroSetButton();
            rtbOutput = new MetroSet_UI.Controls.MetroSetRichTextBox();
            SuspendLayout();
            // 
            // mbtnSelectFile
            // 
            mbtnSelectFile.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            mbtnSelectFile.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            mbtnSelectFile.DisabledForeColor = Color.Gray;
            mbtnSelectFile.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            mbtnSelectFile.HoverBorderColor = Color.FromArgb(95, 207, 255);
            mbtnSelectFile.HoverColor = Color.FromArgb(95, 207, 255);
            mbtnSelectFile.HoverTextColor = Color.White;
            mbtnSelectFile.IsDerivedStyle = true;
            mbtnSelectFile.Location = new Point(15, 126);
            mbtnSelectFile.Name = "mbtnSelectFile";
            mbtnSelectFile.NormalBorderColor = Color.FromArgb(65, 177, 225);
            mbtnSelectFile.NormalColor = Color.FromArgb(65, 177, 225);
            mbtnSelectFile.NormalTextColor = Color.White;
            mbtnSelectFile.PressBorderColor = Color.FromArgb(35, 147, 195);
            mbtnSelectFile.PressColor = Color.FromArgb(35, 147, 195);
            mbtnSelectFile.PressTextColor = Color.White;
            mbtnSelectFile.Size = new Size(184, 43);
            mbtnSelectFile.Style = MetroSet_UI.Enums.Style.Light;
            mbtnSelectFile.StyleManager = null;
            mbtnSelectFile.TabIndex = 0;
            mbtnSelectFile.Text = "Select Text File";
            mbtnSelectFile.ThemeAuthor = "Narwin";
            mbtnSelectFile.ThemeName = "MetroLite";
            mbtnSelectFile.Click += mbtnSelectFile_Click;
            // 
            // rtbOutput
            // 
            rtbOutput.AutoWordSelection = false;
            rtbOutput.BorderColor = Color.FromArgb(155, 155, 155);
            rtbOutput.DisabledBackColor = Color.FromArgb(204, 204, 204);
            rtbOutput.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            rtbOutput.DisabledForeColor = Color.FromArgb(136, 136, 136);
            rtbOutput.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            rtbOutput.HoverColor = Color.FromArgb(102, 102, 102);
            rtbOutput.IsDerivedStyle = true;
            rtbOutput.Lines = null;
            rtbOutput.Location = new Point(309, 126);
            rtbOutput.MaxLength = 32767;
            rtbOutput.Name = "rtbOutput";
            rtbOutput.ReadOnly = false;
            rtbOutput.Size = new Size(856, 426);
            rtbOutput.Style = MetroSet_UI.Enums.Style.Light;
            rtbOutput.StyleManager = null;
            rtbOutput.TabIndex = 1;
            rtbOutput.ThemeAuthor = "Narwin";
            rtbOutput.ThemeName = "MetroLite";
            rtbOutput.WordWrap = true;
            // 
            // MainMenu
            // 
            AllowResize = false;
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1180, 632);
            Controls.Add(rtbOutput);
            Controls.Add(mbtnSelectFile);
            Name = "MainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Style = MetroSet_UI.Enums.Style.Dark;
            Text = "Main Menu";
            TextColor = Color.FromArgb(65, 177, 225);
            ThemeName = "MetroDark";
            ResumeLayout(false);
        }

        #endregion

        private MetroSet_UI.Controls.MetroSetButton mbtnSelectFile;
        private MetroSet_UI.Controls.MetroSetRichTextBox rtbOutput;
    }
}