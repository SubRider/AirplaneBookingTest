using System.Collections.Generic;

static class Renderer
{
    private static Button? _selectedButton;
    public static Button? SelectedButton { get { return _selectedButton; } }

    public static void ShowWindow(Window window)
    {
        int totalReferenceHeight = window.ReferenceHeight;

        Console.SetCursorPosition(0, 0 + totalReferenceHeight);
        string border = "";
        border += "╔";
        for (int w = 1; w < window.Width - 1; w++) border += "═";
        border += "╗\n";

        for (int h = 1; h < window.Height - 1; h++)
        {
            string spaces = new(' ', (window.Width - 2));
            border += "║" + spaces + "║";
        }

        border += "╚";
        for (int w = 1; w < window.Width - 1; w++) border += "═";
        border += "╝";
        Console.Write(border);
        if (Button.Buttons.Count > 0)
        {
            
            ShowButtons(window);
            ShowText(window);
        } 
        window.Updated = false;
    }
    public static void ShowWindows()
    {
        if (Window.Windows.Count > 0) 
        {
            Window.Windows[0].Update();
            if (Window.Windows[0].Updated)
            {
                bool cleared = false;
                foreach (Window window in Window.Windows)
                {
                    window.Update();
                    if (!cleared) Console.Clear();
                    ShowWindow(window);
                    cleared = true;
                }
                try
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                }
                catch { }
            }
        }
    }

    public static void ShowButton(Button button)
    {
        if (button.TrueYPosition >= button.Reference.ReferenceHeight + button.Reference.Height - 1) return;
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        Console.ForegroundColor = button.Color;
        Console.BackgroundColor = (button == _selectedButton) ? ConsoleColor.DarkBlue : ConsoleColor.Black;
        Console.Write(button.Text);
        Console.ResetColor();
    }

    public static void ShowButtons(Window window)
    {
        foreach (Button button in Button.Buttons.Where(b => b.Reference == window))
        {
            ShowButton(button);
        }
    }
    public static void ShowText(Window window)
    {
        if (window.Text == null) return;
        Console.SetCursorPosition(1, 1);
        string[] words = window.Text.Split(' ');
        foreach (string word in words)
        {

            if (Console.CursorLeft + word.Length >= window.Width - 1) Console.Write("\n║");
            Console.Write(word + ' ');
        }
    }

    public static void ShowSeat(Seat seat)
    {
        int offset = seat.SeatClass switch
        {
            "First" => 0,
            "Business" => 1,
            "Economy" => 2
        } ;
        if (seat.SeatNumber == 1)
        {
            Console.SetCursorPosition((seat.RowNumber - 1) * 3 + offset, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(seat.RowNumber);
        }

        Button button = new((seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green, "■", seat.SeatNumber + 1, (seat.RowNumber - 1) * 3 + offset, new Window(), () => 
        {   seat.Booked = true;BookingMenu.Seats.Add(seat);
            if (BookingMenu.Seats.Count >= BookingMenu.AmountOfSeatsReserved) { BookingMenu.Reserving(); } } ); 
    }

    public static void ShowSeats(List<Seat> seats)
    {
        foreach(Seat seat in seats) 
        {
            ShowSeat(seat);
        }
    }

    public static void HighlightButton(Button button, bool dehilight = false)
    {
        if (dehilight)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
            Console.ForegroundColor = button.Color;
            Console.Write(button.Text);
            Console.ResetColor();
            Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
            _selectedButton = null;
            return;
        }
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        Console.ForegroundColor = button.Color;
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write(button.Text);
        Console.ResetColor();
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        _selectedButton = button;     
    }

    public static void ClearLine()
    {
        Console.SetCursorPosition(1, Console.CursorTop);
        string wipeLine = new(' ', Console.WindowWidth - 2);
        Console.Write(wipeLine);
    }

    public static void ClearLines()
    {
        for (int h = 1; h <= Console.WindowHeight * 0.85 - 2; h++)
        {
            Console.SetCursorPosition(1, h);
            ClearLine();
        }
    }
    public static void Clear()
    {
        InputChecker.Clear();
        Window.Clear();
        Button.Clear();
        Console.ResetColor(); 
        Console.Clear();
    }

    public static void Refresh()
    {
        InputChecker.Clear();
        foreach (Button button in Button.Buttons.Where(b => b.KeepAfterRefresh = false)) button.Remove();
        ClearLines();
    }
}
