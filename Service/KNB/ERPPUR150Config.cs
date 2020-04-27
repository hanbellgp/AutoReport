using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace KNB.config
{
    class ERPPUR150Config : NotificationConfig
    {
        public ERPPUR150Config(DBServerType dbType, string connName) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ERPPUR150DS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  purcontract.facno,  purcontract.vdrno, purvdr.vdrna,purcontract.itnbr,invmas.itdsc,purcontract.enddate ");
            sb.Append(" FROM purcontract ,purvdr , invmas where purcontract.vdrno =purvdr.vdrno  ");
            sb.Append(" and purcontract.itnbr =invmas.itnbr  ");
            sb.Append(" and enddate < CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), dateadd(month,+2,getdate()),112),6) + '01') ");
            sb.Append(" and enddate >= CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), GETDATE(),112),6) + '01') ");
            sb.Append(" order by enddate ");
            Fill(sb.ToString(), ds, "tbresult");
        }
    }
}
