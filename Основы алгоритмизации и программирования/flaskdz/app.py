import json
import os
from datetime import datetime

from flask import Flask, render_template, redirect, url_for, session, flash
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, SelectField, SubmitField
from wtforms.validators import DataRequired, Email, Length, EqualTo
from werkzeug.security import generate_password_hash, check_password_hash

BASE_DIR = os.path.abspath(os.path.dirname(__file__))
USERS_FILE = os.path.join(BASE_DIR, "users.json")

app = Flask(__name__)
app.config["SECRET_KEY"] = os.environ.get("SECRET_KEY", os.urandom(24).hex())

GROUPS = ["ИС-21", "ИС-22", "ПР-21", "ПР-22", "БД-21", "БД-22"]

SCHEDULE = {
    "ИС-21": [
        {"day": "Понедельник", "lessons": [
            {"time": "09:00–10:30", "subject": "Основы алгоритмизации", "teacher": "Иванов И.И.", "room": "204"},
            {"time": "10:45–12:15", "subject": "Базы данных", "teacher": "Петрова А.С.", "room": "310"},
            {"time": "12:45–14:15", "subject": "Веб-разработка", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Математика", "teacher": "Козлова Н.М.", "room": "401"},
            {"time": "10:45–12:15", "subject": "Английский язык", "teacher": "Смирнова Е.А.", "room": "215"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "Python-программирование", "teacher": "Сидоров П.В.", "room": "112"},
            {"time": "10:45–12:15", "subject": "Физика", "teacher": "Орлов Д.К.", "room": "303"},
            {"time": "12:45–14:15", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Базы данных", "teacher": "Петрова А.С.", "room": "310"},
            {"time": "10:45–12:15", "subject": "Основы алгоритмизации", "teacher": "Иванов И.И.", "room": "204"},
        ]},
        {"day": "Пятница", "lessons": [
            {"time": "09:00–10:30", "subject": "Веб-разработка", "teacher": "Сидоров П.В.", "room": "112"},
            {"time": "10:45–12:15", "subject": "История", "teacher": "Фёдорова Л.П.", "room": "208"},
        ]},
    ],
    "ИС-22": [
        {"day": "Понедельник", "lessons": [
            {"time": "10:45–12:15", "subject": "Основы алгоритмизации", "teacher": "Иванов И.И.", "room": "204"},
            {"time": "12:45–14:15", "subject": "Математика", "teacher": "Козлова Н.М.", "room": "401"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Веб-разработка", "teacher": "Сидоров П.В.", "room": "112"},
            {"time": "10:45–12:15", "subject": "Базы данных", "teacher": "Петрова А.С.", "room": "310"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "Английский язык", "teacher": "Смирнова Е.А.", "room": "215"},
            {"time": "10:45–12:15", "subject": "Python-программирование", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Физика", "teacher": "Орлов Д.К.", "room": "303"},
            {"time": "10:45–12:15", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
        {"day": "Пятница", "lessons": [
            {"time": "09:00–10:30", "subject": "История", "teacher": "Фёдорова Л.П.", "room": "208"},
        ]},
    ],
    "ПР-21": [
        {"day": "Понедельник", "lessons": [
            {"time": "09:00–10:30", "subject": "Правоведение", "teacher": "Никитин В.С.", "room": "501"},
            {"time": "10:45–12:15", "subject": "Документоведение", "teacher": "Белова О.Н.", "room": "402"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Русский язык", "teacher": "Гусева Т.И.", "room": "207"},
            {"time": "10:45–12:15", "subject": "История", "teacher": "Фёдорова Л.П.", "room": "208"},
            {"time": "12:45–14:15", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "Документоведение", "teacher": "Белова О.Н.", "room": "402"},
            {"time": "10:45–12:15", "subject": "Английский язык", "teacher": "Смирнова Е.А.", "room": "215"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Правоведение", "teacher": "Никитин В.С.", "room": "501"},
        ]},
        {"day": "Пятница", "lessons": [
            {"time": "09:00–10:30", "subject": "Русский язык", "teacher": "Гусева Т.И.", "room": "207"},
            {"time": "10:45–12:15", "subject": "Информатика", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
    ],
    "ПР-22": [
        {"day": "Понедельник", "lessons": [
            {"time": "10:45–12:15", "subject": "Русский язык", "teacher": "Гусева Т.И.", "room": "207"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Правоведение", "teacher": "Никитин В.С.", "room": "501"},
            {"time": "10:45–12:15", "subject": "Документоведение", "teacher": "Белова О.Н.", "room": "402"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "История", "teacher": "Фёдорова Л.П.", "room": "208"},
            {"time": "10:45–12:15", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Английский язык", "teacher": "Смирнова Е.А.", "room": "215"},
            {"time": "10:45–12:15", "subject": "Информатика", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
        {"day": "Пятница", "lessons": []},
    ],
    "БД-21": [
        {"day": "Понедельник", "lessons": [
            {"time": "09:00–10:30", "subject": "Бухгалтерский учёт", "teacher": "Романова И.В.", "room": "505"},
            {"time": "10:45–12:15", "subject": "Экономика", "teacher": "Тихонов С.П.", "room": "508"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Математика", "teacher": "Козлова Н.М.", "room": "401"},
            {"time": "10:45–12:15", "subject": "Информатика", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "Бухгалтерский учёт", "teacher": "Романова И.В.", "room": "505"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Русский язык", "teacher": "Гусева Т.И.", "room": "207"},
            {"time": "10:45–12:15", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
        {"day": "Пятница", "lessons": [
            {"time": "09:00–10:30", "subject": "Экономика", "teacher": "Тихонов С.П.", "room": "508"},
        ]},
    ],
    "БД-22": [
        {"day": "Понедельник", "lessons": [
            {"time": "09:00–10:30", "subject": "Экономика", "teacher": "Тихонов С.П.", "room": "508"},
        ]},
        {"day": "Вторник", "lessons": [
            {"time": "09:00–10:30", "subject": "Бухгалтерский учёт", "teacher": "Романова И.В.", "room": "505"},
            {"time": "10:45–12:15", "subject": "Математика", "teacher": "Козлова Н.М.", "room": "401"},
        ]},
        {"day": "Среда", "lessons": [
            {"time": "09:00–10:30", "subject": "Английский язык", "teacher": "Смирнова Е.А.", "room": "215"},
            {"time": "10:45–12:15", "subject": "Информатика", "teacher": "Сидоров П.В.", "room": "112"},
        ]},
        {"day": "Четверг", "lessons": [
            {"time": "09:00–10:30", "subject": "Русский язык", "teacher": "Гусева Т.И.", "room": "207"},
        ]},
        {"day": "Пятница", "lessons": [
            {"time": "09:00–10:30", "subject": "Физкультура", "teacher": "Морозов А.А.", "room": "Спортзал"},
        ]},
    ],
}


def load_users():
    if not os.path.exists(USERS_FILE):
        return {}
    with open(USERS_FILE, "r", encoding="utf-8") as f:
        try:
            return json.load(f)
        except json.JSONDecodeError:
            return {}


def save_users(users):
    with open(USERS_FILE, "w", encoding="utf-8") as f:
        json.dump(users, f, ensure_ascii=False, indent=2)


class RegisterForm(FlaskForm):
    username = StringField(
        "Имя пользователя",
        validators=[DataRequired(), Length(min=3, max=40)],
    )
    email = StringField("Email", validators=[DataRequired(), Email()])
    group = SelectField("Группа", choices=[(g, g) for g in GROUPS], validators=[DataRequired()])
    password = PasswordField("Пароль", validators=[DataRequired(), Length(min=6, max=128)])
    confirm = PasswordField(
        "Повторите пароль",
        validators=[DataRequired(), EqualTo("password", message="Пароли не совпадают")],
    )
    submit = SubmitField("Зарегистрироваться")


class LoginForm(FlaskForm):
    username = StringField("Имя пользователя", validators=[DataRequired()])
    password = PasswordField("Пароль", validators=[DataRequired()])
    submit = SubmitField("Войти")


@app.context_processor
def inject_current_user():
    username = session.get("username")
    if not username:
        return {"current_user": None}
    user = load_users().get(username)
    return {"current_user": user}


@app.route("/")
def index():
    if session.get("username"):
        return redirect(url_for("schedule"))
    return redirect(url_for("login"))


@app.route("/register", methods=["GET", "POST"])
def register():
    if session.get("username"):
        return redirect(url_for("schedule"))

    form = RegisterForm()
    if form.validate_on_submit():
        users = load_users()
        username = form.username.data.strip()

        if username in users:
            flash("Пользователь с таким именем уже существует", "error")
            return render_template("register.html", form=form)

        if any(u["email"].lower() == form.email.data.lower() for u in users.values()):
            flash("Этот email уже используется", "error")
            return render_template("register.html", form=form)

        users[username] = {
            "username": username,
            "email": form.email.data,
            "group": form.group.data,
            "password_hash": generate_password_hash(form.password.data),
            "created_at": datetime.utcnow().isoformat(),
        }
        save_users(users)

        session["username"] = username
        flash("Регистрация успешна. Добро пожаловать!", "success")
        return redirect(url_for("schedule"))

    return render_template("register.html", form=form)


@app.route("/login", methods=["GET", "POST"])
def login():
    if session.get("username"):
        return redirect(url_for("schedule"))

    form = LoginForm()
    if form.validate_on_submit():
        users = load_users()
        username = form.username.data.strip()
        user = users.get(username)

        if not user or not check_password_hash(user["password_hash"], form.password.data):
            flash("Неверное имя пользователя или пароль", "error")
            return render_template("login.html", form=form)

        session["username"] = username
        flash(f"С возвращением, {username}!", "success")
        return redirect(url_for("schedule"))

    return render_template("login.html", form=form)


@app.route("/schedule")
def schedule():
    username = session.get("username")
    if not username:
        flash("Требуется вход в систему", "error")
        return redirect(url_for("login"))

    user = load_users().get(username)
    if not user:
        session.clear()
        flash("Пользователь не найден, войдите снова", "error")
        return redirect(url_for("login"))

    group = user["group"]
    group_schedule = SCHEDULE.get(group, [])
    return render_template(
        "schedule.html",
        user=user,
        group=group,
        schedule=group_schedule,
    )


@app.route("/logout", methods=["POST"])
def logout():
    session.clear()
    flash("Вы вышли из системы", "success")
    return redirect(url_for("login"))


if __name__ == "__main__":
    app.run(debug=True)