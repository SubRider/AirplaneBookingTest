class Seat
{
    public string Class { get; set; }
    public int RowNumber { get; set; }
    public char SeatLetter { get; set; }
    public int SeatNumber { get; set; }

    public bool Booked { get; set; }

    public Seat(string _class, int rowNumber, int seatNumber)
    {
        List<char> seatLetters = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        Class = _class;
        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatLetter = seatLetters[seatNumber-1];
        Booked = false;
    }

}
