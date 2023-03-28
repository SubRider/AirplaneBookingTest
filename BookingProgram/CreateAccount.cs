using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
class CreateAccount
{
    public static void userInfo()
    {
        Button.Clear();
        Console.Clear();
        Console.Write("Enter account ID: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter email address: ");
        string emailAddress = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        Console.Write("Enter full name: ");
        string fullName = Console.ReadLine();

        
        AccountModel account = new AccountModel(id, emailAddress, password, fullName);
        
        AccountsLogic accountlogic = new AccountsLogic();

        // deze line zou iest anders moeten returnen 
        accountlogic.UpdateList(account);  

        BookingMenu.StartScreen();
    }
}