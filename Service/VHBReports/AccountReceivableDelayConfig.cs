using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using CrystalDecisions.CrystalReports.Engine;
using Hanbell.AutoReport.Config;

namespace Hanbell.AutoReport.VHB
{
    public class AccountReceivableDelayConfig : NotificationConfig
    {
        public AccountReceivableDelayConfig()
        {
        }

        public AccountReceivableDelayConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSAccountReceivableDelay();
            this.reportList.Add(new AccountReceivableDelayReport());
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {

            string baseday = Base.Format(DateTime.Now.Date.ToString(), "yyyyMMdd");

            int day1, day2, day3, day4, day5;
            if (args == null || args.Count != 5)
            {
                day1 = 1; day2 = 2; day3 = 3; day4 = 4; day5 = 5;
            }
            else
            {
                day1 = int.Parse(args["day1"].ToString());
                day2 = day1 + int.Parse(args["day2"].ToString());
                day3 = day2 + int.Parse(args["day3"].ToString());
                day4 = day3 + int.Parse(args["day4"].ToString());
                day5 = day4 + int.Parse(args["day5"].ToString());
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  b.cuskind,b.areacode,a.cusno,b.cusna,j.userno,j.username,");
            sb.Append("SUM(CASE WHEN warngdate > '");
            sb.Append(baseday);
            sb.Append("' THEN (booamt - recamt) ELSE 0 END) AS notdelay,");
            sb.Append("SUM(CASE WHEN warngdate <='");
            sb.Append(baseday);
            sb.Append("' THEN (booamt - recamt) ELSE 0 END) AS isdelay,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) = LEFT('");
            sb.Append(baseday);
            sb.Append("', 6) AND warngdate > '");
            sb.Append(baseday);
            sb.Append("' THEN (booamt - recamt) ELSE 0 END) AS willdelay,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT('");
            sb.Append(baseday);
            sb.Append("', 6) AND LEFT(CONVERT(CHAR(8), warngdate, 112), 6) > LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day1);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) AND warngdate <= '");
            sb.Append(baseday);
            sb.Append("' THEN (booamt - recamt) ELSE 0 END) AS period1,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day1);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) AND LEFT(CONVERT(CHAR(8), warngdate, 112), 6) > LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day2);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) THEN (booamt - recamt) ELSE 0 END) AS period2,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day2);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) AND LEFT(CONVERT(CHAR(8), warngdate, 112), 6) > LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day3);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) THEN (booamt - recamt) ELSE 0 END) AS period3,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day3);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) AND LEFT(CONVERT(CHAR(8), warngdate, 112), 6) > LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day4);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) THEN (booamt - recamt) ELSE 0 END) AS period4,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day4);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) AND LEFT(CONVERT(CHAR(8), warngdate, 112), 6) > LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day5);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) THEN (booamt - recamt) ELSE 0 END) AS period5,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) <= LEFT(CONVERT(CHAR(8), DATEADD(month, ( -1 ) * ");
            sb.Append(day5);
            sb.Append(",'");
            sb.Append(baseday);
            sb.Append("'), 112), 6) THEN (booamt - recamt) ELSE 0 END) AS period6,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) = LEFT('");
            sb.Append(baseday);
            sb.Append("', 6) THEN ( booamt - recamt ) ELSE 0 END) AS thismonrec,");
            sb.Append("SUM(CASE WHEN LEFT(CONVERT(CHAR(8), warngdate, 112), 6) < LEFT('");
            sb.Append(baseday);
            sb.Append("', 6) THEN ( booamt - recamt )  ELSE 0 END) AS beftotal ");
            sb.Append(" FROM armhad a ");
            sb.Append(" LEFT OUTER JOIN cdrcus b  ON a.cusno = b.cusno ");
            sb.Append(" LEFT OUTER JOIN secuser j ON a.mancode = j.userno ");
            sb.Append(" WHERE ( booamt - recamt ) > 0  AND bildat <= '");
            sb.Append(baseday);
            sb.Append("' AND accno = '1310' GROUP BY a.cusno, b.cusna, j.userno,j.username,b.cuskind,b.areacode order by b.cuskind,b.areacode,j.userno ");

            Fill(sb.ToString(), ds, "tblresult");

        }

    }
}
