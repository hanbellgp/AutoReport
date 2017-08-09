using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.EFGP
{
    public class WorkItemNeedSign : NotificationContent
    {

        protected override void Init()
        {
            base.Init();
            this.nc = new WorkItemNeedSignConfig(DBServerType.MSSQL, "EFGP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tblprocess").Rows.Count > 0)
            {
                this.content = this.GetContent(nc.GetDataTable("tblprocess"), null, null);
                AddNotify(new MailNotify());
            }

        }

        protected override void SendAddtionalNotification()
        {
            string managerId;
            string table1, table2;
            string[] p = new string[] { };

            foreach (DataRow row in nc.GetDataTable("tblprocess").Rows)
            {
                if (p.Contains(row["id"].ToString())) continue;
                if (row["id"].ToString().Substring(0, 1).Equals("H") || row["id"].ToString().Substring(0, 1).Equals("Q") || row["id"].ToString().Substring(0, 1).Equals("V")) continue;
                Array.Resize(ref p, p.Length + 1);
                p.SetValue(row["id"].ToString(), p.Length - 1);
                nc.GetDataTable("tblprocess").DefaultView.RowFilter = " id='" + row["id"].ToString() + "'";
                table1 = GetHTMLTable(nc.GetDataTable("tblprocess").DefaultView.ToTable(), null, null);

                nc.GetDataTable("tblprocess").DefaultView.RowFilter = " id='" + row["id"].ToString() + "' AND delaydays > 3 ";
                if (nc.GetDataTable("tblprocess").DefaultView.ToTable().Rows.Count > 0)
                {
                    table2 = "以下单据已超过3天<br/>" + GetHTMLTable(nc.GetDataTable("tblprocess").DefaultView.ToTable(), null, null);
                }
                else
                {
                    table2 = "没有超过3天及以上的未签核单据";
                }


                NotificationContent msg = new NotificationContent();
                msg.AddTo(GetMailAddressByEmployeeIdFromOA(row["id"].ToString()));
                //抄送直属主管
                managerId = GetManagerIdByEmployeeIdFromOA(row["id"].ToString());
                if (managerId.ToString() != "" && !managerId.ToString().Equals("C0616"))
                {
                    msg.AddCc(GetMailAddressByEmployeeIdFromOA(managerId));
                }
                if (!row["id"].ToString().Substring(0, 1).Equals("0"))
                {
                    msg.AddCc("C0616@hanbell.com.cn");
                }
                msg.subject = this.subject;
                msg.content = GetContentHead() + table1 + "<br/><br/><br/>" + table2 + GetContentFooter();
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }
        }

    }
}