static class Calendar
{
    public static void PrintCal(int year, int month)
    {
        int minYear = 2010;
        int maxYear = 2060;

        Console.Clear();
        // validate year and month
        if (year < minYear || year > maxYear)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid year. Please enter a year between 2023 and 2025.");
            Console.ForegroundColor = ConsoleColor.Gray;
            return;
        }

        if (month < 1 || month > 12)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid month. Please enter a valid month between 1 and 12.");
            Console.ForegroundColor = ConsoleColor.Gray;
            return;
        }

        // determine first- and last day of month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DayOfWeek startingDay = firstDayOfMonth.DayOfWeek;
        int firstDay = ((int)startingDay + 7 - 1) % 7;

        int lastDayOfMonth = DateTime.DaysInMonth(year, month);

        // print calendar
        Console.WriteLine($"{new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM")}, {year}\n");
        Console.WriteLine("|   Mo   Tu   We   Th   Fr   Sa   Su");
        Console.WriteLine("|-------------------------------------");
        for (int i = 0; i < 6; i++)
        {
            Console.Write("|");
            for (int j = 1 - firstDay; j < 8 - firstDay; j++)
            {
                if (j + (7 * i) > lastDayOfMonth || j + (7 * i) < 1)
                {
                    Console.Write("     ");
                }
                else if (j + (7 * i) < 10)
                {
                    Console.Write($"    {j + (7 * i)}");
                }
                else
                {
                    Console.Write($"   {j + (7 * i)}");
                }
            }
            Console.WriteLine("\n|");
        }

        // page controls
        bool quit = false;
        while (!quit)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            ConsoleKey key = keyInfo.Key;

            if (key == ConsoleKey.RightArrow && month < 12)
            {
                PrintCal(year, month + 1);
            }
            else if (key == ConsoleKey.LeftArrow && month > 1)
            {
                PrintCal(year, month - 1);
            }
            else if (key == ConsoleKey.RightArrow && month == 12)
            {
                PrintCal(year + 1, 1);
            }
            else if (key == ConsoleKey.LeftArrow && month == 1)
            {
                PrintCal(year - 1, 12);
            }
            else if (key == ConsoleKey.UpArrow && year < maxYear)
            {
                PrintCal(year + 1, month);
            }
            else if (key == ConsoleKey.DownArrow && year > minYear)
            {
                PrintCal(year - 1, month);
            }
            else if (key == ConsoleKey.Q)
            {
                Console.Clear();
                quit = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{key} is not a valid key");
                Thread.Sleep(1000);
                ClearLastLine();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
    public static void ClearLastLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, currentLineCursor - 1);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor - 2);
    }
}