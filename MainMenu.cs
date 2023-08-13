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
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MetroSet_UI.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using MathNet.Symbolics;

namespace LPR381_Project
{
    public partial class MainMenu : MetroSetForm
    {
        List<string> lp = new List<string>();
        public MainMenu()
        {
            InitializeComponent();
        }
        private void PrintTables(List<BranchTable>? tables = null, bool branchBound = false, List<string>? headers = null)
        {
            //pnlBranches.Controls.Clear();
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

            cbForm.Location = new System.Drawing.Point(1816, 4);
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
                try
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

                    LinearModel lm = new LinearModel(lp.ToArray());
                    rtbFileOutput.AppendText("\nSIMPLEX CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonSimplexConstraintsToString() + "\n");

                    rtbFileOutput.AppendText("\nTWO-PHSES CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.WFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonTwoPhaseConstraintsToString() + "\n");

                    rtbFileOutput.AppendText("\nDUALITY CANONICAL FORM:\n");
                    rtbFileOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
                    rtbFileOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");


                    btnSolve.Enabled = true;
                    cboMethod.Enabled = true;
                    cboMethod.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    MessageBox.Show("The textfile was not in the correct format.\n" +
                            "Please ensure that each variable is separated by a space and there are not any additional spaces. All numbers except the rhs have signs (+/-). And should have a min/max at the start.\n"
                            , "Textfile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            try
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
                LinearModel lm = new LinearModel(lp.ToArray());
                rtbFileOutput.AppendText("\nSIMPLEX CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonSimplexConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nTWO-PHSES CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.WFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonTwoPhaseConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nDUALITY CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");


                btnSolve.Enabled = true;
                cboMethod.Enabled = true;
                cboMethod.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("The textfile was not in the correct format.\n" +
                            "Please ensure that each variable is separated by a space and there are not any additional spaces. All numbers except the rhs have signs (+/-). And should have a min/max at the start.\n"
                            , "Textfile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            lblSolutionEmpty.Visible = false;

            btnConstraints.Enabled = true;
            mtxtCon.Enabled = true;

            LinearModel lm = new LinearModel(lp.ToArray());


            List<BranchTable> branches = new List<BranchTable>();
            List<string> headers = new List<string>();
            List<string> rowHeaders = new List<string>();
            double[,] finalTable = new double[lm.SimplexInitial.GetLength(0), lm.SimplexInitial.GetLength(1)];

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
                    List<double[,]> tables = sp.PrimalSimplexAlgorithm();
                    finalTable = tables[tables.Count - 1];

                    rowHeaders.Add($"Z");

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    int rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");
                    int count = 1;
                    foreach (var table in tables)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }

                    btnOutputClear_Click(sender, e);

                    PrintTables(branches);
                    cboCARangeCol.Items.Clear();
                    cboCARangeRow.Items.Clear();

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        cboCARangeCol.Items.Add(kvp.Key);
                    }

                    int conCounter = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains("E")))
                        {
                            cboCARangeCol.Items.Add(kvp.Key + conCounter.ToString());
                            conCounter++;
                        }
                    }
                    cboCARangeCol.Items.Add("rhs");
                    cboCARangeRow.Items.Add("Z");
                    for (int i = 0; i < lm.ConstraintsSimplex.Count; i++)
                    {
                        cboCARangeRow.Items.Add($"Constraint {i + 1}");
                    }


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
                    //finalTable = result[result.Count - 1];


                    rowHeaders.Add("W");
                    rowHeaders.Add($"Z");

                    count = 1;
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            if (kvp.Key == "E")
                            {
                                headers.Add(kvp.Key);
                                headers.Add("A");
                            }
                            else
                            {
                                headers.Add(kvp.Key);
                            }

                        }
                    }
                    headers.Add("rhs");


                    foreach (var table in result)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }
                    btnOutputClear_Click(sender, e);

                    cboCARangeCol.Items.Clear();
                    cboCARangeRow.Items.Clear();

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        cboCARangeCol.Items.Add(kvp.Key);
                    }

                    conCounter = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains("E")))
                        {
                            if (kvp.Key == "E")
                            {
                                cboCARangeCol.Items.Add(kvp.Key + conCounter.ToString());
                                cboCARangeCol.Items.Add("A" + conCounter.ToString());
                            }
                            else
                            {
                                cboCARangeCol.Items.Add(kvp.Key + conCounter.ToString());
                            }
                            
                            conCounter++;
                        }
                    }
                    cboCARangeCol.Items.Add("rhs");
                    cboCARangeRow.Items.Add("W");
                    cboCARangeRow.Items.Add("Z");
                    for (int i = 0; i < lm.ConstraintsSimplex.Count; i++)
                    {
                        cboCARangeRow.Items.Add($"Constraint {i + 1}");
                    }

                    PrintTables(branches);
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

                    List<double[,]> dualResult = sd.DualSimplexAlgorithm();
                    finalTable = dualResult[dualResult.Count - 1];

                    rowHeaders.Add($"Z");

                    count = 1;
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");

                    foreach (var table in dualResult)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }
                    btnOutputClear_Click(sender, e);

                    PrintTables(branches);

                    cboCARangeCol.Items.Clear();
                    cboCARangeRow.Items.Clear();

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        cboCARangeCol.Items.Add(kvp.Key);
                    }

                    conCounter = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains("E")))
                        {
                            cboCARangeCol.Items.Add(kvp.Key + conCounter.ToString());
                            conCounter++;
                        }
                    }
                    cboCARangeCol.Items.Add("rhs");
                    cboCARangeRow.Items.Add("Z");
                    for (int i = 0; i < lm.ConstraintsSimplex.Count; i++)
                    {
                        cboCARangeRow.Items.Add($"Constraint {i + 1}");
                    }

                    EnableElements();
                    break;
                case 3:
                    if (lm.SignRes.Contains("int") || lm.SignRes.Contains("bin"))
                    {
                        MessageBox.Show("Some of the values in this Linear Programming model are either Integer (int) or binary (bin). \n" +
                            "The Dual Simplex alogrithm will not be able to satisfy these value restrictions but will still continue to solve it as unrestricted.\n" +
                            "Try using the 'Branch and Bound' or 'Cutting Plane' algorithms for values that have int or bin.", "Sign Restrictions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sd = new Simplex(lm.SimplexInitial, lm.ProblemType);

                    dualResult = sd.DualSimplexAlgorithm();
                    finalTable = dualResult[dualResult.Count - 1];

                    rowHeaders.Add($"Z");

                    count = 1;
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }

                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;

                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }
                    headers.Add("rhs");

                    
                    BranchnBound branchnBound = new BranchnBound(finalTable);
                    PrintTables(branchnBound.Solve(lm.ProblemType, headers, rowHeaders));
                    break;
                case 4:
                    CuttingPlane cp = new CuttingPlane(lm.SimplexInitial, lm.ProblemType, lm.SignRes.ToArray());


                    List<List<double[,]>> cpResultList = cp.CuttingPlaneSolve();
                    List<double[,]> cpResult = new List<double[,]>();

                    rowHeaders.Add($"Z");

                    count = 1;

                    foreach (var iteration in cpResultList)
                    {
                        foreach (var table in iteration)
                        {
                            cpResult.Add(table);
                        }
                    }
                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        headers.Add(kvp.Key);
                    }
                    rowCount = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        rowHeaders.Add($"{rowCount}");
                        rowCount++;


                        foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                        {
                            headers.Add(kvp.Key);
                        }
                    }

                    rowHeaders.Add($"{rowCount}");
                    headers.Add("S");
                    headers.Add("rhs");

                    foreach (var table in cpResult)
                    {
                        BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                        branches.Add(newTable);
                        count++;
                    }

                    btnOutputClear_Click(sender, e);
                    PrintTables(branches);

                    cboCARangeCol.Items.Clear();
                    cboCARangeRow.Items.Clear();

                    foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                    {
                        cboCARangeCol.Items.Add(kvp.Key);
                    }

                    conCounter = 1;
                    foreach (var con in lm.ConstraintsSimplex)
                    {
                        foreach (var kvp in con.Where(x => x.Key.Contains('S') || x.Key.Contains("E")))
                        {
                            cboCARangeCol.Items.Add(kvp.Key + conCounter.ToString());
                            conCounter++;
                        }
                    }
                    cboCARangeCol.Items.Add("rhs");
                    cboCARangeRow.Items.Add("Z");
                    for (int i = 0; i < lm.ConstraintsSimplex.Count; i++)
                    {
                        cboCARangeRow.Items.Add($"Constraint {i + 1}");
                    }

                    EnableElements();
                    break;
                default:
                    MessageBox.Show("Invalid method selected, please try another method.", "Method Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            CriticalAnalysis ca = new CriticalAnalysis(lm.SimplexInitial, finalTable);
            rtbOutput.Text = "";
            rtbOutput.AppendText("\nCbv = ");

            string caOutput = "[\t";
            for (int i = 0; i < ca.cBV.Length; i++)
            {
                caOutput += $"{Math.Round(ca.cBV[i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");

            caOutput = "";
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(ca.B).Transpose();
            double[,] bArray = matrixB.ToArray();

            rtbOutput.AppendText("\n B =");
            for (int i = 0; i < bArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }

            caOutput = "";

            rtbOutput.AppendText($"\n B-1 =");
            double[,] bInverseArray = ca.BInverse.Transpose().ToArray();
            for (int i = 0; i < bInverseArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bInverseArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bInverseArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }


            rtbOutput.AppendText($"\nCbvB-1 =");
            caOutput = "[\t";
            double[,] cBVbInverse = ca.CbvBinverse.ToArray();
            for (int i = 0; i < cBVbInverse.GetLength(1); i++)
            {
                caOutput += $"{Math.Round(cBVbInverse[0, i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");


            rtbOutput.AppendText($"\nb =");
            caOutput = "";
            for (int i = 0; i < ca.z.Length; i++)
            {
                caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
            }
            rtbOutput.AppendText(caOutput + "\n");

            cboShadowPriceVar.Items.Clear();
            for (int i = 1; i <= lm.ConstraintsSimplex.Count; i++)
            {
                cboShadowPriceVar.Items.Add($"Constraint {i}");
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
            LinearModel lm = new LinearModel(lp.ToArray());
            Simplex s = new Simplex(lm.DualityInitial, lm.DualProblemType);
            Simplex sp = new Simplex(lm.SimplexInitial, lm.ProblemType);

            List<double[,]> tables = s.DualSimplexAlgorithm();
            List<double[,]> dualTables = sp.DualSimplexAlgorithm();

            List<string> headers = new List<string>();
            List<string> rowHeaders = new List<string>();
            List<BranchTable> branches = new List<BranchTable>();

            rowHeaders.Add($"W");

            int count = 1;
            foreach (var kvp in lm.DualityFunction.Where(x => x.Key.Contains('Y')))
            {
                headers.Add(kvp.Key);
            }

            int rowCount = 1;
            foreach (var con in lm.DualityConstraints)
            {
                rowHeaders.Add($"{rowCount}");
                rowCount++;

                foreach (var kvp in con.Where(x => !x.Key.Contains('Y') && x.Key != "rhs" && x.Key != "sign"))
                {
                    headers.Add(kvp.Key);
                }
            }
            headers.Add("rhs");


            foreach (var table in tables)
            {
                BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                branches.Add(newTable);
                count++;
            }

            btnOutputClear_Click(sender, e);
            PrintTables(branches);

            CriticalAnalysis ca = new CriticalAnalysis(lm.DualityInitial, tables[tables.Count - 1]);
            rtbOutput.Text = "";
            rtbOutput.AppendText("\nCbv = ");

            string caOutput = "[\t";
            for (int i = 0; i < ca.cBV.Length; i++)
            {
                caOutput += $"{Math.Round(ca.cBV[i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");

            caOutput = "";
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(ca.B).Transpose();
            double[,] bArray = matrixB.ToArray();

            rtbOutput.AppendText("\n B =");
            for (int i = 0; i < bArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }

            caOutput = "";

            rtbOutput.AppendText($"\n B-1 =");
            double[,] bInverseArray = ca.BInverse.Transpose().ToArray();
            for (int i = 0; i < bInverseArray.GetLength(0); i++)
            {
                caOutput = "\t[\t";
                for (int j = 0; j < bInverseArray.GetLength(1); j++)
                {
                    caOutput += $"{Math.Round(bInverseArray[i, j], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");
            }


            rtbOutput.AppendText($"\nCbvB-1 =");
            caOutput = "[\t";
            double[,] cBVbInverse = ca.CbvBinverse.ToArray();
            for (int i = 0; i < cBVbInverse.GetLength(1); i++)
            {
                caOutput += $"{Math.Round(cBVbInverse[0, i], 4)};\t";
            }
            caOutput += "]";
            rtbOutput.AppendText(caOutput + "\n");


            rtbOutput.AppendText($"\nb =");
            caOutput = "";
            for (int i = 0; i < ca.z.Length; i++)
            {
                caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
            }
            rtbOutput.AppendText(caOutput + "\n");

            if (tables[tables.Count - 1][0, tables[tables.Count - 1].GetLength(1)-1] == dualTables[dualTables.Count - 1][0, dualTables[dualTables.Count - 1].GetLength(1) - 1])
            {
                rtbOutput.AppendText("The model has a strong duality\n");
            }
            else
            {
                rtbOutput.AppendText("The model has a weak duality\n");
            }
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

                List<List<double>> newTable = AddConstraint(listCon, listTable, finalTable.GetLength(1), finalTable.GetLength(0));

                double[,] tab = ListToArray(newTable);

                Simplex sp = new Simplex(tab, lm.ProblemType);
                //rtbOutput.Text = sp.PrintDual();
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
        private bool IsBasic(int columnIndex, List<List<double>> table, int rows)
        {
            List<double> column = new List<double>();
            //Grab values from column
            for (int i = 0; i < rows; i++)
            {
                // If element is one or zero add to list (straight forward) 
                // BUT if the element is anything else then it is a non-basic variable (IsBasic = false = non-basic and vice versa)
                if (table[i][columnIndex] == 0 || table[i][columnIndex] == 1)
                {
                    column.Add(table[i][columnIndex]);
                }
                else
                {
                    return false;
                }
            }
            // Sum of basic variable column = 1 (accounts for example: 0 1 1 which would pass the previous test)
            return column.Sum() == 1;
        }
        public List<List<double>> AddConstraint(List<double> constraint, List<List<double>> table, int columns, int rows)
        {
            //List<double> sum = new List<double>();
            for (int i = 0; i < constraint.Count(); i++)
            {
                constraint[i] *= -1;
            }
            List<int> clashes = new List<int>();
            for (int i = 0; i < columns; i++)
            {
                if (IsBasic(i, table, rows))
                {
                    double sum = 0;
                    int rowClash = 0;
                    for (int j = 0; j < rows; j++)
                    {
                        sum += table[j][i];
                        if (table[j][i] == 1)
                        {
                            rowClash = j;
                        }
                    }
                    if (sum != sum + constraint[i])
                    {
                        clashes.Add(rowClash);
                    }
                }
            }

            for (int i = 0; i < rows; i++)
            {
                //if (i == columns - 2)
                //{
                //    table[i].Insert(columns - 2, constraint[constraint.Count() - 2]);
                //}
                //else
                //{
                table[i].Insert(columns - 2, 0);
                //}
            }
            columns++;
            for (int i = 0; i < clashes.Count(); i++)
            {
                List<double> newRow = new List<double>();
                for (int j = 0; j < columns; j++)
                {
                    double newElement = table[clashes[i]][j] - constraint[j];
                    newRow.Add(newElement);
                }
                table.Add(newRow);
            }

            return table;
        }

        private void cboCAChangeRow_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCAChangeValue.Text = cboCAChangeRow.Items[cboCAChangeRow.SelectedIndex].ToString();
            txtCAChangeValue.BackColor = Color.FromArgb(34, 34, 34);
        }

        private void btnOutputClear_Click(object sender, EventArgs e)
        {
            // Iterate through the child controls of the panel and remove DataGridView controls
            for (int i = pnlBranches.Controls.Count - 1; i >= 0; i--)
            {
                if (pnlBranches.Controls[i] is DataGridView)
                {
                    pnlBranches.Controls.RemoveAt(i);
                }
            }
            //lblSolutionEmpty.Visible = true;
        }

        private void btnCAOutputClear_Click(object sender, EventArgs e)
        {
            rtbOutput.Text = "";
        }

        private void btnCAChanges_Click(object sender, EventArgs e)
        {
            if (lp[cboCAChangeRow.SelectedIndex].Split(" ").Length != txtCAChangeValue.Text.Split(" ").Length)
            {
                MessageBox.Show("Ensure that the value you are trying to enter's form matches the selcted row that you want to change");
            }
            else
            {
                lp[cboCAChangeRow.SelectedIndex] = txtCAChangeValue.Text;

                LinearModel lm = new LinearModel(lp.ToArray());
                List<BranchTable> branches = new List<BranchTable>();

                rtbFileOutput.Text = "";
                cboCAChangeRow.Items.Clear();

                txtCAChangeValue.Text = "";


                foreach (var item in lp)
                {
                    rtbFileOutput.AppendText(item + "\n");
                    cboCAChangeRow.Items.Add(item);
                }

                cboCAChangeRow.SelectedIndex = 0;

                rtbFileOutput.AppendText("\nSIMPLEX CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonSimplexConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nTWO-PHSES CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.WFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.ObjFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonTwoPhaseConstraintsToString() + "\n");

                rtbFileOutput.AppendText("\nDUALITY CANONICAL FORM:\n");
                rtbFileOutput.AppendText(lm.CanonDualFunctionToString() + "\n");
                rtbFileOutput.AppendText(lm.CanonDualConstraintsToString() + "\n");

                Simplex sd = new Simplex(lm.SimplexInitial, lm.ProblemType);

                List<string> headers = new List<string>();
                List<string> rowHeaders = new List<string>();

                List<double[,]> dualResult = sd.DualSimplexAlgorithm();
                double[,] finalTable = dualResult[dualResult.Count - 1];

                rowHeaders.Add($"Z");

                int count = 1;
                foreach (var kvp in lm.ObjectiveFunction.Where(x => x.Key.Contains('X')))
                {
                    headers.Add(kvp.Key);
                }

                int rowCount = 1;
                foreach (var con in lm.ConstraintsSimplex)
                {
                    rowHeaders.Add($"{rowCount}");
                    rowCount++;

                    foreach (var kvp in con.Where(x => !x.Key.Contains('X') && x.Key != "rhs" && x.Key != "sign"))
                    {
                        headers.Add(kvp.Key);
                    }
                }
                headers.Add("rhs");

                foreach (var table in dualResult)
                {
                    BranchTable newTable = new BranchTable(count.ToString(), table, headers, rowHeaders);
                    branches.Add(newTable);
                    count++;
                }
                btnOutputClear_Click(sender, e);
                PrintTables(branches);


                CriticalAnalysis ca = new CriticalAnalysis(lm.SimplexInitial, finalTable);
                rtbOutput.Text = "";
                rtbOutput.AppendText("\nCbv = ");

                string caOutput = "[\t";
                for (int i = 0; i < ca.cBV.Length; i++)
                {
                    caOutput += $"{Math.Round(ca.cBV[i], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");

                caOutput = "";
                Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(ca.B).Transpose();
                double[,] bArray = matrixB.ToArray();

                rtbOutput.AppendText("\n B =");
                for (int i = 0; i < bArray.GetLength(0); i++)
                {
                    caOutput = "\t[\t";
                    for (int j = 0; j < bArray.GetLength(1); j++)
                    {
                        caOutput += $"{Math.Round(bArray[i, j], 4)};\t";
                    }
                    caOutput += "]";
                    rtbOutput.AppendText(caOutput + "\n");
                }

                caOutput = "";

                rtbOutput.AppendText($"\n B-1 =");
                double[,] bInverseArray = ca.BInverse.Transpose().ToArray();
                for (int i = 0; i < bInverseArray.GetLength(0); i++)
                {
                    caOutput = "\t[\t";
                    for (int j = 0; j < bInverseArray.GetLength(1); j++)
                    {
                        caOutput += $"{Math.Round(bInverseArray[i, j], 4)};\t";
                    }
                    caOutput += "]";
                    rtbOutput.AppendText(caOutput + "\n");
                }


                rtbOutput.AppendText($"\nCbvB-1 =");
                caOutput = "[\t";
                double[,] cBVbInverse = ca.CbvBinverse.ToArray();
                for (int i = 0; i < cBVbInverse.GetLength(1); i++)
                {
                    caOutput += $"{Math.Round(cBVbInverse[0, i], 4)};\t";
                }
                caOutput += "]";
                rtbOutput.AppendText(caOutput + "\n");


                rtbOutput.AppendText($"\nb =");
                caOutput = "";
                for (int i = 0; i < ca.z.Length; i++)
                {
                    caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
                }
                rtbOutput.AppendText(caOutput + "\n");
            }
        }

        private void btnShadowPrices_Click(object sender, EventArgs e)
        {
            int selectedConstraint = cboShadowPriceVar.SelectedIndex;

            LinearModel lm = new LinearModel(lp.ToArray());
            Simplex sp = new Simplex(lm.SimplexInitial, lm.ProblemType);

            List<double[,]> tables = sp.DualSimplexAlgorithm();

            CriticalAnalysis ca = new CriticalAnalysis(lm.SimplexInitial, tables[tables.Count - 1]);

            rtbOutput.Text = "";
            rtbOutput.AppendText($"\nSHADOW PRICE FOR CONSTRAINT {selectedConstraint + 1}\n");

            rtbOutput.AppendText($"\nCbvB-1 x b =");
            string caOutput = "[\t";
            double[,] cBVbInverse = ca.CbvBinverse.ToArray();
            for (int i = 0; i < cBVbInverse.GetLength(1); i++)
            {
                caOutput += $"{Math.Round(cBVbInverse[0, i], 4)}\t";
            }
            caOutput += "] x";


            for (int i = 0; i < ca.z.Length; i++)
            {
                caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
                caOutput += new string('\t', cBVbInverse.GetLength(1) + 2);
            }
            Matrix<double> matrixZ = Matrix<double>.Build.DenseOfColumnArrays(ca.z);

            double zOld = ca.CbvBinverse.Multiply(matrixZ).ToArray()[0, 0];

            rtbOutput.AppendText(caOutput + "\n");
            rtbOutput.AppendText($"Zold = {Math.Round(zOld, 3)}\n\n");


            double[] newB = ca.z;

            newB[selectedConstraint] += 1;
            rtbOutput.AppendText($"\nCbvB-1 x b =");
            caOutput = "[\t";
            for (int i = 0; i < cBVbInverse.GetLength(1); i++)
            {
                caOutput += $"{Math.Round(cBVbInverse[0, i], 4)}\t";
            }
            caOutput += "] x";


            for (int i = 0; i < ca.z.Length; i++)
            {
                caOutput += $"\t[\t{Math.Round(ca.z[i], 4)}\t]\n";
                caOutput += new string('\t', cBVbInverse.GetLength(1) + 2);
            }
            Matrix<double> newMatrixZ = Matrix<double>.Build.DenseOfColumnArrays(newB);

            double zNew = ca.CbvBinverse.Multiply(newMatrixZ).ToArray()[0, 0];

            rtbOutput.AppendText(caOutput + "\n");
            rtbOutput.AppendText($"Znew = {Math.Round(zNew, 3)}\n\n");


            rtbOutput.AppendText($"Shadow Price = Znew - Zold = {Math.Round(zNew - zOld, 3)}\n\n");
        }

        private void btnCARanges_Click(object sender, EventArgs e)
        {
            if (cboCAChangeRow.SelectedIndex != -1 && cboCARangeCol.SelectedIndex != -1)
            {
                int selectedRow = cboCARangeRow.SelectedIndex;
                int selectedCol = cboCARangeCol.SelectedIndex;

                LinearModel lm = new LinearModel(lp.ToArray());
                Simplex sp = new Simplex(lm.SimplexInitial, lm.ProblemType);
                Simplex spTwoPhase = new Simplex(lm.TwoPhaseInitial, lm.ProblemType);

                List<double[,]> tables = new List<double[,]>();
                bool twoPhase = false;

                switch (cboMethod.SelectedIndex)
                {
                    case 0:
                        {
                            tables = sp.PrimalSimplexAlgorithm();
                        }
                        break;
                    case 1:
                        {
                            tables = spTwoPhase.TwoPhaseAlgorithm(lm.TwoPhaseArtificialColumns);
                            twoPhase = true;
                        }
                        break;
                    case 2:
                        {
                            tables = sp.DualSimplexAlgorithm();
                        }
                        break;
                    case 4:
                        {
                            CuttingPlane cp = new CuttingPlane(lm.SimplexInitial, lm.ProblemType, lm.SignRes.ToArray());
                            List<List<double[,]>> cpResultList = cp.CuttingPlaneSolve();

                            foreach (var iteration in cpResultList)
                            {
                                foreach (var iterTable in iteration)
                                {
                                    tables.Add(iterTable);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }


                double[,] table = tables[tables.Count - 1];
                if (!twoPhase)
                {
                    CriticalAnalysis ca = new CriticalAnalysis(lm.SimplexInitial, table);
                    string result = ca.CalculateRanges(selectedCol, selectedRow, lm.ProblemType);
                    rtbOutput.Text = result;
                }
                else
                {
                    CriticalAnalysis ca = new CriticalAnalysis(lm.TwoPhaseInitial, table);
                    string result = ca.CalculateRanges(selectedCol, selectedRow, lm.ProblemType);
                    rtbOutput.Text = result;
                }   
            }
            else
            {
                MessageBox.Show("Ensure that you have selected a value from both comboboxes.");
            }
        }
    }
}
