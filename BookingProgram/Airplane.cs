using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

class Airplane
{
    public static List<Airplane> Planes { get; set; } = new();
    public int ID { get; set; }
    public string Model { get; set; }

    public Airplane(string model)
    {
        Model = model;
        ID = Planes.Count;
        Planes.Add(this);
    }

    public override string ToString()
    {
        return $"ID: {ID} Model: {Model}";
    }
}
