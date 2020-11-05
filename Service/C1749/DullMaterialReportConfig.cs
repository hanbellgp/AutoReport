using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class DullMaterialReportConfig : NotificationConfig
    {
        public DullMaterialReportConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ToolStockDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
            string date2 = DateTime.Now.AddMonths(-3).ToString("yyyyMM");
            string date3 = DateTime.Now.AddMonths(-6).ToString("yyyyMM");
            string date4 = DateTime.Now.AddMonths(-12).ToString("yyyyMM");

            //string wherein = "'ZZZ', '000', 'ADD', 'ADJ', 'AIG', 'AJI', 'AJO', 'AOG', 'CAA', 'CAD', 'CCA', 'CDS', 'DIS', 'IAC', 'IAD', 'IAE', 'IAH','IAJ', 'IAK', 'IJT', 'ITB', 'JR1', 'JR2', 'PBA', 'RAD'";
            //var wherein2 = wherein.Split(',');
            //var wherein3 = string.Join("or m.type = ", wherein2);

            string sqlca = getSql(date1, date1);

            string sqlca2 = getSql(date1, date2);
            System.Data.DataTable dt = new System.Data.DataTable();
            
            Fill(sqlca, ds, "dbtlb");
        }

        private string getSql(string date1, string date2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select m.wareh,h.whdsc,m.itnbr,s.itdsc,sum(m.trnqys) AS trnqys1,0 AS trnqys2,0 AS trnqys3,0 AS trnqys4,0 AS trnqys5 ");
            sb.Append(" from invmon m LEFT JOIN invwh h ON h.facno = m.facno AND h.prono = m.prono AND m.wareh = h.wareh  ");
            sb.Append(" LEFT JOIN invmas s ON s.itnbr = m.itnbr  ");
            sb.Append(" where m.facno = 'C' and m.prono = '1'  ");
            sb.Append(" and m.trtype not in ('ZZZ','000','ADD','ADJ','AIG','AJI','AJO','AOG','CAA','CAD','CCA','CDS','DIS','IAC','IAD','IAE','IAH', ");
            sb.Append(" 'IAJ','IAK','IJT','ITB','JR1','JR2','PBA','RAD')  ");
            sb.Append(" and m.trnqys>0 ");
            if (!"".Equals(date1))
            {
                sb.Append(" and m.yearmon <= '201910'").Append(date1);
            }
            if (!"".Equals(date2))
            {
                sb.Append(" and m.yearmon > '201909' ").Append(date2);
            }
            sb.Append(" group by m.wareh,h.whdsc,m.itnbr,s.itdsc  ");
            sb.Append(" UNION ALL  ");
            sb.Append(" select m.wareh,h.whdsc,m.itnbr,s.itdsc,sum(m.trnqys) AS trnqys1,0 AS trnqys2,0 AS trnqys3,0 AS trnqys4,0 AS trnqys5  ");
            sb.Append(" from invmonh m LEFT JOIN invwh h ON h.facno = m.facno AND h.prono = m.prono AND m.wareh = h.wareh  ");
            sb.Append(" LEFT JOIN invmas s ON s.itnbr = m.itnbr  ");
            sb.Append(" where m.facno = 'C' and m.prono = '1'  ");
            sb.Append(" and m.trtype not in ('ZZZ','000','ADD','ADJ','AIG','AJI','AJO','AOG','CAA','CAD','CCA','CDS','DIS','IAC','IAD','IAE','IAH', ");
            sb.Append(" 'IAJ','IAK','IJT','ITB','JR1','JR2','PBA','RAD')  ");
            sb.Append(" and m.trnqys>0 ");
            if (!"".Equals(date1))
            {
                sb.Append(" and m.yearmon <= '201910'").Append(date1);
            }
            if (!"".Equals(date2))
            {
                sb.Append(" and m.yearmon > '201909' ").Append(date2);
            }
            sb.Append(" group by m.wareh,h.whdsc,m.itnbr,s.itdsc  ");
            return sb.ToString();
        }

        private string getDate()
        {
            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
            string date2 = DateTime.Now.AddMonths(-3).ToString("yyyyMM");
            string date3 = DateTime.Now.AddMonths(-6).ToString("yyyyMM");
            string date4 = DateTime.Now.AddMonths(-12).ToString("yyyyMM");


            return "";
        }
    }
}
