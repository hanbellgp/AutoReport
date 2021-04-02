using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class JiechangshangbaobiaoConfig_K : NotificationConfig
    {
        public JiechangshangbaobiaoConfig_K()
        { 
        }
        public JiechangshangbaobiaoConfig_K(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new JiechangshangbaobiaoDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select distinct a.facno , '厂商' as '借出类别',a.trno,a.itnbr,s.itdsc, a.trdate,a.prebkdate,b.brtrno,a.cusno,r.vdrna,a.wareh,w.whdsc,");
            sb.Append("a.cp_trnqy1,a.cp_trnqy2,a.cp_retqy1,a.cp_retqy2,a.cp_trnqy1-a.cp_retqy1-a.cp_saleqy1 as '未归还数1',");
            sb.Append("a.cp_trnqy2-a.cp_retqy2-a.cp_saleqy2 as '未归还数2',s.unmsr2,s.unmsr1,a.fixnr,a.varnr,a.hadstatus,a.dtastatus,a.resno, ");
            sb.Append("m.cdesc,a.headperson,u.username,a.hmark02,a.depno,d.depname,a.userno,x.username ");
            sb.Append("from (SELECT  cdrlndta.facno as 'facno',   cdrlndta.trno as 'trno',   cdrlndta.itnbr as 'itnbr',   cdrlndta.trdate as 'trdate', ");
            sb.Append("cdrlndta.prebkdate as 'prebkdate',     ");
            sb.Append("sum(case substring(invmas.judco,2,1) when '1' then trnqy1 when '3'  then trnqy1 * invmas.rate2  else trnqy1 end) as cp_trnqy1, ");
            sb.Append("sum(trnqy2) as cp_trnqy2, ");
            sb.Append("sum(case substring(invmas.judco,2,1) when '1' then retqy1 when '3'  then retqy1 * invmas.rate2  else retqy1 end) as cp_retqy1,");
            sb.Append("sum(retqy2) as cp_retqy2, ");
            sb.Append("sum(case substring(invmas.judco,2,1) when '1' then saleqy1 when '3'  then saleqy1 * invmas.rate2  else saleqy1 end) as cp_saleqy1,");
            sb.Append("sum(saleqy2) as cp_saleqy2,   cdrlndta.fixnr as 'fixnr',   cdrlndta.varnr as 'varnr',   cdrlndta.wareh as  'wareh', ");
            sb.Append("cdrlndta.status as 'dtastatus', cdrlnhad.status as 'hadstatus',    cdrlnhad.cusno as 'cusno',   cdrlnhad.headperson as 'headperson',");
            sb.Append("cdrlnhad.resno as 'resno',cdrlnhad.hmark02 as 'hmark02',cdrlnhad.userno as 'userno' ,cdrlnhad.depno ");
            sb.Append("FROM cdrlndta left join invmas on cdrlndta.itnbr=invmas.itnbr,cdrlnhad  ");
            sb.Append("WHERE ( cdrlnhad.facno = cdrlndta.facno ) and  ( cdrlnhad.trno = cdrlndta.trno )   ");
            sb.Append("AND (cdrlndta.facno = '{0}' ");
            sb.Append("and cdrlnhad.status <> 'N' and cdrlnhad.status <> 'W'");
            sb.Append("and cdrlndta.status <>'W' and cdrlndta.status<>'C' and cdrlnhad.objtype='PJ')");
            sb.Append("GROUP BY cdrlndta.facno,   cdrlndta.trno,   cdrlnhad.trno,   cdrlndta.itnbr,   cdrlndta.trdate,   cdrlndta.prebkdate,   cdrlnhad.cusno, ");
            sb.Append("cdrlndta.fixnr,   cdrlndta.varnr,  ");
            sb.Append("cdrlndta.wareh,cdrlndta.status,cdrlnhad.status,cdrlnhad.headperson,cdrlnhad.resno,cdrlnhad.hmark02,cdrlnhad.userno,cdrlnhad.depno) a ");
            sb.Append("left join cdrbrdta b on b.facno=a.facno and  b.lntrno=a.trno and b.itnbr=a.itnbr and b.varnr=a.varnr ");
            sb.Append("left join secuser u on u.userno=a.headperson ");
            sb.Append("left join secuser x on x.userno=a.userno,invmas s,miscode m , purvdr r,invwh w,misdept d ");
            sb.Append("where a.itnbr=s.itnbr  and ( m.ckind='IL' and m.code=a.resno )  ");
            sb.Append("and((a.cp_trnqy1-a.cp_retqy1-a.cp_saleqy1>0 )  or (a.cp_trnqy2-a.cp_retqy2-a.cp_saleqy2>0))");
            sb.Append("and r.vdrno=a.cusno and a.wareh=w.wareh and d.depno=a.depno");

            Fill(String.Format(sb.ToString(), args["facno"]), ds, "tblresult3");
        }
    }
}
