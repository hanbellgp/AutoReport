using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class ShpKehuConfig : NotificationConfig
    {
        public ShpKehuConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSShpKehu();
            this.reportList.Add(new ShpKehuReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @" select xx.*, cast( isnull(p.stdprice,0) as decimal(16,2)) stdprice, 
        cast(case when p.stdprice is null then 0 else  round(100*(xx.unpris)/p.stdprice,2) end as decimal(10,2)) 'stdaccout',p.pricingid '标准定价单号' from 
        (select qh.mancode ,se.username 'manname' ,se3.username 'keyinname',cu.cusna ,convert(char(12), h.shpdate,112) shpdate ,d.itnbrcus ,d.itnbr ,
        im.itdsc ,d.shpqy1 '本次出货数量',qd.quaqy1 '报价数量' ,cast(round( h.ratio*d.unpris ,2) as decimal(16,2)) 'unpris' , 
        se2.username ,cast ( qd.contunpri as decimal(16,2)) contunpri ,
        cast(case qd.contunpri when 0 then 0 else round(100*h.ratio*d.unpris/qd.contunpri,2) end as decimal(16,2)) 'conaccout',
        case qh.isspecial when 'Y' then '特殊流程' else '权限审批' end  'flowtype',s.cdesc 'resdesc',
        s1.cdesc 'prtypedesc',qh.cusno  ,qh.quono ,dh.pricingtype,dh.cdrno ,h.shpno ,d.trseq ,dd.dmark1, dd.dmark2 , d.trseq
        from cdrqhad qh left join secuser se on qh.mancode=se.userno
        left join secuser se2 on qh.oacfuser=se2.userno  left join secuser se3 on qh.userno =se3.userno  left join miscode s1  on qh.pricingtype = s1.code and s1.ckind='1C' 
        ,cdrdmas dd ,cdrhmas dh ,cdrdta d ,  invpri ih , cdrhad h ,miscode s,cdrcus cu,invmas im,cdrqdta qd  where qh.facno='C' and dh.facno='C' and  ih.facno='C'  and
        qh.facno=qd.facno and qh.quono=qd.quono and dh.facno=dd.facno and dh.cdrno=dd.cdrno and h.facno =d.facno and h.shpno =d.shpno 
        and dd.itnbr=d.itnbr  and dd.itnbr=qd.itnbr and qd.dmark1 = dd.dmark1 and d.itnbr = im.itnbr 
        and d.ctrseq=dd.trseq and qd.dmark1=dd.dmark1 and qd.dmark2=dd.dmark2 and qd.quaqy1=dd.cdrqy1
        and  qh.bcdrno = dh.cdrno and  d.cdrno=dh.cdrno  and  ih.itnbr=d.itnbr  and dh.cusno=cu.cusno  and qh.hquosta in ('Y','T','P') and dh.pricingtype in ('RC') 
        and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
          and s.ckind='1O'   and qh.apprresno=s.code
        and  ( dd.dmark1 <> '' or dd.dmark2 <> ''  or h.depno in ('1E000','1E100','1F100','1B000','1D000','1D100','1C000','1C100','1G120','1N120','1B100','1D100','1Q000',
        '1H100','1G120','1Q000','1F500' ) )  and h.houtsta='Y'  and  ih.yearmon = convert(char(6),DATEADD(month,-1,getdate()),112) 
        and convert(char(4), h.shpdate,112) =convert(char(4),DATEADD(month,-1,getdate()),112) 
        union all
        select qh.mancode ,se.username 'manname' ,se3.username 'keyinname',cu.cusna , convert(char(12), h.shpdate,112) shpdate ,d.itnbrcus ,d.itnbr ,
        im.itdsc ,d.shpqy1 '出货数量',qd.quaqy1 '报价数量' , cast(round( h.ratio*d.unpris ,2) as decimal(16,2)) 'unpris' ,
        se2.username ,cast ( qd.contunpri as decimal(16,2)) contunpri ,
        cast(case qd.contunpri when 0 then 0 else round(100*h.ratio*d.unpris/qd.contunpri,2) end as decimal(16,2)) 'conaccout',
        case qh.isspecial when 'Y' then '特殊流程' else '权限审批' end  'flowtype',s.cdesc 'resdesc',
        s1.cdesc 'prtypedesc',qh.cusno ,qh.quono ,dh.pricingtype, dh.cdrno ,h.shpno ,d.trseq ,dd.dmark1, dd.dmark2 , d.trseq
        from njerp..cdrqhad qh left join test..secuser se on qh.mancode=se.userno  left join secuser se2 on qh.oacfuser=se2.userno
        left join test..secuser se3 on qh.userno =se3.userno  left join miscode s1  on qh.pricingtype = s1.code and s1.ckind='1C' ,njerp..cdrdmas dd
        ,njerp..cdrhmas dh ,njerp..cdrdta d ,  njerp..invpri ih , njerp..cdrhad h ,miscode s,njerp..cdrcus cu,invmas im,njerp..cdrqdta qd 
        where qh.facno='N' and dh.facno='N' and  ih.facno='N'  and qh.facno=qd.facno and qh.quono=qd.quono and  dh.facno=dd.facno and dh.cdrno=dd.cdrno 
        and h.facno =d.facno and h.shpno =d.shpno and dd.itnbr=d.itnbr  and dd.itnbr=qd.itnbr and qd.dmark1 = dd.dmark1 and d.itnbr = im.itnbr 
        and d.ctrseq=dd.trseq and qd.dmark1=dd.dmark1 and qd.dmark2=dd.dmark2 and qd.quaqy1=dd.cdrqy1  and  qh.bcdrno = dh.cdrno and  d.cdrno=dh.cdrno 
        and ih.itnbr=d.itnbr  and dh.cusno=cu.cusno and qh.hquosta in ('Y','T','P') and dh.pricingtype in ('RC')
        and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
         and s.ckind='1O'   and qh.apprresno=s.code  and  ( dd.dmark1 <> '' or dd.dmark2 <> ''  or h.depno in ('1E000',
        '1E100','1F100','1B000','1D000','1D100','1C000','1C100', '1G120','1N120','1B100','1D100','1Q000','1H100','1G120','1Q000' ,'1F500') )  and h.houtsta='Y' 
        and  ih.yearmon = convert(char(6),DATEADD(month,-1,getdate()),112) and convert(char(4), h.shpdate,112) =convert(char(4),DATEADD(month,-1,getdate()),112)
        union all
        select qh.mancode ,se.username 'manname' ,se3.username 'keyinname',cu.cusna , convert(char(12), h.shpdate,112) shpdate ,d.itnbrcus ,d.itnbr ,
        im.itdsc ,d.shpqy1 '出货数量',qd.quaqy1 '报价数量' , cast(round( h.ratio*d.unpris ,2) as decimal(16,2)) 'unpris' ,
        se2.username ,cast ( qd.contunpri as decimal(16,2)) contunpri ,
        cast(case qd.contunpri when 0 then 0 else round(100*h.ratio*d.unpris/qd.contunpri,2) end as decimal(16,2)) 'conaccout',
        case qh.isspecial when 'Y' then '特殊流程' else '权限审批' end  'flowtype',s.cdesc 'resdesc',
        s1.cdesc 'prtypedesc',qh.cusno  ,qh.quono ,dh.pricingtype, dh.cdrno ,h.shpno ,d.trseq ,dd.dmark1, dd.dmark2 , d.trseq
        from cqerp..cdrqhad qh left join test..secuser se on qh.mancode=se.userno  left join secuser se2 on qh.oacfuser=se2.userno
        left join test..secuser se3 on qh.userno =se3.userno  left join miscode s1  on qh.pricingtype = s1.code and s1.ckind='1C' ,cqerp..cdrdmas dd
        ,cqerp..cdrhmas dh ,cqerp..cdrdta d ,  cqerp..invpri ih , cqerp..cdrhad h ,miscode s,cqerp..cdrcus cu,invmas im,cqerp..cdrqdta qd 
        where qh.facno='C4' and dh.facno='C4' and  ih.facno='C4'  and qh.facno=qd.facno and qh.quono=qd.quono and  dh.facno=dd.facno and dh.cdrno=dd.cdrno 
        and h.facno =d.facno and h.shpno =d.shpno and dd.itnbr=d.itnbr  and dd.itnbr=qd.itnbr and qd.dmark1 = dd.dmark1 and d.itnbr = im.itnbr 
        and d.ctrseq=dd.trseq and qd.dmark1=dd.dmark1 and qd.dmark2=dd.dmark2 and qd.quaqy1=dd.cdrqy1  and  qh.bcdrno = dh.cdrno and  d.cdrno=dh.cdrno 
        and ih.itnbr=d.itnbr  and dh.cusno=cu.cusno and qh.hquosta in ('Y','T','P') and dh.pricingtype in ('RC') 
        and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
         and s.ckind='1O'   and qh.apprresno=s.code  and  ( dd.dmark1 <> '' or dd.dmark2 <> ''  or h.depno in ('1E000',
        '1E100','1F100','1B000','1D000','1D100','1C000','1C100', '1G120','1N120','1B100','1D100','1Q000','1H100','1G120','1Q000' ,'1F500','1V000' ,'1V100') )  and h.houtsta='Y' 
        and  ih.yearmon = convert(char(6),DATEADD(month,-1,getdate()),112) and convert(char(4), h.shpdate,112) =convert(char(4),DATEADD(month,-1,getdate()),112)
        union all
        select qh.mancode ,se.username 'manname' ,se3.username 'keyinname',cu.cusna ,convert(char(12), h.shpdate,112) shpdate ,d.itnbrcus ,d.itnbr ,
        im.itdsc ,d.shpqy1 '出货数量',qd.quaqy1 '报价数量' , cast(round( h.ratio*d.unpris ,2) as decimal(16,2)) 'unpris' , 
        se2.username ,cast ( qd.contunpri as decimal(16,2)) contunpri ,
        cast(case qd.contunpri when 0 then 0 else round(100*h.ratio*d.unpris/qd.contunpri,2) end as decimal(16,2)) 'conaccout',
        case qh.isspecial when 'Y' then '特殊流程' else '权限审批' end  'flowtype',s.cdesc 'resdesc',s1.cdesc 'prtypedesc',qh.cusno ,qh.quono ,dh.pricingtype,
        dh.cdrno ,h.shpno ,d.trseq ,dd.dmark1, dd.dmark2 , d.trseq  from gzerp..cdrqhad qh left join test..secuser se on qh.mancode=se.userno
        left join secuser se2 on qh.oacfuser=se2.userno  left join test..secuser se3 on qh.userno =se3.userno  left join miscode s1  on qh.pricingtype = s1.code and s1.ckind='1C' 
        ,gzerp..cdrdmas dd ,gzerp..cdrhmas dh ,gzerp..cdrdta d ,  gzerp..invpri ih , gzerp..cdrhad h ,miscode s,gzerp..cdrcus cu,invmas im,gzerp..cdrqdta qd 
        where qh.facno='G' and dh.facno='G' and  ih.facno='G'  and  qh.facno=qd.facno and qh.quono=qd.quono and  dh.facno=dd.facno and dh.cdrno=dd.cdrno 
        and h.facno =d.facno and h.shpno =d.shpno and dd.itnbr=d.itnbr  and dd.itnbr=qd.itnbr and qd.dmark1 = dd.dmark1 and d.itnbr = im.itnbr 
        and d.ctrseq=dd.trseq and qd.dmark1=dd.dmark1 and qd.dmark2=dd.dmark2 and qd.quaqy1=dd.cdrqy1 and  qh.bcdrno = dh.cdrno and  d.cdrno=dh.cdrno 
        and ih.itnbr=d.itnbr  and dh.cusno=cu.cusno and qh.hquosta in ('Y','T','P') and dh.pricingtype in ('RC') 
        and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
          and s.ckind='1O'   and qh.apprresno=s.code  and  ( dd.dmark1 <> '' or dd.dmark2 <> ''  or h.depno in ('1E000',
        '1E100','1F100','1B000','1D000','1D100','1C000','1C100', '1G120','1N120','1B100','1D100','1Q000','1H100','1G120','1Q000','1F500' ) )  and h.houtsta='Y' 
        and  ih.yearmon = convert(char(6),DATEADD(month,-1,getdate()),112) and convert(char(4), h.shpdate,112) =convert(char(4),DATEADD(month,-1,getdate()),112)
        union all
        select qh.mancode ,se.username 'manname' ,se3.username 'keyinname',cu.cusna ,convert(char(12), h.shpdate,112) shpdate ,d.itnbrcus ,d.itnbr ,
        im.itdsc ,d.shpqy1 '出货数量',qd.quaqy1 '报价数量' , cast(round( h.ratio*d.unpris ,2) as decimal(16,2)) 'unpris' ,
        se2.username ,cast ( qd.contunpri as decimal(16,2)) contunpri ,
        cast(case qd.contunpri when 0 then 0 else round(100*h.ratio*d.unpris/qd.contunpri,2) end as decimal(16,2)) 'conaccout',
        case qh.isspecial when 'Y' then '特殊流程' else '权限审批' end  'flowtype',s.cdesc 'resdesc',
        s1.cdesc 'prtypedesc',qh.cusno ,qh.quono ,dh.pricingtype, dh.cdrno ,h.shpno ,d.trseq ,dd.dmark1, dd.dmark2 , d.trseq
        from jnerp..cdrqhad qh left join test..secuser se on qh.mancode=se.userno left join secuser se2 on qh.oacfuser=se2.userno left join test..secuser se3 on qh.userno =se3.userno
        left join miscode s1  on qh.pricingtype = s1.code and s1.ckind='1C'  ,jnerp..cdrdmas dd  ,jnerp..cdrhmas dh ,jnerp..cdrdta d  ,  jnerp..invpri ih 
        , jnerp..cdrhad h ,miscode s,jnerp..cdrcus cu,invmas im,jnerp..cdrqdta qd  where qh.facno='J' and dh.facno='J' and  ih.facno='J'  and
        qh.facno=qd.facno and qh.quono=qd.quono and   dh.facno=dd.facno and dh.cdrno=dd.cdrno and h.facno =d.facno and h.shpno =d.shpno 
        and dd.itnbr=d.itnbr  and dd.itnbr=qd.itnbr and qd.dmark1 = dd.dmark1 and d.itnbr = im.itnbr 
        and d.ctrseq=dd.trseq and qd.dmark1=dd.dmark1 and qd.dmark2=dd.dmark2 and qd.quaqy1=dd.cdrqy1 and  qh.bcdrno = dh.cdrno and  d.cdrno=dh.cdrno 
        and  ih.itnbr=d.itnbr  and dh.cusno=cu.cusno and qh.hquosta in ('Y','T','P') and dh.pricingtype in ('RC') 
        and ( qh.oacfuser <> '' and qh.oacfuser is not null ) 
         and s.ckind='1O'   and qh.apprresno=s.code  and  ( dd.dmark1 <> '' or dd.dmark2 <> ''  or h.depno in ('1E000',
        '1E100','1F100','1B000','1D000','1D100','1C000','1C100', '1G120','1N120','1B100','1D100','1Q000','1H100','1G120','1Q000','1F500' ) )  and h.houtsta='Y' 
        and  ih.yearmon = convert(char(6),DATEADD(month,-1,getdate()),112) and convert(char(4), h.shpdate,112) =convert(char(4),DATEADD(month,-1,getdate()),112)) xx left join 
        ( select a.pricingid,a.pricingtype, case a.pricingtype when 'AAD' then  b.price05 when 'AJC' then b.price06 
        when 'PAC' then  b.price06 when 'PAD' then  b.price08 when 'RC' then b.price04 when 'SDS' then b.price07 when 'AJSC' then b.price04  end   'stdprice'
        ,b.itnbr ,b.itemno from pricingpolicy a ,pricing b where a.pricingid=b.pricingid and a.status='V'  and a.pricingtype in ('AAD','AJC','PAD','PAC','RC','SDS','AJSC') 
        and not exists( select * from pricingpolicy c ,pricing d where c.pricingid=d.pricingid and c.status='V'
        and c.pricingtype in ('AAD','AJC','PAD','PAC','RC','SDS','AJSC') and a.pricingtype=c.pricingtype and b.itnbr= d.itnbr  and b.itemno=d.itemno  and c.pricingid > a.pricingid )
        ) p on xx.pricingtype=p.pricingtype and xx.itnbr=p.itnbr and (((xx.dmark1<> '' and not(xx.dmark1  is null)) and  xx.dmark1=p.itemno ) or (xx.dmark1='' or xx.dmark1 is null)) order by shpno";

            Fill(sqlstr, ds, "ShpKehu");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}


    }
}