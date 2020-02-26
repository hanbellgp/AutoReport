using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0160
{
    public class Invmas : NotificationContent
    {

        public Invmas()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new InvmasConfig(Hanbell.AutoReport.Core.DBServerType.SybaseASE, "KNBERP");
            nc.InitData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                string[] title = { "品号", "品名", "规格", "大类", "大类名称", "归类", "科目", "说明" };
                int[] width = { 100, 200, 200, 40, 100, 40, 40, 80 };
                this.content = GetContent(nc.GetDataTable("tbl"), title, width);
                AddNotify(new MailNotify());
            }
        }

    }
}
