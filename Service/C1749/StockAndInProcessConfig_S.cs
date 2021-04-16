using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class StockAndInProcessConfig_S:NotificationConfig
    {
        public StockAndInProcessConfig_S(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new StockAndInProcessDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select cdrdmmodel.cmcmodel, a.itnbr, ");
            sb.Append(" isnull(sum(case a.wareh when 'FSFL' then onhand1 end),0) as 'FSFL', ");
            sb.Append(" isnull(sum(case a.wareh when 'FSSC' then onhand1 end),0) as 'FSSC', ");
            sb.Append(" isnull(sum(case a.wareh when 'FSJY' then onhand1 end),0) as 'FSJY', ");
            sb.Append(" isnull(sum(case a.wareh when 'EW01' then onhand1 end),0) as 'EW01', ");
            sb.Append(" isnull(sum(onhand1),0) as total ");
            sb.Append(" from ( ");
            sb.Append(" select invbat.itnbr,wareh,fixnr,sum(onhand1) as onhand1 from invbat,invmas ");
            sb.Append(" where invbat.itnbr=invmas.itnbr ");
            sb.Append(" and invmas.itcls IN('3879','3979','3179','3886','3976','3176','3890','3180','3980') ");
            sb.Append(" and (invbat.onhand1-invbat.preqy1<>0) and wareh = 'EW01' ");
            sb.Append(" AND invbat.itnbr LIKE '39%' ");
            sb.Append(" group by invbat.itnbr,wareh,fixnr ");
            sb.Append(" union all ");
            sb.Append(" select manwipbat.itnbr,manwipbat.prosscode as wareh,manwipbat.fixnr,sum(onhand1) as onhand1  from manwipbat ");
            sb.Append(" left join borprc on borprc.prosscode=manwipbat.prosscode, ");
            sb.Append(" invmas,manmas where manwipbat.itnbr=invmas.itnbr and ");
            sb.Append(" manwipbat.facno=manmas.facno and manwipbat.manno=manmas.manno ");
            sb.Append(" and invmas.itcls IN('3879','3979','3179','3886','3976','3176','3890','3180','3980') ");
            sb.Append(" AND manwipbat.itnbr LIKE '39%' ");
            sb.Append(" and onhand1<>0 and linecode='SJ' ");
            sb.Append(" group by manwipbat.itnbr,manwipbat.prosscode,manwipbat.fixnr ");
            sb.Append(" ) a left join cdrdmmodel on cdrdmmodel.itnbr=a.itnbr ");
            sb.Append(" GROUP BY cdrdmmodel.cmcmodel, a.itnbr ");
            //<-- 20191227增加代理品
            Fill(sb.ToString(), ds, "tbl");
        }
    }
}
