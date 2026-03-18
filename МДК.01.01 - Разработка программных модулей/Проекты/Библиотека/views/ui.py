# Модуль пользовательского интерфейса
# Presentation Layer (UI)
# Адаптировано для PostgreSQL через psycopg2

from typing import Optional

from business_layer.books_service import BookService

class BookUI:
    # Консольный интерфейс 
    def __init__(self, service: BookService):
        self.service = service
    
    def clear_screen(self):
        # Очистка экрана
        import os
        os.system('cls' if os.name == 'nt' else 'clear')
    
    def show_header(self, title: str):
        # Показать заголовок
        print("\n" + "=" * 50)
        print(f"  {title}")
        print("=" * 50)
    
    def show_main_menu(self) -> str:
        # Показ главного меню
        self.show_header("📚 БИБЛИОТЕКА")
        print("1. 📖 Показать все книги")
        print("2. 🔍 Поиск книг")
        print("3. ➕ Добавить книгу")
        print("4. ✏️ Редактировать книгу")
        print("5. 🗑️ Удалить книгу")
        print("6. 🔄 Изменить доступность")
        print("7. 📊 Статистика")
        print("8. 🚪 Выход")
        print("=" * 50)
        return input("\nВаш выбор: ").strip()
    
    def show_books(self, books: list, title: str = "Список книг"):
        # Просмотр книг
        self.show_header(title)
        
        if not books:
            print("\nСписок пуст")
            return
        
        print(f"\n{'ID':<4} {'Название':<25} {'Автор':<20} {'Год':<4} {'Статус':<10}")
        print("-" * 70)
        
        for book in books:
            status = "Доступна" if book['is_available'] else "Выдана"
            title_display = book['title'][:23] + "..." if len(book['title']) > 25 else book['title']
            author_display = book['author'][:18] + "..." if len(book['author']) > 20 else book['author']
            
            print(f"{book['id']:<4} {title_display:<25} {author_display:<20} {book['year']:<4} {status:<10}")
        
        print(f"\nВсего книг: {len(books)}")
    
    def show_book_details(self, book: dict):
        # Просмотр информации о книгах
        self.show_header("Информация о книге")
        print(f"ID:        {book['id']}")
        print(f"Название:  {book['title']}")
        print(f"Автор:     {book['author']}")
        print(f"Год:       {book['year']}")
        print(f"Жанр:      {book.get('genre') or 'Не указан'}")
        status = "Доступна" if book['is_available'] else "Выдана"
        print(f"Статус:    {status}")
    
    def input_book_data(self, existing_book: dict = None) -> dict:
        # Форма ввода данных
        print(("Редактирование" if existing_book else "Добавление") + " книги")
        print("-" * 50)
        
        if existing_book:
            print(f"(Текущее значение показано в скобках, Enter для сохранения)")
        
        title = input(f"Название: {f'({existing_book["title"]}) ' if existing_book else ''}")
        author = input(f"Автор: {f'({existing_book["author"]}) ' if existing_book else ''}")
        
        year_str = input(f"Год: {f'({existing_book["year"]}) ' if existing_book else ''}")
        genre = input(f"Жанр: {f'({existing_book.get("genre")}) ' if existing_book and existing_book.get('genre') else ''}")
        
        # Обработка значений
        result = {}
        
        if title.strip() or not existing_book:
            result['title'] = title.strip()
        if author.strip() or not existing_book:
            result['author'] = author.strip()
        
        if year_str.strip():
            try:
                result['year'] = int(year_str)
            except ValueError:
                print("Некорректный год, используется старое значение")
        elif not existing_book:
            result['year'] = self.service.CURRENT_YEAR
        
        if genre is not None:  # Пустая строка - валидное значение
            result['genre'] = genre.strip()
        
        return result
    
    def input_book_id(self, prompt: str = "Введите ID книги: ") -> Optional[int]:
        # Ввод ID книги
        try:
            return int(input(prompt))
        except ValueError:
            print("ID должен быть числом!")
            return None
    
    def show_statistics(self):
        """Показать статистику библиотеки"""
        self.show_header("Статистика библиотеки")
        
        stats = self.service.get_statistics()
        
        print(f"\nВсего книг:     {stats['total']}")
        print(f"Доступно:       {stats['total'] - stats['unavailable']}")
        print(f"Выдано:         {stats['unavailable']}")
        
        if stats['total'] > 0:
            percent = (stats['total'] - stats['unavailable']) / stats['total'] * 100
            print(f"📈 Доступность:    {percent:.1f}%")
    
    def show_message(self, message: str, success: bool = True):
        # Кастомное сообщение
        icon = "✅" if success else "❌"
        print(f"\n{icon} {message}")
    
    def confirm(self, message: str) -> bool:
        # Кастомное подтверждение
        response = input(f"{message} (y/n): ").strip().lower()
        return response in ('y', 'yes', 'д', 'да')
    
    def search_books(self):
        """Форма поиска книг"""
        self.show_header("Поиск книг")
        
        print("\nПоиск по:")
        print("1. Названию")
        print("2. Автору")
        print("3. Отмена")
        
        choice = input("\nВаш выбор: ").strip()
        
        if choice == '1':
            query = input("Введите название: ")
            books = self.service.search_book(query, 'title')
            self.show_books(books, f"Результаты поиска по названию: '{query}'")
        elif choice == '2':
            query = input("Введите автора: ")
            books = self.service.search_book(query, 'author')
            self.show_books(books, f"Результаты поиска по автору: '{query}'")
        elif choice == '3':
            return
        else:
            self.show_message("Неверный выбор", False)
    
    def add_book_form(self):
        # Форма добавления книги
        data = self.input_book_data()
        
        success, message = self.service.add_book(
            title=data.get('title', ''),
            author=data.get('author', ''),
            year=data.get('year', self.service.CURRENT_YEAR),
            genre=data.get('genre', '')
        )
        
        self.show_message(message, success)
    
    def edit_book_form(self):
        # Форма редактирования книги
        book_id = self.input_book_id()
        if book_id is None:
            return
        
        book = self.service.db.get_book_by_id(book_id)
        if not book:
            self.show_message(f"Книга с ID {book_id} не найдена", False)
            return
        
        self.show_book_details(dict(book))
        
        if not self.confirm("\nРедактировать эту книгу?"):
            return
        
        data = self.input_book_data(existing_book=dict(book))
        
        success, message = self.service.update_book(
            book_id=book_id,
            title=data.get('title'),
            author=data.get('author'),
            year=data.get('year'),
            genre=data.get('genre')
        )
        
        self.show_message(message, success)
    
    def delete_book_form(self):
        # Форма удаления книги
        book_id = self.input_book_id()
        if book_id is None:
            return
        
        book = self.service.db.get_book_by_id(book_id)
        if not book:
            self.show_message(f"Книга с ID {book_id} не найдена", False)
            return
        
        self.show_book_details(dict(book))
        
        if not self.confirm("\nВы уверены, что хотите удалить эту книгу?"):
            return
        
        success, message = self.service.delete_book(book_id)
        self.show_message(message, success)
    
    def toggle_availability_form(self):
        # Форма смены доступа
        book_id = self.input_book_id()
        if book_id is None:
            return
        
        success, message = self.service.toggle_book_avialable(book_id)
        self.show_message(message, success)
    
    def run(self):
        # Запуск приложения
        while True:
            choice = self.show_main_menu()
            
            if choice == '1':
                books = self.service.get_all_books()
                self.show_books(books)
            
            elif choice == '2':
                self.search_books()
            
            elif choice == '3':
                self.add_book_form()
            
            elif choice == '4':
                self.edit_book_form()
            
            elif choice == '5':
                self.delete_book_form()
            
            elif choice == '6':
                self.toggle_availability_form()
            
            elif choice == '7':
                self.show_statistics()
            
            elif choice == '8':
                self.show_header("Выход")
                print("\n👋 До свидания! Приходите ещё!")
                break
            
            else:
                self.show_message("Неверный выбор, попробуйте снова", False)
            
            input("\nНажмите Enter для продолжения...")
