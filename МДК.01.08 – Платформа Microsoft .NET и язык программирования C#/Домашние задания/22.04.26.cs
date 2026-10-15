using System;
namespace ConsoleApp1;
abstract class BankAccount
{
    public string AccountNumber { get; set; }
    public int Balance { get; set; }

    public void Deposit(int amount)
    {
        Balance = Balance + amount;
    }

    public abstract void Withdraw(int amount);

    public void DisplayBalance()
    {
        Console.WriteLine("Счет: " + AccountNumber);
        Console.WriteLine("Баланс: " + Balance);
        Console.WriteLine();
    }
}

class SavingsAccount : BankAccount
{
    public int MinimumBalance { get; set; }

    public override void Withdraw(int amount)
    {
        if (Balance - amount >= MinimumBalance)
        {
            Balance = Balance - amount;
        }
        else
        {
            Console.WriteLine("Нельзя опустить баланс ниже минимума");
        }
    }
}

class CheckingAccount : BankAccount
{
    public int Fee { get; set; }

    public override void Withdraw(int amount)
    {
        Balance = Balance - amount - Fee;
    }
}

class CreditAccount : BankAccount
{
    public int Limit { get; set; }

    public override void Withdraw(int amount)
    {
        if (Balance - amount >= -Limit)
        {
            Balance = Balance - amount;
        }
        else
        {
            Console.WriteLine("Превышен лимит кредита");
        }
    }
}

class Program2
{
    static void Main()
    {
        SavingsAccount sa = new SavingsAccount();
        sa.AccountNumber = "S001";
        sa.MinimumBalance = 100;
        sa.Deposit(1000);
        sa.Withdraw(500);
        sa.Withdraw(500);
        sa.DisplayBalance();

        CheckingAccount ca = new CheckingAccount();
        ca.AccountNumber = "C001";
        ca.Fee = 50;
        ca.Deposit(1000);
        ca.Withdraw(300);
        ca.DisplayBalance();

        CreditAccount cra = new CreditAccount();
        cra.AccountNumber = "CR001";
        cra.Limit = 500;
        cra.Deposit(1000);
        cra.Withdraw(1300);
        cra.Withdraw(500);
        cra.DisplayBalance();
    }
}