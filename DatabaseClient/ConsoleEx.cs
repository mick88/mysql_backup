using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseClient
{
    /// <summary>
    /// Extra console methods
    /// </summary>
    static class ConsoleEx
    {
        private const int DEFAULT_PROGRESS_SIZE = 40;
        private const ConsoleColor
            ERROR_COLOR = ConsoleColor.Red,
            WARNING_COLOR = ConsoleColor.Yellow,
            PROMPT_COLOR = ConsoleColor.DarkGreen;

        public static void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteColor(string text, ConsoleColor color, ConsoleColor bgColor)
        {
            Console.BackgroundColor = bgColor;
            WriteColor(text, color);
        }

        public static void WriteLineColor(string text, ConsoleColor color)
        {
            WriteColor(text, color);
            Console.WriteLine();
        }

        public static void WriteLineColor(string text, ConsoleColor color, ConsoleColor bgColor)
        {
            WriteColor(text + Environment.NewLine, color, bgColor);
        }

        public static void WriteError(string message)
        {
            WriteLineColor("ERROR: " + message, ERROR_COLOR);
        }

        public static void WriteWarning(string message)
        {
            WriteLineColor("WARNING: " + message, WARNING_COLOR);
        }

        public static string GetInput(string prompt)
        {
            WriteColor(prompt, PROMPT_COLOR);
            return Console.ReadLine();
        }

        public static string GetInput()
        {
            return GetInput("Input: ");
        }

        public static void PressAnyKeyPrompt()
        {
            Console.Write("Press any key to continue..");
            Console.ReadKey();
        }

        public static bool PromptYn(string prompt)
        {
            string message = string.Format("{0} (Y/N)", prompt);
            Console.Write(message);

            char response = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (response)
            {
                case 'y': return true;
                case 'n': return false;
                default: return PromptYn(prompt);
            }
        }

        public static StringBuilder BuildProgressBar(float progress, int size)
        {
            if (progress > 1f) progress = 1f;

            int filled = (int)((float)size * progress),
                empty = size - filled;
            StringBuilder builder = new StringBuilder("\r[");
            for (int i = 0; i < filled; i++) builder.Append('#');
            for (int i = 0; i < empty; i++) builder.Append(' ');
            builder.Append(']');

            return builder;
        }

        public static void DrawProgressBar(float progress)
        {
            DrawProgressBar(progress, DEFAULT_PROGRESS_SIZE);
        }

        public static void DrawProgressBar(float progress, int size)
        {
            Console.Write(new StringBuilder('\r').Append(BuildProgressBar(progress, size)));
        }
    }
}
