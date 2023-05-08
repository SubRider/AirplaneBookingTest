class Window
{
    public static List<Window> Windows = new();
    private int _previousWidth = 0;
    private int _previousHeight = 0;
    private int _width;
    private int _height;
    private readonly double _xScale;
    private readonly double _yScale;
    private readonly Window? _reference;
    private readonly List<Button> _buttons = new();
    public int Width { get { return _width; } }
    public int Height { get { return _height; } }
    public Window? Reference { get { return _reference; } }
    public int ReferenceHeight { get; set; }
    public List<Button> Buttons { get { return _buttons; } }
    public bool Updated { get; set; }
    
    public Window(double xScale, double yScale, Window? reference, List<Button> buttons, bool keepObject)
    {
        _xScale = xScale;
        _yScale = yScale;
        _width = (int)(Console.WindowWidth * _xScale);
        _height = (int)(Console.WindowHeight * _yScale);
        _reference = reference;
        ReferenceHeight = (_reference != null) ? _reference.Height + _reference.ReferenceHeight : 0;
        Updated = true;
        if (keepObject) Windows.Add(this);
    }
    public Window(double xScale, double yScale, Window? reference, List<Button> buttons): this(xScale, yScale, reference, buttons, true) { }
    public Window(double xScale, double yScale, Window reference) : this(xScale, yScale, reference, new(), true) { }
    public Window(double xScale, double yScale, List<Button> buttons) : this(xScale, yScale, null, buttons, true) { }
    public Window(double xScale, double yScale, bool keepObject) : this(xScale, yScale, null, new(), keepObject) { }
    public Window(double xScale, double yScale) : this(xScale, yScale, null, new(), true) { }
    public Window(Window reference, List<Button> buttons) : this(1, 1, reference, buttons, true) { }
    public Window(): this(1, 1, null, new(), true) { }

    public void Update()
    {
        _width = (int)(Console.WindowWidth * _xScale);
        _height = (int)(Console.WindowHeight * _yScale);
        if (_width != _previousWidth || _height != _previousHeight)
        {
            Updated = true;
            _previousWidth = _width;
            _previousHeight = _height;
            ReferenceHeight = (_reference != null) ? _reference.Height + _reference.ReferenceHeight : 0;
        }
    }
    private void Remove()
    {
        Windows.Remove(this);
    }
    public static void Clear()
    {
        List<Window> tempWindows = new(Windows);
        foreach(Window window in tempWindows)
        {
            window.Remove();
        }
    }
}
