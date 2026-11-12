import os
from datetime import datetime, timedelta

from flask import Flask, render_template, redirect, url_for, session, flash
from flask_session import Session
from flask_sqlalchemy import SQLAlchemy
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField
from wtforms.validators import DataRequired, Email, Length, EqualTo, Regexp
from werkzeug.security import generate_password_hash, check_password_hash


BASE_DIR = os.path.abspath(os.path.dirname(__file__))

app = Flask(__name__)
app.config["SECRET_KEY"] = os.environ.get("SECRET_KEY", os.urandom(24).hex())

app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///" + os.path.join(BASE_DIR, "users.db")
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

app.config["SESSION_TYPE"] = "sqlalchemy"
app.config["SESSION_SQLALCHEMY"] = None
app.config["SESSION_SQLALCHEMY_TABLE"] = "sessions"
app.config["SESSION_PERMANENT"] = True
app.config["SESSION_USE_SIGNER"] = True
app.config["SESSION_KEY_PREFIX"] = "session:"

db = SQLAlchemy(app)

app.config["SESSION_SQLALCHEMY"] = db
Session(app)


class User(db.Model):
    __tablename__ = "users"

    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(64), unique=True, nullable=False, index=True)
    email = db.Column(db.String(120), unique=True, nullable=False, index=True)
    password_hash = db.Column(db.String(255), nullable=False)
    created_at = db.Column(db.DateTime, default=datetime.utcnow, nullable=False)

    def set_password(self, password):
        self.password_hash = generate_password_hash(password)

    def check_password(self, password):
        return check_password_hash(self.password_hash, password)


class RegisterForm(FlaskForm):
    username = StringField(
        "Имя пользователя",
        validators=[
            DataRequired(),
            Length(min=3, max=64),
            Regexp(r"^[A-Za-z0-9_]+$", message="Только латиница, цифры и _"),
        ],
    )
    email = StringField("Email", validators=[DataRequired(), Email(), Length(max=120)])
    password = PasswordField(
        "Пароль", validators=[DataRequired(), Length(min=6, max=128)]
    )
    confirm = PasswordField(
        "Повторите пароль",
        validators=[DataRequired(), EqualTo("password", message="Пароли не совпадают")],
    )
    submit = SubmitField("Зарегистрироваться")


class LoginForm(FlaskForm):
    username = StringField("Имя пользователя", validators=[DataRequired()])
    password = PasswordField("Пароль", validators=[DataRequired()])
    remember = BooleanField("Запомнить меня")
    submit = SubmitField("Войти")


def current_user():
    uid = session.get("user_id")
    if not uid:
        return None
    return db.session.get(User, uid)


@app.route("/")
def index():
    if current_user():
        return redirect(url_for("profile"))
    return redirect(url_for("login"))


@app.route("/register", methods=["GET", "POST"])
def register():
    if current_user():
        return redirect(url_for("profile"))

    form = RegisterForm()
    if form.validate_on_submit():
        if User.query.filter_by(username=form.username.data).first():
            flash("Пользователь с таким именем уже существует", "error")
            return render_template("register.html", form=form)
        if User.query.filter_by(email=form.email.data).first():
            flash("Пользователь с таким email уже существует", "error")
            return render_template("register.html", form=form)

        user = User(username=form.username.data, email=form.email.data)
        user.set_password(form.password.data)
        db.session.add(user)
        db.session.commit()

        session.clear()
        session["user_id"] = user.id
        session.permanent = True
        app.permanent_session_lifetime = timedelta(days=7)
        flash("Регистрация успешна", "success")
        return redirect(url_for("profile"))

    return render_template("register.html", form=form)


@app.route("/login", methods=["GET", "POST"])
def login():
    if current_user():
        return redirect(url_for("profile"))

    form = LoginForm()
    if form.validate_on_submit():
        user = User.query.filter_by(username=form.username.data).first()
        if user is None or not user.check_password(form.password.data):
            flash("Неверное имя пользователя или пароль", "error")
            return render_template("login.html", form=form)

        session.clear()
        session["user_id"] = user.id
        session.permanent = True
        if form.remember.data:
            app.permanent_session_lifetime = timedelta(days=30)
        else:
            app.permanent_session_lifetime = timedelta(days=7)
        return redirect(url_for("profile"))

    return render_template("login.html", form=form)


@app.route("/profile")
def profile():
    user = current_user()
    if user is None:
        flash("Требуется вход в систему", "error")
        return redirect(url_for("login"))
    return render_template("profile.html", user=user)


@app.route("/logout", methods=["POST"])
def logout():
    session.clear()
    flash("Вы вышли из системы", "success")
    return redirect(url_for("login"))


with app.app_context():
    db.create_all()


if __name__ == "__main__":
    app.run(debug=True)