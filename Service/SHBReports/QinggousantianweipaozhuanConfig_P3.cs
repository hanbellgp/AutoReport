using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class QinggousantianweipaozhuanConfig_P3 : QinggousantianweipaozhuanConfig
    {

        public QinggousantianweipaozhuanConfig_P3(DBServerType dbtype, string connName, string notification)
        {
            PrepareDBUtil(dbtype, Base.GetDBConnectionString(connName));
            this.ds = new QinggousantianweipaozhuanDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select   b.sugstno as '请购单号' ,b.sugstdate as '请购日期',b.itnbr as '品号',m.itdsc as '品名',b.prqy1 as '数量',datediff(day,b.sugstdate,GETDATE())-3 as diff ");
            sb.Append("from   pursugst b left join invmas m on m.itnbr = b.itnbr ");
            sb.Append("left join invmas q on q.itnbr = b.itnbr ");
            sb.Append("where b.facno = '{0}' ");
            sb.Append("AND datediff(day,b.sugstdate,GETDATE()) > 3 ");
            sb.Append("AND Convert(varchar(6),b.sugstdate,112) <= CONVERT(varchar(6),getdate(),112) ");
            sb.Append("order by b.sugstdate ASC ");


            Fill(String.Format(sb.ToString(), args["facno"]), this.ds, "tblprocess");
        }
    }
}
