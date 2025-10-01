namespace NitroType3
{
    public class SecureConfig
    {
        // These should be loaded from a secure file or environment variables
        // For now, using placeholder values that need to be replaced
        public static string FirebaseProjectId => GetSecureValue("FIREBASE_PROJECT_ID", "nt-key-9d14a");
        public static string FirebaseApiKey => GetSecureValue("FIREBASE_API_KEY", "AIzaSyBKltYZtofayhD0OGYANx5iFjr4NvmWPRs");
        public static string AdminPassword => GetSecureValue("ADMIN_PASSWORD", "Bo250067!");
        
        private static string GetSecureValue(string key, string defaultValue)
        {
            try
            {
                // Try to get from environment variables first
                string? envValue = Environment.GetEnvironmentVariable(key);
                if (!string.IsNullOrEmpty(envValue))
                    return envValue;
                    
                // Try to get from secure config file
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secure.config");
                if (File.Exists(configPath))
                {
                    string[] lines = File.ReadAllLines(configPath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(key + "="))
                        {
                            return line.Substring(key.Length + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Error loading secure config for {key}: {ex.Message}");
            }
            
            return defaultValue;
        }
    }
}
