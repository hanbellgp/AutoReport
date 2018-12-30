using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
   public  class KehuConfig: NotificationConfig
    {
       public KehuConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSKehu();
            this.reportList.Add(new KehuReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification, this.ToString());
        }


       public override void InitData()
       {
           string sqlstr = @" SELECT xx.*,cast(isnull(p.stdprice, 0) AS DECIMAL(16, 2)) stdprice,cast(CASE WHEN p.stdprice IS NULL THEN 0  ELSE round(100 * (xx.unpris) / p.stdprice, 2) END AS DECIMAL(10, 2)) 'stdaccout',
  p.pricingid '标准定价单号' FROM (SELECT qh.mancode,se.username 'manname', se3.username 'keyinname',cu.cusna,convert(CHAR(12), dh.cfmdate, 112) cfmdate, d.itnbrcus,
  dd.itnbr, im.itdsc,d.shpqy1  '本次出货数量',qd.quaqy1 '报价数量',cast(round(dh.ratio * d.unpris, 2) AS DECIMAL(16, 2)) 'unpris', se2.username,
  cast(qd.contunpri AS DECIMAL(16, 2)) contunpri,cast(CASE qd.contunpri WHEN 0 THEN 0 ELSE round(100 * dh.ratio * dd.unpris / qd.contunpri, 2) END AS DECIMAL(16, 2)) 'conaccout',
     CASE qh.isspecial WHEN 'Y' THEN '特殊流程' ELSE '权限审批' END  'flowtype',s.cdesc 'resdesc', s1.cdesc 'prtypedesc', qh.cusno,qh.oacfuser,qh.quono,dh.pricingtype,dh.cdrno,
    d.shpno, d.trseq,dd.dmark1, dd.dmark2,dd.trseq FROM cdrqhad qh LEFT JOIN secuser se ON qh.mancode = se.userno LEFT JOIN secuser se2 ON qh.oacfuser = se2.userno
     LEFT JOIN secuser se3 ON qh.userno = se3.userno LEFT JOIN miscode s1 ON qh.pricingtype = s1.code AND s1.ckind = '1C',
    cdrdmas dd   left join   cdrdta d  on  d.facno = dd.facno and d.cdrno = dd.cdrno  and dd.trseq=d.ctrseq AND dd.itnbr = d.itnbr, cdrhmas dh, miscode s, cdrcus cu, invmas im,
     cdrqdta qd,invpri ih  WHERE qh.facno = 'C' AND dh.facno = 'C' AND ih.facno = 'C' AND qh.facno = qd.facno AND dh.facno = dd.facno AND qh.quono = qd.quono  AND dh.cdrno = dd.cdrno
      AND dd.itnbr = qd.itnbr AND qd.dmark1 = dd.dmark1 AND dd.itnbr = im.itnbr AND qd.dmark1 = dd.dmark1 AND qd.dmark2 = dd.dmark2 AND qd.quaqy1 = dd.cdrqy1
         AND qh.bcdrno = dh.cdrno  AND ih.itnbr = dd.itnbr AND dh.cusno = cu.cusno AND qh.hquosta IN ('Y', 'T', 'P') AND dh.pricingtype IN ('RC') 
    and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
       AND s.ckind = '1O' AND qh.apprresno = s.code AND (dd.dmark1 <> '' OR dd.dmark2 <> '' OR dh.depno IN
       ('1E000', '1E100', '1F100', '1B000', '1D000', '1D100', '1C000', '1C100', '1G120', '1N120', '1B100', '1D100', '1Q000',
        '1H100', '1G120', '1Q000', '1F500')) AND   --dh.hrecsta = 'Y' AND
      ih.yearmon = convert(CHAR(6), DATEADD(MONTH, -1, getdate()), 112) and convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) < convert(CHAR(6), DATEADD(MONTH, 0, getdate()), 112)
      AND convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) >= convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)+ '01' AND convert(CHAR(4), dh.cfmdate, 112) = convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)
   UNION ALL
   SELECT
     qh.mancode, se.username  'manname',se3.username  'keyinname', cu.cusna,convert(CHAR(12), dh.cfmdate, 112)  cfmdate,
     d.itnbrcus,dd.itnbr, im.itdsc, d.shpqy1   '出货数量', qd.quaqy1   '报价数量', cast(round(dh.ratio * d.unpris, 2) AS DECIMAL(16, 2))  'unpris',se2.username,
     cast(qd.contunpri AS DECIMAL(16, 2))  contunpri, cast(CASE qd.contunpri
          WHEN 0 THEN 0 ELSE round(100 * dh.ratio * dd.unpris / qd.contunpri, 2) END AS DECIMAL(16, 2)) 'conaccout',
     CASE qh.isspecial  WHEN 'Y' THEN '特殊流程' ELSE '权限审批' END  'flowtype', s.cdesc 'resdesc',s1.cdesc  'prtypedesc',
     qh.cusno, qh.oacfuser,qh.quono,dh.pricingtype, dh.cdrno, d.shpno,d.trseq,dd.dmark1, dd.dmark2,dd.trseq
   FROM njerp..cdrqhad qh LEFT JOIN test..secuser se ON qh.mancode = se.userno LEFT JOIN test..secuser se2 ON qh.oacfuser = se2.userno
     LEFT JOIN test..secuser se3 ON qh.userno = se3.userno LEFT JOIN miscode s1 ON qh.pricingtype = s1.code AND s1.ckind = '1C'
     , njerp..cdrdmas dd  left join  njerp.. cdrdta d  on  d.facno = dd.facno and d.cdrno = dd.cdrno  and dd.trseq=d.ctrseq AND dd.itnbr = d.itnbr
     , njerp..cdrhmas dh , miscode s, njerp..cdrcus cu, invmas im, njerp..cdrqdta qd,  njerp..invpri ih
   WHERE qh.facno = 'N' AND dh.facno = 'N' AND ih.facno = 'N' AND qh.facno = qd.facno AND dh.facno = dd.facno 	AND qh.quono = qd.quono  AND dh.cdrno = dd.cdrno
           AND dd.itnbr = qd.itnbr AND qd.dmark1 = dd.dmark1 AND dd.itnbr = im.itnbr AND qd.dmark1 = dd.dmark1 AND qd.dmark2 = dd.dmark2 AND qd.quaqy1 = dd.cdrqy1
         AND qh.bcdrno = dh.cdrno  AND ih.itnbr = dd.itnbr AND dh.cusno = cu.cusno AND qh.hquosta IN ('Y', 'T', 'P') AND dh.pricingtype IN ('RC') 
         and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
         AND s.ckind = '1O' AND qh.apprresno = s.code
         AND (dd.dmark1 <> '' OR dd.dmark2 <> '' OR dh.depno IN('1E000', '1E100', '1F100', '1B000', '1D000', '1D100', '1C000', '1C100', '1G120', '1N120', '1B100', '1D100', '1Q000',
        '1H100', '1G120', '1Q000', '1F500')) AND   --dh.hrecsta = 'Y' AND
         ih.yearmon = convert(CHAR(6), DATEADD(MONTH, -1, getdate()), 112) and convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) < convert(CHAR(6), DATEADD(MONTH, 0, getdate()), 112)
      AND convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) >= convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)+ '01' AND convert(CHAR(4), dh.cfmdate, 112) = convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)
   UNION ALL
  SELECT  qh.mancode, se.username   'manname', se3.username  'keyinname',cu.cusna,convert(CHAR(12), dh.cfmdate, 112)  cfmdate,d.itnbrcus,dd.itnbr,im.itdsc,d.shpqy1  '出货数量',
     qd.quaqy1 '报价数量',cast(round(dh.ratio * d.unpris, 2) AS DECIMAL(16, 2))  'unpris', se2.username,cast(qd.contunpri AS DECIMAL(16, 2)) contunpri, cast(CASE qd.contunpri
          WHEN 0 THEN 0 ELSE round(100 * dh.ratio * dd.unpris / qd.contunpri, 2) END AS DECIMAL(16, 2)) 'conaccout',
     CASE qh.isspecial WHEN 'Y' THEN '特殊流程'
     ELSE '权限审批' END  'flowtype',s.cdesc 'resdesc', s1.cdesc 'prtypedesc',qh.cusno,qh.oacfuser,
     qh.quono,dh.pricingtype,dh.cdrno, d.shpno,d.trseq,dd.dmark1, dd.dmark2, dd.trseq
   FROM jnerp..cdrqhad qh LEFT JOIN test..secuser se ON qh.mancode = se.userno LEFT JOIN test..secuser se2 ON qh.oacfuser = se2.userno
     LEFT JOIN test..secuser se3 ON qh.userno = se3.userno LEFT JOIN miscode s1 ON qh.pricingtype = s1.code AND s1.ckind = '1C'
     , jnerp..cdrdmas dd  left join  jnerp.. cdrdta d  on  d.facno = dd.facno and d.cdrno = dd.cdrno  and dd.trseq=d.ctrseq AND dd.itnbr = d.itnbr
     , jnerp..cdrhmas dh , miscode s, jnerp..cdrcus cu, invmas im, jnerp..cdrqdta qd,  jnerp..invpri ih
   WHERE qh.facno = 'J' AND dh.facno = 'J' AND ih.facno = 'J' AND qh.facno = qd.facno AND dh.facno = dd.facno
			AND qh.quono = qd.quono  AND dh.cdrno = dd.cdrno AND dd.itnbr = qd.itnbr AND qd.dmark1 = dd.dmark1 AND dd.itnbr = im.itnbr
      AND qd.dmark1 = dd.dmark1 AND qd.dmark2 = dd.dmark2 AND qd.quaqy1 = dd.cdrqy1
         AND qh.bcdrno = dh.cdrno  AND ih.itnbr = dd.itnbr AND dh.cusno = cu.cusno AND  qh.hquosta IN ('Y', 'T', 'P') AND dh.pricingtype IN ('RC') 
    and ( qh.oacfuser <> '' and qh.oacfuser is not null )
          AND s.ckind = '1O' AND qh.apprresno = s.code
         AND (dd.dmark1 <> '' OR dd.dmark2 <> '' OR dh.depno IN('1E000', '1E100', '1F100', '1B000', '1D000', '1D100', '1C000', '1C100', '1G120', '1N120', '1B100', '1D100', '1Q000',
        '1H100', '1G120', '1Q000', '1F500'))
		 AND   --dh.hrecsta = 'Y' AND
      ih.yearmon = convert(CHAR(6), DATEADD(MONTH, -1, getdate()), 112) and convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) < convert(CHAR(6), DATEADD(MONTH, 0, getdate()), 112)
      AND convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) >= convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)+ '01' AND convert(CHAR(4), dh.cfmdate, 112) = convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)
   UNION ALL
   SELECT
     qh.mancode,se.username  'manname', se3.username   'keyinname', cu.cusna,convert(CHAR(12), dh.cfmdate, 112) cfmdate, d.itnbrcus,dd.itnbr,im.itdsc, d.shpqy1 '出货数量',
     qd.quaqy1 '报价数量',cast(round(dh.ratio * d.unpris, 2) AS DECIMAL(16, 2)) 'unpris',se2.username,cast(qd.contunpri AS DECIMAL(16, 2))  contunpri,
     cast(CASE qd.contunpri   WHEN 0  THEN 0 ELSE round(100 * dh.ratio * dd.unpris / qd.contunpri, 2) END AS DECIMAL(16, 2)) 'conaccout',
     CASE qh.isspecial WHEN 'Y' THEN '特殊流程' ELSE '权限审批' END  'flowtype',
     s.cdesc 'resdesc',  s1.cdesc 'prtypedesc', qh.cusno,qh.oacfuser,qh.quono, dh.pricingtype, dh.cdrno, d.shpno,d.trseq, dd.dmark1, dd.dmark2,
     dd.trseq FROM gzerp..cdrqhad qh LEFT JOIN test..secuser se ON qh.mancode = se.userno LEFT JOIN test..secuser se2 ON qh.oacfuser = se2.userno
     LEFT JOIN test..secuser se3 ON qh.userno = se3.userno LEFT JOIN miscode s1 ON qh.pricingtype = s1.code AND s1.ckind = '1C' ,
     gzerp..cdrdmas dd  left join  gzerp.. cdrdta d  on  d.facno = dd.facno and d.cdrno = dd.cdrno  and dd.trseq=d.ctrseq AND dd.itnbr = d.itnbr , gzerp..cdrhmas dh , miscode s, gzerp..cdrcus cu, invmas im,
     gzerp..cdrqdta qd,  gzerp..invpri ih WHERE qh.facno = 'G' AND dh.facno = 'G' AND ih.facno = 'G' AND qh.facno = qd.facno AND dh.facno = dd.facno AND qh.quono = qd.quono  AND dh.cdrno = dd.cdrno
           AND dd.itnbr = qd.itnbr AND qd.dmark1 = dd.dmark1 AND dd.itnbr = im.itnbr  AND qd.dmark1 = dd.dmark1 AND qd.dmark2 = dd.dmark2 AND qd.quaqy1 = dd.cdrqy1 AND qh.bcdrno = dh.cdrno
     AND ih.itnbr = dd.itnbr AND dh.cusno = cu.cusno AND qh.hquosta IN ('Y', 'T', 'P') AND dh.pricingtype IN ('RC')  and ( qh.oacfuser <> '' and qh.oacfuser is not null ) AND s.ckind = '1O' AND qh.apprresno = s.code AND (dd.dmark1 <> '' OR dd.dmark2 <> '' OR dh.depno IN
       ('1E000', '1E100', '1F100', '1B000', '1D000', '1D100', '1C000', '1C100', '1G120', '1N120', '1B100', '1D100', '1Q000', '1H100', '1G120', '1Q000', '1F500'))
		 AND   --dh.hrecsta = 'Y' AND
       ih.yearmon = convert(CHAR(6), DATEADD(MONTH, -1, getdate()), 112) and  convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) < convert(CHAR(6), DATEADD(MONTH, 0, getdate()), 112)
      AND convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) >= convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)+ '01'  AND convert(CHAR(4), dh.cfmdate, 112) = convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)
    UNION ALL
   SELECT
     qh.mancode, se.username   'manname', se3.username 'keyinname',cu.cusna,convert(CHAR(12), dh.cfmdate, 112) cfmdate, d.itnbrcus, dd.itnbr, im.itdsc,
     d.shpqy1  '出货数量',qd.quaqy1  '报价数量', cast(round(dh.ratio * d.unpris, 2) AS DECIMAL(16, 2))  'unpris', se2.username,cast(qd.contunpri AS DECIMAL(16, 2))  contunpri,
     cast(CASE qd.contunpri WHEN 0 THEN 0 ELSE round(100 * dh.ratio * dd.unpris / qd.contunpri, 2) END AS DECIMAL(16, 2)) 'conaccout',
     CASE qh.isspecial WHEN 'Y' THEN '特殊流程'  ELSE '权限审批' END   'flowtype',s.cdesc 'resdesc',s1.cdesc 'prtypedesc', qh.cusno,qh.oacfuser,qh.quono,dh.pricingtype,
     dh.cdrno, d.shpno,d.trseq, dd.dmark1, dd.dmark2,dd.trseq  FROM cqerp..cdrqhad qh LEFT JOIN test..secuser se ON qh.mancode = se.userno  LEFT JOIN test..secuser se2 ON qh.oacfuser = se2.userno
     LEFT JOIN test..secuser se3 ON qh.userno = se3.userno LEFT JOIN miscode s1 ON qh.pricingtype = s1.code AND s1.ckind = '1C'
     , cqerp..cdrdmas dd  left join  cqerp.. cdrdta d  on  d.facno = dd.facno and d.cdrno = dd.cdrno  and dd.trseq=d.ctrseq AND dd.itnbr = d.itnbr
     , cqerp..cdrhmas dh , miscode s, cqerp..cdrcus cu, invmas im,cqerp..cdrqdta qd,  cqerp..invpri ih  WHERE qh.facno = 'C4' AND dh.facno = 'C4' AND ih.facno = 'C4' AND qh.facno = qd.facno AND dh.facno = dd.facno
			AND qh.quono = qd.quono  AND dh.cdrno = dd.cdrno  AND dd.itnbr = qd.itnbr AND qd.dmark1 = dd.dmark1 AND dd.itnbr = im.itnbr AND qd.dmark1 = dd.dmark1 AND qd.dmark2 = dd.dmark2 AND qd.quaqy1 = dd.cdrqy1
         AND qh.bcdrno = dh.cdrno  AND ih.itnbr = dd.itnbr AND dh.cusno = cu.cusno AND qh.hquosta IN ('Y', 'T', 'P') AND dh.pricingtype IN ('RC') 
     and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
		 AND s.ckind = '1O' AND qh.apprresno = s.code AND (dd.dmark1 <> '' OR dd.dmark2 <> '' OR dh.depno IN
       ('1E000', '1E100', '1F100', '1B000', '1D000', '1D100', '1C000', '1C100', '1G120', '1N120', '1B100', '1D100', '1Q000', '1H100', '1G120', '1Q000', '1F500'))
		 AND   --dh.hrecsta = 'Y' AND
        ih.yearmon = convert(CHAR(6), DATEADD(MONTH, -1, getdate()), 112) and convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) < convert(CHAR(6), DATEADD(MONTH, 0, getdate()), 112)
      AND convert(CHAR(6), DATEADD(MONTH, 0, dh.cfmdate), 112) >= convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)+ '01' AND convert(CHAR(4), dh.cfmdate, 112) = convert(CHAR(4), DATEADD(MONTH, -1, getdate()), 112)
   ) xx LEFT JOIN
  (SELECT a.pricingid, a.pricingtype, CASE a.pricingtype  WHEN 'AAD' THEN b.price05 WHEN 'AJC'  THEN b.price06  WHEN 'PAC' THEN b.price06
     WHEN 'PAD' THEN b.price08 WHEN 'RC' THEN b.price04 WHEN 'SDS' THEN b.price07  WHEN 'AJSC'  THEN b.price04 END 'stdprice',
     b.itnbr, b.itemno FROM pricingpolicy a, pricing b WHERE a.pricingid = b.pricingid AND a.status = 'V' AND a.pricingtype IN ('AAD', 'AJC', 'PAD', 'PAC', 'RC', 'SDS', 'AJSC')
     AND NOT exists(SELECT *  FROM pricingpolicy c, pricing d WHERE c.pricingid = d.pricingid AND c.status = 'V' AND c.pricingtype IN ('AAD', 'AJC', 'PAD', 'PAC', 'RC', 'SDS', 'AJSC') AND
    a.pricingtype = c.pricingtype AND b.itnbr = d.itnbr AND b.itemno = d.itemno AND   c.pricingid > a.pricingid)) p ON xx.pricingtype = p.pricingtype AND xx.itnbr = p.itnbr AND
         (((xx.dmark1 <> '' AND NOT (xx.dmark1 IS NULL)) AND xx.dmark1 = p.itemno) OR (xx.dmark1 = '' OR xx.dmark1 IS NULL)) order by cdrno ";

           Fill(sqlstr, ds, "Kehu");


       }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}
        

    }
}