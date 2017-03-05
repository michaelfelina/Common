using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Common.Methods.Enumumerators;
using Common.Methods.Extensions;

namespace Common.Methods
{
    public enum AuthenticationType
    {
        SQLAuthentication = 1,
        WindowsAuthentication = 2,
        None = 0,
    }

    public static class AuthenticationTypeEx
    {
        public static string ToString(this AuthenticationType value)
            => System.Enum.GetName(typeof(AuthenticationType), value);

        public static int ToInt(this AuthenticationType value) => (int) value;
    }

    public class MSSQL
    {
        public SqlConnection OConn { get; set; }
        public AuthenticationType Authentication { get; set; }

        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool Connected { get; set; }

        public OperationResult Connect()
        {
            OperationResult result = new OperationResult();
            try
            {
                string strConnection;
                switch (Authentication)
                {
                    case AuthenticationType.WindowsAuthentication:
                        strConnection = "server=" + Server + ";database=" + Database + ";integrated security=true;";
                        break;
                    case AuthenticationType.SQLAuthentication:
                        strConnection = "server=" + Server + ";database=" + Database + ";integrated security=False;uid=" +
                                        Username + ";password=" + Password + ";";
                        break;
                    default:
                        result.Success = false;
                        result.Add("No Specified Authentication Type");
                        return result;
                }
                OConn = new SqlConnection(strConnection);
                OConn.Open();
                Connected = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult Connect(string ConnectionString)
        {
            OperationResult result = new OperationResult();
            try
            {
                OConn = new SqlConnection(ConnectionString);
                OConn.Open();
                Connected = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public void CloseConnection()
        {
            if (OConn != null)
            {
                OConn.Close();
                Connected = false;
                OConn.Dispose();
            }
            OConn = null;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc, ref int newId)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure};

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((Status) cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    if (newId == -1)
                    {
                        newId = cmd.Parameters["@NewID"].Value.ToInt();
                        if (newId <= 0)
                        {
                            result.Success = false;
                            result.Add("No ID returned from database after insert");
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure};

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status) cmd.Parameters["@DBStatus"].Value != Status.Success)
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc,
            ref int newId, ref int errorCode)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure};

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status) cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    errorCode = cmd.Parameters["@ErrorCode"].Value.ToInt();
                    if (errorCode == 0)
                    {
                        if (newId == -1)
                        {
                            newId = cmd.Parameters["@NewID"].ToInt();
                            if (newId <= 0)
                            {
                                result.Success = false;
                                result.Add("No ID returned from database after insert");
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Add("Save Failed");
                    }

                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc,
            ref int newId, SqlTransaction trans, ref int errorCode)
        {
            var result = new OperationResult();

            try
            {
                var cmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure, Transaction = trans};
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status) cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    errorCode = cmd.Parameters["@ErrorCode"].Value.ToInt();
                    if (errorCode == 0)
                    {
                        if (newId == -1)
                        {
                            newId = cmd.Parameters["@NewID"].Value.ToInt();
                            if (newId <= 0)
                            {
                                result.Success = false;
                                result.Add("No ID returned from database after insert");
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Add("Save Failed");
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc,
            ref int newId, SqlTransaction trans, SqlConnection cn, ref int errorCode)
        {
            var result = new OperationResult();

            try
            {
                var cmd = new SqlCommand(sProc, cn)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = trans
                };
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status) cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    errorCode = cmd.Parameters["@ErrorCode"].Value.ToInt();
                    if (errorCode == 0)
                    {
                        if (newId == -1)
                        {
                            newId = cmd.Parameters["@NewID"].Value.ToInt();
                            if (newId <= 0)
                            {
                                result.Success = false;
                                result.Add("No ID returned from database after insert");
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Add("Save Failed");
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc,
            ref int newId, SqlTransaction trans, SqlConnection cn)
        {
            var result = new OperationResult();

            try
            {
                var cmd = new SqlCommand(sProc, cn)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = trans
                };
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status)cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    if (newId == -1)
                    {
                        newId = cmd.Parameters["@NewID"].Value.ToInt();
                        if (newId <= 0)
                        {
                            result.Success = false;
                            result.Add("No ID returned from database after insert");
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult Save(List<SqlParameter> parameters, string sProc,
            ref int newId,  ref int errorCode, SqlConnection cn)
        {
            var result = new OperationResult();

            try
            {
                var cmd = new SqlCommand(sProc, cn) { CommandType = CommandType.StoredProcedure};
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status)cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    errorCode = cmd.Parameters["@ErrorCode"].Value.ToInt();
                    if (errorCode == 0)
                    {
                        if (newId == -1)
                        {
                            newId = cmd.Parameters["@NewID"].Value.ToInt();
                            if (newId <= 0)
                            {
                                result.Success = false;
                                result.Add("No ID returned from database after insert");
                            }
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Add("Save Failed");
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }


        public OperationResult Save(List<SqlParameter> parameters, string sProc, SqlTransaction trans, ref int newId)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure, Transaction = trans};

                if (parameters != null)
                {
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                oParam = cmd.Parameters.Add(new SqlParameter("@NewID", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((Status) cmd.Parameters["@DBStatus"].Value == Status.Success)
                {
                    if (newId == -1)
                    {
                        newId = cmd.Parameters["@NewID"].Value.ToInt();
                        if (newId <= 0)
                        {
                            result.Success = false;
                            result.Add("No ID returned from database after insert");
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.Add("Save Failed");
                }
            }
            catch (Exception)
            {
                result.Success = false;
                result.Add("Save Failed");
            }
            return result;
        }

        public OperationResult GetData(List<SqlParameter> parameters, string sProc, out DataTable dtResult)
        {
            var result = new OperationResult();
            dtResult = new DataTable();
            try
            {
                var oCmd = new SqlCommand(sProc, OConn) {CommandType = CommandType.StoredProcedure};
                dtResult = loaDataTable(parameters, oCmd);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult GetQueryResult(List<SqlParameter> parameters, string query, out DataTable dtResult)
        {
            var result = new OperationResult();
            dtResult = new DataTable();
            try
            {
                var oCmd = new SqlCommand(query, OConn) { CommandType = CommandType.Text };
                dtResult = loaDataTableNoOutput(parameters, oCmd);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }
        
        public OperationResult GetData(List<SqlParameter> parameters, string sProc, SqlTransaction trans,
            out DataTable dtResult)
        {
            var result = new OperationResult();
            dtResult = new DataTable();
            try
            {
                var oCmd = new SqlCommand(sProc, OConn, trans) {CommandType = CommandType.StoredProcedure};
                dtResult = loaDataTable(parameters, oCmd);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult GetData(List<SqlParameter> parameters, string sProc, SqlConnection cn,
            out DataTable dtResult)
        {
            var result = new OperationResult();
            dtResult = new DataTable();

            try
            {
                var oCmd = new SqlCommand(sProc, cn) {CommandType = CommandType.StoredProcedure};
                dtResult = loaDataTable(parameters, oCmd);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult GetData(List<SqlParameter> parameters, string sProc, SqlTransaction trans,
            SqlConnection cn, out DataTable dtResult)
        {
            var result = new OperationResult();
            dtResult = new DataTable();

            try
            {
                var oCmd = new SqlCommand(sProc, cn, trans) { CommandType = CommandType.StoredProcedure };
                dtResult = loaDataTable(parameters, oCmd);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        private DataTable loaDataTable(List<SqlParameter> parameters, SqlCommand oCmd)
        {
            var dtResult = new DataTable();
            if (parameters != null)
            {
                foreach (var param in parameters)
                    oCmd.Parameters.Add(param);
            }
            var oParam = oCmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
            oParam.Direction = ParameterDirection.Output;
            oCmd.CommandTimeout = 120;
            var drResults = oCmd.ExecuteReader();
            dtResult.Load(drResults);
            if ((Status)oCmd.Parameters["@DBStatus"].Value != Status.Success) dtResult = null;

            return dtResult;
        }
        private DataTable loaDataTableNoOutput(List<SqlParameter> parameters, SqlCommand oCmd)
        {
            var dtResult = new DataTable();
            if (parameters != null)
            {
                foreach (var param in parameters)
                    oCmd.Parameters.Add(param);
            }
            
            oCmd.CommandTimeout = 120;
            var drResults = oCmd.ExecuteReader();
            dtResult.Load(drResults);

            return dtResult;
        }

        public OperationResult Execute(List<SqlParameter> parameters, string sProc)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) { CommandType = CommandType.StoredProcedure };
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((Status) cmd.Parameters["@DBStatus"].Value != Status.Success)
                {
                    result.Success = false;
                    result.Add("Execute Query Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }

            return result;
        }

        public OperationResult Execute(List<SqlParameter> parameters, string sProc, SqlTransaction trans)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, OConn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((Status) cmd.Parameters["@DBStatus"].Value != Status.Success)
                {
                    result.Success = false;
                    result.Add("Execute Query Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult Execute(List<SqlParameter> parameters, string sProc, SqlTransaction trans, SqlConnection cn)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(sProc, cn) { CommandType = CommandType.StoredProcedure, Transaction = trans };
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var oParam = cmd.Parameters.Add(new SqlParameter("@DBStatus", SqlDbType.Int));
                oParam.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((Status)cmd.Parameters["@DBStatus"].Value != Status.Success)
                {
                    result.Success = false;
                    result.Add("Execute Query Failed");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult ExecuteQuery(List<SqlParameter> parameters, string Query)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(Query, OConn) { CommandType = CommandType.Text };
                if (parameters != null)
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult ExecuteQuery(List<SqlParameter> parameters, string Query, SqlTransaction trans)
        {
            var result = new OperationResult();
            try
            {
                var cmd = new SqlCommand(Query, OConn) { CommandType = CommandType.Text, Transaction = trans };
                if (parameters != null)
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public OperationResult CreateDatabase(string databaseName)
        {
            var result = new OperationResult();
            try
            {
                var Query = $"Create Database {databaseName}";
                var cmd = new SqlCommand(Query, OConn) { CommandType = CommandType.Text };
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Add(ex.Message);
            }
            return result;
        }

        public bool IsDatabaseExist(string databaseName)
        {
            var success = true;
            try
            {
                var result = new DataTable();
                var Query = $"select * FROM master.dbo.sysdatabases  WHERE name = '{databaseName}'";
                var oCmd = new SqlCommand(Query, OConn)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 120
                };
                var drResults = oCmd.ExecuteReader();
                result.Load(drResults);
                if (result.Rows.Count <= 0)
                {
                    success = false;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return success;
        }
    }
}
