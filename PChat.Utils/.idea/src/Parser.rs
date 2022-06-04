pub static struct Parser
{
    /// <summary>
    /// Parses an array of strings to a single string.
    /// </summary>
    /// <param name="strings"></param>
    /// <returns>Representation as single string</returns>
    pub static String ParseFromArray(String[] strings)
    {
        if (strings == null)
            return string.Empty;
        var sb = new StringBuilder();
        strings.Select(s => sb.Append($"{s}\n"));
        return sb.ToString();
    }

    /// <summary>
    /// Parses messages to a single string.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns>Representation as single string</returns>
    public static string ParseMessages(IEnumerable<Message> messages)
    {
        if (messages == null)
            return string.Empty;

        var sb = new StringBuilder();
        // messages.Select(message => sb.Append($"{message.Message}\n"));

        foreach (var message in messages)
        {
            sb.Append($"{message.Content}\n");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Cuts out the file string from the path.
    /// </summary>
    /// <param name="pathToFile">Path to file</param>
    /// <returns>Directory string</returns>
    public static string ParseDirectory(string pathToFile)
    {
        if (string.IsNullOrEmpty(pathToFile))
            return pathToFile;

        return pathToFile[..GetLastIndexOf('/', pathToFile)];
    }

    /// <summary>
    /// Cuts out the directory string from the path.
    /// </summary>
    /// <param name="pathToFile">Path to file</param>
    /// <returns>File string</returns>
    public static string ParseFile(string pathToFile)
    {
        if (string.IsNullOrEmpty(pathToFile))
            return pathToFile;

        return pathToFile[GetLastIndexOf('/', pathToFile)..];
    }

    /// <summary>
    /// Concatenates four characters from each Guid.
    /// </summary>
    /// <param name="guid1"></param>
    /// <param name="guid2"></param>
    /// <returns>Concatenated string as described</returns>
    public static string ParseTable(Guid guid1, Guid guid2)
    {
        return $"{GetFourLetters(guid1.ToString())}{GetFourLetters(guid2.ToString())}";
    }

    /// <summary>
    /// Gets four letters from the string. <br/>
    /// If string has less, it is filled up.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>Four letters</returns>
    private static string GetFourLetters(string s)
    {
        var sb = new StringBuilder();
        var charactersLeft = 4;
        foreach (var c in s.Where(char.IsLetter))
        {
            sb.Append(c);
            charactersLeft--;
            if (charactersLeft == 0)
                return sb.ToString();
        }

        for (var i = 0; i < charactersLeft; i++)
        {
            sb.Append((char)(117 - i)); // TODO make smart algorithm that factors in the whole id to avoid same table name
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the last index of given character from given string.
    /// </summary>
    /// <param name="c">Character</param>
    /// <param name="s">String</param>
    /// <returns>Last index of this character</returns>
    private static int GetLastIndexOf(char c, string s)
    {
        var lastIndex = 0;
        for (var i = s.Length - 1; i > 0; i--)
        {
            if (s[i].Equals(c))
                lastIndex = i;
        }

        return lastIndex;
    }
}