static class AccountLogic
{
    public static List<AccountModel> Accounts { get; set; } = new();
    static public AccountModel? CurrentAccount { get; private set; }


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
                AccountModel account = new(ID, email.Input, password.Input, name.Input);
                Accounts.Add(account);
                CurrentAccount = account;
                JsonCommunicator.Write("Accounts.json", Accounts);
                BookingMenu.NextMenu();
            });
        }
        else { }
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
