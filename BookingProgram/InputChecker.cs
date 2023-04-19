using System;

static class InputChecker
{
    private static int _cursorLeft;
    private static int _cursorTop;
    public static void CheckInput(ConsoleKey? input = null)
    {
        int index;
        _cursorTop = Console.GetCursorPosition().Top;
        _cursorLeft = Console.GetCursorPosition().Left;
        if (input == null)
        {
            input = GetKey();
        }
        else if (Console.KeyAvailable)
        {

            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
            
        }
        if (input == ConsoleKey.Escape) BookingMenu.Quit = true;
        else if (input == ConsoleKey.Enter)
        {
            index = Button.ButtonLocations.IndexOf((_cursorLeft, _cursorTop));
            if (index != -1) Button.Buttons[index].Activate();
        }
        else
        {
            try
            {
                index = Button.ButtonLocations.IndexOf((_cursorLeft, _cursorTop));
                int newLocation = 10;
                bool found = false;
                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow)
                {
                    newLocation = Button.ButtonYLocations[index];
                    while (!found && newLocation >= 0)
                    {
                        newLocation -= 1;
                        found = Button.ButtonYLocations.Contains(newLocation);
                        
                    }
                    if (found) 
                    {
                        index = Button.ButtonLocations.IndexOf((_cursorLeft, newLocation));
                        if (index != -1) _cursorTop = newLocation;
                    } 
                }
                else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow)
                {
                    newLocation = Button.ButtonYLocations[index];
                    while (!found && newLocation <= Button.ButtonYLocations.Max())
                    {
                        newLocation += 1;
                        found = Button.ButtonYLocations.Contains(newLocation);
                    }
                    if (found)
                    {
                        index = Button.ButtonLocations.IndexOf((_cursorLeft, newLocation));
                        if (index != -1) _cursorTop = newLocation;
                    }
                }
                else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow)
                {
                    newLocation = Button.ButtonXLocations[index];
                    while (!found && newLocation >= 0)
                    {
                        newLocation -= 1;
                        found = Button.ButtonXLocations.Contains(newLocation);
                    }
                    if (found)
                    {
                        index = Button.ButtonLocations.IndexOf((newLocation, _cursorTop));
                        if (index != -1) _cursorLeft = newLocation;
                    }
                }
                else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow)
                {
                    newLocation = Button.ButtonXLocations[index];
                    while (!found && newLocation <= Button.ButtonXLocations.Max())
                    {
                        newLocation += 1;
                        found = Button.ButtonXLocations.Contains(newLocation);
                    }
                    if (found)
                    {
                        index = Button.ButtonLocations.IndexOf((newLocation, _cursorTop));
                        if (index != -1) _cursorLeft = newLocation;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
        }
        index = Button.ButtonLocations.IndexOf((_cursorLeft, _cursorTop));
        if (index != -1) Renderer.HighlightButton(Button.Buttons[index]);
        return;
    }
    public static ConsoleKey GetKey()
    {
        return Console.ReadKey(intercept: true).Key;
    }
    public static void JumpToButton(int index)
    {
        _cursorLeft = Button.ButtonXLocations[index];
        _cursorTop = Button.ButtonYLocations[index];
        Console.SetCursorPosition(_cursorLeft, _cursorTop);
    }
}