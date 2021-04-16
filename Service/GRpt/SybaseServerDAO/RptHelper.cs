using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Reporting.WebForms;
using Hanbell.DBUtility;

namespace Hanbell.Common.Report
{
    public class RptHelper : DbHelperAdapter
    {
        public RptHelper() { }

        public RptHelper(IDbConnection conn)
        {
            MyConnection = conn;
        }

        public RptHelper(IDbConnection conn, IDbTransaction trans)
        {
            MyConnection = conn;
            MyTrans = trans;
        }


        //public RptHelper(ReportViewer RptViewer, DataSet RptSource, string RptPath, string RptName) { }


        public ReportViewer RptViewer { get; set; }
        public DataTable RptSource { get; set; }
        public string RptPath { get; set; }
        public string RptName { get; set; }




        /// <summary>
        /// 
        /// </summary>
        public void RptBind()
        {
            try
            {
                
                ReportDataSource rds = new ReportDataSource(RptName, RptSource);
                RptViewer.LocalReport.ReportPath = RptPath;
                RptViewer.LocalReport.DataSources.Add(rds);
                RptViewer.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reportPath"></param>
        ///// <param name="reportName"></param>
        ///// <param name="reportSql"></param>
        //public void BindReport(string reportPath,string reportName,string reportSql)
        //{
        //    try
        //    {
        //        DataTable dt = base.Query(reportSql);
        //        if (dt.Rows.Count>0)
        //        {
        //            ReportDataSource rds = new ReportDataSource(reportName, dt);
        //            RptViewer.LocalReport.ReportPath = reportPath;
        //            RptViewer.LocalReport.DataSources.Add(rds);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
            
            
        //}





    }
}
