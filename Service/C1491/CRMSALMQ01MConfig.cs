using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMSALMQ01MConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
           public CRMSALMQ01MConfig()
        {
        }

           public CRMSALMQ01MConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CRMSALMQ01MDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {

            string sqlstr = @"SELECT  A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                            B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM cdrhad A,cdrdta B,cdrcus C,secuser D,cdrdmas E,miscode F,miscode n
                            WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq
                            AND A.depno=F.code AND F.ckind='GE'
                            AND A.houtsta = 'Y' AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND E.dmark1=n.code
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' 
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1B000','1B100','1C000','1C100','1C700','1D000','1D100','1E000','1E100','1F000','1F100','1F500','1V000','1V100','1T100','1T000')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                            FROM cdrhad A,cdrdta B,cdrcus C,secuser D,miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.itnbrcus != '' and B.cdrno = '9'AND
                            C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1B000','1B100','1C000','1C100','1C700','1D000','1D100','1E000','1E100','1F000','1F100','1F500','1V000','1V100','1T100','1T000')
                            UNION ALL
                            SELECT  A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,
                            C.cusna,A.mancode,D.username,B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,
                            B.n_code_DC FROM cdrbhad A,cdrbdta B,cdrcus C,secuser D,cdrdmas E,miscode F,miscode n
                            WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                            AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                             AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND 
                             left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1B000','1B100','1C000','1C100','1C700','1D000','1D100','1E000','1E100','1F000','1F100','1F500','1V000','1V100')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM gzerp..cdrhad A,gzerp..cdrdta B,gzerp..cdrcus C,gzerp..secuser D,gzerp..cdrdmas E,gzerp..miscode F,gzerp..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN('1D000','1D100')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1 ,B.n_code_DC 
                            FROM gzerp..cdrhad A,gzerp..cdrdta B,gzerp..cdrcus C,gzerp..secuser D,gzerp..miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.itnbrcus != '' and B.cdrno = '9'AND
                            C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1D000','1D100')
                            UNION ALL
                            SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM gzerp..cdrbhad A,gzerp..cdrbdta B,gzerp..cdrcus C,gzerp..secuser D,gzerp..cdrdmas E,gzerp..miscode F,gzerp..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN('1D000','1D100')
                              UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM njerp..cdrhad A,njerp..cdrdta B,njerp..cdrcus C,njerp..secuser D,njerp..cdrdmas E,njerp..miscode F,njerp..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN('1E000','1E100')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                            FROM njerp..cdrhad A,njerp..cdrdta B,njerp..cdrcus C,njerp..secuser D,njerp..miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.itnbrcus != '' and B.cdrno = '9'AND
                            C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1E000','1E100')
                            UNION ALL
                            SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM njerp..cdrbhad A,njerp..cdrbdta B,njerp..cdrcus C,njerp..secuser D,njerp..cdrdmas E,njerp..miscode F,njerp..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN('1E000','1E100')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM jnerp..cdrhad A,jnerp..cdrdta B,jnerp..cdrcus C,jnerp..secuser D,jnerp..cdrdmas E,jnerp..miscode F,jnerp..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN('1C000','1C100','1C700') 
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                            FROM jnerp..cdrhad A,jnerp..cdrdta B,jnerp..cdrcus C,jnerp..secuser D,jnerp..miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.itnbrcus != '' and B.cdrno = '9'AND
                            C.cusno NOT IN('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN('1C000','1C100','1C700') 
                            UNION ALL 
                            SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM jnerp..cdrbhad A,jnerp..cdrbdta B,jnerp..cdrcus C,jnerp..secuser D,jnerp..cdrdmas E,jnerp..miscode F,jnerp..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN ('1C000','1C100','1C700') 
                            UNION ALL 
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM cqerp..cdrhad A,cqerp..cdrdta B,cqerp..cdrcus C,cqerp..secuser D,cqerp..cdrdmas E,cqerp..miscode F,cqerp..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                               AND  A.depno IN ('1V000','1V100') 
                             UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                            FROM cqerp..cdrhad A,cqerp..cdrdta B,cqerp..cdrcus C,cqerp..secuser D,cqerp..miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.itnbrcus != '' and B.cdrno = '9'AND
                            C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN ('1V000','1V100')
                            UNION ALL 
                            SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM cqerp..cdrbhad A,cqerp..cdrbdta B,cqerp..cdrcus C,cqerp..secuser D,cqerp..cdrdmas E,cqerp..miscode F,cqerp..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code 
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN ('1V000','1V100') 
                            ORDER BY  A.depno , A.facno ,A.shpno";
            //AND left(convert(varchar(30),A.bakdate,111),10) >=  convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) AND left(convert(varchar(30),A.bakdate,111),10) <= CONVERT(varchar(100), GETDATE(), 111)
            Fill(sqlstr, ds, "tbcrmsalmq01m");

        }
    }
}
