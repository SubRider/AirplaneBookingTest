static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
    public static AccountModel ActiveUser;

    public static void Start()
    {
        while (true)
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Welcome to the login page");
            Console.WriteLine("\nPlease enter your email address");
            string email = Console.ReadLine();
            if (email == "admin") 
            { 
                BookingMenu.AdminMenu();
                return;
            }

            Console.WriteLine("Please enter your password");
            string password = "";
            string hiddenString = "";
            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter) break;
                if (input.Key == ConsoleKey.Backspace) password = password.Remove(password.Length - 1);
                else password += input.KeyChar;
                hiddenString = new('*', password.Length);
                Console.SetCursorPosition(0, 6);
                Console.Write(new string(' ', password.Length + 1));
                Console.SetCursorPosition(0, 6);
                Console.Write(hiddenString);
            } 

            ActiveUser = new AccountModel(0, email, password,"");
            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("\nWelcome back " + acc.FullName);
                ActiveUser = acc;
                Thread.Sleep(1000);
                Console.CursorVisible = false;
                BookingMenu.StartScreen();
                break;
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
                Thread.Sleep(700);
                continue; 
            }
        }
    }
}
