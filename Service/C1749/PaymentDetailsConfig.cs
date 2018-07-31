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
//            string sqlStr = @"SELECT h.apno AS apno ,CONVERT(NVARCHAR(10), h.apdate, 112) AS apdate,CONVERT(NVARCHAR(10), h.indate, 112) AS indate,h.depno AS depno,
//                            d.depname AS depname1, i.centerid AS centerid1, f.centerid AS centerid,e.cdesc AS depname2,h.apusrno AS apusrno,a.cdesc AS cdesc1,
//                            h.userno AS userno,b.cdesc AS cdesc2,f.apdsc AS apdsc,f.ogdkid AS ogdkid,f.pric AS pric,f.payqty AS payqty,f.coin AS coin,f.ratio AS ratio,
//                            f.acpamtfs AS acpamtfs,f.budgetacc AS budgetacc,g.accname AS accname FROM apmaph h 
//			                LEFT JOIN miscode a ON a.code = h.apusrno AND a.ckind = '9E'
//			                LEFT JOIN miscode b ON b.code = h.userno AND b.ckind = '9E'
//			                LEFT JOIN misdept d ON d.depno = h.depno
//                            LEFT JOIN budgetcenter i ON h.depno = i.deptid,
//			                apmapd f LEFT JOIN miscode e ON e.code = f.centerid and e.ckind='9N'
//			                LEFT JOIN budgetacc g ON g.accno = f.budgetacc
//                            WHERE h.facno = f.facno AND h.apno = f.apno  AND h.aptyp = f.aptyp AND ( h.aptyp = '5' OR h.aptyp = '0')--费用类请款0,5分摊
//			                AND i.centerid <> f.centerid
// 			                AND h.apdate > '20180101' AND (DateDiff(DAY ,h.apdate,getdate())=0)";

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT h.apno AS apno ,CONVERT(NVARCHAR(10), h.apdate, 112) AS apdate,CONVERT(NVARCHAR(10), h.indate, 112) AS indate,h.depno AS depno,");
            sql.Append(" d.depname AS depname1, i.centerid AS centerid1, f.centerid AS centerid,e.cdesc AS depname2,h.apusrno AS apusrno,a.cdesc AS cdesc1,");
            sql.Append(" h.userno AS userno,b.cdesc AS cdesc2,f.apdsc AS apdsc,f.ogdkid AS ogdkid,f.pric AS pric,f.payqty AS payqty,f.coin AS coin,f.ratio AS ratio,");
            sql.Append(" f.acpamtfs AS acpamtfs,f.budgetacc AS budgetacc,g.accname AS accname FROM apmaph h ");
            sql.Append(" LEFT JOIN miscode a ON a.code = h.apusrno AND a.ckind = '9E'");
            sql.Append(" LEFT JOIN miscode b ON b.code = h.userno AND b.ckind = '9E'");
            sql.Append(" LEFT JOIN misdept d ON d.depno = h.depno");
            sql.Append(" LEFT JOIN budgetcenter i ON h.depno = i.deptid,");
            sql.Append(" apmapd f LEFT JOIN miscode e ON e.code = f.centerid and e.ckind='9N'");
            sql.Append(" LEFT JOIN budgetacc g ON g.accno = f.budgetacc");
            sql.Append(" WHERE h.facno = f.facno AND h.apno = f.apno  AND h.aptyp = f.aptyp AND ( h.aptyp = '5' OR h.aptyp = '0')");
            sql.Append(" AND i.centerid <> f.centerid");
            sql.Append(" AND h.apdate > '20180101' AND (DateDiff(DAY ,h.apdate,getdate())=0)");
            Fill(sql.ToString(), ds, "tlbpayment");
        }
    }
}
