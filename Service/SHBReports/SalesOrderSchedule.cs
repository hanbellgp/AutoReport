using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SalesOrderSchedule : NotificationContent
    {

        public SalesOrderSchedule()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new SalesOrderScheduleConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            //生成附件后删除Table中的栏位
            nc.GetDataTable("tblresult").Columns.Remove("sn");
            nc.GetDataTable("tblresult").Columns.Remove("remark1");

            string[] title = { "客户编号", "客户简称", "业务", "姓名", "客户单号", "订单", "机型", "品名", "数量", "交货日期", "已入库", "已出货", "制令", "组装日期", "入库日期" };
            int[] width = { 80, 100, 60, 60, 150, 100, 120, 160, 50, 80, 60, 60, 120, 80, 80 };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected override void SendAddtionalNotification()
        {
            string[] title = { "客户编号", "客户简称", "业务", "姓名", "客户单号", "订单", "机型", "品名", "数量", "交货日期", "已入库", "已出货", "制令", "组装日期", "入库日期" };
            int[] width = { 80, 100, 60, 60, 150, 100, 120, 160, 50, 80, 60, 60, 120, 80, 80 };
            string[] optstr = new string[] { };
            foreach (DataRow item in this.nc.GetDataTable("tblresult").Rows)
            {
                if (optstr.Contains(item["mancode"].ToString())) continue;//跳出重复值
                Array.Resize(ref optstr, optstr.Length + 1);
                optstr.SetValue(item["mancode"].ToString(), optstr.Length - 1);
                this.nc.GetDataTable("tblresult").DefaultView.RowFilter = "mancode='" + item["mancode"].ToString() + "'";

                NotificationContent msg = new NotificationContent();
                msg.content = GetContent(nc.GetDataTable("tblresult").DefaultView.ToTable(), title, width);
                msg.subject = this.subject;
                msg.AddTo(item["mancode"].ToString() + "@" + Base.GetMailAccountDomain());
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }
        }

    }
}
