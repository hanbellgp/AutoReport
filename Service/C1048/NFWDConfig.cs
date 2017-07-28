using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class NFWDConfig : NotificationConfig
    {


        public NFWDConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSNFWD();
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"Select  "+
            " hzfwd008,resal002,hzfwd002,hzfwd006,hzfwd004,resdd007,resak002, "+
            " LEFT (CONVERT(varchar(8),hzfwd.CREATE_DATE,112),6) as rq  "+
            " from hzfwd ,resda,resal ,resdd,resak "+
            " where resda021='1' and resda001='HZFWD' and resda002=hzfwd002 and hzfwd.CREATE_DATE>'2015%' "+
            " and hzfwd008=resal001 and resdd001='HZFWD' and resdd002=hzfwd002 and resdd015='1' and resdd007=resak001  " +
            " order by resdd007 ";

            Fill(sqlstr, ds, "tblresult");

            
        }

        
        

    }
}
