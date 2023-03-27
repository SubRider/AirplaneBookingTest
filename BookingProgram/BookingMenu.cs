static class BookingMenu
{
    public static bool Quit = false;
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
        Button browse = new(ConsoleColor.Green, "Browse flights", 3,() => SeatMenu());
        Button history = new(ConsoleColor.Green, "Flight history", 4, () => History());
        Renderer.ShowButtons();
        InputChecker.JumpToButton(0);
    }

    public static void SeatMenu()
    {
        Button.Clear();
        Console.Clear();
        Plane plane = new("747");
        plane.ShowSeats();
    }
    public static void History()
    {
        Console.Clear();
        Console.WriteLine("Placeholder for Flight History");
    }
}

