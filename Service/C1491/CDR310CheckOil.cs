using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CDR310CheckOil : Hanbell.AutoReport.Config.NotificationContent
    {
        public CDR310CheckOil()
        {
        }

        protected override void Init()
        {
            base.Init();


            nc = new C1491.CDR310CheckOilConfig(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            string[] title = { "订单编号", "确认人员", "序号", "品号", "客户品号", "客户简称", "规格油品", "身份卡油品" };
            int[] width = { 100, 100, 50, 140, 140, 140, 140, 140 };
            this.content = GetContent(nc.GetDataTable("tbresult"), title, width);

            AddNotify(new MailNotify());

        }

    }
}
