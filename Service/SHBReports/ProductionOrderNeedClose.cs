using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ProductionOrderNeedClose : NotificationContent
    {

        public ProductionOrderNeedClose()
        {
        }

        protected override void Init()
        {

            base.Init();
            this.nc = new ProductionOrderNeedCloseConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            string[] title = { "制令编号", "采购单号", "件号", "品名", "厂商", "输入日期", "预计交期", "制令数", "已入库",  "已发料", "已领用", "已点收", "已验收" };
            int[] width = { 80, 80, 120, 180, 70, 80, 80, 50, 50, 50, 50, 50, 50 };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }

    }
}
