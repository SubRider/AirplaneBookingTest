class Plane
{
    public string Model { get; set; }
    public List<Seat> FirstClassSeats { get; set; }
    public List<Seat> BusinessClassSeats { get; set; }
    public List<Seat> EconomyClassSeats { get; set; }

    public Plane(string model)
    {
        Model = model;
        FirstClassSeats = LoadSeats(6, 10, 0, "First");
        BusinessClassSeats = LoadSeats(6, 10, 10, "Business");
        EconomyClassSeats = LoadSeats(6, 10, 20, "Economy");
    }
    public void ShowSeats()
    {
        Console.Clear();
        foreach (Seat seat in FirstClassSeats)
        {

            int cursorX = seat.RowNumber * 3;
            if (seat.SeatNumber == 1)
            {
                Console.SetCursorPosition(cursorX - 1, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(seat.RowNumber);
            }
            int cursorY = seat.SeatNumber + 1;
            Console.SetCursorPosition(cursorX - 1,cursorY - 1);
            ConsoleColor seatColor = (seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.ForegroundColor = seatColor;
            Console.Write("▀");
        }
        foreach (Seat seat in BusinessClassSeats)
        {
            int cursorX = seat.RowNumber * 3 + 1;
            if (seat.SeatNumber == 1)
            {
                Console.SetCursorPosition(cursorX - 1, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(seat.RowNumber);
            }
            int cursorY = seat.SeatNumber + 1;
            Console.SetCursorPosition(cursorX - 1, cursorY - 1);
            ConsoleColor seatColor = (seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.ForegroundColor = seatColor;
            Console.Write("▀");
        }
        foreach (Seat seat in EconomyClassSeats)
        {
            int cursorX = seat.RowNumber * 3 + 2;
            if (seat.SeatNumber == 1)
            {
                Console.SetCursorPosition(cursorX - 1, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(seat.RowNumber);
            }
            int cursorY = seat.SeatNumber + 1;
            Console.SetCursorPosition(cursorX - 1, cursorY - 1);
            ConsoleColor seatColor = (seat.Booked) ? ConsoleColor.Red : ConsoleColor.Green;
            Console.ForegroundColor = seatColor;
            Console.Write("▀");
        }
        Console.ResetColor();
    }
    public static List<Seat> LoadSeats(int rowSize, int amountOfRows, int prevClassRows, string _class)
    {
        //Add getSeatsFromJson
        List<Seat> seats = new();
        for (int row = 1; row <= amountOfRows; row++)
        {
            for (int seatNumber = 1; seatNumber <= rowSize; seatNumber++)
            {
                Seat seat = new(_class, row + prevClassRows, seatNumber);
                seats.Add(seat);
            }
        }
        return seats;
    }


}
