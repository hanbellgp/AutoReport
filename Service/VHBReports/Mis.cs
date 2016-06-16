using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.VHB
{
    public class Mis:NotificationContent
    {
        public Mis()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new MisConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc.InitData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }

            string[] title = { "编号", "部门", "部门" };
            int[] width = { 60, 200, 200 };
            this.content = GetContent(nc.GetDataTable("tbl"), null);

            AddNotify(new MailNotify());

        }


    }
}
