using System;

class Program100
{
    static void Main()
    {
        string word = "программирование";
        char[] guessed = new char[word.Length];
        for (int i = 0; i < guessed.Length; i++)
        {
            guessed[i] = '_';
        }

        int mistakes = 0;
        int maxMistakes = 6;
        bool win = false;

        while (mistakes < maxMistakes && !win)
        {
            Console.Clear();
            ShowWord(guessed);
            ShowHangman(mistakes);
            Console.WriteLine("Ошибки: " + mistakes + " из " + maxMistakes);
            Console.Write("Введите букву: ");
            char letter = Console.ReadLine()[0];

            bool found = false;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter)
                {
                    guessed[i] = letter;
                    found = true;
                }
            }

            if (!found)
            {
                mistakes++;
            }

            win = true;
            for (int i = 0; i < guessed.Length; i++)
            {
                if (guessed[i] == '_')
                {
                    win = false;
                }
            }
        }

        Console.Clear();
        ShowWord(guessed);
        ShowHangman(mistakes);

        if (win)
        {
            Console.WriteLine("Вы победили! Слово: " + word);
        }
        else
        {
            Console.WriteLine("Вы проиграли. Слово было: " + word);
        }
    }

    static void ShowWord(char[] guessed)
    {
        for (int i = 0; i < guessed.Length; i++)
        {
            Console.Write(guessed[i] + " ");
        }
        Console.WriteLine();
    }

    static void ShowHangman(int mistakes)
    {
        if (mistakes == 0)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 1)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 2)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine("  |   |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 3)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|   |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 4)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\ |");
            Console.WriteLine("      |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 5)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\ |");
            Console.WriteLine(" /    |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
        else if (mistakes == 6)
        {
            Console.WriteLine("  +---+");
            Console.WriteLine("  |   |");
            Console.WriteLine("  O   |");
            Console.WriteLine(" /|\\ |");
            Console.WriteLine(" / \\ |");
            Console.WriteLine("      |");
            Console.WriteLine("=========");
        }
    }
}
