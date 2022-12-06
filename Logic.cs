static partial class Program
{
    private static void ProcessLogic()
    {
        ProcessLogic(0.1);
    }

    static void ProcessLogic(double DigitProbability)
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
                    case Cell.UltraProjectile:
                        if (processProjectile)
                        {
                            _field[column, row] = Cell.Empty;
                            if (row != 0)
                            {
                                AddUltraProjectile(column, row - 1);
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
                    if (_ultraCannonEndTime > _currentTime)
                    {
                        AddUltraProjectile(_cannonX, _cannonY - 1);
                    }
                    else
                    {
                        AddProjectile(_cannonX, _cannonY - 1);
                    }
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
                    if (_field[column, row] is < Cell.Digit1 or > Cell.Digit9)
                    {
                        continue;
                    }
                    _field[column, row] = Cell.Empty;
                }
            }
        }
    }
}