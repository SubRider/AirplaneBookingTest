using System.Reflection;
using System.Security.Cryptography;

static class BookingMenu
{
    public static bool Quit = false;
    public static string ReservationChoice;
    public static int FlightID;
    public static Plane CurrentPlane;
    public static int AmountOfSeatsReserved;
    public static string SingleOrRetour;
    public static bool MenuUpdated;
    public static List<Seat> Seats = new();
    static void Main()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Plane.Planes = JsonCommunicator.Read<Plane>("Planes.json");
        ReservationDataPacket.Reservations = JsonCommunicator.Read<ReservationDataPacket>("reservations.json");

        Console.CursorVisible = false;

        StartScreen();
        while (!Quit)
        {
            InputChecker.CheckInput();
            Renderer.ShowWindows();
            if (MenuUpdated)
            {
                
                InputChecker.JumpToButton(0);
                MenuUpdated = false;
            }
            
        }
    }
    public static void AddMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.1, reference);
        _ = new Button("Home", 0, 0, menuBar, "left", () => StartScreen());
        _ = new Button("Book Flight", 0, 1, menuBar, "left", () => ClassReservationMenu());
        _ = new Button("My Flights", 0, 2, menuBar, "left", () => History());
        _ = new Button("Account", 0, 3, menuBar, "left", () => { });
        _ = new Button("Info", 0, 4, menuBar, "left", () => AirlineInfo());
    }
    public static void StartScreen()
    {

        Renderer.Clear();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        //Thread.Sleep(4000);
        Console.Clear();
        Window w1 = new(1, 0.9);
        _ = new Button(ConsoleColor.Blue, "Sign up", 2, w1, "bottom", ()=> CreateAccount.userInfo());
        _ = new Button(ConsoleColor.Green, "Login", 1, w1, "bottom", () => UserLogin.Start());   
        _ = new Button(ConsoleColor.DarkRed, "Exit", 0, w1, "bottom", ()=> Quit = true);
        AddMenuBar(w1);
        MenuUpdated = true;    
    }

    public static void MainMenu()
    {
        Console.CursorVisible = false;
        Renderer.Clear();
        Button browse = new("Browse flights", 1, new Window(), () => ClassReservationMenu());
        Button history = new("Flight history", 2, new Window(), () => History());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void ClassReservationMenu()
    {
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination" });
        flightSearch.Activate();
        FlightID = flightSearch.ID;
        int index = Flight.Flights.FindIndex(i => i.ID == FlightID);
        CurrentPlane = Plane.Planes.Find(i => i.ID == Flight.Flights[index].AirplaneID);

        Renderer.Clear();
        Console.WriteLine("Select Class");
        Button first = new("First Class", 2, new Window(), () => { ReservationChoice = "First";RetourOrSingle(); });
        Button business = new("Business Class", 3, new Window(), () => { ReservationChoice = "Business";RetourOrSingle(); });
        Button economy = new("Economy Class", 4, new Window(), () => { ReservationChoice = "Economy";RetourOrSingle(); });
        Button back = new("back", 6, new Window(), () => MainMenu());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void RetourOrSingle()
    {
        Renderer.Clear();
        Console.WriteLine("Select if you want a retour flight or a single flight:\n");
        Button Retour = new("Retour", 2, new Window(), () => {SingleOrRetour = "Retour"; SeatsReservationMenu(); }); 
        Button Single = new("Single", 3, new Window(), () => {SingleOrRetour = "Single"; SeatsReservationMenu(); });
        Button back = new("back", 6, new Window(), () => ClassReservationMenu());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void SeatsReservationMenu()
    {
        Renderer.Clear();
        bool choosing = true;
        while (choosing)
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("How many seats? (Test Maximum: 12):");
            
            try
            {
                AmountOfSeatsReserved = Convert.ToInt32(Console.ReadLine());
                if (AmountOfSeatsReserved > 12)
                {
                    Console.WriteLine("Unable to reserve more than twelve seats");
                    Thread.Sleep(700);
                    continue;
                }

                break;
            }

            catch (Exception e)
            {
                
                Console.WriteLine("Not a valid number");
                Thread.Sleep(700);
            }
        }
        SeatMenu();
    }

    public static void SeatMenu()
    {
        Console.CursorVisible = false;
        Renderer.Clear();
        Seats = new();
        Renderer.ShowSeats(CurrentPlane.FirstClassSeats);
        Renderer.ShowSeats(CurrentPlane.BusinessClassSeats);
        Renderer.ShowSeats(CurrentPlane.EconomyClassSeats);
        Button back = new("back", 12, new Window(), () => SeatsReservationMenu());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void Reserving()
    {
        Button.Clear();
        Console.Clear();
        ReservationDataPacket reservations = new(Seats, FlightID, UserLogin.ActiveUser.Id);
        JsonCommunicator.Write("Reservations.json", ReservationDataPacket.Reservations);
        Console.WriteLine("Successfully Reserved.");
        JsonCommunicator.Write("Planes.json", Plane.Planes);
        Thread.Sleep(700);
        MainMenu();
    }
    public static void History()
    {
        Button.Clear(); 
        Console.Clear();
        Console.WriteLine("Your previous flights:\n");
        FlightToHistory.ViewHistory(UserLogin.ActiveUser);
        (int x, int y) = Console.GetCursorPosition();
        Button back = new("back", y, 20, new Window(), () => MainMenu());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void AdminMenu()
    {
        
        Button.Clear();
        Console.Clear();
        Button button0 = new("Show Flights", 0, new Window(), () => FlightListMenu());
        Button button1 = new("Add flight", 1, new Window(), () => AddFlightMenu());
        Button button2 = new("Edit flights", 2, new Window(), () => EditFlightMenu());
        Button button3 = new("Remove Flights", 3, new Window(), () => RemoveFlightMenu());
        Button back = new("back", 6, new Window(), () => UserLogin.Start());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void FlightListMenu()
    {
        Renderer.Clear();
        foreach (Flight flight in Flight.Flights)
        {
            Console.WriteLine($"Flight Number: {flight.ID +1}\nOrigin: {flight.Origin} \nDestination: {flight.Destination}" +
                                $"\nDeparture Date: {flight.Date} \nPlane ID: {flight.AirplaneID}\n");
            Console.WriteLine();
        }
        (int x, int y) = Console.GetCursorPosition();
        Button back = new("back", y, new Window(), () => AdminMenu());
        //Renderer.ShowButtons();
    }
    public static void EditFlightMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination", "ID" });
        flightSearch.Activate();
        Console.Clear();
        Button.Clear();
        foreach (Flight f in Flight.Flights)
        {
            if (f.ID == flightSearch.ID)
            {
                PropertyInfo propertyInfo = typeof(Flight).GetProperty(flightSearch.SearchCategory);
                Console.WriteLine("Give the new value:");
                var value = Console.ReadLine();
                propertyInfo.SetValue(f, value);
                break;
            }
        }
        JsonCommunicator.Write("Flights.json", Flight.Flights);
        AdminMenu();
    }
    public static void AddFlightMenu()
    {
        Button.Clear();
        Console.Clear();
        Console.Write("Origin: ");
        string origin = Console.ReadLine();
        Console.Clear();
        Console.Write("Destination: ");
        string destination = Console.ReadLine();
        Console.Clear();
        Console.Write("Departure Date: ");
        string date = Console.ReadLine();
        Flight _ = new(origin, destination, date, 01);
        JsonCommunicator.Write("Flights.json", Flight.Flights);
        AdminMenu();
    }

    public static void RemoveFlightMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination", "ID" });
        flightSearch.Activate();

        Renderer.Clear();
        List<Flight> tempFlights = new(Flight.Flights);
        foreach (Flight f in tempFlights)
        {
            if (f.ID == flightSearch.ID)
            {
                Button.Clear();
                Button yes = new("Yes", 2, new Window(), () => {
                    Flight.Flights.Remove(f);
                    JsonCommunicator.Write("Flights.json", Flight.Flights);
                    AdminMenu();
                });
                Button no = new("No", 3, new Window(), () => { AdminMenu(); });
                break;
            }
        }
        Console.Clear();
        Console.WriteLine("Are you sure you want to delete this flight?:");
        //Renderer.ShowButtons();
        
    }

    public static void AirlineInfo()
    {
        Renderer.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nAbout");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nWelcome to our airline!");
        Console.WriteLine("We are a company dedicated to providing exceptional travel experiences to our passengers. " +
                          "\nOur goal is to take you to your destination safely, comfortably, and on time. " +
                          "\nWith a team of experienced pilots, friendly cabin crew, and state-of-the-art aircraft, " +
                          "we strive to make your journey enjoyable from start to finish. " +
                          "\nWhether you're traveling for business or pleasure, " +
                          "we look forward to welcoming you on board and taking you to your next adventure.\n");

        string phoneNumber = "010 546 7465";
        string email = "info@rotterdamairlines.com";
        Console.WriteLine($"Phone Number: {phoneNumber} \nE-mail: {email}");
        Console.ResetColor();
        Button back = new("back", 12, new Window(), () => StartScreen());
        //Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
}


