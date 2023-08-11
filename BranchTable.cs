using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPR381_Project
{
    internal class BranchTable
    {
        string level;
        double[,] table;        
        DataGridView dataGrid;
        public BranchTable(string level, double[,] table)
        {
            this.Level = level;
            this.Table = table;
            ///////////////////
            ///Style data grid here
            dataGrid = new DataGridView
            {
                Width = 500,
                Top = 0,
                Left = 0,
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                BackgroundColor = Color.FromArgb(34, 34, 34),
                // Lines go brrr
                GridColor = Color.FromArgb(34, 34, 34),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(30, 30, 30),
                    // Text
                    ForeColor = Color.FromArgb(255, 255, 255),
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(28, 131, 174),
                    // Text
                    ForeColor = Color.FromArgb(255, 255, 255),
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(40, 40, 40),
                    // Text
                    ForeColor = Color.FromArgb(255, 255, 255),
                },
            };
            ///////////////////
            dataGrid.RowHeadersDefaultCellStyle.BackColor = Color.Yellow;
            int cols = table.GetLength(1);
            int rows = table.GetLength(0);
            dataGrid.Width = cols * 100 + 5;
            dataGrid.Height = (rows + 1) * 30;
            for (int j = 0; j < cols; j++)
            {
                dataGrid.Columns.Add($"Column{j}", $"Header {j}");
                dataGrid.Columns[j].Width = 100;
            }            
            for (int i = 0; i < rows; i++)
            {
                string[] row = new string[cols]; 
                for (int j = 0; j < cols; j++)
                {
                    row[j] = table[i, j].ToString();                    
                }
                dataGrid.Rows.Add(row);
                dataGrid.Rows[i].Height = 30;
            }                        
        }

        public double[,] Table { get => table; set => table = value; }
        public string Level { get => level; set => level = value; }
        public DataGridView DataGrid { get => dataGrid; set => dataGrid = value; }
    }
}
