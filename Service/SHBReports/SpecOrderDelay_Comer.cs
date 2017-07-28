using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderDelay_Comer: SpecOrderNotification
    {

        public SpecOrderDelay_Comer()
        {
        }

        protected override void Init()
        {
            SetMailHead();

            nc = new SpecOrderDelayConfig(DBServerType.SybaseASE, "ComerERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }
            string[] title = { "编号", "项目", "标注", "计划日期", "实际日期", "延迟", "单号", "序号", "内容", "数量", "已验收", "部门", "负责人", "计划日期", "实际完成", "延迟" };
            int[] width = { 80, 200, 50, 70, 70, 50, 80, 45, 200, 50, 50, 50, 60, 70, 70, 50 };
            this.content = GetContent(nc.GetDataTable("tblresults"), title, width);

            if (nc.GetDataTable("tblresults").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

    }
}
