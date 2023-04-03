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

        Button.Clear();
        Console.Clear();
        Console.WriteLine("Select Class");
        Button first = new(ConsoleColor.White, "First Class", 2, () => ReservationChoice = "first", () => RetourOrSingle());
        Button business = new(ConsoleColor.White, "Business Class", 3, () => ReservationChoice = "business", () => RetourOrSingle());
        Button economy = new(ConsoleColor.White, "Economy Class", 4, () => ReservationChoice = "economy", () => RetourOrSingle());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void RetourOrSingle()
    {
        Button.Clear();
        Console.Clear();
        Console.WriteLine("Select if you want a retour flight or a single flight:\n");
        Button Retour = new(ConsoleColor.White, "Retour", 2, () => SingleOrRetour = "Retour", () => SeatsReservationMenu());
        Button Single = new(ConsoleColor.White, "Single", 3, () => SingleOrRetour = "Single", () => SeatsReservationMenu());
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
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }
    public static void History()
    {
        Console.Clear();
        Console.WriteLine("Placeholder for Flight History");
    }
}

