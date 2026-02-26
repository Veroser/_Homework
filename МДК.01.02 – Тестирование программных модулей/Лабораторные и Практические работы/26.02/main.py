import pytest
from unittest.mock import patch, MagicMock
import sys
sys.path.append('c:/VSCODE/gui_with_db')
from services.recipe_service import RecipeService

class TestRecipeGetAll:

    def test_get_all_return_list(self, mock_conn, mock_cursor):
        mock_cursor.fetchall.return_value = [
            (1, "Том Ям", "Суп", "medium", "Основное блюдо"),
            (2, "Пельмени", "Мучное", "enum", "Основное блюдо")
        ]

        with patch('db.db.get_conn', return_value=mock_conn):
            result = RecipeService.get_all()

        assert isinstance(result, list)
        assert len(result) == 2
        assert result[0][1] == "Том Ям"

    def test_get_all_calls_corret_query(self, mock_conn, mock_cursor):
        with patch('db.db.get_conn', return_value=mock_conn):
            RecipeService.get_all()


        mock_cursor.execute.assert_called_once()


        call_args = mock_cursor.execute.call_args[0][0]
        assert 'SELECT' in call_args.upper()
        assert 'recipes.recipes' in call_args

    def test_get_all_handles_db_error(self, mock_conn, mock_cursor):

        mock_cursor.execute.side_effect = Exception('DB Error')
        
        with patch('db.db.get_conn', return_value=mock_conn):
            with pytest.raises(Exception):
                RecipeService.get_all()
            

            if hasattr(mock_conn, 'rollback'):
                mock_conn.rollback.assert_called_once()

    def test_get_all_closes_resources(self, mock_conn, mock_cursor):
        mock_cursor.fetchall.return_value = []
        with patch('db.db.get_conn', return_value=mock_conn):
            RecipeService.get_all()
        mock_cursor.close.assert_called_once()
        mock_conn.close.assert_called_once()


import pytest
from unittest.mock import patch, MagicMock


class T1:

    def m1(self, x, y, z):
        x.lastrowid = 7

        with patch('alpha.beta.gamma', return_value=x):
            r = Z1.create('Борщ', 3, 'сложный')

        assert r == 7
        x.commit.assert_called_once()

    def m2(self, x, y):
        d = {
            'a': 'Лазанья',
            'b': 5,
            'c': 'простой'
        }

        with patch('alpha.beta.gamma', return_value=x):
            Z1.create(d['a'], d['b'], d['c'])

        y.execute.assert_called_once()

        p1 = y.execute.call_args[0][0]
        p2 = y.execute.call_args[0][1]

        assert 'INSERT' in p1.upper()
        assert 'cookbook.recipes' in p1
        assert p2 == (d['a'], d['b'], d['c'])

    def m3(self, x, y):
        y.execute.side_effect = Exception('Ошибка БД')

        with patch('alpha.beta.gamma', return_value=x):
            with pytest.raises(Exception):
                Z1.create('Ужин', 2, 'средний')

            x.rollback.assert_called_once()

    def m4(self, x, y):
        x.lastrowid = 42

        with patch('alpha.beta.gamma', return_value=x):
            Z1.create('Завтрак', 1, 'легкий')

        y.close.assert_called_once()
        x.close.assert_called_once()

    def m5(self, x, y):
        with patch('alpha.beta.gamma', return_value=x):
            with pytest.raises(Exception):
                Z1.create('', None, 'неверный')


class T2:

    def a1(self, x, y):
        y.fetchone.return_value = (3, "Борщ", "Первое", "сложный", "Горячее")

        with patch('alpha.beta.gamma', return_value=x):
            r = Z2.fetch(3)

        assert r is not None
        assert r[0] == 3
        assert r[1] == "Борщ"
        assert r[3] == "сложный"

    def a2(self, x, y):
        y.fetchone.return_value = None

        with patch('alpha.beta.gamma', return_value=x):
            r = Z2.fetch(777)

        assert r is None

    def a3(self, x, y):
        rid = 9

        with patch('alpha.beta.gamma', return_value=x):
            Z2.fetch(rid)

        y.execute.assert_called_once()

        c1 = y.execute.call_args[0][0]
        c2 = y.execute.call_args[0][1]

        assert 'SELECT' in c1.upper()
        assert 'cookbook.recipes' in c1
        assert 'WHERE' in c1.upper()
        assert c2[0] == rid

    def a4(self, x, y):
        y.execute.side_effect = Exception('Ошибка соединения')

        with patch('alpha.beta.gamma', return_value=x):
            with pytest.raises(Exception):
                Z2.fetch(5)

    def a5(self, x, y):
        y.fetchone.return_value = (2, "Десерт", 4, "средний")

        with patch('alpha.beta.gamma', return_value=x):
            Z2.fetch(2)

        y.close.assert_called_once()
        x.close.assert_called_once()

    def a6(self, x, y):
        variants = [7, "7"]

        for vid in variants:
            y.reset_mock()
            y.fetchone.return_value = (int(str(vid)), "Пицца", 2, "легкий")

            with patch('alpha.beta.gamma', return_value=x):
                r = Z2.fetch(vid)

            assert r is not None
            assert r[0] == int(str(vid))
