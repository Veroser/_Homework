#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

struct Book {
    std::string title;
    std::string author;
    int year;
};

struct Student {
    std::string name;
    double grade;
};

void printBook(const Book& s) {
    std::cout << "Название: " << s.title << ", автор: " << s.author << ", год: " << s.year << std::endl;
}

void increaseGrade(Student& s, double value) {
    s.grade += value;
}

int main() {
    std::vector<Student> students = {
        {"Миша", 4.5},
        {"Маша", 3.8},
        {"Рома", 4.9},
        {"Кирилл", 4.2},
        {"Евгений", 4.7}
    };

    auto maxElement = std::max_element(students.begin(), students.end(), 
        [](const Student& a, const Student& b) {
            return a.grade < b.grade;
        });

    if (maxElement != students.end()) {
        std::cout << "Студент с максимальным баллом: " << maxElement->name 
                  << ", балл: " << maxElement->grade << std::endl;
    }

    return 0;
}