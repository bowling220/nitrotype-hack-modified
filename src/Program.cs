namespace NitroType3
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Logger.Log("========== NEW LAUNCH ==========");
            
            AppDomain.CurrentDomain.UnhandledException += LogApplicationUnhandledException;
            AppDomain.CurrentDomain.ProcessExit += LogApplicationProcessExit;

            Logger.Log("Sending Usage Information to Server");
            Connections.UsageReport();

            Logger.Log("Checking For Updates");
            ShouldUpdate();

            Logger.Log("Initializing Window");
            try
            {
                ApplicationConfiguration.Initialize();
                Logger.Log("Application configuration initialized successfully");
                
                Logger.Log("Checking Authentication");
                try
                {
                    if (!AuthService.IsAuthenticated())
                    {
                        AuthForm authForm = new AuthForm();
                        if (authForm.ShowDialog() != DialogResult.OK)
                        {
                            Logger.Log("Authentication cancelled by user");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log("Authentication error: " + ex.Message, Logger.Level.Error);
                    MessageBox.Show(
                        "Authentication failed: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                
                Logger.Log("Creating main form");
                Form1 mainForm = new Form1();
                Logger.Log("Main form created successfully");
                
                Logger.Log("Starting application");
                Application.Run(mainForm);
                Logger.Log("Application started successfully");
            }
            catch (System.EntryPointNotFoundException)
            {
                MessageBox.Show(
                    "This program is not supported on your device. (GetThreadDpiHostingBehavior not in DLL USER32.dll)",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }
            catch (Exception ex)
            {
                Logger.Log("Fatal error during application startup: " + ex.Message, Logger.Level.Error);
                Logger.Log("Stack trace: " + ex.StackTrace, Logger.Level.Error);
                
                MessageBox.Show(
                    "A fatal error occurred during startup: " + ex.Message + "\n\nCheck the logs for more details.",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
        }

        private static void LogApplicationProcessExit(object? sender, EventArgs e)
        {
            UserConfig.Save();
            Logger.Log("========== SAFE EXITING ==========");
        }

        private static void LogApplicationUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(
                "UnhandledException = " + e.ToString(),
                Logger.Level.Error
            );

            Logger.Log(
                "ExceptionObject = " + e.ExceptionObject.ToString(),
                Logger.Level.Error
            );

            Logger.Log(
                "IsExceptionTerminating = " + e.IsTerminating.ToString(),
                Logger.Level.Error
            );

            if (e.IsTerminating) {
                DialogResult ShareConsent = MessageBox.Show(
                    "There was a fatal error, would you like to send the error logs to us so we can resolve the issue faster?\n" +
                        "No information about you or your computer will be sent.",
                    "Fatal Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (ShareConsent == DialogResult.Yes)
                {
                    Connections.ErrorReport(e.ToString(), e.ExceptionObject.ToString());
                }
            }
        }

        static async void ShouldUpdate()
        {
            bool IsUpdate = await Updates.ShouldUpdate();

            if (IsUpdate)
            {
                Logger.Log("Update Available");
                DialogResult WantsUpdate = MessageBox.Show(
                    "Looks like there's a new version available! Would you like to download it now?",
                    "Update Available!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (WantsUpdate == DialogResult.Yes)
                {
                    Logger.Log("Opening Update Link");
                    Connections.OpenLink("https://github.com/kgsensei/NitroTypeHack2/releases/latest");
                }
            }
        }
    }
}
