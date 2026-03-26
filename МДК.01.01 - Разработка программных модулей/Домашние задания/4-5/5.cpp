#include<iostream>
using namespace std;

class Car{
public:
    string brand;
    int year;
    int speed;
    void setData(string b, int y, int s){
        brand = b;
        year = y;
        speed = s;
    }
    void printInfo(){
        cout << "Марка: " << brand << endl;
        cout << "Год выпуска: " << year << endl;
        cout << "Скорость: " << speed << endl;
    }
};

class BankAccount{
public:
    int number;
    string owner;
    int balance;
    void setData(int n, string o, int b){
        number = n;
        owner = o;
        balance = b;
    }

    void deposit(int amount){
        balance += amount;
    }
    void withdraw(int amount){
        balance -= amount;
    }
    void getBalance(){
        cout << "Баланс: " << balance << endl;
    }

};

class Animal{ 
protected:
    string name;
public:
    void setName(string n){
        name = n;
    }
};

int main(){

}