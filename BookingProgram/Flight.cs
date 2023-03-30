using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

class Flight
{
    public int ID;
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Date;
    public Plane Airplane { get; set; }

    public Flight(string origin, string destination, string date, Plane airplane)
    {
        Origin = origin;
        Date = date;
        Destination = destination;
        Airplane = airplane;
    }

    public void GiveID()
    {
        int id = 1;

        var JsonText = File.ReadAllText("Flights.json");
        var flights = JsonConvert.DeserializeObject<List<Flight>>(JsonText);
        foreach (var flight in flights)
        {
            id++;
            ID = id;
        }
    }
}
