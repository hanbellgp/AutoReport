using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class DpcwlycbConfig : NotificationConfig
    {
        public DpcwlycbConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSDpcwlycb();
            //this.reportList.Add(new DpcwlycbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select a.wareh,a.whdsc,a.itnbr,a.itdsc,a.trnqys,a.trnqy1,a.onhand1,a.unmsr1,a.trdate,a.ts from  "+
        " (select i.wareh,w.whdsc,i.itnbr,m.itdsc,v.trnqys,t.trnqy1,i.onhand1,m.unmsr1, t.trno, "+
        " CONVERT(varchar(12),t.trdate,112) as trdate, "+
        " CONVERT(varchar(12),getdate(),112) as td,v.yearmon "+
        " ,datediff(day,CONVERT(varchar(12),t.trdate,112),CONVERT(varchar(12),getdate(),112))as ts "+
        " from invmas m ,invwh w,invbal i  "+
        " left join invtrn t on i.itnbr=t.itnbr and i.wareh=t.wareh and  t.iocode='1'  "+
        " and datediff(day,CONVERT(varchar(12),t.trdate,112),CONVERT(varchar(12),getdate(),112))>9 "+
        " left join invmon v on i.itnbr=v.itnbr and i.wareh=v.wareh  "+
        " and v.yearmon = CONVERT(varchar(6),dateadd(month,-2,getdate()),112)and v.trtype='ZZZ'and v.trnqys>0 "+
        " where (i.wareh='ZP06' or i.wareh='ZP07' or i.wareh='ZP08')and i.onhand1>0 and i.itnbr=m.itnbr  "+
        " and i.wareh=w.wareh  "+
        " union all "+
        " select i.wareh,w.whdsc,i.itnbr,m.itdsc,v.trnqys,t.trnqy1,i.onhand1,m.unmsr1, t.trno, "+
        " CONVERT(varchar(12),t.trdate,112) as trdate, "+
        " CONVERT(varchar(12),getdate(),112) as td,v.yearmon "+
        " ,datediff(day,CONVERT(varchar(12),t.trdate,112),CONVERT(varchar(12),getdate(),112))as ts "+
        " from invmas m ,invwh w,invbal i  "+
        " left join invtrnh t on i.itnbr=t.itnbr and i.wareh=t.wareh and  t.iocode='1'  "+
        " and  convert(varchar(6),t.trdate,112)>=convert(varchar(6),dateadd(month,-1,getdate()),112) "+
        " left join invmon v on i.itnbr=v.itnbr and i.wareh=v.wareh  "+
        " and v.yearmon = CONVERT(varchar(6),dateadd(month,-2,getdate()),112)and v.trtype='ZZZ'and v.trnqys>0 "+
        " where (i.wareh='ZP06' or i.wareh='ZP07' or i.wareh='ZP08')and i.onhand1>0 and i.itnbr=m.itnbr  "+
        " and i.wareh=w.wareh  " +
        " )a where  a.trnqys is not null or a.trno is not null ";

            Fill(sqlstr, ds, "Dpcwlycb");

            
        }     

    }
}


