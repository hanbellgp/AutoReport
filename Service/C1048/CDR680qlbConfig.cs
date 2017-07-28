using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class CDR680qlbConfig : NotificationConfig
    {
        public CDR680qlbConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR680qlb();
            this.reportList.Add(new CDR680qlbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select c.cusno,r.cusna,CONVERT(varchar(12), c.shpdate,111) as shpdate ,s.shpno,s.itnbr, " +
           "  m.itdsc ,(s.lckqty - s.compqty)as qlsl " +
           "   from cdrasryshort s,cdrdtaasry d ,cdrhad c,invmas m,cdrcus r " +
           "   where s.facno = d.facno and s.prono = d.prono and s.trno=d.trno and s.shpno=d.shpno and s.cdrno=d.cdrno and  " +
           "   s.itnbr=d.itnbr and s.trseq=d.trseq and s.shpno=c.shpno and s.facno=c.facno and s.itnbr=m.itnbr and  " +
           "   r.cusno=c.cusno and s.lckqty - s.compqty>0 ";

            Fill(sqlstr, ds, "CDR680qlb");

            
        }     

    }
}

