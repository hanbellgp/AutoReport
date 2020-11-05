using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMOrdersM_BranchConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CRMOrdersM_BranchConfig() { 
        }

        public CRMOrdersM_BranchConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CRMOrdersMDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr2 = @"SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
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
                            AND  A.depno IN ('{1}')
                            UNION  ALL
                            SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM {0}..cdrhmas A,{0}..cdrdmas B,{0}..cdrcus C,{0}..secuser D,{0}..invmas E,{0}..miscode F,{0}..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                                AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111)
                            AND A.depno IN ('{1}') ";

            string sqlstr3 = @"SELECT A.facno,A.cdrno,B.trseq,left(convert(varchar(30),A.cfmdate,111),10) as cfmdate,
                            A.depno,F.cdesc,C.cusno,C.cusna,A.mancode,D.username,
                            B.itnbr,E.itdsc,B.itnbrcus,B.cdrqy1,B.unpris,B.tramts,B.dmark1,n.cdesc as cdesc1,B.n_code_DC
                            FROM qtcerp..cdrhmas A,qtcerp..cdrdmas B,qtcerp..cdrcus C,qtcerp..secuser D,qtcerp..invmas E,qtcerp..miscode F,qtcerp..miscode n
                            WHERE A.cdrno=B.cdrno AND A.cusno=C.cusno AND A.mancode=D.userno AND B.itnbr=E.itnbr
                            AND A.depno=F.code AND F.ckind='GE'
                            AND ( A.hrecsta= 'Y' or A.hrecsta = 'C')  AND  (B.drecsta != '98' and B.drecsta != '99' and B.drecsta != '10')
                            AND B.itnbrcus !=''  AND C.cusno NOT IN ('SGD00088','SCQ00146','SJS00254','SSD00107')
                            AND n.ckind='1R' AND B.dmark1=n.code
                            AND left(convert(varchar(30),A.cfmdate,111),10) >= CONVERT(CHAR(8), dateadd(month,-1,getdate()),111)+'01'
                            AND left(convert(varchar(30),A.cfmdate,111),10) < convert(VARCHAR(100),dateadd(dd,-day(getdate())+1,getdate()),111) ";

            if(args["erp"].Equals("jnerp")){
                sqlstr2 += sqlstr3;
            }
            Fill(String.Format(sqlstr2, args["erp"], args["depno"]), ds, "tbcrmorders");
        }
    }

}
