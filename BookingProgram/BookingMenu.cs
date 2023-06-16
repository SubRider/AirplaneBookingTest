﻿using System;
using System.ComponentModel.DataAnnotations;
 using System.Data;
 using System.Globalization;
using System.Xml.Linq;

static class BookingMenu
{
    public static bool Quit = false;
    public static string Origin = "";
    public static string Destination = "";
    public static string DepartureDate = "";
    public static string ArrivalDate = "";
    public static string ReservationChoice;
    public static int FlightID = -1;
    public static int ResultID = -1;
    public static Airplane CurrentPlane;
    public static int AmountOfSeatsReserved;
    public static string SingleOrRetour;
    public static Action CurrentMenu;
    public static Action NextMenu;
    public static List<Seat> Seats = new();
    static void Main()
    {

        Flight.Flights = JsonCommunicator.Read<Flight>("Flights.json");
        Airplane.Planes = JsonCommunicator.Read<Airplane>("Airplanes.json");
        AccountLogic.Accounts = JsonCommunicator.Read<AccountModel>("Accounts.json");
        ReservationModel.Reservations = JsonCommunicator.Read<ReservationModel>("reservations.json");
        City.CreateCountryList();

        Console.CursorVisible = true;
        Renderer.Clear();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        //Thread.Sleep(4000);
        StartScreen();
        while (!Quit)
        {
            Renderer.ShowWindows();
            InputChecker.CheckInput();
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
        _ = new Button("Log out", 0, 5, menuBar, "left", () => LogOut());
    }
    public static void StartScreen()
    {
        Renderer.Clear();

        if (AccountLogic.CurrentAccount == null) { AccountMenu(); return; }
        Window w1 = new(1, 0.85);
        w1.Text += "Welcome to the homescreen.";

        AddMenuBar(w1);
    }

    public static void AccountMenu()
    {
        Renderer.Clear();
        CurrentMenu = () => AccountMenu();
        NextMenu = () => AccountMenu();
        Window w1 = new();
        if (AccountLogic.CurrentAccount == null) 
        {
            
            _ = new Button(ConsoleColor.Blue, "Sign up", 2, w1, "bottom", () => AccountLogic.CreateAccount(false));
            _ = new Button(ConsoleColor.Green, "Sign in", 1, w1, "bottom", () => LoginMenu(false));
            _ = new Button(ConsoleColor.DarkRed, "Exit", 0, w1, "bottom", () => Quit = true);
        }
        else
        {
            w1.Text +=  $"\u001b[96m Account\u001b[0m\n║\u001b[96m----------\u001b[0m\n║\n║Name: {AccountLogic.CurrentAccount.FullName}\n" +
                        $"║Email address: {AccountLogic.CurrentAccount.EmailAddress}\n" +
                        $"║Phone number: " + (AccountLogic.CurrentAccount.PhoneNumber != null ? AccountLogic.CurrentAccount.PhoneNumber : "");
            _ = new Button("Edit", 3, w1, "bottom", () => EditAccount(false));
            _ = new Button("Delete Account", 2, w1, "bottom", () =>
            {
                Renderer.Clear();
                Window w1 = new();
                w1.Text += "Are you sure you want to delete your account?";
                _ = new Button("Yes", 3, w1, "top", () => DeleteAccount());
                _ = new Button("No", 4, w1, "top", () => AccountMenu());
                AddMenuBar(w1);
            });
        }
        
        AddMenuBar(w1);
    }

    public static void EditAccount(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            Window w1 = new();
            CurrentMenu = () => EditAccount(true);
            NextMenu = () => StartScreen();
            w1.Text +=  $"\u001b[96m Edit Account\u001b[0m\n║\u001b[96m---------------\u001b[0m\n║* All empty fields will not change\n║\n║";
            InputButton name = new("Name", 4, w1);
            InputButton email = new("Email address", 5, w1);
            InputButton phone = new("Phone number", 6, w1);
            _ = new Button("Save", 3, w1, "bottom", () =>
            {
                List<AccountModel> accountList = AccountsAccess.LoadAll();
                
                // change account details if not empty
                if (name.Input.Count() > 0) 
                { 
                    AccountLogic.CurrentAccount.FullName = name.Input; 

                    foreach (AccountModel item in accountList)
                    {
                        if (item.Id == AccountLogic.CurrentAccount.Id)
                        {
                            item.FullName = name.Input;
                        }
                    }
                }
                if (email.Input.Count() > 0) 
                { 
                    AccountLogic.CurrentAccount.EmailAddress = email.Input; 

                    foreach (AccountModel item in accountList)
                    {
                        if (item.Id == AccountLogic.CurrentAccount.Id)
                        {
                            item.EmailAddress = email.Input;
                        }
                    }
                }
                if (phone.Input.Count() > 0) 
                { 
                    AccountLogic.CurrentAccount.PhoneNumber = phone.Input; 

                    foreach (AccountModel item in accountList)
                    {
                        if (item.Id == AccountLogic.CurrentAccount.Id)
                        {
                            item.PhoneNumber = phone.Input;
                        }
                    }
                }  

                // write new details to json and go back to accountmenu
                AccountsAccess.WriteAll(accountList); 
                Renderer.Clear();
                Console.WriteLine("Changed Details");
                Thread.Sleep(1000);
                AccountMenu();             
            });
            _ = new Button("Back", 2, w1, "bottom", () => AccountMenu());
            
            AddMenuBar(w1);
        }
    }

    public static void DeleteAccount()
    {
        Renderer.Clear();
        Window w1 = new();
        List<AccountModel> TempAccountList = new(AccountLogic.Accounts);
        foreach (AccountModel account in TempAccountList)
        {
            if (account.Id == AccountLogic.CurrentAccount.Id)
            {
                AccountLogic.Accounts.Remove(account);
            }
        }
        JsonCommunicator.Write("Accounts.json", AccountLogic.Accounts);
        Console.SetCursorPosition(1, 1);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Account deleted");
        Thread.Sleep(700);
        AccountLogic.CurrentAccount = null;
        StartScreen();
    }   

    public static void LoginMenu(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            Window w1 = new();
            CurrentMenu = () => LoginMenu(true);
            NextMenu = () => StartScreen();
            InputButton email = new("Email", 0, w1);
            InputButton password = new("Password", 1, w1);
            _ = new Button("Continue", 3, w1, () =>
            {
                if (email.Input == "admin") { AdminMenu(); return; }
                AccountModel account = AccountLogic.CheckLogin(email.Input, password.Input);
                if (account != null)
                {
                    Console.SetCursorPosition(1, 5);
                    Console.WriteLine("Welcome back " + account.FullName);
                    Thread.Sleep(1000);
                    NextMenu();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(1, 5);
                    Console.WriteLine("Incorrect email or password");
                    Thread.Sleep(700);
                    Console.SetCursorPosition(1, 5);
                    Console.WriteLine("                                  ");
                }
                
            });
            AddMenuBar(w1);
        }
    }

    public static void LogOut()
    {
        if (AccountLogic.CurrentAccount == null)
        {
            StartScreen();
        }
        else
        {
            Renderer.Clear();
            Console.WriteLine("Logged out");
            Thread.Sleep(1000);
            AccountLogic.CurrentAccount = null;
            StartScreen();
        }
    }
  
    public static void FlightSearchMenu(bool loop)
    {      
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => FlightSearchMenu(true);
            NextMenu = () => ClassReservationMenu();
            Window w1 = new(1, 0.85);
            InputButton originInput = new("Origin", 0, w1, () => {CountrySelection(false, "origin");});
            originInput.Input = Origin;
            InputButton destinationInput = new("Destination", 1, w1, () => {CountrySelection(false, "destination");});
            destinationInput.Input = Destination;
            InputButton departureInput = new("Departure (press enter to see calendar)", 2, w1, () => CalendarMenu(DateTime.Now.Month, DateTime.Now.Year, "Departure"));
            departureInput.Input = DepartureDate;
            InputButton arrivalInput = new("Arrival (press enter to see calendar)", 3, w1, () => CalendarMenu(DateTime.Now.Month, DateTime.Now.Year, "Arrival"));
            arrivalInput.Input = ArrivalDate;

            AddMenuBar(w1);
        }
        if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input), ("DepartureDate", InputButton.InputButtons[2].Input), ("ArrivalDate", InputButton.InputButtons[3].Input) });
 
    }
    
    public static void CountrySelection(bool loop, string inputField)
    {
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => CountrySelection(true, inputField);
            NextMenu = () =>
            {
                if (inputField == "origin") Origin = City.FindByID(ResultID).ToString();
                if (inputField == "destination") Destination = City.FindByID(ResultID).ToString();
                FlightSearchMenu(false);
            };
            Window w1 = new(1, 0.85);
            InputButton city = new("City", 0, w1);
            AddMenuBar(w1);
        }
        if (loop) Renderer.ClearLines();
        SearchMenu<City> citySearch = new(City.Cities);
        citySearch.Activate(new() { ("Name", InputButton.InputButtons[0].Input) });
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
    }

    public static void ConfirmationMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        w1.Text += "Are you sure you want to book the following seats?";
        foreach (Seat seat in Seats)
        {
            w1.Text += $" ^ {seat}";
        }

        _ = new Button("Yes", AmountOfSeatsReserved + 2, w1, () => Reserving());
        _ = new Button ("No", AmountOfSeatsReserved + 3, w1, () => SeatMenu());
        AddMenuBar(w1);
    }

    public static void SeatMenu()
    {
        
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        if (ReservationChoice == "First") Renderer.ShowSeats(CurrentPlane.FirstClassSeats, w1);
        else if (ReservationChoice == "Business") Renderer.ShowSeats(CurrentPlane.BusinessClassSeats, w1);
        else Renderer.ShowSeats(CurrentPlane.EconomyClassSeats, w1);
        Button back = new("back", 0, w1, "bottom", () => SeatsReservationMenu());
        AddMenuBar(w1);
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
            _ = new Button("Sign in", 2, w1, () => LoginMenu(false));
            //InputButton email = new("Email", 2, w1, () => Reserving());
        }
        else
        {
            foreach (Seat seat in Seats) seat.Booked = true;
            _ = new ReservationModel(Seats, FlightID, AccountLogic.CurrentAccount.Id);
            JsonCommunicator.Write("Reservations.json", ReservationModel.Reservations);
            Console.WriteLine("Successfully Reserved.");
            JsonCommunicator.Write("Airplanes.json", Airplane.Planes);

            Seats = new();
            AmountOfSeatsReserved = 0;

            Thread.Sleep(700);
            StartScreen();
        }

    }

    public static void History()
    {
        Renderer.Clear();
        Window w1 = new(1, 0.85);
        w1.Text += $"\u001b[96m History\u001b[0m\n║\u001b[96m----------\u001b[0m\n║\n║";
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
    }

    public static void CancelFlight()
    {
        Renderer.Clear();
        CurrentMenu = () => CancelFlight();
        Window w1 = new(1, 0.85);
        w1.Text += $"\u001b[96m Cancel\u001b[0m\n║\u001b[96m----------\u001b[0m\n║\n║";
        int count = 2;
        if (AccountLogic.CurrentAccount == null) w1.Text += "You are not logged in";
        else
        {
            foreach (ReservationModel reservation in ReservationModel.Reservations)
            {
                if (reservation.CustomerID == AccountLogic.CurrentAccount.Id)
                {
                    Flight flight = Flight.FindByID(reservation.FlightID);
                    Button FlightButton = new Button($"{flight}", count + 1, w1, "left", () => reservation.RemoveFlight(ReservationModel.Reservations, reservation.FlightID));
                    count =+ 1;
                }
            }
        }
        AddMenuBar(w1);
    }
    public static void AddAdminMenuBar(Window reference)
    {
        Window menuBar = new(1, 0.15, reference);
        _ = new Button("Show flights", 0, 0, menuBar, "left", () => DisplayFlights(false));
        _ = new Button("Show Airplanes", 0, 1, menuBar, "left", () => AirplaneListMenu());
        _ = new Button("Add flights", 0, 2, menuBar, "left", () => AddFlightMenu(false));
        _ = new Button("Add Airplanes", 0, 3, menuBar, "left", () => AddAirplaneMenu());
        _ = new Button("Edit flights", 0, 4, menuBar, "left", () => EditFlightMenu(false));
        _ = new Button("Remove flights", 0, 5, menuBar, "left", () => RemoveFlightMenu(false));
        _ = new Button("Dashboard", 0, 6, menuBar, "left", () => Dashboard());
        _ = new Button("Log out", 0, 7, menuBar, "left", () => LogOut());
    }
    public static void AdminMenu()
    {

        Renderer.Clear();
        Window w1 = new(1, 0.85);
        AddAdminMenuBar(w1);
    }


    public static void DisplayFlights(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            CurrentMenu = () => DisplayFlights(true);
            Window w1 = new(1, 0.85);
            InputButton origin = new("Origin", 0, w1);
            InputButton destination = new("Destination", 1, w1);
            AddAdminMenuBar(w1);
        }
        if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input) });
    }
    public static void AirplaneListMenu()
    {
        Renderer.Clear();
        Window w1 = new();
        foreach (Airplane airplane in Airplane.Planes) w1.Text += $"{airplane} ^ ";
        AddAdminMenuBar(w1);
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
            InputButton departureDate = new("Departure date", 2, w1);
            InputButton arrivalDate = new("Arrival date", 3, w1);
            InputButton airplaneID = new("Airplane ID", 4, w1);
            _ = new Button("Confirm edit", 6, w1, () =>
            {
                try 
                { 
                    DateTime DepartureDate = DateTime.Parse(departureDate.Input);
                    DateTime ArrivalDate = DateTime.Parse(departureDate.Input);
                    Convert.ToInt32(airplaneID.Input);
                    _ = new Flight(origin.Input, destination.Input, departureDate.Input, arrivalDate.Input, Convert.ToInt32(airplaneID.Input));
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
        }
        else if (loop) Renderer.ClearLines();
        SearchMenu<Flight> flightSearch = new(Flight.Flights);
        flightSearch.Activate(new() { ("Origin", InputButton.InputButtons[0].Input), ("Destination", InputButton.InputButtons[1].Input), ("ID", InputButton.InputButtons[2].Input) });
    }

    public static void Dashboard()
    {
        Renderer.Clear();
        Window w1 = new();
        int AmountofFlightsReserved = 0;
        int AmountofAirplanes = 0;
        foreach (ReservationModel reservation in ReservationModel.Reservations)
        {
            AmountofFlightsReserved += 1;
        }
        foreach (Airplane airplane in Airplane.Planes)
        {
            AmountofAirplanes += 1;
        }
        w1.Text = $"Amount of flights booked: {AmountofFlightsReserved}\n║Amount of seats booked: {AmountOfSeatsReserved}\n║Amount of airplanes: {AmountofAirplanes}\n║Amount of revenue: $";

        AddAdminMenuBar(w1);
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
        w1.Text += " \u001b[96mAbout\u001b[0m ^" +
                    " \u001b[96m--------\u001b[0m ^" +
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
    }

    public static string CalendarMenu(int month, int year, string direction)
    {
        int minYear = 1;
        int maxYear = 9999;

        Renderer.Clear();
        Window w1 = new(1, 0.85);

        // give the page a header tekst to make sure user knows what they are supposed to do on that page
        if (direction == "Departure")
        {
            w1.Text +=  $"\u001b[96m Departure date\u001b[0m\n║\u001b[96m------------------\u001b[0m\n║";
        }
        else if (direction == "Arrival")
        {
            w1.Text +=  $"\u001b[96m Arrival date\u001b[0m\n║\u001b[96m---------------\u001b[0m\n║";
        }

        w1.Text +=  "\n║\n║\n║\u001b[92m Mo Tu We Th Fr Sa Su \u001b[0m  \n║\n║";

        List<string> months = new() { "January", "February", "March", "April", "May", "June", 
                                    "July", "August", "September", "October", "November", "December" };
        _ = new Button($"{months[month-1]}", 3, 1, w1, () => { }, false);
        _ = new Button($"{year}", 3, 4, w1, () => { }, false);
        Button previous = new("Previous", 2, 1, w1, "bottom", () =>
        {
            if (month == 1) CalendarMenu(12, year - 1, direction);
            else CalendarMenu(month - 1, year, direction);
        });
        Button next = new("Next", 2, 5, w1, "bottom", () =>
        {
            if (month == 12) CalendarMenu(1, year + 1, direction);
            else CalendarMenu(month + 1, year, direction);
        });
        Calendar.PrintCal(year, month, minYear, maxYear, w1, direction, month, year);
        //w1.Text += $" {Calendar.PrintCal(year, month, minYear, maxYear, w1)}";

        _ = new Button ("Back", 0, 1, w1, "bottom", () => FlightSearchMenu(false));

        AddMenuBar(w1);

        return "25-06-2023";
    }
}