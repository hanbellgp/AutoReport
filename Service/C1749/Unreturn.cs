using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Unreturn : NotificationContent
    {
        public Unreturn()
        {

        }
        protected override void Init()
        {
            base.Init();
            nc = new UnreturnConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            String[] title = { "对象类别", "借出对象", "借出对象名称", "金额(元)", "本年目标" };
            int[] width = { 100, 100, 100, 100, 100 };
            DataTable dt1 = nc.GetDataTable("unreturn");
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
            newRow["depno"] = "";
            newRow["cdesc"] = "";
            newRow["cost"] = sum;
            newRow["actor"] = "";
            dt1.Rows.Add(newRow);
            dt1.AcceptChanges();
            this.content = GetContent(dt1, title, width);
            if (nc.GetDataTable("unreturn").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
        protected override void SendAddtionalNotification()
        {
            //base.SendAddtionalNotification();
            //string title = nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), "sql");
            string managerId;
            string headperson;
            string table1;
            string[] p = new string[] { };

            String[] title = { "借出单号", "对象类别", "借出对象", "借出对象名称", "借出部门", "借出部门名称", "负责人", "负责人姓名", "借出日期", "预计归还日期", "品号", "品名", "借出数量", "归还数量", "未归还数量", "单位", "逾期天数" };
            int[] width = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 50, 50, 50, 50, 50 };
            foreach (DataRow row in nc.GetDataTable("tlb").Rows)
            {
                if (!p.Contains(row["headperson"].ToString()))
                {
                    Array.Resize(ref p, p.Length + 1);
                    p.SetValue(row["headperson"].ToString(), p.Length - 1);
                }
                table1 = GetHTMLTable(nc.GetDataTable("tlb").DefaultView.ToTable(), title, width);
                NotificationContent msg = new NotificationContent();
                headperson = row["headperson"].ToString();
                //headperson = "C1749";
                if (headperson != "" && headperson != null)
                {
                    msg.AddTo(GetMailAddressByEmployeeIdFromOA(headperson));
                }
                ////抄送直属主管
                //managerId = GetManagerIdByEmployeeIdFromOA(headperson);
                //if (managerId.ToString() != "" && !managerId.ToString().Equals("C0616"))
                //{
                //    msg.AddCc(GetMailAddressByEmployeeIdFromOA(managerId));
                //}
                msg.subject = this.subject;
                msg.content = GetContentHead() + table1 + GetContentFooter();
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }

        }
    }
}
