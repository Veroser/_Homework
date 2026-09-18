from flask import Flask, render_template, request

app = Flask(__name__)

days_ru = {
    "mon": "Понедельник",
    "tue": "Вторник",
    "wed": "Среда",
    "thu": "Четверг",
    "fri": "Пятница",
}

schedule = {
    "mon": [
        {"time": "09:00–10:30", "subject": "Математика", "room": "101"},
        {"time": "10:45–12:15", "subject": "Физика", "room": "202"},
        {"time": "12:45–14:15", "subject": "Программирование", "room": "305"},
    ],
    "tue": [
        {"time": "09:00–10:30", "subject": "История", "room": "110"},
        {"time": "10:45–12:15", "subject": "Английский язык", "room": "404"},
        {"time": "12:45–14:15", "subject": "Физкультура", "room": "Спортзал"},
    ],
    "wed": [
        {"time": "09:00–10:30", "subject": "Алгоритмы и структуры данных", "room": "305"},
        {"time": "10:45–12:15", "subject": "Базы данных", "room": "306"},
        {"time": "12:45–14:15", "subject": "Математический анализ", "room": "101"},
    ],
    "thu": [
        {"time": "09:00–10:30", "subject": "Операционные системы", "room": "307"},
        {"time": "10:45–12:15", "subject": "Веб-программирование", "room": "305"},
        {"time": "12:45–14:15", "subject": "Философия", "room": "210"},
    ],
    "fri": [
        {"time": "09:00–10:30", "subject": "Теория вероятностей", "room": "101"},
        {"time": "10:45–12:15", "subject": "Компьютерные сети", "room": "308"},
        {"time": "12:45–14:15", "subject": "Проектная работа", "room": "305"},
    ],
}

@app.route('/')
def index():
    day = request.args.get('day', 'mon')
    lessons = schedule.get(day, [])
    return render_template('index.html', day=day, lessons=lessons, days_ru=days_ru)

if __name__ == '__main__':
    app.run(debug=True)