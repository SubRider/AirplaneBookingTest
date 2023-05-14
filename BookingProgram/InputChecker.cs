using System;


static class InputChecker
{
    private static int _cursorLeft;
    private static int _cursorTop;
    private static int _lastLeft;
    private static int _lastTop;
    private static Button _selectedButton;
    private static Button newButton = _selectedButton;

    // Method to check and process keyboard input
    public static void CheckInput()
    {
        int index;
        _cursorTop = _lastTop = Console.GetCursorPosition().Top;
        _cursorLeft = _lastLeft = Console.GetCursorPosition().Left;
        ConsoleKeyInfo input = default;


        if (Console.KeyAvailable)
        {
            input = Console.ReadKey(true);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        if (Button.Buttons.Count > 0)
        {
            if (_selectedButton == null)
            {
                _selectedButton = Button.Buttons[0];
                JumpToButton(_selectedButton);
            }
        }

        // Handle specific key input
        if (input.Key == ConsoleKey.Escape) { BookingMenu.Quit = true; return; }
        else if (input.Key == ConsoleKey.Enter) { _selectedButton.Activate(); return; }
        else if (_selectedButton is InputButton)
        {
            InputButton button = _selectedButton as InputButton;
            if (input.Key == ConsoleKey.Backspace)
            {
                if (button.Input.Length > 0)
                {
                    button.Input = button.Input.Remove(button.Input.Length - 1);
                    Renderer.ClearLine();
                    Renderer.ShowButton(button);
                    BookingMenu.CurrentMenu();
                }

            }
            else if (input.KeyChar >= 0 && input.KeyChar <= 127 && input.KeyChar != default)
            {
                button.Input += input.KeyChar;
                Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
                Renderer.ShowButton(button);
                BookingMenu.CurrentMenu();
                return;
            }
        }

        try
        {
            int newLocation;

            // Process arrow keys or WASD keys to navigate through buttons
            if (input.Key == ConsoleKey.W || input.Key == ConsoleKey.UpArrow)
            {
                // Find the nearest button above the current button
                newLocation = _selectedButton.TrueYPosition;
                index = -1;
                while (index == -1)
                {
                    newLocation -= 1;
                    if (newLocation < 0)
                    {
                        Window reference = _selectedButton.Reference.Reference;
                        if (reference != null && reference.Buttons.Count > 0)
                        {
                            Renderer.HighlightButton(_selectedButton, true);
                            JumpToButton(_selectedButton.Reference.Reference.Buttons[^1]);
                        }
                        break;
                    }
                    index = Button.ButtonLocations.FindIndex(positionDelegate =>
                    {
                        (int x, int y) position = positionDelegate.Invoke();
                        return position.x == _selectedButton.TrueXPosition && position.y == newLocation;
                    });
                }
                if (index != -1)
                {
                    _cursorTop = newLocation;
                    _cursorLeft = _selectedButton.TrueXPosition;
                    newButton = Button.Buttons[index];
                }
            }
            else if (input.Key == ConsoleKey.S || input.Key == ConsoleKey.DownArrow)
            {
                newLocation = _selectedButton.TrueYPosition;
                index = -1;
                while (index == -1)
                {
                    newLocation += 1;
                    if (newLocation > Button.ButtonLocations.Select(positionDelegate => positionDelegate.Invoke().y).Max())
                    {
                        Window reference = _selectedButton.Reference.ReferencedBy;
                        if (reference != null && reference.Buttons.Count > 0)
                        {
                            Renderer.HighlightButton(_selectedButton, true);
                            JumpToButton(_selectedButton.Reference.ReferencedBy.Buttons[0]);
                        }
                        break;
                    }
                    index = Button.ButtonLocations.FindIndex(positionDelegate =>
                    {
                        (int x, int y) position = positionDelegate.Invoke();
                        return position.x == _selectedButton.TrueXPosition && position.y == newLocation;
                    });
                }
                if (index != -1)
                {
                    _cursorTop = newLocation;
                    _cursorLeft = _selectedButton.TrueXPosition;
                    newButton = Button.Buttons[index];
                }
            }
            else if (input.Key == ConsoleKey.A || input.Key == ConsoleKey.LeftArrow)
            {
                // Find the nearest button to the left of the current button
                newLocation = _selectedButton.TrueXPosition;
                index = -1;
                while (index == -1)
                {
                    newLocation -= 1;
                    if (newLocation < 0) break;
                    index = Button.ButtonLocations.FindIndex(positionDelegate =>
                    {
                        (int x, int y) position = positionDelegate.Invoke();
                        return position.x == newLocation && position.y == _selectedButton.TrueYPosition;
                    });
                }
                if (index != -1)
                {
                    _cursorTop = _selectedButton.TrueYPosition;
                    _cursorLeft = newLocation;
                    newButton = Button.Buttons[index];
                }
            }
            else if (input.Key == ConsoleKey.D || input.Key == ConsoleKey.RightArrow)
            {
                newLocation = _selectedButton.TrueXPosition;
                index = -1;
                while (index == -1)
                {
                    newLocation += 1;
                    if (newLocation > Button.ButtonLocations.Select(positionDelegate => positionDelegate.Invoke().x).Max()) break;
                    index = Button.ButtonLocations.FindIndex(positionDelegate =>
                    {
                        (int x, int y) position = positionDelegate.Invoke();
                        return position.x == newLocation && position.y == _selectedButton.TrueYPosition;
                    });
                }
                if (index != -1)
                {
                    _cursorTop = _selectedButton.TrueYPosition;
                    _cursorLeft = newLocation;
                    newButton = Button.Buttons[index];
                }
            }
        }
        catch (ArgumentOutOfRangeException)
        {

        }
        Console.SetCursorPosition(_cursorLeft, _cursorTop);
        

        // Highlight the button at the current cursor position
        if (_lastLeft != _cursorLeft || _lastTop != _cursorTop)
        {
            Renderer.HighlightButton(_selectedButton, true);
            Renderer.HighlightButton(newButton);
            _selectedButton = newButton;
        }
        return;
    }

    // Method to move the cursor to a specified button by index

    public static void Clear()
    {
        _selectedButton = null;
    }
    public static void JumpToButton(int index) => Console.SetCursorPosition(Button.Buttons[index].TrueXPosition, Button.Buttons[index].TrueYPosition);

    public static void JumpToButton(Button? button)
    {
        if (button == null) return;
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        Renderer.HighlightButton(button);
    }
}
