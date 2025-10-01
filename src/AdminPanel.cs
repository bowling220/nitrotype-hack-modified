using System.ComponentModel;

namespace NitroType3
{
    public partial class AdminPanel : Form
    {
        private TextBox keyInputTextBox;
        private DateTimePicker expiryDatePicker;
        private Button createKeyButton;
        private Button deactivateKeyButton;
        private Button deleteKeyButton;
        private ListBox keysListBox;
        private Button refreshButton;
        private Label statusLabel;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Panel loginPanel;
        private Panel adminPanel;
        private bool isAuthenticated = false;
        private List<KeyInfo> currentKeys = new List<KeyInfo>();

        public AdminPanel()
        {
            InitializeComponent();
            ShowLoginPanel();
        }

        private void InitializeComponent()
        {
            this.keyInputTextBox = new TextBox();
            this.expiryDatePicker = new DateTimePicker();
            this.createKeyButton = new Button();
            this.deactivateKeyButton = new Button();
            this.keysListBox = new ListBox();
            this.refreshButton = new Button();
            this.statusLabel = new Label();
            this.passwordTextBox = new TextBox();
            this.loginButton = new Button();
            this.loginPanel = new Panel();
            this.adminPanel = new Panel();
            this.SuspendLayout();
            
            // Set form styling
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            
            // 
            // loginPanel
            // 
            this.loginPanel.BackColor = Color.FromArgb(37, 37, 38);
            this.loginPanel.Controls.Add(this.loginButton);
            this.loginPanel.Controls.Add(this.passwordTextBox);
            this.loginPanel.Dock = DockStyle.Fill;
            this.loginPanel.Location = new Point(0, 0);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new Size(570, 290);
            this.loginPanel.TabIndex = 0;
            
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = Color.FromArgb(30, 30, 30);
            this.passwordTextBox.BorderStyle = BorderStyle.FixedSingle;
            this.passwordTextBox.ForeColor = Color.White;
            this.passwordTextBox.Location = new Point(200, 120);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.PlaceholderText = "Enter admin password...";
            this.passwordTextBox.Size = new Size(200, 23);
            this.passwordTextBox.TabIndex = 0;
            this.passwordTextBox.KeyPress += PasswordTextBox_KeyPress;
            
            // 
            // loginButton
            // 
            this.loginButton.BackColor = Color.FromArgb(0, 120, 215);
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = FlatStyle.Flat;
            this.loginButton.ForeColor = Color.White;
            this.loginButton.Location = new Point(250, 160);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new Size(100, 30);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += LoginButton_Click;
            this.loginButton.MouseEnter += Button_MouseEnter;
            this.loginButton.MouseLeave += Button_MouseLeave;
            
            // 
            // adminPanel
            // 
            this.adminPanel.BackColor = Color.FromArgb(37, 37, 38);
            this.adminPanel.Controls.Add(this.statusLabel);
            this.adminPanel.Controls.Add(this.refreshButton);
            this.adminPanel.Controls.Add(this.keysListBox);
            this.adminPanel.Controls.Add(this.deactivateKeyButton);
            this.adminPanel.Controls.Add(this.createKeyButton);
            this.adminPanel.Controls.Add(this.expiryDatePicker);
            this.adminPanel.Controls.Add(this.keyInputTextBox);
            this.adminPanel.Dock = DockStyle.Fill;
            this.adminPanel.Location = new Point(0, 0);
            this.adminPanel.Name = "adminPanel";
            this.adminPanel.Size = new Size(570, 290);
            this.adminPanel.TabIndex = 1;
            this.adminPanel.Visible = false;
            
            // 
            // keyInputTextBox
            // 
            this.keyInputTextBox.BackColor = Color.FromArgb(30, 30, 30);
            this.keyInputTextBox.BorderStyle = BorderStyle.FixedSingle;
            this.keyInputTextBox.ForeColor = Color.White;
            this.keyInputTextBox.Location = new Point(12, 30);
            this.keyInputTextBox.Name = "keyInputTextBox";
            this.keyInputTextBox.PlaceholderText = "Enter key to create/deactivate...";
            this.keyInputTextBox.Size = new Size(200, 23);
            this.keyInputTextBox.TabIndex = 0;
            
            // 
            // expiryDatePicker
            // 
            this.expiryDatePicker.BackColor = Color.FromArgb(30, 30, 30);
            this.expiryDatePicker.CalendarForeColor = Color.White;
            this.expiryDatePicker.CalendarMonthBackground = Color.FromArgb(30, 30, 30);
            this.expiryDatePicker.CalendarTitleBackColor = Color.FromArgb(0, 120, 215);
            this.expiryDatePicker.CalendarTitleForeColor = Color.White;
            this.expiryDatePicker.CalendarTrailingForeColor = Color.Gray;
            this.expiryDatePicker.ForeColor = Color.White;
            this.expiryDatePicker.Location = new Point(12, 70);
            this.expiryDatePicker.Name = "expiryDatePicker";
            this.expiryDatePicker.Size = new Size(200, 23);
            this.expiryDatePicker.TabIndex = 1;
            this.expiryDatePicker.Value = DateTime.Now.AddYears(1);
            
            // 
            // createKeyButton
            // 
            this.createKeyButton.BackColor = Color.FromArgb(0, 120, 215);
            this.createKeyButton.FlatAppearance.BorderSize = 0;
            this.createKeyButton.FlatStyle = FlatStyle.Flat;
            this.createKeyButton.ForeColor = Color.White;
            this.createKeyButton.Location = new Point(12, 110);
            this.createKeyButton.Name = "createKeyButton";
            this.createKeyButton.Size = new Size(100, 30);
            this.createKeyButton.TabIndex = 2;
            this.createKeyButton.Text = "Create Key";
            this.createKeyButton.UseVisualStyleBackColor = false;
            this.createKeyButton.Click += CreateKeyButton_Click;
            this.createKeyButton.MouseEnter += Button_MouseEnter;
            this.createKeyButton.MouseLeave += Button_MouseLeave;
            
            // 
            // deactivateKeyButton
            // 
            this.deactivateKeyButton.BackColor = Color.FromArgb(196, 43, 28);
            this.deactivateKeyButton.FlatAppearance.BorderSize = 0;
            this.deactivateKeyButton.FlatStyle = FlatStyle.Flat;
            this.deactivateKeyButton.ForeColor = Color.White;
            this.deactivateKeyButton.Location = new Point(120, 250);
            this.deactivateKeyButton.Name = "deactivateKeyButton";
            this.deactivateKeyButton.Size = new Size(100, 30);
            this.deactivateKeyButton.TabIndex = 3;
            this.deactivateKeyButton.Text = "Deactivate Key";
            this.deactivateKeyButton.UseVisualStyleBackColor = false;
            this.deactivateKeyButton.Click += DeactivateKeyButton_Click;
            this.deactivateKeyButton.MouseEnter += Button_MouseEnter;
            this.deactivateKeyButton.MouseLeave += Button_MouseLeave;
            
            // 
            // deleteKeyButton
            // 
            this.deleteKeyButton = new Button();
            this.deleteKeyButton.BackColor = Color.FromArgb(196, 43, 28);
            this.deleteKeyButton.FlatAppearance.BorderSize = 0;
            this.deleteKeyButton.FlatStyle = FlatStyle.Flat;
            this.deleteKeyButton.ForeColor = Color.White;
            this.deleteKeyButton.Location = new Point(10, 250);
            this.deleteKeyButton.Name = "deleteKeyButton";
            this.deleteKeyButton.Size = new Size(100, 30);
            this.deleteKeyButton.TabIndex = 4;
            this.deleteKeyButton.Text = "Delete Key";
            this.deleteKeyButton.UseVisualStyleBackColor = false;
            this.deleteKeyButton.Click += DeleteKeyButton_Click;
            this.deleteKeyButton.MouseEnter += Button_MouseEnter;
            this.deleteKeyButton.MouseLeave += Button_MouseLeave;
            
            // 
            // keysListBox
            // 
            this.keysListBox.BackColor = Color.FromArgb(30, 30, 30);
            this.keysListBox.BorderStyle = BorderStyle.FixedSingle;
            this.keysListBox.ForeColor = Color.White;
            this.keysListBox.Location = new Point(250, 30);
            this.keysListBox.Name = "keysListBox";
            this.keysListBox.Size = new Size(300, 200);
            this.keysListBox.TabIndex = 4;
            this.keysListBox.MouseDoubleClick += KeysListBox_MouseDoubleClick;
            this.keysListBox.MouseDown += KeysListBox_MouseDown;
            
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = Color.FromArgb(0, 120, 215);
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = FlatStyle.Flat;
            this.refreshButton.ForeColor = Color.White;
            this.refreshButton.Location = new Point(250, 240);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new Size(100, 30);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += RefreshButton_Click;
            this.refreshButton.MouseEnter += Button_MouseEnter;
            this.refreshButton.MouseLeave += Button_MouseLeave;
            
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.statusLabel.ForeColor = Color.White;
            this.statusLabel.Location = new Point(12, 10);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(100, 15);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "Admin Panel";
            
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(570, 290);
            this.Controls.Add(this.loginPanel);
            this.Controls.Add(this.adminPanel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminPanel";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Key Management - Admin Panel";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ShowLoginPanel()
        {
            loginPanel.Visible = true;
            adminPanel.Visible = false;
            passwordTextBox.Focus();
        }

        private void ShowAdminPanel()
        {
            loginPanel.Visible = false;
            adminPanel.Visible = true;
            LoadKeys();
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            if (passwordTextBox.Text == SecureConfig.AdminPassword)
            {
                isAuthenticated = true;
                ShowAdminPanel();
            }
            else
            {
                MessageBox.Show("Invalid password!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordTextBox.Clear();
                passwordTextBox.Focus();
            }
        }

        private void PasswordTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private async void CreateKeyButton_Click(object? sender, EventArgs e)
        {
            if (!isAuthenticated) return;
            
            if (string.IsNullOrWhiteSpace(keyInputTextBox.Text))
            {
                MessageBox.Show("Please enter a key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            createKeyButton.Enabled = false;
            createKeyButton.Text = "Creating...";
            statusLabel.Text = "Creating key...";

            try
            {
                bool success = await AuthService.CreateKeyAsync(keyInputTextBox.Text.Trim(), expiryDatePicker.Value);
                
                if (success)
                {
                    statusLabel.Text = "Key created successfully!";
                    keyInputTextBox.Clear();
                    LoadKeys();
                }
                else
                {
                    statusLabel.Text = "Failed to create key.";
                    MessageBox.Show("Failed to create key. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error creating key.";
                MessageBox.Show("Error creating key: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                createKeyButton.Enabled = true;
                createKeyButton.Text = "Create Key";
            }
        }

        private async void DeactivateKeyButton_Click(object? sender, EventArgs e)
        {
            if (!isAuthenticated) return;
            
            if (string.IsNullOrWhiteSpace(keyInputTextBox.Text))
            {
                MessageBox.Show("Please enter a key to deactivate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to deactivate key: {keyInputTextBox.Text}?", 
                "Confirm Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result != DialogResult.Yes)
                return;

            deactivateKeyButton.Enabled = false;
            deactivateKeyButton.Text = "Deactivating...";
            statusLabel.Text = "Deactivating key...";

            try
            {
                bool success = await AuthService.DeactivateKeyAsync(keyInputTextBox.Text.Trim());
                
                if (success)
                {
                    statusLabel.Text = "Key deactivated successfully!";
                    keyInputTextBox.Clear();
                    LoadKeys();
                }
                else
                {
                    statusLabel.Text = "Failed to deactivate key.";
                    MessageBox.Show("Failed to deactivate key. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error deactivating key.";
                MessageBox.Show("Error deactivating key: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                deactivateKeyButton.Enabled = true;
                deactivateKeyButton.Text = "Deactivate Key";
            }
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            if (!isAuthenticated) return;
            LoadKeys();
        }

        private async void LoadKeys()
        {
            if (!isAuthenticated) return;
            
            statusLabel.Text = "Loading keys...";
            keysListBox.Items.Clear();
            
            try
            {
                var keys = await AuthService.GetAllKeysAsync();
                currentKeys.Clear();
                
                if (keys != null && keys.Count > 0)
                {
                    currentKeys.AddRange(keys);
                    foreach (var key in keys)
                    {
                        string status = key.IsActive ? "Active" : "Inactive";
                        string expiry = key.ExpiryDate?.ToString("yyyy-MM-dd") ?? "No expiry";
                        string displayText = $"{key.KeyName} - {status} - Expires: {expiry} - [Double-click: Toggle] [Right-click: Delete]";
                        keysListBox.Items.Add(displayText);
                    }
                    statusLabel.Text = $"Loaded {keys.Count} keys - Double-click to toggle, Right-click to delete";
                }
                else
                {
                    keysListBox.Items.Add("No keys found");
                    statusLabel.Text = "No keys found";
                }
            }
            catch (Exception ex)
            {
                keysListBox.Items.Add($"Error loading keys: {ex.Message}");
                statusLabel.Text = "Error loading keys";
                Logger.Log("Error loading keys: " + ex.Message, Logger.Level.Error);
            }
        }

        private async void KeysListBox_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (!isAuthenticated) return;
            
            int selectedIndex = keysListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < currentKeys.Count)
            {
                var selectedKey = currentKeys[selectedIndex];
                
                try
                {
                    statusLabel.Text = $"Toggling {selectedKey.KeyName}...";
                    
                    bool success;
                    if (selectedKey.IsActive)
                    {
                        // Deactivate the key
                        success = await AuthService.DeactivateKeyAsync(selectedKey.KeyName);
                        if (success)
                        {
                            selectedKey.IsActive = false;
                            statusLabel.Text = $"Deactivated {selectedKey.KeyName}";
                        }
                    }
                    else
                    {
                        // Activate the key
                        success = await AuthService.ActivateKeyAsync(selectedKey.KeyName);
                        if (success)
                        {
                            selectedKey.IsActive = true;
                            statusLabel.Text = $"Activated {selectedKey.KeyName}";
                        }
                    }
                    
                    if (success)
                    {
                        // Refresh the display
                        LoadKeys();
                    }
                    else
                    {
                        statusLabel.Text = "Failed to toggle key status";
                        MessageBox.Show("Failed to toggle key status. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Error toggling key status";
                    MessageBox.Show("Error toggling key status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Log("Error toggling key status: " + ex.Message, Logger.Level.Error);
                }
            }
        }

        private async void KeysListBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (!isAuthenticated) return;
            
            // Check if it's a right-click
            if (e.Button == MouseButtons.Right)
            {
                // Get the item at the mouse position
                int index = keysListBox.IndexFromPoint(e.Location);
                if (index >= 0 && index < currentKeys.Count)
                {
                    var selectedKey = currentKeys[index];
                    
                    var result = MessageBox.Show($"Are you sure you want to DELETE key: {selectedKey.KeyName}?\n\nThis action cannot be undone!", 
                        "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            statusLabel.Text = $"Deleting {selectedKey.KeyName}...";
                            
                            bool success = await AuthService.DeleteKeyAsync(selectedKey.KeyName);
                            
                            if (success)
                            {
                                statusLabel.Text = $"Deleted {selectedKey.KeyName}";
                                LoadKeys(); // Refresh the list
                            }
                            else
                            {
                                statusLabel.Text = "Failed to delete key";
                                MessageBox.Show("Failed to delete key. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            statusLabel.Text = "Error deleting key";
                            MessageBox.Show("Error deleting key: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.Log("Error deleting key: " + ex.Message, Logger.Level.Error);
                        }
                    }
                }
            }
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button == deactivateKeyButton)
                {
                    button.BackColor = Color.FromArgb(220, 60, 40);
                }
                else
                {
                    button.BackColor = Color.FromArgb(0, 100, 180);
                }
            }
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button == deactivateKeyButton || button == deleteKeyButton)
                {
                    button.BackColor = Color.FromArgb(196, 43, 28);
                }
                else
                {
                    button.BackColor = Color.FromArgb(0, 120, 215);
                }
            }
        }

        private async void DeleteKeyButton_Click(object? sender, EventArgs e)
        {
            if (!isAuthenticated) return;
            
            if (string.IsNullOrWhiteSpace(keyInputTextBox.Text))
            {
                MessageBox.Show("Please enter a key to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to DELETE key: {keyInputTextBox.Text}?\n\nThis action cannot be undone!", 
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (result != DialogResult.Yes)
                return;

            deleteKeyButton.Enabled = false;
            deleteKeyButton.Text = "Deleting...";
            statusLabel.Text = "Deleting key...";

            try
            {
                bool success = await AuthService.DeleteKeyAsync(keyInputTextBox.Text.Trim());
                
                if (success)
                {
                    statusLabel.Text = "Key deleted successfully!";
                    keyInputTextBox.Clear();
                    LoadKeys();
                }
                else
                {
                    statusLabel.Text = "Failed to delete key.";
                    MessageBox.Show("Failed to delete key. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error deleting key.";
                MessageBox.Show("Error deleting key: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                deleteKeyButton.Enabled = true;
                deleteKeyButton.Text = "Delete Key";
            }
        }
    }
}