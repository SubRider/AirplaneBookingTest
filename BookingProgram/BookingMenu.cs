static class BookingMenu
{
    private static ButtonTemplate ExitButton = new(ConsoleColor.DarkRed, "Exit", "exit");
    private static ButtonTemplate LoginButton = new(ConsoleColor.Green, "Login", "login");
    private static ButtonTemplate BrowseButton = new(ConsoleColor.Green, "Browse flights", "browse");
    private static ButtonTemplate HistoryButton = new(ConsoleColor.Green, "Flight history", "history");

    public static bool CursorXMovement = false;
    public static bool Quit = false;

    static void Main()
    {
        Console.Clear();
        StartScreen();
        while (!Quit)
        {
            InputChecker.CheckInput(CursorXMovement);
        }
    }
    public static void StartScreen()
    {
        Console.ResetColor();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Button login = new(LoginButton, 21);   
        Button exit = new(ExitButton, 22);
        login.Show();
        exit.Show();
        Console.SetCursorPosition(0, 21);
    }

    public static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome [user]");
        Button browse = new(BrowseButton, 3);
        Button history = new(HistoryButton, 4);
        browse.Show();
        history.Show();
        Console.SetCursorPosition(0, 3);
    }

    public static void SeatMenu()
    {
        CursorXMovement = true;
        Console.Clear();
        Plane plane = new("747");
        plane.ShowSeats();
    }
}

