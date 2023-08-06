using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LPR381_Project
{
    internal class Duality
    {
        private string[] lp;

        public Duality(string[] lp)
        {
            this.LP = lp;
        }

        public string[] LP { get => lp; set => lp = value; }

        private string MakeDual()
        {
            // Get all the values needed.
            //string[] rows = LP.Split('\n');
            string[] rows = LP;
            string type = "";
            List<string> rhs = new List<string>();
            List<string> signlist = new List<string>();
            List<string> restrictionList = new List<string>();
            List<string[]> simplexForm = new List<string[]>();

            for (int i = 0; i < rows.Length; i++)
            {
                int startIndex = 0;
                int endIndex = 0;
                string[] value;

                string[] columns = rows[i].Split(' ');
                //If it is the 1st row.
                if (i == 0)
                {
                    type = columns[0];
                    value = new string[columns.Length - 1];
                    startIndex = 1;
                    endIndex = columns.Length;
                }
                // If it is the last row.
                else if (i == rows.Length - 1)
                {
                    restrictionList = columns.ToList();
                    value = new string[0];
                    startIndex = columns.Length;
                    endIndex = columns.Length;
                }
                else
                {
                    rhs.Add(columns[columns.Length - 1]);

                    signlist.Add(columns[columns.Length - 2]);

                    value = new string[columns.Length - 2];
                    startIndex = 0;
                    endIndex = columns.Length - 2;
                }

                for (int j = startIndex; j < endIndex; j++)
                {
                    value[j - startIndex] = columns[j];
                }

                simplexForm.Add(value);
            }

            // Change from max to min or min to max.
            List<string[]> dualForm = new List<string[]>();
            List<string> insert = new List<string>();
            string dualType = "";
            if (type == "max")
            {
                insert.Add("min");
                dualType = "min";
            }
            else
            {
                insert.Add("max");
                dualType = "max";
            }
            // Make the rhs values the objective function weights.
            foreach (var val in rhs)
            {
                if (double.Parse(val) >= 0)
                {
                    insert.Add("+" + val);
                }
                else
                {
                    insert.Add(val);
                }
            }
            dualForm.Add(insert.ToArray());

            // Add decision variables in constraints.
            // Make temporary array to iterate through.
            string[,] tempArr = new string[simplexForm.Count - 2, simplexForm.ToArray()[0].Length];
            string[] z = new string[simplexForm.ToArray()[0].Length];
            int a = -1;
            foreach (var row in simplexForm)
            {
                if (a == -1)
                {
                    for (int k = 0; k < row.Length; k++)
                    {
                        z[k] = row[k];
                    }
                    a++;
                    continue;
                }
                for (int k = 0; k < row.Length; k++)
                {
                    tempArr[a, k] = row[k];
                }
                a++;
            }

            // Add the data.
            for (int i = 0; i < tempArr.GetLength(1); i++)
            {
                // Add the weights.
                insert.Clear();
                for (int j = 0; j < tempArr.GetLength(0); j++)
                {
                    insert.Add(tempArr[j, i]);
                }
                // Add the signs.
                string[] signRestrictionArr = restrictionList.ToArray();
                if (dualType == "max")
                {
                    switch (signRestrictionArr[i])
                    {
                        case "+":
                            insert.Add("<=");
                            break;
                        case "-":
                            insert.Add(">=");
                            break;
                        case "urs":
                            insert.Add("=");
                            break;
                        default:
                            insert.Add("<=");
                            break;
                    }
                }
                else
                {
                    switch (signRestrictionArr[i])
                    {
                        case "+":
                            insert.Add(">=");
                            break;
                        case "-":
                            insert.Add("<=");
                            break;
                        case "urs":
                            insert.Add("=");
                            break;
                        default:
                            insert.Add(">=");
                            break;
                    }
                }

                // Add the rhs.
                insert.Add(z[i].Replace("+", " ").Trim());

                dualForm.Add(insert.ToArray());
            }

            // Add the sign restrictions.
            insert.Clear();
            string[] signArr = signlist.ToArray();
            for (int i = 0; i < tempArr.GetLength(0); i++)
            {
                if (type == "max")
                {
                    switch (signArr[i])
                    {
                        case ">=":
                            insert.Add("-");
                            break;
                        case "<=":
                            insert.Add("+");
                            break;
                        case "=":
                            insert.Add("urs");
                            break;
                        default:
                            insert.Add("-");
                            break;
                    }
                }
                else
                {
                    switch (signArr[i])
                    {
                        case ">=":
                            insert.Add("+");
                            break;
                        case "<=":
                            insert.Add("-");
                            break;
                        case "=":
                            insert.Add("urs");
                            break;
                        default:
                            insert.Add("+");
                            break;
                    }
                }
            }

            dualForm.Add(insert.ToArray());

            string final = "";

            foreach (var row in dualForm)
            {
                if (dualForm.IndexOf(row) == dualForm.Count - 1)
                {
                    final = final + string.Join(" ", row);
                }
                else
                {
                    final = final + string.Join(" ", row) + "\n";
                }
            }

            return final;

            //string line = "";
            //foreach (var item in dualForm)
            //{
            //    foreach (var val in item)
            //    {
            //        line = line + val + "\t";
            //    }
            //    line = line + "\n";
            //}

            //Console.WriteLine(line);

            //Console.WriteLine("Problem: " + type + "\n");
            //// Output Simplex.
            //string line = "";
            //foreach (var item in simplexForm)
            //{
            //    foreach (var val in item)
            //    {
            //        line = line + val + "\t";
            //    }
            //    line = line + "\n";
            //}

            //Console.WriteLine(line);

            //line = "";
            //foreach (var item in signlist)
            //{
            //    line = line + item + "\t";
            //}
            //Console.WriteLine(line);

            //line = "";
            //foreach (var item in rhs)
            //{
            //    line = line + item + "\t";
            //}
            //Console.WriteLine(line);

            //line = "";
            //foreach (var item in restrictionList)
            //{
            //    line = line + item + "\t";
            //}
            //Console.WriteLine(line);
        }

        public string PrintResults()
        {
            string dualForm = MakeDual();
            LinearModel dualModel = new LinearModel(dualForm.Split("\n"));

            List<double[,]> iteration = dualModel.SimplexTables;
            string line = "";

            foreach (var table in iteration)
            {
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    string res = "";
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        res += $"{table[i, j]}\t";
                    }
                    line += $"{res}\n";
                }

                line += "\n\n";
            }

            return line;
        }
    }
}
