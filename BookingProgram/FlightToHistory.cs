using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
class FlightToHistory
{
    private Flight flight;
    private UserAccount user;
    int ID = 1;

    public void AddToHistory(Flight flight, UserAccount user)
    {
        ID += 1;
        string Filepath = "FlightHistory.json";
        string username = user.Email;
        string origin = flight.Origin;
        string destination = flight.Destination;
        string date = flight.Date;
        

        string json = File.ReadAllText(Filepath);
        JArray jArray = JsonConvert.DeserializeObject<JArray>(json);

        var FlightData = new JObject();
        FlightData.Add("ID", ID.ToString());
        FlightData.Add("User", username);
        FlightData.Add("Origin", origin);
        FlightData.Add("Destination", destination);
        FlightData.Add("Date", date);

        jArray.Add(FlightData);

        json = JsonConvert.SerializeObject(jArray, Formatting.Indented);
        File.WriteAllText(Filepath, json);
    }
}