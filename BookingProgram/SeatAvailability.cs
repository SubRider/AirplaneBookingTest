static class SeatAvailability
{
    public static void CheckAvailability(Flight flight) 
    {
        List<Seat> s1 = FillSeats(flight, "First", flight.FirstClassRowSize);
        List<Seat> s2 = FillSeats(flight, "Business", flight.BusinessClassRowSize);
        List<Seat> s3 = FillSeats(flight, "Economy", flight.EconomyClassRowSize);

        int g1 = FindBiggestGroup(s1, flight.FirstClassRowSize);
        int g2 = FindBiggestGroup(s2, flight.BusinessClassRowSize);
        int g3 = FindBiggestGroup(s3, flight.EconomyClassRowSize);
        
        (bool,bool) aAw1 = CheckAisleAndWindow(s1, flight.FirstClassRowSize);
        (bool, bool) aAw2 = CheckAisleAndWindow(s2, flight.BusinessClassRowSize);
        (bool, bool) aAw3 = CheckAisleAndWindow(s3, flight.EconomyClassRowSize);
        flight.FirstClassAvailability = new(g1, aAw1.Item1, aAw1.Item2);
        flight.BusinessClassAvailability = new(g2, aAw2.Item1, aAw2.Item2);
        flight.EconomyClassAvailability = new(g3, aAw3.Item1, aAw3.Item2);
        
    }
    public static List<Seat> FillSeats(Flight flight, string seatClass, int rowSize)
    {
        List<Seat> simulatedSeats = new(flight.Seats);
        List<Group> bookedGroups = new(flight.Groups);
        List<List<Seat>> smallestGroups = new();

        while (bookedGroups.Count > 0)
        {
            List<int> bookedGroupSizes = new();
            smallestGroups = FindSmallestGroups(simulatedSeats, seatClass, rowSize);
            smallestGroups = new(smallestGroups.OrderBy(n => n.Count).ToList());
            foreach (Group group in bookedGroups) bookedGroupSizes.Add(group.Size);
            bookedGroupSizes = new(bookedGroupSizes.Distinct().OrderBy(n => n).ToList());
            foreach(int i in Enumerable.Range(0, smallestGroups.Count))
            {
                if (smallestGroups[i].Count >= bookedGroupSizes.Min())
                {
                    for (int j = 0; j < bookedGroupSizes.Min(); j++)
                    {
                        simulatedSeats.Find(s => s.SeatNumber == smallestGroups[i][j].SeatNumber && s.RowNumber == smallestGroups[i][j].RowNumber).Booked = true;
                    }
                    bookedGroups.Remove(bookedGroups.Where(g => g.Size == bookedGroupSizes.Min()).First());
                    break;
                }
                else continue;
            }
        }
        smallestGroups = FindSmallestGroups(simulatedSeats, seatClass, rowSize);
        return simulatedSeats;
    }
    private static (bool, bool) CheckAisleAndWindow(List<Seat> seats, int rowSize)
    {
        bool aisle = false;
        bool window = false;
        List<int> threeDivisibleSeats = new() { 3, 4, 6, 7 };
        foreach (Seat seat in seats)
        {
            if ((seat.SeatNumber == 1 || seat.SeatNumber == rowSize) && !seat.Booked) window = true;
            if (rowSize % 3 == 0 && threeDivisibleSeats.Contains(seat.SeatNumber) && !seat.Booked) aisle = true;
            else if (seat.SeatNumber == 2 || seat.SeatNumber == 3 && !seat.Booked) aisle = true;
            if (window && aisle) break;
        }
        return (aisle, window);
    }

    private static int FindBiggestGroup(List<Seat> seats, int maxRowSize)
    {
        List<int> emptyRows = new();
        int biggestGroup = new();

        foreach (Seat seat in seats)
        {
            int emptySeats = 0;
            if (!seat.Booked) emptySeats++;
            else continue;
            if (emptySeats == maxRowSize)
            {
                emptyRows.Add(seat.RowNumber);
            }
        }
        int groupSize = 0;
        foreach (int row in Enumerable.Range(0,emptyRows.Count))
        {
            if (emptyRows[row] == emptyRows[row + 1] - 1)
            {
                groupSize += maxRowSize;
            }
            else
            {
                if (groupSize > biggestGroup) biggestGroup = groupSize;
                groupSize = 0;
            }
        }
        return biggestGroup;
    }
    private static List<List<Seat>> FindSmallestGroups(List<Seat> seats,string seatClass, int maxRowSize)
    {
        List<List<Seat>> smallestGroups = new();
        List<Seat> group = new();
        List<Seat> smallestGroup = new();


        foreach (Seat seat in seats.Where(s => s.SeatClass == seatClass))
        {
            int row = seat.RowNumber;
            if (!seat.Booked) group.Add(seat);
            else
            {
                if (smallestGroup.Count == 0) smallestGroup = new(group);
                else if (group.Count < smallestGroup.Count && group.Count != 0) smallestGroup = new(group);
                group = new();
            }
            if (seat.SeatNumber == maxRowSize)
            {
                if (smallestGroup.Count == 0) smallestGroup = new(group);
                else if (group.Count < smallestGroup.Count && group.Count != 0) smallestGroup = new(group);
                if (smallestGroup.Count != 0) smallestGroups.Add(smallestGroup);
                smallestGroup = new();
                group = new();
            }
        }        
        return smallestGroups;
    }
}