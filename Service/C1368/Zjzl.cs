using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Zjzl : NotificationContent
    {

        public Zjzl()
        {

        }

        protected override void Init()
        {
            base.Init();

            //nc = new ZjzlConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc = new ZjzlConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }

            string[] title = { "产品别", "呆滞一年以内", "呆滞一年以上", "合计",};
            int[] width = { 150, 150, 150, 150};

            //string[] title = { };
            //this.content = GetContent(nc.GetDataTable("tblresult"), title, width);
            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();


            AddNotify(new MailNotify());

        }



    }
}
