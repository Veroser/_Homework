from db.task_repository import TaskRepository
import pandas as pd


class TaskManager:
    def __init__(self):
        self.repo = TaskRepository()
        self.tasks_df = self.repo.get_all()

    def refresh_tasks(self):
        self.tasks_df = self.repo.get_all()

    def display_tasks(self, df=None):
        if df is None:
            df = self.tasks_df

        if df.empty:
            print("\nСписок задач пуст")
            return

        print("\nID  Статус          Приоритет   Название                      Дата создания")
        for _, task in df.iterrows():
            status_icon = {
                'todo': '',
                'in_progress': '',
                'done': ''
            }.get(task['status'], '')

            created_date = task['created_at'].strftime('%d.%m.%Y') if pd.notna(task['created_at']) else 'N/A'
            print(
                f"{task['id']:<3} {status_icon}{task['status']:<14} {task['priority']:<10} {task['title'][:28]:<28} {created_date}")

    def show_menu(self):
        print("\nTASK MANAGER - Менеджер задач")
        print("1   Показать все задачи")
        print("2   Добавить задачу")
        print("3   Обновить статус")
        print("4   Удалить задачу")
        print("5   Фильтр по статусу")
        print("0   Выход")

    def add_task(self):
        print("\nДобавление новой задачи")

        title = input("Название задачи: ").strip()
        if not title:
            print("Название задачи обязательно!")
            return

        description = input("Описание (необязательно): ").strip()

        print("\nСтатусы: todo, in_progress, done")
        status = input("Статус (по умолчанию todo): ").strip() or "todo"

        try:
            priority = int(input("Приоритет (1-5): ").strip() or "3")
            if priority < 1 or priority > 5:
                print("Приоритет должен быть от 1 до 5")
                return
        except ValueError:
            print("Приоритет должен быть числом")
            return

        if self.repo.add(title, description, status, priority):
            self.refresh_tasks()

    def update_task_status(self):
        if self.tasks_df.empty:
            print("\nНет задач для обновления")
            return

        print("\nОбновление статуса задачи")

        try:
            task_id = int(input("ID задачи: "))
        except ValueError:
            print("ID должен быть числом")
            return

        task = self.tasks_df[self.tasks_df['id'] == task_id]
        if task.empty:
            print(f"Задача с ID {task_id} не найдена")
            return

        print(f"\nТекущая задача: {task.iloc[0]['title']}")
        print(f"Текущий статус: {task.iloc[0]['status']}")
        print("\nНовый статус (todo/in_progress/done): ", end="")
        new_status = input().strip().lower()

        if new_status not in ['todo', 'in_progress', 'done']:
            print("Некорректный статус")
            return

        if self.repo.update_status(task_id, new_status):
            self.refresh_tasks()

    def delete_task(self):
        if self.tasks_df.empty:
            print("\nНет задач для удаления")
            return

        print("\nУдаление задачи")

        try:
            task_id = int(input("ID задачи для удаления: "))
        except ValueError:
            print("ID должен быть числом")
            return

        task = self.tasks_df[self.tasks_df['id'] == task_id]
        if task.empty:
            print(f"Задача с ID {task_id} не найдена")
            return

        print(f"\nЗадача: {task.iloc[0]['title']}")
        confirm = input("Подтвердите удаление (y/n): ").strip().lower()

        if confirm == 'y':
            if self.repo.delete(task_id):
                self.refresh_tasks()
        else:
            print("Удаление отменено")

    def filter_by_status(self):
        print("\nФильтр по статусу")
        print("Доступные статусы: todo, in_progress, done")

        status = input("Статус: ").strip().lower()

        if status not in ['todo', 'in_progress', 'done']:
            print("Некорректный статус")
            return

        filtered_df = self.repo.get_by_status(status)
        if not filtered_df.empty:
            print(f"\nЗадачи со статусом '{status}':")
            self.display_tasks(filtered_df)
        else:
            print(f"\nНет задач со статусом '{status}'")

    def run(self):
        print("Запуск Task Manager...")

        while True:
            self.show_menu()
            choice = input("Выберите действие: ").strip()

            if choice == '1':
                self.display_tasks()

            elif choice == '2':
                self.add_task()

            elif choice == '3':
                self.update_task_status()

            elif choice == '4':
                self.delete_task()

            elif choice == '5':
                self.filter_by_status()

            elif choice == '0':
                print("\nДо свидания!")
                break

            else:
                print("\nНекорректный выбор. Попробуйте снова.")

            input("\nНажмите Enter для продолжения...")


if __name__ == "__main__":
    app = TaskManager()
    app.run()