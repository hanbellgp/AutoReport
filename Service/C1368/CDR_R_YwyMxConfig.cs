using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{

    public class CDR_R_YwyMxConfig : NotificationConfig
    {
        public CDR_R_YwyMxConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR_R_YwyMx();
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
	     end) as '区域单位',username  as '区域负责',itnbrtype as '类别',
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
	     end),username,itnbrtype


union all 
select (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end) as '区域单位',username  as '区域负责','小计',
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
	     end),username

order by (case when depno like '1C%' then '济南'
				 when depno like '1B%' then '华东'
				 when depno like '1D%' then '广州&重庆'
				 when depno like '1E%' then '南京'
				 when depno like '1F%' then '营销'
	     end),username 
         ";

            Fill(sqlstr, ds, "CDR_R_YwyMx");


        }

        //移除数据表中多余字段
        public override void ConfigData()
        {
            ds.Tables["CDR_R_YwyMx"].Columns.Remove(ds.Tables["CDR_R_YwyMx"].Columns["depnoarea"]);
            //ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        }

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
