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
        Renderer.Clear();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        Thread.Sleep(4000);
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
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Home", 0, 0, menuBar, "left", () => StartScreen());
        _ = new Button("Book Flight", 0, 1, menuBar, "left", () => ClassReservationMenu());
        _ = new Button("My Flights", 0, 2, menuBar, "left", () => History());
        _ = new Button("Account", 0, 3, menuBar, "left", () => AccountMenu());
        _ = new Button("Info", 0, 4, menuBar, "left", () => AirlineInfo());
    }
    public static void StartScreen()
    {
        Renderer.Clear();
        if (UserLogin.ActiveUser == null) { AccountMenu(); return; }
        Window w1 = new(1, 0.85);
        w1.Text += "Welcome to the homescreen.";
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void AccountMenu()
    {
        Renderer.Clear();
        Console.Clear();
        Window w1 = new(1, 0.8);
        if (UserLogin.ActiveUser == null) 
        {
            _ = new Button(ConsoleColor.Blue, "Sign up", 2, w1, "bottom", () => CreateAccount.userInfo());
            _ = new Button(ConsoleColor.Green, "Login", 1, w1, "bottom", () => UserLogin.Start());
            _ = new Button(ConsoleColor.DarkRed, "Exit", 0, w1, "bottom", () => Quit = true);
        }
        else
        {
            w1.Text +=  $"Info:\n║Name: {UserLogin.ActiveUser.FullName}\n" +
                        $"║Email Adress: {UserLogin.ActiveUser.EmailAddress}\n" +
                        $"║Phone Number:";
            _ = new Button("Delete Account", 3, w1, "bottom", () => { });
        }
        AddMenuBar(w1);
        MenuUpdated = true;
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
    }
    public static void RetourOrSingle()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        Console.WriteLine("Select if you want a retour flight or a single flight:\n");
        Button Retour = new("Retour", 2, w1, () => {SingleOrRetour = "Retour"; SeatsReservationMenu(); }); 
        Button Single = new("Single", 3, w1, () => {SingleOrRetour = "Single"; SeatsReservationMenu(); });
        Button back = new("back", 0, w1, "bottom", () => ClassReservationMenu());
        AddMenuBar(w1);
        MenuUpdated = true;
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
        Window w1 = new(1, 0.85);
        Console.CursorVisible = false;
        Renderer.Clear();
        Seats = new();
        Renderer.ShowSeats(CurrentPlane.FirstClassSeats);
        Renderer.ShowSeats(CurrentPlane.BusinessClassSeats);
        Renderer.ShowSeats(CurrentPlane.EconomyClassSeats);
        Button back = new("back", 0, w1, "bottom", () => SeatsReservationMenu());
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void Reserving()
    {
        Renderer.Clear();
        _ = new ReservationDataPacket(Seats, FlightID, UserLogin.ActiveUser.Id);
        JsonCommunicator.Write("Reservations.json", ReservationDataPacket.Reservations);
        Console.WriteLine("Successfully Reserved.");
        JsonCommunicator.Write("Planes.json", Plane.Planes);
        Thread.Sleep(700);
        StartScreen();
    }
    public static void History()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        if (UserLogin.ActiveUser == null) w1.Text += "You are not logged in";
        else
        {
            w1.Text += ("Your previous flights:");
            FlightToHistory.ViewHistory(UserLogin.ActiveUser);
        }
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void AdminMenu()
    {

        Renderer.Clear();
        Window w1 = new(1, 0.85);
         _ = new Button("Show Flights", 0, w1, () => FlightListMenu());
        Button button1 = new Button("Add flight", 1, w1, () => AddFlightMenu());
        Button button2 = new Button("Edit flights", 2, w1, () => EditFlightMenu());
        Button button3 = new Button("Remove Flights", 3, w1, () => RemoveFlightMenu());
        Button back = new ("back", 0, w1, "bottom", () => StartScreen());
        AddMenuBar(w1);
        MenuUpdated = true;
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
        Window w1 = new(1, 0.85);
        //Console.ForegroundColor = ConsoleColor.Magenta;
        w1.Text += "About \n";
        //Console.ForegroundColor = ConsoleColor.Yellow;
        w1.Text += "║Welcome to our airline!" +
                    "We are a company dedicated to providing exceptional travel experiences to our passengers. " +
                    "Our goal is to take you to your destination safely, comfortably, and on time. " +
                    "With a team of experienced pilots, friendly cabin crew, and state-of-the-art aircraft, " +
                    "we strive to make your journey enjoyable from start to finish. " +
                    "Whether you're traveling for business or pleasure, " +
                    "we look forward to welcoming you on board and taking you to your next adventure.\n";

        string phoneNumber = "010 546 7465";
        string email = "info@rotterdamairlines.com";
        w1.Text += $"║Phone Number: {phoneNumber} \n║E-mail: {email}";
        AddMenuBar(w1);
    }
}


