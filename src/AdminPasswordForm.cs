namespace NitroType3
{
    public partial class AdminPasswordForm : Form
    {
        private TextBox passwordTextBox;
        private Button loginButton;
        private Button cancelButton;
        private Label statusLabel;

        public AdminPasswordForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.passwordTextBox = new TextBox();
            this.loginButton = new Button();
            this.cancelButton = new Button();
            this.statusLabel = new Label();
            this.SuspendLayout();
            
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new Point(12, 50);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.PlaceholderText = "Enter admin password...";
            this.passwordTextBox.Size = new Size(300, 23);
            this.passwordTextBox.TabIndex = 0;
            this.passwordTextBox.KeyPress += PasswordTextBox_KeyPress;
            
            // 
            // loginButton
            // 
            this.loginButton.Location = new Point(12, 90);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new Size(100, 30);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += LoginButton_Click;
            
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
            this.statusLabel.Text = "Enter admin password to bypass key:";
            
            // 
            // AdminPasswordForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(324, 141);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordTextBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminPasswordForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (SimpleAuth.ValidateAdminPassword(passwordTextBox.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    statusLabel.Text = "Invalid admin password. Please try again.";
                    statusLabel.ForeColor = Color.Red;
                    passwordTextBox.Clear();
                    passwordTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error: " + ex.Message;
                statusLabel.ForeColor = Color.Red;
            }
        }

        private void PasswordTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
