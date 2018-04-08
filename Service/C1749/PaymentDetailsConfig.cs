using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class PaymentDetailsConfig:NotificationConfig
    {
        public PaymentDetailsConfig(DBServerType dbType, string ConnName,string notifaction) 
        {
            PrepareDBUtil(dbType,Base.GetDBConnectionString(ConnName));
            this.ds = new PaymentDetailsDS();
            this.args = Base.GetParameter(notifaction,this.ToString());
        }
        public override void InitData()
        {
            string sqlStr = @"SELECT h.apno AS apno ,CONVERT(NVARCHAR(10), h.apdate, 112) AS apdate,CONVERT(NVARCHAR(10), h.indate, 112) AS indate,h.depno AS depno,
            d.depname AS depname1, i.centerid AS centerid1, f.centerid AS centerid,e.depname AS depname2,h.apusrno AS apusrno,a.cdesc AS cdesc1,
            h.userno AS userno,b.cdesc AS cdesc2,f.apdsc AS apdsc,f.ogdkid AS ogdkid,f.pric AS pric,f.payqty AS payqty,f.coin AS coin,f.ratio AS ratio,
            f.acpamtfs AS acpamtfs,f.budgetacc AS budgetacc,g.accname AS accname FROM apmaph h LEFT JOIN miscode a ON a.code = h.apusrno 
             AND a.ckind = '9E' LEFT JOIN miscode b ON b.code = h.userno AND b.ckind = '9E' LEFT JOIN misdept d ON d.depno = h.depno 
            LEFT JOIN budgetcenter i ON h.depno = i.deptid,apmapd f LEFT JOIN misdept e ON e.depno = f.centerid LEFT JOIN budgetacc g ON g.accno = f.budgetacc 
            WHERE h.facno = f.facno AND h.apno = f.apno  AND h.aptyp = f.aptyp   AND h.apdate > '20180101' AND (DateDiff(DAY ,h.apdate,getdate())=0)";
            Fill(sqlStr, ds, "tlbpayment");
        }
    }
}
