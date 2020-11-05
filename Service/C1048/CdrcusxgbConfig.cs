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
        //    string sqlstr = @"select cdrcus.CREATE_DATE as sqrq,sqr ,sqrC ,sqbm ,sqbmC,zcusds ,cusno,cusna ,"+
        //" case jxb when 'R' then N'冷媒产品' when 'AH' then N'机体' when 'AA' then N'机组' when 'AW' then N'无油机组'"+
        //" when 'P' then N'真空'when 'CM' then N'离心式机组,热泵ODM'when 'FW' then N'服务'else '' end AS jxb,"+
        //" cusds ,oskfs,address ,cusbakna ,cusacctno,uniform ,contactman ,tell ,fax ,qt,ncusds,skfs ,qtskfsc ,"+
        //" naddress ,ncusbakna ,ncusacctno ,nuniform ,"+
        //" ncontactman ,ntell ,nfax ,nqt,why from cdrcus";

            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(" select createdate,substring(hdn_sqr,1,5) as appuser,substring(hdn_sqr,7,3) as appusername,substring(hdn_sqbm,1,5) as deptno,substring(hdn_sqbm,7,10) as dept, ");
            sqlstr.Append(" cusno,cusna,title,cuskind ,bcusds,bskfs,baddress,bcusbakna,bcusacctno,buniform,bcontactman,btel1,bfax,bman,bqt, ");
            sqlstr.Append(" cusds, ");
            sqlstr.Append(" (case skfs when '01' then N'款到发货' when '02' then N'发票到后15天' when '03' then N'发票到后30天' when '04' then N'发票到后60天' ");
            sqlstr.Append(" when 'A1' then N'A1签约后30%出货前30%验收30%质保10%' when 'A2' then N'A2签约后30%到货后30%验收30%质保10%' when 'A3' then N'A3签约后30%出货前30%验收40%' ");
            sqlstr.Append(" when 'A4' then N'A4预付30%～70%，验收10日内支付剩余余款' when 'A5' then N'预付30%～70%，验收10日内支付剩余余款，质保一年5%' ");
            sqlstr.Append(" when 'A6' then N'A6合同签订10天预付20%，出货前80%' when 'A7' then N'A7合同签订10天预付30%，出货后60天支付70%' when 'A8' then N'A8合同签订10天预付30%，出货15天验收后30天支付70%' ");
            sqlstr.Append(" when 'A9' then N'A9年度保养合同，预收50%，验收后45%质保1年5%' when 'QT' then 'QT其他' ");
            sqlstr.Append(" end ) as skfs,qtskfs,address,cusbakna,cusacctno,uniform,contactman,tel1,fax,man,qt,explain ");
            sqlstr.Append(" from HK_YX007 where convert(VARCHAR(4),createdate,112)>=convert(VARCHAR(4),getdate(),112) ORDER BY createdate ");
            Fill(sqlstr.ToString(), ds, "tblresult");
        }



    }
}
