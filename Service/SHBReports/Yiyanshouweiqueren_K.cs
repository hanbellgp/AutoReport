using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Yiyanshouweiqueren_K : NotificationContent
    {
        public Yiyanshouweiqueren_K() { 
        
        }
        protected override void Init()
        {
            base.Init();

            nc = new YiyanshouweiquerenConfig_K(Core.DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("tblresult"), null);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
               // AddNotify(new MailNotify());
                AddNotify(new MailNotify());
            }
        }
    }
}
