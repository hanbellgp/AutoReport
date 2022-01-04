using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WeiweihuchangshangfenpeilvdangConfig_K : NotificationConfig
    {
        public WeiweihuchangshangfenpeilvdangConfig_K()
        { 
        }
        public WeiweihuchangshangfenpeilvdangConfig_K(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new Weiweihuchangshangfenpeilvdang_KDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select purvdr.vdrna, d.vdrno,d.buyer,username, d.facno,d.prono,d.pono,a.trseq,a.itnbr,invmas.itdsc ");
            sb.Append("from purhad d,purdta a ,secuser ,purvdr,invmas ");
            sb.Append("where d.buyer=secuser.userno and  d.facno=a.facno and d.prono=a.prono and d.pono=a.pono  ");
            sb.Append("and invmas.itcls not in ( '1014','2013','2015','9014','3016','3015','3013','3716','4079') ");
            sb.Append("and invmas.itclscode not in ('7','8','B') ");
            sb.Append("and d.indate >=dateadd(dd,-day(dateadd(month,-1,getdate()))+1,dateadd(month,-1,getdate()))  ");
            sb.Append("and d.indate<= dateadd(dd,-day(getdate()),getdate())  and purvdr.vdrno=d.vdrno and invmas.itnbr=a.itnbr   ");
            sb.Append("and a.itnbr < > '9' and ((a.itnbr not in (select purdis.itnbr from purdis where purdis.vdrno=d.vdrno)) ");
            sb.Append("or (d.vdrno not in (select purdis.vdrno from purdis where purdis.itnbr=a.itnbr)) ");
            sb.Append("or (a.itnbr not in (select purdis.itnbr from purdis  ) )) and d.pono not like 'AC%' and a.itnbr not in ((a.itnbr LIKE '%-9QC%' OR ( len(a.itnbr) >= 14 AND (substring(a.itnbr, 1, 14) LIKE '%-C1%' OR substring(a.itnbr, 1, 14) LIKE '%-C2%') AND len(substring(a.itnbr, 1, 14)) - len(STR_REPLACE(substring(a.itnbr, 1, 14), '-', NULL)) = 1))");

           
            Fill(sb.ToString() ,ds, "tblresult5");
        }
    }
}
