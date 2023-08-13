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
        public List<string> headers;
        List<string> rowHeaders;

        DataGridView dataGrid;
        public BranchTable(BranchTable table) 
        {
            this.headers = table.headers;
            this.level = table.level;
            this.rowHeaders = table.rowHeaders;
            this.table = table.table;
            this.DataGrid = table.DataGrid;
        }
        public BranchTable(string level, double[,] table, List<string> headers, List<string> rowHeaders)
        {
            this.Level = level;
            this.Table = table;
            this.headers = headers;
            this.rowHeaders = rowHeaders;
            ///////////////////
            ///Style data grid here
            dataGrid = new DataGridView
            {
                Width = 500,
                Top = 0,
                Left = 0,
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = true,
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
                RowHeadersDefaultCellStyle = new DataGridViewCellStyle
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
            populateGrid();
        }
        public void populateGrid()
        {
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            int cols = table.GetLength(1);
            int rows = table.GetLength(0);
            dataGrid.Width = (cols + 1) * 100 + 5;
            dataGrid.Height = (rows + 1) * 30;
            dataGrid.RowHeadersWidth = 100;

            for (int j = 0; j < cols; j++)
            {
                dataGrid.Columns.Add($"Column{j}", $" {headers[j]}");
                dataGrid.Columns[j].Width = 100;
            }
            for (int i = 0; i < rows; i++)
            {
                string[] row = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                    row[j] = Math.Round(table[i, j], 3).ToString();
                }

                dataGrid.Rows.Add(row);
                dataGrid.Rows[i].Height = 30;
                dataGrid.Rows[i].HeaderCell.Value = rowHeaders[i];

            }
        }
        public double[,] Table { get => table; set => table = value; }
        public string Level { get => level; set => level = value; }
        public DataGridView DataGrid { get => dataGrid; set => dataGrid = value; }
    }
}