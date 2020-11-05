using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class OAHKGL037Config : Hanbell.AutoReport.Config.NotificationConfig
    {
        public OAHKGL037Config()
        {
        }

        public OAHKGL037Config(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new OAHKGL037DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select  distinct  a.hdn_dept,a.emplyer,e.userName  ,a.cctime  , b.address2 ,b.sy , a.sgls  ,a.zgls , a.total  ,a.hctime , a.processSerialNumber ");
            sb.Append("from  HK_GL037 a left outer join Users e on e.id = a.emplyer , HK_GL037_Detail b , ProcessInstance c ");
            sb.Append("where a.formSerialNumber = b.formSerialNumber and c.serialNumber = a.processSerialNumber and c.currentState = 3 ");
            sb.Append("and a.clxz = '2' and (b.ycrq_txt >= CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), dateadd(month,-1,getdate()),112),6) + '01') and b.ycrq_txt < CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), GETDATE(),112),6) + '01')) ");
            //sb.Append("and '1' = {0}");
            sb.Append("order by a.hdn_dept, a.cctime ");

            //Fill(String.Format(sb.ToString(), "1"), this.ds, "tbprivatecar");
            Fill(String.Format(sb.ToString()), this.ds, "tbprivatecar");

        }
    }
}
