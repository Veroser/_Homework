using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFigures.SelectionChanged += CmbFigures_SelectionChanged;
        }

        private void CmbFigures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spInput.Children.Clear();
            if (cmbFigures.SelectedIndex == 0)
            {
                spInput.Children.Add(new TextBlock { Text = "Радиус:" });
                spInput.Children.Add(new TextBox { Name = "txtRadius" });
            }
            else if (cmbFigures.SelectedIndex == 1)
            {
                spInput.Children.Add(new TextBlock { Text = "Ширина:" });
                spInput.Children.Add(new TextBox { Name = "txtWidth" });
                spInput.Children.Add(new TextBlock { Text = "Высота:" });
                spInput.Children.Add(new TextBox { Name = "txtHeight" });
            }
            else if (cmbFigures.SelectedIndex == 2)
            {
                spInput.Children.Add(new TextBlock { Text = "Основание:" });
                spInput.Children.Add(new TextBox { Name = "txtBase" });
                spInput.Children.Add(new TextBlock { Text = "Высота:" });
                spInput.Children.Add(new TextBox { Name = "txtHeightTriangle" });
            }
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            double area = 0;
            try
            {
                if (cmbFigures.SelectedIndex == 0)
                {
                    double r = double.Parse(((TextBox)spInput.Children[1]).Text);
                    area = Math.PI * r * r;
                }
                else if (cmbFigures.SelectedIndex == 1)
                {
                    double w = double.Parse(((TextBox)spInput.Children[1]).Text);
                    double h = double.Parse(((TextBox)spInput.Children[3]).Text);
                    area = w * h;
                }
                else if (cmbFigures.SelectedIndex == 2)
                {
                    double b = double.Parse(((TextBox)spInput.Children[1]).Text);
                    double h = double.Parse(((TextBox)spInput.Children[3]).Text);
                    area = 0.5 * b * h;
                }
                txtResult.Text = $"Площадь: {area:F2}";
            }
            catch
            {
                txtResult.Text = "Ошибка: введите числа";
            }
        }
    }
}