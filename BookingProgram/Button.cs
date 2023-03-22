class Button
{
    public static List<int> ButtonYLocations = new();
    public static List<(int, int)> ButtonLocations = new();
    public static List<Button> Buttons = new();
    private readonly ConsoleColor Color;
    private readonly string Text;
    private readonly string Type;
    public int YPosition;
    public int XPosition;
    public Button(ButtonTemplate buttonTemplate, int yPosition)
    {
        Color = buttonTemplate.Color;
        Text = buttonTemplate.Text;
        Type = buttonTemplate.Type;
        YPosition = yPosition;
        ButtonYLocations.Add(YPosition);
        Buttons.Add(this);
    }
    public Button(ButtonTemplate buttonTemplate, int yPosition, int xPosition)
    {
        Color = buttonTemplate.Color;
        Text = buttonTemplate.Text;
        Type = buttonTemplate.Type;
        YPosition = yPosition;
        XPosition = xPosition;
        ButtonLocations.Add((XPosition, YPosition));
        Buttons.Add(this);
    }
    public void Show()
    {
        Console.SetCursorPosition(0, YPosition);
        Console.ForegroundColor = Color;
        Console.Write(Text);
        Console.ResetColor();
        Console.SetCursorPosition(0, YPosition);
    }
    public void Activate()
    {
        switch (Type)
        {
            case "start":
                ConsoleApp.MainMenu();
                break;
            case "exit":
                ConsoleApp.Quit = true;
                break;
            case "account":
                break;
            case "seat":
                break;
        }
    }
}