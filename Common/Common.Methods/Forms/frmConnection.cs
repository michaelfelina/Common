using System;
using System.Windows.Forms;
using Common.Methods;

namespace Common.Methods
{
    public partial class frmConnection : Form
    {
        private string ConnectionName;
        public frmConnection(string _ConnectionName)
        {
            InitializeComponent();
            ConnectionName = _ConnectionName;
        }

        private void frmConnection_Load(object sender, EventArgs e)
        {
            var mssql = new MSSQL();
            var result = mssql.GetSetting(ConnectionName);
            if (result.Success)
            {
                txtServer.Text = mssql.Server;
                txtDatabase.Text = mssql.Database;
                if (mssql.Authentication == AuthenticationType.WindowsAuthentication)
                {
                    optWindowsAuthentication.Checked = true;
                }
                else
                {
                    optSQLAuthentication.Checked = true;
                    txtUsername.Text = mssql.Username;
                    txtPassword.Text = mssql.Password;
                }
            }
        }

        private void optWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (optWindowsAuthentication.Checked)
            {
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
                txtUsername.Text = "";
                txtPassword.Text = "";
            }
            else
            {
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate()) return;
            var mssql = GetValues();
            var operationResult = mssql.SaveSetting(ConnectionName);
            if (operationResult.Success)
            {
                MessageBox.Show("Save Success", "Database Connection");
            }
            else
            {
                MessageBox.Show($"Save Failed. {operationResult.ErrorMessage}", "Database Connection");
            }
            btnSave.Enabled = false;
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(txtServer.Text))
            {
                MessageBox.Show("Please input Server Name", "Database Connection");
                return true;
            }
            if (string.IsNullOrEmpty(txtDatabase.Text))
            {
                MessageBox.Show("Please input Database Name", "Database Connection");
                return true;
            }
            if (optSQLAuthentication.Checked)
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Please input User Name", "Database Connection");
                    return true;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please input Password", "Database Connection");
                    return true;
                }
            }
            return false;
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            if (Validate()) return;
            var mssql = GetValues();
            var operationResult = mssql.Connect();
            if (operationResult.Success)
            {
                if (mssql.Connected)
                {
                    mssql.CloseConnection();
                    btnSave.Enabled = true;
                    MessageBox.Show("Connection Success", "Database Connection");
                }
                else
                {
                    btnSave.Enabled = false;
                    MessageBox.Show("Connection Failed", "Database Connection");
                }
            }
            else
            {
                btnSave.Enabled = false;
                MessageBox.Show(operationResult.ErrorMessage, "Database Connection");
            }
        }

        private MSSQL GetValues()
        {
            var result = new MSSQL();
            result.Server = txtServer.Text.Trim();
            result.Database = txtDatabase.Text.Trim();
            result.Authentication = optWindowsAuthentication.Checked
                ? AuthenticationType.WindowsAuthentication
                : AuthenticationType.SQLAuthentication;
            result.Username = txtUsername.Text.Trim();
            result.Password = txtPassword.Text.Trim();
            return result;
        }
    }
}
