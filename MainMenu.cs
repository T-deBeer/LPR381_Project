using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private void PrintTables(List<BranchTable>? tables = null, bool branchBound = false)
        {
            pnlBranches.Controls.Clear();
            tables = new List<BranchTable>
            {
                new BranchTable("1", new double[,] { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("6", new double[,] { { 6, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("2", new double[,] { { 2, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("3", new double[,] { { 3, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("5", new double[,] { { 5, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("4", new double[,] { { 4, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("7", new double[,] { { 7, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("8", new double[,] { { 8, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
                new BranchTable("9", new double[,] { { 9, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } }),
            };

            if (branchBound == false)
            {
                tables = tables.OrderBy(x => int.Parse(x.Level)).ToList();
                int top = 20;
                foreach (BranchTable table in tables)
                {
                    table.DataGrid.Top = top;
                    table.DataGrid.Left = pnlBranches.Width / 2 - table.DataGrid.Width / 2;
                    top += table.DataGrid.Height + 40;
                    pnlBranches.Controls.Add(table.DataGrid);
                    table.DataGrid.CurrentCell = null;
                }
            }
            else
            {
                List<BranchTable> placementQueue = tables.OrderBy(x => x.Level.Length).ToList();

                List<BranchTable> sub1 = tables.Where(x => x.Level[0] == '1').ToList();

                List<BranchTable> sub2 = tables.Where(x => x.Level[0] == '2').ToList();


                double x = Math.Pow(2, placementQueue.Last().Level.Length) / 2;
                int y = placementQueue.Select(x => x.Level.Length).Distinct().Count();

                int width = (int)x * placementQueue.Last().DataGrid.ClientSize.Width + 100;
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
            lblShadowPrices.ForeColor = Color.FromArgb(28, 131, 174);
            lblConstraint.ForeColor = Color.FromArgb(28, 131, 174);

            btnDuality.Enabled = false;
            btnCARanges.Enabled = false;
            btnSolve.Enabled = false;
            cboMethod.Enabled = false;
            cboCARangeRow.Enabled = false;
            cboCARangeCol.Enabled = false;
            txtCAChangeValue.Enabled = false;
            btnCAChanges.Enabled = false;
            btnConstraints.Enabled = false;
            btnShadowPrices.Enabled = false;
            cboShadowPriceVar.Enabled = false;
            txtCAChangeValue.Enabled = false;

            cbForm.Location = new System.Drawing.Point(1816, 4);
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

            int width = (int)x * placementQueue.Last().DataGrid.ClientSize.Width + 100;
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
                lp.Clear();
                foreach (string filePath in droppedFiles)
                {
                    lines = File.ReadAllLines(filePath);
                    cboCAChangeRow.Items.Clear();
                    foreach (var item in lines)
                    {
                        rtbFileOutput.AppendText(item + "\n");
                        lp.Add(item);
                        cboCAChangeRow.Items.Add(item);
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
            lp.Clear();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filePath = ofd.FileName;

                    lines = File.ReadAllLines(filePath);
                }
            }
            rtbFileOutput.Text = "";
            cboCAChangeRow.Items.Clear();
            foreach (var item in lines)
            {
                rtbFileOutput.AppendText(item + "\n");
                lp.Add(item);
                cboCAChangeRow.Items.Add(item);
            }

            btnSolve.Enabled = true;
            cboMethod.Enabled = true;
            cboMethod.SelectedIndex = 0;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            btnConstraints.Enabled = true;
            mtxtCon.Enabled = true;

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
                    EnableElements();
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
                    EnableElements();
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
                    EnableElements();
                    break;
                case 3:
                    // Branch and Bound
                    break;
                case 4:
                    CuttingPlane cp = new CuttingPlane(lm.SimplexInitial, lm.ProblemType, lm.SignRes.ToArray());
                    rtbOutput.Text = cp.PrintResults();
                    EnableElements();
                    break;
                default:
                    MessageBox.Show("Invalid method selected, please try another method.", "Method Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            void EnableElements()
            {
                btnDuality.Enabled = true;

                cboCAChangeRow.Enabled = true;
                cboCAChangeRow.SelectedIndex = 0;
                txtCAChangeValue.Enabled = true;
                btnCAChanges.Enabled = true;

                btnCARanges.Enabled = true;
                cboCARangeCol.Enabled = true;
                cboCARangeRow.Enabled = true;

                btnShadowPrices.Enabled = true;
                cboShadowPriceVar.Enabled = true;
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
                if (finalTable[0, i] < 0)
                {
                    MessageBox.Show("The solution is unbounded given the problem.\n\nTry inspecing the Linear programming problem for any variables that can be unbounded.", "Unbounded solution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            // Multiple solutions.
            for (int i = 0; i < finalTable.GetLength(1); i++)
            {
                if (finalTable[0, i] == 0)
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

        private void btnConstraints_Click(object sender, EventArgs e)
        {
            List<string> constraintToAdd = new List<string>(mtxtCon.Text.Split(" "));

            LinearModel lm = new LinearModel(lp.ToArray());
            Simplex solution = new Simplex(lm.SimplexInitial, lm.ProblemType);

            List<double[,]> solutionTables = solution.DualSimplexAlgorithm();
            double[,] finalTable = solutionTables[solutionTables.Count - 1];


            //Constraint variable
            Dictionary<string, string> newConstraint = new Dictionary<string, string>();
            List<Dictionary<string, string>> tempConstraints = new List<Dictionary<string, string>>();
            List<int> basicVariables = new List<int>();

            if (constraintToAdd.Count != lm.ObjectiveFunction.Count + 2)
            {
                MessageBox.Show("Please ensure that the form of the constraint mathces the form of the original model.");
            }
            else
            {

                for (int i = 0; i < finalTable.GetLength(1); i++)
                {
                    double sum = 0;
                    for (int j = 0; j < finalTable.GetLength(0); j++)
                    {
                        sum += finalTable[j, i];
                    }

                    if (sum == 1)
                    {
                        basicVariables.Add(i);
                    }
                }

                newConstraint.Add("sign", constraintToAdd[constraintToAdd.Count - 2]);
                newConstraint.Add("rhs", constraintToAdd[constraintToAdd.Count - 1]);

                constraintToAdd.RemoveAt(constraintToAdd.Count - 2);
                constraintToAdd.RemoveAt(constraintToAdd.Count - 1);

                for (int i = 0; i < constraintToAdd.Count; i++)
                {
                    newConstraint.Add($"X{i + 1}", constraintToAdd[i]);
                }

                //Add Slack and Excess variables
                switch (newConstraint["sign"])
                {
                    case "<=":
                        {
                            newConstraint.Add("S", "+1");
                        }
                        break;

                    case ">=":
                        {
                            newConstraint.Add("E", "+1");
                            foreach (var kvp in newConstraint.Where(x => x.Key.Contains('X') || x.Key.Contains("rhs")))
                            {
                                newConstraint[kvp.Key] = (double.Parse(newConstraint[kvp.Key]) * -1).ToString();
                            }
                        }
                        break;

                    case "=":
                        {
                            Dictionary<string, string> temp = new Dictionary<string, string>(newConstraint);
                            newConstraint.Add("S", "+1");

                            foreach (var kvp in temp.Where(x => x.Key.Contains('X') || x.Key.Contains("rhs")))
                            {
                                temp[kvp.Key] = (double.Parse(temp[kvp.Key]) * -1).ToString();
                            }

                            temp.Add("E", "+1");

                            tempConstraints.Add(temp);
                        }
                        break;
                }

                //Create table entry for new constraint
                double[] newConstraintRow = new double[finalTable.GetLength(1) + 1];

                int count = 0;
                foreach (var kvp in newConstraint.Where(x => x.Key.Contains('X')))
                {
                    newConstraintRow[count] = double.Parse(newConstraint[kvp.Key]);
                    count++;
                }

                newConstraintRow[finalTable.GetLength(1) - 1] = 1;
                newConstraintRow[finalTable.GetLength(1)] = double.Parse(newConstraint["rhs"]);

                List<List<double>> listTable = ArrayToList(finalTable);
                List<double> listCon = new List<double>(newConstraintRow);

                List<List<double>> newTable = AddConstraint(listCon, listTable, finalTable.GetLength(0), finalTable.GetLength(1));

                double[,] tab = ListToArray(newTable);

                Simplex sp = new Simplex(tab, lm.ProblemType);
                rtbOutput.Text = sp.PrintDual();
            }


        }
        public List<List<double>> ArrayToList(double[,] table)
        {
            // Populate Nested lists with values from the array
            List<List<double>> listTable = new List<List<double>>();

            for (int i = 0; i < table.GetLength(0); i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    row.Add(table[i, j]);
                }
                listTable.Add(row);
            }
            return listTable;
        }
        public double[,] ListToArray(List<List<double>> table)
        {
            // Populate Nested lists with values from the array
            double[,] listTable = new double[table.Count, table[0].Count];

            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < table[i].Count; j++)
                {
                    listTable[i, j] = table[i][j];
                }
            }
            return listTable;
        }
        public List<List<double>> AddConstraint(List<double> constraint, List<List<double>> table, int columns, int rows)
        {
            double sumWithoutConstraint = 0;
            List<double> sum = new List<double>();
            List<int> clashes = new List<int>();

            for (int i = 0; i < columns; i++)
            {
                if (i == table[0].Count - 2)
                {
                    table[i].Insert(table[0].Count() - 2, constraint[constraint.Count() - 2]);
                }
                else
                {
                    table[i].Insert(table[0].Count() - 2, 0);
                }
            }
            //columns++;
            for (int i = 0; i < rows; i++)
            {
                int row = 0;
                for (int j = 0; j < columns; j++)
                {
                    sumWithoutConstraint += table[j][i];
                    row = j;
                }
                if (sumWithoutConstraint == 1)
                {
                    if (sumWithoutConstraint + constraint[i + 1] != 1)
                    {
                        clashes.Add(row);
                    }
                }
                sum.Add(sumWithoutConstraint);
            }

            if (clashes.Count > 0)
            {
                for (int i = 0; i < constraint.Count(); i++)
                {
                    constraint[i] = table[clashes[0]][i] - constraint[i];
                }
            }

            table.Add(constraint);
            rows++;
            return table;
        }

        private void cboCAChangeRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCAChangeValue.Text = cboCAChangeRow.Items[cboCAChangeRow.SelectedIndex].ToString();


            
        }
    }
}
