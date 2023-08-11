using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class CriticalAnalysis
    {
        private double[,] initialTable;
        private double[,] finalTable;

        public CriticalAnalysis(double[,] initialTable, double[,] finalTable)
        {
            this.InitialTable = initialTable;
            this.FinalTable = finalTable;
        }

        public double[,] InitialTable { get => initialTable; set => initialTable = value; }
        public double[,] FinalTable { get => finalTable; set => finalTable = value; }
            // BV
        public int[] GetBasicVariableIndexes()
        {
            int numRows = FinalTable.GetLength(0);
            int numCols = FinalTable.GetLength(1);
            int[] basicVariableIndexes = new int[numRows - 1];

            for (int i = 1; i < numRows; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    if (Math.Abs(FinalTable[i, j] - 1.0) < 1e-10)
                    {
                        basicVariableIndexes[i - 1] = j;
                        break;
                    }
                }
            }

            return basicVariableIndexes;
        }

        //// xBV
        //public static double[,] GetBasicVariablesMatrix(double[,] initialTable, int[] basicVariableIndexes)
        //{
        //    int numRows = basicVariableIndexes.Length;
        //    int numCols = initialTable.GetLength(1);
        //    double[,] basicVariablesMatrix = new double[numRows, numCols];

        //    for (int i = 0; i < numRows; i++)
        //    {
        //        int basicVarIdx = basicVariableIndexes[i];
        //        for (int j = 0; j < numCols; j++)
        //        {
        //            basicVariablesMatrix[i, j] = initialTable[i + 1, j];
        //        }
        //    }

        //    return basicVariablesMatrix;
        //}

        // CBV
        public double[] GetObjectiveFunctionCoefficients()
        {
            int[] basicVariableIndexes = GetBasicVariableIndexes();
            int numRows = basicVariableIndexes.Length;
            double[] objectiveFunctionCoefficients = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                int basicVarIdx = basicVariableIndexes[i];
                objectiveFunctionCoefficients[i] = InitialTable[0, basicVarIdx];
            }

            return objectiveFunctionCoefficients;
        }

        // B
        public double[,] GetBasicVariableColumns()
        {
            int[] basicVariableIndexes = GetBasicVariableIndexes();
            int numRows = basicVariableIndexes.Length;
            int numCols = numRows;
            double[,] B = new double[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                int basicVarIdx = basicVariableIndexes[i];
                for (int j = 0; j < numRows; j++)
                {
                    B[j, i] = InitialTable[j + 1, basicVarIdx];
                    //Console.WriteLine($"B Matrix values: {B[j, i]}");
                }
            }

            return B;
        }

        // b
        public double[] GetRHSColumnValues()
        {
            int numRows = InitialTable.GetLength(0) - 1;
            double[] rhsColumn = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                rhsColumn[i] = InitialTable[i + 1, InitialTable.GetLength(1) - 1];
                //Console.WriteLine($"RHS Constraint values: {rhsColumn[i]}");
            }

            return rhsColumn;
        }
    }

        ////////////////////////////////////////////////////////////////////////
}