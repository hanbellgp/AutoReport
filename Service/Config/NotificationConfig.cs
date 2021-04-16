using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Hanbell.AutoReport.Core;
using Sybase.Data.AseClient;

namespace Hanbell.AutoReport.Config
{
    public abstract class NotificationConfig : IDisposable
    {
        protected Hashtable args;
        protected DBServerType dbtype;
        protected string dbconnstr;
        protected DbConnection dbconn;
        protected DbCommand dbcomm;
        protected DbDataAdapter da;
        protected DataSet ds;
        protected ArrayList reportList = new ArrayList();

        protected DbConnection CreateDbConnection(DBServerType dbType, string connectionString)
        {
            switch (dbType)
            {
                case DBServerType.MSSQL:
                    return new SqlConnection(connectionString);
                case DBServerType.OLEDB:
                    return new OleDbConnection(connectionString);
                case DBServerType.SybaseASE:
                    return new AseConnection(connectionString);
                default:
                    return new SqlConnection(connectionString);
            }
        }

        protected DbCommand CreateDbCommand(DBServerType dbType)
        {
            switch (dbType)
            {
                case DBServerType.MSSQL:
                    return new SqlCommand();
                case DBServerType.OLEDB:
                    return new OleDbCommand();
                case DBServerType.SybaseASE:
                    return new AseCommand();
                default:
                    return new SqlCommand();
            }
        }

        protected DbDataAdapter CreateDbDataAdapter(DBServerType dbType)
        {
            switch (dbType)
            {
                case DBServerType.MSSQL:
                    return new SqlDataAdapter();
                case DBServerType.OLEDB:
                    return new OleDbDataAdapter();
                case DBServerType.SybaseASE:
                    return new AseDataAdapter();
                default:
                    return new SqlDataAdapter();
            }
        }

        protected DbParameter CreateDbParameter(DBServerType dbType)
        {
            switch (dbType)
            {
                case DBServerType.MSSQL:
                    return new SqlParameter();
                case DBServerType.OLEDB:
                    return new OleDbParameter();
                case DBServerType.SybaseASE:
                    return new AseParameter();
                default:
                    return new SqlParameter();
            }
        }

        private void PrepareDBUtil()
        {
            switch (this.dbtype)
            {
                case DBServerType.MSSQL:
                    dbconn = new SqlConnection(this.dbconnstr);
                    dbcomm = new SqlCommand();
                    dbcomm.Connection = (SqlConnection)dbconn;
                    da = new SqlDataAdapter();
                    da.SelectCommand = (SqlCommand)dbcomm;
                    break;
                case DBServerType.OLEDB:
                    dbconn = new OleDbConnection(this.dbconnstr);
                    dbcomm = new OleDbCommand();
                    dbcomm.Connection = (OleDbConnection)dbconn;
                    da = new OleDbDataAdapter();
                    da.SelectCommand = (OleDbCommand)dbcomm;
                    break;
                case DBServerType.SybaseASE:
                    dbconn = new AseConnection(this.dbconnstr);
                    dbcomm = new AseCommand();
                    dbcomm.Connection = (AseConnection)dbconn;
                    da = new AseDataAdapter();
                    da.SelectCommand = (AseCommand)dbcomm;
                    break;
                default:
                    dbconn = new SqlConnection(this.dbconnstr);
                    dbcomm = new SqlCommand();
                    dbcomm.Connection = (SqlConnection)dbconn;
                    da = new SqlDataAdapter();
                    da.SelectCommand = (SqlCommand)dbcomm;
                    break;
            }
        }

        protected void PrepareDBUtil(DBServerType dbType, string connectionString)
        {
            this.dbtype = dbType;
            this.dbconnstr = connectionString;
            PrepareDBUtil();
        }

        protected int ExecSql(string sqlstr)
        {
            DbConnection sqlconn = CreateDbConnection(this.dbtype, this.dbconnstr);
            DbCommand sqlcomm = CreateDbCommand(this.dbtype);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.CommandText = sqlstr;
            sqlcomm.Connection = sqlconn;
            try
            {
                if (sqlconn.State == ConnectionState.Closed) sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (sqlconn.State == ConnectionState.Open) sqlconn.Close();
                sqlcomm.Dispose();
                sqlconn.Dispose();
            }
        }

        protected DataTable GetQueryTable(string sqlstr)
        {
            DataTable tbl = new DataTable();
            try
            {
                this.dbcomm.CommandText = sqlstr;
                dbcomm.CommandTimeout = 180;
                if (this.dbconn.State == ConnectionState.Closed) this.dbconn.Open();
                tbl.Clear();
                da.Fill(tbl);
                return tbl;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (this.dbconn.State == ConnectionState.Open) this.dbconn.Close();
            }
        }

        protected string GetQueryString(string sqlstr)
        {
            try
            {
                this.dbcomm.CommandText = sqlstr;
                dbcomm.CommandTimeout = 180;
                if (this.dbconn.State == ConnectionState.Closed) this.dbconn.Open();
                return this.dbcomm.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (this.dbconn.State == ConnectionState.Open) this.dbconn.Close();
            }
        }

        protected void Fill(string sqlstr, DataSet ds, String tbl)
        {
            try
            {
                dbcomm.CommandText = sqlstr;
                dbcomm.CommandTimeout = 180;
                da.Fill(ds, tbl);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public abstract void InitData();

        public virtual void ConfigData()
        {
        }

        public DataSet GetDataSet()
        {
            return ds;
        }

        public DataTable GetDataTable(string tableName)
        {
            return ds.Tables[tableName];
        }

        public ArrayList GetReportList()
        {
            return reportList;
        }

        public string ExportReport(ExportFormatType fileType, string fileName, ReportClass report)
        {

            string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + (!fileName.Equals("") ? fileName : Security.EncryptWithMD5(DateTime.Now.ToString()));
            switch (fileType)
            {
                case ExportFormatType.PortableDocFormat:
                    fileFullName += ".pdf";
                    break;
                case ExportFormatType.HTML32:
                    fileFullName += ".html";
                    break;
                case ExportFormatType.HTML40:
                    fileFullName += ".html";
                    break;
                case ExportFormatType.Excel:
                case ExportFormatType.ExcelRecord:
                    fileFullName += ".xls";
                    break;
                case ExportFormatType.WordForWindows:
                    fileFullName += ".doc";
                    break;
                case ExportFormatType.CrystalReport:
                    fileFullName += ".rpt";
                    break;
                default:
                    fileFullName += ".pdf";
                    break;
            }

            try
            {
               report.ExportToDisk(fileType, fileFullName);
                return fileFullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExportReportToHTML(string fileName, ReportClass report)
        {
            try
            {
                return ExportReport(ExportFormatType.HTML40,fileName,report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConfigReport()
        {
            if (ds != null && reportList != null)
            {
                foreach (ReportClass item in reportList)
                {
                    item.SetDataSource(this.ds);
                }
            }
        }

        public void Dispose()
        {
            foreach (ReportClass item in reportList)
            {
                item.Dispose();
            }
            if (this.reportList != null) this.reportList.Clear();
            if (this.ds != null) this.ds.Dispose();
            if (this.da != null) this.da.Dispose();
            if (this.dbcomm != null) this.dbcomm.Dispose();
            if (this.dbconn != null) this.dbconn.Dispose();
            this.dbconnstr = null;
        }

        public int ExecSql(DBServerType dbType, string connectionString, string sqlstr)
        {
            DbConnection sqlconn = CreateDbConnection(dbType, connectionString);
            DbCommand sqlcomm = CreateDbCommand(dbType);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.CommandText = sqlstr;
            sqlcomm.Connection = sqlconn;
            try
            {
                if (sqlconn.State == ConnectionState.Closed) sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (sqlconn.State == ConnectionState.Open) sqlconn.Close();
                sqlcomm.Dispose();
                sqlconn.Dispose();
            }
        }

        public DataTable GetQueryTable(DBServerType dbType, string connectionString, string sqlstr)
        {
            DbConnection sqlconn = CreateDbConnection(dbType, connectionString);
            DbCommand sqlcomm = CreateDbCommand(dbType);
            DbDataAdapter sqlda = CreateDbDataAdapter(dbType);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.CommandText = sqlstr;
            sqlcomm.Connection = sqlconn;
            sqlda.SelectCommand = sqlcomm;
            DataTable tbl = new DataTable("tbl");
            try
            {
                if (sqlconn.State == ConnectionState.Closed) sqlconn.Open();
                sqlda.Fill(tbl);
                return tbl;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (sqlconn.State == ConnectionState.Open) sqlconn.Close();
                sqlda.Dispose();
                sqlcomm.Dispose();
                sqlconn.Dispose();
            }
        }

        public string GetQueryString(DBServerType dbType, string connectionString, string sqlstr)
        {
            DbConnection sqlconn = CreateDbConnection(dbType, connectionString);
            DbCommand sqlcomm = CreateDbCommand(dbType);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.CommandText = sqlstr;
            sqlcomm.Connection = sqlconn;
            try
            {
                if (sqlconn.State == ConnectionState.Closed) sqlconn.Open();
                return sqlcomm.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (sqlconn.State == ConnectionState.Open) sqlconn.Close();
                sqlcomm.Dispose();
                sqlconn.Dispose();
            }
        }

        public void Fill(DBServerType dbType, string connectionString, string sqlstr, DataSet ds, String tbl)
        {
            DbConnection sqlconn = CreateDbConnection(dbType, connectionString);
            DbCommand sqlcomm = CreateDbCommand(dbType);
            DbDataAdapter sqlda = CreateDbDataAdapter(dbType);
            sqlcomm.CommandType = CommandType.Text;
            sqlcomm.CommandText = sqlstr;
            sqlcomm.Connection = sqlconn;
            sqlda.SelectCommand = sqlcomm;
            try
            {
                if (sqlconn.State == ConnectionState.Closed) sqlconn.Open();
                sqlda.Fill(ds, tbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlconn.State == ConnectionState.Open) sqlconn.Close();
                sqlda.Dispose();
                sqlcomm.Dispose();
                sqlconn.Dispose();
            }
        }

    }

}
