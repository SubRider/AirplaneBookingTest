﻿using System;
using System.Globalization;
static class BookingMenu
{
    public static bool Quit = false;
    public static string ReservationChoice;
    public static int FlightID = -1;
    public static int ResultID = -1;
    public static Airplane CurrentPlane;
    public static int AmountOfSeatsReserved;
    public static string SingleOrRetour;
    public static bool MenuUpdated;
    public static Action CurrentMenu;
    public static Action NextMenu;
    public static List<Seat> Seats = new();
    static void Main()
    {

        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Airplane.Planes = JsonCommunicator.Read<Airplane>("Airplanes.json");
        AccountLogic.Accounts = JsonCommunicator.Read<AccountModel>("Accounts.json");
        ReservationModel.Reservations = JsonCommunicator.Read<ReservationModel>("reservations.json");

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
        Console.Clear();
    }
    public static void AddMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Home", 0, 0, menuBar, "left", () => StartScreen());
        _ = new Button("Book Flight", 0, 1, menuBar, "left", () => FlightSearchMenu(false));
        _ = new Button("My Flights", 0, 2, menuBar, "left", () => History());
        _ = new Button("Account", 0, 3, menuBar, "left", () => AccountMenu());
        _ = new Button("Info", 0, 4, menuBar, "left", () => AirlineInfo());
        _ = new Button("Cal", 0, 5, menuBar, "left", () => CalendarMenu(DateTime.Now.Month, DateTime.Now.Year));
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
            _ = new Button(ConsoleColor.Green, "Sign in", 1, w1, "bottom", () => UserLogin.Start());
            _ = new Button(ConsoleColor.DarkRed, "Exit", 0, w1, "bottom", () => Quit = true);
        }
        else
        {
            w1.Text +=  $"Info:\n║Name: {AccountLogic.CurrentAccount.FullName}\n" +
                        $"║Email Adress: {AccountLogic.CurrentAccount.EmailAddress}\n" +
                        $"║Phone Number:";
            //_ = new Button("Delete Account", 3, w1, "bottom", () => { });
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
        CurrentPlane = Airplane.Planes.Find(i => i.ID == Flight.Flights[index].AirplaneID);
        Window w1 = new(1, 0.85);
        Button first = new("First Class", 0, w1, () => { ReservationChoice = "First";RetourOrSingle(); });
        Button business = new("Business Class", 1, w1, () => { ReservationChoice = "Business";RetourOrSingle(); });
        Button economy = new("Economy Class", 2, w1, () => { ReservationChoice = "Economy";RetourOrSingle(); });
        _ = new Button("back", 0, w1, "bottom", () => FlightSearchMenu(false));

        AddMenuBar(w1);
        MenuUpdated = true;
    }
    public static void RetourOrSingle()
    {
        Renderer.Clear();

        Window w1 = new(1, 0.85);
        Console.WriteLine("Select if you want a retour flight or a single flight:\n");
        Button Retour = new("Retour", 0, w1, () => {SingleOrRetour = "Retour"; SeatsReservationMenu(); }); 
        Button Single = new("Single", 1, w1, () => {SingleOrRetour = "Single"; SeatsReservationMenu(); });
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
            _ = new Button("Sign up", 1, w1, () => AccountLogic.CreateAccount(false));
            _ = new Button("Sign in", 2, w1, () => UserLogin.Start());
            //InputButton email = new("Email", 2, w1, () => Reserving());
        }
        else
        {
            _ = new ReservationModel(Seats, FlightID, AccountLogic.CurrentAccount.Id);
            JsonCommunicator.Write("Reservations.json", ReservationModel.Reservations);
            Console.WriteLine("Successfully Reserved.");
            JsonCommunicator.Write("Airplanes.json", Airplane.Planes);
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
            w1.Text += ("Your previous flights: ^ ");
            Console.SetCursorPosition(0, 3);
            foreach (ReservationModel reservation in ReservationModel.Reservations)
            {
                if (reservation.CustomerID == AccountLogic.CurrentAccount.Id)
                {
                    Flight flight = Flight.FindByID(reservation.FlightID);
                    w1.Text += $"{flight} ^ ";
                    w1.Text += ("Seats: ^ ");
                    foreach (Seat seat in reservation.Seats)
                    {
                        w1.Text += ($"{seat} ^ ");
                    }
                }
            }
        }
        AddMenuBar(w1);
        MenuUpdated = true;
    }
    public static void AddAdminMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Show flights", 0, 0, menuBar, "left", () => FlightListMenu());
        _ = new Button("Show Airplanes", 0, 1, menuBar, "left", () => AirplaneListMenu());
        _ = new Button("Add flights", 0, 2, menuBar, "left", () => AddFlightMenu(false));
        _ = new Button("Add Airplanes", 0, 3, menuBar, "left", () => AddAirplaneMenu());
        _ = new Button("Edit flights", 0, 4, menuBar, "left", () => EditFlightMenu(false));
        _ = new Button("Remove flights", 0, 5, menuBar, "left", () => RemoveFlightMenu(false));
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
        foreach (Flight flight in Flight.Flights) w1.Text += $"{flight} ^ ";
        AddAdminMenuBar(w1);
        MenuUpdated = true;
    }
    public static void AirplaneListMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        foreach (Airplane airplane in Airplane.Planes) w1.Text += $"{airplane} ^ ";
        AddAdminMenuBar(w1);
        MenuUpdated = true;
    }

    public static void AddAirplaneMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        _ = new Button("Boeing 737", 0, w1, () =>
        {
            _ = new Airplane("737");
            JsonCommunicator.Write("Airplanes.json", Airplane.Planes);
            Console.Write("Sucessfully created a new airplane.");
            Thread.Sleep(700);
            AdminMenu();
        });
        _ = new Button("Boeing 787", 1, w1, () =>
        {
            _ = new Airplane("787");
            JsonCommunicator.Write("Airplanes.json", Airplane.Planes);
            Console.Write("Sucessfully created a new airplane.");
            Thread.Sleep(700);
            AdminMenu();
        });
        _ = new Button("Airbus A330", 2, w1, () =>
        {
            _ = new Airplane("A330");
            JsonCommunicator.Write("Airplanes.json", Airplane.Planes);
            Console.Write("Sucessfully created a new airplane.");
            Thread.Sleep(700);
            AdminMenu();
        });
        AddAdminMenuBar(w1);
        MenuUpdated = true;
    }
    public static void AddFlightMenu(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => AddFlightMenu(true);
            Window w1 = new();
            InputButton origin = new("Origin", 0, w1);
            InputButton destination = new("Destination", 1, w1);
            InputButton departureDate = new("Departure", 2, w1);
            InputButton arrivalDate = new("Arrival", 3, w1);
            InputButton airplaneID = new("Airplane ID", 4, w1);
            _ = new Button("Confirm edit", 6, w1, () =>
            {
                try 
                { 
                    DateTime DepartureDate = DateTime.Parse(departureDate.Input);
                    DateTime ArrivalDate = DateTime.Parse(departureDate.Input);
                    Convert.ToInt32(airplaneID.Input);
                    _ = new Flight(origin.Input, destination.Input, departureDate.Input, Convert.ToInt32(airplaneID.Input));
                    JsonCommunicator.Write("Flights.json", Flight.Flights);
                    AdminMenu(); 
                }
                catch 
                {
                    Console.SetCursorPosition(1, 6);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input");
                    Thread.Sleep(1200);
                    Console.ResetColor();
                    origin.Input = "";
                    destination.Input = "";
                    departureDate.Input = "";
                    arrivalDate.Input = "";
                    airplaneID.Input = "";

                    Renderer.ClearLines();
                    Renderer.ShowButtons(w1);
                }
            });
            AddAdminMenuBar(w1);
            MenuUpdated = true;
        }
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
            InputButton airplaneID = new("Airplane ID", 3, w1);
            airplaneID.Input = Convert.ToString(flight.AirplaneID);
            _ = new Button("Confirm edit", 4, w1, () =>
            {
                try 
                { 
                    flight = Flight.Flights[index];
                    flight.Origin = origin.Input;
                    flight.Destination = destination.Input;
                    flight.ID = Convert.ToInt32(flightID.Input);
                    flight.AirplaneID = Convert.ToInt32(airplaneID.Input);
                    JsonCommunicator.Write("Flights.json", Flight.Flights);
                    AdminMenu();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(1, 6);
                    Console.Write("Invalid input of an ID (only numbers)");
                    Console.ResetColor();
                    Thread.Sleep(1200);
                    Renderer.ClearLines();
                    Renderer.ShowButtons(w1);
                }
            });
            AddAdminMenuBar(w1);
            MenuUpdated = true;
        } 
    }



    public static void RemoveFlightMenu(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => RemoveFlightMenu(true);
            NextMenu = () => ConfirmDeletionMenu();
            Window w1 = new();
            InputButton origin = new("Origin", 0, w1);
            InputButton destination = new("Destination", 1, w1);
            InputButton flightID = new("Flight ID", 2, w1);
            AddAdminMenuBar(w1);
            MenuUpdated = true;
        }
        else if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input), ("ID", InputButton.InputButtons[2].Input) });
    }

    public static void ConfirmDeletionMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        Flight flight = Flight.FindByID(ResultID);
        w1.Text += $"Are you sure you want to remove this flight?: ^ {flight}";
        
        _ = new Button("Yes", 2, w1, () =>
        {
            Flight.Flights.Remove(flight);
            JsonCommunicator.Write("Flights.json", Flight.Flights);
            Button.Clear();
            Console.Write("Flight successfully removed.");
            Thread.Sleep(700);
            AdminMenu();
        });
        _ = new Button("No", 3, w1, () => { AdminMenu(); });
    }
    public static void AirlineInfo()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        // Console.ForegroundColor = ConsoleColor.Magenta;
        w1.Text += " About ^" +
                    " -------- ^" +
                    " ^";
        // Console.ForegroundColor = ConsoleColor.Yellow;
        w1.Text +=  " Welcome to our airline! ^" +
                    " ^" +
                    " We are a company dedicated to providing exceptional travel experiences to our passengers." +
                    " Our goal is to take you to your destination safely, comfortably, and on time. ^" +
                    " With a team of experienced pilots, friendly cabin crew, and state-of-the-art aircraft," +
                    " we strive to make your journey enjoyable from start to finish. ^" +
                    " Whether you're traveling for business or pleasure," +
                    " we look forward to welcoming you on board and taking you to your next adventure. ^" +
                    " ^" +
                    " ^";
        string phoneNumber = "010 546 7465";
        string email = "info@rotterdamairlines.com";
        w1.Text += $" Phone Number:  {phoneNumber} ^ E-mail:        {email}";
        AddMenuBar(w1);
        MenuUpdated = true;
    }

    public static void CalendarMenu(int month, int year)
    {
        int minYear = 1;
        int maxYear = 9999;

        Renderer.Clear();
        Window w1 = new(1, 0.85);
        w1.Text += $" {Calendar.PrintCal(year, month, minYear, maxYear)}";
        
        if (month == 12)
        {
            _ = new Button("Previous", 1, 1, w1, "bottom", () => CalendarMenu(month - 1, year));
            _ = new Button("Next", 1, 40, w1, "bottom", () => CalendarMenu(1, year + 1));
        }
        else if (month == 1)
        {
            _ = new Button("Previous", 1, 1, w1, "bottom", () => CalendarMenu(12, year - 1));
            _ = new Button("Next", 1, 40, w1, "bottom", () => CalendarMenu(month + 1, year));
        }
        else 
        {
            _ = new Button("Previous", 1, 1, w1, "bottom", () => CalendarMenu(month - 1, year));
            _ = new Button("Next", 1, 40, w1, "bottom", () => CalendarMenu(month + 1, year));
        }
        AddMenuBar(w1);
        MenuUpdated = true;
    }
}
