using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using Hanbell.AutoReport.Config;

namespace C1491
{
    class CRMSALMQ01M : Hanbell.AutoReport.Config.NotificationContent
    {

        protected override void Init()
        {
            base.Init();
            nc = new CRMSALMQ01MConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("tbcrmsalmq01m").Rows.Count > 0)
            {
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

                DataTableToExcel(nc.GetDataTable("tbcrmsalmq01m"), GetReportName(this.ToString()), true);
                AddNotify(new MailNotify());
            }

        }
        //protected override void Init()
        //{
        //    base.Init();
        //    this.nc = new CRMSALMQ01MConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
        //    this.nc.InitData();
        //    this.nc.ConfigData();

        //    if (nc.GetDataTable("tbcrmsalmq01m").Rows.Count > 0)
        //    {
        //        this.content = this.GetContent(nc.GetDataTable("tbcrmsalmq01m"), null, null);
        //         AddNotify(new MailNotify());
        //    }

        //}
        /*
        protected override void SendAddtionalNotification()
        {
            string deptid;
            string managerId;
            string table1;
            string[] p = new string[] { };

            foreach (DataRow row in nc.GetDataTable("tbcrmsalmq01m").Rows)
            {
                deptid = row["hdn_dept"].ToString().Substring(0, 2) + "000";
                if (p.Contains(deptid)) continue;
                //if (row["id"].ToString().Substring(0, 1).Equals("H") || row["id"].ToString().Substring(0, 1).Equals("Q") || row["id"].ToString().Substring(0, 1).Equals("V")) continue;
                Array.Resize(ref p, p.Length + 1);
                p.SetValue(deptid, p.Length - 1);
                nc.GetDataTable("tbprivatecar").DefaultView.RowFilter = "hdn_dept like'" + row["hdn_dept"].ToString().Substring(0, 2) + "%'";
                table1 = GetHTMLTable(nc.GetDataTable("tbprivatecar").DefaultView.ToTable(), null, null);

                if (nc.GetDataTable("tbprivatecar").DefaultView.ToTable().Rows.Count > 0)
                {
                    table1 = "以下各部门私车公用人员每月私车公用的汇总表<br/>" + table1;
                }



                NotificationContent msg = new NotificationContent();
                //发送给部门主管（大部门）
                managerId = GetManagerIdByDeptIdFromOA(deptid);
                if (managerId == null || managerId == "")
                {
                    table1 = "未找到单据 , 部门：" + deptid;
                    msg.AddTo(GetMailAddressByEmployeeIdFromOA("C1491"));
                    continue;
                }
                else if (managerId.ToString() != "")
                {
                    msg.AddTo(GetMailAddressByEmployeeIdFromOA(managerId));
                    //msg.AddTo(GetMailAddressByEmployeeIdFromOA("C1491"));
                }

                msg.subject = this.subject;
                msg.content = GetContentHead() + table1 + GetContentFooter();
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }
        }
        */
    }
}
