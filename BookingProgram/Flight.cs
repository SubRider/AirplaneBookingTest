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

    public Flight(string origin, string destination, string departureDate, string arrivalDate, int airplaneID)
    {
        Origin = origin;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        Destination = destination;
        AirplaneID = airplaneID;
        ID = Flights.Count;
        Flights.Add(this);
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