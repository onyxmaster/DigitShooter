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
        Crate,
        Star,
        Projectile,
        Cannon,
    }

    static Cell[,] _field;
    static int _cannonX;
    static int _cannonY;

    static int _prevCannonX;
    static int _prevCannonY;
    static long _slowTimeEndTime;
    static long _fireEndTime;
    static long _ultraCannonEndTime;
    static bool _cannonFired;
    static bool _bombFired;
    static bool _ultraBombFired;
    static bool _slowTimeActivated;
    static bool _fireActivated;
    static bool _ultraCannonActivated;
    static long _projectileTime;
    static long _digitGenTime;
    static long _digitMoveTime;
    static long _bombTime;
    static long _ultraBombTime;
    static bool _gameOver;
    static bool _hasBomb;
    static bool _hasUltraBomb;
    static int _slowTimeCount;
    static int _fireCount;
    static int _ultraCannonCount;

    static readonly Random _rng = new();
    static int _score;
    static long _currentTime;

    static void Main(string[] args)
    {
        _slowTimeCount = _fireCount = _ultraCannonCount = 1;
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
                _ultraBombFired = true;
                break;

            case ConsoleKey.S:
                _slowTimeActivated = true;
                break;

            case ConsoleKey.F:
                _fireActivated = true;
                break;

            case ConsoleKey.V:
                _ultraCannonActivated = true;
                break;

        }
    }

    static void ProcessLogic()
    {
        _currentTime = GetCurrentTime();
        _field[_prevCannonX, _prevCannonY] = Cell.Empty;
        _prevCannonX = _cannonX;
        _prevCannonY = _cannonY;
        AddCannon(_cannonX, _cannonY);

        const int ProjectileDelay = 100;
        var processProjectile = _currentTime - _projectileTime > ProjectileDelay;
        if (processProjectile)
        {
            _projectileTime = _currentTime;
        }

        const int BombDelay = 5000;
        var processBomb = _currentTime - _bombTime > BombDelay;
        _hasBomb = processBomb;

        if (_slowTimeActivated)
        {
            _slowTimeActivated = false;
            if (_slowTimeCount > 0)
            {
                _slowTimeCount--;
                const int SlowTimeTime = 5000;
                _slowTimeEndTime = _currentTime + SlowTimeTime;
            }
        }

        if (_fireActivated)
        {
            _fireActivated = false;
            if (_fireCount > 0)
            {
                _fireCount--;
                const int FireTime = 5000;
                _fireEndTime = _currentTime + FireTime;
            }
        }

        if (_ultraCannonActivated)
        {
            _ultraCannonActivated = false;
            if (_ultraCannonCount > 0)
            {
                _ultraCannonCount--;
                const int UltraCannonTime = 10000;
                _ultraCannonEndTime = _currentTime + UltraCannonTime;
            }
        }

        if (_bombFired)
        {
            _bombFired = false;
            if (processBomb)
            {
                _bombTime = _currentTime;
                _hasBomb = false;
                var BombRadius = 4;
                AddBomb(BombRadius);
            }
        }

        const int UltraBombDelay = 1000000;
        var processUltraBomb = _currentTime - _ultraBombTime > UltraBombDelay;
        _hasUltraBomb = processUltraBomb;

        if (_ultraBombFired)
        {
            _ultraBombFired = false;
            if (processUltraBomb)
            {
                _ultraBombTime = _currentTime;
                _hasUltraBomb = false;
                var UltraBombRadius = 10;
                AddBomb(UltraBombRadius);
            }
        }

        var DigitDelay = 500;
        if (_slowTimeEndTime > _currentTime)
        {
            DigitDelay *= 3;
        }
        var generateDigits = _currentTime - _digitGenTime > DigitDelay;
        if (generateDigits)
        {
            _digitGenTime = _currentTime;
        }

        var moveDigits = _currentTime - _digitMoveTime > DigitDelay;
        if (moveDigits)
        {
            _digitMoveTime = _currentTime;
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

                    case Cell.Crate:
                        if (moveDigits)
                        {
                            _field[column, row] = Cell.Empty;
                            if (row != _field.GetLength(1) - 1)
                            {
                                AddCrate(column, row + 1);
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
                const double DigitProbability = 2;
                if (_rng.NextDouble() >= DigitProbability)
                {
                    continue;
                }
                if (_field[column, 0] != Cell.Empty)
                {
                    continue;
                }
                var digit = Cell.Digit1 + _rng.Next(10);
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
        if (_fireEndTime > _currentTime)
        {
            const int FireRadius = 4;
            for (int row = _cannonY - FireRadius; row < _cannonY + FireRadius; row++)
            {
                if (row < 0 || row > _field.GetLength(1) - 1)
                {
                    continue;
                }

                for (int column = _cannonX - FireRadius; column < _cannonX + FireRadius; column++)
                {
                    if (column < 0 || column > _field.GetLength(0) - 1)
                    {
                        continue;
                    }
                    if (column == _cannonX && row == _cannonY)
                    {
                        continue;
                    }
                    _field[column, row] = Cell.Empty;
                }
            }
        }
    }

    private static void AddBomb(int radius)
    {
        for (int row = _cannonY - radius; row < _cannonY + radius; row++)
        {
            if (row < 0 || row > _field.GetLength(1) - 1)
            {
                continue;
            }

            for (int column = _cannonX - radius; column < _cannonX + radius; column++)
            {
                if (column < 0 || column > _field.GetLength(0) - 1)
                {
                    continue;
                }
                if (column == _cannonX && row == _cannonY)
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

    static void CollideCrate(int column, int row)
    {
        var number = _rng.Next(3);
        switch (number)
        {
            case 0:
                _slowTimeCount += 1;
                break;

            case 1:
                _fireCount += 1;
                break;

            case 2:
                _ultraCannonCount += 1;
                break;
            default: 
                Console.WriteLine("");   
                Console.WriteLine("бе");
                break;
        }

    }

    private static void AddCannon(int column, int row)
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
                CollideDigit(column + 5, row + 5);
                return;

            case Cell.Crate:
                CollideCrate(column, row);
                _field[column + 1, row + 1] = Cell.Digit9;
                return;
        }

        _field[column, row] = Cell.Cannon;
    }

    static void CollideProjectile(int column, int row, Cell digit)
    {
        if (_ultraCannonEndTime > _currentTime)
        {
            _field[column, row] = Cell.Empty;
        }
        else
        {
            _field[column, row] = digit - 1;
        }
    }

    static void AddProjectile(int column, int row)
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
                CollideProjectile(column, row, cell);
                return;

            case Cell.Crate:
                CollideCrate(column, row);
                _field[column, row] = Cell.Empty;
                return;
        }

        _field[column, row] = Cell.Projectile;
    }

    static void AddCrate(int column, int row)
    {
        if (row == _field.GetLength(1) - 1)
        {
            return;
        }
        if (_field[column, row] == Cell.Cannon)
        {
            CollideCrate(column, row);
            return;
        }
        if (_field[column, row] == Cell.Projectile)
        {
            CollideCrate(column, row);
            return;
        }

        _field[column, row] = Cell.Crate;
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

                    case Cell.Crate:
                        symbol = 'C';
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
        Console.Write(_hasBomb ? 'B' : ' ');
        Console.Write(_hasUltraBomb ? 'U' : ' ');
        Console.WriteLine();
        Console.WriteLine($"{_slowTimeCount:D10} {_fireCount:D10} {_ultraCannonCount:D10}");
    }
}
