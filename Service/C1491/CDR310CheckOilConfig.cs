using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace C1491
{
    class CDR310CheckOilConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CDR310CheckOilConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CDR310CheckOilDS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {

            string sqlstr1 = @"SELECT  DD.cdrno+'_'+Convert(varchar(4),DD.trseq) AS 'NO',DD.cdrno,DD.cfmuserno,DD.cusna,DD.trseq,DD.itnbr,DD.itnbrcus,AA.specifitesc 
                                FROM ( select A.facno , A.cdrno ,A.ctrseq, A.sfktrno,B.parts ,B.specifitesc from cdrhpopsfk A 
                                left join cdrdpopsfk B on A.sfktrno=B.sfktrno   AND A.facno=B.facno where B.parts='LD' ) AA 
                                inner JOIN (select h.facno ,h.cdrno,h.cfmuserno,cs.cusna ,d.trseq,d.itnbr,d.itnbrcus 
                                FROM cdrhmas h LEFT JOIN cdrcus cs ON cs.cusno  = h.cusno, cdrdmas d  WHERE  
                                h.facno  = d.facno  and h. cdrno =d.cdrno 
                                and   h. indate >= LEFT(CONVERT(CHAR(8), GETDATE(),112),8)  and h.indate < LEFT(CONVERT(CHAR(8),  dateadd(day, 1, getdate()) ,112),8) 
                                 )DD ON AA.facno = DD.facno   AND  AA.cdrno  = DD.cdrno and AA.ctrseq = DD.trseq 
                                 order by DD.cdrno ,DD.trseq ";

            //Fill(sqlstr, ds, "cdrsfkoil");

            var table1 = GetQueryTable(sqlstr1);

            string sqlstr2 = @"select sb.cdrno+'_'+Convert(varchar(4),sb.trseq) AS 'NO', sb.* from cdrbomsub sb inner JOIN ( 
                            select h.facno ,h.cdrno,h.cfmuserno,cs.cusna ,d.trseq,d.itnbr,d.itnbrcus 
                            FROM cdrhmas h LEFT JOIN cdrcus cs ON cs.cusno  = h.cusno , cdrdmas d  WHERE  h.facno  = d.facno  and h. cdrno =d.cdrno 
                             and   h.indate >= LEFT(CONVERT(CHAR(8), GETDATE(),112),8)  and h.indate < LEFT(CONVERT(CHAR(8),  dateadd(day, 1, getdate()) ,112),8) 
                         )DD ON sb.facno = DD.facno   AND  sb.cdrno  = DD.cdrno and sb.trseq = DD.trseq order by DD.cdrno ,DD.trseq ";

            var table2 = GetQueryTable(sqlstr2);

            foreach (DataRow tb1row in table1.Rows)
            {
                var key = tb1row["NO"].ToString();
                var tb1yp = tb1row["specifitesc"].ToString().Trim();
                var sltrow = table2.Select(string.Format("NO = '{0}'", key));
                if (sltrow != null)
                {
                    if (sltrow.Length > 0)
                    {
                        DataRow slted = sltrow.FirstOrDefault();
                        string ti = this.GetYP(slted);
                        if (!string.IsNullOrEmpty(ti))
                        {
                            string tbRow2yp = slted["itsdesc" + ti].ToString();
                            if (!tbRow2yp.Equals(tb1yp))
                            {
                                //往新表插入
                                DataRow addrow = ds.Tables["tbresult"].NewRow();
                                addrow["cdrno"] = tb1row["cdrno"];
                                addrow["cfmuserno"] = tb1row["cfmuserno"];
                                addrow["ctrseq"] = tb1row["trseq"];
                                addrow["itnbr"] = tb1row["itnbr"];
                                addrow["itnbrcus"] = tb1row["itnbrcus"];
                                addrow["cusna"] = tb1row["cusna"];
                                addrow["oilsub"] = slted["itnbrs" + ti].ToString();
                                addrow["oilsfk"] = tb1yp;
                                ds.Tables["tbresult"].Rows.Add(addrow);

                            }
                        }
                        table2.Rows.Remove(sltrow.FirstOrDefault());
                    }
                }
            }

        }

        private string GetYP(DataRow dataRow)
        {
            for (int i = 1; i <= 25; i++)
            {
                var name = dataRow["itdesc" + i].ToString().Trim();
                if (name.IndexOf("油品") > -1)
                {
                    return i.ToString();
                }
            }
            return string.Empty;
        }

    }
}


