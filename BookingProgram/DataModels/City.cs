public class City : IHasID
{
    private static List<string> _cityList = new()
    {
        "Abu Dhabi", "Abuja", "Accra", "Addis Ababa", "Algiers", "Amman", "Amsterdam", "Ankara", "Antananarivo", "Apia",
        "Ashgabat", "Asmara", "Astana", "Asunción", "Athens", "Avarua", "Baghdad", "Baku", "Bamako",
        "Bandar Seri Begawan", "Bangkok", "Bangui", "Banjul", "Basseterre", "Beijing", "Beirut", "Belgrade", "Belmopan",
        "Berlin", "Bern", "Bishkek", "Bissau", "Bogotá", "Brasília", "Bratislava", "Brazzaville", "Bridgetown",
        "Brussels", "Bucharest", "Budapest", "Buenos Aires", "Bujumbura", "Cairo", "Canberra", "Caracas", "Castries",
        "Cayenne", "Charlotte Amalie", "Chisinau", "Cockburn Town", "Colombo", "Conakry", "Copenhagen", "Dakar",
        "Damascus", "Dhaka", "Dili", "Djibouti", "Dodoma", "Doha", "Dublin", "Dushanbe", "Edinburgh", "Freetown",
        "Funafuti", "Gaborone", "Georgetown", "Gibraltar", "Guatemala City", "Hanoi", "Harare", "Havana", "Helsinki",
        "Honiara", "Islamabad", "Jakarta", "Jerusalem", "Juba", "Kabul", "Kampala", "Kathmandu", "Khartoum", "Kiev",
        "Kigali", "Kingston", "Kingstown", "Kinshasa", "Kuala Lumpur", "Kuwait City", "Libreville", "Lilongwe", "Lima",
        "Lisbon", "Ljubljana", "Lomé", "London", "Luanda", "Lusaka", "Luxembourg", "Madrid", "Majuro", "Malabo", "Male",
        "Mamoudzou", "Managua", "Manama", "Manila", "Maputo", "Maseru", "Mbabane", "Mexico City", "Minsk", "Mogadishu",
        "Monaco", "Monrovia", "Montevideo", "Moroni", "Moscow", "Muscat", "Nairobi", "Nassau", "Naypyidaw", "N'Djamena",
        "New Delhi", "Niamey", "Nicosia", "Nouakchott", "Nuku'alofa", "Oslo", "Ottawa", "Ouagadougou", "Palikir",
        "Panama City", "Paramaribo", "Paris", "Phnom Penh", "Podgorica", "Port Louis", "Port Moresby", "Port of Spain",
        "Port Vila", "Port-au-Prince", "Porto-Novo", "Prague", "Praia", "Pretoria", "Pyongyang", "Quito", "Rabat",
        "Reykjavík", "Riga", "Riyadh", "Road Town", "Rome", "Roseau", "Saint George's", "Saint John's", "San José",
        "San Juan", "San Marino", "San Salvador", "Sana'a", "Santiago", "Santo Domingo", "São Tomé", "Sarajevo",
        "Seoul", "Singapore", "Skopje", "Sofia", "Stockholm", "Sucre", "Sukhumi", "Suva", "Taipei", "Tallinn", "Tarawa",
        "Tashkent", "Tbilisi", "Tegucigalpa", "Tehran", "Thimphu", "Tirana", "Tokyo", "Tripoli", "Tskhinvali", "Tunis",
        "Ulaanbaatar", "Vaduz", "Valletta", "Vatican City", "Victoria", "Vienna", "Vientiane", "Vilnius", "Warsaw",
        "Washington, D.C.", "Wellington", "Windhoek", "Yaoundé", "Yerevan", "Zagreb"
    };

    public static List<City> Cities = new();
    public int ID { get; set; }
    public string Name { get; set; }

    public City(int iD, string city)
    {
        ID = iD;
        Name = city;
    }

    public static void CreateCountryList()
    {
        foreach (string city in _cityList)
        {
            int id = Cities.Count;
            City capital = new City(id, city);
            Cities.Add(capital);
        }
    }   

    public override string ToString()
    {
        return $"{Name}";
    }
}
