using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class ServiceInventoryReportConfig:NotificationConfig
    {
        public ServiceInventoryReportConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ServiceInventoryReportDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            //铸件
            sb.Append(" select b.itnbr,s.itdsc,s.spdsc,s.unmsr1,b.onhand1,b.wareh,(p.unitavgcst*b.onhand1) as cst  ");
            sb.Append(" from invbal b  ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr ");
            sb.Append(" LEFT JOIN invpri p on b.facno = p.facno and b.itnbr = p.itnbr ");
            sb.Append(" where b.wareh in ('EKF02','EAKF02') ");
            sb.Append(" and b.itcls in ('3202','3802','3015','3111','3013') ");
            sb.Append(" and b.onhand1>0 ");
            sb.Append(" and p.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            Fill(sb.ToString(), ds, "zjtbl");
            sb.Length = 0;
            //电机
            sb.Append(" select b.itnbr,s.itdsc,s.spdsc,s.unmsr1,b.onhand1,b.wareh,(p.unitavgcst*b.onhand1) as cst  ");
            sb.Append(" from invbal b  ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr ");
            sb.Append(" LEFT JOIN invpri p on b.facno = p.facno and b.itnbr = p.itnbr ");
            sb.Append(" where b.wareh in ('EKF02','EAKF02') ");
            sb.Append(" and b.itcls in ('3014','3104') ");
            sb.Append(" and b.onhand1>0 ");
            sb.Append(" and p.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            Fill(sb.ToString(), ds, "djtbl");
            sb.Length = 0;
            //轴承
            sb.Append(" select b.itnbr,s.itdsc,s.spdsc,s.unmsr1,b.onhand1,b.wareh,(p.unitavgcst*b.onhand1) as cst  ");
            sb.Append(" from invbal b  ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr ");
            sb.Append(" LEFT JOIN invpri p on b.facno = p.facno and b.itnbr = p.itnbr ");
            sb.Append(" where b.wareh in ('EKF02','EAKF02') ");
            sb.Append(" and b.itcls in ('4009') ");
            sb.Append(" and b.onhand1>0 ");
            sb.Append(" and p.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            Fill(sb.ToString(), ds, "zctbl");
            sb.Length = 0;
            //油品
            sb.Append(" select b.itnbr,s.itdsc,s.spdsc,s.unmsr1,b.onhand1,b.wareh,(p.unitavgcst*b.onhand1) as cst  ");
            sb.Append(" from invbal b  ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr ");
            sb.Append(" LEFT JOIN invpri p on b.facno = p.facno and b.itnbr = p.itnbr ");
            sb.Append(" where b.wareh in ('EKF02','EAKF02') ");
            sb.Append(" and b.itcls in ('5062','5061','9017') ");
            sb.Append(" and b.onhand1>0 ");
            sb.Append(" and p.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            Fill(sb.ToString(), ds, "yptbl");
            sb.Length = 0;

        }

    }
}
