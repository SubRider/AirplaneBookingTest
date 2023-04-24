using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

static class Renderer
{
    private static Button? _selectedButton;
    public static void ShowButton(Button button)
    {
        //if (_selectedButton == (button.XPosition, button.YPosition)) button.Highlight();
        Console.SetCursorPosition(button.XPosition, button.YPosition);
        Console.ForegroundColor = button.Color;
        Console.BackgroundColor = button.HighlightColor;
        Console.Write(button.Text);
        Console.ResetColor();
    }

    public static void ShowButtons()
    {
        foreach (Button button in Button.Buttons)
        {
            ShowButton(button);
        }
        InputChecker.JumpToButton(0);
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

        ConsoleColor seatColor = (seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green;
        Button button = new(seatColor, "■", seat.SeatNumber + 1, (seat.RowNumber - 1) * 3 + offset, () => 
        { seat.Booked = true;BookingMenu.Seats.Add(seat); if (BookingMenu.Seats.Count >= BookingMenu.AmountOfSeatsReserved) { BookingMenu.Reserving(); } } ); 
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
            Console.SetCursorPosition(button.XPosition, button.YPosition);
            Console.ForegroundColor = button.Color;
            Console.Write(button.Text);
            Console.ResetColor();
            Console.SetCursorPosition(button.XPosition, button.YPosition);
            _selectedButton = null;
            return;
        }
        Console.SetCursorPosition(button.XPosition, button.YPosition);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = button.Color;
        Console.Write(button.Text);
        Console.ResetColor();
        Console.SetCursorPosition(button.XPosition, button.YPosition);
        _selectedButton = button;
    }
    public static void Clear()
    {
        _selectedButton = null;
        Button.Clear();
        Console.ResetColor(); 
        Console.Clear();
    }
}
