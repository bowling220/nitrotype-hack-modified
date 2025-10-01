namespace NitroType3
{
    class Config
    {
        // Booleans
        public static bool AutoStart { get; set; } = true;
        public static bool AutoGame { get; set; } = true;
        public static bool UseNitros { get; set; } = true;
        public static bool AutoEmoji { get; set; } = false;

        // Base Ints
        public static int TypingRate { get; set; } = 100;
        public static int Accuracy { get; set; } = 98;

        // Modifier Ints
        public static int TypingRateVariancy { get; set; } = 2;
        public static int AccuracyVariancy { get; set; } = 2;

        // Internal Runtime Settings
        public static bool CheatRunning { get; set; } = false;

        // External Links
        public static string DiscordLink { get; set; } = "https://www.nitrotype.com/team/EXAM";
    }
}
