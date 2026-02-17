#include <iostream>
#include <string>
#include <sstream>
#include <stdexcept>

int main() {
    std::string input;
    int number;
    
    std::cout << "Введите число: ";
    std::getline(std::cin, input);
    
    try {
        std::stringstream ss(input);
        if (!(ss >> number)) {
            throw std::invalid_argument("Введено не число!");
        }
        
        if (number < 0) {
            throw std::runtime_error("Число отрицательное!");
        }
        
        std::cout << "Вы ввели корректное число: " << number << std::endl;
    }
    catch (const std::invalid_argument& e) {
        std::cerr << "Ошибка: " << e.what() << std::endl;
    }
    catch (const std::runtime_error& e) {
        std::cerr << "Ошибка: " << e.what() << std::endl;
    }
    catch (...) {
        std::cerr << "Неизвестная ошибка!" << std::endl;
    }
    
    return 0;
}