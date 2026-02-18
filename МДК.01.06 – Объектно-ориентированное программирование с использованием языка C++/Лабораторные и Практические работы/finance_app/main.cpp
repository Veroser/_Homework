#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <limits>
#include <algorithm>

struct Operation {
    int id;
    std::string type;
    std::string category;
    double amount;
    std::string date;
    std::string description;
};

std::vector<Operation> operations;
int nextId = 1;

void clearInputStream() {
    std::cin.clear();
    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
}

void addOperation() {
    Operation op;
    op.id = nextId++;
    
    std::cout << "Выберите тип операции (1 - доход, 2 - расход): ";
    int typeChoice;
    std::cin >> typeChoice;
    clearInputStream();
    
    if (typeChoice == 1) {
        op.type = "доход";
    } else if (typeChoice == 2) {
        op.type = "расход";
    } else {
        std::cout << "Неверный выбор операции!\n";
        return;
    }
    
    std::cout << "Введите категорию: ";
    std::getline(std::cin, op.category);
    
    std::cout << "Введите сумму: ";
    std::cin >> op.amount;
    clearInputStream();
    
    if (op.amount <= 0) {
        std::cout << "Сумма должна быть положительной!\n";
        return;
    }
    
    std::cout << "Введите дату (ГГГГ-ММ-ДД): ";
    std::getline(std::cin, op.date);
    
    std::cout << "Введите описание (необязательно): ";
    std::getline(std::cin, op.description);
    
    operations.push_back(op);
    std::cout << "Запись успешно добавлена!\n";
}

void displayOperations(const std::vector<Operation>& ops) {
    if (ops.empty()) {
        std::cout << "Нет операций для отображения.\n";
        return;
    }
    
    std::cout << "\n";
    std::cout << std::left << std::setw(5) << "ID" 
              << std::setw(10) << "Тип" 
              << std::setw(15) << "Категория" 
              << std::setw(10) << "Сумма" 
              << std::setw(12) << "Дата" 
              << "Описание\n";
    std::cout << std::string(70, '-') << "\n";
    
    for (const auto& op : ops) {
        std::cout << std::left << std::setw(5) << op.id 
                  << std::setw(10) << op.type 
                  << std::setw(15) << op.category 
                  << std::right << std::setw(9) << op.amount << " " 
                  << std::left << std::setw(11) << op.date 
                  << op.description << "\n";
    }
}

void viewOperations() {
    if (operations.empty()) {
        std::cout << "Список операций пуст.\n";
        return;
    }
    
    std::cout << "\n1. Все операции\n";
    std::cout << "2. Только доходы\n";
    std::cout << "3. Только расходы\n";
    std::cout << "4. Операции за период\n";
    std::cout << "Выберите вариант: ";
    
    int choice;
    std::cin >> choice;
    clearInputStream();
    
    std::vector<Operation> filtered;
    
    switch (choice) {
        case 1:
            displayOperations(operations);
            break;
        case 2:
            for (const auto& op : operations) {
                if (op.type == "доход") {
                    filtered.push_back(op);
                }
            }
            displayOperations(filtered);
            break;
        case 3:
            for (const auto& op : operations) {
                if (op.type == "расход") {
                    filtered.push_back(op);
                }
            }
            displayOperations(filtered);
            break;
        case 4: {
            std::string startDate, endDate;
            std::cout << "Введите начальную дату (ГГГГ-ММ-ДД): ";
            std::getline(std::cin, startDate);
            std::cout << "Введите конечную дату (ГГГГ-ММ-ДД): ";
            std::getline(std::cin, endDate);
            
            for (const auto& op : operations) {
                if (op.date >= startDate && op.date <= endDate) {
                    filtered.push_back(op);
                }
            }
            displayOperations(filtered);
            break;
        }
        default:
            std::cout << "Неверный выбор!\n";
    }
}

void deleteOperation() {
    if (operations.empty()) {
        std::cout << "Нет операций для удаления.\n";
        return;
    }
    
    std::cout << "Введите ID операции для удаления: ";
    int id;
    std::cin >> id;
    clearInputStream();
    
    auto it = std::find_if(operations.begin(), operations.end(), 
                           [id](const Operation& op) { return op.id == id; });
    
    if (it != operations.end()) {
        operations.erase(it);
        std::cout << "Операция удалена.\n";
    } else {
        std::cout << "Операция с таким ID не найдена.\n";
    }
}

void showStatistics() {
    if (operations.empty()) {
        std::cout << "Нет данных для статистики.\n";
        return;
    }
    
    double totalIncome = 0;
    double totalExpense = 0;
    
    for (const auto& op : operations) {
        if (op.type == "доход") {
            totalIncome += op.amount;
        } else {
            totalExpense += op.amount;
        }
    }
    
    std::cout << "\nФИНАНСОВАЯ СТАТИСТИКА\n";
    std::cout << "Общий доход: " << totalIncome << "\n";
    std::cout << "Общий расход: " << totalExpense << "\n";
    std::cout << "Текущий баланс: " << totalIncome - totalExpense << "\n";
    
    std::cout << "\nСтатистика по категориям:\n";
    std::vector<std::string> categories;
    std::vector<double> categoryIncome;
    std::vector<double> categoryExpense;
    
    for (const auto& op : operations) {
        auto it = std::find(categories.begin(), categories.end(), op.category);
        if (it == categories.end()) {
            categories.push_back(op.category);
            if (op.type == "доход") {
                categoryIncome.push_back(op.amount);
                categoryExpense.push_back(0);
            } else {
                categoryIncome.push_back(0);
                categoryExpense.push_back(op.amount);
            }
        } else {
            int index = it - categories.begin();
            if (op.type == "доход") {
                categoryIncome[index] += op.amount;
            } else {
                categoryExpense[index] += op.amount;
            }
        }
    }
    
    for (size_t i = 0; i < categories.size(); i++) {
        std::cout << categories[i] << ": доход " << categoryIncome[i] 
                  << ", расход " << categoryExpense[i] << "\n";
    }
}

void saveToFile() {
    std::string filename;
    std::cout << "Введите имя файла для сохранения: ";
    std::getline(std::cin, filename);
    
    std::ofstream file(filename);
    if (!file.is_open()) {
        std::cout << "Ошибка открытия файла для записи!\n";
        return;
    }
    
    file << "ID,Type,Category,Amount,Date,Description\n";
    for (const auto& op : operations) {
        file << op.id << "," << op.type << "," << op.category << "," 
             << op.amount << "," << op.date << "," << op.description << "\n";
    }
    
    file.close();
    std::cout << "Данные сохранены в файл " << filename << "\n";
}

void loadFromFile() {
    std::string filename;
    std::cout << "Введите имя файла для загрузки: ";
    std::getline(std::cin, filename);
    
    std::ifstream file(filename);
    if (!file.is_open()) {
        std::cout << "Файл не найден. Будет создан новый список операций.\n";
        return;
    }
    
    operations.clear();
    nextId = 1;
    
    std::string line;
    std::getline(file, line);
    
    while (std::getline(file, line)) {
        std::stringstream ss(line);
        std::string field;
        Operation op;
        
        std::getline(ss, field, ',');
        op.id = std::stoi(field);
        
        std::getline(ss, op.type, ',');
        std::getline(ss, op.category, ',');
        
        std::getline(ss, field, ',');
        op.amount = std::stod(field);
        
        std::getline(ss, op.date, ',');
        std::getline(ss, op.description, ',');
        
        operations.push_back(op);
        if (op.id >= nextId) {
            nextId = op.id + 1;
        }
    }
    
    file.close();
    std::cout << "Данные загружены из файла " << filename << "\n";
}

int main() {   
    while (true) {
        std::cout << "\nГЛАВНОЕ МЕНЮ:\n";
        std::cout << "1. Добавить операцию\n";
        std::cout << "2. Просмотреть операции\n";
        std::cout << "3. Удалить операцию\n";
        std::cout << "4. Показать статистику\n";
        std::cout << "5. Сохранить в файл\n";
        std::cout << "6. Загрузить из файла\n";
        std::cout << "0. Выход\n";
        std::cout << "Выберите действие: ";
        
        int choice;
        std::cin >> choice;
        clearInputStream();
        
        switch (choice) {
            case 1:
                addOperation();
                break;
            case 2:
                viewOperations();
                break;
            case 3:
                deleteOperation();
                break;
            case 4:
                showStatistics();
                break;
            case 5:
                saveToFile();
                break;
            case 6:
                loadFromFile();
                break;
            case 0:
                std::cout << "Программа завершена.\n";
                return 0;
            default:
                std::cout << "Неверный выбор. Попробуйте снова.\n";
        }
    }
    
    return 0;
}