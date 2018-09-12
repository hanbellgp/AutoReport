using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{

    public class CDR_R_KehuYXSConfig : CDR_R_KehuNHZConfig
    {
        public CDR_R_KehuYXSConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR_R_KehuNHZ();
            this.args = Base.GetParameter(notification,this.ToString());
        }


        public override void InitData()
        {
            Updatepros();
            String sqlstr = "select protype,itnbrcus,cusna,sum(armqy) as qty,sum(shpamts) as amts  from cdr_rs_rqymx " +
            //" where left(depno,2) in ('{0}') and convert(varchar(6),shpmonq,112)=convert(varchar(6),dateadd(month,-13,getdate()),112)   " +          //去年的这个月的上个月的数据
            " where left(depno,2) in ('{0}') and convert(varchar(6),shpmonq,112)=convert(varchar(6),dateadd(month,-1,getdate()),112)   " +            //今年的这个月的上个月的数据
            " group by protype,itnbrcus,cusna  order by protype,cusna ";

            Fill(String.Format(sqlstr, args["depno"]), ds, "CDR_R_KehuNHZ");

        }

    }
}