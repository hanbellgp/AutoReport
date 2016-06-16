using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderVendor : NotificationContent
    {
        public SpecOrderVendor()
        {
        }

        protected override void Init()
        {
            base.Init();
            nc = new SpecOrderVendorConfig(Core.DBServerType.SybaseASE, "SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            string[] title = { "项目编号", "采购单号", "厂商编号", "厂商简称", "内容", "物料件号", "数量", "负责人", "姓名", "交货日期", "邮箱" };
            int[] width = { 80, 80, 80, 100, 180, 140, 45, 50, 60, 70, 160 };
            this.content = GetContent(nc.GetDataTable("tblcdrspec"), title, width);

            if (nc.GetDataTable("tblcdrspec").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected override void SendAddtionalNotification()
        {
            string[] title = { "项目编号", "采购单号", "厂商编号", "厂商简称", "内容", "物料件号", "数量", "负责人", "姓名", "交货日期", "邮箱" };
            int[] width = { 80, 80, 80, 120, 160, 140, 45, 50, 60, 70, 160 };
            string[] optstr = new string[] { };
            foreach (DataRow item in this.nc.GetDataTable("tblcdrspec").Rows)
            {
                if (optstr.Contains(item["vdrno"].ToString())) continue;//跳出重复值
                Array.Resize(ref optstr, optstr.Length + 1);
                optstr.SetValue(item["vdrno"].ToString(), optstr.Length - 1);

                this.nc.GetDataTable("tblcdrspec").DefaultView.RowFilter = "vdrno='" + item["vdrno"].ToString() + "'";

                if (item["email"].ToString() != "")
                {
                    NotificationContent msg = new NotificationContent();
                    msg.content = GetContent(nc.GetDataTable("tblcdrspec").DefaultView.ToTable(), title, width);
                    msg.subject = this.subject;
                    msg.AddTo(item["email"].ToString());
                    msg.cc = this.cc;
                    msg.AddNotify(new MailNotify());
                    msg.Update();
                    msg.Dispose();
                }
            }
        }


    }
}
