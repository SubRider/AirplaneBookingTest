class InputButton: Button
{
    public static List<InputButton> InputButtons = new();
    private string _inputName;
    private string _input;
    public override string Text { get { return _inputName + ": " + _input; } set { _input = value; } }
    public string Input { get { return _input; } set { _input = value; } }
    public InputButton(string inputName, int yPosition, Window reference, string referenceSide) : base("     ", yPosition, reference, referenceSide, () => { }) 
    {
        _inputName = inputName;
        Input = "";
        InputButtons.Add(this);
    }
    public InputButton(string inputName, int yPosition, Window reference) : this(inputName, yPosition, reference, "top") { }
}
