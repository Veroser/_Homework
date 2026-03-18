-- Таблица books (Книги)

create table books(
    id serial primary key,                      -- Уникальный идентификатор книги
    title varchar(200) not null,                -- Название книги
    author varchar(100) not null,               -- Имя автора
    year integer not null check(year >= 0 and year <= 2026),   -- Год издания
    genre varchar(45),                          -- Жанр
    is_available boolean default true,          -- Наличие
    created_at timestamp default current_timestamp,            -- Дата добавления
    updated_at timestamp default current_timestamp             -- Дата последнего изменения
);


-- Индексация таблицы

-- Индекс для поиска по названию
create index idx_books_title on books using btree (title);

-- Индекс для поиска по автору
create index idx_books_author on books using btree (author);

-- Индекс для поиска по жанру
create index idx_books_jenre on books using btree (genre);

-- Индекс по поиску доступных книг
create index idx_books_available on books using btree (is_available);

-- Добавление временной ветки
create or replace function update_update_at_column()
returns trigger as $$
begin
    new.updated_at = current_timestamp;
    return new;
end;
$$ language plpgsql;

-- Триггер для таблицы books
create trigger update_boks_update_at
    before update on books
    for each row
    execute function update_update_at_column()

-- Тестовые данные
insert into books(title, author, year, genre, is_available) values
('Война и мир', 'Лев Толстой', 1869, 'Роман', TRUE),
('Преступление и наказание', 'Фёдор Достоевский', 1866, 'Роман', TRUE),
('Мастер и Маргарита', 'Михаил Булгаков', 1967, 'Роман', FALSE),
('1984', 'Джордж Оруэлл', 1949, 'Антиутопия', TRUE),
('Гарри Поттер и философский камень', 'Дж. К. Роулинг', 1997, 'Фэнтези', TRUE),
('Идиот', 'Фёдор Достоевский', 1869, 'Роман', TRUE),
('Анна Каренина', 'Лев Толстой', 1877, 'Роман', FALSE),
('Собачье сердце', 'Михаил Булгаков', 1925, 'Повесть', TRUE),
('Мы', 'Евгений Замятин', 1924, 'Антиутопия', TRUE),
('Преступление и наказание', 'Фёдор Достоевский', 1866, 'Роман', TRUE);

-- Представление: только доступные книги
create view available_books as 
select id, title, author, year, genre
from books
where is_available = TRUE;

-- Представление: статистика по жанрам
create view genre_stats as
select 
genre,
count(*) as total_books,
sum(case when is_available then 1 else 0 end) as available_count,
sum(case when is_available then 0 else 1 end) as unavailable_count
from books
group by genre
order by total_books desc;

-- Представление: статистика по авторам
create view author_stats as 
select 
author,
count(*) as book_count
from books
group by author
order by books_count desc;