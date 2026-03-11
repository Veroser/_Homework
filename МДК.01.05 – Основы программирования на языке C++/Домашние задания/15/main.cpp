#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

using namespace std;

struct Student {
    string name;
    double grade;
    string subject;
    
    Student(string n, double g, string s) : name(n), grade(g), subject(s) {}
};

int main() {
    Student group[4] = {
        Student("Анна", 4.8, "Математика"),
        Student("Борис", 3.9, "Физика"),
        Student("Виктор", 4.6, "Химия"),
        Student("Галина", 4.2, "Биология")
    };
    
    cout << "Студенты с оценкой > 4.5:" << endl;
    for (int i = 0; i < 4; i++) {
        if (group[i].grade > 4.5) {
            cout << group[i].name << " - " << group[i].grade << endl;
        }
    }
    
    vector<Student> students;
    students.push_back(Student("Елена", 4.7, "Биология"));
    students.push_back(Student("Фёдор", 4.9, "Биология"));
    students.push_back(Student("Григорий", 4.5, "Математика"));
    students.push_back(Student("Дарья", 4.8, "Химия"));
    students.push_back(Student("Иван", 4.6, "Биология"));
    
    string targetSubject = "Биология";
    double maxGrade = -1;
    string topStudent = "";
    
    for (const auto& student : students) {
        if (student.subject == targetSubject && student.grade > maxGrade) {
            maxGrade = student.grade;
            topStudent = student.name;
        }
    }
    
    cout << "\n " << targetSubject << ": " << topStudent << " - " << maxGrade << endl;
    
    vector<Student> studentList;
    
    auto addStudent = [&](string name, double grade, string subject) {
        studentList.push_back(Student(name, grade, subject));
    };
    
    auto editStudent = [&](string name, double newGrade, string newSubject) {
        for (auto& student : studentList) {
            if (student.name == name) {
                student.grade = newGrade;
                student.subject = newSubject;
                break;
            }
        }
    };
    
    auto deleteStudent = [&](string name) {
        studentList.erase(
            remove_if(studentList.begin(), studentList.end(),
                     [&](const Student& s) { return s.name == name; }),
            studentList.end()
        );
    };
    
    auto searchStudent = [&](string name) -> Student* {
        for (auto& student : studentList) {
            if (student.name == name) {
                return &student;
            }
        }
        return nullptr;
    };
    
    addStudent("Иван", 4.5, "Математика");
    addStudent("Мария", 4.8, "Физика");
    addStudent("Петр", 4.2, "Химия");
    
    cout << "\nНачальный список:" << endl;
    for (const auto& s : studentList) {
        cout << s.name << " - " << s.grade << " (" << s.subject << ")" << endl;
    }
    
    editStudent("Иван", 4.7, "Биология");
    
    cout << "\nПосле изменения оценки и предмета Ивана:" << endl;
    for (const auto& s : studentList) {
        cout << s.name << " - " << s.grade << " (" << s.subject << ")" << endl;
    }
    
    deleteStudent("Петр");
    
    cout << "\nПосле удаления Петра:" << endl;
    for (const auto& s : studentList) {
        cout << s.name << " - " << s.grade << " (" << s.subject << ")" << endl;
    }
    
    Student* found = searchStudent("Мария");
    if (found != nullptr) {
        cout << "\nНайден студент: " << found->name << " - " << found->grade << " (" << found->subject << ")" << endl;
    } else {
        cout << "\nСтудент не найден" << endl;
    }
    
    return 0;
}