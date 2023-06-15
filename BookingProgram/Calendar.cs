static class Calendar
{
    public static string PrintCal(int year, int month, int minYear, int maxYear, Window w1, string direction, int selectedMonth, int selectedYear)
    {
        Console.Clear();
        DateTime Date = default;

        // determine first- and last day of month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DayOfWeek startingDay = firstDayOfMonth.DayOfWeek;
        int firstDay = ((int)startingDay + 7 - 1) % 7;

        int lastDayOfMonth = DateTime.DaysInMonth(year, month);

        DateTime currentDate = DateTime.Now;
        int currentDay = currentDate.Day;
        int currentMonth = currentDate.Month;
        int currentYear = currentDate.Year;

        // print calendar buttons
        int dayNumber = 1;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Action errorAction = () => 
                {
                    Renderer.Clear();
                    Console.WriteLine("\u001b[91mThis date is not available");
                    Thread.Sleep(850);
                    BookingMenu.CalendarMenu(DateTime.Now.Month, DateTime.Now.Year, direction);
                };
                int number = dayNumber;

                int day = j + (7 * i);
                if (day < firstDay) continue;
                if (dayNumber > lastDayOfMonth)
                {
                    _ = new Button(" ", (i + 4) * 2, j + 1, w1, () => { }, false);
                }
                else 
                {
                    if (year > currentYear) _ = new Button($"{dayNumber}", (i + 4) * 2, j + 1, w1, () =>
                    {
                        if (direction == "Departure")
                        {
                            BookingMenu.FlightSearchMenu(false, "", "", $"{number}-{selectedMonth}-{selectedYear}", "");
                        }
                        else BookingMenu.FlightSearchMenu(false, "", "", "", $"{number}-{selectedMonth}-{selectedYear}");
                    });
                    else if (year == currentYear)
                    {
                        if (month == currentMonth)
                        {
                            if (dayNumber >= currentDay) _ = new Button($"{dayNumber}", (i + 4) * 2, j + 1, w1, () =>
                            {
                                if (direction == "Departure")
                                {
                                    BookingMenu.FlightSearchMenu(false, "", "", $"{number}-{selectedMonth}-{selectedYear}", "");
                                }
                                else BookingMenu.FlightSearchMenu(false, "", "", "", $"{number}-{selectedMonth}-{selectedYear}");
                            });
                            else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", (i + 4) * 2, j + 1, w1, errorAction);
                        }

                        else if (month > currentMonth) _ = new Button($"{dayNumber}", (i + 4) * 2, j + 1, w1, () =>
                        {
                            if (direction == "Departure")
                            {
                                BookingMenu.FlightSearchMenu(false, "", "", $"{number}-{selectedMonth}-{selectedYear}", "");
                            }
                            else BookingMenu.FlightSearchMenu(false, "", "", "", $"{number}-{selectedMonth}-{selectedYear}");
                        });
                        else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", (i + 4) * 2, j + 1, w1, errorAction);
                    }
                    else _ = new Button(ConsoleColor.DarkGray, $"{dayNumber}", (i + 4) * 2, j + 1, w1, errorAction);
                }
                dayNumber++;
            }
        }
        List<Button> B = Button.Buttons;

        
        
        return $"";
    }
}