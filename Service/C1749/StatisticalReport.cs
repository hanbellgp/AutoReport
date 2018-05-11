using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class StatisticalReport : NotificationContent
    {
        public StatisticalReport() { 
        
        }
        protected override void Init()
        {
            base.Init();

            nc = new StatisticalReportConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
   
            nc.InitData();
            nc.ConfigData();
            DataTable dt= addERPdata();
            //dt.DefaultView.RowFilter = "BQ023C <>''";
            dt = dt.DefaultView.ToTable();
            this.content = GetContentHead() + GetContentFooter();
            if (dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "集团免费服务金额责任归属月统计表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                //DataTableToExcel(nc.GetDataTable("SRtlb"), fileFullName, true);
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
                
            }
        }

        protected DataTable addERPdata()
        {
            DataTable erpDt = nc.GetDataTable("SRtlb");
            DataTable oaDt = GetManagerOIDByEmployeeIdFromOA();
            DataTable crmDt = getManagerOIDByEmployeeIdFromCRM();
            DataTable totalDt = erpDt.Clone();
            try
            {
                for (int i = 0; i < crmDt.Rows.Count; i++)
                {
                    string CRMBQ001 = crmDt.Rows[i]["BQ001"].ToString();
                    for (int j = 0; j < erpDt.Rows.Count; j++)
                    {
                        string kfno = erpDt.Rows[j]["kfno"].ToString();
                        if (CRMBQ001.Equals(kfno))
                        {
                            erpDt.Rows[j]["MY008"] = crmDt.Rows[i]["MY008"];
                            erpDt.Rows[j]["CUSTOMER"] = crmDt.Rows[i]["CUSTOMER"];
                            erpDt.AcceptChanges();

                        }
                    }
                }
                //return erpDt;
                for (int i = 0; i <oaDt.Rows.Count; i++)
                {
                    string BQ001 = oaDt.Rows[i]["BQ001"].ToString();
                    for (int j = 0; j < erpDt.Rows.Count; j++)
                    {
                        string kfno = erpDt.Rows[j]["kfno"].ToString();
                        //erpDt.Rows[i]["BQ023C"] = "test";
                        if (BQ001.Equals(kfno))
                        {

                            erpDt.Rows[j]["BQ023C"] = oaDt.Rows[i]["BQ023C"];
                            erpDt.Rows[j]["BQ133C"] = oaDt.Rows[i]["BQ133C"];
                            erpDt.Rows[j]["propotion"] = oaDt.Rows[i]["propotion"];
                            //DataRow dr = totalDt.NewRow();
                            //dr[j] = erpDt.Rows[j];
                            erpDt.AcceptChanges();

                            //totalDt.Rows.Add(dr[j]);
                            //erpDt.Rows[i]["BQ023C"] = "test";
                        }
                    }
                }
                return erpDt;
            }
            catch(Exception ex) {
                return null;
            }
        }

        protected DataTable GetManagerOIDByEmployeeIdFromOA()
        {
            if (nc == null) return null;
           // string sqlstr = "select BQ001,BQ023C,BQ133C,propotion from SERI12 where datediff(mm,BQ021,getdate())=2 order by BQ001";
            string sqlstr = "select BQ001,BQ023C,BQ133C,propotion from SERI12 where datediff(mm,BQ021,getdate())<4 order by BQ001";
            return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr));

        }

        protected DataTable getManagerOIDByEmployeeIdFromCRM()
        {
            if (nc == null) return null;
            string sql001 = "SELECT DISTINCT BQ001,SERBQ.CUSTOMER as CUSTOMER,MY008 FROM SERBQ,REPTC,PORMZ,PORMY WHERE SERBQ.BQ001 = REPTC.TC054 AND REPTC.TC001 = PORMZ.MZ005 AND REPTC.TC002 = PORMZ.MZ006 AND PORMZ.MZ001 = PORMY.MY001 AND PORMZ.MZ002 = PORMY.MY002 AND SERBQ.CREATE_DATE >='20180101' AND SERBQ.CREATE_DATE <= '20180501' and MY012 = '3'";
            return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("CRM"), String.Format(sql001));
        }
    }
}
