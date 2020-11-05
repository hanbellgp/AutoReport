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
            //上海ERP
            nc = new StatisticalReportConfig(DBServerType.MSSQL, "EFGP", this.ToString());
            //柯茂ERP
            //nc = new StatisticalReportConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
   
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
            DataTable oaDt = nc.GetDataTable("SRtlb");
            DataTable erpHzDt = GetManagerOIDByEmployeeIdFromERP();//汉钟的材料费
            DataTable erpCmDt = GetManagerOIDByEmployeeIdFromERP_Comer();//柯茂的材料费
            IEnumerable<DataRow> query1 = erpHzDt.AsEnumerable().Union(erpCmDt.AsEnumerable(), DataRowComparer.Default);
            DataTable erpDt = query1.CopyToDataTable();

            DataTable crmDt = getManagerOIDByEmployeeIdFromCRM();//CRM差旅费
            DataTable yfDt = new DataTable();
            DataTable yfOADt = getManagerOIDByEmployeeIdFromOAForYf();//OA中的运费
            DataTable yfERPDt = getManagerOIDByEmployeeIdFromERPForYF();//ERP中的运费
            DataTable yfERPDt_Comer = getManagerOIDByEmployeeIdFromERPForYF_Comer();//ERP中的运费
            //获取两个数据源（Datatable）的并集
            IEnumerable<DataRow> query2 = yfOADt.AsEnumerable().Union(yfERPDt.AsEnumerable().Union(yfERPDt_Comer.AsEnumerable(), DataRowComparer.Default), DataRowComparer.Default);
            yfDt = query2.CopyToDataTable();
            DataTable totalDt = erpDt.Clone();
            try
            {
                // OA 的 客诉不良原因 责任判定 责任判定比率
                for (int i = 0; i < oaDt.Rows.Count; i++)
                {
                    string BQ001 = oaDt.Rows[i]["BQ001"].ToString().Trim();
                    for (int j = 0; j < erpDt.Rows.Count; j++)
                    {
                        string kfno = erpDt.Rows[j]["kfno"].ToString().Trim();
                        //erpDt.Rows[i]["BQ023C"] = "test";
                        if (BQ001 == kfno )
                        {
                            oaDt.Rows[i]["trno"] = erpDt.Rows[j]["trno"].ToString();
                            oaDt.Rows[i]["resno"] = erpDt.Rows[j]["resno"].ToString();
                            oaDt.Rows[i]["itnbr"] = erpDt.Rows[j]["itnbr"].ToString();
                            oaDt.Rows[i]["itdsc"] = erpDt.Rows[j]["itdsc"].ToString();
                            oaDt.Rows[i]["trnqy1"] = erpDt.Rows[j]["trnqy1"].ToString();
                            oaDt.Rows[i]["tramt"] = erpDt.Rows[j]["tramt"].ToString();
                            oaDt.AcceptChanges();

                        }
                    }
                }
                // CRM取差旅费和客户
                for (int i = 0; i < oaDt.Rows.Count; i++)
                {
                    string OABQ001 = oaDt.Rows[i]["BQ001"].ToString().Trim();
                   
                    for (int j = 0; j < crmDt.Rows.Count; j++)
                    {
                        string CRMBQ001 = crmDt.Rows[j]["BQ001"].ToString().Trim();

                        if (OABQ001 == CRMBQ001)
                        {
                            oaDt.Rows[i]["MY008"] = crmDt.Rows[j]["MY008"].ToString();
                            oaDt.AcceptChanges();
                        }
                    }
                }
                //OA中的运费
                for (int i = 0; i < oaDt.Rows.Count; i++)
                {
                    string OABQ001 = oaDt.Rows[i]["BQ001"].ToString().Trim();
                    for (int j = 0; j < yfDt.Rows.Count; j++)
                    {
                        string yfBQ001 = yfDt.Rows[j]["kfno"].ToString().Trim();
                        if (OABQ001 == yfBQ001)
                        {
                            oaDt.Rows[i]["total"] = yfDt.Rows[j]["total"].ToString();
                            oaDt.AcceptChanges();
                        }

                    }
                }
                return oaDt;
            }
                
            catch(Exception ex) {
                return null;
            }
        }


        //汉钟 ERP 中 取 数量和材料费 
        protected DataTable GetManagerOIDByEmployeeIdFromERP()
        {
            if (nc == null) return null;
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(" select a.facno as facno,a.prono as prono,a.kfno as kfno,a.trno as trno,a.itnbr as itnbr,a.resno as resno, a.itdsc as itdsc,a.varnr as varnr,a.trnqy1 as trnqy1,t.tramt as tramt from ( ");
            sqlstr.Append(" SELECT h.facno,h.prono,h.kfno,h.trno,d.itnbr,h.resno, v.itdsc,d.varnr,d.trnqy1,d.trseq,h.trtype ");
            sqlstr.Append(" from invdtah d  LEFT JOIN  invhadh h on d.facno=h.facno and d.prono =h.prono and d.trno = h.trno ");
            sqlstr.Append(" LEFT JOIN  invmas v on d.itnbr = v.itnbr ");
            sqlstr.Append(" where d.facno='C' and d.prono='1' and h.facno='C' and h.prono='1' and (h.trtype in ('IAF' ,'IAG')) ");
            sqlstr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND convert(varchar(8),h.trdate,112)>='20180101' and convert(varchar(8),h.trdate,112)<='20190331' ");
            sqlstr.Append(" )as a ");
            sqlstr.Append(" LEFT JOIN  invtrnh t on a.facno=t.facno and a.prono=t.prono and a.itnbr = t.itnbr  and  a.trno= t.trno  and t.trseq=a.trseq and t.trno= a.trno and t.trtype = a.trtype ");
            sqlstr.Append(" where t.facno='C' and (t.trtype ='IAF' or t.trtype= 'IAG') and t.prono= '1' and year(trdate)>=2018 ");

            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBERP"), sqlstr.ToString());
        }
        //柯茂ERP 中 取 数量和材料费 
        protected DataTable GetManagerOIDByEmployeeIdFromERP_Comer()
        {
            if (nc == null) return null;
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(" select a.facno as facno,a.prono as prono,a.kfno as kfno,a.trno as trno,a.itnbr as itnbr,a.resno as resno, a.itdsc as itdsc,a.varnr as varnr,a.trnqy1 as trnqy1,t.tramt as tramt from ( ");
            sqlstr.Append(" SELECT h.facno,h.prono,h.kfno,h.trno,d.itnbr,h.resno, v.itdsc,d.varnr,d.trnqy1,d.trseq,h.trtype ");
            sqlstr.Append(" from invdtah d  LEFT JOIN  invhadh h on d.facno=h.facno and d.prono =h.prono and d.trno = h.trno ");
            sqlstr.Append(" LEFT JOIN  invmas v on d.itnbr = v.itnbr ");
            sqlstr.Append(" where d.facno='K' and d.prono='1' and h.facno='K' and h.prono='1' and (h.trtype in ('IAF' ,'IAG')) ");
            sqlstr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND convert(varchar(8),h.trdate,112)>='20180101' and convert(varchar(8),h.trdate,112)<='20190331' ");
            sqlstr.Append(" )as a ");
            sqlstr.Append(" LEFT JOIN  invtrnh t on a.facno=t.facno and a.prono=t.prono and a.itnbr = t.itnbr  and  a.trno= t.trno  and t.trseq=a.trseq and t.trno= a.trno and t.trtype = a.trtype ");
            sqlstr.Append(" where t.facno='K' and (t.trtype ='IAF' or t.trtype= 'IAG') and t.prono= '1' and year(trdate)>=2018 ");

            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("ComerERP"), sqlstr.ToString());
        }

       //CRM取差旅费和客户
        protected DataTable getManagerOIDByEmployeeIdFromCRM()
        {
            if (nc == null) return null;
            StringBuilder sql001 = new StringBuilder();
            sql001.Append("SELECT DISTINCT BQ001,SERBQ.CUSTOMER as CUSTOMER,MY008,SERBQ.BQ501 as BQ501 FROM SERBQ,REPTC,PORMZ,PORMY WHERE SERBQ.BQ001 = REPTC.TC054 AND REPTC.TC001 = PORMZ.MZ005 AND REPTC.TC002 = PORMZ.MZ006 ");
            //sql001.Append(" AND PORMZ.MZ001 = PORMY.MY001 AND PORMZ.MZ002 = PORMY.MY002 AND  MY012 = '3' AND year(SERBQ.CREATE_DATE)=2018 and datediff(mm,SERBQ.CREATE_DATE,getdate())<=12 ");
            sql001.Append(" AND PORMZ.MZ001 = PORMY.MY001 AND PORMZ.MZ002 = PORMY.MY002 AND  MY012 = '3' AND convert(varchar(7),SERBQ.CREATE_DATE,111)>='2018/01' AND convert(varchar(7),SERBQ.CREATE_DATE,111)<='2019/02' ");
            //return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("CRM"), String.Format(sql001));
            return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("CRM"),sql001.ToString());
        }


        //OA中取运费
        protected DataTable getManagerOIDByEmployeeIdFromOAForYf() {
            StringBuilder yfSql = new StringBuilder();
            if(nc == null){
                return null;
            }
            yfSql.Append("SELECT kfno,fwno,total,cusno,cusna FROM ( SELECT  DISTINCT h5.* FROM  HK_FW005 h5 ");
            yfSql.Append(" INNER JOIN  ProcessInstance pi on h5.processSerialNumber=pi.serialNumber ");
            yfSql.Append(" INNER JOIN WorkItem wi on pi.contextOID=wi.contextOID  where ( h5.fwno <> '' and h5.fwno <> '-') and (h5.kfno <> '' and h5.kfno <> '-') ");
            yfSql.Append(" and h5.total <> 0 and h5.type in ('1','2','3') and h5.mftype <> '' ");
            //yfSql.Append(" AND year(wi.completedTime)>=2018 AND datediff(mm,wi.completedTime,getdate())>=1 ) as a ");
            yfSql.Append(" AND wi.completedTime>='20180101' AND wi.completedTime<='20190331' ) as a ");
            yfSql.Append(" UNION  all ");
            yfSql.Append(" SELECT kfno,fwno,yf as 'tatal',cusno,cusna FROM ( SELECT DISTINCT h6.* FROM HK_FW006 h6 ");
            yfSql.Append(" INNER JOIN ProcessInstance pi on h6.processSerialNumber=pi.serialNumber ");
            yfSql.Append(" INNER JOIN WorkItem wi on pi.contextOID= wi.contextOID WHERE ( h6.fwno <> '' and  h6.fwno <> '-') and ( h6.kfno <> '' and  h6.kfno <> '-') ");
            yfSql.Append(" and h6.yf>0 and h6.rettype = 2 and h6.returntype in ('2','4','8') ");
            //yfSql.Append(" AND year(wi.completedTime)>=2018 AND datediff(mm,wi.completedTime,getdate())>=1 ) as a ");
            yfSql.Append(" AND wi.completedTime>='20180101' AND wi.completedTime<='20190331' ) as a ");
            return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), yfSql.ToString());
        }
        //汉钟ERP CDRN20的运费
        public DataTable getManagerOIDByEmployeeIdFromERPForYF()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select kfno,fwno,freight as total,h.cusno,s.cusna from cdrlnhad h LEFT JOIN cdrfre c on c.shpno = h.trno and c.facno = h.facno LEFT JOIN cdrcus s on h.cusno = s.cusno ");
            sb.Append(" where  h.facno='C' AND h.status ='Y' and  h.resno = '03' and ( h.fwno <> ''and h.fwno <> '-') and (h.kfno <> '' and h.kfno <> '-') and c.freight> 0 ");
            sb.Append(" AND YEAR(h.cfmdate)>=2018 ");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBERP"), sb.ToString());
        }

        //柯茂ERP CDRN20的运费
        public DataTable getManagerOIDByEmployeeIdFromERPForYF_Comer()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select kfno,fwno,freight as total,h.cusno,s.cusna from cdrlnhad h LEFT JOIN cdrfre c on c.shpno = h.trno and c.facno = h.facno LEFT JOIN cdrcus s on h.cusno = s.cusno ");
            sb.Append(" where  h.facno='K' AND h.status ='Y' and  h.resno = '03' and ( h.fwno <> ''and h.fwno <> '-') and (h.kfno <> '' and h.kfno <> '-') and c.freight> 0 ");
            sb.Append(" AND YEAR(h.cfmdate)>=2018 ");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("ComerERP"), sb.ToString());
        }


        //ERP中材料费暂时不用
        public DataTable getManagerOIDByEmployeeIdFromERPForCLF() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.facno as facno,a.prono as prono,a.trno as trno,sum(a.tramt) as tramt from  invtrnh a where  a.facno='C' and a.prono='1' ");
            //sb.Append(" and (a.trtype in ('IAF' ,'IAG')) AND year(a.trdate)=2018  and datediff(mm,a.trdate,getdate())<=12 GROUP BY  a.facno,a.prono,a.trno");
            sb.Append(" and (a.trtype in ('IAF' ,'IAG')) AND a.trdate>='20180101' AND a.trdate<='20190331' GROUP BY  a.facno,a.prono,a.trno");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("ComerERP"), sb.ToString());
        }
    }
}
