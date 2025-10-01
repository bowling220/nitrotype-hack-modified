using Microsoft.Web.WebView2.Core;

namespace NitroType3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            {
                Logger.Log("Form1 constructor started");
                
                try
                {
                    _ = CoreWebView2Environment.GetAvailableBrowserVersionString();
                    Logger.Log("WebView2 runtime check passed");
                }
                catch (WebView2RuntimeNotFoundException)
                {
                    Logger.Log("Missing WebView2 Runtime", Logger.Level.Error);
                    MessageBox.Show(
                        "You don't have the Microsoft WebView2 Component installed.\nThis is a requirement to run the cheat.\nPlease install it then run the cheat again.",
                        "Fatal Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Environment.Exit(0); // Don't call Close() because window is not initialized yet
                }

                Logger.Log("Initializing components");
                InitializeComponent();
                Logger.Log("Components initialized successfully");
                
                Logger.Log("Loading previous user settings");
                LoadPreviousUser();
                Logger.Log("User settings loaded successfully");
                
                Logger.Log("Setting up webview");
                SetupWebview();
                Logger.Log("Webview setup completed successfully");

                Text = "NitroType Cheat v" + Updates.VersionCode;
                
                // Auto emoji is now handled by JavaScript message when race starts
                
                Logger.Log("Form1 constructor completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Log("Fatal error in Form1 constructor: " + ex.Message, Logger.Level.Error);
                Logger.Log("Stack trace: " + ex.StackTrace, Logger.Level.Error);
                
                MessageBox.Show(
                    "A fatal error occurred while initializing the application: " + ex.Message + "\n\nCheck the logs for more details.",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                throw; // Re-throw to be caught by Program.cs
            }
            
            // Wire up the FormClosing event handler
            this.FormClosing += Form1_FormClosing;
            
            // Initialize emoji indicator
            UpdateEmojiIndicator();
        }

        private void LoadPreviousUser()
        {
            int Opens = UserConfig.Get("UsrCnf_OpenAmount");
            Logger.Log("Open Number:" + Opens);
            
            if (Opens != 0)
            {
                try
                {
                    Config.TypingRate = UserConfig.Get("UsrCnf_TypingRate_Real");
                    typingRateSlider.Value = UserConfig.Get("UsrCnf_TypingRate_Visual");
                    // Update typing rate label
                    int Total = typingRateSlider.Maximum + typingRateSlider.Minimum;
                    int RealRate = Total - typingRateSlider.Value;
                    int WPMCalculation = (int)(60 / ((double)RealRate / 1000) / 5);
                    typingRateSliderLabel.Text = "Typing Rate: ~" + WPMCalculation;

                    Config.TypingRateVariancy = UserConfig.Get("UsrCnf_TypingRateV");
                    typingRateVarianceSlider.Value = Config.TypingRateVariancy;
                    typingRateVarianceLabel.Text = "Typing Rate Variance: ±" + typingRateVarianceSlider.Value;

                    Config.Accuracy = UserConfig.Get("UsrCnf_Accuracy");
                    accuracySlider.Value = Config.Accuracy;
                    accuracySliderLabel.Text = "Accuracy: " + Config.Accuracy + "%";

                    Config.AccuracyVariancy = UserConfig.Get("UsrCnf_AccuracyV");
                    accuracyVarianceSlider.Value = Config.AccuracyVariancy;
                    accuracyVarianceLabel.Text = "Accuracy Variance: ±" + Config.AccuracyVariancy + "%";

                    Config.AutoStart = UserConfig.Get("UsrCnf_AutoStart");
                    autostart.Checked = Config.AutoStart;
                    startButton.Enabled = !Config.AutoStart;
                    if (Config.AutoStart)
                    {
                        UI_Change_Start_Colors(24, 85, 133);
                    }
                    else
                    {
                        UI_Change_Start_Colors(214, 47, 58);
                    }

                    // Force AutoGame and UseNitros to be checked by default
                    Config.AutoGame = true;
                    Config.UseNitros = true;
                    autogame.Checked = true;
                    usenitros.Checked = true;

       // Config.AutoEmoji = UserConfig.Get("UsrCnf_AutoEmoji"); // Temporarily disabled
       // autoemoji.Checked = Config.AutoEmoji; // Temporarily disabled
       autoemoji.Checked = Config.AutoEmoji; // Use default value
                }
                catch (Exception)
                {
                    UserConfig.Reset();
                }
            }

            Opens++;
            UserConfig.Set("UsrCnf_OpenAmount", Opens);
        }

        private async void SetupWebview()
        {
            Logger.Log("Building WebView2");
            CoreWebView2EnvironmentOptions coreWebView2EnvironmentOptions = new()
            {
                AreBrowserExtensionsEnabled = true,
            };

            var webViewEnvOptions = await CoreWebView2Environment.CreateAsync(
                null,
                null,
                coreWebView2EnvironmentOptions
            );

            Logger.Log("Ensuring Initialization");
            await webView.EnsureCoreWebView2Async(webViewEnvOptions);

            Logger.Log("Injecting Captcha Solver");
            await webView.CoreWebView2.Profile.AddBrowserExtensionAsync(
                System.IO.Directory.GetCurrentDirectory() + @"\extensions\hlifkpholllijblknnmbfagnkjneagid"
            );

            Logger.Log("Hooking Request, Response, and Message Events");
            webView.CoreWebView2.WebResourceRequested += RequestBlocker;
            webView.CoreWebView2.WebResourceResponseReceived += RacePageLoadedChecker;
            webView.CoreWebView2.WebMessageReceived += WebMessageRecieved;
            webView.CoreWebView2.AddWebResourceRequestedFilter(
                null,
                CoreWebView2WebResourceContext.All
            );

            Logger.Log("Loading NitroType.com");
            webView.Source = new Uri("https://nitrotype.com");
            
            // Mark as initialized
            isInitialized = true;
            
            // Update emoji indicator after page loads
            UpdateEmojiIndicator();
        }

        private void UI_Change_Start_Colors(int r, int b, int g)
        {
            Color color = Color.FromArgb(r, b, g);
            startButton.BackColor = color;
            startButton.FlatAppearance.BorderColor = color;
            startButton.FlatAppearance.MouseOverBackColor = color;
            startButton.FlatAppearance.MouseDownBackColor = color;
        }

        private void UI_Update_Autostart(object sender, EventArgs e)
        {
            Logger.Log("Auto Start Value Changed:" + autostart.Checked.ToString());
            startButton.Enabled = !autostart.Checked;
            Config.AutoStart = autostart.Checked;
            if (Config.AutoStart)
            {
                UI_Change_Start_Colors(24, 85, 133);
            }
            else
            {
                UI_Change_Start_Colors(214, 47, 58);
            }
            UserConfig.Save();
        }

        private void UI_Update_Autogame(object sender, EventArgs e)
        {
            Logger.Log("Auto Game Value Changed:" + autogame.Checked.ToString());
            Config.AutoGame = autogame.Checked;
            UserConfig.Save();
        }

        private void UI_Update_Usenitros(object sender, EventArgs e)
        {
            Logger.Log("Use Nitros Value Changed:" + usenitros.Checked.ToString());
            Config.UseNitros = usenitros.Checked;
            UserConfig.Save();
        }

        private void UI_Update_Autoemoji(object sender, EventArgs e)
        {
            Logger.Log("Auto Emoji Value Changed:" + autoemoji.Checked.ToString());
            Config.AutoEmoji = autoemoji.Checked;
            
            // Show warning when user enables Auto Emoji (but not during form closing)
            if (autoemoji.Checked && !this.IsDisposed && this.Visible)
            {
                MessageBox.Show(
                    "⚠️ Auto Emoji Feature Warning ⚠️\n\n" +
                    "The Auto Emoji feature is still in development and may not work reliably.\n\n" +
                    "• It may require manual interaction to trigger\n" +
                    "• The feature might not work in all scenarios\n" +
                    "• We're working to improve its reliability\n\n" +
                    "If you experience issues, please uncheck this option.",
                    "Development Feature",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            
            // Update emoji indicator visibility
            UpdateEmojiIndicator();
            
            // Auto emoji is now handled by JavaScript message when race starts
            UserConfig.Save();
        }

        private void UpdateEmojiIndicator()
        {
            if (emojiIndicator == null) return;
            
            // Show indicator only if Auto Emoji is enabled and we're on the start screen
            bool shouldShow = Config.AutoEmoji && IsOnStartScreen();
            emojiIndicator.Visible = shouldShow;
            
            if (shouldShow)
            {
                // Set text and styling to indicate user needs to click
                emojiIndicator.Text = "Click here once before race starts";
                emojiIndicator.ForeColor = Color.LimeGreen;
                emojiIndicator.BackColor = Color.FromArgb(46, 49, 65);
                emojiIndicator.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private bool IsOnStartScreen()
        {
            try
            {
                if (webView?.CoreWebView2 == null) return false;
                
                // Check if we're on the main NitroType page (not in a race)
                string currentUrl = webView.Source?.ToString() ?? "";
                return currentUrl.Contains("nitrotype.com") && !currentUrl.Contains("/race");
            }
            catch
            {
                return false;
            }
        }

        private async void SimulateEmojiKeyPress()
        {
            if (!Config.AutoEmoji || !isInitialized) return;
            
            try
            {
                // Check if WebView2 is still valid
                if (webView?.CoreWebView2 == null)
                {
                    Logger.Log("WebView2 is not initialized, skipping emoji key press");
                    return;
                }
                
                // Randomly select emoji key 1-7
                Random random = new Random();
                int emojiKey = random.Next(1, 8); // 1-7 inclusive
                
                Logger.Log("Simulating emoji key press using DevTools protocol - Key: " + emojiKey);
                
                // Try the "char" approach like other characters in Controller.cs
                string emojiArgs = @"{ ""type"": ""char"", ""text"": """ + emojiKey + @""" }";
                await webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchKeyEvent", emojiArgs);
                
                Logger.Log("Emoji key '" + emojiKey + "' pressed successfully");
            }
            catch (Exception ex)
            {
                Logger.Log("Error simulating emoji key press: " + ex.Message, Logger.Level.Error);
            }
        }


        private void UI_Click_Discord(object sender, EventArgs e)
        {
            Logger.Log("Discord Button Clicked");
            Connections.OpenLink(Config.DiscordLink);
        }

        private void UI_Click_Admin(object sender, EventArgs e)
        {
            Logger.Log("Admin Panel Button Clicked");
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.ShowDialog();
        }

        private async void UI_Click_Start(object sender, EventArgs e)
        {
            Logger.Log("Manual Start Clicked");
            if (webView.Source.AbsolutePath == "/race" && !Config.AutoStart)
            {
                await webView.ExecuteScriptAsync(
                    @"if(document.getElementsByClassName('raceChat').length ? false : true) {
                        z = document.getElementsByClassName('dash-letter');
                        m = '';
                        for(let i = 0 ; i < z.length; i++) {
                            m = m + z[i].innerText
                        };
                        window.chrome.webview.postMessage('' + m);
                    } else {
                        window.chrome.webview.postMessage('GAME_NOT_STARTED_ERROR');
                    }"
                );
            }
            else
            {
                MessageBox.Show(
                    "Enter A race before starting the cheat.",
                    "Internal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void UI_Slider_AccuracyVariance(object sender, EventArgs e)
        {
            accuracyVarianceLabel.Text = "Accuracy Variance: ±" + accuracyVarianceSlider.Value;
            Config.AccuracyVariancy = accuracyVarianceSlider.Value;
            UserConfig.Save();
        }

        private void UI_Slider_AccuracySlider(object sender, EventArgs e)
        {
            accuracySliderLabel.Text = "Accuracy: " + accuracySlider.Value + "%";
            Config.Accuracy = accuracySlider.Value;
            UserConfig.Save();
        }

        private void UI_Slider_TypingRateVariance(object sender, EventArgs e)
        {
            typingRateVarianceLabel.Text = "Typing Rate Variance: ±" + typingRateVarianceSlider.Value;
            Config.TypingRateVariancy = typingRateVarianceSlider.Value;
            UserConfig.Save();
        }

        private void UI_Slider_TypingRate(object sender, EventArgs e)
        {
            int Total = typingRateSlider.Maximum + typingRateSlider.Minimum;
            int RealRate = Total - typingRateSlider.Value;
            Config.TypingRate = RealRate;
            int WPMCalculation = (int)(60 / ((double)RealRate / 1000) / 5);
            typingRateSliderLabel.Text = "Typing Rate: ~" + WPMCalculation;
            UserConfig.Set("UsrCnf_TypingRate_Visual", typingRateSlider.Value);
            UserConfig.Save();
        }

        private void InjectAutoStart()
        {
            var r = new Random();
            string funcName = new(Enumerable.Range(0, 30).Select(n => (Char)r.Next(65, 90)).ToArray());

            Logger.Log("Injecting Auto Start Script");

            webView.ExecuteScriptAsync(
                @"function " + funcName + @"() {
                    if(document.getElementsByClassName('raceChat').length ? false : true) {
                        let z = document.getElementsByClassName('dash-letter');
                        let m = '';
                        for(let i = 0; i < z.length; i++) {
                            m = m + z[i].innerText
                        };
                        window.chrome.webview.postMessage('' + m);
                    } else {
                        setTimeout(() => {" + funcName + @"();}, 10);                    
                    }
                }

                // Auto Emoji Function - Now handled by C# timer
                function autoEmoji() {
                    console.log('Auto emoji function called - handled by C# timer');
                }

                setTimeout(() => {" + funcName + @"();}, 2000);
                
                // Auto emoji - race detection only (no immediate trigger)
                if(" + Config.AutoEmoji.ToString().ToLower() + @") {
                    let emojiTriggered = false;
                    let raceCheckCount = 0;
                    
                    // Check for race start every 200ms
                    let raceCheckInterval = setInterval(() => {
                        if(!emojiTriggered) {
                            raceCheckCount++;
                            
                            // Check for multiple race indicators
                            let raceStarted = document.getElementsByClassName('raceChat').length > 0 ||
                                            document.querySelector('.race-container') !== null ||
                                            document.querySelector('[data-race]') !== null ||
                                            document.querySelector('.typing-area') !== null ||
                                            document.querySelector('.race') !== null ||
                                            document.querySelector('[class*=""race""]') !== null ||
                                            document.querySelector('[class*=""typing""]') !== null;
                            
                            if(raceStarted) {
                                emojiTriggered = true;
                                clearInterval(raceCheckInterval);
                                console.log('Race started - triggering auto emoji');
                                setTimeout(() => {
                                    window.chrome.webview.postMessage('TRIGGER_EMOJI');
                                }, 1500);
                            }
                            
                            // Fallback: trigger after 10 seconds if no race detected
                            if(raceCheckCount > 50) { // 50 * 200ms = 10 seconds
                                emojiTriggered = true;
                                clearInterval(raceCheckInterval);
                                console.log('Auto emoji fallback triggered after 10 seconds');
                                setTimeout(() => {
                                    window.chrome.webview.postMessage('TRIGGER_EMOJI');
                                }, 1000);
                            }
                        }
                    }, 200);
                }

                setInterval(() => {
                    const m = ""Validated!Playon."";
                    const x = document.querySelector(""h1.tsxxl.mbs"");
                    const f = Array.from(document.querySelectorAll(""div.tc-i"")).find(l => l.textContent.replace(/ /g, """") === m);
                    if(x) {
                        if(x.textContent === ""Communications Error"") {
                            window.location.reload();
                        }
                    }
                    if(f) {
                        if(f.textContent.replace(/ /g, """") === m) {
                            window.location.reload();
                        }
                    }
                }, 500);"
            );
        }

        private void WebMessageRecieved(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (!Config.CheatRunning)
            {
                string BrowserData = e.TryGetWebMessageAsString();
                Logger.Log("Web Message Recieved");

                if (BrowserData == "GAME_NOT_STARTED_ERROR")
                {
                    MessageBox.Show(
                        "The game hasn't started yet.",
                        "Internal Error",
                        MessageBoxButtons.OK
                    );
                }
                else if (BrowserData == "TRIGGER_EMOJI")
                {
                    // Trigger emoji when race starts
                    Logger.Log("Race started - triggering auto emoji");
                    SimulateEmojiKeyPress();
                }
                else
                {
                    Controller.SimulateTypingText(BrowserData, webView);
                }
            }
        }

        private void RacePageLoadedChecker(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            string Uri = e.Request.Uri;

            if (
                Uri.Contains("nitrotype.com/race") &&
                !Uri.Contains("nitrotype.com/racer/") &&
                Config.AutoStart
            )
            {
                InjectAutoStart();
            }

            webView.ExecuteScriptAsync(
                @"setInterval(() => {
                    const tmpx = document.querySelectorAll('.profile-ad, .ad, .goldTeaser');
                    for (let i = 0; i < tmpx.length; i++) {
                        tmpx[i].remove();
                    }
                }, 100);"
            );
        }

        private void RequestBlocker(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            try
            {
                if (webView?.CoreWebView2 == null) return;
                
                bool Blocked = AdBlocker.IsBlocked(e.Request.Uri);

                if (Blocked)
                {
                    e.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(
                        null,
                        404,
                        "Resource Blocked by Client",
                        null
                    );
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error in RequestBlocker: " + ex.Message, Logger.Level.Error);
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                Logger.Log("Application closing - cleaning up WebView2");
                
                // Stop any ongoing operations first
                if (webView?.CoreWebView2 != null)
                {
                    try
                    {
                        // Clear event handlers to prevent callbacks during disposal
                        webView.CoreWebView2.WebResourceRequested -= RequestBlocker;
                        webView.CoreWebView2.WebResourceResponseReceived -= RacePageLoadedChecker;
                        webView.CoreWebView2.WebMessageReceived -= WebMessageRecieved;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error clearing WebView2 event handlers: " + ex.Message, Logger.Level.Error);
                    }
                }
                
                // Dispose the WebView2 control
                if (webView != null)
                {
                    webView.Dispose();
                    webView = null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error during cleanup: " + ex.Message, Logger.Level.Error);
            }
        }
    }
}
