using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace KNB.config
{
    public class DianshouweiyanshouConfig : NotificationConfig
    {
        public DianshouweiyanshouConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DianshouweiyanshouDS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select a.acceptno,a.vdrno,c.vdrna,i.itcls,a.itnbr,i.itdsc, ");
            sb.Append(" (a.accqy1+a.accqy2) as 'accqy',purach.indate,datediff(hour,purach.indate,GETDATE()) as 'delaytime','' as 'remark' ");
            sb.Append(" from purach,puracd  a ");
            sb.Append(" left join purhad h on a.facno = h.facno and a.prono = h.prono and a.pono = h.pono  ");
            sb.Append(" left join purdtamap m on a.pono=m.pono and a.trseq=m.trseq ");
            sb.Append(" right join purvdr c on a.vdrno=c.vdrno right join invmas i on a.itnbr=i.itnbr ");
            sb.Append(" where purach.acceptno=a.acceptno and purach.facno=a.facno and a.prono = '1' ");
            sb.Append(" and a.accsta <> 'W' and a.accsta = 'R'  and h.posrc <> '5' ");
            sb.Append(" union all ");
            sb.Append(" select puracd.acceptno,puracd.vdrno,purvdr.vdrna,invmas.itcls,invmas.itnbr,invmas.itdsc, ");
            sb.Append(" (puracd.accqy1+puracd.accqy2) as 'accqy',purach.indate,datediff(hour,purach.indate,GETDATE()) as 'delaytime','' ");
            sb.Append(" FROM purach,puracd ");
            sb.Append(" left join invmas on invmas.itnbr=puracd.itnbr ");
            sb.Append(" right join purvdr on puracd.vdrno=purvdr.vdrno,asspurhad ");
            sb.Append(" WHERE purach.facno = puracd.facno  and  purach.prono = puracd.prono and  purach.acceptno = puracd.acceptno and ");
            sb.Append(" puracd.accsta='R'  AND puracd.facno = asspurhad.facno and puracd.prono = asspurhad.prono and  puracd.pono = asspurhad.pono ");
            sb.Append(" AND  Convert(varchar(6),purach.acceptdate,112) = Convert(varchar(6),getdate(),112) ");

            Fill(sb.ToString(), ds, "tbl");
        }
    }
}
