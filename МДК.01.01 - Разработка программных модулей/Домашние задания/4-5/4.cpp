/*Создайте класс Rectangle, который содержит поля:

ширина (width)

высота (height)

Методы:

setSize(int w, int h) — установить размеры

area() — вычислить площадь

perimeter() — вычислить периметр*/
#include<iostream>
using namespace std;

class Student {
private:
    string name;
    int age;
    double grade;
public:
    void setData(string name, int age, double grade) {
        this->name = name;
        this->age = age;
        this->grade = grade;
    }
    void printInfo() {
        cout << "Name: " << name << endl;
        cout << "Age: " << age << endl;
        cout << "Grade: " << grade << endl;
    }
};

class Calculator {

    int sum(int a, int b) {
        return a + b;
    }
    int sub(int a, int b) {
        return a - b;
    }
    int mul(int a, int b) {
        return a * b;
    }
    int div(int a, int b) {
        return a / b;
    }
};

class Rectangle {    
public:
    double width, height;

    void setSize(int w, int h){
        width = w;
        height = h;
    }
    double area() {
        return width * height;
    }

    double perimeter() {
        return 2 * (width + height);
    }
};

int main() {
    Student s1;
    s1.setData("Вадим", 20, 3.5);
    s1.printInfo();
    return 0;
}