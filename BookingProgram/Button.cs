using System.Reflection.Metadata.Ecma335;

class Button
{
    // Static lists to store positions and instances of all created buttons
    public static List<(int x, int y)> ButtonLocations = new();
    public static List<Button> Buttons = new();

    private readonly Action _function;
    private readonly string _referenceSide;
    private readonly Window _reference;
    // Readonly properties for button text color and button text
    public ConsoleColor Color { get; }
    public string Text { get; }
    public string ReferenceSide { get { return _referenceSide; } }
    public Window Reference { get { return _reference; } }
    public int TrueYPosition { get { switch (_referenceSide)
                                        {
                                            case "top":
                                                return _reference.ReferenceHeight + 1 + RelativeYPosition;
                                            case "bottom":
                                                return _reference.ReferenceHeight + _reference.Height - 2 - RelativeYPosition;
                                            default:
                                                return 0;
                                        }
                                    }
                                }
    public int TrueXPosition { get { return RelativeXPosition + 1; } }

    // Properties to store button position and associated function
    public int RelativeYPosition { get; set; }
    public int RelativeXPosition { get; set; }
    
    // Main constructor for creating a button with all properties
    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Window reference, string referenceSide, Action function)
    {
        Color = color;
        Text = text;
        RelativeYPosition = yPosition;
        RelativeXPosition = xPosition;
        _function = function;
        _reference = reference;
        _referenceSide = referenceSide;

        // Add button positions and instance to the static lists
        int tempRelativeY = 0;
        if (_referenceSide == "bottom") tempRelativeY = reference.Height - RelativeYPosition;
        ButtonLocations.Add((RelativeXPosition, tempRelativeY));
        Buttons.Add(this);
    }

    // Overloaded constructors with different sets of parameters

    public Button(string text, int yPosition, Window reference, string referenceSide, Action function) : 
        this(ConsoleColor.White, text, yPosition, 0, reference, referenceSide, function) { }
    public Button(string text, int yPosition, Window reference, Action function) :
        this(ConsoleColor.White, text, yPosition, 0, reference, "top", function) { }
    public Button(string text, int yPosition, int xPosition, Window reference, Action function) : 
        this(ConsoleColor.White, text, yPosition, xPosition, reference, "top", function) { }
    public Button(string text, int yPosition, int xPosition, Window reference, string referenceSide, Action function) :
        this(ConsoleColor.White, text, yPosition, xPosition, reference, referenceSide, function) { }
    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Window reference, Action function) :
        this(color, text, yPosition, xPosition, reference, "top", function) { }
    public Button(ConsoleColor color, string text, int yPosition, Window reference, string referenceSide, Action function) :
        this(color, text, yPosition, 0, reference, referenceSide, function) { }
    public Button(ConsoleColor color, string text, int yPosition, Window reference, Action function) : 
        this(color, text, yPosition, 0, reference, "top", function) { }

    // Method to activate the button by calling its associated function
    public void Activate()
    {
        _function();
    }

    // Static method to remove a button from the static lists and set its reference to null
    private void Remove()
    {
        int index = Buttons.IndexOf(this);
        ButtonLocations.RemoveAt(index);
        Buttons.Remove(this);
    }

    // Static method to clear all buttons by removing them from the static lists and setting their references to null
    public static void Clear()
    {
        List<Button> tempButtons = new(Buttons);
        foreach (Button button in tempButtons)
        {
            button.Remove();
        }
    }
}
