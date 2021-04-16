using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class ERPPURX460Config:NotificationConfig
    {
        public ERPPURX460Config(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ERPPURX460DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct purdta.facno , purdta.prono , purdta.pono ,purdta.trseq , purdta.itnbr , invmas.itdsc, purdta.poqy1 ,");
            sb.Append("purdta.askdate ,purdta.askdateo ,purdta.okqy1 ,( purdta.poqy1-purdta.okqy1 ) as 'wjh', ");
            sb.Append("(purdta.accqy1-purdta.okqy1 -purdta.badqy1-purdta.stqy1) as 'ydwy',purhask.userno  ,purhad.buyer ");
            sb.Append("FROM purdta ,purhad ,invmas ,purdtamap ,purhask ");
            sb.Append("WHERE ( purhad.facno = purdta.facno ) and ( purhad.pono = purdta.pono and purdta.pono = purdtamap.pono and purdta.trseq = purdtamap.trseq ) and ( ( purhad.posrc <> '5' )) ");
            sb.Append("and purdta.itnbr =invmas.itnbr and ( purdta.poqy1-purdta.okqy1 ) > 0 and  purdta.askdate >= CONVERT(CHAR(8), dateadd(month,-3,getdate()),112) ");
            sb.Append("and  (purdta.askdate <= getdate()) ");
            sb.Append("and purhask.prno =purdtamap.srcno ");
            sb.Append("and purdta.dposta not in ('95','98','99') ");
            sb.Append("order by  purdta.facno , purdta.prono , purdta.askdate ,  purdta.askdateo ");
            Fill(String.Format(sb.ToString()), this.ds, "tbresult");

        }
    }
}
