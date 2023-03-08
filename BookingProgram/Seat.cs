class Seat
{
    public string Class { get; set; }
    public int RowNumber { get; set; }
    public char SeatLetter { get; set; }

    public Seat(string _class, int rowNumber, char seatLetter)
    {
        Class = _class;
        RowNumber = rowNumber;
        SeatLetter = seatLetter;
    }

}
