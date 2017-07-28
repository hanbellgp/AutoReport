using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class JZchmxbConfig : NotificationConfig
    {

        public JZchmxbConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSJZchmxb();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {

            string sqlstr = @"select     d.itnbr,s.itdsc,s.spdsc2,  " +
        " c.cusna as 'cusna' ,   "+
        " convert(char(6),h.shpdate,112)  as 'yearmon',  "+
        " cast ((d.shpamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
        " cast (d.shpqy1 as decimal(16,2)) as 'shpqy' "+
        " from [test].[dbo].cdrdta d  "+
        " left join [test].[dbo].invmas s on s.itnbr=d.itnbr  "+
        " left join [test].[dbo].cdrhad h  "+
        " on  d.facno=h.facno   "+
        " and d.shpno=h.shpno ,[test].[dbo].cdrcus c   "+
        " where h.facno = 'C'   "+
        " and c.cusno=h.cusno  "+
        " and h.houtsta <> 'W'  "+
        " and h.depno in ('1E000','1E100','1B000','1D000','1D100','1C000','1C100','1G120','1N120','1B100','1D100','1Q000')   "+
        " and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328','SGD00263')   " +
        " and d.itnbr in(select itnbr from [test].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80'))  "+
        " and DateDiff(year,h.shpdate,getdate())=0 "+
        " union all  "+
        " select d.itnbr,s.itdsc,s.spdsc2, "+
        " c.cusna , "+
        " convert(char(6),h.bakdate,112)  as 'yearmon',  "+
        " -1*cast ((d.bakamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
        " -1*cast (d.bshpqy1 as decimal(16,2)) as 'shpqy' "+ 
        " from [test].[dbo].cdrbhad h  "+
        " right join [test].[dbo].cdrbdta d  "+
        " left join [test].[dbo].invmas s on s.itnbr=d.itnbr  "+
        " on h.bakno=d.bakno , [test].[dbo].cdrcus c  "+
        " where h.facno = 'C'  "+
        " and h.baksta<>'W'  "+
        " and c.cusno=h.cusno  "+
        " and h.depno in ('1E000','1E100','1B000','1D000','1D100','1C000','1C100','1G120','1N120','1B100','1D100','1Q000')  "+
        " and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328','SGD00263')  "+
        " and d.itnbr in(select itnbr from [test].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80'))    "+
        " and DateDiff(year,h.bakdate,getdate())=0 "+
        " union all  "+
        " select    d.itnbr,s.itdsc,s.spdsc2, "+
        " c.cusna as 'cusna' ,   "+
        " convert(char(6),h.shpdate,112)  as 'yearmon',  "+
        " cast ((d.shpamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts',  "+
        " cast (d.shpqy1 as decimal(16,2)) as 'shpqy'  "+
        " from [njerp].[dbo].cdrdta d  "+
        " left join [njerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
        " inner join [njerp].[dbo].cdrhad h  "+
        " on  d.facno=h.facno   "+
        " and d.shpno=h.shpno ,[njerp].[dbo].cdrcus c   "+
        " where h.facno = 'N'    "+
        " and c.cusno=h.cusno  "+
        " and h.houtsta <> 'W'   "+
        " and h.depno in('1E000')  "+
        " and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328')  "+
        " and d.itnbr in(select itnbr from [njerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
        " and DateDiff(year,h.shpdate,getdate())=0  "+
        " union all  "+
        " select    d.itnbr,s.itdsc,s.spdsc2, "+
        " c.cusna , "+
       "  convert(char(6),h.bakdate,112)  as 'yearmon',  "+
        " -1*cast ((d.bakamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
        " -1*cast (d.bshpqy1 as decimal(16,2)) as 'shpqy'  "+
        " from [njerp].[dbo].cdrbhad h  "+
        " right join [njerp].[dbo].cdrbdta d "+
        " left join [njerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
        " on h.bakno=d.bakno , [njerp].[dbo].cdrcus c  "+
        " where     h.facno = 'N'  "+
        " and h.baksta<>'W'  "+
        " and c.cusno=h.cusno  "+
        " and h.depno in('1E000')  "+
        " and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328')  "+
        " and d.itnbr in(select itnbr from [njerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
       "  and DateDiff(year,h.bakdate,getdate())=0  "+
        " union all  "+
        " select    d.itnbr,s.itdsc,s.spdsc2, "+
        " c.cusna as 'cusna' ,   "+
       "  convert(char(6),h.shpdate,112)  as 'yearmon',  "+
        " cast ((d.shpamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts',  "+
        " cast (d.shpqy1 as decimal(16,2)) as 'shpqy'  "+
        " from [jnerp].[dbo].cdrdta d  "+
        " left join [jnerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
        " inner join [jnerp].[dbo].cdrhad h  "+
        " on  d.facno=h.facno  and d.shpno=h.shpno ,[jnerp].[dbo].cdrcus c   "+
        " where h.facno = 'J'    "+
        " and c.cusno=h.cusno  "+
        " and h.houtsta <> 'W'   "+
        " and h.depno in ('1C100')   "+
        " and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328')  "+
        " and d.itnbr in(select itnbr from [jnerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
        " and DateDiff(year,h.shpdate,getdate())=0   "+
        " union all  "+
        " select    d.itnbr,s.itdsc,s.spdsc2, "+
        " c.cusna , "+
        " convert(char(6),h.bakdate,112)  as 'yearmon',  "+
       "  -1*cast ((d.bakamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
       "  -1*cast (d.bshpqy1 as decimal(16,2)) as 'shpqy'  "+
       "  from [jnerp].[dbo].cdrbhad h  "+
       "  right join [jnerp].[dbo].cdrbdta d  "+
       "  left join [jnerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
       "  on h.bakno=d.bakno , [jnerp].[dbo].cdrcus c  "+
       "  where h.facno = 'J' and h.baksta<>'W'  "+
       "  and c.cusno=h.cusno and h.depno in ('1C100')  "+
       "  and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328' )  "+
       "  and d.itnbr in(select itnbr from [jnerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
       "  and DateDiff(year,h.bakdate,getdate())=0   "+
       "  union all  "+
       "  select    d.itnbr,s.itdsc,s.spdsc2, "+
       "  c.cusna as 'cusna' ,   "+
       "  convert(char(6),h.shpdate,112)  as 'yearmon',  "+
       "  cast ((d.shpamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
       "  cast (d.shpqy1 as decimal(16,2)) as 'shpqy'  "+
      "   from [gzerp].[dbo].cdrdta d  "+
      "   left join [gzerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
      "   inner join [gzerp].[dbo].cdrhad h  "+
      "   on  d.facno=h.facno   "+
      "   and d.shpno=h.shpno ,[gzerp].[dbo].cdrcus c   "+
       "  where h.facno = 'G'   "+
      "   and c.cusno=h.cusno  "+
      "   and h.houtsta <> 'W'   "+
       "  and h.depno in('1D100')   "+
      "   and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328')  "+
       "  and d.itnbr in(select itnbr from [gzerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
       "  and DateDiff(year,h.shpdate,getdate())=0  "+
       "  union all  "+
       "  select    d.itnbr,s.itdsc,s.spdsc2, "+
       "  c.cusna , "+
       "  convert(char(6),h.bakdate,112)  as 'yearmon',  "+
       "  -1*cast ((d.bakamts * h.ratio/(1+h.taxrate)) as decimal(16,2)) as 'shpamts', "+
       "  -1*cast (d.bshpqy1 as decimal(16,2)) as 'shpqy'  "+
       "  from [gzerp].[dbo].cdrbhad h  "+
       "  right join [gzerp].[dbo].cdrbdta d  "+
       "  left join [gzerp].[dbo].invmas s on s.itnbr=d.itnbr  "+
       "  on h.bakno=d.bakno , [gzerp].[dbo].cdrcus c  "+
       "  where h.facno = 'G'  "+
       "  and h.baksta<>'W' "+
       "  and c.cusno=h.cusno  "+
       "  and h.depno in('1D100')  "+
       "  and h.cusno not in ('SJS00254','SGD00088','SSD00107','SSH00328')  "+
       "  and d.itnbr in(select itnbr from [gzerp].[dbo].invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80')) "+
       "  and DateDiff(year,h.bakdate,getdate())=0 ";


            Fill(sqlstr, ds, "tbl");

            DataRow newRow;
            String itnbr = "", computeqty, colname, mon;
            string filter = " itnbr='{0}' and  yearmon ='{1}' ";
            decimal shpqy;
            string[] p = new string[] { };
            string thisyear = DateTime.Now.ToString("yyyy");
            foreach (DataRow item in ds.Tables["tbl"].Rows)
            {

                itnbr = item["itnbr"].ToString();
                if (p.Contains(item["itnbr"].ToString())) continue;
                Array.Resize(ref p, p.Length + 1);
                p.SetValue(itnbr, p.Length - 1);

                newRow = ds.Tables["tblresult"].NewRow();
               
                newRow["itnbr"] = item["itnbr"];
                newRow["itdsc"] = item["itdsc"];

                for (int i = 1; i < 13; i++)
                {
                    mon = i.ToString("00");
                    shpqy = 0;
                    computeqty = ds.Tables["tbl"].Compute(" SUM(shpqy)", string.Format(filter, itnbr, thisyear + mon)).ToString();
                    
                    if (computeqty != null && computeqty != "")
                    {
                        shpqy = decimal.Parse(computeqty);
                    }
                 
                    colname = "q" + mon;
                    newRow[colname] = shpqy;
                   
                }

                ds.Tables["tblresult"].Rows.Add(newRow);



            }

        }

    }
}
