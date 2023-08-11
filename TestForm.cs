using MetroSet_UI.Forms;
using System.Text.Json.Serialization;

namespace LPR381_Project
{
    public partial class TestForm : MetroSetForm
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void mbtnSelectFile_Click(object sender, EventArgs e)
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
            rtbOutput.Text += "SIMPLEX CANONICAL FORM:\n";
            rtbOutput.Text += (lp.CanonObjFunctionToString());
            rtbOutput.Text += ("\nsubject to:\n");
            rtbOutput.Text += (lp.CanonSimplexConstraintsToString());
            rtbOutput.Text += "\n\n";

            //double[,] table = lp.SimplexTables[lp.SimplexTables.Count - 1];
            //for (int i = 0; i < table.GetLength(0); i++)
            //{
            //    string res = "";
            //    for (int j = 0; j < table.GetLength(1); j++)
            //    {
            //        res += $"{table[i, j]}\t";
            //    }
            //    rtbOutput.Text += $"{res}\n";
            //}

            //CuttingPlane cp = new CuttingPlane(lp.SimplexInitial, lp.ProblemType, lp.SignRes.ToArray());
            //rtbOutput.Text += "\n\n";
            //rtbOutput.Text += "CUTTING PLANE:\n";
            //rtbOutput.Text += cp.PrintResults();

            //Duality dual = new Duality(lines);
            //rtbOutput.Text += "\n\n";
            //rtbOutput.Text += "DUALITY CHECK:\n";
            //rtbOutput.Text += dual.PrintResults();

            //rtbOutput.AppendText(lp.CanonDualFunctionToString() + "\n");
            //rtbOutput.AppendText(lp.CanonDualConstraintsToString() + "\n");
            //Simplex s = new Simplex(lp.DualityInitial, lp.DualProblemType);
            //List<double[,]> results = s.DualSimplexAlgorithm();
            //string line = "";
            //foreach (var item in results)
            //{
            //    for (int i = 0; i < item.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < item.GetLength(1); j++)
            //        {
            //            line += item[i, j] + "\t";
            //        }
            //        line += "\n";
            //    }
            //    line += "\n";
            //}
            //rtbOutput.AppendText(line);

            //Simplex s = new Simplex(lp.SimplexInitial, lp.ProblemType);
            //List<double[,]> ne = s.PrimalSimplexAlgorithm();
            //string line = "";
            //foreach (var item in ne)
            //{
            //    for (int i = 0; i < item.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < item.GetLength(1); j++)
            //        {
            //            line += item[i, j] + "\t";
            //        }
            //        line += "\n";
            //    }
            //    line += "\n";
            //}
            //rtbOutput.AppendText(line);

            //Simplex s = new Simplex(lp.TwoPhaseInitial, lp.ProblemType);
            //List<double[,]> ne = s.TwoPhaseAlgorithm(lp.TwoPhaseArtificialColumns);
            //string line = "";
            //foreach (var item in ne)
            //{
            //    for (int i = 0; i < item.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < item.GetLength(1); j++)
            //        {
            //            line += item[i, j] + "\t";
            //        }
            //        line += "\n";
            //    }
            //    line += "\n";
            //}
            //rtbOutput.AppendText(line);

            //Accessible with

            //Example
            double[,] initialTable = new double[,]
                {
                    { -3, -2, 0, 0, 0 },
                    { 1, 1, 1, 0, 4 },
                    { 2, -2, 0, 1, -8 }
                };

            double[,] matrixOptimal = new double[,]
            {
                    { 0, 0, 2.5, 0.25, 8 },
                    { 1, 0, 0.5, 0.25, 0 },
                    { 0, 1, 0.5, -0.25, 4 }
            };

            CriticalAnalysis ca = new CriticalAnalysis(initialTable, matrixOptimal);

            // BV*
            int[] basicVariableIndexes = ca.GetBasicVariableIndexes();

            //// XBV
            //double[,] xBV = MathPreliminaries.GetBasicVariablesMatrix(initialTable, basicVariableIndexes);

            // CBV
            double[] CBV = ca.GetObjectiveFunctionCoefficients();
            string line = "";
            for (int i = 0; i < CBV.Length; i++)
            {
                line += CBV[i].ToString() + "\t";
            }
            rtbOutput.Text = rtbOutput.Text + "\n\nCBV:\n" + line;

            // B
            double[,] B = ca.GetBasicVariableColumns();
            line = "";
            for (int i = 0; i < B.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    line += B[i, j] + "\t";
                }
                line += "\n";
            }
            rtbOutput.Text = rtbOutput.Text + "\n\nB:\n" + line;

            // b
            double[] b = ca.GetRHSColumnValues();
            line = "";
            for (int i = 0; i < CBV.Length; i++)
            {
                line += b[i].ToString() + "\t";
            }
            rtbOutput.Text = rtbOutput.Text + "\n\nb:\n" + line;
        }
    }
}