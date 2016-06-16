using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderUpdateState : NotificationContent
    {
        public SpecOrderUpdateState()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new SpecOrderUpdateStateConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();

            this.content = this.GetContentHead() + "<br/><br/><br/>" + this.GetContentFooter() ;

            AddNotify(new MailNotify());

        }

    }
}
