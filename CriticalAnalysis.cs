using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace LPR381_Project
{
    internal class CriticalAnalysis
    {
        private double[,] initialTable;
        private double[,] finalTable;
        private int[] basicVariableIndexes;
        private double[] cBV;
        private double[,] B;
        private double[] b;

        public CriticalAnalysis(double[,] initialTable, double[,] finalTable)
        {
            this.InitialTable = initialTable;
            this.FinalTable = finalTable;

            basicVariableIndexes = GetBasicVariableIndexes();
            cBV = GetObjectiveFunctionCoefficients();
            B = GetBasicVariableColumns();
            b = GetRHSColumnValues();
        }

        public double[,] InitialTable { get => initialTable; set => initialTable = value; }
        public double[,] FinalTable { get => finalTable; set => finalTable = value; }
        // BV
        private int[] GetBasicVariableIndexes()
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

        // MATRIX MATH EXAMPLES:
        public double[,] MatrixMultiplyExample(double[] b, double[,] B)
        {
            // Reshape the vector into a 1x3 matrix (double[,])
            double[,] newb = new double[1, b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                newb[0, i] = b[i];
            }

            Matrix<double> matrixb = Matrix<double>.Build.DenseOfArray(newb);
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(B);

            Matrix<double> result = matrixb * matrixB;

            return result.ToArray();
        }

        public double DeterminantExample(double[,] B)
        {
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(B);

            double result = matrixB.Determinant();

            return result;
        }
    }
}
