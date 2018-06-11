using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SHBNianDuCaiGouPaiMingConfig : NotificationConfig
    {
        public SHBNianDuCaiGouPaiMingConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));

            this.ds = new ComerNDCGPMDS();
            //this.reportList.Add(new NianDuCaiGouPaiMingReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            //查询年度年度采购金额
            String sqlStr = @"select top 30 convert(varchar(4),a.trdat,112) as yer,a.vdrno,a.vdrna ,0 as m_puramt, 0 as m_order,
            sum(a.acpamt)/10000 as y_puramt , 0 as y_order,0 as lm_puramt,0 as lm_order,0 as ly_puramt,0 as ly_order from ( 
            SELECT apmpyh.facno,apmpyh.prono,apmpyh.vdrno,purvdr.vdrna,acpamt,0 as ordern,apmpyh.trdat FROM apmpyh, purvdr,purhad 
            WHERE (apmpyh.vdrno = purvdr.vdrno) and  (purhad.facno = apmpyh.facno) and (purhad.prono = apmpyh.prono) and (purhad.pono = apmpyh.pono) and ((apmpyh.pyhkind = '1'))  AND
            (apmpyh.facno = 'C' and apmpyh.prono = '1' and 
            year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))))a 
            GROUP BY convert(varchar(4),a.trdat,112),a.vdrno ,a.vdrna 
            order by sum(a.acpamt) desc";
            Fill(sqlStr, ds, "ndcgpm");
            //查询上个月采购金额
            sqlStr = @"select convert(varchar(6),a.trdat,112) as 'mon',a.vdrno,a.vdrna ,sum(a.acpamt)/10000 as 'm_puramt',0 as 'm_order' from (
            SELECT apmpyh.facno,apmpyh.prono,apmpyh.vdrno,purvdr.vdrna,acpamt,0 as ordern,apmpyh.trdat FROM apmpyh, purvdr,purhad WHERE (apmpyh.vdrno = purvdr.vdrno) and 
            (purhad.facno = apmpyh.facno) and (purhad.prono = apmpyh.prono) and (purhad.pono = apmpyh.pono) and ((apmpyh.pyhkind = '1')) AND (apmpyh.facno = 'C' and apmpyh.prono = '1' and 
            convert(varchar(6),apmpyh.trdat,112)= convert(varchar(6),dateadd(month,-1,getdate()),112)))a 
            GROUP BY convert(varchar(6),a.trdat,112),a.vdrno ,a.vdrna    
            order by sum(a.acpamt) desc";
            Fill(sqlStr, ds, "mpuramt");
            //添加到ndcgpm表中
            foreach (DataRow item in ds.Tables["ndcgpm"].Rows)
            {
                foreach (DataRow row in ds.Tables["mpuramt"].Rows)
                {
                    if (item["vdrno"].ToString() == row["vdrno"].ToString())
                    {
                        item["m_puramt"] = row["m_puramt"];
                        item["m_order"] = row["m_order"];
                    }
                }
            }

            //查询同期排名
            sqlStr = @"select convert(varchar(4),a.trdat,112) as 'mom',a.vdrno,a.vdrna,sum(a.acpamt)/10000 as 'lm_puramt',0 as 'lm_order' from (
            SELECT  apmpyh.facno,apmpyh.prono,apmpyh.vdrno,purvdr.vdrna ,acpamt,0 as ordern,apmpyh.trdat
            FROM apmpyh,purvdr,purhad  WHERE (apmpyh.vdrno = purvdr.vdrno) and (purhad.facno = apmpyh.facno) and  (purhad.prono = apmpyh.prono) and   
            (purhad.pono = apmpyh.pono) and ((apmpyh.pyhkind = '1'))  AND (apmpyh.facno = 'C' and apmpyh.prono = '1' and 
            convert(varchar(6),apmpyh.trdat,112) < = convert( varchar(6),dateadd(month,-13,getdate()),112) 
            and convert(varchar(4),apmpyh.trdat,112) = convert( varchar(4),dateadd(month,-13,getdate()),112)))a 
            GROUP BY convert(varchar(4),a.trdat,112),a.vdrno,a.vdrna 
            order by sum(a.acpamt) desc";
            //添加到ndcgpm表去
            Fill(sqlStr, ds, "lmpuramt");
            foreach (DataRow item in ds.Tables["ndcgpm"].Rows)
            {
                foreach (DataRow row in ds.Tables["lmpuramt"].Rows)
                {
                    if (item["vdrno"].ToString() == row["vdrno"].ToString())
                    {
                        item["lm_puramt"] = row["lm_puramt"];
                        item["lm_order"] = row["lm_order"];
                    }
                }
            }

            //查询去年排名
            sqlStr = @"select convert(varchar(4),a.trdat,112) as 'yer',a.vdrno,a.vdrna ,sum(a.acpamt)/10000 as 'ly_puramt',0 as 'ly_order' from (
            SELECT apmpyh.facno,apmpyh.prono,apmpyh.vdrno,purvdr.vdrna,acpamt,0 as ordern,apmpyh.trdat FROM apmpyh,purvdr,purhad 
            WHERE (apmpyh.vdrno = purvdr.vdrno) and ( purhad.facno = apmpyh.facno ) and ( purhad.prono = apmpyh.prono ) and   
            (purhad.pono = apmpyh.pono) and ((apmpyh.pyhkind = '1')) AND (apmpyh.facno = 'C' and apmpyh.prono = '1' and 
            year(apmpyh.trdat)= year(dateadd(year,-1,dateadd(month,-1,getdate())))))a 
            GROUP BY convert(varchar(4),a.trdat,112),a.vdrno,a.vdrna    
            order by sum(a.acpamt) desc";
            //添加到ndcgpm表里去
            Fill(sqlStr, ds, "lypuramt");
            foreach (DataRow item in ds.Tables["ndcgpm"].Rows)
            {
                foreach (DataRow row in ds.Tables["lypuramt"].Rows)
                {
                    if (item["vdrno"].ToString() == row["vdrno"].ToString())
                    {
                        item["ly_puramt"] = row["ly_puramt"];
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
            tbl = ds.Tables["ndcgpm"].Copy();
            tbl.DefaultView.Sort = "ly_puramt DESC";

            ds.Tables["ndcgpm"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();

            foreach (DataRow item in tbl.Rows)
            {
                i++;
                //Console.WriteLine(item["lm_puramt"].ToString());
                if (!item["ly_puramt"].ToString().Equals("0"))
                {
                    item["ly_order"] = i;
                }
                else
                {
                    item["ly_order"] = 0;
                }
                DataRow r = ds.Tables["ndcgpm"].NewRow();
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
                ds.Tables["ndcgpm"].Rows.Add(r);
            }
            //再排同期采购
            i = 0;
            tbl = ds.Tables["ndcgpm"].Copy();
            tbl.DefaultView.Sort = "lm_puramt DESC";

            ds.Tables["ndcgpm"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();

            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["lm_puramt"].ToString().Equals("0"))
                {
                    item["lm_order"] = i;
                }
                else
                {
                    item["lm_order"] = 0;
                }
                DataRow r = ds.Tables["ndcgpm"].NewRow();
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
                ds.Tables["ndcgpm"].Rows.Add(r);
            }
            //上月采购排序
            i = 0;
            tbl = ds.Tables["ndcgpm"].Copy();
            tbl.DefaultView.Sort = "m_puramt DESC";

            ds.Tables["ndcgpm"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();
            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["m_puramt"].ToString().Equals("0"))
                {
                    item["m_order"] = i;
                }
                else
                {
                    item["m_order"] = 0;
                }
                DataRow r = ds.Tables["ndcgpm"].NewRow();
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
                ds.Tables["ndcgpm"].Rows.Add(r);
            }
            //年度采购排序
            i = 0;
            tbl = ds.Tables["ndcgpm"].Copy();
            tbl.DefaultView.Sort = "y_puramt DESC";

            ds.Tables["ndcgpm"].Rows.Clear();
            tbl = tbl.DefaultView.ToTable();
            foreach (DataRow item in tbl.Rows)
            {
                i++;
                if (!item["y_puramt"].ToString().Equals("0"))
                {
                    item["y_order"] = i;
                }
                else
                {
                    item["y_order"] = 0;
                }
                DataRow r = ds.Tables["ndcgpm"].NewRow();
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
                ds.Tables["ndcgpm"].Rows.Add(r);
            }
            ds.AcceptChanges();

        }

    }
}
