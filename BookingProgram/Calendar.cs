static class Calendar
{
    public static string PrintCal(int year, int month, int minYear, int maxYear, Window w1)
    {
        Console.Clear();
        DateTime Date = default;

        // validate year and month
        if (year < minYear || year > maxYear)
        {
            return $"Invalid year. Please enter a year between {minYear} and {maxYear}.";
        }

        if (month < 1 || month > 12)
        {
            return "Invalid month. Please enter a valid month between 1 and 12.";
        }

        // determine first- and last day of month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DayOfWeek startingDay = firstDayOfMonth.DayOfWeek;
        int firstDay = ((int)startingDay + 7 - 1) % 7;

        int lastDayOfMonth = DateTime.DaysInMonth(year, month);

        DateTime currentDate = DateTime.Now;
        int currentDay = currentDate.Day;
        int currentMonth = currentDate.Month;
        int currentYear = currentDate.Year;

        // print calendar
        string calendar = "\n";
        /*        calendar += $"║   {(new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM")).ToUpper()}, {year}\n\n" +
                            "║  ┌──────────────────────────────────────┐\n" +
                            "║  │   Mo   Tu   We   Th   Fr   Sa   Su   │\n" +
                            "║  ├──────────────────────────────────────┤\n";*/
        int dayNumber = 1;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                
                int day = j + (7 * i);
                if (day < firstDay) continue;
                if (dayNumber > lastDayOfMonth)
                {
                    _ = new Button(" ", i + 2, j, w1, () => { }, false);
                }
                else 
                {
                    if (year > currentYear) _ = new Button($"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, day); });
                    else if (year == currentYear)
                    {
                        if (month == currentMonth)
                        {
                            if (dayNumber >= currentDay) _ = new Button($"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, dayNumber); });
                            else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, dayNumber); });
                        }
                        else if (month > currentMonth) _ = new Button($"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, dayNumber); });
                        else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, dayNumber); });
                    }
                    else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", i + 2, j, w1, () => { Date = new DateTime(year, month, dayNumber); });
                }
                dayNumber++;
            }
        }
        List<Button> B = Button.Buttons;
        //calendar += "║  └──────────────────────────────────────┘";
        return $"";
    }
}