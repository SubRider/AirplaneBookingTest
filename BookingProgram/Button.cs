class Button
{
    public static List<int> ButtonYLocations = new();
    public static List<int> ButtonXLocations = new();
    public static List<(int, int)> ButtonLocations = new();
    public static List<Button> Buttons = new();
    public readonly ConsoleColor Color;
    public readonly string Text;
    public int YPosition;
    public int XPosition;
    public Action Function1;
    public Action Function2;
    public bool Highlighted = false;
    public DateTime HighlightTime;

    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Action function1, Action function2)
    {
        Color = color;
        Text = text;
        YPosition = yPosition;
        XPosition = xPosition;
        Function1 = function1;
        Function2 = function2;
        ButtonYLocations.Add(YPosition);
        ButtonXLocations.Add(XPosition);
        ButtonLocations.Add((XPosition, YPosition));
        Buttons.Add(this);
    }
    public Button(ConsoleColor color, string text, int yPosition, Action function) : this(color, text, yPosition, 0, function,() => { })
    {

    }
    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Action function) : this(color, text, yPosition, xPosition, function, () => { })
    {

    }
    public Button(ConsoleColor color, string text, int yPosition, Action function1, Action function2) : this(color, text, yPosition, 0, function1, function2)
    {

    }
    public void Activate()
    {
        Function1();
        Function2();
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