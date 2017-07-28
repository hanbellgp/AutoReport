using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class NFWLLDConfig : NotificationConfig
    {


        public NFWLLDConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSNFWLLD();
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select "+
        " hzfwlld009,resal002,hzfwlld002,hzfwlld004,hzfwlld003,resdd007,resak002,LEFT (CONVERT(varchar(8),hzfwlld.CREATE_DATE,112),6) as rq   " +
        " from hzfwlld ,resda,resal,resdd,resak "+
        " where   resda001='HZFWLLD' and resda002=hzfwlld002 and hzfwlld.CREATE_DATE>'2015%' "+
        " and hzfwlld009=resal001 and resdd001='HZFWJAD' and resdd002=hzfwlld002 and resdd015='1' and resdd007=resak001 " +
          "   order by resdd007";

            Fill(sqlstr, ds, "tblresult");

            
        }

        
        

    }
}
