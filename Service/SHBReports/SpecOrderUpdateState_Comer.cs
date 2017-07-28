using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderUpdateState_Comer: NotificationContent
    {
        public SpecOrderUpdateState_Comer()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new SpecOrderUpdateStateConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();

            this.content = this.GetContentHead() + "<br/><br/><br/>" + this.GetContentFooter() ;

            AddNotify(new MailNotify());

        }

    }
}
