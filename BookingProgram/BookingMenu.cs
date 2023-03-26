static class BookingMenu
{
    private static ButtonTemplate StartButton = new(ConsoleColor.Green, "Start", "start");
    private static ButtonTemplate ExitButton = new(ConsoleColor.DarkRed, "Exit", "exit");
    private static ButtonTemplate LoginButton = new(ConsoleColor.Green, "Login", "login");
    private static ButtonTemplate BrowseButton = new(ConsoleColor.Green, "Browse flights", "browse");
    private static ButtonTemplate HistoryButton = new(ConsoleColor.Green, "Flight history", "history");
    public static bool Quit = false;

    static void Main()
    {
        Console.Clear();
        StartScreen();
        while (!Quit)
        {
            InputChecker.CheckInput(false);
        }
    }
    public static void StartScreen()
    {
        Console.ResetColor();
        ToASCIIArt.Write("Rotterdam");
        ToASCIIArt.Write("Airlines", 1);
        Console.ForegroundColor = ConsoleColor.Green;
        Button start = new(StartButton, 21);
        Button exit = new(ExitButton, 22);
        start.Show();
        exit.Show();
    }
    public static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome [user]");
        Button login = new(LoginButton, 2);
        login.Show();
    }

    public static void UserMenu()
    {
        Console.Clear();
        Button browse = new(BrowseButton, 3);
        Button history = new(HistoryButton, 4);
        browse.Show();
        history.Show();
    }
}

