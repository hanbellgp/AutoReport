using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class CDR_R_YwyMxAll : NotificationContent
    {

        public CDR_R_YwyMxAll()
        {

        }

        protected override void Init()
        {
            base.Init();

            //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc = new CDR_R_YwyMxAllConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            string[] title = { "分类" };
            int[] width = { 50 };

            //string[] title = { };
            this.content = GetContent(nc.GetDataTable("CDR_R_YwyMxAll"), title, width);


            AddNotify(new MailNotify());

        }



    }
}
