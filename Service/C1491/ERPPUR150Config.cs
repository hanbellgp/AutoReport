using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Globalization;
using Hanbell.AutoReport.Config;

namespace C1491
{
    public class ERPPUR150Config : Hanbell.AutoReport.Config.NotificationConfig
    {
        public ERPPUR150Config(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSPUR150();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {


            string sqlstr = @" SELECT  purcontract.facno,  purcontract.vdrno, purvdr.vdrna,purcontract.itnbr,   invmas.itdsc,purcontract.enddate
			FROM purcontract ,purvdr , invmas where purcontract.vdrno =purvdr.vdrno 
			and purcontract.itnbr =invmas.itnbr 
			and enddate < CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), dateadd(month,+2,getdate()),112),6) + '01') and enddate >= CONVERT(DATETIME, LEFT(CONVERT(CHAR(8), GETDATE(),112),6) + '01')
			order by enddate";

            Fill(sqlstr, ds, "tbresult");


        }


       
    }
}
