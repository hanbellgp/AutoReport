using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderPP:NotificationContent
    {

        public SpecOrderPP()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new SpecOrderPPConfig(DBServerType.SybaseASE, "SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            string[] title = { "编号", "项目", "产品名称", "预计交期", "序号", "内容", "物料件号", "数量", "负责人", "姓名", "计划日期", "备注" };
            int[] width = { 80, 200, 160, 70, 45, 160, 140, 45, 50, 60, 70, 220 };
            this.content = GetContent(nc.GetDataTable("tblcdrspec"), title, width);

            if (nc.GetDataTable("tblcdrspec").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }

        protected override void SendAddtionalNotification()
        {
            //string[] title = { "编号", "项目", "产品名称", "预计交期", "序号", "内容", "物料件号", "数量", "负责人", "姓名", "计划日期", "备注" };
            //int[] width = { 80, 200, 160, 70, 45, 160, 140, 45, 50, 60, 70, 220 };
            //string[] optstr = new string[] { };
            //foreach (DataRow item in this.nc.GetDataTable("tblcdrspec").Rows)
            //{
            //    if (optstr.Contains(item["man"].ToString())) continue;//跳出重复值
            //    Array.Resize(ref optstr, optstr.Length + 1);
            //    optstr.SetValue(item["man"].ToString(), optstr.Length - 1);
            //    this.nc.GetDataTable("tblcdrspec").DefaultView.RowFilter = "man='" + item["man"].ToString() + "'";

            //    NotificationContent msg = new NotificationContent();
            //    msg.content = GetContent(nc.GetDataTable("tblcdrspec").DefaultView.ToTable(), title, width);
            //    msg.subject = this.subject;
            //    msg.AddTo(item["man"].ToString() + "@hanbell.com.cn");
            //    msg.AddCc(GetManagerIdByEmployeeIdFromOA(item["man"].ToString()) + "@hanbell.com.cn");//抄送给直接主管
            //    foreach (var receiver in to.Values)
            //    {
            //        msg.AddTo(receiver.ToString());
            //    }
            //    foreach (var copy in cc.Values)
            //    {
            //        msg.AddCc(copy.ToString());
            //    }
            //    foreach (var copy in bcc.Values)
            //    {
            //        msg.AddBcc(copy.ToString());
            //    }
            //    msg.AddNotify(new MailNotify());
            //    msg.Update();
            //    msg.Dispose();
            //}
        }

    }
}
