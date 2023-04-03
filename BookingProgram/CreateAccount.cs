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
        Console.CursorVisible = true;
    
        Console.Write("Enter email address: ");
        string emailAddress = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        Console.Write("Enter full name: ");
        string fullName = Console.ReadLine();

        var jsondata = AccountsAccess.LoadAll();
        int id = 1;
        foreach( var data in jsondata )
        {
            var Id = data.Id;
            id++;
        }
    
        AccountModel account = new AccountModel(id, emailAddress, password, fullName);
        
        AccountsLogic accountlogic = new AccountsLogic();

        accountlogic.UpdateList(account);  

        BookingMenu.MainMenu();
    }
}