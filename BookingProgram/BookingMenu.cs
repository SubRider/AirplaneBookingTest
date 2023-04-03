static class BookingMenu
{
    public static bool Quit = false;
    public static string ReservationChoice;
    public static int AmountOfSeatsReserved;
    public static string SingleOrRetour;
    static void Main()
    {
        Console.Clear();
        Console.CursorVisible = true;
        StartScreen();
        while (!Quit)
        {
            InputChecker.CheckInput();
        }
    }
    public static void StartScreen()
    {
        Console.ResetColor();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Button SignIn = new(ConsoleColor.Blue, "Sign up", 20, ()=> CreateAccount.userInfo());
        Button login = new(ConsoleColor.Green, "Login", 21, () => UserLogin.Start());   
        Button exit = new(ConsoleColor.DarkRed, "Exit", 22, ()=> Quit = true);
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void MainMenu()
    {
        //Console.CursorVisible = false;
        Button.Clear();
        Console.Clear();
        Button browse = new(ConsoleColor.White, "Browse flights", 1,() => ClassReservationMenu());
        Button history = new(ConsoleColor.White, "Flight history", 2, () => History());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void ClassReservationMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination" });
        flightSearch.Activate();
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Select Class");
        Button first = new(ConsoleColor.White, "First Class", 2, () => { ReservationChoice = "first";RetourOrSingle(); });
        Button business = new(ConsoleColor.White, "Business Class", 3, () => { ReservationChoice = "business";RetourOrSingle(); });
        Button economy = new(ConsoleColor.White, "Economy Class", 4, () => { ReservationChoice = "economy";RetourOrSingle(); });
        Button back = new("back", 6, () => MainMenu());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void RetourOrSingle()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Select if you want a retour flight or a single flight:\n");
        Button Retour = new(ConsoleColor.White, "Retour", 2, () => {SingleOrRetour = "Retour"; SeatsReservationMenu(); }); 
        Button Single = new(ConsoleColor.White, "Single", 3, () => {SingleOrRetour = "Single"; SeatsReservationMenu(); });
        Button back = new("back", 6, () => ClassReservationMenu());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void SeatsReservationMenu()
    {
        Button.Clear();
        Console.Clear();
        bool choosing = true;
        while (choosing)
        {
            //Console.CursorVisible = true;
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
        //Console.CursorVisible = false;
        Button.Clear();
        Console.Clear();
        Plane plane = new("747");
        Renderer.ShowSeats(plane.FirstClassSeats, 0);
        Renderer.ShowSeats(plane.BusinessClassSeats, 1);
        Renderer.ShowSeats(plane.EconomyClassSeats, 2);
        Button back = new("back", 12, () => SeatsReservationMenu());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void History()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Placeholder for Flight History");
        Button back = new("back", 6, () => MainMenu());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void AdminMenu()
    {
        
        Button.Clear();
        Console.Clear();
        Button button1 = new("edit flights", 0, () => EditFlightMenu());
        Button button2 = new("add flight", 1, () => AddFlightMenu());
        Button back = new("back", 6, () => UserLogin.Start());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void EditFlightMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination", "ID" });
        flightSearch.Activate();
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
        DateTime date = DateTime.Parse(Console.ReadLine());
        Flight _ = new("Boeing 747", origin, destination, date);
        JsonCommunicator.Write("Flights.json", Flight.Flights);
        AdminMenu();

    }
}

