using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Methods;

namespace Common.Methods
{
    public static class Settings
    {
        static readonly string filename = $"{Application.StartupPath}\\settings.cfg";

        public static OperationResult Save(string settings, MSSQL mssql)
        {
            var result = new OperationResult();

            try
            {
                if (string.IsNullOrWhiteSpace(settings))
                {
                    result.Success = false;
                    result.Add("settings is null or empty");
                }
                else
                {
                    StdLib.Common.INIWriteValue(settings, "Server", mssql.Server, filename);
                    StdLib.Common.INIWriteValue(settings, "Database", mssql.Database, filename);
                    StdLib.Common.INIWriteValue(settings, "Authentication", mssql.Authentication.ToInt().ToString(), filename);
                    StdLib.Common.INIWriteValue(settings, "Username", mssql.Username, filename);
                    StdLib.Common.INIWriteValue(settings, "Password", mssql.Password, filename);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public static OperationResult GetInfo(string settings, out MSSQL mssql)
        {
            var result = new OperationResult();
            mssql = new MSSQL();
            try
            {
                if (string.IsNullOrWhiteSpace(settings))
                {
                    result.Success = false;
                    result.Add("settings is null or empty");
                }
                else
                {
                    mssql.Server = StdLib.Common.INIReadValue(settings, "Server", filename);
                    mssql.Database = StdLib.Common.INIReadValue(settings, "Database", filename);
                    mssql.Authentication = (AuthenticationType) StdLib.Common.INIReadValue(settings, "Authentication", filename).ToInt();
                    mssql.Username = StdLib.Common.INIReadValue(settings, "Username", filename);
                    mssql.Password = StdLib.Common.INIReadValue(settings, "Password", filename);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }
    }
}
