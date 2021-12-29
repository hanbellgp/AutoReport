using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class YidianshouweiyanshouConfig_K : NotificationConfig
    {
        public YidianshouweiyanshouConfig_K()
        { 
        }
        public YidianshouweiyanshouConfig_K(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new Yidianshouweiyanshou_KDS();

            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.acceptno as '点收编号',a.vdrno as '厂商编号',c.vdrna as '厂商简称',i.itcls as '大类',a.itnbr as '品号',i.itdsc as '品名', ");
            sb.Append("a.accqy1+a.accqy2 as '点收数量',a.indate as '点收时间',datediff(hour,a.indate,GETDATE()) as '延误小时数','' as '备注'  ");
            sb.Append("from purach,puracd  a  ");
            sb.Append("left join purhad h on a.facno = h.facno and a.prono = h.prono and a.pono = h.pono left join purdtamap m on a.pono=m.pono and a.trseq=m.trseq ");
            sb.Append("right join purvdr c on a.vdrno=c.vdrno right join invmas i on a.itnbr=i.itnbr  ");
            sb.Append("where a.facno='{0}' and purach.acceptno=a.acceptno and purach.facno=a.facno and a.prono = '1'  ");
            sb.Append("and i.itcls in {1}  ");
            sb.Append("AND a.accsta <> 'W' and a.accsta = 'R'  and h.posrc <> '5'　 ");
            sb.Append("union all  ");
            sb.Append("SELECT puracd.acceptno,puracd.vdrno,purvdr.vdrna,invmas.itcls,invmas.itnbr,invmas.itdsc,puracd.accqy1+puracd.accqy2,purach.indate,datediff(hour,purach.indate,GETDATE()),''  ");
            sb.Append("FROM purach,puracd left join invmas on invmas.itnbr=puracd.itnbr right join purvdr on puracd.vdrno=purvdr.vdrno,asspurhad  ");
            sb.Append("WHERE purach.facno = puracd.facno  and  purach.prono = puracd.prono and  purach.acceptno = puracd.acceptno and   ");
            sb.Append("puracd.accsta='R'  AND puracd.facno = asspurhad.facno and puracd.prono = asspurhad.prono and  puracd.pono = asspurhad.pono ");
            sb.Append("AND  Convert(varchar(6),purach.acceptdate,112) = Convert(varchar(6),getdate(),112)");

            Fill(String.Format(sb.ToString(), args["facno"], args["itcls"]), this.ds, "tblresult7");
        }
    }
}
