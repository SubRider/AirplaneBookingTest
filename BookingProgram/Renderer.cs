using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

static class Renderer
{
    public static void ShowButton(Button button)
    {
        Console.SetCursorPosition(0, button.YPosition);
        Console.ForegroundColor = button.Color;
        Console.Write(button.Text);
        Console.ResetColor();
        Console.SetCursorPosition(0, button.YPosition);
    }
    public static void ShowButtons()
    {
        foreach (Button button in Button.Buttons)
        {
            ShowButton(button);
        }
    }
    public static void HighlightButton(Button button, bool dehilight = false)
    {
        if (dehilight)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, button.YPosition);
            Console.ForegroundColor = button.Color;
            Console.Write(button.Text);
            Console.ResetColor();
            Console.SetCursorPosition(0, button.YPosition);
            button.HighlightTime = DateTime.MinValue;
        }
        TimeSpan timeHighlighed = DateTime.Now - button.HighlightTime;
        if (timeHighlighed.TotalMilliseconds > 500)
        {
            if (button.Highlighted)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                button.Highlighted = false;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                button.Highlighted = true;
            }
            button.HighlightTime = DateTime.Now;
            Console.SetCursorPosition(0, button.YPosition);
            Console.ForegroundColor = button.Color;
            Console.Write(button.Text);
            Console.ResetColor();
            Console.SetCursorPosition(0, button.YPosition);
        }
    }
}
