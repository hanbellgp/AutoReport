using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ProductiveTurnoverConfig:NotificationConfig
    {
        public ProductiveTurnoverConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new ProductiveTurnoverDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        /**增加3个参数 facno,wareh,itcls*/
        public override void InitData()
        {
            string wareh = args["wareh"].ToString();
            string itcls1 = args["itcls1"].ToString();
            string itcls2 = args["itcls2"].ToString();
            string facno = args["facno"].ToString();


            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT a.itcls,a.clsdsc,a.itnbr,a.itdsc,a.genre1,a.wareh,a.trnqys,a.onhand1,trnqyh3,trnqyh2,trnqyh1, ");
            sb.Append(" round((isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0) )/3,2) AS 'avgtrnqys', ");
            sb.Append(" (CASE WHEN (isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0))>0 ");
            sb.Append(" THEN convert(decimal(18,2),round(a.onhand1/round((isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0))/3,2),2)*30) else 0 END) AS 'zzts', ");
            sb.Append(" p.vdrna,cgwj,isnull(p.leadtime,0) as leadtime,'' AS 'cy',p.pominqy,q.safqy,q.maxqy,a.lindate,a.abcclass ");
            sb.Append(" FROM ( ");
            sb.Append(" SELECT a.facno,a.prono,a.itcls,s.clsdsc, a.itnbr,m.itdsc,m.genre1,a.wareh, a.trnqys,b.onhand1,b.lindate,m.abcclass ");
            sb.Append(" FROM invmon a ");
            sb.Append(" LEFT JOIN invmas m ON a.itnbr = m.itnbr  ");
            sb.Append(" LEFT JOIN invcls s ON s.itcls = a.itcls AND s.itcls = m.itcls ");
            sb.Append(" LEFT JOIN invbal b ON a.facno = b.facno AND a.prono = b.prono AND a.wareh = b.wareh AND a.itnbr = b.itnbr ");
            //<--取库存的入库的最大日期
            //sb.Append(" LEFT JOIN (select facno,prono,itnbr,onhand1,lindate from invbal a where facno = 'C' and prono = '1'  ");
            //sb.Append(" and lindate = (select max(lindate) from invbal b where a.facno = b.facno and a.prono = b.prono and a.itnbr = b.itnbr)) b ");
            //sb.Append(" ON a.facno = b.facno AND a.prono = b.prono AND a.itnbr = b.itnbr   ");
            //-->
            sb.Append(" WHERE  a.facno =  ").Append(facno);
            sb.Append(" AND a.prono = '1' AND a.trnqys >0 ");
            sb.Append(" AND a.wareh IN  ").Append(wareh);
            sb.Append(" AND a.itcls IN  ").Append(itcls1);
            sb.Append(" and a.iocode='Z'  ");
            sb.Append(" AND (m.jityn <> 'J' AND m.jityn <> 'P')  ");
            sb.Append(" AND a.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            sb.Append(" ) a ");
            sb.Append(" LEFT JOIN ( select v.vdrna,dt.itnbr,dt.leadtime,dt.pominqy from  purdis ds , purvdrit dt LEFT JOIN purvdr v ON dt.vdrno = v.vdrno ");
            sb.Append(" where ds.mainyn='Y' ");
            sb.Append(" and ds.vdrno=dt.vdrno and ds.itnbr=dt.itnbr ) p ON a.itnbr = p.itnbr  ");
            sb.Append(" left join invsafqy q on a.itnbr =q.itnbr AND a.facno = q.facno AND a.prono = q.prono ");
            sb.Append(" LEFT JOIN (SELECT facno,prono,itnbr,isnull(sum(( poqy1  -  okqy1 )),0) AS 'cgwj'  ");
            sb.Append(" FROM purdta WHERE facno = ").Append(facno);
            sb.Append(" AND prono = '1'  AND ( poqy1  -  okqy1 )>0 and dposta not in ('98','99') ");
            sb.Append(" GROUP BY facno,prono,itnbr) m ON a.itnbr = m.itnbr AND a.facno = m.facno AND a.prono = m.prono  ");
            sb.Append(" LEFT JOIN   ( ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh1 FROM invmon h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and d.syscode IN ('10') ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h1 ON a.facno = h1.facno AND a.prono = h1.prono AND a.itnbr = h1.itnbr ");
            sb.Append(" LEFT JOIN   ( ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh2 FROM invmon h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and d.syscode IN ('10') ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-2,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h2 ON a.facno = h2.facno AND a.prono = h2.prono AND a.itnbr = h2.itnbr ");
            sb.Append(" LEFT JOIN   ( ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh3 FROM invmonh h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND  ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and d.syscode IN ('10') ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-3,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h3 ON a.facno = h3.facno AND a.prono = h3.prono AND a.itnbr = h3.itnbr ");
            sb.Append(" UNION ALL  ");

            sb.Append(" SELECT a.itcls,a.clsdsc,a.itnbr,a.itdsc,a.genre1,a.wareh,a.trnqys,a.onhand1,trnqyh3,trnqyh2,trnqyh1, ");
            sb.Append(" round((isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0) )/3,2) AS 'avgtrnqys', ");
            sb.Append(" (CASE WHEN (isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0))>0 ");
            sb.Append(" THEN convert(decimal(18,2),round(a.onhand1/round((isnull(trnqyh3,0) + isnull(trnqyh2,0) + isnull(trnqyh1,0))/3,2),2)*30) else 0 END) AS 'zzts', ");
            sb.Append(" p.vdrna,cgwj,isnull(p.leadtime,0) as leadtime,'' AS 'cy',p.pominqy,q.safqy,q.maxqy,a.lindate,a.abcclass ");
            sb.Append(" FROM ( ");
            sb.Append(" SELECT a.facno,a.prono,a.itcls,s.clsdsc, a.itnbr,m.itdsc,m.genre1,a.wareh, a.trnqys,b.onhand1,b.lindate,m.abcclass ");
            sb.Append(" FROM invmon a ");
            sb.Append(" LEFT JOIN invmas m ON a.itnbr = m.itnbr  ");
            sb.Append(" LEFT JOIN invcls s ON s.itcls = a.itcls AND s.itcls = m.itcls ");
            sb.Append(" LEFT JOIN invbal b ON a.facno = b.facno AND a.prono = b.prono AND a.wareh = b.wareh AND a.itnbr = b.itnbr ");
            //<--取库存的入库的最大日期
            //sb.Append(" LEFT JOIN (select facno,prono,itnbr,onhand1,lindate from invbal a where facno = 'C' and prono = '1'  ");
            //sb.Append(" and lindate = (select max(lindate) from invbal b where a.facno = b.facno and a.prono = b.prono and a.itnbr = b.itnbr)) b ");
            //sb.Append(" ON a.facno = b.facno AND a.prono = b.prono AND a.itnbr = b.itnbr   ");
            //-->
            sb.Append(" WHERE  a.facno = ").Append(facno).Append(" AND a.prono = '1' AND a.trnqys >0 ");
            sb.Append(" AND a.wareh IN  ").Append(wareh);
            sb.Append(" AND a.itcls IN  ").Append(itcls2);
            sb.Append(" and a.iocode='Z'  ");
            sb.Append(" AND (m.jityn <> 'J' AND m.jityn <> 'P')  ");
            sb.Append(" AND a.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            sb.Append(" ) a ");
            sb.Append(" LEFT JOIN ( select v.vdrna,dt.itnbr,dt.leadtime,dt.pominqy from  purdis ds , purvdrit dt LEFT JOIN purvdr v ON dt.vdrno = v.vdrno ");
            sb.Append(" where ds.mainyn='Y' ");
            sb.Append(" and ds.vdrno=dt.vdrno and ds.itnbr=dt.itnbr ) p ON a.itnbr = p.itnbr  ");
            sb.Append(" left join invsafqy q on a.itnbr =q.itnbr AND a.facno = q.facno AND a.prono = q.prono ");
            sb.Append(" LEFT JOIN (SELECT facno,prono,itnbr,isnull(sum(( poqy1  -  okqy1 )),0) AS 'cgwj'  ");
            sb.Append(" FROM purdta WHERE facno = ").Append(facno);
            sb.Append(" AND prono = '1'  AND ( poqy1  -  okqy1 )>0 and dposta not in ('98','99') ");
            sb.Append(" GROUP BY facno,prono,itnbr) m ON a.itnbr = m.itnbr AND a.facno = m.facno AND a.prono = m.prono  ");
            sb.Append(" LEFT JOIN   (  ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh1 FROM invmon h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and (d.syscode IN ('50','30') or h.trtype = 'ARY')  ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-1,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h1 ON a.facno = h1.facno AND a.prono = h1.prono AND a.itnbr = h1.itnbr ");
            sb.Append(" LEFT JOIN   (  ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh2 FROM invmon h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and (d.syscode IN ('50','30') or h.trtype = 'ARY')  ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-2,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h2 ON a.facno = h2.facno AND a.prono = h2.prono AND a.itnbr = h2.itnbr ");
            sb.Append(" LEFT JOIN   (  ");
            sb.Append(" SELECT h.facno,h.prono,h.itnbr,h.yearmon,sum(h.trnqys)  as trnqyh3 FROM invmonh h,invdou d WHERE ");
            sb.Append(" h.trtype = d.trtype AND  ");
            sb.Append(" h.facno = ").Append(facno);
            sb.Append(" and h.prono = '1'  and h.iocode = '2' and (d.syscode IN ('50','30') or h.trtype = 'ARY')  ");
            sb.Append(" and h.yearmon = convert(VARCHAR(6),DATEADD(MONTH,-3,getdate()),112) ");
            sb.Append(" group BY h.facno,h.prono,h.itnbr,h.yearmon) h3 ON a.facno = h3.facno AND a.prono = h3.prono AND a.itnbr = h3.itnbr ");
            Fill(sb.ToString(), ds, "dbtlb");

            foreach (DataRow item in ds.Tables["dbtlb"].Rows)
            {
                if (item["zzts"] != null && item["leadtime"] != null)
                {
                    Double zzts = Double.Parse(item["zzts"].ToString());
                    Double leadtime = Double.Parse(item["leadtime"].ToString());
                    Double cy = zzts - leadtime;
                    item["cy"] = cy;
                }
                item["itnbr"] = "'"+item["itnbr"];
                ds.Tables["dbtlb"].AcceptChanges();
            }

        }
    }
}
