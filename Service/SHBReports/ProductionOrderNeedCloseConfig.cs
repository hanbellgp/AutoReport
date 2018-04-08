using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ProductionOrderNeedCloseConfig : NotificationConfig
    {

        public ProductionOrderNeedCloseConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ProductionOrderNeedCloseDS();
            this.reportList.Add(new ProductionOrderNeedCloseReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            //StringBuilder sb = new StringBuilder();

            //sb.Append("SELECT  manno ,'' AS pono , itnbrf ,");
            //sb.Append("itdsc = ( SELECT  itdsc FROM  invmas WHERE  invmas.itnbr = manmas.itnbrf ) ,");
            //sb.Append("vdrno ,keyindate ,mandate ,manqty ,finqty ,issqty ,");
            //sb.Append("modqty = ISNULL(( SELECT SUM(modqty) FROM  manbor WHERE manbor.facno = manmas.facno AND manbor.prono = manmas.prono AND manbor.manno = manmas.manno ),0) ,");
            //sb.Append("0 AS accqy1 ,0 AS okqy1 FROM    manmas WHERE   manstatus IN ( 'A', 'B' )");
            //sb.Append("AND manqty - finqty - (SELECT SUM(modqty) FROM manbor WHERE  manbor.facno = manmas.facno AND manbor.prono = manmas.prono  AND manbor.manno = manmas.manno) = 0 ");
            //sb.Append("AND typecode = '01' AND DATEDIFF(DAY, manmas.keyindate, GETDATE()) >= 30 AND issqty = 0 ");
            //sb.Append(" UNION ");
            //sb.Append("SELECT  manno ,'' AS pono , itnbrf ,");
            //sb.Append("itdsc = ( SELECT  itdsc FROM  invmas WHERE  invmas.itnbr = manmas.itnbrf ) ,");
            //sb.Append("vdrno ,keyindate ,mandate ,manqty ,finqty ,issqty ,");
            //sb.Append("modqty = ISNULL(( SELECT SUM(modqty) FROM  manbor WHERE manbor.facno = manmas.facno AND manbor.prono = manmas.prono AND manbor.manno = manmas.manno ),0) ,");
            //sb.Append("0 AS accqy1 ,0 AS okqy1 FROM  manmas WHERE   manstatus < 'I' AND manstatus > 'D' ");
            //sb.Append(" AND (SELECT SUM(wipqty) FROM  manbor WHERE manbor.facno = manmas.facno AND manbor.prono = manmas.prono  AND manbor.manno = manmas.manno ) = 0 ");
            //sb.Append(" AND (SELECT ISNULL(SUM(CASE iocode WHEN '1' THEN trnqy1  WHEN '2' THEN (0 - trnqy1) END),0) FROM  mansfctrn WHERE  mansfctrn.facno = manmas.facno ");
            //sb.Append(" AND mansfctrn.manno = manmas.manno AND trtype IN ( '600', '700' ) ) = 0 ");
            //sb.Append(" UNION ");
            //sb.Append("SELECT asspurhad.manno ,purhad.pono ,itnbrf ,itdsc = ( SELECT itdsc FROM invmas WHERE invmas.itnbr = manmas.itnbrf) ,");
            //sb.Append(" purhad.vdrno ,  keyindate , mandate , manqty , finqty ,issqty ,");
            //sb.Append("modqty = ISNULL(( SELECT SUM(modqty) FROM  manbor WHERE manbor.facno = manmas.facno AND manbor.prono = manmas.prono AND manbor.manno = manmas.manno ),0) ,");
            //sb.Append("purdta.accqy1 ,purdta.okqy1 FROM    purhad , purdta ,asspurhad ,manmas ");
            //sb.Append(" WHERE   ( purdta.facno = purhad.facno ) AND ( purdta.pono = purhad.pono ) AND ( purhad.prono = purdta.prono ) AND manmas.manno = asspurhad.manno ");
            //sb.Append(" AND purhad.pono = asspurhad.pono AND purhad.pono LIKE 'AC%'  AND poqy1 - accqy1 = 0 AND purdta.dposta < '95' AND DATEDIFF(DAY, manmas.keyindate, GETDATE()) >= 30 ");

            string sqlstr = @"SELECT asspurhad.manno ,purhad.pono ,itnbrf ,itdsc = ( SELECT itdsc FROM invmas WHERE invmas.itnbr = manmas.itnbrf) ,
             purhad.vdrno ,  keyindate , mandate , manqty , finqty ,issqty ,
            modqty = ISNULL(( SELECT SUM(modqty) FROM  manbor WHERE manbor.facno = manmas.facno AND manbor.prono = manmas.prono AND manbor.manno = manmas.manno ),0) ,
            purdta.accqy1 ,purdta.okqy1 FROM    purhad , purdta ,asspurhad ,manmas
             WHERE   ( purdta.facno = purhad.facno ) AND ( purdta.pono = purhad.pono ) AND ( purhad.prono = purdta.prono ) AND manmas.manno = asspurhad.manno
            AND purhad.pono = asspurhad.pono AND purhad.pono LIKE 'AC%'  AND poqy1 - accqy1 = 0 AND purdta.dposta < '95' AND DATEDIFF(DAY, manmas.keyindate, GETDATE()) >= 30";

            //Fill(sb.ToString(), ds, "tblresult");
            Fill(sqlstr, ds, "tblresult");

        }

    }
}
