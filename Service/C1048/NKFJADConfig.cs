using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class NKFJADConfig : NotificationConfig
    {


        public NKFJADConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSNKFJAD();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select hzkfjad011,resal002,hzkfjad003,hzkfjad002, resdd007,resak002, "+
        " left( convert(varchar(8),dbo.hzkfjad.CREATE_DATE,112),6) as rq "+
        " from hzkfjad ,resda ,resal,resdd ,resak "+
        " where resda021='1'and resda001='HZKFJAD' AND resda002=hzkfjad002 and dbo.hzkfjad.CREATE_DATE >'2015%' "+
        " and hzkfjad011=resal001 and resdd001='HZKFJAD' and resdd002=hzkfjad002 and resdd015='1' and resdd007= resak001   "+
        " order by resak002 ";

            Fill(sqlstr, ds, "tblresult");

            
        }

        
        

    }
}
