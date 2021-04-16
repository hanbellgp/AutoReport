using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Qinggousantianweipaozhuan_V : NotificationContent
    {
        public Qinggousantianweipaozhuan_V()
        {
        }
        protected override void Init()
        {
            base.Init();
            nc = new QinggousantianweipaozhuanConfig_V(Core.DBServerType.SybaseASE, "VHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("tblprocessv"), null);

            if (nc.GetDataTable("tblprocessv").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
