using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class GuestComplaints:NotificationContent
    {
        public GuestComplaints() { 
        }

        protected override void Init()
        {
            base.Init();
            nc = new GuestComplaintsConfig(DBServerType.MSSQL, "CRM", this.ToString());
            nc.InitData();
            nc.ConfigData();
            //string[] title = { "结案日期", "案件编号", "客户代号", "客户简称", "问题描述", "产品别", "区域别", "客诉类别", "收费否", "责任判定", "结案码", "是否客诉", "原因分析说明", "接案日期", "是否在原厂保固期" };
            string[] title = { "责任部门", "区域别", "产品别", "案件编号", "客户代号", "客户简称", "产品序号", "客诉类别", "责任判定", "是否客诉", "收费否", "是否在原厂保固期", "问题描述", "接案日期", "原因分析说明","结案码", "结案日期" };
            int[] width = { 100, 120, 120, 120, 80, 80, 80, 80, 80, 80, 80, 80, 100, 80, 80,80,80 };
            this.content = GetContent(nc.GetDataTable("tlbGuest"), title, width);
            if (nc.GetDataTable("tlbGuest").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
