using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class BOMSQConfig:NotificationConfig
    {
        public BOMSQConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new BOMSQDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT bomnat.itnbrf,bomnat.itnbr,a.itdsc as newitdsc,bomnat.olditnbr,b.itdsc as olditdsc,bomnat.ratio,c.ecnno,s.username, ");
            sb.Append(" (case c.chgkind when  'A' then '新增' when  'B' then '修改' when  'C' then '删除' else '直接替换'  end ) as chgkind, ");
            sb.Append(" bomnat.modifydate from bomnat ");
            sb.Append(" left OUTER JOIN invmas a ON bomnat.itnbr = a.itnbr ");
            sb.Append(" left OUTER JOIN invmas b on bomnat.olditnbr = b.itnbr ");
            sb.Append(" LEFT JOIN bomrec c ON c.ecnno= bomnat.ecnno and c.seqnr=bomnat.seqnr ");
            sb.Append(" LEFT JOIN secuser s on s.userno=c.modifyuser ");
            sb.Append(" INNER JOIN  bomrecd on bomnat.itnbrf=bomrecd.itnbrf and  bomnat.ecnno= bomrecd.ecnno and bomnat.seqnr = bomrecd.seqnr ");
            sb.Append(" WHERE (a.unmsr1<>'米' and b.unmsr1 <> '米') and  CAST(bomnat.ratio AS INT) <> bomnat.ratio ");
            Fill(sb.ToString(), ds, "bomtlb");
        }
        //--
//        SELECT bomnat.itnbrf,bomnat.itnbr,a.itdsc as newitdsc,d.stdqty,bomnat.olditnbr,b.itdsc as olditdsc,bomnat.ratio,c.ecnno,s.username,
//  (case c.chgcode when  'A' then '新增' when  'B' then '修改' when  'C' then '删除' when  'D' then '直接替换' when  'E' then '自然替换'   end ) as chgkind,
//  bomnat.modifydate from bomnat
//  left OUTER JOIN invmas a ON bomnat.itnbr = a.itnbr
//  left OUTER JOIN invmas b on bomnat.olditnbr = b.itnbr
//  LEFT JOIN bomrec c ON c.ecnno= bomnat.ecnno and c.seqnr=bomnat.seqnr
//  LEFT JOIN secuser s on s.userno=c.modifyuser
//  INNER JOIN  bomrecd on bomnat.itnbrf=bomrecd.itnbrf and  bomnat.ecnno= bomrecd.ecnno and bomnat.seqnr = bomrecd.seqnr
//  LEFT JOIN bomasd d on bomnat.itnbrf = d.itnbrf and bomnat.itnbr = d.itnbr
//WHERE (a.unmsr1<>'米' and b.unmsr1 <> '米') and  CAST(bomnat.ratio AS INT) <> bomnat.ratio
        //--


    }
}
