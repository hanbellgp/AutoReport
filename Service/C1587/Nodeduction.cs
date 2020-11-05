using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;


namespace C1587
{
    public class Nodeduction : Hanbell.AutoReport.Config.NotificationContent
    {

        public Nodeduction()
        {
        }

        protected override void Init()
        {

            base.Init();
            nc = new NodeductionConfig(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            string[] title = { "出货日期", "出货单号", "件号", "件号名称", "出货数量" };
            int[] width = { 150, 200, 150, 150,150 };
            DataTable dt = nc.GetDataTable("tblresult");
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);
            if (dt.Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }


    }
}
