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
        private List<int[]> basicVariableIndexes;
        private List<double[]> cBV;
        private List<double[,]> B;
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
        public List<int[]> GetBasicVariableIndexes()
        {
            int numRows = FinalTable.GetLength(0);
            int numCols = FinalTable.GetLength(1);
            List<int> basicVariableIndexes = new List<int>();
            List<int> nonBasicVariableIndexes = new List<int>();

            List<int[]> BV_NBV = new List<int[]>();

            double sum = 0;
            for (int i = 0; i < numCols - 1; i++)
            {
                sum = 0;
                for (int j = 1; j < numRows; j++)
                {
                    if (FinalTable[j,i] == 1 || FinalTable[j, i] == 0)
                    {
                        sum += FinalTable[j, i];
                    }
                    else
                    {
                        sum = 1000;
                        break;
                    }
                }
                if (sum == 1)
                {
                    basicVariableIndexes.Add(i);
                }
                else
                {
                    nonBasicVariableIndexes.Add(i);
                }
            }
            BV_NBV.Add(basicVariableIndexes.ToArray());
            BV_NBV.Add(nonBasicVariableIndexes.ToArray());

            return BV_NBV;
        }

        // CBV & CNBV
        public List<double[]> GetObjectiveFunctionCoefficients()
        {
            int[] BVIndexes = basicVariableIndexes.ElementAt(0);
            int[] nonBVIndexes = basicVariableIndexes.ElementAt(1);
            int numBVRows = BVIndexes.Length;
            int numNBVRows = nonBVIndexes.Length;
            double[] bvObjC = new double[numBVRows];
            double[] nbvObjC = new double[numNBVRows];

            List<double[]> CBV_CNBV = new List<double[]>();

            for (int i = 0; i < numBVRows; i++)
            {
                bvObjC[i] = InitialTable[0, BVIndexes[i]];
            }

            for (int i = 0; i < numBVRows; i++)
            {
                nbvObjC[i] = InitialTable[0, nonBVIndexes[i]];
            }

            CBV_CNBV.Add(bvObjC);
            CBV_CNBV.Add(nbvObjC);

            return CBV_CNBV;
        }

        // B & N
        public List<double[,]> GetBasicVariableColumns()
        {
            int[] BVIndexes = basicVariableIndexes.ElementAt(0);
            int[] nonBVIndexes = basicVariableIndexes.ElementAt(1);
            int numBVRows = FinalTable.GetLength(0) - 1;
            int numNBVRows = FinalTable.GetLength(0) - 1;
            int numBVCols = BVIndexes.Length;
            int numNBVCols = nonBVIndexes.Length;

            double[,] B = new double[numBVRows, numBVCols];
            double[,] N = new double[numNBVRows, numNBVCols];

            List<double[,]> B_N = new List<double[,]>();
            int iterB = 0;
            int iterN = 0;

            for (int i = 0; i < FinalTable.GetLength(1) - 1; i++)
            {
                if (BVIndexes.Contains(i))
                {
                    for (int j = 0; j < B.GetLength(0); j++)
                    {
                        //Console.WriteLine($"B Matrix values: {B[j, i]}");
                        B[j, iterB] = InitialTable[j + 1, BVIndexes[Array.IndexOf(BVIndexes, i)]];
                    }
                    iterB++;
                } 
                else if (nonBVIndexes.Contains(i))
                {
                    for (int j = 0; j < B.GetLength(0); j++)
                    {
                        //Console.WriteLine($"B Matrix values: {B[j, i]}");
                        N[j, iterN] = InitialTable[j + 1, nonBVIndexes[Array.IndexOf(nonBVIndexes, i)]];
                    }
                    iterN++;
                }
            }

            B_N.Add(B);
            B_N.Add(N);

            return B_N;
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
