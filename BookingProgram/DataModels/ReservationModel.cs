class ReservationModel
{
    public static List<ReservationModel> Reservations { get; set; } = new();
    public int ReservationID { get; set; }
    public List<Seat> Seats { get; set; }
    public int FlightID { get; set; }
    public int CustomerID { get; set; }

    public ReservationModel(List<Seat> seats, int flightID, int customerID)
    {
        
        Seats = seats;
        FlightID = flightID;
        CustomerID = customerID;
        ReservationID = Reservations.Count;
        Reservations.Add(this);
    }

    public void RemoveFlight(List<ReservationModel> Reservations, int flightID)
    {
        List<Seat> seatsCopy = new List<Seat>(BookingMenu.Seats);
        foreach (Seat seat in seatsCopy)
        {
            BookingMenu.Seats.Remove(seat);
        }
        Reservations.RemoveAll(reservation => reservation.FlightID == flightID);
        UpdateJsonData(Reservations);
        Renderer.Clear();
        Console.WriteLine("Succesfully canceled");
        Thread.Sleep(2000);
        BookingMenu.StartScreen();
    }

    public void UpdateJsonData(List<ReservationModel> reservations)
    {
        string path = "Reservations.json";
        JsonCommunicator.Write(path, reservations);
    }
}