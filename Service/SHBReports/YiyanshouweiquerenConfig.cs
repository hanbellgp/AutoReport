using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class YiyanshouweiquerenConfig : NotificationConfig
    {

        public YiyanshouweiquerenConfig()
        {
        }

        public YiyanshouweiquerenConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new YiyanshouweiquerenDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = "select a.acceptno as '点收编号',a.vdrno as '厂商编号',c.vdrna as '厂商简称',i.itcls as '大类',a.itnbr as '品号',i.itdsc as '品名'," +
             " a.accqy1+a.accqy2 as '点收数量',purach.indate as '点收时间',a.recivedate as '验收日期',a.wareh as '库号',datediff(day,a.recivedate,GETDATE()) as '延误天数','' as '备注' " +
             " from purach,puracd  a " +
             " left join purhad h on a.facno = h.facno and a.prono = h.prono and a.pono = h.pono left join purdtamap m on a.pono=m.pono and a.trseq=m.trseq " +
             " right join purvdr c on a.vdrno=c.vdrno right join invmas i on a.itnbr=i.itnbr " +
             " where a.facno='{0}' and purach.acceptno=a.acceptno and purach.facno=a.facno and a.prono = '1' " +
             " AND a.accsta <> 'W' and a.accsta = 'N'  and h.posrc <> '5'　" +
             " AND Convert(varchar(10),purach.acceptdate,112) <= CONVERT(varchar(10),dateadd(day,-1,getdate()),112)" +
             " AND Convert(varchar(10),a.recivedate,112) <= CONVERT(varchar(10),dateadd(day,-1,getdate()),112)" +
             " union all " +
             " SELECT puracd.acceptno,puracd.vdrno,purvdr.vdrna,invmas.itcls,invmas.itnbr,invmas.itdsc,puracd.accqy1+puracd.accqy2,purach.indate,puracd.recivedate,puracd.wareh,datediff(day,puracd.recivedate,GETDATE()),'' " +
             " FROM purach,puracd left join invmas on invmas.itnbr=puracd.itnbr right join purvdr on puracd.vdrno=purvdr.vdrno,asspurhad " +
             " WHERE purach.facno = puracd.facno  and  purach.prono = puracd.prono and  purach.acceptno = puracd.acceptno and  " +
             " puracd.accsta='N'  AND puracd.facno = asspurhad.facno and puracd.prono = asspurhad.prono and  puracd.pono = asspurhad.pono " +
             " AND  Convert(varchar(10),purach.acceptdate,112) = Convert(varchar(10),dateadd(day,-1,getdate()),112) " +
             " AND Convert(varchar(10),puracd.recivedate,112) = Convert(varchar(10),dateadd(day,-1,getdate()),112) order by recivedate asc";

            Fill(String.Format(sqlstr, args["facno"]), ds, "tblresult");
        }
    }
}
