class ReservationDataPacket
{
    public Flight Flight { get; set; }
    public List<Seat> Seats { get; set; }

    public ReservationDataPacket(Flight flight, List<Seat> seats)
    {
        Flight = flight;
        Seats = seats;
    }
}
