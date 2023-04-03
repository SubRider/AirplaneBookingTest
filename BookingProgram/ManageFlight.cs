using System.IO;
using Newtonsoft;
using Newtonsoft.Json;


class ManageFlights
{
    public Flight Flight;
    public ManageFlights(Flight flight)
    {
        Flight = flight;
    }

    public void AddFlight()
    {
        string path = "Flights.json";
        List<Flight> listOfFlights = new List<Flight>() {};

        var JsonText = File.ReadAllText(path);
        if (JsonText != null)
        {
            listOfFlights = JsonConvert.DeserializeObject<List<Flight>>(JsonText);
        }

        listOfFlights.Add(Flight);
        
        var Json = JsonConvert.SerializeObject(listOfFlights, Formatting.Indented);
        File.WriteAllText(path, Json);
    }

    public void RemoveFlightByID(int id)
    {
        string path = "Flights.json";

        var JsonText = File.ReadAllText(path);
        var listOfFlights = JsonConvert.DeserializeObject<List<Flight>>(JsonText);

        foreach (var flight in listOfFlights)
        {
            if (id == flight.ID)
            {
                listOfFlights.Remove(flight);
                break;
            }
        }

        var Json = JsonConvert.SerializeObject(listOfFlights, Formatting.Indented);
        File.WriteAllText(path, Json);
    }
}