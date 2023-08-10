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
        List<string> lp = new List<string>();
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
            lblCAChangeDesc.ForeColor = Color.FromArgb(255, 255, 255);
            lblCAChangePos.ForeColor = Color.FromArgb(255, 255, 255);
            lblShadowPrices.ForeColor = Color.FromArgb(28, 131, 174);
            lblConstraint.ForeColor = Color.FromArgb(28, 131, 174);

            btnDuality.Enabled = false;
            btnCARanges.Enabled = false;
            btnSolve.Enabled = false;
            cboMethod.Enabled = false;
            cboCARangeRow.Enabled = false;
            cboCARangeCol.Enabled = false;
            txtCAChanges.Enabled = false;
            btnCAChanges.Enabled = false;
            btnConstraints.Enabled = false;
            cboConstraintVar.Enabled = false;
            cboConstraintComp.Enabled = false;
            txtConstraintVal.Enabled = false;
            btnShadowPrices.Enabled = false;
            cboShadowPriceVar.Enabled = false;

            cbForm.Location = new System.Drawing.Point(1164, 4);
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
                lp.Clear();
                foreach (string filePath in droppedFiles)
                {
                    lines = File.ReadAllLines(filePath);
                    foreach (var item in lines)
                    {
                        rtbFileOutput.AppendText(item + "\n");
                        lp.Add(item);
                    }
                }
                btnSolve.Enabled = true;
                cboMethod.Enabled = true;
                btnDuality.Enabled = true;
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

            //double[,] table = lp.SimplexTables[lp.SimplexTables.Count - 1];

            //for (int i = 0; i < table.GetLength(0); i++)
            //{
            //    string res = "";
            //    for (int j = 0; j < table.GetLength(1); j++)
            //    {
            //        res += $"{table[i, j]}\t";
            //    }
            //    rtbFileOutput.Text += $"{res}\n";
            //}
            btnSolve.Enabled = true;
            cboMethod.Enabled = true;
            cboMethod.SelectedIndex = 0;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            LinearModel lm = new LinearModel(lp.ToArray());
            switch (cboMethod.SelectedIndex)
            {
                case 0:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Some of the values in this Linear Programming model are either Integer (int) or binary (bin). \n" +
                            "The Primal Simplex Algorithm will not be able to satisfy these value restrictions but will still continue to solve it as unrestricted.\n" +
                            "Try using the 'Branch and Bound' or 'Cutting Plane' algorithms for values that have int or bin.", "Sign Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Simplex sp = new Simplex(lm.SimplexInitial, lm.ProblemType);
                    rtbOutput.Text = sp.PrintPrimal();
                    break;
                case 1:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Some of the values in this Linear Programming model are either Integer (int) or binary (bin). \n" +
                            "The Two-Phase Simplex alogrithm will not be able to satisfy these value restrictions but will still continue to solve it as unrestricted.\n" +
                            "Try using the 'Branch and Bound' or 'Cutting Plane' algorithms for values that have int or bin.", "Sign Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Simplex st = new Simplex(lm.TwoPhaseInitial, lm.ProblemType);
                    List<double[,]> result = st.TwoPhaseAlgorithm(lm.TwoPhaseArtificialColumns);
                    rtbOutput.Text = st.PrintTwoPhase(result);
                    break;
                case 2:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Some of the values in this Linear Programming model are either Integer (int) or binary (bin). \n" +
                            "The Dual Simplex alogrithm will not be able to satisfy these value restrictions but will still continue to solve it as unrestricted.\n" +
                            "Try using the 'Branch and Bound' or 'Cutting Plane' algorithms for values that have int or bin.", "Sign Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Simplex sd = new Simplex(lm.SimplexInitial, lm.ProblemType);
                    rtbOutput.Text = sd.PrintDual();
                    break;
                case 3:
                    // Branch and Bound
                    break;
                case 4:
                    CuttingPlane cp = new CuttingPlane(lm.SimplexInitial, lm.ProblemType, lm.SignRes.ToArray());
                    rtbOutput.Text = cp.PrintResults();
                    break;
                default:
                    MessageBox.Show("Invalid method selected, please try another method.", "Method Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnDuality_Click(object sender, EventArgs e)
        {
            rtbOutput.Text = "";
            LinearModel lm = new LinearModel(lp.ToArray());
            rtbOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
            rtbOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");
            Simplex s = new Simplex(lm.DualityInitial, lm.DualProblemType);
            rtbOutput.AppendText("\n" + s.PrintDual() + "\n");
        }

        public bool CheckFeasibility(List<double[,]> result)
        {
            double[,] finalTable = result.ElementAt(result.Count - 1);
            // Infeasible solution.
            double sum = 0;
            for (int i = 0; i < finalTable.GetLength(0); i++)
            {
                for (int j = 0; j < finalTable.GetLength(1); j++)
                {
                    sum += finalTable[i, j];
                }
            }
            if (sum == 0)
            {
                MessageBox.Show("The solution is infeasible given the problem and the method used.\n\nTry another method or inspect the Linear programming problem.", "Infeasible solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Unbounded solution.
            for (int i = 0; i < finalTable.GetLength(1); i++)
            {
                if (finalTable[0,i] < 0)
                {
                    MessageBox.Show("The solution is unbounded given the problem.\n\nTry inspecing the Linear programming problem for any variables that can be unbounded.", "Unbounded solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            // Multiple solutions.
            for (int i = 0; i < finalTable.GetLength(1); i++)
            {
                if (finalTable[0,i] == 0)
                {
                    sum = 0;
                    for (int j = 0; j < finalTable.GetLength(0); j++)
                    {
                        sum += finalTable[j, i];
                    }
                    if (sum != 1)
                    {
                        MessageBox.Show("There are multiple solutions for the given problem.\n\nKeep in mind only one of these possible solutions will be displayed.", "Multiple solutions", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            return true;
        }
    }
}
