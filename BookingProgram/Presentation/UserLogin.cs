static class UserLogin
{
    public static void Start()
    {
        while (true)
        {
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


            AccountModel account = AccountLogic.CheckLogin(email, password);
            if (account != null)
            {
                Console.WriteLine("\nWelcome back " + account.FullName);
                Thread.Sleep(1000);
                Console.CursorVisible = false;
                BookingMenu.CurrentMenu();
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
