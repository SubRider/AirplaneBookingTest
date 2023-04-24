class ReservationDataPacket
{
    public static List<ReservationDataPacket> Reservations { get; set; } = new();
    public int ReservationID { get; set; }
    public List<Seat> Seats { get; set; }
    public int FlightID { get; set; }
    public int CustomerID { get; set; }

    public ReservationDataPacket(List<Seat> seats, int flightID, int customerID)
    {
        
        Seats = seats;
        FlightID = flightID;
        CustomerID = customerID;
        ReservationID = Reservations.Count;
        Reservations.Add(this);
    }
}
