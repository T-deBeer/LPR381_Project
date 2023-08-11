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
    }
}
