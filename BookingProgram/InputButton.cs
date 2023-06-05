class InputButton: Button
{
    public static List<InputButton> InputButtons = new();
    private readonly string _inputName;
    private string _input;
    public override string Text { get { return _inputName + ": " + _input; } set { _input = value; } }
    public string Input { get { return _input; } set { _input = value; } }
    public InputButton(string inputName, int yPosition, Window reference, string referenceSide, Action function) : base("     ", yPosition, reference, referenceSide, function) 
    {
        _inputName = inputName;
        Input = "";
        InputButtons.Add(this);
    }
    public InputButton(string inputName, int yPosition, Window reference, Action function) : this(inputName, yPosition, reference, "top", function) { }
    public InputButton(string inputName, int yPosition, Window reference) : this(inputName, yPosition, reference, "top", () => { }) { }

    public override void Remove()
    {
        int index = Buttons.IndexOf(this);
        ButtonLocations.RemoveAt(index);
        Buttons.Remove(this);
        InputButtons.Remove(this);
    }

    // Static method to clear all buttons by removing them from the static lists and setting their references to null
    public new static void Clear()
    {
        List<InputButton> tempButtons = new(InputButtons);
        foreach (InputButton button in tempButtons)
        {
            button.Remove();
        }
    }
}
