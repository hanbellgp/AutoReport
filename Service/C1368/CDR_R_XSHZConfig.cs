using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    
    public class CDR_R_XSHZConfig : NotificationConfig
    {
        public CDR_R_XSHZConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR_R_XSHZ();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification,this.ToString());
        }


        public override void InitData()
        {
            Updatepros();
            string sqlstr = @"select left(shpmonq,4),protype,
 sum(case when right(shpmonq,2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when right(shpmonq,2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when right(shpmonq,2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when right(shpmonq,2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when right(shpmonq,2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when right(shpmonq,2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when right(shpmonq,2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when right(shpmonq,2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when right(shpmonq,2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when right(shpmonq,2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when right(shpmonq,2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when right(shpmonq,2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when right(shpmonq,2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when right(shpmonq,2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when right(shpmonq,2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when right(shpmonq,2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when right(shpmonq,2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when right(shpmonq,2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when right(shpmonq,2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when right(shpmonq,2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when right(shpmonq,2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when right(shpmonq,2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when right(shpmonq,2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when right(shpmonq,2)= '12' then shpamts else 0 end) as [12月金额],
 sum(armqy) as '台数合计',sum(shpamts) as '金额合计'
 from (SELECT distinct b.* FROM cdr_rs_rflzb b,
                  (select  *from cdr_rs_rflzb   where left(shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112)   group by shpmonq,protype ) a  
where right(b.shpmonq,2)=right(a.shpmonq,2)) a
where (left(shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112)
or left(shpmonq,4)=convert(varchar(4),dateadd(month,-13,getdate()),112))
group by left(shpmonq,4),protype
union all 
select left(shpmonq,4),'小计',
 sum(case when right(shpmonq,2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when right(shpmonq,2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when right(shpmonq,2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when right(shpmonq,2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when right(shpmonq,2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when right(shpmonq,2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when right(shpmonq,2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when right(shpmonq,2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when right(shpmonq,2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when right(shpmonq,2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when right(shpmonq,2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when right(shpmonq,2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when right(shpmonq,2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when right(shpmonq,2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when right(shpmonq,2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when right(shpmonq,2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when right(shpmonq,2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when right(shpmonq,2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when right(shpmonq,2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when right(shpmonq,2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when right(shpmonq,2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when right(shpmonq,2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when right(shpmonq,2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when right(shpmonq,2)= '12' then shpamts else 0 end) as [12月金额],
 sum(armqy) as '台数合计',sum(shpamts) as '金额合计'
 from (SELECT distinct b.* FROM (SELECT shpmonq,sum(armqy) as armqy,sum(shpamts) as shpamts FROM cdr_rs_rflzb  group by shpmonq ) b,
(select  *from cdr_rs_rflzb   where left(shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112)   group by shpmonq,protype ) a  
where right(b.shpmonq,2)=right(a.shpmonq,2)) a
where (left(shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112)
or left(shpmonq,4)=convert(varchar(4),dateadd(month,-13,getdate()),112))
group by left(shpmonq,4)
order by left(shpmonq,4) desc
         ";

            Fill(sqlstr, ds, "CDR_R_XSHZ");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}

        private int Updatepros()
        {

            DbCommand sqlcomm = CreateDbCommand(this.dbtype);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = this.dbconn;
            sqlcomm.CommandText = "rs_cdrrchuhuo_7";
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
