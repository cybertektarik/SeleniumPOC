namespace SeleniumPOC.Common
{
    internal static class InvestmentFunctions
    {
        private static List<string> sectors = new List<string>()
        {
            "FOOD", "WASTE", "AGRICULTURE", "ART", "CONSTRUCTION", "UTILITIES", "WHOLESALE"
        };

        private static List<string> occupations = new List<string>()
        {
            "ACCOUNTANT", "ACTUARY", "ADJUSTER", "ATHLETE", "BARBER", "CAREGIVER", "DEVELOPER", "WRITER"
        };

        private static Random random = new Random();

        public static string GetRandomSector()
        {
            var index = random.Next(sectors.Count);
            return sectors[index];
        }

        public static string GetRandomOccupation()
        {
            var index = random.Next(occupations.Count);
            return occupations[index];
        }
    }
}

