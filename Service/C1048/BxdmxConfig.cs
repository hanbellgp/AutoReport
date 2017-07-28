using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class BxdmxConfig : NotificationConfig
    {
        public BxdmxConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSBxdmx();
            this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }
        public override void InitData()
        {
            Fill("SELECT bmpa00c AS, bmpa06c,bmpa06cC,bmpa01c,bmpa07c,bmpa07cC,dept,deptC,bmpb20f,text1," +
            " datetime1,text3,textarea1,bz,bxd002002 ,bmpb05c " +
            " FROM dbo.bxd002,dbo.bxd002_1 ,resda " +
            " WHERE bxd002001=bxd002_1001 AND bxd002002=bxd002_1002 AND (bmpb05c='6731' OR bmpb05c='6631' )  " +
            " AND DateDiff(month,resda019,getdate())=1 AND resda021='2' AND resda001=bxd002001   " +
            " AND resda020='3' AND resda002=bxd002002  AND resda002=bxd002_1002 AND bmpa00c='C'", ds, "Bxdmx");

            
        }


    }
}
