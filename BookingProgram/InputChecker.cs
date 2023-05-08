using System;


static class InputChecker
{
    private static int _cursorLeft;
    private static int _cursorTop;
    private static int _lastLeft;
    private static int _lastTop;
    private static Button button = Renderer.SelectedButton;
    private static Button newButton = button;

    // Method to check and process keyboard input
    public static void CheckInput(ConsoleKey? input = null)
    {
        int index;
        _cursorTop = _lastTop = Console.GetCursorPosition().Top;
        _cursorLeft = _lastLeft = Console.GetCursorPosition().Left;


        if (Console.KeyAvailable && input == null)
        {
            input = Console.ReadKey(intercept: true).Key;
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
        }

        // Handle specific key input
        if (input == ConsoleKey.Escape) BookingMenu.Quit = true;
        else if (input == ConsoleKey.Enter)
        {
            Renderer.SelectedButton?.Activate();
        }
        else
        {
            button = Renderer.SelectedButton;
            try
            {
                int newLocation;

                // Process arrow keys or WASD keys to navigate through buttons
                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow)
                {
                    // Find the nearest button above the current button
                    newLocation = button.RelativeYPosition;
                    index = -1;
                    while (index == -1)
                    {
                        if (button.ReferenceSide == "top" || button.ReferenceSide == "left")
                        {
                            newLocation -= 1;
                            if (newLocation < 0)
                            {
                                Renderer.HighlightButton(button, true);
                                JumpToButton(button.Reference.Reference.Buttons[^1]);
                                break;
                            }
                        }
                        else if (button.ReferenceSide == "bottom")
                        {
                            newLocation += 1;
                            if (newLocation > Button.ButtonLocations.Select(t => t.y).Max())
                            {
                                Renderer.HighlightButton(button, true);
                                if (button.Reference.Reference != null) JumpToButton(button.Reference.Reference.Buttons[^1]);
                                break;
                            }
                            index = Button.ButtonLocations.IndexOf((button.RelativeXPosition, newLocation));
                        }
                         
                    }
                    if (index != -1) 
                    { 
                        _cursorTop = Button.Buttons[index].TrueYPosition;
                        newButton = Button.Buttons[index];
                    }
                }
                else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow)
                {
                    newLocation = button.RelativeYPosition;
                    index = -1;
                    while (index == -1)
                    {
                        if (button.ReferenceSide == "top" || button.ReferenceSide == "left")
                        {
                            newLocation += 1;
                            if (newLocation > Button.ButtonLocations.Select(t => t.y).Max())
                            {
                                if (button.Reference.ReferencedBy != null) 
                                {
                                    Renderer.HighlightButton(button, true);
                                    JumpToButton(button.Reference.ReferencedBy.Buttons[0]);
                                }
                                break;
                            };
                            index = Button.ButtonLocations.IndexOf((button.RelativeXPosition, newLocation));
                        }
                        else if (button.ReferenceSide == "bottom")
                        {
                            newLocation -= 1;
                            if (newLocation < 0) 
                            {
                                Renderer.HighlightButton(button, true);
                                JumpToButton(button.Reference.ReferencedBy.Buttons[0]);
                                break;
                            }
                            index = Button.ButtonLocations.IndexOf((button.RelativeXPosition, newLocation));
                        }
                        
                    }
                    if (index != -1)
                    {
                        _cursorTop = Button.Buttons[index].TrueYPosition;
                        newButton = Button.Buttons[index];
                    }
                }
                else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow)
                {
                    // Find the nearest button to the left of the current button
                    newLocation = button.RelativeXPosition;
                    index = -1;
                    while (index == -1)
                    {
                        newLocation -= 1;
                        if (newLocation < 0) break;
                        index = Button.ButtonLocations.IndexOf((newLocation, button.RelativeYPosition));
                    }
                    if (index != -1) 
                    {
                        _cursorLeft = Button.Buttons[index].TrueXPosition;
                        newButton = Button.Buttons[index];
                    }
                }
                else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow)
                {
                    newLocation = button.RelativeXPosition;
                    index = -1;
                    while (index == -1)
                    {
                        newLocation += 1;
                        if (newLocation > Button.ButtonLocations.Select(t => t.x).Max()) break;
                        index = Button.ButtonLocations.IndexOf((newLocation, button.RelativeYPosition));
                    }
                    if (index != -1)
                    {
                        _cursorLeft = Button.Buttons[index].TrueXPosition;
                        newButton = Button.Buttons[index];
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
        }

        // Highlight the button at the current cursor position
        if (_lastLeft != _cursorLeft || _lastTop != _cursorTop)
        {
            Renderer.HighlightButton(button, true);
            Renderer.HighlightButton(newButton);
        }
        return;
    }

    // Method to move the cursor to a specified button by index
    public static void JumpToButton(int index) => Console.SetCursorPosition(Button.Buttons[index].TrueXPosition, Button.Buttons[index].TrueYPosition);

    public static void JumpToButton(Button? button)
    {
        if (button == null) return;
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        Renderer.HighlightButton(button);
    }
}
