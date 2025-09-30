using System.Text;

namespace NitroType3
{
    class Connections
    {
        public static void UsageReport()
        {
            // Usage reporting disabled - BuildEnvironment.cs was excluded from repository
            Logger.Log("Usage reporting disabled");
        }

        public static void ErrorReport(string? errorMessage = "Unknown", string? stackTrace = "Unknown")
        {
            // Error reporting disabled - BuildEnvironment.cs was excluded from repository
            Logger.Log("Error reporting disabled: " + errorMessage);
        }

        public static void OpenLink(string url)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", url);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Error: Couldn't open link, lacking permissions.\n\n" + url,
                    "Internal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
