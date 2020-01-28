namespace HttpConnector
{
    using System;

    public static class Extensions
    {
        public static void Print(this string message, ConsoleColor color = ConsoleColor.Green)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
    }
}
