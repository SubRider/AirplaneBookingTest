static class InputChecker
{
    public static void CheckInput(bool leftRight)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKey input = Console.ReadKey(true).Key;
            int cursorTop = Console.GetCursorPosition().Top;
            int cursorLeft = Console.GetCursorPosition().Left;
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            if (input == ConsoleKey.Escape) ConsoleApp.Quit = true;
            if (input == ConsoleKey.Enter)
            {
                int index = Button.ButtonYLocations.IndexOf(cursorTop);
                if (index != -1) Button.Buttons[index].Activate();
                index = Button.ButtonLocations.IndexOf((cursorLeft, cursorTop));
                if (index != -1) Button.Buttons[index].Activate();
            }
            else if (!leftRight)
            {
                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow) cursorTop--;
                else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow) cursorTop++;
            }
            else if (leftRight)
            {
                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow) cursorTop--;
                else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow) cursorTop++;
                else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow) cursorLeft--;
                else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow) cursorLeft++;
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
        return;
    }
}