using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSet_UI.Forms;
using static System.Windows.Forms.LinkLabel;

namespace LPR381_Project
{
    public partial class MainMenu : MetroSetForm
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void pnlDragnDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void pnlDragnDrop_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                string[] lines;
                rtbFileOutput.Text = "";
                foreach (string filePath in droppedFiles)
                {
                    lines = File.ReadAllLines(filePath);
                    foreach (var item in lines)
                    {
                        rtbFileOutput.AppendText(item + "\n");
                    }
                }
            }
        }
    }
}
