class Flight
{
    public static List<Flight> Flights { get; set; } = new();
    public int ID { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public string Airplane { get; set; }


    public Flight(string airplane, string origin, string destination, DateTime date)
    {
        ID = Flights.Count;
        Airplane = airplane;
        Origin = origin;
        Destination = destination;
        Date = date;
        Flights.Add(this);
    }
}
