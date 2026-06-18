using System;
using System.Windows;

namespace CreditCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tbAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму кредита (положительное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(tbRate.Text, out double rate) || rate < 0)
            {
                MessageBox.Show("Введите корректную процентную ставку (неотрицательное число).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(tbTerm.Text, out int term) || term <= 0)
            {
                MessageBox.Show("Введите корректный срок (целое положительное число месяцев).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double monthlyRate = (rate / 100) / 12;

            double monthlyPayment;
            double totalPayment;
            double overpayment;

            if (monthlyRate == 0)
            {
                monthlyPayment = amount / term;
                totalPayment = amount;
                overpayment = 0;
            }
            else
            {
                double pow = Math.Pow(1 + monthlyRate, term);
                monthlyPayment = amount * (monthlyRate * pow) / (pow - 1);
                totalPayment = monthlyPayment * term;
                overpayment = totalPayment - amount;
            }

            tbMonthlyPayment.Text = $"Ежемесячный платёж: {monthlyPayment:F2} ₽";
            tbTotalPayment.Text = $"Общая сумма выплат: {totalPayment:F2} ₽";
            tbOverpayment.Text = $"Переплата: {overpayment:F2} ₽";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbAmount.Text = string.Empty;
            tbRate.Text = string.Empty;
            tbTerm.Text = string.Empty;
            tbMonthlyPayment.Text = string.Empty;
            tbTotalPayment.Text = string.Empty;
            tbOverpayment.Text = string.Empty;
        }
    }
}