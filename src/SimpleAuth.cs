namespace NitroType3
{
    public class SimpleAuth
    {
        private static bool isAuthenticated = false;
        private static string? currentKey = null;

        // Simple hardcoded keys for testing (replace with your actual keys)
        private static readonly string[] ValidKeys = {
            "DEMO-KEY-12345",
            "TEST-KEY-67890",
            "ADMIN-KEY-99999"
        };

        private static readonly string AdminPassword = "Bo250067!";

        public static bool ValidateKey(string key)
        {
            try
            {
                Logger.Log("Validating key: " + key.Substring(0, Math.Min(8, key.Length)) + "...");
                
                // Check if it's a valid key
                if (ValidKeys.Contains(key.ToUpper()))
                {
                    isAuthenticated = true;
                    currentKey = key;
                    Logger.Log("Key validation successful");
                    return true;
                }
                
                Logger.Log("Invalid key");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log("Key validation error: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static bool ValidateAdminPassword(string password)
        {
            try
            {
                if (password == AdminPassword)
                {
                    isAuthenticated = true;
                    currentKey = "ADMIN_BYPASS";
                    Logger.Log("Admin authentication successful");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log("Admin validation error: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static bool IsAuthenticated()
        {
            return isAuthenticated;
        }

        public static string? GetCurrentKey()
        {
            return currentKey;
        }

        public static void Logout()
        {
            isAuthenticated = false;
            currentKey = null;
            Logger.Log("User logged out");
        }

        public static void SetAdminAuthenticated()
        {
            isAuthenticated = true;
            currentKey = "ADMIN_BYPASS";
            Logger.Log("Admin authentication successful - bypassing key validation");
        }
    }
}
