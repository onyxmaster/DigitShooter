static partial class Program
{ 
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

                    case Cell.UltraProjectile:
                        symbol = '+';
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