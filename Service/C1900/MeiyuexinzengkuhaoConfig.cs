using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class MeiyuexinzengkuhaoConfig : NotificationConfig
    {
        public MeiyuexinzengkuhaoConfig()
        {
        }

        public MeiyuexinzengkuhaoConfig(DBServerType dbtype, string connName, string notification)
        {
            PrepareDBUtil(dbtype, Base.GetDBConnectionString(connName));
            this.ds = new MeiyuexinzengkuhaoDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  * from invwh WHERE indate>= (select   convert(varchar(10),dateadd(dd,-day(dateadd(month,-1,getdate()))+1,dateadd(month,-1,getdate())),112)) ");
            sb.Append("AND indate<=(select   convert(varchar(10),dateadd(dd,-day(getdate()),getdate()),112)) ");
            sb.Append("order by indate ASC ");

            Fill(String.Format(sb.ToString(), args["facno"]), this.ds, "warehtable");
        }

    }
}
