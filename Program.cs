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
    static int _prevCannonX;
    static bool _cannonFired;


    static void Main(string[] args)
    {
        _field = new Cell[50, 30];
        Console.CursorVisible = false;
        while (true)
        {
            ProcessInput();
            ProcessLogic();
            DrawField();
        }
    }

    static void ProcessInput()
    {
        if (!Console.KeyAvailable)
        {
            return;
        }

        var key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.RightArrow:
                if (_cannonX != _field.GetLength(0) - 1)
                {
                    _cannonX += 1;
                }
                break;

            case ConsoleKey.LeftArrow:
                if (_cannonX != 0)
                {
                    _cannonX -= 1;
                }
                break;

            case ConsoleKey.Spacebar:
                _cannonFired = true;
                break;
        }
    }

    static void ProcessLogic()
    {
        _field[_prevCannonX, _field.GetLength(1) - 1] = Cell.Empty;
        _prevCannonX = _cannonX;
        _field[_cannonX, _field.GetLength(1) - 1] = Cell.Cannon;
        for (int row = 0; row < _field.GetLength(1); row++)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                var cell = _field[column, row];
                switch (cell)
                {
                    case Cell.Projectile:
                        _field[column, row] = Cell.Empty;
                        if (row != 0)
                        {
                            AddProjectile(column, row - 1);
                        }
                        break;
                }
            }
        }
        if (_cannonFired)
        {
            AddProjectile(_cannonX, _field.GetLength(1) - 2);
            _cannonFired = false;
        }
    }

    static void AddProjectile(int column, int row)
    {
        _field[column, row] = Cell.Projectile;
    }

    static void DrawField()
    {
        Console.SetCursorPosition(0, 0);
        for (int row = 0; row < _field.GetLength(1); row++)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                char symbol;
                var cell = _field[column, row];
                switch (cell)
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
