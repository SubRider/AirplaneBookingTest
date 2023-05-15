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
}
