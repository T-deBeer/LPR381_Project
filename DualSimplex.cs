using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class DualSimplex
    {
        private double[,] initialTable;
        private string problemType;

        public DualSimplex(double[,] intialTable, string problemType)
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

        //// DUAL SIMPLEX ALGORITHM
        //public List<double[,]> DualSimplexSolve()
        //{
        //    List<double[,]> SimplexTables = new List<double[,]>();
        //    SimplexTables.Add(IntialTable);

        //    bool simplexOptimal = false;
        //    List<Dictionary<int, int>> SimplexTablesPivots = new List<Dictionary<int, int>>();

        //    while (!simplexOptimal)
        //    {
        //        double[,] table = SimplexTables[SimplexTables.Count - 1];

        //        int rows = table.GetLength(0);
        //        int columns = table.GetLength(1);

        //        int pivotRowIndex = -1;

        //        double pivotCol = 1000000;
        //        int pivotColIndex = -1;

        //        // Find the minimum value in the last column
        //        double minValue = 0;
        //        for (int i = 1; i < rows; i++)
        //        {
        //            double currentValue = table[i, columns - 1];

        //            if (currentValue < 0 && currentValue < minValue)
        //            {
        //                pivotRowIndex = i;
        //                minValue = currentValue;
        //            }
        //        }

        //        //No negative was found in the rhs
        //        if (minValue >= 0)
        //        {
        //            //Pivot selection for MAX problem
        //            if (ProblemType == "max")
        //            {
        //                minValue = 0;
        //                for (int i = 0; i < table.GetLength(1) - 1; i++)
        //                {
        //                    double currentValue = table[0, i];
        //                    if (currentValue < 0 && currentValue < minValue)
        //                    {
        //                        pivotColIndex = i;
        //                        minValue = currentValue;
        //                    }
        //                }

        //            }
        //            //Pivot selection for MIN problem
        //            else
        //            {
        //                minValue = 0;
        //                for (int i = 0; i < table.GetLength(1) - 1; i++)
        //                {
        //                    double currentValue = table[0, i];
        //                    if (!(currentValue < 0) && currentValue > minValue)
        //                    {
        //                        pivotColIndex = i;
        //                        minValue = currentValue;
        //                    }
        //                }
        //            }

        //            if (pivotColIndex == -1)
        //            {
        //                simplexOptimal = true;
        //                break;
        //            }

        //            //pivot row selection
        //            minValue = 100000;
        //            for (int i = 1; i < table.GetLength(0); i++)
        //            {
        //                double currentValue = table[i, table.GetLength(1) - 1] / table[i, pivotColIndex];

        //                if (currentValue < minValue && !(currentValue < 0) && !double.IsNaN(currentValue))
        //                {
        //                    pivotRowIndex = i;
        //                    minValue = currentValue;
        //                }
        //            }

        //            if (pivotRowIndex == -1)
        //            {
        //                simplexOptimal = true;
        //                break;
        //            }

        //            //Add pivot information
        //            Dictionary<int, int> pivots = new Dictionary<int, int>();
        //            pivots.Add(pivotColIndex, pivotRowIndex);
        //            SimplexTablesPivots.Add(pivots);

        //            //Adds table to solution
        //            SimplexTables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
        //        }
        //        //Negative was found in the rhs following dual
        //        else
        //        {

        //            if (pivotRowIndex == -1)
        //            {
        //                simplexOptimal = true;
        //                break;
        //            }

        //            //Dual pivot col selection
        //            for (int j = 0; j < table.GetLength(1) - 1; j++)
        //            {
        //                double currentValue = Math.Abs(table[0, j] / table[pivotRowIndex, j]);
        //                //Console.WriteLine(currentValue);

        //                if (table[pivotRowIndex, j] <= 0 && currentValue < pivotCol)
        //                {
        //                    pivotCol = currentValue;
        //                    pivotColIndex = j;
        //                }
        //            }

        //            if (pivotColIndex == -1)
        //            {
        //                simplexOptimal = true;
        //                break;
        //            }

        //            //Adds pivot column and row info
        //            Dictionary<int, int> pivots = new Dictionary<int, int>();
        //            pivots.Add(pivotColIndex, pivotRowIndex);
        //            SimplexTablesPivots.Add(pivots);

        //            //Adds table to solution
        //            SimplexTables.Add(PivotTable(table, pivotColIndex, pivotRowIndex));
        //        }
        //    }

        //    return SimplexTables;
        //}

        public List<double[,]> DualSimplexAlgorithm(double[,] initTable, string type)
        {
            List<double[,]> tables = new List<double[,]>();

            tables.Add(initTable);
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
                    if (type == "max")
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
    }
}
