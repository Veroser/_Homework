/*Создать класс DynamicBuffer:

хранит динамический массив

выделяет память в конструкторе

освобождает в деструкторе

содержит метод заполнения массива*/

#include <iostream>
using namespace std;

class Book{
private:
    string title;
    int pages;

public:
    Book(int amountPages){
        title = "Война и мир";
        pages = amountPages;
    }

    void getbook(){
        cout << "Название книги: " << title << endl;
        cout << "Количество страниц: " << pages << endl;
    }
};

class DynamicBuffer{
private:
    int* buffer;
    int size;

public:
    DynamicBuffer(int s) : size(s) {
        buffer = new int[size];
    }

    ~DynamicBuffer() {
        delete[] buffer;
    }

    void set(){
        for (int i = 0; i <size; i++){
            buffer[i] = i;
        }
    }

};

int main(){ 
    
}


