'''
Библиотека книг - простое приложение для управления книгами с целью демонстрации слоистой архитектуры

Структура проекта:
|_data_layer/           - Уровень доступа к данным(DAL)
|_business_layer/       - Уровень бизнес-логики(BLL)
|_presentation_layer/   - Уровень интерфейса(UI)
|_main.py               - Точка входа

Предварительные требования:
1. Установлен PostgreSQL
2. Наличие базы данных library/library_db
3. Установлен psycopg2: pip install psycopg2-binary
'''

import sys
from pathlib import Path

# Добавление корень проекта в путь импортов
project_root = Path(__file__).parent
sys.path.insert(0, str(project_root))

DB_CONFIG = {
    'host': 'localhost',
    'port': 5432,
    'database': 'library_db',
    'user': 'postgres',
    'password': 'password'
}

from data_layer.db import Database
from business_layer.books_service import BookService
from presentation_layer.ui import BookUI

def main():  
    # Точка входа в программу
    db = Database(DB_CONFIG)
    
    try:
        db.connect()
        print("Подключение успешно!")
        
        # Инициализация структуры БД
        db.init_database()
        print("База данных инициализирована")
        
        # Добавление тестовых данных, если база пуста
        books = db.get_all_books()
        if not books:
            print("\nДобавление тестовых книг...")
            db.seed_sample_data()
        
        # 2. Business Layer - бизнес-логика
        service = BookService(db)
        
        # 3. Presentation layer - интерфейс
        ui = BookUI(service)
        
        # Запуск приложения
        ui.run()
        
    except Exception as e:
        print(f"\nОшибка: {e}")
        print("\nВозможные причины:")
        print("  1. PostgreSQL не запущен")
        print("  2. База данных 'library_db' не существует")
        print("  3. Неверные учётные данные в DB_CONFIG")
        print("  4. Не установлен psycopg2-binary")
        print("\nДля создания базы данных выполните:")
        print("  psql -U postgres -c 'CREATE DATABASE library_db;'")
        print("\nИли загрузите дамп:")
        print("  psql -U postgres -d library_db -f db.sql")
        raise
    finally:
        # Закрытие соединения
        db.close()


if __name__ == '__main__':
    main()