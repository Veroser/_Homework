#include <iostream>
#include <vector>
#include <string>

using namespace std;

struct Engine {
    string model;
    int power;
};

struct Car {
    string brand;
    int year;
    Engine engine;
};

struct Author {
    string name;
    string country;
};

struct Book {
    string title;
    int year;
    Author author;
};

struct Student {
    string name;
    string city;
    double grade;
};

void printStudentsByCity(const vector<Student>& students, const string& city) {
    cout << "Студенты, проживающие в городе " << city << ":" << endl;
    for (const auto& student : students) {
        if (student.city == city) {
            cout << student.name << " (оценка: " << student.grade << ")" << endl;
        }
    }
}

int main() {
    Car car;
    car.brand = "Toyota";
    car.year = 2020;
    car.engine.model = "3.0L 2JZ-GTE";
    car.engine.power = 320;
    
    cout << "Марка: " << car.brand << endl;
    cout << "Год выпуска: " << car.year << endl;
    cout << "Модель двигателя: " << car.engine.model << endl;
    cout << "Мощность: " << car.engine.power << " л.с." << endl;
    
    vector<Book> books = {
        {"Война и мир", 1869, {"Лев Толстой", "Россия"}},
        {"Преступление и наказание", 1866, {"Федор Достоевский", "Россия"}},
        {"Мастер и Маргарита", 1967, {"Михаил Булгаков", "Россия"}}
    };
    
    cout << "\nСписок книг:" << endl;
    for (const auto& book : books) {
        cout << "\"" << book.title << "\" (" << book.year << ") - " << book.author.name 
             << " (" << book.author.country << ")" << endl;
    }
    
    vector<Student> students = {
        {"Анна", "Москва", 4.8},
        {"Борис", "Санкт-Петербург", 4.5},
        {"Виктор", "Москва", 4.2},
        {"Галина", "Новосибирск", 4.9}
    };
    
    printStudentsByCity(students, "Москва");
    
    return 0;
}