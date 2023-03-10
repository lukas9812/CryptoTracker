namespace Assignment.Helpers;

public static class StreamConvertor
{
    public static Stream GenerateStreamFromString(string sValue)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(sValue);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}