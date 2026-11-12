import os
from flask import Flask, render_template, redirect, url_for
from flask_wtf import FlaskForm
from wtforms import FloatField, IntegerField, SubmitField
from wtforms.validators import DataRequired, NumberRange

app = Flask(__name__)
app.config["SECRET_KEY"] = os.urandom(24)


class BMIForm(FlaskForm):
    weight = FloatField(
        "Вес (кг)",
        validators=[DataRequired(), NumberRange(min=20, max=300)],
    )
    height = IntegerField(
        "Рост (см)",
        validators=[DataRequired(), NumberRange(min=100, max=250)],
    )
    age = IntegerField(
        "Возраст",
        validators=[DataRequired(), NumberRange(min=1, max=120)],
    )
    submit = SubmitField("Рассчитать")


def get_category(bmi):
    if bmi < 18.5:
        return "Недовес"
    if bmi < 25:
        return "Норма"
    if bmi < 30:
        return "Избыток"
    return "Ожирение"


@app.route("/", methods=["GET", "POST"])
def index():
    form = BMIForm()
    if form.validate_on_submit():
        weight = form.weight.data
        height = form.height.data
        bmi = weight / ((height / 100) ** 2)
        bmi_rounded = round(bmi, 1)
        category = get_category(bmi)
        return render_template(
            "result.html",
            bmi=bmi_rounded,
            category=category,
            weight=weight,
            height=height,
            age=form.age.data,
        )
    return render_template("index.html", form=form)


if __name__ == "__main__":
    app.run(debug=True)