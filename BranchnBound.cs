using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class BranchnBound
    {
        private List<List<double>> initialTable;
        private List<double> xRHS;
        private int xColumnsAmount;
        private int columns;
        private int rows;
        private List<List<double>> ArrayToList(double[,] table)
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
        public BranchnBound(double[,] initialTable, int xColumnsAmount)
        {
            this.initialTable = ArrayToList(initialTable);
            this.xColumnsAmount = xColumnsAmount;
            columns = initialTable.GetLength(1);
            rows = initialTable.GetLength(0);
        }
        private bool IsBasic(int columnIndex, List<List<double>> table)
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
        private List<double> FindXValues(List<List<double>> table)
        {
            List<double> rhs = new List<double>();

            for (int i = 0; i < xColumnsAmount; i++)
            {
                if (IsBasic(i, table))
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (table[j][i] == 1)
                        {
                            rhs.Add(table[j][columns - 1]);
                        }
                    }
                }
                else
                {
                    rhs.Add(0);
                }

            }
            return rhs;
        }
        private int DetermineCutRow(List<List<double>> table)
        {
            List<double> rhs = FindXValues(table);
            rhs.RemoveAll(x => x == 0);
            List<double> fractions = new List<double>();
            for (int i = 0; i < rows; i++)
            {
                double fraction = Math.Floor(rhs[i] - rhs[i]);
                fractions.Add(Math.Abs(fraction - 0.5));
            }
            double lowestFraction = fractions.Min();
            bool hasMinDuplicates = fractions.FindAll(x => x == lowestFraction).Count > 1;
            if (hasMinDuplicates)
            {
                int indexOfLargestRHS = 0;
                for (int i = 1; i < fractions.Count(); i++)
                {
                    if (fractions[i] == lowestFraction && rhs[i] > rhs[indexOfLargestRHS])
                    {
                        indexOfLargestRHS = i;
                    }
                }
                return indexOfLargestRHS;
            }
            else
            {
                return fractions.IndexOf(lowestFraction);
            }
        }
        public List<List<double>> GenerateConstraints(int rowIndex, int columnIndex, List<List<double>> table)
        {
            List<List<double>> newRows = new List<List<double>>();
            double rhs = table[rowIndex][columns - 1];

            List<double> constraintRow = Enumerable.Repeat(0.0, columns + 1).ToList();
            double rhsLower = Math.Floor(rhs);
            constraintRow[columnIndex] = 1.0;
            constraintRow[columns - 1] = rhsLower;
            constraintRow.Insert(columns - 2, 1);
            constraintRow.ForEach(x => x *= -1);//multiply after adding

            newRows.Add(constraintRow);

            constraintRow.Clear();
            constraintRow = Enumerable.Repeat(0.0, columns + 1).ToList();
            double rhsHigher = Math.Ceiling(rhs);
            constraintRow[columnIndex] = 1.0;
            constraintRow[columns - 1] = rhsHigher;
            constraintRow.Insert(columns - 2, -1);

            newRows.Add(constraintRow);

            return newRows;
        }
        private List<List<double>> AddConstraint(List<double> constraint, BranchTable branchTable)
        {
            List<List<double>> table = ArrayToList(branchTable.Table);
            List<int> clashes = new List<int>();
            for (int i = 0; i < columns; i++)
            {
                if (IsBasic(i, table))
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
                table[i].Insert(columns - 2, 0);
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
            rows++;
            return table;
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
        public void Solve(string problemType, List<string> headers)
        {
            Queue<BranchTable> tableQueue = new Queue<BranchTable> ();
            List<BranchTable> branches = new List<BranchTable>();

            tableQueue.Enqueue(new BranchTable("0", ListToArray(initialTable), headers));

            while (tableQueue.Count != 0)
            {
                BranchTable branchTable = tableQueue.Dequeue();
                List<List<double>> table = ArrayToList(branchTable.Table);      
                
                int row = DetermineCutRow(table);                    /// ifi
                List<List<double>> constraints = GenerateConstraints(row, table[row].IndexOf(1), table);

                for (int i = 0; i < constraints.Count(); i++)
                {
                    List<List<double>> newTable = AddConstraint(constraints[i], branchTable);
                    //solvedTable = Solve table
                    Simplex simplex = new Simplex(ListToArray(newTable), problemType);
                    List<double[,]> pivots = simplex.DualSimplexAlgorithm();
                    ///

                    List<List<double>> optimalTable = ArrayToList(pivots.Last());
                    double sum = 0;

                    for (int j = 0; j < optimalTable.Count(); j++)
                    {
                        sum += optimalTable[j][columns - 1];
                    }
                    if (Math.Floor(sum) - sum != 0)
                    {
                        tableQueue.Enqueue(optimalTable);
                    }
                    branches.Add(newTable);
                }
            }
        }
    }
}
