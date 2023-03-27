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
        foreach (Seat seat in FirstClassSeats) ShowSeat(seat, 0);
        foreach (Seat seat in BusinessClassSeats) ShowSeat(seat, 1);
        foreach (Seat seat in EconomyClassSeats) ShowSeat(seat, 2);
        Renderer.ShowButtons();
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
