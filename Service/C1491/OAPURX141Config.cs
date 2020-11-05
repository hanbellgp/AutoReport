using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class OAPURX141Config : Hanbell.AutoReport.Config.NotificationConfig
    {
        public OAPURX141Config(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new OAPURX141DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {


            string sqlstr = @" SELECT a.facno ,a.applyuser ,a.applydate, b.itnbr ,b.itdsc ,b.unmsr1,b.costintax ,
                                b.cost  , b.asscostintax, b.asscost ,(CASE WHEN b.Isactualquo = '1' THEN N'实际报价'  WHEN b.Isactualquo = '2' THEN N'预估单价'
                                WHEN b.Isactualquo = '3' THEN N'预估转实际报价' else b.Isactualquo  end ) as Isactualquo,b.remark
                                FROM  HK_CW003 a ,HK_CW003_Detail b ,ProcessInstance e
                                WHERE  a.formSerialNumber =b.formSerialNumber and a.processSerialNumber = e.serialNumber and e.currentState = 3
                                AND left(convert(varchar(30),a.applydate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                                AND left(convert(varchar(30),a.applydate,111),10) <  convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                                AND a.facno = '{0}'
                                ORDER BY a.applydate";

           // Fill(sqlstr, ds, "tbresult");
            Fill(String.Format(sqlstr, args["facno"]), ds, "tbresult");


        }


       
    }

}
