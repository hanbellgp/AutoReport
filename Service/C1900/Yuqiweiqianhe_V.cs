using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Yuqiweiqianhe_V : NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            this.nc = new YuqiweiqianheConfig_V(DBServerType.MSSQL, "EFGP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tblprocess1").Rows.Count > 0)
            {
                this.content = this.GetContent(nc.GetDataTable("tblprocess1"), null, null);
                AddNotify(new MailNotify());
            }

        }
        protected override void SendAddtionalNotification()
        {
            string managerId;
            string table1, table2;
            string[] p = new string[] { };
            DataTable tbl;
            foreach (DataRow row in nc.GetDataTable("tblprocess1").Rows)
            {
                if (row["id"].ToString().Substring(0, 1).Equals("H") || row["id"].ToString().Substring(0, 1).Equals("Q") ) continue;
                if (p.Contains(row["id"].ToString())) continue;
                Array.Resize(ref p, p.Length + 1);
                p.SetValue(row["id"].ToString(), p.Length - 1);
                nc.GetDataTable("tblprocess1").DefaultView.RowFilter = " id='" + row["id"].ToString() + "'";
                tbl = nc.GetDataTable("tblprocess1").DefaultView.ToTable();
                tbl.Columns.Remove("deptno");
                tbl.Columns.Remove("dept");
                table1 = GetHTMLTable(tbl, null, null);

                nc.GetDataTable("tblprocess1").DefaultView.RowFilter = " id='" + row["id"].ToString() + "' AND delaydays > 2 ";
                tbl = nc.GetDataTable("tblprocess1").DefaultView.ToTable();
                tbl.Columns.Remove("deptno");
                tbl.Columns.Remove("dept");
                if (tbl.Rows.Count > 0)
                {
                    table2 = "以下单据已超过2天,请向主管说明原因!<br/>" + GetHTMLTable(tbl, null, null);
                }
                else
                {
                    table2 = "没有超过2天及以上的未签核单据";
                }


                NotificationContent msg = new NotificationContent();
                //msg.AddTo("C0160@hanbell.com.cn");
                string useradd = GetMailAddressByEmployeeIdFromOA(row["id"].ToString());
                if (String.IsNullOrEmpty(useradd))
                {
                    continue;
                }
                msg.AddTo(useradd);
                if (tbl.Rows.Count > 0)
                {
                    //抄送直属主管
                    managerId = GetManagerIdByEmployeeIdFromOA(row["id"].ToString());
                    string usermanageradd = GetMailAddressByEmployeeIdFromOA(managerId);
                    if (!String.IsNullOrEmpty(usermanageradd))
                    {
                        msg.AddCc(usermanageradd);
                    }
                   
                   // msg.AddBcc("dtianyu@outlook.com");
                }
            
                msg.subject = row["deptno"].ToString() + row["dept"].ToString() + "_" + this.subject;
                msg.content = GetContentHead() + table1 + "<br/><br/><br/>" + table2 + GetContentFooter();
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }
        }

    }
}
