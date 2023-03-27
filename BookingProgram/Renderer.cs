﻿using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

static class Renderer
{
    public static void ShowButton(Button button)
    {
        Console.SetCursorPosition(button.XPosition, button.YPosition);
        Console.ForegroundColor = button.Color;
        Console.Write(button.Text);
        Console.ResetColor();
    }
    public static void ShowButtons()
    {
        foreach (Button button in Button.Buttons)
        {
            ShowButton(button);
        }
    }
    public static void ShowSeat(Seat seat, int offset)
    {
        if (seat.SeatNumber == 1)
        {
            Console.SetCursorPosition((seat.RowNumber - 1) * 3 + offset, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(seat.RowNumber);
        }

        ConsoleColor seatColor = (seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green;
        Button button = new(seatColor, "■", seat.SeatNumber + 1, (seat.RowNumber - 1) * 3 + offset, () => Console.ResetColor());
    }
    public static void ShowSeats(List<Seat> seats, int offset)
    {
        foreach(Seat seat in seats) 
        {
            ShowSeat(seat, offset);
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
            button.HighlightTime = DateTime.MinValue;
        }
        TimeSpan timeHighlighed = DateTime.Now - button.HighlightTime;
        if (timeHighlighed.TotalMilliseconds > 500)
        {
            if (button.Highlighted)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                button.Highlighted = false;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                button.Highlighted = true;
            }
            button.HighlightTime = DateTime.Now;
            Console.SetCursorPosition(button.XPosition, button.YPosition);
            Console.ForegroundColor = button.Color;
            Console.Write(button.Text);
            Console.ResetColor();
            Console.SetCursorPosition(button.XPosition, button.YPosition);
        }
    }
}