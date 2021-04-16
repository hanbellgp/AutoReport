using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class HSCustomerRankingConfig:NotificationConfig
    {
        public HSCustomerRankingConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new HSCustomerRankingDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            //整年度
            String sqlStr = @"select a.yer,a.cusno,a.cusna,0 as m_amt,0 as m_order, Convert(decimal(18,2),sum(amt)/10000)  as y_amt,0 as y_order,0 as lm_amt,
                            0 as lm_order,0 as ly_amt,0 as ly_order,0 as ly_grow,0 as ly_grower
                            from (
                            select convert(varchar(4),h.shpdate,112) as yer,h.cusno,c.cusna,
                            sum(cast((case when h.coin<>'RMB' then d.shpamts*h.ratio else (case when h.tax = '1' then d.shpamts*h.ratio else d.shpamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                            from cdrdta d,cdrhad h,cdrcus c
                            where h.shpno=d.shpno and c.cusno = h.cusno  and h.houtsta not in ('W','N')
                            and year(h.shpdate) = year(dateadd(month,-1,getdate()))
                            GROUP BY convert(varchar(4),h.shpdate,112),h.cusno,c.cusna
                            UNION ALL
                            select convert(varchar(4),h.bakdate,112) as yer,h.cusno as cusno,c.cusna,
                            sum(-1*cast((case when h.coin<>'RMB' then d.bakamts*h.ratio else (case when h.tax = '1' then d.bakamts*h.ratio else d.bakamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                            from cdrbdta d,cdrbhad h,cdrcus c
                            where h.bakno=d.bakno  and c.cusno = h.cusno and h.baksta not in ('W','N') and h.owarehyn='Y'
                            and year(h.bakdate) = year(dateadd(month,-1,getdate()))
                            GROUP BY 	convert(varchar(4),h.bakdate,112),h.cusno,c.cusna
                            UNION ALL
                            select convert(varchar(4),a.bildat,112) as yer,a.ivocus as cusno,c.cusna,
                            sum(-1*cast((case when a.taxkd = '1' then a.losamts else a.losamts/(1+a.taxrate) end) as decimal(12,2))) as amt
                            from armblos a,cdrbdta d,cdrcus c
                            where a.facno=d.facno  and a.bakno=d.bakno and a.trseq=d.trseq and c.cusno = a.ivocus
                            and year(a.bildat) = year(dateadd(month,-1,getdate()))
                            GROUP BY 	convert(varchar(4),a.bildat,112),a.ivocus,c.cusna
                            ) a GROUP BY	a.yer,a.cusno,a.cusna
                            ORDER BY Convert(decimal(18,2),sum(amt)/10000) DESC";

            Fill(sqlStr, ds, "tbl");

            //上月
            sqlStr = @"select a.yer,a.cusno,a.cusna,Convert(decimal(18,2),sum(amt)/10000) as m_amt,0 as m_order,0 as y_amt,0 as y_order,0 as lm_amt,
                        0 as lm_order,0 as ly_amt,0 as ly_order,0 as ly_grow,0 as ly_grower
                        from (
                        select convert(varchar(4),h.shpdate,112) as yer,h.cusno,c.cusna,
                        sum(cast((case when h.coin<>'RMB' then d.shpamts*h.ratio else (case when h.tax = '1' then d.shpamts*h.ratio else d.shpamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                        from cdrdta d,cdrhad h,cdrcus c
                        where h.shpno=d.shpno and c.cusno = h.cusno  and h.houtsta not in ('W','N')
                        and convert(varchar(6),h.shpdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)
                        GROUP BY 	convert(varchar(4),h.shpdate,112),h.cusno,c.cusna
                        UNION ALL
                        select convert(varchar(4),h.bakdate,112) as yer,h.cusno as cusno,c.cusna,
                        sum(-1*cast((case when h.coin<>'RMB' then d.bakamts*h.ratio else (case when h.tax = '1' then d.bakamts*h.ratio else d.bakamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                        from cdrbdta d,cdrbhad h,cdrcus c
                        where h.bakno=d.bakno  and c.cusno = h.cusno and h.baksta not in ('W','N') and h.owarehyn='Y'
                        and convert(varchar(6),h.bakdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)
                        GROUP BY 	convert(varchar(4),h.bakdate,112),h.cusno,c.cusna
                        UNION ALL
                        select convert(varchar(4),a.bildat,112) as yer,a.ivocus as cusno,c.cusna,
                        sum(-1*cast((case when a.taxkd = '1' then a.losamts else a.losamts/(1+a.taxrate) end) as decimal(12,2))) as amt
                        from armblos a,cdrbdta d,cdrcus c
                        where a.facno=d.facno  and a.bakno=d.bakno and a.trseq=d.trseq and c.cusno = a.ivocus
                        and convert(varchar(6),a.bildat,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)
                        GROUP BY 	convert(varchar(4),a.bildat,112),a.ivocus,c.cusna
                        ) a GROUP BY	a.yer,a.cusno,a.cusna
                        ORDER BY Convert(decimal(18,2),sum(amt)/10000) desc";
            Fill(sqlStr, ds, "mamt"); 
            //添加到tbl表中
            foreach (DataRow item in ds.Tables["tbl"].Rows)
            {
                foreach (DataRow row in ds.Tables["mamt"].Rows)
                {
                    if (item["cusno"].ToString() == row["cusno"].ToString())
                    {
                        item["m_amt"] = row["m_amt"];
                        item["m_order"] = row["m_order"];
                    }
                }
            }

            //同期
            sqlStr = @"select a.yer,a.cusno,a.cusna,0 as m_amt,0 as m_order,0 as y_amt,0 as y_order,Convert(decimal(18,2),sum(amt)/10000) as lm_amt,
                    0 as lm_order,0 as ly_amt,0 as ly_order,0 as ly_grow,0 as ly_grower
                    from (
                    select convert(varchar(4),h.shpdate,112) as yer,h.cusno,c.cusna,
                    sum(cast((case when h.coin<>'RMB' then d.shpamts*h.ratio else (case when h.tax = '1' then d.shpamts*h.ratio else d.shpamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                    from cdrdta d,cdrhad h,cdrcus c
                    where h.shpno=d.shpno and c.cusno = h.cusno  and h.houtsta not in ('W','N')
                    and convert(varchar(6),h.shpdate,112) <= convert(varchar(6),dateadd(month,-13,getdate()),112)
                    and convert(varchar(4),h.shpdate,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),h.shpdate,112),h.cusno,c.cusna
                    UNION ALL
                    select convert(varchar(4),h.bakdate,112) as yer,h.cusno as cusno,c.cusna,
                    sum(-1*cast((case when h.coin<>'RMB' then d.bakamts*h.ratio else (case when h.tax = '1' then d.bakamts*h.ratio else d.bakamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                    from cdrbdta d,cdrbhad h,cdrcus c
                    where h.bakno=d.bakno  and c.cusno = h.cusno and h.baksta not in ('W','N') and h.owarehyn='Y'
                    and convert(varchar(6),h.bakdate,112) <= convert(varchar(6),dateadd(month,-13,getdate()),112)
                    and convert(varchar(4),h.bakdate,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),h.bakdate,112),h.cusno,c.cusna
                    UNION ALL
                    select convert(varchar(4),a.bildat,112) as yer,a.ivocus as cusno,c.cusna,
                    sum(-1*cast((case when a.taxkd = '1' then a.losamts else a.losamts/(1+a.taxrate) end) as decimal(12,2))) as amt
                    from armblos a,cdrbdta d,cdrcus c
                    where a.facno=d.facno  and a.bakno=d.bakno and a.trseq=d.trseq and c.cusno = a.ivocus
                    and convert(varchar(6),a.bildat,112) <= convert(varchar(6),dateadd(month,-13,getdate()),112)
                    and convert(varchar(4),a.bildat,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),a.bildat,112),a.ivocus,c.cusna
                    ) a GROUP BY	a.yer,a.cusno,a.cusna
                    ORDER BY Convert(decimal(18,2),sum(amt)/10000) desc";
            //添加到tbl表里去
            Fill(sqlStr, ds, "lmamt");

            foreach (DataRow item in ds.Tables["tbl"].Rows)
            {
                foreach (DataRow row in ds.Tables["lmamt"].Rows)
                {
                    if (item["cusno"].ToString() == row["cusno"].ToString())
                    {
                        item["lm_amt"] = row["lm_amt"];
                        item["lm_order"] = row["lm_order"];
                    }
                }
            }

            //去年年度
            sqlStr = @"select a.yer,a.cusno,a.cusna,0 as m_amt,0 as m_order,0 as y_amt,0 as y_order,0 as lm_amt,
                    0 as lm_order,Convert(decimal(18,2),sum(amt)/10000) as ly_amt,0 as ly_order,0 as ly_grow,0 as ly_grower
                    from (
                    select convert(varchar(4),h.shpdate,112) as yer,h.cusno,c.cusna,
                    sum(cast((case when h.coin<>'RMB' then d.shpamts*h.ratio else (case when h.tax = '1' then d.shpamts*h.ratio else d.shpamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                    from cdrdta d,cdrhad h,cdrcus c
                    where h.shpno=d.shpno and c.cusno = h.cusno  and h.houtsta not in ('W','N')
                    and convert(varchar(4),h.shpdate,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),h.shpdate,112),h.cusno,c.cusna
                    UNION ALL
                    select convert(varchar(4),h.bakdate,112) as yer,h.cusno as cusno,c.cusna,
                    sum(-1*cast((case when h.coin<>'RMB' then d.bakamts*h.ratio else (case when h.tax = '1' then d.bakamts*h.ratio else d.bakamts*h.ratio/(h.taxrate+1) end) end) as decimal(12,2))) as amt
                    from cdrbdta d,cdrbhad h,cdrcus c
                    where h.bakno=d.bakno  and c.cusno = h.cusno and h.baksta not in ('W','N') and h.owarehyn='Y'
                    and convert(varchar(4),h.bakdate,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),h.bakdate,112),h.cusno,c.cusna
                    UNION ALL
                    select convert(varchar(4),a.bildat,112) as yer,a.ivocus as cusno,c.cusna,
                    sum(-1*cast((case when a.taxkd = '1' then a.losamts else a.losamts/(1+a.taxrate) end) as decimal(12,2))) as amt
                    from armblos a,cdrbdta d,cdrcus c
                    where a.facno=d.facno  and a.bakno=d.bakno and a.trseq=d.trseq and c.cusno = a.ivocus
                    and convert(varchar(4),a.bildat,112) = convert(varchar(4),dateadd(month,-13,getdate()),112)
                    GROUP BY 	convert(varchar(4),a.bildat,112),a.ivocus,c.cusna
                    ) a GROUP BY	a.yer,a.cusno,a.cusna
                    ORDER BY Convert(decimal(18,2),sum(amt)/10000) desc";
            //添加到tbl表去
            Fill(sqlStr, ds, "lyamt");
            foreach (DataRow item in ds.Tables["tbl"].Rows)
            {
                foreach (DataRow row in ds.Tables["lyamt"].Rows)
                {
                    if (item["cusno"].ToString() == row["cusno"].ToString())
                    {
                        item["ly_amt"] = row["ly_amt"];
                        item["ly_order"] = row["ly_order"];
                    }
                }
            }
        }
        //最后排序
        public override void ConfigData()
        {
            //先排去年采购金额
            int i = 0;
            DataTable tbl;
            tbl = ds.Tables["tbl"].Copy();
            tbl.DefaultView.Sort = "lm_amt DESC";

            ds.Tables["tbl"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();

            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["lm_amt"].ToString().Equals("0"))
                {
                    item["lm_order"] = i;
                }
                else
                {
                    item["lm_order"] = 0;
                }
                DataRow r = ds.Tables["tbl"].NewRow();
                r[0] = item[0];
                r[1] = item[1];
                r[2] = item[2];
                r[3] = item[3];
                r[4] = item[4];
                r[5] = item[5];
                r[6] = item[6];
                r[7] = item[7];
                r[8] = item[8];
                r[9] = item[9];
                r[10] = item[10];
                ds.Tables["tbl"].Rows.Add(r);
            }
            //再排同期采购
            i = 0;
            tbl = ds.Tables["tbl"].Copy();
            tbl.DefaultView.Sort = "ly_amt DESC";

            ds.Tables["tbl"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();

            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["ly_amt"].ToString().Equals("0"))
                {
                    item["ly_order"] = i;
                }
                else
                {
                    item["ly_order"] = 0;
                }
                DataRow r = ds.Tables["tbl"].NewRow();
                r[0] = item[0];
                r[1] = item[1];
                r[2] = item[2];
                r[3] = item[3];
                r[4] = item[4];
                r[5] = item[5];
                r[6] = item[6];
                r[7] = item[7];
                r[8] = item[8];
                r[9] = item[9];
                r[10] = item[10];
                ds.Tables["tbl"].Rows.Add(r);
            }
            //上月采购排序
            i = 0;
            tbl = ds.Tables["tbl"].Copy();
            tbl.DefaultView.Sort = "m_amt DESC";

            ds.Tables["tbl"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();
            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["m_amt"].ToString().Equals("0"))
                {
                    item["m_order"] = i;
                }
                else
                {
                    item["m_order"] = 0;
                }
                DataRow r = ds.Tables["tbl"].NewRow();
                r[0] = item[0];
                r[1] = item[1];
                r[2] = item[2];
                r[3] = item[3];
                r[4] = item[4];
                r[5] = item[5];
                r[6] = item[6];
                r[7] = item[7];
                r[8] = item[8];
                r[9] = item[9];
                r[10] = item[10];
                ds.Tables["tbl"].Rows.Add(r);
            }
            //年度采购排序
            i = 0;
            tbl = ds.Tables["tbl"].Copy();
            tbl.DefaultView.Sort = "y_amt DESC";

            ds.Tables["tbl"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();
            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["y_amt"].ToString().Equals("0"))
                {
                    item["y_order"] = i;
                }
                else
                {
                    item["y_order"] = 0;
                }
                DataRow r = ds.Tables["tbl"].NewRow();
                r[0] = item[0];
                r[1] = item[1];
                r[2] = item[2];
                r[3] = item[3];
                r[4] = item[4];
                r[5] = item[5];
                r[6] = item[6];
                r[7] = item[7];
                r[8] = item[8];
                r[9] = item[9];
                r[10] = item[10];
                ds.Tables["tbl"].Rows.Add(r);
            }
            ds.AcceptChanges();

        }
    }
}
