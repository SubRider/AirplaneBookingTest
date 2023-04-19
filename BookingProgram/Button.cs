class Button
{
    // Fields to store the highlight start time and whether the button is highlighted
    private DateTime _highlightStart;
    private bool _highlighted = false;

    // Static lists to store positions and instances of all created buttons
    public static List<int> ButtonYLocations = new();
    public static List<int> ButtonXLocations = new();
    public static List<(int, int)> ButtonLocations = new();
    public static List<Button> Buttons = new();

    // Readonly properties for button text color and button text
    public readonly ConsoleColor Color;
    public readonly string Text;

    // Property to get the background color based on the button's highlighted state
    public ConsoleColor HighlightColor { get { return (_highlighted) ? ConsoleColor.DarkBlue : ConsoleColor.Black; } set { } }

    // Properties to store button position and associated function
    public int YPosition;
    public int XPosition;
    public Action Function;

    // Property to get the time elapsed since the button was highlighted
    public TimeSpan HighlightTime { get { return DateTime.Now - _highlightStart; } }

    // Main constructor for creating a button with all properties
    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Action function)
    {
        Color = color;
        Text = text;
        YPosition = yPosition;
        XPosition = xPosition;
        Function = function;

        // Add button positions and instance to the static lists
        ButtonYLocations.Add(YPosition);
        ButtonXLocations.Add(XPosition);
        ButtonLocations.Add((XPosition, YPosition));
        Buttons.Add(this);
    }

    // Overloaded constructors with different sets of parameters
    public Button(string text, int yPosition, Action function) : this(ConsoleColor.White, text, yPosition, 0, function)
    {

    }
    public Button(string text, int yPosition, int xPosition, Action function) : this(ConsoleColor.White, text, yPosition, xPosition, function)
    {

    }
    public Button(ConsoleColor color, string text, int yPosition, Action function) : this(color, text, yPosition, 0, function)
    {

    }

    // Method to activate the button by calling its associated function and highlighting it
    public void Activate()
    {
        Function();
        Highlight();
    }

    // Method to highlight the button, update the _highlightStart field, and call Renderer.HighlightButton()
    public void Highlight()
    {
        _highlightStart = DateTime.Now;
        _highlighted = true;
        Renderer.HighlightButton(this);
    }

    // Static method to remove a button from the static lists and set its reference to null
    public static void RemoveButton(Button button)
    {
        int index = Buttons.IndexOf(button);
        ButtonXLocations.RemoveAt(index);
        ButtonYLocations.RemoveAt(index);
        ButtonLocations.RemoveAt(index);
        Buttons.Remove(button);
        button = null;
    }

    // Static method to clear all buttons by removing them from the static lists and setting their references to null
    public static void Clear()
    {
        List<Button> tempButtons = new(Buttons);
        foreach (Button button in tempButtons)
        {
            RemoveButton(button);
        }
    }
}