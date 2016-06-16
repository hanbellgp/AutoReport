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

            string sqlstr = "select '服务部' as '部门','1' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',315 as '目标'," +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='KF02' " +
            " UNION ALL " +
            " select '服务部' as '部门','2' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',80 as '目标', " +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='AKF02' " +
            " UNION ALL " +
            " select '服务部' as '部门','3' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',100 as '目标', " +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='KF01' " +
            " UNION ALL " +
            " select '服务部' as '部门','4' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',70 as '目标', " +
            " h.whdsc as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y,[test].[dbo].invwh h " +
            " where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " and y.wareh=h.wareh and h.wareh='KF03' " +
            " union all " +
            " select '服务部' as '部门','5' as '顺序','上海ERP' as 'ERP区域' ,y.wareh as '库号',(amts/10000) as '金额',15 as '目标'," +
            " y.wareh as '库名',left(convert(char(8), dateadd(day, -1, getdate()), 112),8) " +
            " from invamtshistory y where trdate=left(convert(char(8), dateadd(day, -1, getdate()), 112),8) and y.wareh='在制' ";
            Fill(sqlstr, this.ds, "tblresult");

            sqlstr = "select trdate,wareh,amts from invamtshistory where datediff(day,convert(datetime,trdate),getdate())<=30 and  wareh in ('KF03','KF02','AKF02','KF01','在制') ";
            Fill(sqlstr, this.ds, "tblall");

            sqlstr = "select trdate,wareh,amts from invamtshistory where datediff(day,convert(datetime,trdate),getdate())<=30 and  wareh ='{0}' ";
            Fill(String.Format(sqlstr, "KF02"), this.ds, "tblKF02");
            Fill(String.Format(sqlstr, "AKF02"), this.ds, "tblAKF02");
            Fill(String.Format(sqlstr, "KF01"), this.ds, "tblKF01");
            Fill(String.Format(sqlstr, "KF03"), this.ds, "tblKF03");
            Fill(String.Format(sqlstr, "在制"), this.ds, "tblZZ");


        }




    }
}
