class UserAccount
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Phone { get; set; }
    public int LoyaltyPoints { get; set; }

    public UserAccount(string firstName, string lastName, string email, int phone, int loyaltyPoints)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        LoyaltyPoints = loyaltyPoints;
    }
}
