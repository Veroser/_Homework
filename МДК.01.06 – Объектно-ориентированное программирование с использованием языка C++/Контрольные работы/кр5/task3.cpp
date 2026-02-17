#include <iostream>
#include <fstream>
#include <vector>
#include <numeric>

int main() {
    std::ofstream outFile("numbers.txt");
    if (!outFile) {
        std::cerr << "Ошибка создания файла!" << std::endl;
        return 1;
    }
    
    int numbers_to_write[] = {10, 20, 30, 40, 50};
    for (int num : numbers_to_write) {
        outFile << num << " ";
    }
    outFile.close();
    
    std::ifstream inFile("numbers.txt");
    if (!inFile) {
        std::cerr << "Ошибка открытия файла!" << std::endl;
        return 1;
    }
    
    std::vector<int> numbers;
    int num;
    while (inFile >> num) {
        numbers.push_back(num);
    }
    inFile.close();
    
    if (!numbers.empty()) {
        double average = std::accumulate(numbers.begin(), numbers.end(), 0.0) / numbers.size();
        std::cout << "Числа из файла: ";
        for (int n : numbers) std::cout << n << " ";
        std::cout << "\nСреднее значение: " << average << std::endl;
    }
    
    return 0;
}