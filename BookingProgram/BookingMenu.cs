static class BookingMenu
{
    public static bool Quit = false;
    public static string ReservationChoice;
    public static int AmountOfSeatsReserved;
    static void Main()
    {
        Console.Clear();
        Console.CursorVisible = false;
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
        Button login = new(ConsoleColor.Green, "Login", 21, () => MainMenu());   
        Button exit = new(ConsoleColor.DarkRed, "Exit", 22, ()=> Quit = true);
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void MainMenu()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Welcome [user]");
        Button browse = new(ConsoleColor.White, "Browse flights", 3,() => ClassReservationMenu());
        Button history = new(ConsoleColor.White, "Flight history", 4, () => History());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void ClassReservationMenu()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Select Class");
        Button first = new(ConsoleColor.White, "First Class", 3, () => ReservationChoice = "first", () => SeatsReservationMenu());
        Button business = new(ConsoleColor.White, "Business Class", 4, () => ReservationChoice = "business", () => SeatsReservationMenu());
        Button economy = new(ConsoleColor.White, "Economy Class", 5, () => ReservationChoice = "economy", () => SeatsReservationMenu());
        Renderer.ShowButtons();
    }
    public static void SeatsReservationMenu()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("How many seats? (Test Maximum: 12):");
        bool choosing = true;
        while (choosing)
        {
            AmountOfSeatsReserved = Convert.ToInt32(Console.ReadLine());
            if (AmountOfSeatsReserved > 12)
            {
                Console.WriteLine("Unable to reserve more than twelve seats");
                continue;
            }
            break;
        }
        SeatMenu();
    }

    public static void SeatMenu()
    {
        Button.Clear();
        Console.Clear();
        Plane plane = new("747");
        Renderer.ShowSeats(plane.FirstClassSeats, 0);
        Renderer.ShowSeats(plane.BusinessClassSeats, 1);
        Renderer.ShowSeats(plane.EconomyClassSeats, 2);
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void History()
    {
        Console.Clear();
        Console.WriteLine("Placeholder for Flight History");
    }
}

