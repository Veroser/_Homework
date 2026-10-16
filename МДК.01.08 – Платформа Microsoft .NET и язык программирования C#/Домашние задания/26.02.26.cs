using System;

class Program103
{
    static void Main()
    {
        int width = 10;
        int height = 10;
        int playerX = 0;
        int playerY = 0;
        int monsterX = 9;
        int monsterY = 9;
        int turns = 0;
        int maxTurns = 20;

        while (turns < maxTurns)
        {
            Console.Clear();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == playerX && y == playerY)
                    {
                        Console.Write("P ");
                    }
                    else if (x == monsterX && y == monsterY)
                    {
                        Console.Write("M ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Ход: " + turns + " из " + maxTurns);

            if (playerX == monsterX && playerY == monsterY)
            {
                Console.WriteLine("Монстр поймал игрока! Игра окончена.");
                return;
            }

            Console.Write("Введите направление (w/a/s/d): ");
            char move = Console.ReadLine()[0];

            if (move == 'w' && playerY > 0)
            {
                playerY--;
            }
            else if (move == 's' && playerY < height - 1)
            {
                playerY++;
            }
            else if (move == 'a' && playerX > 0)
            {
                playerX--;
            }
            else if (move == 'd' && playerX < width - 1)
            {
                playerX++;
            }

            if (monsterX < playerX)
            {
                monsterX++;
            }
            else if (monsterX > playerX)
            {
                monsterX--;
            }
            else if (monsterY < playerY)
            {
                monsterY++;
            }
            else if (monsterY > playerY)
            {
                monsterY--;
            }

            turns++;

            if (playerX == monsterX && playerY == monsterY)
            {
                Console.Clear();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == playerX && y == playerY)
                        {
                            Console.Write("P ");
                        }
                        else if (x == monsterX && y == monsterY)
                        {
                            Console.Write("M ");
                        }
                        else
                        {
                            Console.Write(". ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Монстр поймал игрока! Игра окончена.");
                return;
            }
        }

        Console.WriteLine("Игрок продержался 20 ходов и победил!");
    }
}
