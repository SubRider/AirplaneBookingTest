using System;


static class InputChecker
{
    private static int _cursorLeft;
    private static int _cursorTop;

    // Method to check and process keyboard input
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

        // Handle specific key input
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

                // Process arrow keys or WASD keys to navigate through buttons
                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow)
                {
                    // Find the nearest button above the current button
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
                    // Find the nearest button below the current button
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
                    // Find the nearest button to the left of the current button
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
                    // Find the nearest button to the right of the current button
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

        // Highlight the button at the current cursor position
        index = Button.ButtonLocations.IndexOf((_cursorLeft, _cursorTop));
        if (index != -1) Renderer.HighlightButton(Button.Buttons[index]);

        return;
    }

    // Method to get a single key press
    public static ConsoleKey GetKey()
    {
        return Console.ReadKey(intercept: true).Key;
    }

    // Method to move the cursor to a specified button by index
    public static void JumpToButton(int index)
    {
        _cursorLeft = Button.ButtonXLocations[index];
        _cursorTop = Button.ButtonYLocations[index];
        Console.SetCursorPosition(_cursorLeft, _cursorTop);
    }
}