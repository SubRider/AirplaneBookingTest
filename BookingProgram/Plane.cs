﻿class Plane
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
