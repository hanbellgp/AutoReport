using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderAnalyze : NotificationContent
    {

        public SpecOrderAnalyze()
        {
        }

        protected override void Init()
        {

            base.Init();
            nc = new SpecOrderAnalyzeConfig(Core.DBServerType.SybaseASE, "SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }

            string[] title = { "编号", "客户", "客户简称", "订单编号", "机型", "数量", "计划出货", "实际出货", "延误", "计划入库", "实际入库", "延误", "技术延", "生管延", "采购延" };
            int[] width = { 70, 70, 100, 80, 150, 45, 70, 70, 45, 70, 70, 45, 55, 55, 55 };
            this.content = GetContent(nc.GetDataTable("tblcdrspec"), title, width);

            if (nc.GetDataTable("tblcdrspec").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }

    }
}
