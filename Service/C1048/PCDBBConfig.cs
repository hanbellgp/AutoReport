using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class PCDBBConfig : NotificationConfig
    {


        public PCDBBConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSPCDBB();
            this.reportList.Add(new PcdbbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select noamc002,noatk002,  "+
               " left( convert(varchar(8),noamc025,112),8) as rq, "+
               " noamc012,noatk023,noatk007,noatk008,noatk009,noatk010,noamc013,noamc014, "+
               " left( convert(varchar(8),noamc026,112),8) as hcrq,noamc015 ,noamc005,noamc003 " +
               "  from noamc LEFT JOIN resda on resda002=noamc002 and resda001=noamc001,noatk  " +
               "  where noatk030=noamc002 and  resda021='2' and resda021 is not Null and resda001='CARE008'  ";

            Fill(sqlstr, ds, "DSpcdbb");

            
        }

        
    }
}
