static partial class Program
{
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

            case ConsoleKey.U:
                _ultraBombFired = true;
                break;

            case ConsoleKey.S:
                _slowTimeActivated = true;
                break;
 
            //  case ConsoleKey.F:
            //      _fireActivated = true;
            //      break;

            case ConsoleKey.V:
                _ultraCannonActivated = true;
                break;
            case ConsoleKey.H:
                Console.ReadLine();
                break;
        }
    }

}