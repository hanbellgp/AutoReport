using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{

    public class CDR_R_YwyMxAllConfig : NotificationConfig
    {
        public CDR_R_YwyMxAllConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR_R_YwyMxAll();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
            Updatepros();
            string sqlstr = @"select (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end) as '区域单位','小计'  as '区域负责',itnbrtype as '类别',
 sum(case when right(convert(varchar(6),shpdate,112),2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when right(convert(varchar(6),shpdate,112),2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when right(convert(varchar(6),shpdate,112),2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when right(convert(varchar(6),shpdate,112),2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '12' then shpamts else 0 end) as [12月金额],
 sum(armqy) as '台数合计',sum(shpamts) as '金额合计'
 from cdr_ywymx 
where convert(varchar(4),shpdate,112)=convert(varchar(4),dateadd(month,-1,getdate()),112)
group by (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end),itnbrtype

union all 
select areatype,'小计'  as '区域负责','折让' as '类别',
 0 as [1月台数],   
 sum(case when right(convert(varchar(6),trdat,112),2)= '01' then zrje else 0 end) as [1月金额],      
 0 as [2月台数], 
 sum(case when right(convert(varchar(6),trdat,112),2)= '02' then zrje else 0 end) as [2月金额],      
 0 as [3月台数],  
 sum(case when right(convert(varchar(6),trdat,112),2)= '03' then zrje else 0 end) as [3月金额],      
 0 as [4月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '04' then zrje else 0 end) as [4月金额],
 0 as [5月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '05' then zrje else 0 end) as [5月金额],
0 as [6月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '06' then zrje else 0 end) as [6月金额],
 0 as [7月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '07' then zrje else 0 end) as [7月金额],
 0 as [8月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '08' then zrje else 0 end) as [8月金额],
0 as [9月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '09' then zrje else 0 end) as [9月金额],
0 as [10月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '10' then zrje else 0 end) as [10月金额],
0 as [11月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '11' then zrje else 0 end) as [11月金额],
0 as [12月台数],
 sum(case when right(convert(varchar(6),trdat,112),2)= '12' then zrje else 0 end) as [12月金额],
 0 as '台数合计',sum(zrje) as '金额合计'
from (
select  'ARM232' as type,a.trno,a.trdat,a.zlk,a.hmark1,a.hmark2,
(case a.amtco when 'P' then b.psamts when 'M' then b.psamts*(-1) end) as zrje,(case when b.depno like '1C%' then '济南'
				 when b.depno like '1B%' then '华东'
				 when b.depno like '1D%' then '广州&重庆'
				 when b.depno like '1E%' then '南京'
				 when b.depno like '1F%' then '营销'
	     end) as areatype
from
armpmm a,armacq b  where a.facno=b.facno and  a.trno =b.trno and a.amtco='M' and   a.hmark1 ='R'
union all
select 'ARM423' as type,a.recno,a.recdate,a.zlk,a.hmark1,a.hmark2,b.recamt ,
(case when a.hmark2='HB' then '济南'
				 when a.hmark2='HD' then '华东'
				 when a.hmark2 in ('CQ','HN') then '广州&重庆'
				 when a.hmark2 ='NJ' then '南京'
	     end) as areatype
from
armrech a,armrec b  where a.facno=b.facno  and a.recno=b.recno  and b.raccno='6001' and a.prgno='ARM423' and   a.hmark1 ='R'
union all 
select 'ARM270'as type,bilno,bildat ,rkd,depno,mancode,shpamts*(-1) ,
(case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end) as areatype from
armbil  where  armbil.rkd   = 'RQ11' and   depno in ( '1D000','1B100','1E100','1D100','1C100','1C000','1E000','1B000','1F100','1F000' ) 
union all
select 'ARM235'as type,a.shpno,a.shpdate,b.expdsc,c.cuskind,c.areacode,a.apmamt ,
(case when a.depno like '1C%' then '济南'
				 when a.depno like '1B%' then '华东'
				 when a.depno like '1D%' then '广州&重庆'
				 when a.depno like '1E%' then '南京'
				 when a.depno like '1F%' then '营销'
	     end) as areatype  from
armicdh a,armexp b ,cdrcus c  where a.shpno=b.shpno  and a.decode=b.decode  and a.salecode=b.salecode  and c.cusno=a.cusno and c.cuskind='R'
) a
where convert(varchar(4),trdat,112)=convert(varchar(4),dateadd(month,-1,getdate()),112)
group by  areatype

union all
select areatype,'总计'  as '区域负责','合计',
 sum(case when right(convert(varchar(6),shpdate,112),2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when right(convert(varchar(6),shpdate,112),2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when right(convert(varchar(6),shpdate,112),2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when right(convert(varchar(6),shpdate,112),2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when right(convert(varchar(6),shpdate,112),2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when right(convert(varchar(6),shpdate,112),2)= '12' then shpamts else 0 end) as [12月金额],
 sum(armqy) as '台数合计',sum(shpamts) as '金额合计' 
from (
select (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end) as areatype,armqy,shpamts,shpdate
 from cdr_ywymx 
where convert(varchar(4),shpdate,112)=convert(varchar(4),dateadd(month,-1,getdate()),112)
union all 
select areatype,0 as armqy,zrje as shpamts,shpdate
from (
select  'ARM232' as type,a.trno,a.trdat as shpdate,a.zlk,a.hmark1,a.hmark2,
(case a.amtco when 'P' then b.psamts when 'M' then b.psamts*(-1) end) as zrje,(case when b.depno like '1C%' then '济南'
				 when b.depno like '1B%' then '华东'
				 when b.depno like '1D%' then '广州&重庆'
				 when b.depno like '1E%' then '南京'
				 when b.depno like '1F%' then '营销'
	     end) as areatype
from
armpmm a,armacq b  where a.facno=b.facno and  a.trno =b.trno and a.amtco='M' and   a.hmark1 ='R'
union all
select 'ARM423' as type,a.recno,a.recdate,a.zlk,a.hmark1,a.hmark2,b.recamt ,
(case when a.hmark2='HB' then '济南'
				 when a.hmark2='HD' then '华东'
				 when a.hmark2 in ('CQ','HN') then '广州&重庆'
				 when a.hmark2 ='NJ' then '南京'
	     end) as areatype
from
armrech a,armrec b  where a.facno=b.facno  and a.recno=b.recno  and b.raccno='6001' and a.prgno='ARM423' and   a.hmark1 ='R'
union all 
select 'ARM270'as type,bilno,bildat ,rkd,depno,mancode,shpamts*(-1) ,
(case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end) as areatype from
armbil  where  armbil.rkd   = 'RQ11' and   depno in ( '1D000','1B100','1E100','1D100','1C100','1C000','1E000','1B000','1F100','1F000' ) 
union all
select 'ARM235'as type,a.shpno,a.shpdate,b.expdsc,c.cuskind,c.areacode,a.apmamt ,
(case when a.depno like '1C%' then '济南'
				 when a.depno like '1B%' then '华东'
				 when a.depno like '1D%' then '广州&重庆'
				 when a.depno like '1E%' then '南京'
				 when a.depno like '1F%' then '营销'
	     end) as areatype  from
armicdh a,armexp b ,cdrcus c  where a.shpno=b.shpno  and a.decode=b.decode  and a.salecode=b.salecode  and c.cusno=a.cusno and c.cuskind='R'
) a
where convert(varchar(4),shpdate,112)=convert(varchar(4),dateadd(month,-1,getdate()),112)
)  b
group by areatype

order by (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end)
         ";

            Fill(sqlstr, ds, "CDR_R_YwyMxAll");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["CDR_R_YwyMxAll"].Columns.Remove(ds.Tables["CDR_R_YwyMxAll"].Columns["depnoarea"]);
        //    //ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}

        private int Updatepros()
        {

            DbCommand sqlcomm = CreateDbCommand(this.dbtype);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = this.dbconn;
            sqlcomm.CommandText = "rs_cdrrchuhuo_5";
            if (dbconn.State == ConnectionState.Closed)
            {
                dbconn.Open();
            }
            try
            {
                sqlcomm.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
            }
        }


    }
}
