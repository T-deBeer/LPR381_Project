using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class Simplex
    {
        private double[,] initialTable;
        private string problemType;

        public Simplex(double[,] initialTable, string problemType)
        {
            this.InitialTable = initialTable;
            this.ProblemType = problemType;
        }

        public double[,] InitialTable { get => initialTable; set => initialTable = value; }
        public string ProblemType { get => problemType; set => problemType = value; }

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

        public List<double[,]> DualSimplexAlgorithm()
        {
            List<double[,]> tables = new List<double[,]>();

            tables.Add(InitialTable);
            bool simplexOptimal = false;

            do
            {
                double[,] table = tables[tables.Count - 1];

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

                    if (double.IsNegative(currentValue) && currentValue < minValue)
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
                            if (double.IsNegative(currentValue) && currentValue < minValue)
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
                            if (!double.IsNegative(currentValue) && currentValue > minValue)
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

                        if (currentValue < minValue && !double.IsNegative(currentValue) && !double.IsNaN(currentValue))
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

                    //Adds table to solution
                    tables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
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

                    //Adds table to solution
                    tables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
                }

            } while (!simplexOptimal);

            return tables;
        }

        public List<double[,]> PrimalSimplexAlgorithm()
        {
            List<double[,]> tables = new List<double[,]>();

            tables.Add(InitialTable);
            bool simplexOptimal = false;

            do
            {
                double[,] table = tables[tables.Count - 1];

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

                    if (double.IsNegative(currentValue) && currentValue < minValue)
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
                            if (double.IsNegative(currentValue) && currentValue < minValue)
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
                            if (!double.IsNegative(currentValue) && currentValue > minValue)
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

                        if (currentValue < minValue && !double.IsNegative(currentValue) && !double.IsNaN(currentValue))
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

                    //Adds table to solution
                    tables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
                }
                //Negative was found in the rhs following dual
                else
                {

                    tables.Add(new double[table.GetLength(0), table.GetLength(1)]);
                    break;
                }

            } while (!simplexOptimal);

            return tables;
        }

        public List<double[,]> TwoPhaseAlgorithm(List<int> artificialColumns)
        {
            List<double[,]> tables = new List<double[,]>();

            tables.Add(InitialTable);
            bool twoPhaseOptimal = false;
            do
            {
                bool valid = false;
                int pivotRowIndex = -1;
                int pivotColIndex = -1;


                double[,] table = tables[tables.Count - 1];

                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[0, i] > 0)
                    {
                        valid = true;
                    }
                }

                if (valid)
                {
                    //Select pivot column using W
                    double maxValue = 0;
                    for (int i = 0; i < table.GetLength(1) - 1; i++)
                    {
                        double currentValue = table[0, i];

                        if (currentValue > maxValue && !double.IsNegative(currentValue))
                        {
                            maxValue = currentValue;
                            pivotColIndex = i;
                        }
                    }
                }
                else
                {
                    //Select pivot col using Z
                    double maxValue = 0;
                    for (int i = 0; i < table.GetLength(1) - 1; i++)
                    {
                        if (!artificialColumns.Contains(i))
                        {
                            double currentValue = table[1, i];

                            if (ProblemType == "max")
                            {
                                if (currentValue < maxValue && double.IsNegative(currentValue))
                                {
                                    maxValue = currentValue;
                                    pivotColIndex = i;
                                }
                            }
                            else
                            {
                                if (currentValue > maxValue && !double.IsNegative(currentValue))
                                {
                                    maxValue = currentValue;
                                    pivotColIndex = i;
                                }
                            }
                        }
                    }
                }

                if (pivotColIndex == -1)
                {
                    twoPhaseOptimal = true;
                    break;
                }

                double minValue = 100000;
                for (int i = 2; i < table.GetLength(0); i++)
                {
                    double currentValue = table[i, table.GetLength(1) - 1] / table[i, pivotColIndex];
                    if (currentValue < minValue && !double.IsNegative(currentValue) && !double.IsNaN(currentValue))
                    {
                        pivotRowIndex = i;
                        minValue = currentValue;
                    }
                }

                if (pivotRowIndex == -1)
                {
                    twoPhaseOptimal = true;
                    break;
                }

                tables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));


            } while (!twoPhaseOptimal);

            return tables;
        }

        public string PrintPrimal()
        {
            List<double[,]> result = PrimalSimplexAlgorithm();
            string line = "";
            foreach (var item in result)
            {
                for (int i = 0; i < item.GetLength(0); i++)
                {
                    for (int j = 0; j < item.GetLength(1); j++)
                    {
                        line += Math.Round(item[i, j], 4) + "\t";
                    }
                    line += "\n";
                }
                line += "\n";
            }

            return line;
        }

        public string PrintTwoPhase(List<double[,]> result)
        {
            string line = "";
            foreach (var item in result)
            {
                for (int i = 0; i < item.GetLength(0); i++)
                {
                    for (int j = 0; j < item.GetLength(1); j++)
                    {
                        line += Math.Round(item[i, j], 4) + "\t";
                    }
                    line += "\n";
                }
                line += "\n";
            }

            return line;
        }
        public string PrintDual()
        {
            List<double[,]> result = DualSimplexAlgorithm();
            string line = "";
            foreach (var item in result)
            {
                for (int i = 0; i < item.GetLength(0); i++)
                {
                    for (int j = 0; j < item.GetLength(1); j++)
                    {
                        line += Math.Round(item[i, j], 4) + "\t";
                    }
                    line += "\n";
                }
                line += "\n";
            }

            return line;
        }
    }
}
