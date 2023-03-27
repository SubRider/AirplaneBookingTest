using System;

static class InputChecker
{
    public static int CursorLeft;
    public static int CursorTop;
    public static void CheckInput()
    {
        int index;
        CursorTop = Console.GetCursorPosition().Top;
        CursorLeft = Console.GetCursorPosition().Left;
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
                index = Button.ButtonLocations.IndexOf((CursorLeft, CursorTop));
                if (index != -1) Button.Buttons[index].Activate();
            }
            else
            {
                try
                {
                    index = Button.ButtonLocations.IndexOf((CursorLeft, CursorTop));
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
                        if (found) CursorTop = newLocation;
                    }
                    else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow)
                    {
                        newLocation = Button.ButtonYLocations[index];
                        while (!found && newLocation <= Button.ButtonYLocations.Max())
                        {
                            newLocation += 1;
                            found = Button.ButtonYLocations.Contains(newLocation);
                        }
                        if (found) CursorTop = newLocation;
                    }
                    else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow)
                    {
                        newLocation = Button.ButtonXLocations[index];
                        while (!found && newLocation >= 0)
                        {
                            newLocation -= 1;
                            found = Button.ButtonXLocations.Contains(newLocation);
                        }
                        if (found) CursorLeft = newLocation;
                    }
                    else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow)
                    {
                        newLocation = Button.ButtonXLocations[index];
                        while (!found && newLocation <= Button.ButtonXLocations.Max())
                        {
                            newLocation += 1;
                            found = Button.ButtonXLocations.Contains(newLocation);
                        }
                        if (found) CursorLeft = newLocation;
                    }
                    if (Button.Buttons[index].Highlighted) Renderer.HighlightButton(Button.Buttons[index], true);
                }
                catch (ArgumentOutOfRangeException)
                {

                }
                Console.SetCursorPosition(CursorLeft, CursorTop);
            }
            
        }
        index = Button.ButtonLocations.IndexOf((CursorLeft, CursorTop));
        if (index != -1) Renderer.HighlightButton(Button.Buttons[index]);
        return;
    }
    public static void JumpToButton(int index)
    {
        CursorLeft = Button.ButtonXLocations[index];
        CursorTop = Button.ButtonYLocations[index];
        Console.SetCursorPosition(CursorLeft, CursorTop);
    }
}