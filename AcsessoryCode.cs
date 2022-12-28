static partial class Program
{
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
                if (_ultraCannonEndTime > _currentTime)
                {
                    _field[column, row] = Cell.UltraProjectile;
                }
                else
                {
                    _field[column, row] = Cell.Projectile;
                }
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
        _field[column, row] = digit - 1;
    }
    static void CollideUltraProjectile(int column, int row)
    {
        _field[column, row] = Cell.Empty;
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

    static void AddUltraProjectile(int column, int row)
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
            case Cell.Projectile:
                CollideUltraProjectile(column, row);
                return;

            case Cell.Crate:
                CollideCrate(column, row);
                _field[column, row] = Cell.Empty;
                return;
        }

        _field[column, row] = Cell.UltraProjectile;
    }

    static void CollideEnergy()
    {
        _energy++;
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
    static void AddAttach(int columnPrev, int rowPrev, int column, int row, Cell cell)
    {
        _field[0, rowPrev + 1] = cell;
        _field[1, rowPrev + 1] = cell;
        _field[2, rowPrev + 1] = cell;
        _field[3, rowPrev + 1] = cell;
        _field[4, rowPrev + 1] = cell;
        _field[column, row] = Cell.Attach;
    }
}