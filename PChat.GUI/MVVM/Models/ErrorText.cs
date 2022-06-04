using Avalonia.Media;

namespace PChat.GUI.MVVM.Models;

public class ErrorText
{
    public ErrorText(string error, Color color)
    {
        Error = error;
        Color = color;
    }
    public string Error { get; }
    public Color Color { get; }
    public string ColorString => Color.ToString();
}