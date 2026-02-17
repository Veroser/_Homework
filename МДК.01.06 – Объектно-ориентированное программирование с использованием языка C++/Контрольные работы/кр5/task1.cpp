#include <iostream>
#include <string>
#include <vector>
#include <sstream>
#include <algorithm>
#include <set>

int main() {
    std::string input = "apple,banana,orange,apple,kiwi";
    std::vector<std::string> words;
    
    std::stringstream ss(input);
    std::string word;
    while (std::getline(ss, word, ',')) {
        words.push_back(word);
    }
        std::set<std::string> unique_words(words.begin(), words.end());
    words.assign(unique_words.begin(), unique_words.end());
    

    std::sort(words.begin(), words.end());
    

    std::cout << "Результат: ";
    for (size_t i = 0; i < words.size(); ++i) {
        std::cout << words[i];
        if (i < words.size() - 1) std::cout << ", ";
    }
    std::cout << std::endl;
    
    return 0;
}