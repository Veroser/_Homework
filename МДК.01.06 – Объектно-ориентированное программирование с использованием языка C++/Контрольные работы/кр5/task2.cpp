#include <iostream>
#include <vector>
#include <numeric>
#include <algorithm>

int main() {
    std::vector<int> numbers = {4, 7, 2, 9, 5, 1, 8, 3, 6, 10};
    
    int sum = std::accumulate(numbers.begin(), numbers.end(), 0);
    std::cout << "Сумма: " << sum << std::endl;   

    int product = std::accumulate(numbers.begin(), numbers.end(), 1, std::multiplies<int>());
    std::cout << "Произведение: " << product << std::endl;
    
    auto [min_it, max_it] = std::minmax_element(numbers.begin(), numbers.end());
    int difference = *max_it - *min_it;
    std::cout << "Максимум: " << *max_it << ", Минимум: " << *min_it << std::endl;
    std::cout << "Разница: " << difference << std::endl;
    
    return 0;
}