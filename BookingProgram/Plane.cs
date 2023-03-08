class Plane
{
    public int NumberOfSeats { get; set; }
    public string Model { get; set; }

    public Plane(int numberOfSeats, string model)
    {
        NumberOfSeats = numberOfSeats;
        Model = model;
    }
}
