using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class PurvdrxgbConfig : NotificationConfig
    {
        public PurvdrxgbConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSPurvdrxgb();
            this.reportList.Add(new PurvdrxgbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select purvdr.CREATE_DATE as sqrq,sqr,sqrC,sqbm,sqbmC,vdrno,vdrna,zvdrds,"+
            " case kind when '01' then N'原料类厂商' when '02' then N'费用类厂商' when '03' then N'工程类厂商' "+
            " when '04' then N'设备类厂商'when '9' then N'未定义'else '' end AS kind ,"+
            " vdrds ,ofkfs,address ,ttbankno ,ttbanknum,uniform ," +
            " contactman ,tel1 ,tel2 ,fax1,qt,nvdrds ,fkfs ,qtskfsc,naddress,nttbankno ,nttbanknum ,nuniform ,ncontactman,ntel1 ,ntel2 ," +
            " nfax1 ,nqt,why "+
            " from purvdr ";

            Fill(sqlstr, ds, "tblresult");


        }

       

    }
}
