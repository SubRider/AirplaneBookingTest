static class BookingMenu
{
    public static bool Quit = false;
    public static string ReservationChoice;
    public static int FlightID = -1;
    public static int ResultID = -1;
    public static Plane CurrentPlane;
    public static int AmountOfSeatsReserved;
    public static string SingleOrRetour;
    public static bool MenuUpdated;
    public static Action CurrentMenu;
    public static Action NextMenu;
    public static List<Seat> Seats = new();
    static void Main()
    {

        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Plane.Planes = JsonCommunicator.Read<Plane>("Planes.json");
        AccountLogic.Accounts = JsonCommunicator.Read<AccountModel>("Accounts.json");
        ReservationDataPacket.Reservations = JsonCommunicator.Read<ReservationDataPacket>("reservations.json");

        Console.CursorVisible = false;
        Renderer.Clear();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        //Thread.Sleep(4000);
        StartScreen();
        while (!Quit)
        {
            Renderer.ShowWindows();
            InputChecker.CheckInput();
            if (MenuUpdated)
            {
                Renderer.ShowWindows();
                InputChecker.JumpToButton(0);
                MenuUpdated = false;
            }
        }
    }
    public static void AddMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Home", 0, 0, menuBar, "left", () => StartScreen());
        _ = new Button("Book Flight", 0, 1, menuBar, "left", () => FlightSearchMenu(false));
        _ = new Button("My Flights", 0, 2, menuBar, "left", () => History());
        _ = new Button("Account", 0, 3, menuBar, "left", () => AccountMenu());
        _ = new Button("Info", 0, 4, menuBar, "left", () => AirlineInfo());
    }
    public static void StartScreen()
    {
        Renderer.Clear();

        if (AccountLogic.CurrentAccount == null) { AccountMenu(); return; }
        Window w1 = new(1, 0.85);
        w1.Text += "Welcome to the homescreen.";

        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void AccountMenu()
    {
        Renderer.Clear();
        CurrentMenu = () => AccountMenu();
        NextMenu = () => AccountMenu();
        Window w1 = new(1, 0.85);
        if (AccountLogic.CurrentAccount == null) 
        {
            
            _ = new Button(ConsoleColor.Blue, "Sign up", 2, w1, "bottom", () => AccountLogic.CreateAccount(false));
            _ = new Button(ConsoleColor.Green, "Login", 1, w1, "bottom", () => UserLogin.Start());
            _ = new Button(ConsoleColor.DarkRed, "Exit", 0, w1, "bottom", () => Quit = true);
        }
        else
        {
            w1.Text +=  $"Info:\n║Name: {AccountLogic.CurrentAccount.FullName}\n" +
                        $"║Email Adress: {AccountLogic.CurrentAccount.EmailAddress}\n" +
                        $"║Phone Number:";
            _ = new Button("Delete Account", 3, w1, "bottom", () => { });
        }
        
        AddMenuBar(w1);
        MenuUpdated = true;
    }
    public static void FlightSearchMenu(bool loop)
    {      
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => FlightSearchMenu(true);
            NextMenu = () => ClassReservationMenu();
            Window w1 = new(1, 0.85);
            InputButton origin = new("Origin", 0, w1);
            InputButton destination = new("Destination", 1, w1);
            AddMenuBar(w1);
            MenuUpdated = true;
        }
        if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input) });
    
    }

    public static void ClassReservationMenu()
    {
        Renderer.Clear();
        FlightID = ResultID;
        int index = Flight.Flights.FindIndex(i => i.ID == FlightID);
        CurrentPlane = Plane.Planes.Find(i => i.ID == Flight.Flights[index].AirplaneID);
        Window w1 = new(1, 0.85);
        Console.WriteLine("Select Class");
        Button first = new("First Class", 2, w1, () => { ReservationChoice = "First";RetourOrSingle(); });
        Button business = new("Business Class", 3, w1, () => { ReservationChoice = "Business";RetourOrSingle(); });
        Button economy = new("Economy Class", 4, w1, () => { ReservationChoice = "Economy";RetourOrSingle(); });
        _ = new Button("back", 0, w1, "bottom", () => FlightSearchMenu(false));

        AddMenuBar(w1);
        MenuUpdated = true;
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
        CurrentMenu = default;
        Window w1 = new();
        InputButton seats = null;
        seats = new("Amount of seats", 0, w1, () => 
        {
            try
            {
                AmountOfSeatsReserved = Convert.ToInt32(seats.Input);
                if (AmountOfSeatsReserved > 12 || AmountOfSeatsReserved <= 0)
                {
                    Console.SetCursorPosition(1, Console.CursorTop + 1);
                    Console.Write("     Unable to reserve this amount of seats");
                    Thread.Sleep(700);
                    seats.Input = "";
                    Renderer.ClearLine();
                    Renderer.ShowButtons(w1);
                }
                else SeatMenu();
            }
            catch
            {
                Console.SetCursorPosition(1, Console.CursorTop + 1);
                Console.Write("     Not a valid number");
                Thread.Sleep(700);
                seats.Input = "";
                Renderer.ClearLine();
                Renderer.ShowButtons(w1);
            }
        }
        );
        _ = new Button("back", 0, w1, "bottom", () => RetourOrSingle());
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void SeatMenu()
    {
        
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        Seats = new();
        if (ReservationChoice == "First") Renderer.ShowSeats(CurrentPlane.FirstClassSeats, w1);
        else if (ReservationChoice == "Business") Renderer.ShowSeats(CurrentPlane.BusinessClassSeats, w1);
        else Renderer.ShowSeats(CurrentPlane.EconomyClassSeats, w1);
        Button back = new("back", 0, w1, "bottom", () => SeatsReservationMenu());
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void Reserving()
    {
        Renderer.Clear();
        CurrentMenu = () => Reserving();
        NextMenu = () => Reserving();
        Window w1 = new();
        if (AccountLogic.CurrentAccount == null)
        {
            w1.Text += "You are not logged in. Please log in or fill in your email below.";
            _ = new Button("Login", 1, w1, () => UserLogin.Start());
            InputButton email = new("Email", 2, w1, () => Reserving());
        }
        else
        {
            _ = new ReservationDataPacket(Seats, FlightID, AccountLogic.CurrentAccount.Id);
            JsonCommunicator.Write("Reservations.json", ReservationDataPacket.Reservations);
            Console.WriteLine("Successfully Reserved.");
            JsonCommunicator.Write("Planes.json", Plane.Planes);
            Thread.Sleep(700);
            StartScreen();
        }

    }
    public static void History()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        if (AccountLogic.CurrentAccount == null) w1.Text += "You are not logged in";
        else
        {
            w1.Text += ("Your previous flights:");
            FlightToHistory.ViewHistory(AccountLogic.CurrentAccount);
        }
        AddMenuBar(w1);
        MenuUpdated = true;
    }
    public static void AddAdminMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Show flights", 0, 0, menuBar, "left", () => FlightListMenu());
        _ = new Button("Add flights", 0, 1, menuBar, "left", () => AddFlightMenu());
        _ = new Button("Edit flights", 0, 2, menuBar, "left", () => EditFlightMenu(false));
        //_ = new Button("Remove flights", 0, 2, menuBar, "left", () => RemoveFlightMenu());
    }
    public static void AdminMenu()
    {

        Renderer.Clear();
        Window w1 = new(1, 0.85);
        AddAdminMenuBar(w1);
        MenuUpdated = true;
    }

    public static void FlightListMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        foreach (Flight flight in Flight.Flights)
        {
            w1.Text += (flight.ToString() + "^");
        }
        Button back = new("back", 0, w1, "bottom", () => AdminMenu());
        AddAdminMenuBar(w1);
        MenuUpdated = true;

    }
    public static void EditFlightMenu(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => EditFlightMenu(true);
            NextMenu = () => ChangeFlightMenu(false);
            Window w1 = new();
            InputButton origin = new("Origin", 0, w1);
            InputButton destination = new("Destination", 1, w1);
            InputButton flightID = new("Flight ID", 2, w1);
            AddAdminMenuBar(w1);
            MenuUpdated = true;
        }
        if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input), ("ID", InputButton.InputButtons[2].Input) });
    }

    public static void ChangeFlightMenu(bool loop)
    {
        if (!loop)
        {
            Flight flight = Flight.FindByID(ResultID);
            int index = flight.FindIndex();

            Renderer.Clear();
            CurrentMenu = () => ChangeFlightMenu(true);
            Window w1 = new();
            InputButton origin = new("Origin", 0, w1);
            origin.Input = flight.Origin;
            InputButton destination = new("Destination", 1, w1);
            destination.Input = flight.Destination;
            InputButton flightID = new("Flight ID", 2, w1);
            flightID.Input = Convert.ToString(flight.ID);
            _ = new Button("Confirm edit", 4, w1, () =>
            {
                flight = Flight.Flights[index];
                flight.Origin = origin.Input;
                flight.Destination = destination.Input;
                flight.ID = Convert.ToInt32(flightID.Input);
                JsonCommunicator.Write("Flights.json", Flight.Flights);
                AdminMenu();
            });
        } 
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

 /*   public static void RemoveFlightMenu()
    {
        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Searcher<Flight> flightSearch = new(Flight.Flights, new() { "Origin", "Destination", "ID" });

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
        
    }*/

    public static void AirlineInfo()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        //Console.ForegroundColor = ConsoleColor.Magenta;
        w1.Text += "About ^";
        //Console.ForegroundColor = ConsoleColor.Yellow;
        w1.Text +=  "Welcome to our airline!" +
                    "We are a company dedicated to providing exceptional travel experiences to our passengers. " +
                    "Our goal is to take you to your destination safely, comfortably, and on time. " +
                    "With a team of experienced pilots, friendly cabin crew, and state-of-the-art aircraft, " +
                    "we strive to make your journey enjoyable from start to finish. " +
                    "Whether you're traveling for business or pleasure, " +
                    "we look forward to welcoming you on board and taking you to your next adventure. ^";

        string phoneNumber = "010 546 7465";
        string email = "info@rotterdamairlines.com";
        w1.Text += $" Phone Number: {phoneNumber} ^ E-mail: {email}";
        AddMenuBar(w1);
        MenuUpdated = true;
    }
}


