using System;

static class Program
{
    enum Cell
    {
        Empty,
        Digit1,
        Digit2,
        Digit3,
        Digit4,
        Digit5,
        Digit6,
        Digit7,
        Digit8,
        Digit9,
        Cannon,
        Projectile,
    }

    static Cell[,] _field;
    static int _cannonX;

    static void Main(string[] args)
    {
        _field = new Cell[4, 6];
        Console.CursorVisible = false;
        while (true)
        {
            DrawField();
        }
    }

    static void DrawField()
    {
        Console.SetCursorPosition(0, 0);
        for (int row = 0; row < _field.GetLength(1); row++)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                char symbol;
                switch (_field[column, row])
                {
                    case Cell.Empty:
                        symbol = '.';
                        break;
                        
                    case Cell.Digit1:
                        symbol = '1';
                        break;

                    case Cell.Digit2:
                        symbol = '2';
                        break;

                    case Cell.Digit3:
                        symbol = '3';
                        break;

                    case Cell.Digit4:
                        symbol = '4';
                        break;

                    case Cell.Digit5:
                        symbol = '5';
                        break;

                    case Cell.Digit6:
                        symbol = '6';
                        break;

                    case Cell.Digit7:
                        symbol = '7';
                        break;

                    case Cell.Digit8:
                        symbol = '8';
                        break;

                    case Cell.Digit9:
                        symbol = '9';
                        break;

                    case Cell.Cannon:
                        symbol = '!';
                        break;

                    case Cell.Projectile:
                        symbol = '^';
                        break;

                    default:
                        symbol = '?';
                        break;
                }

                Console.Write(symbol);
            }
            Console.WriteLine();
        }
        
    }
}
