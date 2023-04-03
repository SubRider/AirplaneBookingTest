using System.Text.Json;

static class JsonCommunicator
{

    public static void Write<T>(string path, List<T> objectList)
    {
        using StreamWriter writer = new(path);
        string jsonString = JsonSerializer.Serialize(objectList);
        writer.Write(jsonString);
    }
    public static List<T> Read<T>(string path)
    {
        using StreamReader reader = new(path);
        string jsonString = reader.ReadToEnd();
        if (jsonString != "")
        {
            List<T> objectList = JsonSerializer.Deserialize<List<T>>(jsonString);
            return objectList;
        }
        return new();
        
    }
}