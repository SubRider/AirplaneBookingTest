using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

class Flight : IHasID
{
    public static List<Flight> Flights { get; set; } = new();
    public int ID { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string DepartureDate { get; set; }
    public string ArrivalDate { get; set; }
    public int AirplaneID { get; set; }
    public List<Seat> FirstClassSeats { get; set; }
    public List<Seat> BusinessClassSeats { get; set; }
    public List<Seat> EconomyClassSeats { get; set; }

    public Flight(string origin, string destination, string departureDate, string arrivalDate, int airplaneID)
    {
        Origin = origin;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        Destination = destination;
        AirplaneID = airplaneID;
        string model = Airplane.Planes.Find(plane => plane.ID == airplaneID).Model;
        ID = Flights.Count;
        switch (model)
        {
            case "737":
                FirstClassSeats = LoadSeats(4, 4, 0, "First");
                BusinessClassSeats = LoadSeats(6, 8, 4, "Business");
                EconomyClassSeats = LoadSeats(6, 17, 12, "Economy");
                break;
            case "787":
                FirstClassSeats = LoadSeats(6, 8, 0, "First");
                BusinessClassSeats = LoadSeats(9, 9, 8, "Business");
                EconomyClassSeats = LoadSeats(9, 14, 17, "Economy");
                break;
            case "A330":
                FirstClassSeats = LoadSeats(6, 1, 0, "First");
                BusinessClassSeats = LoadSeats(6, 3, 1, "Business");
                EconomyClassSeats = LoadSeats(8, 33, 4, "Economy");
                break;
            default:
                FirstClassSeats = LoadSeats(6, 5, 0, "First");
                BusinessClassSeats = LoadSeats(6, 10, 5, "Business");
                EconomyClassSeats = LoadSeats(6, 20, 15, "Economy");
                break;
        }
        Flights.Add(this);
    }
    
    public List<Seat> LoadSeats(int rowSize, int amountOfRows, int prevClassRows, string _class)
    {
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

    public static Flight FindByID(int id)
    {
        return Flights.Find(f => f.ID == id);
    }

    public int FindIndex()
    {
        return Flights.IndexOf(this);
    }

    public override string ToString()
    {
        return $"ID: {ID} Origin: {Origin} Destination: {Destination} Departure date: {DepartureDate} " +
               $"Arrival date: {ArrivalDate} Airplane ID: {AirplaneID} ";
    }
}
