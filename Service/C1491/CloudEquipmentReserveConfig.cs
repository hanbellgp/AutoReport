using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Globalization;
using Hanbell.AutoReport.Config;

namespace C1491
{
    public class CloudEquipmentReserveConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CloudEquipmentReserveConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCloudEquipmentReserve();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {


            string sqlstr = @" SELECT A.itnbr, A.pm, A.wareh, A.whdsc, sum(A.onhand1) as onhand1
                            FROM (select a.itnbr,(select itdsc from invmas where invmas.itnbr=a.itnbr) as pm, a.wareh ,b.whdsc, a.onhand1  from invbal a
                                left join invwh b on a.wareh = b.wareh
                                where a.itnbr in ('4806-GFP9B-05','4806-GFP9B-06','4806-GFH12-01','4806-GFP41-01','4806-GFP41-06',
                                '4806-GFP41-08','4806-GFP41-09','4806-GFP41-10','4806-GFP42-01','4806-GFP42-06','4806-GFP42-08','4806-GFP42-09',
                                '4806-GFP42-10','4806-GFP43-01','4806-GFP43-06','4806-GFP43-08','4806-GFP43-09','4806-GFP43-10','4499-128',
                                '4499-129','4499-130','4806-493')
                                and b.whdsc not LIKE  N'借客户%') A GROUP BY A.itnbr, A.pm, A.wareh, A.whdsc";

            Fill(sqlstr, ds, "tbresult");


        }


       
    }
}
