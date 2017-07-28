using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class PGTBBConfig : NotificationConfig
    {


        public PGTBBConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSPgtbb();
            this.reportList.Add(new PgtbbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }

        

        public override void InitData()
        {
            string sqlstr = @"select pb01_1002, datetime1,empl1,empl1C,resal002, "+
           "  case radio1 when '4' then N'配管图'  else N'' end AS radio1, "+
           " case radio3 when '0' then N'制定'when '1' then N'废止'when '2' then N'修订'when '3' then N'复制' "+
           "  when '4' then N'原稿借出'else N'提供电子档'end AS radio3,textarea1,text17,text12,text19  "+
           "  from pb01_1,pb01_2 ,resda,resak,resal "+
           "  where pb01_1001=pb01_2001 and pb01_1002=pb01_2002 and resda001='PB01_1 ' and resda002=pb01_1002 and resda002=pb01_2002   "+
           "  and resda021='2' and radio1='4' and resak001=empl1 and resal001=resak015 and DateDiff(month,datetime1,getdate())=1  ";

            Fill(sqlstr, ds, "result");

            
        }

        
    }
}
