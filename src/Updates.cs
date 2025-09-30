namespace NitroType3
{
    class Updates
    {
        public static string VersionCode = "4.6.2";

        public static async Task<Boolean> ShouldUpdate()
        {
            // Update checking disabled - BuildEnvironment.cs was excluded from repository
            Logger.Log("Update checking disabled");
            return false;
        }
    }
}
