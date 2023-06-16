class PremiumGroup : Group
{
    public int Size { get; set; }
    public string Side { get; set; }
    public PremiumGroup(int size, string side = ""): base(size)
    {
        Side = side;
    }
}
