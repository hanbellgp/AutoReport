using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class QinggousantianweipaozhuanConfig_V : NotificationConfig
    {
        public QinggousantianweipaozhuanConfig_V()
        {
        }

        public QinggousantianweipaozhuanConfig_V(DBServerType dbtype, string connName, string notification)
        {
            PrepareDBUtil(dbtype, Base.GetDBConnectionString(connName));
            this.ds = new QinggousantianweipaozhuanDS_V();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select   a.prno as '请购单号' ,a.prdate as '请购日期',b.itnbr as '品号',m.itdsc as '品名',b.prqy1 as '数量',datediff(day,a.prdate,GETDATE())-3 as diff ");
            sb.Append("from  purhask a ,purdask b ");
            sb.Append("left join invmas m on m.itnbr = b.itnbr ");
            sb.Append("where a.facno = '{0}' and a.prno = b.prno and a.facno = b.facno and a.prono = b.prono ");
            sb.Append("and datediff(day,a.prdate,GETDATE()) > 3 ");
            sb.Append("AND b.dasksta = '20' ");
           // sb.Append("AND a.depno = '1P300' ");
            sb.Append("AND a.prkind = '3NA' ");
            sb.Append("AND Convert(varchar(6),a.prdate,112) <= CONVERT(varchar(6),getdate(),112) ");
            sb.Append("order by a.prdate ASC ");


            Fill(String.Format(sb.ToString(), args["facno"]), this.ds, "tblprocessv");
        }
    }
}
