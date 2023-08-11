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

            cbForm.Location = new System.Drawing.Point(1164, 4);
            //Testing here
            List<BranchTable> branchTables = new List<BranchTable>
            {
                new BranchTable("1", new double[,] { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("11", new double[,] { { 11, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("12", new double[,] { { 12, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("111", new double[,] { { 111, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("112", new double[,] { { 112, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("121", new double[,] { { 121, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("122", new double[,] { { 122, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("2", new double[,] { { 2, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("21", new double[,] { { 21, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("22", new double[,] { { 22, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("211", new double[,] { { 211, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("212", new double[,] { { 212, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("221", new double[,] { { 221, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("222", new double[,] { { 222, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
            };
            List<BranchTable> placementQueue = branchTables.OrderBy(x => x.Level.Length).ToList();

            List<BranchTable> sub1 = branchTables.Where(x => x.Level[0] == '1').ToList();

            List<BranchTable> sub2 = branchTables.Where(x => x.Level[0] == '2').ToList();


            double x = Math.Pow(2, placementQueue.Last().Level.Length) / 2;
            int y = placementQueue.Select(x => x.Level.Length).Distinct().Count();

            int width = (int) x * placementQueue.Last().DataGrid.ClientSize.Width + 100;
            int height = y * placementQueue.Last().DataGrid.ClientSize.Height + 100;

            int tableWidth = placementQueue.Last().DataGrid.Width;

            for (int i = 0; i < sub1.Count(); i++)
            {
                string level = sub1[i].Level;

                sub1[i].DataGrid.Left = width / 2 - sub1[i].DataGrid.Width / 2;
                sub1[i].DataGrid.Top += sub1.Last().DataGrid.Height + 50;

                for (int j = 1; j < sub1[i].Level.Length; j++)
                {
                    sub1[i].DataGrid.Top += sub1.Last().DataGrid.Height + 50;
                    sub1[i].DataGrid.Left += (sub1[i].Level[j] == '1' ? -1 : 1) * (pnlBranches.Width / (int)Math.Pow(2, j)) / 2;
                }
                pnlBranches.Controls.Add(sub1[i].DataGrid);
                sub1[i].DataGrid.CurrentCell = null;
            }
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
