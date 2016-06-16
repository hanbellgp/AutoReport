using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanbell.AutoReport.Config
{
    class AccountReceivableDelayForSaler:AccountReceivableDelay
    {

        public AccountReceivableDelayForSaler()
        {
        }

        protected override void Init()
        {
            SetMailHead();

            nc = new AccountReceivableDelayForSalerConfig(Core.DBServerType.SybaseASE, "SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();
        }



    }
}
