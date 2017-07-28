using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class CdrcusxgbConfig : NotificationConfig
    {
        public CdrcusxgbConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSPurvdrxgb();
            this.reportList.Add(new CdrcusxgbReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select cdrcus.CREATE_DATE as sqrq,sqr ,sqrC ,sqbm ,sqbmC,zcusds ,cusno,cusna ,"+
        " case jxb when 'R' then N'冷媒产品' when 'AH' then N'机体' when 'AA' then N'机组' when 'AW' then N'无油机组'"+
        " when 'P' then N'真空'when 'CM' then N'离心式机组,热泵ODM'when 'FW' then N'服务'else '' end AS jxb,"+
        " cusds ,oskfs,address ,cusbakna ,cusacctno,uniform ,contactman ,tell ,fax ,qt,ncusds,skfs ,qtskfsc ,"+
        " naddress ,ncusbakna ,ncusacctno ,nuniform ,"+
        " ncontactman ,ntell ,nfax ,nqt,why from cdrcus";

            Fill(sqlstr, ds, "tblresult");


        }



    }
}
