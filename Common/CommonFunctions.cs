using System.Globalization;
using System.Text.RegularExpressions;

namespace SeleniumPOC.Common
{
    internal static class CommonFunctions
    {
        public static decimal ConvertCurrencyToDecimal(string currency)
        {
            return decimal.Parse(currency, NumberStyles.AllowCurrencySymbol | NumberStyles.Number);
        }

        public static string ConvertDecimalToCurrencyString(decimal value)
        {
            return value.ToString("C");
        }

        public static string GenerateRandomDollarAmount(int min, int max)
        {
            var random = new Random();
            string dollarAmount = "$" + random.Next(min, max) + "." + random.Next(1, 99).ToString().PadLeft(2, '0');
            return dollarAmount;
        }

        public static string FormatDollarAmount(string amountInput)
        {
            if (decimal.TryParse(amountInput, out decimal amount))
            {
                return $"${amount:0.00}";
            }
            else
            {
                throw new ArgumentException("Invalid amount input. Please provide a valid number.");
            }
        }

        public static string GenerateRandomWholeDollarAmount(int min, int max)
        {
            var random = new Random();
            string dollarAmount = "$" + random.Next(min, max) + ".00";
            return dollarAmount;
        }

        public static bool FuzzyCurrencyCompare(string expectedCurrency, string actualCurrency, decimal allowedDifference)
        {
            decimal result = Math.Abs(ConvertCurrencyToDecimal(expectedCurrency) - ConvertCurrencyToDecimal(actualCurrency));
            Console.WriteLine("Expected: " + expectedCurrency + " Actual: " + actualCurrency + " with an allowed diff of " + allowedDifference);
            return (result < allowedDifference);
        }

        public static bool IsWithinStandardTradingHours()
        {
            TimeSpan start = new TimeSpan(8, 31, 0); // Market open CST
            TimeSpan end = new TimeSpan(14, 57, 0);  // Market close CST
            TimeSpan now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).TimeOfDay;

            if ((now > start) && (now < end))
                return true;
            else
                return false;
        }

        public static string GenerateRandomName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "k", "l", "m", "n", "p", "q", "r", "s", "sh", "zb", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";

            name += consonants[r.Next(consonants.Length)].ToUpper();
            name += vowels[r.Next(vowels.Length)];

            int b = 2; // Already added 2 characters
            while (b < len)
            {
                name += consonants[r.Next(consonants.Length)];
                b++;
                name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return name;
        }

        public static double ExtractNumberFromText(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
                throw new ArgumentException("Input text is null or whitespace.", nameof(rawText));

            // Remove any non-digit, non-dot characters
            string cleaned = Regex.Replace(rawText, @"[^\d.]", "");

            // Optionally trim any whitespace or special chars
            cleaned = cleaned.Trim();

            if (double.TryParse(cleaned, out double result))
            {
                return result;
            }

            throw new FormatException($"Could not parse numeric value from: '{rawText}' (cleaned: '{cleaned}')");
        }
    }
}

