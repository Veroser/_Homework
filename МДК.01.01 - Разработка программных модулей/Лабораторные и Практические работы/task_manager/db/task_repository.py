from db.connection import get_connection
import pandas as pd


class TaskRepository:
    def __init__(self):
        self.engine = get_connection()

    def get_all(self):
        try:
            query = "SELECT * FROM app.tasks ORDER BY id"
            df = pd.read_sql(query, self.engine)
            return df
        except Exception as e:
            print(f"Ошибка при получении задач: {e}")
            return pd.DataFrame()

    def get_by_status(self, status):
        try:
            query = "SELECT * FROM app.tasks WHERE status = %s ORDER BY id"
            df = pd.read_sql(query, self.engine, params=(status,))
            return df
        except Exception as e:
            print(f"Ошибка при фильтрации по статусу: {e}")
            return pd.DataFrame()

    def add(self, title, description, status, priority):
        conn = None
        try:
            conn = self.engine.raw_connection()
            cursor = conn.cursor()

            query = """
                INSERT INTO app.tasks (title, description, status, priority)
                VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query, (title, description, status, priority))
            conn.commit()
            print("Задача успешно добавлена")
            return True

        except Exception as e:
            if conn:
                conn.rollback()
            print(f"Ошибка при добавлении задачи: {e}")
            return False
        finally:
            if conn:
                cursor.close()
                conn.close()

    def update_status(self, task_id, new_status):
        conn = None
        try:
            conn = self.engine.raw_connection()
            cursor = conn.cursor()

            check_query = "SELECT id FROM app.tasks WHERE id = %s"
            cursor.execute(check_query, (task_id,))
            if not cursor.fetchone():
                print(f"Задача с ID {task_id} не найдена")
                return False

            query = "UPDATE app.tasks SET status = %s WHERE id = %s"
            cursor.execute(query, (new_status, task_id))
            conn.commit()
            print(f"Статус задачи {task_id} успешно обновлен")
            return True

        except Exception as e:
            if conn:
                conn.rollback()
            print(f"Ошибка при обновлении статуса: {e}")
            return False
        finally:
            if conn:
                cursor.close()
                conn.close()

    def delete(self, task_id):
        conn = None
        try:
            conn = self.engine.raw_connection()
            cursor = conn.cursor()

            check_query = "SELECT id FROM app.tasks WHERE id = %s"
            cursor.execute(check_query, (task_id,))
            if not cursor.fetchone():
                print(f"Задача с ID {task_id} не найдена")
                return False

            query = "DELETE FROM app.tasks WHERE id = %s"
            cursor.execute(query, (task_id,))
            conn.commit()
            print(f"Задача {task_id} успешно удалена")
            return True

        except Exception as e:
            if conn:
                conn.rollback()
            print(f"Ошибка при удалении задачи: {e}")
            return False
        finally:
            if conn:
                cursor.close()
                conn.close()