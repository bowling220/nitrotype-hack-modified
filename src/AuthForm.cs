using System.ComponentModel;

namespace NitroType3
{
    public partial class AuthForm : Form
    {
        private TextBox keyTextBox;
        private Button validateButton;
        private Button cancelButton;
        private Label statusLabel;
        private bool isValidated = false;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.keyTextBox = new TextBox();
            this.validateButton = new Button();
            this.cancelButton = new Button();
            this.statusLabel = new Label();
            this.SuspendLayout();
            
            // 
            // keyTextBox
            // 
            this.keyTextBox.Location = new Point(12, 50);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.PlaceholderText = "Enter your license key...";
            this.keyTextBox.Size = new Size(300, 23);
            this.keyTextBox.TabIndex = 0;
            this.keyTextBox.TextChanged += KeyTextBox_TextChanged;
            this.keyTextBox.KeyPress += KeyTextBox_KeyPress;
            
            // 
            // validateButton
            // 
            this.validateButton.Location = new Point(12, 90);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new Size(100, 30);
            this.validateButton.TabIndex = 1;
            this.validateButton.Text = "Validate Key";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += ValidateButton_Click;
            this.validateButton.Enabled = false;
            
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new Point(130, 90);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new Size(100, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += CancelButton_Click;
            
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new Point(12, 20);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size(200, 15);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Please enter your license key to continue:";
            
            // 
            // AuthForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(324, 141);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.validateButton);
            this.Controls.Add(this.keyTextBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "License Key Required";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void KeyTextBox_TextChanged(object? sender, EventArgs e)
        {
            validateButton.Enabled = !string.IsNullOrWhiteSpace(keyTextBox.Text);
        }

        private void KeyTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && validateButton.Enabled)
            {
                ValidateButton_Click(sender, e);
            }
        }

        private async void ValidateButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(keyTextBox.Text))
                return;

            validateButton.Enabled = false;
            validateButton.Text = "Validating...";
            statusLabel.Text = "Validating key with Firebase, please wait...";
            statusLabel.ForeColor = Color.Blue;

            try
            {
                bool isValid = await AuthService.ValidateKeyAsync(keyTextBox.Text.Trim());
                
                if (isValid)
                {
                    statusLabel.Text = "Key validated successfully!";
                    statusLabel.ForeColor = Color.Green;
                    isValidated = true;
                    
                    // Close form after a short delay
                    await Task.Delay(1000);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    statusLabel.Text = "Invalid or expired key. Please try again.";
                    statusLabel.ForeColor = Color.Red;
                    keyTextBox.Clear();
                    keyTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error validating key: " + ex.Message;
                statusLabel.ForeColor = Color.Red;
            }
            finally
            {
                validateButton.Enabled = true;
                validateButton.Text = "Validate Key";
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        public bool IsValidated()
        {
            return isValidated;
        }
    }
}
