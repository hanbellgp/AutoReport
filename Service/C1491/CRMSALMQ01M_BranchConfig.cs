using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMSALMQ01M_BranchConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CRMSALMQ01M_BranchConfig() { 
        }

        public CRMSALMQ01M_BranchConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CRMSALMQ01MDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr2 = @"SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM cdrhad A,cdrdta B,cdrcus C,secuser D,cdrdmas E,miscode F,miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.n_code_DD = '00'  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                                AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'  
                                AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)  AND A.depno IN ('{1}') 
                               UNION ALL
                                SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                                D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                                 FROM cdrhad A,cdrdta B,cdrcus C,secuser D,miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                                AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.n_code_DD = '00' and B.cdrno = '9'AND
                                C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                                AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                                AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                                AND A.depno IN ('{1}')
                                UNION ALL
                               SELECT  A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1 ,B.n_code_DC 
                               FROM cdrbhad A,cdrbdta B,cdrcus C,secuser D,cdrdmas E,miscode F,miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND 
                                 left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)  AND A.depno IN ('{1}')
                               UNION ALL
                               SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1 ,B.n_code_DC 
                               FROM {0}..cdrhad A,{0}..cdrdta B,{0}..cdrcus C,{0}..secuser D,{0}..cdrdmas E,{0}..miscode F,{0}..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.n_code_DD = '00'  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                               AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'  
                                AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                               AND  A.depno IN ('{1}')
                            UNION ALL
                            SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc, A.cusno ,C.cusna ,A.mancode,
                            D.username, B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,''as dmark1 ,''as cdesc1,B.n_code_DC 
                            FROM {0}..cdrhad A,{0}..cdrdta B,{0}..cdrcus C,{0}..secuser D,{0}..miscode F WHERE A.shpno = B.shpno AND A.cusno = C.cusno AND A.mancode = D.userno 
                            AND A.depno = F.code AND F.ckind = 'GE' AND A.houtsta = 'Y' AND B.n_code_DD = '00' and B.cdrno = '9'AND
                            C.cusno NOT IN ('SGD00088', 'SCQ00146', 'SJS00254', 'SSD00107')
                            AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                            AND A.depno IN ('{1}')
                               UNION ALL
                               SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC  
                               FROM {0}..cdrbhad A,{0}..cdrbdta B,{0}..cdrcus C,{0}..secuser D,{0}..cdrdmas E,{0}..miscode F,{0}..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code
                                AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND 
                                left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) 
                               AND  A.depno IN ('{1}') ";

            string sqlstr3 = @" SELECT A.facno,A.shpno,B.trseq,left(convert(varchar(30),A.shpdate,111),10) as shpdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                               B.cdrno,B.itnbr,B.itdsc,B.itnbrcus,B.shpqy1,B.unpris,B.shpamts,E.dmark1,n.cdesc as cdesc1,B.n_code_DC 
                               FROM qtcerp..cdrhad A,qtcerp..cdrdta B,qtcerp..cdrcus C,qtcerp..secuser D,qtcerp..cdrdmas E,qtcerp..miscode F,qtcerp..miscode n
                               WHERE A.shpno=B.shpno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.houtsta = 'Y' AND B.n_code_DD = '00'  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107') AND n.ckind='1R' AND E.dmark1=n.code
                              AND left(convert(varchar(30),A.shpdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'  
                                AND left(convert(varchar(30),A.shpdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                               UNION ALL
                               SELECT A.facno,A.bakno,B.trseq,left(convert(varchar(30),A.bakdate,111),10) as bakdate,A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,B.cdrno,
                               B.itnbr,B.itdsc,B.itnbrcus,-1*B.bshpqy1 AS bshpqy1,B.unpris,-1*B.bakamts AS bakamts,E.dmark1,n.cdesc as cdesc1 ,B.n_code_DC
                               FROM qtcerp..cdrbhad A,qtcerp..cdrbdta B,qtcerp..cdrcus C,qtcerp..secuser D,qtcerp..cdrdmas E,qtcerp..miscode F,qtcerp..miscode n
                               WHERE A.bakno=B.bakno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.cdrno=E.cdrno AND B.ctrseq=E.trseq AND A.depno=F.code AND F.ckind='GE'
                               AND A.baksta = 'Y'  AND n.ckind='1R' AND E.dmark1=n.code 
                               AND left(convert(varchar(30),A.bakdate,111),10) >=  CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' AND  
                               left(convert(varchar(30),A.bakdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) ";

            if(args["erp"].Equals("jnerp")){
                sqlstr2 += sqlstr3;
            } 
            Fill(String.Format(sqlstr2, args["erp"], args["depno"]), ds, "tbcrmsalmq01m");
        }
    }
}
