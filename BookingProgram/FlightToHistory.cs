using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
static class FlightToHistory
{
    static private Flight flight;
    static private AccountModel user;
    static int ID = 1;

    public static void AddToHistory(Flight flight, AccountModel user)
    {
        ID += 1;
        string Filepath = "FlightHistory.json";
        string username = user.EmailAddress;
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
    // Accountmodel vervangen
    public static void ViewHistory(AccountModel user)
    {
        string username = user.EmailAddress;
        string jsonString = File.ReadAllText("FlightHistory.json");
        List<Dictionary<string, string>> list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);
        foreach (Dictionary<string, string> dict in list)
        {
            foreach (KeyValuePair<string, string> field in dict)
            {
                if (user.EmailAddress == dict["User"])
                {
                    Console.WriteLine("{0}: {1}", field.Key, field.Value);
                }
            }
        }
    }
}