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
        _ = new Button("Add flights", 0, 1, menuBar, "left", () => AddFlightMenu(false));
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
        _ = new Button($"Next (", 0, w1, "bottom", () => AdminMenu());
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
                 _ = new Flight(origin.Input, destination.Input, departureDate.Input, Convert.ToInt32(airplaneID.Input));
                JsonCommunicator.Write("Flights.json", Flight.Flights);
                AdminMenu();
            });
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
                    "we look forward to welcoming you on board and taking you to your next adventure. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam aliquet rhoncus nibh nec pulvinar. Proin et tortor et felis tristique imperdiet quis eu lorem. Mauris ullamcorper nisi nibh, et sodales metus fringilla sed. Ut id est consequat, consequat enim venenatis, rutrum metus. Pellentesque sed mi vel risus ultrices volutpat a et lorem. Vestibulum luctus tempus nulla eget iaculis. Curabitur vel lacus eget felis accumsan mollis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer iaculis, tortor in viverra malesuada, nulla dui posuere ipsum, sed lacinia sem erat sit amet risus. Vivamus nunc mauris, imperdiet at magna vestibulum, dictum tincidunt risus. Quisque eu odio finibus, elementum diam sit amet, elementum leo.\r\n\r\nSed efficitur vestibulum cursus. Proin fringilla nibh et venenatis commodo. Sed ut nunc porttitor, molestie odio ac, ultrices ligula. Fusce ullamcorper tristique arcu, quis ornare lorem dictum eget. Donec egestas erat nec sapien tempor, ac egestas sapien lacinia. Integer mollis, diam a euismod scelerisque, ante dui dictum turpis, iaculis fringilla dui lacus eu turpis. Sed dui dui, tincidunt ut mauris tempor, fringilla egestas velit. Nam iaculis cursus ex sed aliquet. Integer feugiat neque sit amet pharetra tempus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam in tincidunt augue. Integer non egestas sem, in porttitor felis. Nulla molestie neque a pellentesque molestie. Integer scelerisque purus vel enim porttitor, ut vulputate ligula pretium.\r\n\r\nSuspendisse non faucibus neque, ac pharetra mi. Sed ultrices ullamcorper molestie. Morbi id semper dui. Vivamus a orci maximus, laoreet tellus non, eleifend urna. Suspendisse mattis sapien quis eros rhoncus vehicula vel quis nulla. Curabitur at feugiat nisl. Ut convallis elit dignissim, pretium arcu sit amet, pellentesque felis. Quisque vel scelerisque arcu. Ut non nulla feugiat, pulvinar nisl non, dictum elit. Praesent laoreet eu lorem pharetra viverra. Aenean sit amet est eu nulla blandit tristique eu eget nisl. Nunc hendrerit enim sed tellus efficitur consectetur. Duis non efficitur magna, aliquet accumsan nisl.\r\n\r\nCras consequat condimentum est et molestie. Nullam rutrum dolor id velit condimentum, aliquam ornare nisl condimentum. Maecenas suscipit faucibus massa in vehicula. Cras ut massa nec felis rhoncus posuere. Praesent leo enim, consectetur nec viverra quis, pretium non urna. Vestibulum volutpat dui vitae auctor bibendum. Nunc blandit pharetra pulvinar. Mauris facilisis, ex quis pharetra dapibus, mauris tellus convallis ex, at sollicitudin sem metus id nibh. Nam hendrerit molestie orci, et sollicitudin est ullamcorper sed. Praesent porttitor nisi a magna mollis, vitae rutrum justo gravida. Quisque diam sapien, feugiat et tempus tincidunt, tincidunt pharetra nisi. Nam cursus tortor quam.\r\n\r\nSed nec diam faucibus, tincidunt augue in, tincidunt magna. Proin quam risus, viverra at ullamcorper eu, mattis sed magna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam massa turpis, pretium vitae urna sed, auctor interdum ex. Nulla facilisi. Integer in fringilla erat. Duis vel lacinia diam, sit amet fringilla mauris. Sed varius ex non risus elementum sollicitudin. Suspendisse ullamcorper sem tempor, rutrum quam sed, laoreet eros. Nam sed nisi nisi. In hac habitasse platea dictumst. Suspendisse nec luctus augue. Maecenas eu massa quis ipsum rutrum tempus. Phasellus vestibulum sollicitudin convallis.\r\n\r\nUt porta massa vel dapibus finibus. Pellentesque ultrices sit amet mi at malesuada. Mauris sed feugiat erat, sed interdum metus. Morbi pharetra accumsan nunc, sed dapibus ligula dictum vitae. Cras pharetra ultricies lectus nec dapibus. Nam interdum sodales fringilla. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Mauris non nisl eget leo auctor maximus ut ut elit. Praesent pulvinar sapien vel pharetra maximus. Curabitur sit amet molestie lacus, id commodo ipsum. Aliquam ac turpis eget justo feugiat consequat. Ut efficitur elit lorem, sed laoreet lacus rhoncus a. Vivamus mattis dolor ut libero cursus posuere.\r\n\r\nMaecenas posuere volutpat porttitor. Ut vitae arcu eleifend, aliquam ligula ut, suscipit arcu. Vestibulum pellentesque dolor nec urna porta, tristique facilisis enim euismod. Morbi blandit efficitur tincidunt. Ut in diam mollis, maximus tortor et, vehicula lorem. Cras placerat volutpat ligula, a tempus libero tristique vel. Aenean felis elit, pretium ac libero id, consequat semper ex. Sed consequat felis in nibh gravida malesuada. Donec suscipit lacus ac tellus gravida porttitor.\r\n\r\nNunc vitae dictum nisl. Morbi eget consectetur nisl, sit amet dapibus ex. Fusce porta lectus ipsum, egestas pellentesque dui luctus in. Duis eu auctor felis. Sed facilisis purus posuere, fermentum nulla nec, convallis felis. Vivamus aliquet elit nec dapibus accumsan. Donec convallis aliquam feugiat. In hac habitasse platea dictumst. Sed non tempus velit. Aenean placerat eget magna at malesuada. Aenean aliquet, quam nec sagittis fringilla, libero erat elementum purus, et pharetra sem erat a sapien. Sed a nisi congue, convallis mauris sit amet, dapibus nulla. Proin ut quam rutrum, hendrerit velit non, egestas mauris. Nullam vitae massa a elit porta tincidunt.\r\n\r\nNullam porta lectus quis orci gravida, et faucibus ligula interdum. In mi urna, eleifend non rhoncus in, consequat vel nisi. Nulla auctor varius arcu, in egestas est dictum sit amet. Suspendisse laoreet nisl sed ultrices porta. Aenean placerat orci sem, vehicula laoreet magna vulputate id. Vestibulum tempus massa nec lacus sollicitudin ornare. Aliquam neque eros, egestas a pretium ut, rhoncus a ipsum. Aenean consequat lectus massa, ut bibendum sem accumsan at. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Curabitur aliquam non justo sed eleifend. Donec accumsan mi quis varius cursus. Duis volutpat dolor vel vestibulum eleifend. Quisque ut leo quam. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Praesent consequat pellentesque purus, eget tristique lacus fringilla gravida. In gravida erat est, non congue sapien mollis in.\r\n\r\nSed posuere ut velit quis gravida. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut sit amet semper orci, eu porta ante. Curabitur finibus nisl quis tellus gravida faucibus. Nulla fermentum, libero ut pellentesque mattis, quam elit varius arcu, eu vulputate libero augue et ipsum. Aliquam congue sodales lectus, id scelerisque leo tempor nec. Sed faucibus nunc a lacus dignissim, ut efficitur orci porttitor. In volutpat sit amet est et vehicula. Duis augue erat, ornare at consectetur a, porta vitae diam. Praesent in purus eu massa volutpat egestas. Curabitur tempor, elit id ullamcorper auctor, justo leo egestas metus, in facilisis urna nisl vitae eros. Quisque at mollis turpis, in aliquet mi. Nam semper mauris non ante eleifend, nec elementum augue laoreet. Maecenas bibendum ante nec est sollicitudin lacinia. Sed non turpis ullamcorper, iaculis justo a, cursus massa.\r\n\r\nVivamus non tortor est. Donec at est urna. Aenean dictum quis eros ac euismod. Sed id tincidunt velit. Maecenas congue accumsan risus, vitae dignissim metus faucibus nec. Maecenas pharetra et risus id venenatis. Aliquam ex dui, auctor eget fringilla non, pretium ut augue.\r\n\r\nSuspendisse potenti. Integer consequat fringilla libero, et sollicitudin elit hendrerit vitae. Phasellus mattis efficitur lorem, eget accumsan tellus maximus vitae. Curabitur quis tellus lectus. Morbi nunc arcu, elementum vel nisl eu, placerat commodo ipsum. Curabitur nisl nunc, rhoncus in dolor a, tincidunt volutpat dui. Praesent iaculis facilisis mauris nec imperdiet. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Aenean varius venenatis orci hendrerit condimentum. Donec eu lacus sit amet leo porta hendrerit eu vel dolor. Nam hendrerit elit a magna mollis, sed pellentesque purus faucibus. Ut vel lobortis nisi. Nam venenatis nulla a nisl suscipit, ac dignissim justo efficitur. Nunc purus neque, egestas ac nulla ac, pharetra bibendum diam.\r\n\r\nFusce enim augue, feugiat sed nisi congue, fermentum ultricies quam. Etiam eu elementum massa. Ut eleifend tincidunt odio eget rutrum. Curabitur ut dolor ligula. Curabitur nisi ex, posuere in pretium in, tincidunt vitae sem. Etiam a nisl libero. Nam eu porttitor nibh. Donec mollis blandit justo, non feugiat eros imperdiet eu. Sed porttitor, ipsum non finibus vulputate, ante libero maximus enim, ac cursus tellus justo quis dolor. Integer fermentum nec nisi vitae lacinia. Donec imperdiet tortor non rhoncus pharetra.\r\n\r\nFusce ultricies est sodales sollicitudin pulvinar. Praesent vitae lobortis justo. In porta, lectus in dictum ultrices, lectus odio ultricies tellus, sit amet faucibus tortor lacus eget leo. Donec imperdiet tempor lacinia. Cras suscipit placerat porttitor. In vel dignissim sem. Suspendisse sapien risus, porttitor ut malesuada eu, vulputate in magna. Phasellus fermentum interdum ligula quis laoreet. In ut justo auctor, accumsan est ac, porttitor libero. Nulla aliquam nunc in elit laoreet, in feugiat felis ullamcorper. Proin tempus finibus turpis, non feugiat eros mattis a. Sed vel massa libero. Etiam erat nulla, dictum eget massa vel, consectetur hendrerit arcu. Praesent in congue tortor.\r\n\r\nVestibulum pharetra dolor sed mi vestibulum, ut mattis leo maximus. Fusce fringilla turpis mi, sit amet tempor sem vestibulum eget. Integer lobortis urna eu leo efficitur lobortis. Nullam ut elementum nisl, mollis auctor enim. Integer faucibus eros id leo efficitur sodales ut eu neque. Fusce sit amet congue neque. Aenean massa orci, tempus vitae velit eu, pulvinar consectetur diam. Integer mattis sem a risus varius scelerisque. Donec eleifend tincidunt rutrum. Vivamus nunc leo, faucibus quis elit eget, rutrum sagittis felis. Vestibulum at ante accumsan, luctus magna dignissim, viverra ipsum. Aenean gravida lorem consequat, vestibulum dolor ut, commodo odio. Donec ac sagittis magna, vitae sodales velit. Quisque ut odio erat.\r\n\r\nInteger vitae sollicitudin leo. Quisque a sapien a urna interdum hendrerit id nec mi. Suspendisse nec porta mi, et rhoncus nisl. Fusce sed porttitor quam. Quisque aliquam ante a imperdiet sagittis. Quisque rhoncus laoreet luctus. Fusce dignissim et risus quis tristique. Pellentesque vehicula nisl eget porttitor volutpat. Aenean vitae elit in dolor imperdiet vulputate at non lectus. Nam tortor dolor, rhoncus id tempus in, lacinia at risus. Morbi auctor facilisis nulla molestie aliquet. Praesent at lacus ac sem facilisis blandit. Vivamus et leo bibendum, gravida nibh a, auctor tortor. Suspendisse nec mollis nulla. Morbi vel nibh vel sapien rhoncus semper sit amet id ante. Aliquam erat volutpat.\r\n\r\nSuspendisse consequat quam a velit malesuada placerat ut non enim. Suspendisse eu velit vel ligula sollicitudin efficitur eu et nisi. Fusce molestie augue porttitor ornare maximus. Praesent est velit, molestie et nisl nec, suscipit vulputate massa. Suspendisse potenti. Sed vel est maximus, pellentesque lorem et, tristique dui. Donec id dui sapien. Donec sed nibh in augue interdum consequat.\r\n\r\nFusce leo nisl, egestas sit amet nisl ac, hendrerit efficitur tortor. Mauris sed arcu nunc. Duis molestie tincidunt felis, id lobortis nisi. Aenean porta magna et ex luctus, quis ullamcorper odio sollicitudin. Quisque a ligula eu ipsum semper hendrerit ac at urna. Praesent vulputate libero at enim lobortis, ullamcorper congue leo egestas. Ut vel risus sit amet nulla dapibus mattis eu sed enim. Sed congue felis ut orci volutpat, vitae aliquet sapien laoreet. Aliquam auctor ante nec ante dapibus ullamcorper. In maximus est diam, et facilisis lectus imperdiet at. Pellentesque est ex, ullamcorper in nibh sit amet, ullamcorper sodales tortor. Curabitur eget mauris eget nibh egestas lobortis. Integer at nibh ac ipsum finibus cursus.\r\n\r\nAliquam finibus mattis lorem et accumsan. Ut non risus ultricies, laoreet neque lacinia, blandit elit. Fusce vitae blandit arcu. Suspendisse potenti. Nunc sit amet auctor metus, a convallis odio. Nulla auctor ipsum sit amet ex elementum, non congue enim mollis. Nunc vel ipsum quis velit interdum porta. Donec in aliquam nunc. Morbi ac molestie mi. Donec venenatis tempus diam, ut lobortis ex facilisis sed. Duis sed justo aliquam, suscipit tortor sed, luctus justo.\r\n\r\nSuspendisse fermentum lacus vel condimentum vehicula. Pellentesque pulvinar quam eu dolor aliquam, vitae finibus nulla condimentum. Phasellus quam urna, molestie vel augue vel, placerat semper enim. Nam viverra eros ac erat vestibulum porta. Aliquam a leo vitae libero ullamcorper tincidunt non vitae lectus. Ut leo mi, dictum dapibus gravida ut, consequat vitae neque. Ut non gravida nisl. Morbi malesuada est non ex suscipit efficitur. Praesent auctor vestibulum turpis. Nulla facilisi. Donec pharetra quam in fermentum sodales. Integer semper porttitor ligula, quis suscipit justo imperdiet eu. ^";

        string phoneNumber = "010 546 7465";
        string email = "info@rotterdamairlines.com";
        w1.Text += $" Phone Number: {phoneNumber} ^ E-mail: {email}";
        AddMenuBar(w1);
        MenuUpdated = true;
    }
}


