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
        private int xColumnsAmount;
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
        public BranchnBound(double[,] initialTable)
        {
            this.initialTable = ArrayToList(initialTable);            
            int columns = initialTable.GetLength(1);
            int rows = initialTable.GetLength(0);
            this.xColumnsAmount = columns - rows;
        }
        private bool IsBasic(int columnIndex, List<List<double>> table)
        {
            List<double> column = new List<double>();
            int rows = table.Count();
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
        private List<double> GetRHS(List<List<double>> table)
        {
            List<double> rhs = new List<double>();
            int rows = table.Count();
            int columns = table[0].Count();
            for (int i = 1; i < rows; i++)
            {
                rhs.Add(table[i][columns - 1]);
            }
            return rhs;
        }
        private int DetermineCutRow(List<List<double>> table)
        {
            List<double> rhs = GetRHS(table);
            List<double> fractions = new List<double>();
            for (int i = 0; i < rhs.Count(); i++)
            {
                double fraction = Math.Floor(rhs[i]) - rhs[i];
                fractions.Add(Math.Abs(Math.Abs(fraction) - 0.5));
            }
            double lowestFraction = fractions.Min();
            bool hasMinDuplicates = fractions.FindAll(x => x == lowestFraction).Count > 1;
            if (hasMinDuplicates)
            {
                int indexOfLargestRHS = 0;
                for (int i = 0; i < fractions.Count(); i++)
                {
                    if (fractions[i] == lowestFraction && rhs[i] > rhs[indexOfLargestRHS])
                    {
                        indexOfLargestRHS = i;
                    }
                }
                return indexOfLargestRHS + 1;
            }
            else
            {
                return fractions.IndexOf(lowestFraction) + 1;
            }
        }
        public List<List<double>> GenerateConstraints(int rowIndex, int columnIndex, List<List<double>> table)
        {
            int columns = table[0].Count();
            List<List<double>> newRows = new List<List<double>>();
           
            double rhs = table[rowIndex][columns - 1];
            List<double> constraintRow = Enumerable.Repeat(0.0, columns).ToList();
            double rhsLower = Math.Floor(rhs);
            constraintRow[columnIndex] = 1.0;
            constraintRow[columns - 1] = rhsLower;
            constraintRow.Insert(columns - 1, 1);                        

            newRows.Add(constraintRow);
        
            constraintRow = Enumerable.Repeat(0.0, columns).ToList();
            double rhsHigher = Math.Ceiling(rhs);
            constraintRow[columnIndex] = 1.0;
            constraintRow[columns - 1] = rhsHigher;
            constraintRow.Insert(columns - 1, -1);            

            newRows.Add(constraintRow);

            return newRows;
        }
        private BranchTable AddConstraint(List<double> constraint, BranchTable branchTable, bool makeNegative = false)
        {
            List<List<double>> table = ArrayToList(branchTable.Table);
            int columns = table[0].Count();
            int rows = table.Count();
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
                table[i].Insert(columns - 1, 0);
            }

            columns++;

            for (int i = 0; i < clashes.Count(); i++)
            {
                List<double> newRow = new List<double>();
                for (int j = 0; j < columns; j++)
                {
                    double newElement = (table[clashes[i]][j] - constraint[j]) * (makeNegative ? -1 : 1);
                    newRow.Add(newElement);
                }
                table.Add(newRow);
            }            
            rows++;
            branchTable.Table = ListToArray(table);
            return branchTable;
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
        public List<BranchTable> Solve(string problemType, List<string> columnHeaders, List<string> rowHeaders)
        {
            Queue<BranchTable> tableQueue = new Queue<BranchTable> ();
            List<BranchTable> branches = new List<BranchTable>();

            tableQueue.Enqueue(new BranchTable("0", ListToArray(initialTable), columnHeaders, rowHeaders));

        //    while (tableQueue.Count != 0)
        //    {
        //        BranchTable branchTable = tableQueue.Dequeue();
        //        List<List<double>> table = ArrayToList(branchTable.Table);      
                
        //        int row = DetermineCutRow(table);                    /// ifi
        //        List<List<double>> constraints = GenerateConstraints(row, table[row].IndexOf(1), table);

                for (int i = 0; i < constraints.Count(); i++)
                {
                    BranchTable branchTableIteration = new BranchTable(branchTable);
                    branchTableIteration = AddConstraint(constraints[i], branchTableIteration, constraints[i].Last() > 0);
                    
                    Simplex simplex = new Simplex(branchTableIteration.Table, problemType);
                    List<double[,]> pivots = simplex.DualSimplexAlgorithm();
                    
                    double[,] optimalTable = pivots.Last();                  
                    if (branchTableIteration.Table != optimalTable)
                    {
                        List<string> newColumnHeaders = columnHeaders;
                        newColumnHeaders.Insert(columnHeaders.Count() - 1, i % 2 == 0 ? "S" : "E");
                        List<string> newRowHeaders = rowHeaders;
                        newRowHeaders.Add((int.Parse(newRowHeaders.Last()) + 1).ToString());

                        BranchTable newAddition = new BranchTable($"{branchTableIteration.Level}{i + 1}", optimalTable, newColumnHeaders, newRowHeaders);
                        bool hasFraction = false;
                        for (int j = 0; j < optimalTable.GetLength(0); j++)
                        {
                            hasFraction = optimalTable[j, optimalTable.GetLength(1) - 1] - Math.Floor(optimalTable[j, optimalTable.GetLength(0) - 1]) > 0;
                            if (hasFraction)
                            {
                                break;
                            }
                        }
                        if (hasFraction)
                        {
                            tableQueue.Enqueue(newAddition);
                        }
                        for (int j = 0; j < pivots.Count(); j++)
                        {
                            branches.Add(new BranchTable(newAddition.Level, optimalTable, columnHeaders, rowHeaders));
                        }
                        branches.Add(newAddition);
                    }
                    else 
                    {
                        MessageBox.Show("");
                    }
                }
            }
            return branches;
        }
    }
}
