#include <iostream>
#include <windows.h>
using namespace std;
/*Задание 1:

Создать динамический массив и заполнить числами от 1 до N
Задание 2:
Создать функцию, создающую динамический массив и найти сумму
элементов данного массива
Задание 3:

Создать динамический массив;

Удалить элемент массива;

Создать новый массив размером N−1;

Скопировать туда всё, кроме удаляемого элемента.*/

int* task1(int n) {
    int *arr = new int[n];
    for (int i = 0; i < n; i++){
        arr[i] = i + 1;
    }

    return arr;
}
int* task2(int n, int& sum){ 
    int *arr = new int[n];
    sum = 0;
    for (int i = 0; i < n; i++){
        arr[i] = i + 1;
        sum += arr[i];
    }
    return 0;
}

int* task3(int n, int& newSize){
    
    int *arr = new int[n];
    for (int i = 0; i < n; i++){
        arr[i] = i + 1;
    }
    newSize = n - 1;
    int* newArr = new int[newSize];
    
    int j = 0;
    for (int i = 0; i < n; i++) {
        if (i != 0) {
            newArr[j] = arr[i];
            j++;
        }
    }
    
    return newArr;
}

int main() {
    SetConsoleOutputCP(CP_UTF8);
    int n;
    int sum;
    cout << "Введите число: ";
    cin >> n;
    int* arr = task1(n);
    cout << "Массив с числами от 1 до " << n << ": ";
    for (int i = 0; i < n; i++) {
        cout << arr[i] << " ";
    }
    cout << endl;
    delete[] arr;

    int* arr2 = task2(n, sum);
    cout << "Сумма элементов массива: " << sum << endl;
    delete[] arr2;

    int newSize;
    int* arr3 = task3(n, newSize);
    cout << "Массив с удалённым элементом под индексом 0: ";
    for (int i = 0; i < newSize; i++) {
        std::cout << arr3[i] << " ";
    }

    delete[] arr3;

    return 0;
}