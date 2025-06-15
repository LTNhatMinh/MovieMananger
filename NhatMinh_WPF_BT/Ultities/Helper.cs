using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public static class Helper
    {
        public static void InputNumber(string str, out int n)
        {
            Console.Write(str + ": ");

            while (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("Invalid input. Please enter a valid number: ");
                Console.Write(str + ": ");
            }
        }

        public static void InputNumber(string str, out double n)
        {
            Console.Write(str + ": ");
            while (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out n) || n <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid number: ");
                Console.Write(str + ": ");
            }
        }

        public static void InputNumber(string str, out long n)
        {
            Console.Write(str + ": ");

            while (!long.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("Invalid input. Please enter a valid number: ");
                Console.Write(str + ": ");
            }
        }

        public static void Input(string str, out string s)
        {
            Console.Write(str + ": ");
            s = Console.ReadLine();

            while (string.IsNullOrEmpty(s))
            {
                Console.WriteLine("Invalid input. Please enter a valid text:");
                Console.Write(str + ": ");
                s = Console.ReadLine();
            }
        }
        public static string FormatNumberWithDots(int number)
        {
            return string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:N0}", number);
        }

        public static void InputDate(string str, out DateOnly date)
        {
            Console.Write(str + " (dd/MM/yyyy): ");

            while (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (dd/MM/yyyy): ");
                Console.Write(str + " (dd/MM/yyyy): ");
            }
        }

        public static void InputDateTime(string str, out DateTime dateTime)
        {
            Console.Write($"{str} (dd/MM/yyyy HH:mm): ");

            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine("Invalid date-time format. Please enter a valid date and time (dd/MM/yyyy HH:mm): ");
                Console.Write($"{str} (dd/MM/yyyy HH:mm): ");
            }
        }

        public static void InputDateTimeWithDefaultTime(string str, out DateTime dateTime)
        {
            Console.Write($"{str} (dd/MM/yyyy): ");

            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (dd/MM/yyyy): ");
                Console.Write($"{str} (dd/MM/yyyy): ");
            }
        }

        public static void InputTimeOnlyWithDate(DateTime datePart, out DateTime dateTime)
        {
            Console.Write("Enter time (HH:mm): ");
            TimeSpan timePart;

            while (!TimeSpan.TryParseExact(Console.ReadLine(), "hh\\:mm", null, out timePart))
            {
                Console.WriteLine("Invalid time format. Please enter a valid time (HH:mm): ");
                Console.Write("Enter time (HH:mm): ");
            }

            dateTime = datePart.Date + timePart;
        }


        public static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Length--;
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (true);

            Console.WriteLine();
            return password.ToString();
        }
    }
}
