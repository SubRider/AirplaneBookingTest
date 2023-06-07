using Newtonsoft.Json.Linq;

static class AccountLogic
{
    public static List<AccountModel> Accounts { get; set; } = new();
    public static AccountModel? CurrentAccount { get; set; }


    public static void CreateAccount(bool loop)
    {
        if (!loop)
        {
            Renderer.Clear();
            BookingMenu.CurrentMenu = () => CreateAccount(true);
            Window w1 = new();
            InputButton email = new("Email", 0, w1);
            InputButton name = new("Full name", 1, w1);
            InputButton password = new("Password", 2, w1);
            InputButton confirmPassword = new("Confirm password", 3, w1);
            
            int ID = Accounts.Count;
            _ = new Button("Continue", 5, w1, () =>
            {
                if (email.Input.Length <= 0 || name.Input.Length <= 0 || password.Input.Length <= 0 ||
                    confirmPassword.Input.Length <= 0)
                {
                    Console.SetCursorPosition(1, 8);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please fill ensure all fields are filled");
                    Thread.Sleep(700);
                    Console.SetCursorPosition(1, 8);
                    Console.WriteLine("                                            ");
                }
                else
                {
                    if (password.Input != confirmPassword.Input)
                    {
                        Console.SetCursorPosition(1, 8);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Make sure the passwords are the same");
                        Thread.Sleep(700);
                        Console.SetCursorPosition(1, 8);
                        Console.WriteLine("                                            ");
                    }
                    else
                    {
                        AccountModel account = new(ID, email.Input, password.Input, name.Input);
                        Accounts.Add(account);
                        CurrentAccount = account;
                        JsonCommunicator.Write("Accounts.json", Accounts);
                        BookingMenu.NextMenu();
                    }
                }
            });
                BookingMenu.AddMenuBar(w1);
        }
    }

    public static AccountModel GetById(int id)
    {
        return Accounts.Find(i => i.Id == id);
    }

    public static AccountModel CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = Accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }
}
