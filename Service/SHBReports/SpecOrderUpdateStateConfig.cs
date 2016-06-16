using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderUpdateStateConfig : SpecOrderNotificationConfig
    {

        public SpecOrderUpdateStateConfig()
        {
        }

        public SpecOrderUpdateStateConfig(DBServerType dbType, string connName, string notification)
            : base(dbType, connName, notification)
        {

        }

        public override void InitData()
        {
            ResetSpecialOrderState();
            ResetSpecialOrderDetailState();
        }
    }
}
