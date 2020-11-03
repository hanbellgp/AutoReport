using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMOrdersMConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CRMOrdersMConfig() { 
        
        }

        public CRMOrdersMConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CRMOrdersMDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = @"SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM cdrhmas A,cdrdmas B,cdrcus C,secuser D,invmas E,miscode F,miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' 
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND  A.depno IN('1B000','1B100','1C000','1C100','1D000','1D100','1E000','1E100','1F000','1F100','1F500','1V000','1V100','1T100','1T000')
                            UNION  ALL
                            SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM gzerp..cdrhmas A,gzerp..cdrdmas B,gzerp..cdrcus C,gzerp..secuser D,gzerp..invmas E,gzerp..miscode F,gzerp..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01' 
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND A.depno IN ('1D000','1D100')
                            UNION  ALL
                            SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM njerp..cdrhmas A,njerp..cdrdmas B,njerp..cdrcus C,njerp..secuser D,njerp..invmas E,njerp..miscode F,njerp..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND A.depno IN('1E000','1E100')
                            UNION  ALL
                            SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM jnerp..cdrhmas A,jnerp..cdrdmas B,jnerp..cdrcus C,jnerp..secuser D,jnerp..invmas E,jnerp..miscode F,jnerp..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND A.depno IN('1C000','1C100')
                            UNION  ALL
                            SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM cqerp..cdrhmas A,cqerp..cdrdmas B,cqerp..cdrcus C,cqerp..secuser D,cqerp..invmas E,cqerp..miscode F,cqerp..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND A.depno IN ('1V000','1V100')
                            ORDER BY  depno,cdrno; ";

            Fill(sqlstr, ds, "tbcrmorders");
            //Fill(String.Format(sqlstr2, args["erp"], args["depno"]), ds, "tbcrmorders");

        }
    }
}
