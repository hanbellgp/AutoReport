using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class QinggousantianweipaozhuanConfig_M3 : QinggousantianweipaozhuanConfig
    {
                public QinggousantianweipaozhuanConfig_M3(DBServerType dbtype, string connName, string notification)
        {
            PrepareDBUtil(dbtype, Base.GetDBConnectionString(connName));
            this.ds = new QinggousantianweipaozhuanDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select   b.batnr as '请购单号' ,b.mrpdate as '请购日期',b.itnbrf as '品号',p.itdsc as '品名',b.manqty as '数量',datediff(day,b.mrpdate,GETDATE())-3 as diff ");
            sb.Append("from   manmot b left join invmas p on p.itnbr = b.itnbrf ");
            sb.Append("where b.facno = '{0}' ");
            sb.Append("AND datediff(day,b.mrpdate,GETDATE()) > 3 ");
            sb.Append("AND Convert(varchar(6),b.mrpdate,112) <= CONVERT(varchar(6),getdate(),112) ");
            sb.Append("order by b.mrpdate ASC ");


            Fill(String.Format(sb.ToString(), args["facno"]), this.ds, "tblprocess");
        }
    }
}
