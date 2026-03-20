#include <iostream>
using namespace std;

class Car {
private:
    int fuel = 0;
public:
    string model;

    void drive(int distance){
        if (fuel >= distance)
            fuel -= distance;
    }

    void refuel(int amount){
        if (amount > 0)
            fuel += amount;
    }

    int getFuel()const {

        return fuel;
    }
};

int main(){
    Car car;
    Car car2;

    car.model = "Toyota";
    car2.model = "BMW";
    car.refuel(25); 
    car2.refuel(30);
    cout << car.model << " Осталось топлива: " << car.getFuel() << "л" << endl;
    cout << car2.model << " Осталось топлива: " << car2.getFuel() << "л" << endl;
    car.drive(20);
    car2.drive(10);
    cout << car.model << " Осталось топлива: " << car.getFuel() << "л" << endl;
    cout << car2.model << " Осталось топлива: " << car2.getFuel() << "л" << endl;


    return 0;

}
