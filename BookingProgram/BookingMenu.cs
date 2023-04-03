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
        Console.Clear();
        Button.Clear();
        Console.ResetColor();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Button SignIn = new(ConsoleColor.Blue, "Sign up", 20, ()=> CreateAccount.userInfo());
        Button login = new(ConsoleColor.Green, "Login", 21, () => UserLogin.Start());   
        Button info = new(ConsoleColor.Magenta, "Info", 22, () => AirlineInfo());
        Button exit = new(ConsoleColor.DarkRed, "Exit", 23, ()=> Quit = true);
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
        Button button1 = new("Edit flights", 0, () => EditFlightMenu());
        Button button2 = new("Add flight", 1, () => AddFlightMenu());
        Button back = new("back", 6, () => UserLogin.Start());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void EditFlightMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination", "ID" });
        flightSearch.Activate();
        Console.Clear();
        Button.Clear();
        Console.Write("New value: ");
        string value = Console.ReadLine();
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

    public static void AirlineInfo()
    {
        Console.Clear();
        Button.Clear();
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

        string phoneNumber = "+31 6 63726482";
        string email = "info@rotterdamairlines.com";
        Console.WriteLine($"Phone Number: {phoneNumber} \nE-mail: {email}");
        Console.ResetColor();
        Button back = new("back", 12, () => StartScreen());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
}

