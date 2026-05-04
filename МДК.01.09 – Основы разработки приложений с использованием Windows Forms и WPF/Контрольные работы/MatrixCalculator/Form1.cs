using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatrixCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDataGridViews();
        }

        private void InitializeDataGridViews()
        {
            ConfigureDataGridView(dataGridViewA, "Матрица A");
            ConfigureDataGridView(dataGridViewB, "Матрица B");
            ConfigureDataGridView(dataGridViewResult, "Результат", true);
        }

        private void ConfigureDataGridView(DataGridView dgv, string title, bool readOnly = false)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.ColumnCount = 2;
            dgv.RowCount = 3;

            dgv.RowHeadersWidth = 40;
            dgv.ColumnHeadersHeight = 40;
            dgv.Font = new Font("Microsoft Sans Serif", 12);
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].HeaderText = (i + 1).ToString();
                dgv.Columns[i].Width = 60;
            }

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
                dgv.Rows[i].Height = 40;
            }

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            if (readOnly)
            {
                dgv.ReadOnly = true;
                dgv.BackgroundColor = SystemColors.Control;
                dgv.DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                dgv.CellValidating -= DataGridView_CellValidating;
                dgv.CellValidating += DataGridView_CellValidating;
            }

            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    dgv.Rows[i].Cells[j].Value = "0";
                }
            }
        }

        private void DataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            if (e.RowIndex < 0 || e.RowIndex >= dgv.RowCount) return;
            if (e.ColumnIndex < 0 || e.ColumnIndex >= dgv.ColumnCount) return;

            if (e.FormattedValue == null) return;

            string value = e.FormattedValue.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                dgv[e.ColumnIndex, e.RowIndex].Value = "0";
                return;
            }

            if (!int.TryParse(value, out int result))
            {
                MessageBox.Show("Пожалуйста, введите целое число! Будет установлен 0.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgv[e.ColumnIndex, e.RowIndex].Value = "0";
            }
        }

        private int[,] GetMatrixFromDataGridView(DataGridView dgv)
        {
            int[,] matrix = new int[2, 2];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    try
                    {
                        if (i < dgv.RowCount && j < dgv.ColumnCount && dgv.Rows[i].Cells[j].Value != null)
                        {
                            string value = dgv.Rows[i].Cells[j].Value.ToString();
                            if (!int.TryParse(value, out int num))
                            {
                                matrix[i, j] = 0;
                            }
                            else
                            {
                                matrix[i, j] = num;
                            }
                        }
                        else
                        {
                            matrix[i, j] = 0;
                        }
                    }
                    catch
                    {
                        matrix[i, j] = 0;
                    }
                }
            }

            return matrix;
        }

        private void SetMatrixToDataGridView(DataGridView dgv, int[,] matrix)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    try
                    {
                        if (i < dgv.RowCount && j < dgv.ColumnCount)
                        {
                            dgv.Rows[i].Cells[j].Value = matrix[i, j].ToString();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int[,] matrixA = GetMatrixFromDataGridView(dataGridViewA);
                int[,] matrixB = GetMatrixFromDataGridView(dataGridViewB);
                int[,] result = new int[2, 2];

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        result[i, j] = matrixA[i, j] + matrixB[i, j];
                    }
                }

                SetMatrixToDataGridView(dataGridViewResult, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сложении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            try
            {
                int[,] matrixA = GetMatrixFromDataGridView(dataGridViewA);
                int[,] matrixB = GetMatrixFromDataGridView(dataGridViewB);
                int[,] result = new int[2, 2];

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        result[i, j] = matrixA[i, j] - matrixB[i, j];
                    }
                }

                SetMatrixToDataGridView(dataGridViewResult, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычитании: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            try
            {
                int[,] matrixA = GetMatrixFromDataGridView(dataGridViewA);
                int[,] matrixB = GetMatrixFromDataGridView(dataGridViewB);
                int[,] result = new int[2, 2];

                result[0, 0] = matrixA[0, 0] * matrixB[0, 0] + matrixA[0, 1] * matrixB[1, 0];
                result[0, 1] = matrixA[0, 0] * matrixB[0, 1] + matrixA[0, 1] * matrixB[1, 1];
                result[1, 0] = matrixA[1, 0] * matrixB[0, 0] + matrixA[1, 1] * matrixB[1, 0];
                result[1, 1] = matrixA[1, 0] * matrixB[0, 1] + matrixA[1, 1] * matrixB[1, 1];

                SetMatrixToDataGridView(dataGridViewResult, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при умножении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}