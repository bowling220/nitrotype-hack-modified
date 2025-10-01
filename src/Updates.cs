namespace NitroType3
{
    class Updates
    {
        public static string VersionCode = "5.0.1";

        public static async Task<Boolean> ShouldUpdate()
        {
            // Update checking disabled - BuildEnvironment.cs was excluded from repository
            Logger.Log("Update checking disabled");
            return false;
        }
    }
}
