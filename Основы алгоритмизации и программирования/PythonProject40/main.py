from flask import Flask, render_template, request, url_for, redirect
from flask_wtf import FlaskForm
from wtforms import StringField, TextAreaField, SubmitField, BooleanField, SelectField
from wtforms.validators import DataRequired
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///tasks.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SECRET_KEY'] = 'secret'

db = SQLAlchemy(app)


class Task(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String, nullable=False)
    description = db.Column(db.String, nullable=True)
    done = db.Column(db.Boolean, default=False)
    category = db.Column(db.String, nullable=False, default='Общее')


CATEGORIES = ['Общее', 'Учёба', 'Работа', 'Дом', 'Личное']

class TaskForm(FlaskForm):
    title = StringField('Название', validators=[DataRequired()])
    description = TextAreaField('Описание')
    category = SelectField('Категория', choices=[(c, c) for c in CATEGORIES])
    done = BooleanField('Выполнено')
    submit = SubmitField('Сохранить')


@app.route('/')
def index():
    category = request.args.get('category', '')
    sort = request.args.get('sort', '')

    query = Task.query
    if category:
        query = query.filter_by(category=category)

    if sort == 'done':
        query = query.order_by(Task.done.desc())
    elif sort == 'undone':
        query = query.order_by(Task.done.asc())

    tasks = query.all()

    total = Task.query.count()
    done_count = Task.query.filter_by(done=True).count()
    undone_count = total - done_count

    return render_template(
        'main.html',
        tasks=tasks,
        categories=CATEGORIES,
        current_category=category,
        current_sort=sort,
        total=total,
        done_count=done_count,
        undone_count=undone_count
    )


@app.route('/add', methods=['GET', 'POST'])
def add():
    form = TaskForm()
    if form.validate_on_submit():
        task = Task(
            title=form.title.data,
            description=form.description.data,
            category=form.category.data,
            done=form.done.data
        )
        db.session.add(task)
        db.session.commit()
        return redirect(url_for('index'))
    return render_template('add.html', form=form)


@app.route('/task/<int:id>')
def task(id):
    task = Task.query.get_or_404(id)
    return render_template('task.html', task=task)



@app.route('/edit-task/<int:id>', methods=['GET', 'POST'])
def edit_task(id):
    task = Task.query.get_or_404(id)
    form = TaskForm(obj=task)
    if form.validate_on_submit():
        task.title = form.title.data
        task.description = form.description.data
        task.category = form.category.data
        task.done = form.done.data
        db.session.commit()
        return redirect(url_for('index'))
    return render_template('edit.html', form=form, task=task)


@app.route('/delete-task/<int:id>')
def delete_task(id):
    task = Task.query.get_or_404(id)
    db.session.delete(task)
    db.session.commit()
    return redirect(url_for('index'))


@app.route('/toggle-task/<int:id>')
def toggle_task(id):
    task = Task.query.get_or_404(id)
    task.done = not task.done
    db.session.commit()
    return redirect(request.referrer or url_for('index'))


if __name__ == '__main__':
    with app.app_context():
        db.create_all()
    app.run(debug=True)