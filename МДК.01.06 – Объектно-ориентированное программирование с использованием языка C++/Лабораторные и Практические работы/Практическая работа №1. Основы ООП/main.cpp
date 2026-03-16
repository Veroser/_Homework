#include <iostream>
#include <string>
using namespace std;

class Student {
private:
    string name;
    int age;
    double grade;

public:
    Student(string name, int age, double grade) {
        this->name = name;
        this->age = age;
        this->grade = grade;
        cout << "Student created" << endl;
    }
    
    ~Student() {
        cout << "Student destroyed" << endl;
    }
    
    void setData(string name, int age, double grade) {
        this->name = name;
        this->age = age;
        this->grade = grade;
    }
    
    void printInfo() {
        cout << "Имя: " << name << endl;
        cout << "Возраст: " << age << " лет" << endl;
        cout << "Средний балл: " << grade << endl;
    }
};

class Car {
public:
    string brand;
    
private:
    int year;
    
protected:
    int speed;     

public:
    Car(string brand, int year, int speed) {
        this->brand = brand;
        this->year = year;
        this->speed = speed;
        cout << "Car created" << endl;
    }
    
    ~Car() {
        cout << "Car destroyed" << endl;
    }
    
    void setYear(int y) {
        year = y;
    }
    
    int getYear() {
        return year;
    }
    
    void setSpeed(int s) {
        speed = s;
    }
    
    int getSpeed() {
        return speed;
    }
    
    void printInfo() {
        cout << "Марка: " << brand << endl;
        cout << "Год выпуска: " << year << endl;
        cout << "Скорость: " << speed << " км/ч" << endl;
    }
};

class Product {
private:
    string name;
    double price;
    int quantity;

public:
    Product(string name, double price, int quantity) {
        this->name = name;
        this->price = price;
        this->quantity = quantity;
        cout << "Product created: " << name << endl;
    }
    
    ~Product() {
        cout << "Product destroyed: " << name << endl;
    }
    
    Product* setData(string name, double price, int quantity) {
        this->name = name;
        this->price = price;
        this->quantity = quantity;
        return this;
    }
    
    void printInfo() {
        cout << "Название: " << name << endl;
        cout << "Цена: " << price << " руб." << endl;
        cout << "Количество на складе: " << quantity << "шт" << endl;
    }
    
    void buy(int amount) {
        if (quantity >= amount) {
            quantity -= amount;
            cout << "Куплено " << amount << " шт товара \"" << name << "\"" << endl;
            cout << "Осталось на складе: " << quantity << " шт" << endl;
        } else {
            cout << "Ошибка: недостаточно товара" << endl;
        }
    }
};

int main() {
    
    Student s1("Alex", 20, 4.5);
    s1.printInfo();
    
    Student s2 = s1;
    s2.printInfo();
    
    s2.setData("Bob", 21, 4.8);
    s1.printInfo();
    s2.printInfo();

    //2
    Car c1("Toyota", 2020, 180);
    c1.printInfo();
    c1.setYear(2021);
    c1.setSpeed(200);
    c1.printInfo();
    
    //3
    Product p1("Laptop", 1500.0, 10);
    p1.printInfo();
    p1.buy(3);
    p1.printInfo();

    return 0;
}