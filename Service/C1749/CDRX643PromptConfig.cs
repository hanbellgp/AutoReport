using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class CDRX643PromptConfig : NotificationConfig
    {
        public CDRX643PromptConfig() 
        { 
        
        }
        public CDRX643PromptConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CDRX643DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            String sqlStr = @"select c.facno as facno,c.abntime as abntime,c.abncolname as abncolname,c.abntrno as abntrno,c.trseq as trseq,
                            c.abntype as abntype,c.prgno as prgno,c.abncoldsc as abncoldsc,c.olddata as olddata,c.newdata as newdata,c.userno as userno,c.mailyn as mailyn 
                              from cdrabn c where prgno = 'CDR643' and olddata <> newdata";
            Fill(sqlStr, ds, "cdr643");
        }
    }
}
