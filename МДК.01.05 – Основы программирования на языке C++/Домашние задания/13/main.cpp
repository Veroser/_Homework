#include <iostream>

//Создать структуру Point с полями x и y. 
//Создать две точки и вывести расстояние между ними (формула любая, без sqrt
//можно).

using namespace std;

struct Point {
    double x;
    double y;
};

double distancePoints(const Point& p1, const Point& p2){
    double dx = p2.x - p1.x;
    double dy = p2.y - p1.y;
    return dx * dx + dy * dy;
}

struct Track {
    string name;
    int duration;
    int count;
};

int main() {
    Track inicialD = {"Night of Fire", 300, 20474844};
    cout << "Название: " << inicialD.name << endl;
    cout << "Длительность: " << inicialD.duration << "c" << endl;
    cout << "Всего прослушиваний: " << inicialD.count << endl;


    Point point1 = {1, 5};
    Point point2 = {4, 9};

    double distance = distancePoints(point1, point2);
    cout << "Расстояние между точками: " << distance << endl;

    return 0;
}
