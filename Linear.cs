using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Linear
    {
        private double[,] text;
        public Linear(double[,] text)
        {
            this.Text = text;
        }

        public double[,] Text { get => text; set => text = value; }

        // Algorithm to pivot on a given table.
        private double[,] PivotTable(double[,] initialTableArray, int pivotColumnIndex, int pivotRowIndex)
        {
            double[,] outTableArray = new double[initialTableArray.GetLength(0), initialTableArray.GetLength(1)];
            for (int i = 0; i < initialTableArray.GetLength(0); i++)
            {
                for (int j = 0; j < initialTableArray.GetLength(1); j++)
                {
                    double value = initialTableArray[i, j] - (initialTableArray[i, pivotColumnIndex] * (initialTableArray[pivotRowIndex, j] / initialTableArray[pivotRowIndex, pivotColumnIndex]));
                    if (value.ToString().Contains(".9999"))
                    {
                        value = Math.Ceiling(value);
                    }
                    outTableArray[i, j] = value;
                }
            }

            for (int i = 0; i < initialTableArray.GetLength(0); i++)
            {
                for (int j = 0; j < initialTableArray.GetLength(1); j++)
                {
                    double value = initialTableArray[pivotRowIndex, j] / initialTableArray[pivotRowIndex, pivotColumnIndex];
                    if (value.ToString().Contains(".9999"))
                    {
                        value = Math.Ceiling(value);
                    }
                    outTableArray[pivotRowIndex, j] = value;
                }
            }
            return outTableArray;
        }


        // DUAL SIMPLEX ALGORITHM
        public List<double[,]> DualSimplex(double[,] LP, string ProblemType)
        {
            List<double[,]> SimplexTables = new List<double[,]>();
            SimplexTables.Add(LP);

            bool simplexOptimal = false;
            List<Dictionary<int, int>> SimplexTablesPivots = new List<Dictionary<int, int>>();

            while (!simplexOptimal)
            {
                double[,] table = SimplexTables[SimplexTables.Count - 1];

                int rows = table.GetLength(0);
                int columns = table.GetLength(1);

                int pivotRowIndex = -1;

                double pivotCol = 1000000;
                int pivotColIndex = -1;

                // Find the minimum value in the last column
                double minValue = 0;
                for (int i = 1; i < rows; i++)
                {
                    double currentValue = table[i, columns - 1];

                    if (currentValue < 0 && currentValue < minValue)
                    {
                        pivotRowIndex = i;
                        minValue = currentValue;
                    }
                }

                //No negative was found in the rhs
                if (minValue >= 0)
                {
                    //Pivot selection for MAX problem
                    if (ProblemType == "max")
                    {
                        minValue = 0;
                        for (int i = 0; i < table.GetLength(1) - 1; i++)
                        {
                            double currentValue = table[0, i];
                            if (currentValue < 0 && currentValue < minValue)
                            {
                                pivotColIndex = i;
                                minValue = currentValue;
                            }
                        }

                    }
                    //Pivot selection for MIN problem
                    else
                    {
                        minValue = 0;
                        for (int i = 0; i < table.GetLength(1) - 1; i++)
                        {
                            double currentValue = table[0, i];
                            if (!(currentValue < 0) && currentValue > minValue)
                            {
                                pivotColIndex = i;
                                minValue = currentValue;
                            }
                        }
                    }

                    if (pivotColIndex == -1)
                    {
                        simplexOptimal = true;
                        break;
                    }

                    //pivot row selection
                    minValue = 100000;
                    for (int i = 1; i < table.GetLength(0); i++)
                    {
                        double currentValue = table[i, table.GetLength(1) - 1] / table[i, pivotColIndex];

                        if (currentValue < minValue && !(currentValue < 0) && !double.IsNaN(currentValue))
                        {
                            pivotRowIndex = i;
                            minValue = currentValue;
                        }
                    }

                    if (pivotRowIndex == -1)
                    {
                        simplexOptimal = true;
                        break;
                    }

                    //Add pivot information
                    Dictionary<int, int> pivots = new Dictionary<int, int>();
                    pivots.Add(pivotColIndex, pivotRowIndex);
                    SimplexTablesPivots.Add(pivots);

                    //Adds table to solution
                    SimplexTables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
                }
                //Negative was found in the rhs following dual
                else
                {

                    if (pivotRowIndex == -1)
                    {
                        simplexOptimal = true;
                        break;
                    }

                    //Dual pivot col selection
                    for (int j = 0; j < table.GetLength(1) - 1; j++)
                    {
                        double currentValue = Math.Abs(table[0, j] / table[pivotRowIndex, j]);
                        //Console.WriteLine(currentValue);

                        if (table[pivotRowIndex, j] <= 0 && currentValue < pivotCol)
                        {
                            pivotCol = currentValue;
                            pivotColIndex = j;
                        }
                    }

                    if (pivotColIndex == -1)
                    {
                        simplexOptimal = true;
                        break;
                    }

                    //Adds pivot column and row info
                    Dictionary<int, int> pivots = new Dictionary<int, int>();
                    pivots.Add(pivotColIndex, pivotRowIndex);
                    SimplexTablesPivots.Add(pivots);

                    //Adds table to solution
                    SimplexTables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
                }
            }
            return SimplexTables;
        }

        // CUTTING PLANE ALGORITHM
        public List<List<double[,]>> CuttingPlane(double[,] LP, string ProblemType)
        {
            List<List<double[,]>> final = new List<List<double[,]>>();

            List<double[,]> iteration = DualSimplex(LP, ProblemType);
            final.Add(iteration);
            LP = iteration[iteration.Count - 1];

            bool keepIterating = true;
            while (keepIterating)
            {
                int bestIndex = 0;
                double temporaryValue = 0;
                double bestDecimal = 10;

                // Get the row to cut by.
                for (int i = 1; i < LP.GetLength(0); i++)
                {
                    double value = LP[i, LP.GetLength(1) - 1];
                    double decimalValue = Math.Round(LP[i, LP.GetLength(1) - 1] % 1 * 10, 0);

                    if (decimalValue >= 5)
                    {
                        decimalValue -= 5;
                    }
                    else
                    {
                        decimalValue = 5 - decimalValue;
                    }

                    if (decimalValue < bestDecimal)
                    {
                        bestIndex = i;
                        bestDecimal = decimalValue;
                        temporaryValue = value;
                    }
                    else if (decimalValue == bestDecimal)
                    {
                        if (value > temporaryValue)
                        {
                            bestIndex = i;
                            bestDecimal = decimalValue;
                            temporaryValue = value;
                        }
                    }
                }

                if (bestDecimal == 5)
                {
                    keepIterating = false;
                    break;
                }

                // Calculate the cut row.
                List<double> result = new List<double>();
                for (int i = 0; i < LP.GetLength(1); i++)
                {
                    if (i == LP.GetLength(1) - 1)
                    {
                        result.Add(1);
                    }

                    if (LP[bestIndex, i] > 0)
                    {
                        result.Add((LP[bestIndex, i] % 1) * -1);
                    }
                    else if (LP[bestIndex, i] < 0)
                    {
                        result.Add((1 + (LP[bestIndex, i] % 1)) * -1);
                    }
                    else
                    {
                        result.Add(0);
                    }
                }

                // Add the new cut row and slack column to the table.
                double[,] newLP = new double[LP.GetLength(0) + 1, LP.GetLength(1) + 1];

                for (int i = 0; i < newLP.GetLength(0); i++)
                {
                    string line = "";
                    for (int j = 0; j < LP.GetLength(1); j++)
                    {
                        if (i == newLP.GetLength(0) - 1)
                        {
                            double[] dummy = result.ToArray();
                            for (int k = 0; k < dummy.Length; k++)
                            {
                                newLP[i, k] = dummy[k];
                                line = line + newLP[i, k] + ", ";
                            }
                            break;
                        }
                        else
                        {
                            if (j == LP.GetLength(1) - 1)
                            {
                                newLP[i, j] = 0;
                                newLP[i, j + 1] = LP[i, j];
                                line = line + newLP[i, j] + ", " + newLP[i, j + 1];
                            }
                            else
                            {
                                newLP[i, j] = LP[i, j];
                                line = line + newLP[i, j] + ", ";
                            }
                        }
                    }
                    //Console.WriteLine(line);
                }

                List<double[,]> Tables = DualSimplex(newLP, ProblemType);
                LP = Tables[Tables.Count - 1];
                final.Add(Tables);
            }
            return final;
        }

        // Print the list of items in the form of a string.
        public string PrintResults(List<List<double[,]>> iterations)
        {
            int iter = 0;
            string output = "";
            foreach (var iteration in iterations)
            {
                iter++;
                output = output + "\nITERATION: " + iter + "\n\n";
                foreach (var table in iteration)
                {
                    for (int i = 0; i < table.GetLength(0); i++)
                    {
                        string line = "";
                        for (int j = 0; j < table.GetLength(1); j++)
                        {
                            line = line + Math.Round(table[i, j], 2) + "\t";
                        }
                        output = output + line + "\n";
                    }
                }
            }

            return output;
        }
    }
}
