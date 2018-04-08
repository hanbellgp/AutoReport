using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class CRM_WeiTuiWuLiaoConfig: NotificationConfig
    {
        public CRM_WeiTuiWuLiaoConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCRM_WeiTuiWuLiao();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification,this.ToString());
        }


        public override void InitData()
        {
            
            string sqlstr = @"select TC054,TD001,TD002,TD003,TC007,TC036,TC008,TC009,TC010,TC011 ,TC012 ,
TC016  ,TC017 ,
TC058,TC197 ,TC198 ,TD046,TD047,TD043,
TA004,TA025,TA026,TA028,TA029,TB004,TB005,TB009

from REPTC,REPTD
,(SELECT TB015,TB016,TB017,TA003,TA004,TA039,TA025,TA026,TA028,TA029,TB004,TB005,TB009 FROM WARTA,WARTB
WHERE WARTA.TA001=WARTB.TB001
AND WARTA.TA002=WARTB.TB002
AND TA001<>'CXLL' AND TA001<>'CXTL' 
 AND TA003>='20170701' AND  TA003<getdate() ) AS B
where REPTC.TC001=REPTD.TD001 AND REPTC.TC002=REPTD.TD002
 AND REPTD.TD001=B.TB015 AND REPTD.TD002=B.TB016 AND REPTD.TD003=B.TB017
 and TD046='Y' and TD047='N' and TD043='N' 
AND TC001<>'CXWX'

";

            Fill(sqlstr, ds, "CRM_WeiTuiWuLiao");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}

   

    }
}
