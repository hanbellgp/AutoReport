using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class HetongshenheConfig : NotificationConfig
    {
        public HetongshenheConfig()
        { 
        }
        public HetongshenheConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new HetongshenheDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.facno,a.appuser,a.dept,a.appdate,a.kind3,a.sealfacno,a.anyone,a.contractno,a.contractdate,a.contractday,a.contractaddress");
            sb.Append(" ,a.fp,a.fpperson,a.fpphone,a.fpmail,a.fpaddress,a.fpbank,a.fpbankno,a.sp,a.spperson,a.spphone,a.spmail,a.spaddress,a.spbank,a.spbankno");
            sb.Append(" ,a.tp,a.tpperson,a.tpphone,a.tpmail,a.tpaddress,a.tpbank,a.tpbankno,a.ffp,a.ffpperson,a.ffpphone,a.ffpmail,a.ffpaddress,a.ffpbank,a.ffpbankno");
            sb.Append(" ,a.coin,a.warrantydate,a.warrantyyear,a.paykind,a.ifpay,a.contractmoney,a.projectname,a.begindate,a.enddate,a.chooseaddress,a.projectaddress");
            sb.Append(" ,a.content,a.processSerialNumber,ProcessInstance.currentState from HK_JH001 a,ProcessInstance");
            sb.Append(" WHERE a.processSerialNumber = ProcessInstance.serialNumber");
            sb.Append(" and appdate>=CONVERT(varchar(7), dateadd(mm,-1,getdate()) , 120) + '-1'");
            sb.Append(" AND appdate<=convert(varchar(10),cast(convert(varchar(8),getdate(),120) + '01' as datetime) - 1,120) ORDER BY appdate");
            Fill(sb.ToString(), ds, "tlb");
        }
    }
}
