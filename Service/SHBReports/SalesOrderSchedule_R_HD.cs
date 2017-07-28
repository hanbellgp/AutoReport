using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SalesOrderSchedule_R_HD : SalesOrderSchedule_R
    {
        public SalesOrderSchedule_R_HD()
        {

        }

        protected override void Init()
        {
            SetMailHead();

            nc = new SalesOrderScheduleConfig_R_HD(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            string[] title = { "客户编号", "客户简称", "业务", "姓名", "客户单号", "订单", "机型", "品名", "数量", "交货日期", "已入库", "已出货", "制令", "组装日期", "入库日期" };
            int[] width = { 80, 100, 60, 60, 120, 100, 120, 160, 50, 80, 60, 60, 120, 80, 80 };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected override void SendAddtionalNotification()
        {

        }
    }
}
