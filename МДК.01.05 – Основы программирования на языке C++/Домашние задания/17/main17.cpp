#include <iostream>
#include <vector>
#include <string>

using namespace std;

using Id = unsigned int;
using Score = int;
using Name = std::string;

struct Player {
    Id id;
    Score score;
    Name name;
};

using Matrix = std::vector<std::vector<int>>;

int sumMatrix(const Matrix& matrix) {
    int sum = 0;
    for (const auto& row : matrix) {
        for (int element : row) {
            sum += element;
        }
    }
    return sum;
}

typedef int (*FuncPtr)(int, int);
using FuncType = int(*)(int, int);

int add(int a, int b) {
    return a + b;
}

int subtract(int a, int b) {
    return a - b;
}

int multiply(int a, int b) {
    return a * b;
}

int calculator(int a, int b, FuncPtr operation) {
    return operation(a, b);
}

template<typename T>
using Vec = std::vector<T>;

int main() {
    Player players[] = {
        {1, 100, "Алексей"},
        {2, 150, "Мария"},
        {3, 200, "Дмитрий"}
    };
    
    for (const auto& player : players) {
        cout << "ID: " << player.id << ", Имя: " << player.name << ", Очки: " << player.score << endl;
    }
    
    Matrix matrix = {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
    cout << "\nСумма элементов матрицы: " << sumMatrix(matrix) << endl;
    
    cout << "\nКалькулятор:" << endl;
    cout << "5 + 3 = " << calculator(5, 3, add) << endl;
    cout << "5 - 3 = " << calculator(5, 3, subtract) << endl;
    cout << "5 * 3 = " << calculator(5, 3, multiply) << endl;
    
    Vec<int> numbers = {1, 2, 3, 4, 5};
    Vec<string> words = {"Hello", "World"};
    
    cout << "\nVec<int>: ";
    for (int num : numbers) {
        cout << num << " ";
    }
    cout << endl;
    
    cout << "Vec<string>: ";
    for (const string& word : words) {
        cout << word << " ";
    }
    cout << endl;
    
    return 0;
}