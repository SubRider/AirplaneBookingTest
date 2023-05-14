class InputButton: Button
{
    public InputButton(string inputName, int yPosition, Window reference) : base(inputName + ":        ", yPosition, reference, () => { }) { }

    public void ReadInput()
    {

    }
}
