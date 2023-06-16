class AvailabilityModel
{
    public int MaxGroupSize { get; set; }
    public bool WindowAvailable { get; set; }
    public bool AisleAvailable { get; set; }

    public AvailabilityModel(int maxGroupSize, bool windowAvailable, bool aisleAvailable)
    {
        MaxGroupSize = maxGroupSize;
        WindowAvailable = windowAvailable;
        AisleAvailable = aisleAvailable;
    }
}