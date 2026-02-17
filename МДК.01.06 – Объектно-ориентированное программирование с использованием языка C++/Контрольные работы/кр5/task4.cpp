#include <iostream>
#include <chrono>

int main() {
    const int ITERATIONS = 10000000;
    
    auto start1 = std::chrono::high_resolution_clock::now();
    volatile int sum1 = 0;
    for (int i = 0; i < ITERATIONS; ++i) {
        sum1 += i;
    }
    auto end1 = std::chrono::high_resolution_clock::now();
    auto duration1 = std::chrono::duration_cast<std::chrono::milliseconds>(end1 - start1);
    

    auto start2 = std::chrono::high_resolution_clock::now();
    volatile long long sum2 = 0;
    for (int i = 0; i < ITERATIONS; ++i) {
        sum2 += static_cast<long long>(i) * i;
    }
    auto end2 = std::chrono::high_resolution_clock::now();
    auto duration2 = std::chrono::duration_cast<std::chrono::milliseconds>(end2 - start2);
    
    std::cout << "Время выполнения первого цикла: " << duration1.count() << " мс" << std::endl;
    std::cout << "Время выполнения второго цикла: " << duration2.count() << " мс" << std::endl;
    std::cout << "Разница: " << std::abs(static_cast<long long>(duration2.count() - duration1.count())) << " мс" << std::endl;
    
    return 0;
}