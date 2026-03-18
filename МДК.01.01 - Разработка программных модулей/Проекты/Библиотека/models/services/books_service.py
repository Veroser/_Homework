'''
Модуль для бизнес-логики
Bussines Logic Layer(BLL)
Адаптировано под PostgreSQL через psycopg2
'''

from data_layer.db import Database
from typing import List, Dict, Tuple, Any

class BookService:
    # Сервис для работы с книгами

    CURRENT_YEAR = 2026
    
    def __init__(self, db: Database):
        # Инизиацилизация сервиса
        self.db = db
    
    def get_all_books(self) -> List[Dict]:
        # Получить все книги
        return self.db.get_all_books()       
    
    def get_available_books(self) -> List[Dict]:
        # Получение книг в наличии
        return self.db.get_available_books()
    
    def search_book(self, query, search_by):
        # Поиск книг
        if not query or not query.strip():
            return []
        
        if search_by == 'author':
            return self.db.get_book_by_author(query)
        
        elif search_by == 'title':
            return self.db.get_book_by_title(query)
        
        else:
            return self.db.get_all_book()
        
    def add_book(self, title, author, year, genre: str = "") -> Tuple[bool, str]:
        # Добавление книги

        # Проверка 1: Название пустое
        if not title or not title.strip():
            return False, 'Название не может быть пустым'
        
        # Проверка 2: Название не слишком длинное
        if len(title.strip()) > 200:
            return False, 'Название слишком длинное(Максимум 200 символов)'
        
        # Проверка 3: Пустой автор
        if not author or not author.strip():
            return False, 'Автор не может быть пустым'
        
        # Проверка 4: Автор не слишком длинный
        if len(author.strip()) > 100:
            return False, 'Имя слишком длинное(Максимум 100 символов)'
        
        # Проверка 5: Год корректный
        if not isinstance(year, int):
            return False, 'Год должен быть целым числом'
        
        if year < 0 or year > self.CURRENT_YEAR:
            return False, 'Год должен быть целым числом'
        
        # Проверка 6: Жанр (необязательно, но если указан - не слишком длинный)
        if genre and len(genre.strip()) > 50:
            return False, 'Жанр слишком длинный (макимум 50 символов)'
        
        try:
            book_id = self.db.add_book(
                title=title.strip(),
                author=author.strip(),
                year=year,
                genre=genre.strip() if genre else "", is_available=True
            )
            return True, f'Книга добавлена с ID: {book_id}'
        except Exception as e:
            return False, f'Ошибка при добавлении: {str(e)}'
        
    def update_book(self, book_id: int, title: str = None, author : str = None, year: int = None,
                    genre: str = None) -> Tuple[bool, str]:
        #Обновить информацию о книге

        # Проверка ID
        if not isinstance(book_id, int) or book_id <= 0:
            return False, "Неккоректный ID книги"
        
        # Проверка существования книги
        book = self.db.get_book_by_id(book_id)
        if not book:
            return False, f'Книга с ID {book_id} не найдена'
        
        # Если переданы новые значения, проверяем их
        if title is not None:
            if not title.strip():
                return False, 'Название не может быть пустым'
            if len(title.strip()) > 200:
                return False, 'Название слишком длинное'
            
        if author is not None:
            if not author.strip():
                return False, 'Название не может быть пустым'
            if len(author.strip()) > 100:
                return False, 'Название слишком длинное'
            
        if year is not None:
            if not isinstance(year, int):
                return False, 'Год должен быть целым числом'
            if year < 0 or year > self.CURRENT_YEAR:
                return False, f'Год должен быть от 0 до {self.CURRENT_YEAR}'
        
        if genre is not None and len(genre.strip()) > 50:
            return False, 'Жанр слишком длинный'
        
        try:
            self.db.update_book(
                book_id=book_id,
                title=title,
                author=author,
                year=year,
                genre=genre
            )
            return True, 'Книга обновление'
        except Exception as e:
            return False, f'Ошибка при обновлении: {str(e)}'

    def delete_book(self, book_id) -> Tuple[bool, str]:
        if not isinstance(book_id, int) or book_id <= 0:
            return False, 'Некорректный ID книги'
        
        # Проверка существования книги
        book = self.db.get_book_by_id(book_id)
        if not book:
            return False, f'Книга ID {book_id} не найдена'
        
        try:
            self.db.delete_book(book_id)
            return True, "Книга удалена"
        except Exception as e:
            return False, f'Ошибка при удалении: {str(e)}'
        
    def toggle_book_avialable(self, book_id: int) -> Tuple[bool, str]:
        # Проверка статуса доступности книги
        if not isinstance(book_id, int) or book_id <= 0:
            return False, "Некорректный ID книги"
        
        book = self.db.get_book_by_id(book_id)
        if not book:
            return False, f'Книга с {book_id} не найдена'
        
        try:
            new_status = self.db.toggle_availability(book_id)
            status_test = 'доступна' if new_status else 'выдана'
            return True, f'Книга {status_test}'
        except Exception as e:
            return False, f'Ошибка: {str(e)}'
        
    def get_statistics(self) -> Dict[str, int]:
        return self.db.get_books_count()