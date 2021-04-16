using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class unreturn_why : NotificationContent
    {
        public unreturn_why() { }

        protected override void Init()
        {
            base.Init();
            nc = new UnreturnWhyConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            String[] title = { "对象类别", "借出原因", "借出对象名称", "金额(元)", "本年目标" };
            int[] width = { 100, 100, 100, 100, 100 };
            DataTable dt1 = nc.GetDataTable("unreturn_why");
            Double sum = 0;
            foreach (DataRow item in dt1.Rows)
            {
                if (item["cost"] != null)
                {
                    sum += Convert.ToDouble(item["cost"].ToString());
                }
            }
            DataRow newRow;
            newRow = dt1.NewRow();
            newRow["objtype"] = "合计";
            newRow["cdesc"] = "";
            newRow["cost"] = sum;
            newRow["actor"] = "";
            dt1.Rows.Add(newRow);
            dt1.AcceptChanges();
            this.content = GetContent(dt1, title, width);
            if (nc.GetDataTable("unreturn_why").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
