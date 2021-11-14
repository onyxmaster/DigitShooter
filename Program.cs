using System;
using System.Diagnostics;

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
        Star,
    }

    static Cell[,] _field;
    static int _cannonX;
    static int _cannonY;

    static int _prevCannonX;
    static int _prevCannonY;
    static bool _cannonFired;
    static bool _bombFired;
    static bool _ultraBombFired;
    static long _projectileTime;
    static long _digitGenTime;
    static long _digitMoveTime;
    static long _bombTime;
    static bool _gameOver;
    static bool _hasBomb;
    static bool _hasUltraBomb;
    static readonly Random _rng = new();
    static int _score;
    static void Main(string[] args)
    {
        
        _score = -40;
        _field = new Cell[5, 40];
        _cannonY = _field.GetLength(1) - 1;
        Console.CursorVisible = false;
        while (!_gameOver)
        {
            ProcessInput();
            ProcessLogic();
            DrawField();
        }
        Console.ReadLine();
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

            case ConsoleKey.DownArrow:
                if (_cannonY != _field.GetLength(1) - 1)
                {
                    _cannonY += 1;
                }
                break;

            case ConsoleKey.UpArrow:
                if (_cannonY != 0)
                {
                    _cannonY -= 1;
                }
                break;

            case ConsoleKey.Spacebar:
                _cannonFired = true;
                break;

            case ConsoleKey.B:
                _bombFired = true;
                break;

            case ConsoleKey.C:
            if (_hasUltraBomb)
            {
                NewMethod(0,8);
            }
                _hasUltraBomb = false;
                break;
    

        }
    }

    static void ProcessLogic()
    {
        var time = GetCurrentTime();
        _field[_prevCannonX, _prevCannonY] = Cell.Empty;
        _prevCannonX = _cannonX;
        _prevCannonY = _cannonY;
        AddCannon(_cannonX, _cannonY);

        const int ProjectileDelay = 100;
        var processProjectile = time - _projectileTime > ProjectileDelay;
        if (processProjectile)
        {
            _projectileTime = time;
        }

        const int BombDelay = 5000;
        var processBomb = time - _bombTime > BombDelay;
        _hasBomb = processBomb;

        var  processUltraBomb = _hasUltraBomb;
        _hasUltraBomb = processUltraBomb;

             if (_bombFired)
            {
                _bombFired = false;
                if (processBomb)
            {
                var BombRadius = 4;
                NewMethod(time, BombRadius);
            }
        }

        const int DigitGenDelay = 300;
        var generateDigits = time - _digitGenTime > DigitGenDelay;
        if (generateDigits)
        {
            _digitGenTime = time;
        }

        const int DigitMoveDelay = 300;
        var moveDigits = time - _digitMoveTime > DigitMoveDelay;
        if (moveDigits)
        {
            _digitMoveTime = time;
            _score += 1;
        }

        for (int row = 0; row < _field.GetLength(1); row++)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                var cell = _field[column, row];
                switch (cell)
                {
                    case Cell.Projectile:
                        if (processProjectile)
                        {
                            _field[column, row] = Cell.Empty;
                            if (row != 0)
                            {
                                AddProjectile(column, row - 1);
                            }
                        }
                        break;
                }
            }
        }

        for (int row = _field.GetLength(1) - 1; row >= 0; row--)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                var cell = _field[column, row];
                switch (cell)
                {
                    case Cell.Digit1:
                    case Cell.Digit2:
                    case Cell.Digit3:
                    case Cell.Digit4:
                    case Cell.Digit5:
                    case Cell.Digit6:
                    case Cell.Digit7:
                    case Cell.Digit8:
                    case Cell.Digit9:
                        if (moveDigits)
                        {
                            _field[column, row] = Cell.Empty;
                            if (row != _field.GetLength(1) - 1)
                            {
                                AddDigit(column, row + 1, cell);
                            }
                        }
                        break;
                }
            }
        }

        if (generateDigits)
        {
            for (int column = 0; column < _field.GetLength(0); column++)
            {
                const double DigitProbability = 0.1;
                if (_rng.NextDouble() >= DigitProbability)
                {
                    continue;
                }
                if (_field[column, 0] != Cell.Empty)
                {
                    continue;
                }
                var digit = Cell.Digit1 + _rng.Next(9);
                AddDigit(column, 0, digit);
            }
        }
        if (_cannonFired)
        {
            if (processProjectile)
            {
                if (_cannonY != 0)
                {
                    AddProjectile(_cannonX, _cannonY - 1);
                }
                _cannonFired = false;
            }
        }
    }

    private static void NewMethod(long time, int BombRadius)
    {
        _bombTime = time;
        _hasBomb = false;
        for (int row = _cannonY - BombRadius; row < _cannonY + BombRadius; row++)
        {
            if (row < 0 || row > _field.GetLength(1) - 1)
            {
                continue;
            }

            for (int column = _cannonX - BombRadius; column < _cannonX + BombRadius; column++)
            {
                if (column < 0 || column > _field.GetLength(0) - 1)
                {
                    continue;
                }
                if (_cannonX == _cannonY)
                {
                    continue;
                }
                _field[column, row] = Cell.Projectile;
            }
        }
    }
    static long GetCurrentTime()
    {
        return Stopwatch.GetTimestamp() * 1000 / Stopwatch.Frequency;
    }

    static void CollideDigit(int column, int row)
    {
        _field[column, row] = Cell.Star;
        _gameOver = true;
    }
    private static void AddCannon(int column, int row)
    {
        if (_field[column, row] != Cell.Empty)
        {
            CollideDigit(column, row);
            return;
        }
        _field[column, row] = Cell.Cannon;
    }

    static void CollideProjectile(int column, int row, Cell digit)
    {
        _field[column, row] = digit - 1;
    }
    static void AddProjectile(int column, int row)
    {
        if (_field[column, row] != Cell.Empty)
        {
            CollideProjectile(column, row, _field[column, row]);
            return;
        }

        _field[column, row] = Cell.Projectile;
    }

    static void AddDigit(int column, int row, Cell digit)
    {
        if (row == _field.GetLength(1) - 1)
        {
            CollideDigit(column, row);
            return;
        }
        if (_field[column, row] == Cell.Cannon)
        {
            CollideDigit(column, row);
            return;
        }
        if (_field[column, row] == Cell.Projectile)
        {
            CollideProjectile(column, row, digit);
            return;
        }

        _field[column, row] = digit;
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

                    case Cell.Star:
                        symbol = '*';
                        break;

                    default:
                        symbol = '?';
                        break;
                }

                Console.Write(symbol);
            }
            Console.WriteLine();
        }
        Console.WriteLine($"{Math.Max(_score, 0):D4}");
        Console.WriteLine(_hasBomb ? "B" : "XXXX");
    }
}
