﻿using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

class Airplane : IHasID
{
    public static List<Airplane> Planes { get; set; } = new();
    public int ID { get; set; }
    public string Model { get; set; }

    public Airplane(string model)
    {
        Model = model;
        ID = Planes.Count + 1;
        Planes.Add(this);
    }
    
    public static Airplane FindByID(int id)
    {
        return Planes.Find(f => f.ID == id);
    }
    
    public override string ToString()
    {
        return $"ID: {ID} Model: {Model}";
    }
}