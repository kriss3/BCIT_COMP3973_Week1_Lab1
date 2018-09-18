using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace NorthwindApp
{
    public class Helper
    {
        public static String DrawSeparator(int numberOfSeparators)
        {
            return string.Concat(Enumerable.Repeat("=", numberOfSeparators));
        }

        public static void DrawGetTopCustomerHeader()
        {
            ForegroundColor = ConsoleColor.Blue;
            WriteLine($"{"Id",2} {"Cust ID",-10}\t{"Company Name",-35}\t{"Contact Name",-20}\t{"Country",-10}\t{"City",-10}");
            ForegroundColor = ConsoleColor.Gray;
        }

        public static void DisplayErrorMessage(string msg)
        {
            WriteLine();
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"Something went wrong. If necessry, please contact staff member with error message:\n {msg}");
            ForegroundColor = ConsoleColor.Gray;
        }
    }
}
