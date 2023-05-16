static class Calendar
{
    public static string PrintCal(int year, int month, int minYear, int maxYear)
    {
        Console.Clear();

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
        calendar += $"    {(new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM")).ToUpper()}, {year}\n\n" +
                    "   |   Mo   Tu   We   Th   Fr   Sa   Su\n" +
                    "   |-------------------------------------\n";
        for (int i = 0; i < 6; i++)
        {
            calendar += "   |";
            for (int j = 1 - firstDay; j < 8 - firstDay; j++)
            {
                // check for current day
                if (month == currentMonth && year == currentYear && j + (7 * i) == currentDay && j + (7 * i) < 10)
                {
                    calendar += $"   [{j + (7 * i)}]";
                    continue;
                } 
                else if (month == currentMonth && year == currentYear && j + (7 * i) == currentDay && j + (7 * i) >= 10 && j + (7 * i) <= lastDayOfMonth)
                {
                    calendar += $"  [{j + (7 * i)}]";
                    continue;
                }

                // check if day has already passed
                if (((j + (7 * i) < currentDay && month <= currentMonth && year <= currentYear) || (month < currentMonth && year <= currentYear) || (year < currentYear)) && (j + (7 * i) > lastDayOfMonth || j + (7 * i) < 1))
                {
                    calendar += $"     ";
                    continue;
                }
                else if (((j + (7 * i) < currentDay && month <= currentMonth && year <= currentYear) || (month < currentMonth && year <= currentYear) || (year < currentYear)) && j + (7 * i) < 10)
                {
                    calendar += $"    \u001b[90m{j + (7 * i)}\u001b[0m";
                    continue;
                } 
                else if (((j + (7 * i) < currentDay && month <= currentMonth && year <= currentYear) || (month < currentMonth && year <= currentYear) || (year < currentYear)) && j + (7 * i) >= 10 && j + (7 * i) <= lastDayOfMonth)
                {
                    calendar += $"   \u001b[90m{j + (7 * i)}\u001b[0m";
                    continue;
                }

                // print the non current days
                if (j + (7 * i) > lastDayOfMonth || j + (7 * i) < 1)
                {
                    calendar += "     ";
                }
                else if (j + (7 * i) < 10)
                {
                    calendar += $"    {j + (7 * i)}";
                }
                else
                {
                    calendar += $"   {j + (7 * i)}";
                }
            }
            calendar += "\n   |\n";
        }
        return $" {calendar}";
    }

    // public static string PageControl(string nextOrPrev, int month, int year, int minYear, int maxYear)
    // {
    //     if (nextOrPrev == "Current")
    //     {
    //         return PrintCal(year, month, minYear, maxYear);
    //     }
    //     else if (nextOrPrev == "Next" && month < 12)
    //     {
    //         return PrintCal(year, month + 1, minYear, maxYear);
    //     }
    //     else if (nextOrPrev == "Prev" && month > 1)
    //     {
    //         return PrintCal(year, month - 1, minYear, maxYear);
    //     }
    //     else if (nextOrPrev == "Next" && month == 12)
    //     {
    //         return PrintCal(year + 1, 1, minYear, maxYear);
    //     }
    //     else if (nextOrPrev == "Prev" && month == 1)
    //     {
    //         return PrintCal(year - 1, 12, minYear, maxYear);
    //     }
    //     return "Invalid choice";
        // else if (key == ConsoleKey.UpArrow && year < maxYear)
        // {
        //     PrintCal(year + 1, month, minYear, maxYear);
        // }
        // else if (key == ConsoleKey.DownArrow && year > minYear)
        // {
        //     PrintCal(year - 1, month, minYear, maxYear);
        // }
    // }
}