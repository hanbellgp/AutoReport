using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class QingjiadanConfig : NotificationConfig
    {
        public QingjiadanConfig()
        { 
        }
        public QingjiadanConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new QingjiadanDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT SerialNumber,facno,applyDate,hdn_applyUser,hdn_applyDept,hdn_employee,date1,time1,date2,time2,reason,leaday1 FROM HK_GL004 a");
            sb.Append(" ,ProcessInstance b WHERE a.processSerialNumber = b.serialNumber AND b.currentState = '1' AND a.leaday1 >='2'");
            Fill(sb.ToString(), ds, "qingjiadantbl");
        }
        
    }
}
