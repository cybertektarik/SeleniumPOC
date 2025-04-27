using System.Globalization;

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
    }
}

