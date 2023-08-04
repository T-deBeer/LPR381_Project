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
            metroSetButton1 = new MetroSet_UI.Controls.MetroSetButton();
            metroSetButton2 = new MetroSet_UI.Controls.MetroSetButton();
            metroSetRichTextBox1 = new MetroSet_UI.Controls.MetroSetRichTextBox();
            SuspendLayout();
            // 
            // metroSetButton1
            // 
            metroSetButton1.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton1.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton1.DisabledForeColor = Color.Gray;
            metroSetButton1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetButton1.HoverBorderColor = Color.FromArgb(95, 207, 255);
            metroSetButton1.HoverColor = Color.FromArgb(95, 207, 255);
            metroSetButton1.HoverTextColor = Color.White;
            metroSetButton1.IsDerivedStyle = true;
            metroSetButton1.Location = new Point(15, 227);
            metroSetButton1.Name = "metroSetButton1";
            metroSetButton1.NormalBorderColor = Color.FromArgb(65, 177, 225);
            metroSetButton1.NormalColor = Color.FromArgb(65, 177, 225);
            metroSetButton1.NormalTextColor = Color.White;
            metroSetButton1.PressBorderColor = Color.FromArgb(35, 147, 195);
            metroSetButton1.PressColor = Color.FromArgb(35, 147, 195);
            metroSetButton1.PressTextColor = Color.White;
            metroSetButton1.Size = new Size(115, 39);
            metroSetButton1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetButton1.StyleManager = null;
            metroSetButton1.TabIndex = 0;
            metroSetButton1.Text = "metroSetButton1";
            metroSetButton1.ThemeAuthor = "Narwin";
            metroSetButton1.ThemeName = "MetroLite";
            // 
            // metroSetButton2
            // 
            metroSetButton2.DisabledBackColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton2.DisabledBorderColor = Color.FromArgb(120, 65, 177, 225);
            metroSetButton2.DisabledForeColor = Color.Gray;
            metroSetButton2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetButton2.HoverBorderColor = Color.FromArgb(95, 207, 255);
            metroSetButton2.HoverColor = Color.FromArgb(95, 207, 255);
            metroSetButton2.HoverTextColor = Color.White;
            metroSetButton2.IsDerivedStyle = true;
            metroSetButton2.Location = new Point(172, 227);
            metroSetButton2.Name = "metroSetButton2";
            metroSetButton2.NormalBorderColor = Color.FromArgb(65, 177, 225);
            metroSetButton2.NormalColor = Color.FromArgb(65, 177, 225);
            metroSetButton2.NormalTextColor = Color.White;
            metroSetButton2.PressBorderColor = Color.FromArgb(35, 147, 195);
            metroSetButton2.PressColor = Color.FromArgb(35, 147, 195);
            metroSetButton2.PressTextColor = Color.White;
            metroSetButton2.Size = new Size(115, 39);
            metroSetButton2.Style = MetroSet_UI.Enums.Style.Light;
            metroSetButton2.StyleManager = null;
            metroSetButton2.TabIndex = 1;
            metroSetButton2.Text = "metroSetButton2";
            metroSetButton2.ThemeAuthor = "Narwin";
            metroSetButton2.ThemeName = "MetroLite";
            // 
            // metroSetRichTextBox1
            // 
            metroSetRichTextBox1.AutoWordSelection = false;
            metroSetRichTextBox1.BorderColor = Color.FromArgb(155, 155, 155);
            metroSetRichTextBox1.DisabledBackColor = Color.FromArgb(204, 204, 204);
            metroSetRichTextBox1.DisabledBorderColor = Color.FromArgb(155, 155, 155);
            metroSetRichTextBox1.DisabledForeColor = Color.FromArgb(136, 136, 136);
            metroSetRichTextBox1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
            metroSetRichTextBox1.HoverColor = Color.FromArgb(102, 102, 102);
            metroSetRichTextBox1.IsDerivedStyle = true;
            metroSetRichTextBox1.Lines = null;
            metroSetRichTextBox1.Location = new Point(15, 85);
            metroSetRichTextBox1.MaxLength = 32767;
            metroSetRichTextBox1.Name = "metroSetRichTextBox1";
            metroSetRichTextBox1.ReadOnly = false;
            metroSetRichTextBox1.Size = new Size(272, 126);
            metroSetRichTextBox1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetRichTextBox1.StyleManager = null;
            metroSetRichTextBox1.TabIndex = 2;
            metroSetRichTextBox1.Text = "metroSetRichTextBox1";
            metroSetRichTextBox1.ThemeAuthor = "Narwin";
            metroSetRichTextBox1.ThemeName = "MetroLite";
            metroSetRichTextBox1.WordWrap = true;
            // 
            // MainMenu
            // 
            AllowResize = false;
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1180, 632);
            Controls.Add(metroSetRichTextBox1);
            Controls.Add(metroSetButton2);
            Controls.Add(metroSetButton1);
            Name = "MainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Style = MetroSet_UI.Enums.Style.Dark;
            Text = "Main Menu";
            TextColor = Color.FromArgb(65, 177, 225);
            ThemeName = "MetroDark";
            ResumeLayout(false);
        }

        #endregion

        private MetroSet_UI.Controls.MetroSetButton metroSetButton1;
        private MetroSet_UI.Controls.MetroSetButton metroSetButton2;
        private MetroSet_UI.Controls.MetroSetRichTextBox metroSetRichTextBox1;
    }
}