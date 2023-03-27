using System;

static class InputChecker
{
    public static void CheckInput()
    {
        int index;
        int cursorTop = Console.GetCursorPosition().Top;
        int cursorLeft = Console.GetCursorPosition().Left;
        if (Console.KeyAvailable)
        {
            ConsoleKey input = Console.ReadKey(true).Key;

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            if (input == ConsoleKey.Escape) BookingMenu.Quit = true;
            if (input == ConsoleKey.Enter)
            {
                index = Button.ButtonLocations.IndexOf((cursorLeft, cursorTop));
                if (index != -1) Button.Buttons[index].Activate();
            }
            else
            {
                try
                {
                    index = Button.ButtonLocations.IndexOf((cursorLeft, cursorTop));
                    if (input == ConsoleKey.W || input == ConsoleKey.UpArrow) cursorTop = Button.ButtonYLocations[index - 1];
                    else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow) cursorTop = Button.ButtonYLocations[index + 1];
                    else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow) cursorLeft = Button.ButtonXLocations[index - 1];
                    else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow) cursorLeft = Button.ButtonXLocations[index + 1];
                    if (Button.Buttons[index].Highlighted) Renderer.HighlightButton(Button.Buttons[index], true);
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
        index = Button.ButtonLocations.IndexOf((cursorLeft, cursorTop));
        if (index != -1) Renderer.HighlightButton(Button.Buttons[index]);
        return;
    }
    public static void JumpToButton(int index)
    {
        int cursorLeft = Button.ButtonXLocations[index];
        int cursorTop = Button.ButtonYLocations[index];
        Console.SetCursorPosition(cursorLeft, cursorTop);
    }
}