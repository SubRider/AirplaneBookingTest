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

    public static int ShowButton(Button button)
    {
        if (button.TrueYPosition >= button.Reference.ReferenceHeight + button.Reference.Height - 1) return button.FindIndex();
        Console.SetCursorPosition(button.TrueXPosition, button.TrueYPosition);
        Console.ForegroundColor = button.Color;
        Console.BackgroundColor = (button == _selectedButton) ? ConsoleColor.DarkBlue : ConsoleColor.Black;
        Console.Write(button.Text);
        Console.ResetColor();
        return -1;
    }

    public static int ShowButtons(Window window)
    {
        int finalIndex = -1;
        foreach (Button button in Button.Buttons.Where(b => b.Reference == window))
        {
            int index = ShowButton(button);
            if (index != -1) finalIndex = index;
        }
        return finalIndex;
    }
    public static void ShowText(Window window, string? text = null)
    {
        text ??= window.Text;
        if (text == null) return;
        Console.SetCursorPosition(1, 1);
        string[] words = text.Split(' ');
        string writtenWords = "";
        string wordOverflow = "";
        foreach (string word in words)
        {
            if (Console.CursorTop >= window.Height - 5) { wordOverflow += word + " "; continue; }
            if (Console.CursorLeft + word.Length >= window.Width - 1) Console.Write("\n║");
            else if (word.Contains('^')) Console.Write("\n║");
            else { Console.Write(word + ' '); writtenWords += word + ' '; }
        }
        wordOverflow.Replace(writtenWords, "");
        if (wordOverflow.Length > 0)
        {
            _ = new Button("Previous page", 1, window, "bottom", () =>
            {
                ClearLines();
                window.PreviousText.Replace(writtenWords, "");
                window.NextText += writtenWords;
                ShowText(window, window.PreviousText);
            });
            _ = new Button("Next page", 0, window, "bottom", () => 
            {
                ClearLines();
                window.PreviousText += writtenWords;
                window.NextText = wordOverflow;
                ShowText(window, window.NextText);
            });
            
        }
        ShowButtons(window);
    }

    public static void ShowSeat(Seat seat, Window window)
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

        Button button = new((seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green, "■", seat.SeatNumber + 1, (seat.RowNumber - 1) * 3 + offset, window, () => 
        {   seat.Booked = true;BookingMenu.Seats.Add(seat);
            if (BookingMenu.Seats.Count >= BookingMenu.AmountOfSeatsReserved) { BookingMenu.Reserving(); } } ); 
    }

    public static void ShowSeats(List<Seat> seats, Window window)
    {
        foreach(Seat seat in seats) 
        {
            ShowSeat(seat, window);
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
        InputButton.Clear();
        Button.Clear();
        Console.ResetColor(); 
        Console.Clear();
    }

}
