using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using CrystalDecisions.Shared;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderRD : NotificationContent
    {
        public SpecOrderRD()
        {
        }

        protected override void Init()
        {
            SetMailHead();

            nc = new SpecOrderRDConfig(DBServerType.SybaseASE, "SHBERP",this.ToString());
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

    }
}
