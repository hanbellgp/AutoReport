using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Globalization;
using Hanbell.AutoReport.Config;


namespace C1587
{
    public class NodeductionConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public NodeductionConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new NodeductionDS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("select a.shpdate as '出货日期',b.cdrno as '出货单号',b.itnbr as '件号',c.itdsc as '件号名称',b.shpqy1 as '出货数量' ");
            sb.Append("from cdrhad a ,cdrdta b,invmas c  where b.itnbr = c.itnbr and a.houtsta = 'N' and a.shpno = b.shpno ");
            sb.Append("and (c.itcls = '3876' or c.itcls = '3879' or c.itcls = '3880' or c.itcls = '3886' or c.itcls = '3889' or c.itcls = '3890' or c.itcls = '3976' or c.itcls = '3979' or c.itcls = '3980')");
            sb.Append("and convert(varchar(8),a.shpdate,112) = convert(varchar(8),dateadd(day,-1,getdate()),112)");
            Fill(sb.ToString(), ds, "tblresult");
        }




    }
}
