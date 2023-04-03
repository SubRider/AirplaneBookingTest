class Button
{
    private DateTime _highlightStart;
    private bool _highlighted = false;
    public static List<int> ButtonYLocations = new();
    public static List<int> ButtonXLocations = new();
    public static List<(int, int)> ButtonLocations = new();
    public static List<Button> Buttons = new();
    public readonly ConsoleColor Color;
    public ConsoleColor HighlightColor { get { return (_highlighted) ? ConsoleColor.DarkBlue : ConsoleColor.Black; } set { } }
    public readonly string Text;
    public int YPosition;
    public int XPosition;
    public Action Function;

    public TimeSpan HighlightTime { get { return DateTime.Now - _highlightStart; } }

    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Action function)
    {
        Color = color;
        Text = text;
        YPosition = yPosition;
        XPosition = xPosition;
        Function = function;

        ButtonYLocations.Add(YPosition);
        ButtonXLocations.Add(XPosition);
        ButtonLocations.Add((XPosition, YPosition));
        Buttons.Add(this);
    }
    public Button(ConsoleColor color, string text, int yPosition, Action function) : this(color, text, yPosition, 0, function)
    {

    }

    public void Activate()
    {
        Function();
        Highlight();
    }
    public void Highlight()
    {
        _highlightStart = DateTime.Now;
        _highlighted = true;
        Renderer.HighlightButton(this);
    }

    public static void RemoveButton(Button button)
    {
        int index = Buttons.IndexOf(button);
        ButtonXLocations.RemoveAt(index);
        ButtonYLocations.RemoveAt(index);
        ButtonLocations.RemoveAt(index);
        Buttons.Remove(button);
        button = null;
    }
    public static void Clear()
    {
        List<Button> tempButtons = new(Buttons);
        foreach (Button button in tempButtons)
        {
            RemoveButton(button);
        }
    }
}
