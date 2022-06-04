using System.Text;
using PChat.Utils;

namespace PChat.Log;

public class PLogger
{
    public static PLogger Singleton { get; set; } = new PLogger();

    public void OverwriteFile(string pathToFile, string content)
    {
        if (string.IsNullOrEmpty(content))
            return;
        
        var fileStream = File.Open(pathToFile, FileMode.OpenOrCreate);
        var data = Encoding.UTF8.GetBytes(content);

        if (fileStream.CanWrite)
            fileStream.WriteAsync(data);
        fileStream.Close();
    }

    public void ToConsole(string content, string tag = "")
    {
        System.Console.WriteLine($"{tag}{content}");
    }

    public string ReadFromFile(string pathToFile)
    {
        try
        {
            var fileStream = File.Open(pathToFile, FileMode.Open);
            var data = new byte[1024];

            if (fileStream.CanRead)
                fileStream.ReadAsync(data);
            fileStream.Close();

            // Hint: maybe UTF32 could be better for asian and other characters (if UTF32 even contains that)
            var dataString = Encoding.UTF8.GetString(data);
            return dataString;
        }
        catch (FileNotFoundException)
        {
            return string.Empty;
        }
    }

    public List<string> ReadLines(string pathToFile)
    {
        try
        {
            return File.ReadAllLines(pathToFile).ToList();
        }
        catch (DirectoryNotFoundException)
        {
            Directory.CreateDirectory(Parser.ParseDirectory(pathToFile));
            return new List<string>();
        }
        catch (FileNotFoundException)
        {
            return new List<string>();
        }
    }

    public void AppendLine(string pathToFile, string line)
    {
        try
        {
            var stream = File.AppendText(pathToFile);
            stream.WriteLine(line);
            stream.Close();
        }
        catch (FileNotFoundException)
        {
            File.Create(pathToFile);
            AppendLine(pathToFile, line);
        }
        catch (DirectoryNotFoundException)
        {
            var directory = pathToFile[..pathToFile.LastIndexOf('/')];
            Directory.CreateDirectory(directory);
            AppendLine(pathToFile, line);
        }
    }


}