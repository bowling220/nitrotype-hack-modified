using System.Text.Json;

namespace NitroType3
{
    public class AuthService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string? cachedKey = null;
        private static bool isAuthenticated = false;

        // Firebase configuration - loaded securely
        private static string FIREBASE_PROJECT_ID => SecureConfig.FirebaseProjectId;
        private static string FIREBASE_API_KEY => SecureConfig.FirebaseApiKey;
        private static string FIREBASE_URL => $"https://firestore.googleapis.com/v1/projects/{FIREBASE_PROJECT_ID}/databases/(default)/documents";

        public static async Task<bool> ValidateKeyAsync(string key)
        {
            try
            {
                Logger.Log("Validating key: " + key.Substring(0, Math.Min(8, key.Length)) + "...");
                Logger.Log("Firebase URL: " + FIREBASE_URL);
                Logger.Log("API Key: " + FIREBASE_API_KEY.Substring(0, 10) + "...");
                
                // Check if key exists in Firebase Firestore
                string url = $"{FIREBASE_URL}/keys/{key}";
                Logger.Log("Request URL: " + url);
                
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                
                var response = await httpClient.SendAsync(request);
                Logger.Log("Response status: " + response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Logger.Log("Response content: " + content);
                    
                    var keyData = JsonSerializer.Deserialize<KeyData>(content);
                    
                    if (keyData != null && keyData.fields != null)
                    {
                        bool isActive = keyData.fields.active?.booleanValue ?? false;
                        bool isExpired = IsKeyExpired(keyData.fields.expiryDate?.stringValue);
                        
                        Logger.Log("Key active: " + isActive + ", Expired: " + isExpired);
                        
                        if (isActive && !isExpired)
                        {
                            cachedKey = key;
                            isAuthenticated = true;
                            Logger.Log("Key validation successful");
                            return true;
                        }
                        else
                        {
                            Logger.Log("Key is inactive or expired");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.Log("Key data is null or fields are null");
                        return false;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Logger.Log("Firebase error: " + errorContent);
                }
                
                Logger.Log("Key validation failed");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log("Key validation error: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static bool IsAuthenticated()
        {
            return isAuthenticated;
        }

        public static void SetAdminAuthenticated()
        {
            isAuthenticated = true;
            cachedKey = "ADMIN_BYPASS";
            Logger.Log("Admin authentication successful - bypassing key validation");
        }

        public static string? GetCachedKey()
        {
            return cachedKey;
        }

        public static void Logout()
        {
            cachedKey = null;
            isAuthenticated = false;
            Logger.Log("User logged out");
        }

        private static bool IsKeyExpired(string? expiryDate)
        {
            if (string.IsNullOrEmpty(expiryDate))
                return false;
                
            if (DateTime.TryParse(expiryDate, out DateTime expiry))
            {
                return DateTime.Now > expiry;
            }
            
            return false;
        }

        // Admin functions for key management
        public static async Task<bool> CreateKeyAsync(string key, DateTime? expiryDate = null)
        {
            try
            {
                string url = $"{FIREBASE_URL}/keys/{key}";
                
                var keyData = new
                {
                    fields = new
                    {
                        active = new { booleanValue = true },
                        createdDate = new { stringValue = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                        expiryDate = new { stringValue = expiryDate?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? "" }
                    }
                };
                
                var json = JsonSerializer.Serialize(keyData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var request = new HttpRequestMessage(HttpMethod.Patch, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                request.Content = content;
                
                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.Log("Error creating key: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static async Task<bool> DeactivateKeyAsync(string key)
        {
            try
            {
                string url = $"{FIREBASE_URL}/keys/{key}";
                
                var keyData = new
                {
                    fields = new
                    {
                        active = new { booleanValue = false }
                    }
                };
                
                var json = JsonSerializer.Serialize(keyData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var request = new HttpRequestMessage(HttpMethod.Patch, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                request.Content = content;
                
                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.Log("Error deactivating key: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static async Task<bool> ActivateKeyAsync(string key)
        {
            try
            {
                Logger.Log("Activating key: " + key);
                
                // Use the same approach as DeactivateKeyAsync but with active = true
                string url = $"{FIREBASE_URL}/keys/{key}";
                var request = new HttpRequestMessage(HttpMethod.Patch, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                
                var updateData = new
                {
                    fields = new
                    {
                        active = new { booleanValue = true }
                    }
                };
                
                string jsonContent = JsonSerializer.Serialize(updateData);
                request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await httpClient.SendAsync(request);
                Logger.Log("Activate key response status: " + response.StatusCode);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Logger.Log("Firebase error: " + errorContent);
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.Log("Error activating key: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static async Task<bool> DeleteKeyAsync(string key)
        {
            try
            {
                Logger.Log("Deleting key: " + key);
                
                string url = $"{FIREBASE_URL}/keys/{key}";
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                
                var response = await httpClient.SendAsync(request);
                Logger.Log("Delete key response status: " + response.StatusCode);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Logger.Log("Firebase error: " + errorContent);
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.Log("Error deleting key: " + ex.Message, Logger.Level.Error);
                return false;
            }
        }

        public static async Task<List<KeyInfo>> GetAllKeysAsync()
        {
            try
            {
                Logger.Log("Fetching all keys from Firebase");
                
                // List all documents in the keys collection
                string url = $"{FIREBASE_URL}/keys";
                
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-Goog-Api-Key", FIREBASE_API_KEY);
                
                var response = await httpClient.SendAsync(request);
                Logger.Log("Response status: " + response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Logger.Log("Response content: " + content);
                    
                    var keysList = JsonSerializer.Deserialize<KeysListResponse>(content);
                    var keys = new List<KeyInfo>();
                    
                    if (keysList?.documents != null)
                    {
                        foreach (var doc in keysList.documents)
                        {
                            if (doc.fields != null)
                            {
                                var keyInfo = new KeyInfo
                                {
                                    KeyName = ExtractKeyNameFromPath(doc.name),
                                    IsActive = doc.fields.active?.booleanValue ?? false,
                                    ExpiryDate = ParseExpiryDate(doc.fields.expiryDate?.stringValue)
                                };
                                keys.Add(keyInfo);
                            }
                        }
                    }
                    
                    Logger.Log($"Successfully loaded {keys.Count} keys");
                    return keys;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Logger.Log("Firebase error: " + errorContent);
                    return new List<KeyInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error fetching keys: " + ex.Message, Logger.Level.Error);
                return new List<KeyInfo>();
            }
        }

        private static string ExtractKeyNameFromPath(string path)
        {
            // Extract key name from path like "projects/nt-key-9d14a/databases/(default)/documents/keys/KEY_NAME"
            var parts = path.Split('/');
            return parts.LastOrDefault() ?? "";
        }

        private static DateTime? ParseExpiryDate(string? expiryDateString)
        {
            if (string.IsNullOrEmpty(expiryDateString))
                return null;
                
            // Try different date formats
            string[] formats = {
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:ss.fffffffZ",
                "yyyy-MM-dd",
                "MM/dd/yyyy",
                "dd/MM/yyyy"
            };
            
            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(expiryDateString, format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                    return result;
            }
            
            // Fallback to general parsing
            if (DateTime.TryParse(expiryDateString, out DateTime fallbackResult))
                return fallbackResult;
                
            Logger.Log("Could not parse expiry date: " + expiryDateString, Logger.Level.Error);
            return null;
        }
    }

    public class KeyData
    {
        public KeyFields? fields { get; set; }
    }

    public class KeyFields
    {
        public BooleanValue? active { get; set; }
        public StringValue? createdDate { get; set; }
        public StringValue? expiryDate { get; set; }
    }

    public class BooleanValue
    {
        public bool booleanValue { get; set; }
    }

    public class StringValue
    {
        public string? stringValue { get; set; }
    }

    public class KeyInfo
    {
        public string KeyName { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class KeysListResponse
    {
        public List<KeyDocument>? documents { get; set; }
    }

    public class KeyDocument
    {
        public string name { get; set; } = "";
        public KeyFields? fields { get; set; }
    }
}
