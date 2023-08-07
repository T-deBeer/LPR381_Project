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

        private void MainMenu_Load(object sender, EventArgs e)
        {
            rtbFileOutput.BackColor = Color.FromArgb(30, 30, 30);
            rtbFileOutput.BorderColor = Color.FromArgb(30, 30, 30);

            rtbOutput.BackColor = Color.FromArgb(30, 30, 30);
            rtbOutput.BorderColor = Color.FromArgb(30, 30, 30);

            lblCAChanges.ForeColor = Color.FromArgb(28, 131, 174);
            lblCADual.ForeColor = Color.FromArgb(28, 131, 174);
            lblCARanges.ForeColor = Color.FromArgb(28, 131, 174);
            lblCASolution.ForeColor = Color.FromArgb(28, 131, 174);
            lblImport.ForeColor = Color.FromArgb(28, 131, 174);
            lblFileOutput.ForeColor = Color.FromArgb(28, 131, 174);
            lblSolution.ForeColor = Color.FromArgb(28, 131, 174);
            lblSolve.ForeColor = Color.FromArgb(28, 131, 174);

            btnDuality.Enabled = false;
            btnCARanges.Enabled = false;
            btnSolve.Enabled = false;
            cboMethod.Enabled = false;
            cboCARangeRow.Enabled = false;
            cboCARangeCol.Enabled = false;
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
                btnSolve.Enabled = true;
                cboMethod.Enabled = true;
                cboMethod.SelectedIndex = 0;
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string[] lines = Array.Empty<string>();

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filePath = ofd.FileName;

                    lines = File.ReadAllLines(filePath);
                }
            }

            LinearModel lp = new LinearModel(lines);
            rtbFileOutput.Text += "SIMPLEX CANONICAL FORM:\n";
            rtbFileOutput.Text += (lp.CanonObjFunctionToString());
            rtbFileOutput.Text += ("\nsubject to:\n");
            rtbFileOutput.Text += (lp.CanonSimplexConstraintsToString());
            rtbFileOutput.Text += "\n\n";

            double[,] table = lp.SimplexTables[lp.SimplexTables.Count - 1];

            for (int i = 0; i < table.GetLength(0); i++)
            {
                string res = "";
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    res += $"{table[i, j]}\t";
                }
                rtbFileOutput.Text += $"{res}\n";
            }
            btnSolve.Enabled = true;
            cboMethod.Enabled = true;
            cboMethod.SelectedIndex = 0;
        }
    }
}
