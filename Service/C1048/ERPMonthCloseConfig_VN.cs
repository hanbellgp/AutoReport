using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ERPMonthCloseConfig_VN:NotificationConfig
    {

        public ERPMonthCloseConfig_VN(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ERPMonthCloseDS();
            this.reportList.Add(new ERPMonthCloseReport());
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {
            string sqlstr =@"select trno as 'trno',trdate as 'date',d.userno as 'userno',r.username as 'name','INV出入库' as 'type' from invhad d,secuser r " +
            " where status='N' and d.userno=r.userno " +
            " and trdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select shpno,d.shpdate,d.userno,r.username,'CDR出货' from cdrhad d,secuser r " +
            " where houtsta='N' and d.userno=r.userno " +
            " and shpdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select bakno,d.bakdate,d.userno,r.username,'CDR退货' from cdrbhad d,secuser r " +
            " where baksta='N' and d.userno=r.userno " +
            " and bakdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select acceptno,d.acceptdate,d.userno,r.username,'验收' from puracd d,secuser r " +
            " where accsta='N' and d.userno=r.userno " +
            " and acceptdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.pisno,d.issdate,d.userno,r.username,'MAN领料' from manpih d,secuser r " +
            " where (issstatus='A' or  issstatus='B' )and d.userno=r.userno " +
            " and issdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.appno,d.appdate,d.appuser,r.username,'申料' from manpah d,secuser r " +
            " where appstatus='N' and d.appuser=r.userno " +
            " and appdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.retno,d.retdate,d.userno,r.username,'MAN退料' from manreh d,secuser r " +
            " where (issstatus='A' or  issstatus='B' ) and  " +
            " d.userno=r.userno " +
            " and retdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " SELECT fshno,fshdat,empno,username,'SFC报工' from sfcfsh d,secuser r " +
            " where (stats='1' ) and  " +
            " d.empno=r.userno " +
            " and fshdat<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " SELECT inpno,indat,keyinuserno,username,'SFC入库' from sfcwah d,secuser r " +
            " where (stats='1' ) and  " +
            " d.keyinuserno=r.userno " +
            " and indat<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union  " +
            " SELECT fshno,fshdat,frmuserno,username,'SFC调拨' from sfctnh d,secuser r " +
            " where (stats='1' ) and  " +
            " d.keyinuserno=r.userno " +
            " and fshdat<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.fshno,d.fshdat,d.keyinuserno,username,'SFC领用' from sfcwph d,secuser r " +
            " where (stats='1' ) and  " +
            " d.keyinuserno=r.userno " +
            " and fshdat<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.trno,d.indate,d.userno,username,'CDRN20借出'  " +
            " from cdrlnhad d,secuser r " +
            " where (status='N' ) and  " +
            " d.userno=r.userno " +
            " and indate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' " +
            " union " +
            " select d.brtrno,d.brdate,d.userno,username,'CDRN30归还'  " +
            " from cdrbrhad d,secuser r " +
            " where (status='N' ) and  " +
            " d.userno=r.userno " +
            " and brdate<left((convert(varchar(8),dateadd(mm,1,getdate()),112)),6)+'01' ";
   

    Fill(sqlstr,ds,"tblresult");

        }
    }
}
