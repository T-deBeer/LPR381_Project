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
            SuspendLayout();
            // 
            // MainMenu
            // 
            AllowResize = false;
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1180, 632);
            Name = "MainMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Style = MetroSet_UI.Enums.Style.Dark;
            Text = "Main Menu";
            TextColor = Color.FromArgb(65, 177, 225);
            ThemeName = "MetroDark";
            ResumeLayout(false);
        }

        #endregion
    }
}