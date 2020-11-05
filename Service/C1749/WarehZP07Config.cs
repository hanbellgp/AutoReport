using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WarehZP07Config:NotificationConfig
    {
        public WarehZP07Config(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new WarehZP07DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = @"select  a.wareh,a.itnbr,b.itdsc,a.trnqy1,a.unmsr1 from invmas b,invtrn a  where a.itnbr = b.itnbr and a.trtype = 'IAC' and a.wareh = 'ZP07' and  iocode = '1' and  datediff(dd,a.indate,getdate())=1";
            //string sqlstr = @"select  a.wareh,a.itnbr,b.itdsc,a.trnqy1,a.unmsr1 from invmas b,invtrn a  where a.itnbr = b.itnbr and a.trtype = 'IAC' and a.wareh = 'ZP07' and  iocode = '1' and  a.indate >= '20180518' and a.indate <  '20180519'";
            Fill(sqlstr, ds, "ZP07tlb");
        }
    }
}
