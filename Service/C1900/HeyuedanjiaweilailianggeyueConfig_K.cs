using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class HeyuedanjiaweilailianggeyueConfig_K : NotificationConfig
    {
        public HeyuedanjiaweilailianggeyueConfig_K()
        {
        }
                public HeyuedanjiaweilailianggeyueConfig_K(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new Heyuedanjiaweilailianggeyue_KDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
                public override void InitData()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT  purcontract.facno,  purcontract.vdrno, purvdr.vdrna,purcontract.itnbr,   invmas.itdsc,purcontract.enddate ");
                    sb.Append("FROM purcontract ,purvdr , invmas where purcontract.vdrno =purvdr.vdrno ");
                    sb.Append("and purcontract.itnbr =invmas.itnbr ");
                    sb.Append("and enddate < CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), dateadd(month,+2,getdate()),112),6) + '01') ");
                    sb.Append("and enddate >= CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), GETDATE(),112),6) + '01') ");
                    sb.Append("order by enddate");

                    Fill(sb.ToString(), ds, "tblresult6");
                }
    }
}
