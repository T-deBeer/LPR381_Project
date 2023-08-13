﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Providers.LinearAlgebra;


namespace LPR381_Project
{
    internal class CriticalAnalysis
    {

        public List<Dictionary<int, int>> basicVariableCoords;
        public double[] cBV;
        public double[,] B;
        public double[] z;

        public CriticalAnalysis(double[,] initialTable, double[,] finalTable)
        {
            this.InitialTable = initialTable;
            this.FinalTable = finalTable;

            this.basicVariableCoords = GetBasicVariableCoords();
            this.cBV = GetCBV();
            this.B = GetB();
            this.z = GetRHS();

            Matrix<double> matrixCbv = Matrix<double>.Build.DenseOfRowArrays(this.cBV);
            Matrix<double> matrixZ = Matrix<double>.Build.DenseOfColumnArrays(this.z);
            Matrix<double> matrixB = Matrix<double>.Build.DenseOfArray(this.B);

            BInverse = matrixB.Inverse().Transpose();
            CbvBinverse = matrixCbv.Multiply(BInverse);
        }

        

        public double[,] InitialTable { get; set; }
        public double[,] FinalTable { get; set; }
        public Matrix<double> BInverse { get; set; }
        public Matrix<double> CbvBinverse { get; set; }

        //Basic Variables
        public List<Dictionary<int, int>> GetBasicVariableCoords()
        {

            List<int> columnIndices = new List<int>();
            List<Dictionary<int, int>> coord = new List<Dictionary<int, int>>();

            for (int i = 0; i < FinalTable.GetLength(1); i++)
            {
                double sum = 0;
                for (int j = 0; j < FinalTable.GetLength(0); j++)
                {
                    sum += FinalTable[j, i];
                }

                if (sum == 1)
                {
                    columnIndices.Add(i);
                }
            }

            for (int i = 0; i < FinalTable.GetLength(0); i++)
            {
                foreach (var index in columnIndices)
                {
                    if (FinalTable[i, index] == 1)
                    {
                        Dictionary<int, int> b = new Dictionary<int, int>();
                        b.Add(i, index);

                        coord.Add(b);
                    }
                }
            }

            return coord;
        }

        public double[] GetCBV()
        {
            double[] CBV = new double[FinalTable.GetLength(0) - 1];

            int count = 0;
            if (basicVariableCoords.Count == CBV.Length)
            {
                foreach (var coord in basicVariableCoords)
                {
                    foreach (var kvp in coord)
                    {
                        CBV[count] = -1 * InitialTable[0, kvp.Value];
                        count++;
                    }
                }
            }
            else
            {
                MessageBox.Show("Something went wrong! No Cbv could be found.");
            }
            

            return CBV;
        }

        public double[,] GetB()
        {
            double[,] B = new double[FinalTable.GetLength(0) - 1, FinalTable.GetLength(0) - 1];

            int count = 0;
            foreach (var coord in basicVariableCoords)
            {
                foreach (var kvp in coord)
                {
                    int row = 0;
                    for (int i = 1; i < InitialTable.GetLength(0); i++)
                    {
                        if (kvp.Key == i)
                        {
                            for (int j = 1; j < InitialTable.GetLength(0); j++)
                            {
                                B[count, row] = InitialTable[j, kvp.Value];
                                row++;
                            }
                            
                        }
                        
                    }
                    count++;
                }
            }

            return B;
        }
        public double[] GetRHS()
        {
            double[] result = new double[InitialTable.GetLength(0) - 1];

            for (int i = 1; i < InitialTable.GetLength(0); i++)
            {
                result[i-1] = InitialTable[i, InitialTable.GetLength(1)-1];
            }

            return result;
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
