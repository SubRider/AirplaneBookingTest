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
        Console.SetCursorPosition(0, 3);
        foreach (ReservationDataPacket reservation in ReservationDataPacket.Reservations)
        {        
            if (reservation.CustomerID == user.Id)
            {
                Flight flight = Flight.Flights.Find(i => i.ID == reservation.FlightID);
                Console.WriteLine($"Origin: {flight.Origin} \nDestination: {flight.Destination}" +
                                    $"\nDeparture Date: {flight.Date}");
                Console.WriteLine("Seats:");
                foreach (Seat seat in reservation.Seats)
                {   
                    Console.WriteLine($"Row: {seat.RowNumber} Seat: {seat.SeatLetter}");
                }
            }
            Console.WriteLine();
        }
    }
}