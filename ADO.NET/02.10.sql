-- Задание 1
-- Аналитический запрос

SELECT
    s.name AS student_name,
    g.name AS group_name,
    s.average_grade,
    COUNT(gr.id) AS grades_count,
    AVG(CAST(gr.grade AS FLOAT)) AS avg_grade
FROM college.students s
JOIN college.groups g ON s.group_id = g.id
LEFT JOIN college.grades gr ON s.id = gr.student_id
GROUP BY s.id, s.name, g.name, s.average_grade
ORDER BY s.name;


-- Задание 2
-- СТЕ и рейтинг

WITH StudentStats AS (
    SELECT
        id,
        name,
        group_id,
        average_grade
    FROM college.students
)
SELECT
    name,
    average_grade,
    RANK() OVER (ORDER BY average_grade DESC) AS rating
FROM StudentStats
ORDER BY rating;


-- Задание 3
-- Рейтинг внутри группы

WITH StudentStats AS (
    SELECT
        s.id,
        s.name AS student_name,
        g.name AS group_name,
        s.average_grade
    FROM college.students s
    JOIN college.groups g ON s.group_id = g.id
)
SELECT
    student_name,
    group_name,
    average_grade,
    RANK() OVER (PARTITION BY group_name ORDER BY average_grade DESC) AS group_rating
FROM StudentStats
ORDER BY group_name, group_rating;


-- Задание 4
-- Представление

CREATE OR ALTER VIEW college.StudentRating
AS
WITH StudentStats AS (
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        s.average_grade
    FROM college.students s
    JOIN college.groups g ON s.group_id = g.id
)
SELECT
    id,
    name,
    group_name,
    average_grade,
    RANK() OVER (PARTITION BY group_name ORDER BY average_grade DESC) AS group_rating
FROM StudentStats;

GO

SELECT * FROM college.StudentRating;

SELECT * FROM college.StudentRating WHERE group_name = N'ИС-31';


-- Задание 5
-- Хранимая процедура

CREATE OR ALTER PROCEDURE college.GetStudentsByMinGrade
    @min_grade FLOAT
AS
BEGIN
    SELECT
        id,
        name,
        age,
        average_grade,
        group_id
    FROM college.students
    WHERE average_grade >= @min_grade
    ORDER BY average_grade DESC;
END;

GO

EXEC college.GetStudentsByMinGrade @min_grade = 4.5;

EXEC college.GetStudentsByMinGrade @min_grade = 3.5;


-- Задание 6
-- Пользовательская функция

CREATE OR ALTER FUNCTION college.GetStudentStatus
(
    @average_grade FLOAT
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @status NVARCHAR(100);

    IF @average_grade >= 4.5
        SET @status = N'Отличник';
    ELSE IF @average_grade >= 3.5
        SET @status = N'Хорошист';
    ELSE
        SET @status = N'Требуется дополнительная подготовка';

    RETURN @status;
END;

GO

SELECT
    name,
    average_grade,
    college.GetStudentStatus(average_grade) AS status
FROM college.students;


-- Задание 7
-- Табличная функция

CREATE OR ALTER FUNCTION college.GetStudentsByGroup
(
    @group_id INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        id,
        name,
        age,
        average_grade
    FROM college.students
    WHERE group_id = @group_id
);

GO

SELECT * FROM college.GetStudentsByGroup(1);

SELECT * FROM college.GetStudentsByGroup(1) WHERE average_grade >= 4.5;


-- Задание 8
-- Триггер аудита

CREATE TABLE college.student_changes
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    old_average_grade FLOAT,
    new_average_grade FLOAT,
    change_date DATETIME NOT NULL DEFAULT GETDATE()
);

GO

CREATE OR ALTER TRIGGER college.trg_Students_AverageGradeAudit
ON college.students
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO college.student_changes (student_id, old_average_grade, new_average_grade, change_date)
    SELECT
        i.id,
        d.average_grade,
        i.average_grade,
        GETDATE()
    FROM inserted i
    JOIN deleted d ON i.id = d.id
    WHERE i.average_grade <> d.average_grade;
END;

GO

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE id = 1;

SELECT * FROM college.student_changes;

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE group_id = 1;

SELECT * FROM college.student_changes;


-- Итоговое задание
-- Единый аналитический запрос

SELECT
    sr.name AS Студент,
    sr.group_name AS Группа,
    sr.average_grade AS Средний_балл,
    college.GetStudentStatus(sr.average_grade) AS Статус,
    sr.group_rating AS Рейтинг_в_группе,
    (SELECT COUNT(*) FROM college.grades g WHERE g.student_id = sr.id) AS Количество_оценок
FROM college.StudentRating sr
ORDER BY sr.group_name, sr.group_rating;