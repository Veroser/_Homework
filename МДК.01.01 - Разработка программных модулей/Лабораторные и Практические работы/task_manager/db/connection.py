from sqlalchemy import create_engine

def get_connection():
    try:
        engine = create_engine(
            'postgresql+psycopg2://postgres:password@localhost:5432/task_manager_db',
            echo=False
        )
        with engine.connect() as conn:
            print("Подключение к базе данных успешно установлено")
        return engine
    except Exception as e:
        print(f"Ошибка подключения к базе данных: {e}")
        return None