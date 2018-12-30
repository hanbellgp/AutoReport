using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Yiyanshouweiqueren : NotificationContent
    {
        public Yiyanshouweiqueren()
        { 
        }

        protected override void Init()
        {
            base.Init();

            nc = new YiyanshouweiquerenConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("tblresult"), null);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
