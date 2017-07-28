using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ERPMonthClose_VN:NotificationContent
    {

        public ERPMonthClose_VN()
        {
        }


        protected override void Init()
        {
            base.Init();

            nc = new ERPMonthCloseConfig_VN(Core.DBServerType.SybaseASE, "VHBERP", this.ToString());

            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            string[] title = { "单据编号", "日期", "使用人", "姓名", "类别", 
                };
            int[] width = { 100, 100, 100, 100, 150 };

            //string[] title = { };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }

    }
}
