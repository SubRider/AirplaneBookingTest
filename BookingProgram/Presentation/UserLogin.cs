static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the login page");
            Console.WriteLine("\nPlease enter your email address");
            string email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine();

            //AccountChecking not working yet
            
            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("\nWelcome back " + acc.FullName);
                Thread.Sleep(1000);
                BookingMenu.MainMenu();
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
