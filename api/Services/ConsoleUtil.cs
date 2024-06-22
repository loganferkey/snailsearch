using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    /// <summary>
    /// A class designed to print to the console various information, primarily debugging/perf details
    /// </summary>
    public class ConsoleUtil
    {
        private Stopwatch stopwatch;
        private ConsoleColor preferredTextColor;
        private ConsoleColor preferredBackgroundColor;

        public ConsoleUtil()
        {
            // Create instance of stopwatch, scoped for multiple timers per instance
            this.stopwatch = new Stopwatch();
            // Init text colors for printing
            this.preferredTextColor = Console.ForegroundColor;
            this.preferredBackgroundColor = Console.BackgroundColor;
        }

        /// <summary>
        /// Starts a stopwatch at the current line of code with debug information
        /// </summary>
        /// <param name="message">Optional: The message you want to print to the terminal when started</param>
        /// <param name="textColor">Optional: The color of the text to be printed</param>
        /// <param name="backgroundColor">Optional: The color behind the text</param>
        public void StartTimer(string? message = null, ConsoleColor? textColor = null, ConsoleColor? backgroundColor = null)
        {
            if (this.stopwatch.IsRunning)
                return; // Fail start if the timer is currently running
            // Check to see if a text color is provided, if so store the color and change text color OR leave it default (but still store it)
            if (textColor != null)
                this.preferredTextColor = (ConsoleColor)textColor;
            if (backgroundColor != null)
                this.preferredBackgroundColor = (ConsoleColor)backgroundColor;
            Console.ForegroundColor = this.preferredTextColor;
            Console.BackgroundColor = this.preferredBackgroundColor;

            // Write the message and start the stopwatch
            if (message != null)
                Console.WriteLine(message);
            Console.ResetColor();
            this.stopwatch.Start();
        }

        /// <summary>
        /// Stop the ongoing timer and fetch the time, optionally print a message
        /// </summary>
        /// <param name="message">Optional: Print a message that can hold the duration inside of it, see function for help</param>
        /// <returns>Returns the duration of the stopwatch since it was started</returns>
        public long StopTimer(string? message = null)
        {
            if (!this.stopwatch.IsRunning)
                return 0; // Wasn't running so no time has passed

            // Stop the watch and return the time!
            this.stopwatch.Stop();
            if (message != null)
            {
                // If your string contains [t] inside of it, [t] will be replaced with the stopwatch time
                Console.ForegroundColor = this.preferredTextColor;
                Console.BackgroundColor = this.preferredBackgroundColor;
                if (message.Contains("[t]"))
                {
                    // You wanted the time included within the message
                    string[] sliced = message.Split("[t]", StringSplitOptions.None);
                    Console.WriteLine(string.Join(this.stopwatch.ElapsedMilliseconds.ToString(), sliced));
                }
                else
                {
                    // You didn't want to add the time into the message automatically
                    Console.WriteLine(message);
                }
                Console.ResetColor();
            }

            return this.stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Write a line in the terminal with provided color
        /// </summary>
        /// <param name="message">The message you want to write</param>
        /// <param name="color">The color it should be in</param>
        public void WriteColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}