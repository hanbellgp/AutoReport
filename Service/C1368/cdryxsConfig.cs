using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class cdryxsConfig : NotificationConfig
    {
        public cdryxsConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DScdryxs();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification,this.ToString());
        }


        public override void InitData()
        {
            
            if( Updatepros()==0 )
            {
                return;
            }
            string sqlstr = @"select b.* ,
[1月台数]+[2月台数]+[3月台数]+[4月台数]+[5月台数]+[6月台数]+[7月台数]+[8月台数]+[9月台数]+[10月台数]+[11月台数]+[12月台数] as '台数合计',
[1月金额]+[2月金额]+[3月金额]+[4月金额]+[5月金额]+[6月金额]+[7月金额]+[8月金额]+[9月金额]+[10月金额]+[11月金额]+[12月金额]  as '金额合计'
from (select protype,itnbrcus,
  sum(case when  right(shpmonq,2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when  right(shpmonq,2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when  right(shpmonq,2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when  right(shpmonq,2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when  right(shpmonq,2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when  right(shpmonq,2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when  right(shpmonq,2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when  right(shpmonq,2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when  right(shpmonq,2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when  right(shpmonq,2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when  right(shpmonq,2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when  right(shpmonq,2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when  right(shpmonq,2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when  right(shpmonq,2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when  right(shpmonq,2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when  right(shpmonq,2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when  right(shpmonq,2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when  right(shpmonq,2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when  right(shpmonq,2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when  right(shpmonq,2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when  right(shpmonq,2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when  right(shpmonq,2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when  right(shpmonq,2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when  right(shpmonq,2)= '12' then shpamts else 0 end) as [12月金额]
 from (select shpmonq,protype,itnbrcus,cusna, sum(armqy)as armqy ,sum(shpamts) as shpamts
from cdr_rs_rqymx 
where left(depno,2) in ('{0}') 
and left (shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112) 
group by shpmonq,protype,itnbrcus,cusna) a
group by protype,itnbrcus) b
where [1月台数]+[2月台数]+[3月台数]+[4月台数]+[5月台数]+[6月台数]+[7月台数]+[8月台数]+[9月台数]+[10月台数]+[11月台数]+[12月台数]<>0
union all
select c.protype ,'小计',c.[1月台数],c.[1月金额],c.[2月台数],c.[2月金额],c.[3月台数],c.[3月金额],c.[4月台数],c.[4月金额],c.[5月台数],c.[5月金额],c.[6月台数],c.[6月金额],c.[7月台数],c.[7月金额],
c.[8月台数],c.[8月金额],c.[9月台数],c.[9月金额],c.[10月台数],c.[10月金额],c.[11月台数],c.[11月金额],c.[12月台数],c.[12月金额],
[1月台数]+[2月台数]+[3月台数]+[4月台数]+[5月台数]+[6月台数]+[7月台数]+[8月台数]+[9月台数]+[10月台数]+[11月台数]+[12月台数] as '台数合计',
[1月金额]+[2月金额]+[3月金额]+[4月金额]+[5月金额]+[6月金额]+[7月金额]+[8月金额]+[9月金额]+[10月金额]+[11月金额]+[12月金额]  as '金额合计'
from (select protype ,
 sum(case when  right(shpmonq,2)= '01' then armqy else 0 end) as [1月台数],   
 sum(case when  right(shpmonq,2)= '01' then shpamts else 0 end) as [1月金额],      
 sum(case when  right(shpmonq,2)= '02' then armqy else 0 end) as [2月台数], 
 sum(case when  right(shpmonq,2)= '02' then shpamts else 0 end) as [2月金额],      
 sum(case when  right(shpmonq,2)= '03' then armqy else 0 end) as [3月台数],  
 sum(case when  right(shpmonq,2)= '03' then shpamts else 0 end) as [3月金额],      
 sum(case when  right(shpmonq,2)= '04' then armqy else 0 end) as [4月台数],
 sum(case when  right(shpmonq,2)= '04' then shpamts else 0 end) as [4月金额],
 sum(case when  right(shpmonq,2)= '05' then armqy else 0 end) as [5月台数],
 sum(case when  right(shpmonq,2)= '05' then shpamts else 0 end) as [5月金额],
 sum(case when  right(shpmonq,2)= '06' then armqy else 0 end) as [6月台数],
 sum(case when  right(shpmonq,2)= '06' then shpamts else 0 end) as [6月金额],
 sum(case when  right(shpmonq,2)= '07' then armqy else 0 end) as [7月台数],
 sum(case when  right(shpmonq,2)= '07' then shpamts else 0 end) as [7月金额],
 sum(case when  right(shpmonq,2)= '08' then armqy else 0 end) as [8月台数],
 sum(case when  right(shpmonq,2)= '08' then shpamts else 0 end) as [8月金额],
 sum(case when  right(shpmonq,2)= '09' then armqy else 0 end) as [9月台数],
 sum(case when  right(shpmonq,2)= '09' then shpamts else 0 end) as [9月金额],
 sum(case when  right(shpmonq,2)= '10' then armqy else 0 end) as [10月台数],
 sum(case when  right(shpmonq,2)= '10' then shpamts else 0 end) as [10月金额],
 sum(case when  right(shpmonq,2)= '11' then armqy else 0 end) as [11月台数],
 sum(case when  right(shpmonq,2)= '11' then shpamts else 0 end) as [11月金额],
 sum(case when  right(shpmonq,2)= '12' then armqy else 0 end) as [12月台数],
 sum(case when  right(shpmonq,2)= '12' then shpamts else 0 end) as [12月金额]
 from (select shpmonq,protype,sum(armqy)as armqy ,sum(shpamts) as shpamts
from cdr_rs_rqymx 
where left(depno,2) in ('{0}')
and left (shpmonq,4)=convert(varchar(4),dateadd(month,-1,getdate()),112) 
group by shpmonq,protype) a
group by protype) c

order by b.protype
";

            Fill(String.Format(sqlstr, args["depno"]), ds, "cdryxs");


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
