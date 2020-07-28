using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class ERPPUR530Config : Hanbell.AutoReport.Config.NotificationConfig
    {
        private DBServerType dBServerType;
        private string p;

        public ERPPUR530Config()
        {
        }

        public ERPPUR530Config(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ERPPUR530DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select distinct  a.acceptno ,a.pono ,m.srcno ,a.vdrno ,c.vdrna ,getdate() as 'dqrq',a.acceptdate ,");
            sb.Append("a.itnbr ,i.itdsc ,a.accqy1+a.accqy2 as 'dssl',purhask.userno ,secuser.username, purhask.depno ,miscode.cdesc,datediff(day,a.acceptdate,GETDATE()) as 'ywts' ");
            sb.Append("from puracd  a left join purhad h on a.facno = h.facno and a.prono = h.prono and a.pono = h.pono ");
            sb.Append("left join purdtamap m on a.pono=m.pono and a.ponotrseq = m.trseq ");
            sb.Append("right join purvdr c on a.vdrno=c.vdrno right join invmas i on a.itnbr=i.itnbr ");
            sb.Append("right join purhask  on  purhask.prno = m.srcno ");
            sb.Append("left join secuser on secuser.userno = purhask.userno ");
            sb.Append("left join miscode on miscode.code = purhask.depno ");
            sb.Append("where a.accsta <> 'W' and a.facno='C' and a.prono = '1' and a.accsta = 'R' ");
            sb.Append("and a.itnbr like '%-GB%' ");
            sb.Append("and h.posrc <> '5' ");
            sb.Append("order by purhask.depno,purhask.userno ,datediff(day,a.acceptdate,GETDATE()) ");
            Fill(String.Format(sb.ToString()), this.ds, "tbresult");

        }
    }
}
