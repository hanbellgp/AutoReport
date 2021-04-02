using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Qinggousantianweipaozhuan_M3_ZJKM:NotificationContent
    {
        public Qinggousantianweipaozhuan_M3_ZJKM() { }
        protected override void Init()
        {
            base.Init();
            nc = new QinggousantianweipaozhuanConfig_M3(Core.DBServerType.SybaseASE, "ZjComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("tblprocess"), null);

            if (nc.GetDataTable("tblprocess").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
