static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the login page");
        while (true)
        {
            Console.WriteLine("\nPlease enter your email address");
            string email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine();
            BookingMenu.MainMenu();
            break;

            //AccountChecking not working yet

            /*
            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("\nWelcome back " + acc.FullName);
                Thread.Sleep(500);
                BookingMenu.UserMenu();
                break;
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
                continue; 
            }
            }*/
        }
    }
}
