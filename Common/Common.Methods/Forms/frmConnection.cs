using System;
using System.Windows.Forms;

namespace Common.Methods.Forms
{
    public partial class frmConnection : Form
    {
        private readonly string _settings;
        public frmConnection(string settings)
        {
            InitializeComponent();
            _settings = settings;
            Initialize();
        }

        private void Initialize()
        {
            txtServer.Text = "";
            txtDatabase.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";

            optSQLAuthentication.Checked = false;
            optWindowsAuthentication.Checked = false;
        }

        private void SetValues(MSSQL mssql)
        {
            txtServer.Text = mssql.Server;
            txtDatabase.Text = mssql.Database;
            if (mssql.Authentication == AuthenticationType.SQLAuthentication)
            {
                optSQLAuthentication.Checked = true;
                txtUsername.Text = mssql.Username;
                txtPassword.Text = mssql.Password;
            }
            else
                optWindowsAuthentication.Checked = true;
        }

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Please input Server Name", "Database Connection");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDatabase.Text))
            {
                MessageBox.Show("Please input Database Name", "Database Connection");
                return false;
            }
            if (optSQLAuthentication.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please input User Name", "Database Connection");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please input Password", "Database Connection");
                    return false;
                }
            }
            return true;
        }

        private void frmConnection_Load(object sender, EventArgs e)
        {
            MSSQL mssql;
            OperationResult result = Settings.GetInfo(_settings, out mssql);
            if (result.Success) SetValues(mssql);
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
            btnSave.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!Validation()) return;
            btnSave.Enabled = false;
            var mssql = new MSSQL() {
                Server = txtServer.Text.Trim()
                , Database = txtDatabase.Text.Trim() };
            if (optSQLAuthentication.Checked)
            {
                mssql.Authentication = AuthenticationType.SQLAuthentication;
                mssql.Username = txtUsername.Text.Trim();
                mssql.Password = txtPassword.Text.Trim();
            }
            else
                mssql.Authentication = AuthenticationType.WindowsAuthentication;

            var result = mssql.Connect();
            if (result.Success)
            {
                btnSave.Enabled = true;
                mssql.CloseConnection();
                MessageBox.Show("Connection Successful", "Database Connection");
            }
            else
                MessageBox.Show(result.MessageList[0], "Database Connection");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validation()) return;
            var mssql = new MSSQL()
            {
                Server = txtServer.Text.Trim(),
                Database = txtDatabase.Text.Trim()
            };
            if (optSQLAuthentication.Checked)
            {
                mssql.Authentication = AuthenticationType.SQLAuthentication;
                mssql.Username = txtUsername.Text.Trim();
                mssql.Password = txtPassword.Text.Trim();
            }
            else
                mssql.Authentication = AuthenticationType.WindowsAuthentication;

            if (Equals(Tag, "GET"))
            {
                Tag = mssql;
                Close();
            }
            else
            {
                var result = Settings.Save(_settings, mssql);
                if (result.Success)
                {
                    MessageBox.Show("Save Successful", "Database Settings");
                    Tag = mssql;
                    Close();
                }
                else
                {
                    Tag = null;
                    MessageBox.Show(result.MessageList[0], "Database Settings");
                }
            }
        }
    }
}
