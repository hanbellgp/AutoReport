using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class FddxgbConfig : NotificationConfig
    {
      

        public FddxgbConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSFddxgb();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select distinct d.cdrno as cdrno ,d.trseq as trseq,m.trseq as trseq2,h.depno as depno,s.depname as depname,h.cusno as cusno,c.cusna as cusna,d.itnbr as itnbr," +
                "d.itnbrcus as itnbrcus,d.cdrqy1 as cdrqy1,m.modnum as modnum,(select username from secuser where h.userno = secuser.userno) as userno, (select username from secuser where h.mancode = secuser.userno) as mancode,"+
                "m.mark1 as mark1 from cdrhmas h,cdrdmas d,cdrmod m,cdrabn a,cdrcus c,misdept s " +
                "where h.facno=d.facno and h.cdrno=d.cdrno and h.cdrno=m.cdrno " +
                "and m.cdrno=a.abntrno and a.prgno='CDR350' and h.cdrno=a.abntrno " +
                "and h.cusno=c.cusno and s.depno=h.depno and m.trseq=d.trseq and " +
                "datediff(month,a.abntime,getdate())=0  and d.itnbrcus<>''  ";

            Fill(sqlstr, ds, "Fddxgb");

            
        }

        //移除数据表中多余字段
        public override void ConfigData()
        {
            ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
            ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        }
        

    }
}
