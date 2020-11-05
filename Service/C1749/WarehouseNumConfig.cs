using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WarehouseNumConfig:NotificationConfig
    {
        public WarehouseNumConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new WarehouseNumDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            //占用储位
            StringBuilder zysb = new StringBuilder();
            zysb.Append(" select c.wareh ,count(c.loc) as num  ");
            zysb.Append(" from ( select top 50 a.loc,b.wareh from asrs_tb_loc_mst a,asrs_tb_loc_dtl b ");
            zysb.Append(" where a.loc_sts in ('S') and a.loc<'090101' and a.loc=b.loc ");
            zysb.Append(" group by a.loc,b.wareh) c ");
            zysb.Append(" group by c.wareh ");
            Fill(zysb.ToString(), ds, "zytlb");

            //空余储位
            StringBuilder kysb = new StringBuilder();
            kysb.Append(" select linecode ,count(linecode) as num from ( ");
            kysb.Append(" select (CASE convert(VARCHAR(2),loc,2) ");
            kysb.Append(" when '01' then '1号线' when '02' then '1号线' ");
            kysb.Append(" when '03' then '2号线' when '04' then '2号线' ");
            kysb.Append(" when '05' then '3号线' when '06' then '3号线' ");
            kysb.Append(" when '07' then '4号线' when '08' then '4号线' ");
            kysb.Append(" end) as linecode from asrs_tb_loc_mst ");
            kysb.Append(" where loc_sts ='N' and loc<'090101' ) as a GROUP BY linecode ");
            Fill(kysb.ToString(), ds, "kytlb");
        }
    }
}
