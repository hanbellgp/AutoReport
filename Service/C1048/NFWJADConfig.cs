using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class NFWJADConfig : NotificationConfig
    {


        public NFWJADConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSNFWJAD();
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select  "+
        " hzfwjad009,resal002,hzfwjad002,hzfwjad003,hzfwd004,resdd007,resak002,LEFT (CONVERT(varchar(8),hzfwjad.CREATE_DATE,112),6) as rq  "+
        " from hzfwjad,resda,resal,resdd,resak,hzfwd "+
        " where resda021='1' and resda001='HZFWJAD' and resda002=hzfwjad002 and hzfwjad.CREATE_DATE>'2015%' "+
        " and hzfwjad009=resal001 and resdd001='HZFWJAD' and resdd002=hzfwjad002 and resdd015='1' and resdd007=resak001  " +
        " and hzfwjad003=hzfwd006 order by resdd007 ";

            Fill(sqlstr, ds, "tblresult");

            
        }

        
        

    }
}
