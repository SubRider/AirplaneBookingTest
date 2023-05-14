using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

class Flight : IHasID
{
    public static List<Flight> Flights { get; set; } = new();
    public int ID { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Date { get; set; }
    public int AirplaneID { get; set; }

    public Flight(string origin, string destination, string date, int airplaneID)
    {
        Origin = origin;
        Date = date;
        Destination = destination;
        AirplaneID = airplaneID;
        ID = Flights.Count;
        Flights.Add(this);
    }

    public void GiveID()
    {
        int id = 1;

        var JsonText = File.ReadAllText("Flights.json");
        var flights = JsonConvert.DeserializeObject<List<Flight>>(JsonText);
        if (flights != null)
        {
            foreach (var flight in flights)
            {
                id++;
                ID = id;
            }
        }
    }

    public override string ToString()
    {
        return $"Origin: {Origin} Destination: {Destination} ID: {ID}";
    }
}
