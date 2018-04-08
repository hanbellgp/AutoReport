using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class SpecialEquipmentConfig : NotificationConfig
    {
        public SpecialEquipmentConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new SpecialEquipmentDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            String sqlStr = @"select distinct a.vdrno as vdrno ,c.vdrna as vdrna,a.acceptdate as acceptdate,a.itnbr as itnbr,i.itdsc as itdsc,a.accqy1+a.accqy2 as 'dssl' 
                            from puracd  a left join purhad h on a.facno = h.facno and a.prono = h.prono and a.pono = h.pono 
                            left join purdtamap m on a.pono=m.pono and a.ponotrseq = m.trseq 
                            right join purvdr c on a.vdrno=c.vdrno right join invmas i on a.itnbr=i.itnbr 
                            right join purhask  on  purhask.prno = m.srcno 
                            left join secuser on secuser.userno = purhask.userno 
                            left join miscode on miscode.code = purhask.depno 
                            where a.accsta = 'R' and  a.facno='C' and a.prono = '1' and 
                            a.itnbr in ('A310-01','A310-02','A310-06','A310-09','A311-04')  
                            and h.posrc <> '5' 
                            order by datediff(day,a.acceptdate,GETDATE())";
            Fill(sqlStr, ds, "tlbequipment");
        }
    }
}
