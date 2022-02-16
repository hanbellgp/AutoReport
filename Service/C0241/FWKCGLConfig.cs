using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0241
{
    public class FWKCGLConfig : NotificationConfig
    {
        public FWKCGLConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new FWKCGLDS();
            this.reportList.Add(new FWKCGLReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {

            string sqlstr = "select '服务部' as '部门','1' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',350 as '目标'," +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='EKF02' " +
            " UNION ALL " +
            " select '服务部' as '部门','2' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',165 as '目标', " +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='EAKF02' " +
            " UNION ALL " +
            //" select '服务部' as '部门','3' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',150 as '目标', " +
            //" h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            //" from invamtshistory y,[test].[dbo].invwh h " +
            //" where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            //" and y.wareh=h.wareh and h.wareh in ('EKF01','KF01') " +
            " SELECT top 1 '服务部' as '部门', '3' as '顺序','上海ERP' as 'ERP区域',a.wareh as '库号',sum(a.je) as '金额',215 as '目标', a.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) from(" +
            " select y.wareh ,(amts/10000) as je, h.whdsc from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            "   and y.wareh=h.wareh and h.wareh in ('EKF01','KF01') )a " + 
            " UNION ALL " +
            //" select '服务部' as '部门','4' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',70 as '目标', " +
            //" h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            //" from invamtshistory y,[test].[dbo].invwh h " +
            //" where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            //" and y.wareh=h.wareh and h.wareh in ('EKF03','KF03') " +
            "SELECT top 1 '服务部' as '部门', '4' as '顺序','上海ERP' as 'ERP区域',a.wareh as '库号',sum(a.je) as '金额',150 as '目标',a.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8)  from " +
            "(select  y.wareh ,(amts/10000) as je,  h.whdsc   from invamtshistory y,[test].[dbo].invwh h " +
            "where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) and y.wareh=h.wareh and h.wareh in ('EKF03','KF03'))a" +
            " union all " +
            " select '服务部' as '部门','5' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',35 as '目标'," +
            " y.wareh as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) and y.wareh='在制' " +
             " UNION ALL " +
             " SELECT top 1 '服务部' as '部门', '6' as '顺序','上海ERP' as 'ERP区域',a.wareh as '库号',sum(a.je) as '金额',18 as '目标', a.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) from(" +
             " select y.wareh ,(amts/10000) as je, h.whdsc from invamtshistory y,[test].[dbo].invwh h " +
             " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
             "   and y.wareh=h.wareh and h.wareh in ('ESKF02') )a " +
             " UNION ALL "+
            " SELECT top 1 '服务部' as '部门', '7' as '顺序','上海ERP' as 'ERP区域',a.wareh as '库号',sum(a.je) as '金额',863 as '目标',a.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8)  from " +
            " (select  y.wareh ,(amts/10000) as je,  h.whdsc   from invamtshistory y,[test].[dbo].invwh h "+
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) and y.wareh=h.wareh and h.wareh in ('EZK09'))a ";
            
            
            

            Fill(sqlstr, this.ds, "tblresult");

            sqlstr = "select trdate,wareh,amts from invamtshistory where datediff(day,convert(datetime,trdate),getdate())<=30 and  wareh in ('EKF03','KF03','EKF02','EAKF02','EKF01','KF01','在制','ESKF02','EZK09') ";
            Fill(sqlstr, this.ds, "tblall");

            sqlstr = "select trdate,wareh,amts from invamtshistory where datediff(day,convert(datetime,trdate),getdate())<=30 and  wareh in {0} ";
            Fill(String.Format(sqlstr, "('EKF02')"), this.ds, "tblKF02");
            Fill(String.Format(sqlstr, "('EAKF02')"), this.ds, "tblAKF02");
            Fill(String.Format(sqlstr, "('EKF01','KF01')"), this.ds, "tblKF01");
            Fill(String.Format(sqlstr, "('EKF03','KF03')"), this.ds, "tblKF03");
            Fill(String.Format(sqlstr, "('在制')"), this.ds, "tblZZ");
            Fill(String.Format(sqlstr, "('ESKF02')"), this.ds, "tlbESKF02");
            Fill(String.Format(sqlstr, "('EZK09')"), this.ds, "tlbEZK09");

        }




    }
}
