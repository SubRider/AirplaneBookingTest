class Flight
{
    public Plane Airplane { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }

    public Flight(Plane airplane, string origin, string destination)
    {
        Airplane = airplane;
        Origin = origin;
        Destination = destination;
    }
}
