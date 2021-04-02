using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Meiyuexinzengkuhao : NotificationContent
    {
        public Meiyuexinzengkuhao()
        {
        }
        protected override void Init()
        {
            base.Init();
            nc = new MeiyuexinzengkuhaoConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("warehtable"), null);

            if (nc.GetDataTable("warehtable").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

    }
}
