using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project.Properties
{
    internal class BranchBound
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
        public BranchBound(double[,] initialTable, int xColumnsAmount)
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
        private List<List<double>> AddConstraint(List<double> constraint, List<List<double>> table)
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
            columns++;
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
                    if (sumWithoutConstraint + constraint[i] != 1)
                    {
                        clashes.Add(row);
                    }
                }
                sum.Add(sumWithoutConstraint);
            }
            for (int i = 0; i < constraint.Count(); i++)
            {
                constraint[i] = table[clashes[0]][i] - constraint[i];
            }
            rows++;
            return table;
        }
        public void Solve()
        {
            Queue<List<List<double>>> tableQueue = new Queue<List<List<double>>>();
            List<List<List<double>>> branches = new List<List<List<double>>>();
            tableQueue.Enqueue(initialTable);
            while (tableQueue.Count != 0)
            {
                List<List<double>> table = tableQueue.Dequeue();
                int row = DetermineCutRow(table);                    /// ifi
                List<List<double>> constraints = GenerateConstraints(row, table[row].IndexOf(1), table);
                for (int i = 0; i < constraints.Count(); i++)
                {
                    List<List<double>> newTable = AddConstraint(constraints[i], table);
                    //solvedTable = Solve table
                    double sum = 0;
                    for (int j = 0; j < newTable.Count(); j++)
                    {
                        sum += newTable[j][columns - 1];
                    }
                    if (Math.Floor(sum) - sum != 0)
                    {
                        tableQueue.Enqueue(newTable);
                    }
                    branches.Add(newTable);
                }
            }
        }
    }
}
