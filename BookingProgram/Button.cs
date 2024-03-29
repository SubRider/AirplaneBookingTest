﻿class Button
{
    // Static lists to store positions and instances of all created buttons
    public static List<Func<(int x, int y)>> ButtonLocations = new();
    public static List<Button> Buttons = new();

    private readonly int _relativeYPosition;
    private readonly int _relativeXPosition;
    private readonly Action _function;
    private readonly string _referenceSide;
    private readonly Window _reference;
    // Readonly properties for button text color and button text
    public ConsoleColor Color { get; set; }
    public virtual string Text { get; set; }
    public string ReferenceSide { get { return _referenceSide; } }
    public Window Reference { get { return _reference; } }
    public bool KeepAfterRefresh { get; set; }
    public bool Selectable { get; set; }
    public bool Visible { get; set; }
    public int TrueYPosition { get { switch (_referenceSide)
                                        {
                                            case "top":
                                                return _reference.ReferenceHeight + 1 + _relativeYPosition;
                                            case "bottom":
                                                return _reference.ReferenceHeight + _reference.Height - 2 - _relativeYPosition;
                                            default:
                                                return _reference.ReferenceHeight + 1 + _relativeYPosition;
                                        }
                                    }
                                }
    public int TrueXPosition { get { switch (_referenceSide) 
            {
                case "left":
                    int leftButtons = 0;
                    foreach (Button button in _reference.Buttons.Where(b => b.ReferenceSide == "left")) leftButtons++;
                    if (leftButtons > 0) return (_reference.Width / leftButtons) * _relativeXPosition + 1;
                    else return (_reference.Width * _relativeXPosition + 1);
                default:
                    if (_relativeXPosition > 0) return _relativeXPosition * 3;
                    else return _relativeXPosition + 1;
        } } }

    // Main constructor for creating a button with all properties
    public Button(ConsoleColor color, string text, int yPosition, int xPosition, Window reference, string referenceSide, Action function, bool selectable = true)
    {
        Color = color;
        Text = text;
        _relativeYPosition = yPosition;
        _relativeXPosition = xPosition;
        _function = function;
        _reference = reference;
        _referenceSide = referenceSide;
        Selectable = selectable;
        Visible = true;

        // Add button positions and instance to the static lists
        Func<(int x, int y)> positionDelegate = () => (TrueXPosition, TrueYPosition);
        if (Selectable) ButtonLocations.Add(positionDelegate);
        _reference.Buttons.Add(this);
        Buttons.Add(this);
    }

    // Overloaded constructors with different sets of parameters

    public Button(string text, int yPosition, Window reference, string referenceSide, Action function) : 
        this(ConsoleColor.White, text, yPosition, 0, reference, referenceSide, function) { }
    public Button(string text, int yPosition, Window reference, Action function) :
        this(ConsoleColor.White, text, yPosition, 0, reference, "top", function) { }
    public Button(string text, int yPosition, Window reference, Action function, bool selectable) :
        this(ConsoleColor.White, text, yPosition, 0, reference, "top", function, selectable)
    { }
    public Button(string text, int yPosition, int xPosition, Window reference, Action function) : 
        this(ConsoleColor.White, text, yPosition, xPosition, reference, "top", function) { }
    public Button(string text, int yPosition, int xPosition, Window reference, Action function, bool selectable) :
    this(ConsoleColor.White, text, yPosition, xPosition, reference, "top", function, selectable)
    { }
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

    public int FindIndex()
    {
        return Buttons.IndexOf(this);
    }
    public int FindLocationIndex()
    {
        return ButtonLocations.FindIndex(positionDelegate =>
        {
            (int x, int y) position = positionDelegate.Invoke();
            return position.x == TrueXPosition && position.y == TrueYPosition;
        });
    }

    // Static method to remove a button from the static lists and set its reference to null
    public virtual void Remove()
    {
        int index = Buttons.IndexOf(this);
        if (Selectable) ButtonLocations.RemoveAt(FindLocationIndex());
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
