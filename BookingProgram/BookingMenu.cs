static class BookingMenu
{
    private static ButtonTemplate StartButton = new(ConsoleColor.Green, "Start", "start");
    private static ButtonTemplate ExitButton = new(ConsoleColor.DarkRed, "Exit", "exit");
    public static bool Quit = false;

    static void Main()
    {
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
    }
}
}
