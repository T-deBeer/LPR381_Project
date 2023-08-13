using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project
{
    internal class AlgebraicExpression
    {
        double coefficient;
        string tag;
        double constant;
        public AlgebraicExpression(string tag, double coefficient = 0, double constant = 0)
        {
            this.Tag = tag;
            this.Coefficient = coefficient;
            this.Constant = constant;
        }

        public double Coefficient { get => coefficient; set => coefficient = value; }
        public string Tag { get => tag; set => tag = value; }
        public double Constant { get => constant; set => constant = value; }

        public void addExpresion(AlgebraicExpression expression)
        {
            Coefficient += expression.Coefficient;
            Constant += expression.Constant;
        }
        public void multiplyConstant(double constant)
        {
            Coefficient = constant;
            Constant = constant;
        }
        override
        public string ToString()
        {
            return $"{Coefficient}{Tag} + {constant}";
        }
    }
}

