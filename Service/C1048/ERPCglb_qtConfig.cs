using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class ERPCglb_qtConfig : ERPCglb_zjConfig
    {
        public ERPCglb_qtConfig()
        {
        }
        //public ERPCglb_qtConfig(DBServerType dbType, string connName)
        //{
        //    PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
        //    this.ds = new DSERPCglb_zj();
        //    this.args = Base.GetParameter(this.ToString());
        //}

        public ERPCglb_qtConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSERPCglb_zj();
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
            base.InitData();

            string sqlstr = @"SELECT  apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype, 
            sum(acpamt) as ty ,  
            sum(case when left( convert(varchar(8),trdat,112),6)=left( convert(varchar(8),dateadd(month,-1,getdate()),112),6) 
            then acpamt else 0 end)as tm, 
            isnull((select sum(case when left(convert(varchar(8),y.trdat,112),6) = left(convert(varchar(8),dateadd(month,-13,getdate()),112) ,6)
            then y.acpamt else 0 end) from apmpyh y ,invmas p,wlfl_db w 
            where y.facno='C' and y.prono='1' and y.itnbr=p.itnbr and p.itcls=w.itcls 
            and wlfl_db.cgtype=w.cgtype and y.pyhkind = '1' 
            and  left(convert(char(8),y.trdat,112),4)=left(convert(char(4),dateadd(month,-13,getdate()),112),4)),0) as lm, 
            isnull((select sum(y.acpamt) from apmpyh y ,invmas p,wlfl_db w 
            where y.facno='C' and y.prono='1' and y.itnbr=p.itnbr  
            and p.itcls=w.itcls and wlfl_db.cgtype=w.cgtype  and y.pyhkind = '1' 
            and left(convert(char(8),y.trdat,112),4)=left(convert(char(8),dateadd(month,-13,getdate()),112),4) ),0) as ly ,
            isnull((select sum(case when left(convert(varchar(8),y.trdat,112),6) <= left(convert(varchar(8),dateadd(month,-13,getdate()),112) ,6)
            then y.acpamt else 0 end) from apmpyh y ,invmas p,wlfl_db w 
            where y.facno='C' and y.prono='1' and y.itnbr=p.itnbr  
            and p.itcls=w.itcls and wlfl_db.cgtype=w.cgtype  and y.pyhkind = '1' 
            and left(convert(char(8),y.trdat,112),4)=left(convert(char(8),dateadd(month,-13,getdate()),112),4) ),0) as llj 
            FROM apmpyh , invmas ,  purhad ,wlfl_db  
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1'  and  purhad.facno = apmpyh.facno  
            and    purhad.prono = apmpyh.prono and   purhad.pono = apmpyh.pono 
            and  apmpyh.itnbr=invmas.itnbr   and invmas.itcls = wlfl_db.itcls   
            AND  year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))  and apmpyh.pyhkind = '1'
            and (wlfl_db.cgtype='A油气桶' or wlfl_db.cgtype='A轴封' or wlfl_db.cgtype='A钣金' or wlfl_db.cgtype='R电机' or 
            wlfl_db.cgtype='R阀类' or wlfl_db.cgtype='齿轮' or wlfl_db.cgtype='加工半成品' or wlfl_db.cgtype='加工成品' or wlfl_db.cgtype='模具' or 
            wlfl_db.cgtype='轴承类' or wlfl_db.cgtype='油品类' or wlfl_db.cgtype='R电磁阀' or wlfl_db.cgtype='A列管零件'or wlfl_db.cgtype='A电机' ) 
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype ";

            Fill(sqlstr, ds, "tbcglb");


            string sqlstr2 = @"SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            wlfl_db.cgtype, sum(acpamt) as ty ,  
            sum(case when left(convert(varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6) 
            then acpamt else 0 end)  as tm, 0 as lm, 0 as ly , 0 as llj     
            FROM apmpyh ,purvdr  , invmas , wlfl_db   
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1' 
            and apmpyh.vdrno = purvdr.vdrno  and  wlfl_db.itcls=invmas.itcls 
            and (wlfl_db.cgtype='A油气桶' or wlfl_db.cgtype='A轴封' or wlfl_db.cgtype='A钣金' or wlfl_db.cgtype='R电机' or 
            wlfl_db.cgtype='R阀类' or wlfl_db.cgtype='齿轮' or wlfl_db.cgtype='加工半成品' or wlfl_db.cgtype='加工成品' or wlfl_db.cgtype='模具' or 
            wlfl_db.cgtype='轴承类' or wlfl_db.cgtype='油品类' or wlfl_db.cgtype='R电磁阀' or wlfl_db.cgtype='A列管零件'or wlfl_db.cgtype='A电机' )
            and apmpyh.itnbr=invmas.itnbr 
            and year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))
            and apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,apmpyh.vdrno , purvdr.vdrna,wlfl_db.cgtype";


            string sqlstr3 = @"SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            wlfl_db.cgtype, sum(acpamt) as ly ,  
            sum(case when left(convert(varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)  as lm ,
            sum(case when left(convert(varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)  as llj   
            FROM apmpyh ,purvdr  , invmas , wlfl_db   
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1' 
            and apmpyh.vdrno = purvdr.vdrno  and  wlfl_db.itcls=invmas.itcls 
            and (wlfl_db.cgtype='A油气桶' or wlfl_db.cgtype='A轴封' or wlfl_db.cgtype='A钣金' or wlfl_db.cgtype='R电机' or 
            wlfl_db.cgtype='R阀类' or wlfl_db.cgtype='齿轮' or wlfl_db.cgtype='加工半成品' or wlfl_db.cgtype='加工成品' or wlfl_db.cgtype='模具' or 
            wlfl_db.cgtype='轴承类' or wlfl_db.cgtype='油品类' or wlfl_db.cgtype='R电磁阀' or wlfl_db.cgtype='A列管零件'or wlfl_db.cgtype='A电机' ) 
            and apmpyh.itnbr=invmas.itnbr 
            and year(apmpyh.trdat)= year(dateadd(month,-13,getdate()))
            and apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,apmpyh.vdrno , purvdr.vdrna,wlfl_db.cgtype";

            Fill(sqlstr2, ds, "tbvdrno");


            string[] cols1 = { "facno", "prono", "cgtype" };
            string[] cols2 = { "ty", "tm", "lm", "ly","llj" };//采购类别需要去年同期和去年全年合并
            MergeSameDataRow(ds.Tables["tbcglb"], cols1, cols2);


            ds.Tables["tbl2"].Clear();

            Fill(sqlstr3, ds, "tbl2");

            foreach (DataRow item in ds.Tables["tbvdrno"].Rows)
            {
                foreach (DataRow row in ds.Tables["tbl2"].Rows)
                {
                    if (item["cgtype"].ToString() == row["cgtype"].ToString() && item["vdrno"].ToString() == row["vdrno"].ToString()&&
                        item["ly"].ToString()=="0")
                    {
                        item["lm"] = Decimal.Parse(row["lm"].ToString());
                        item["ly"] = Decimal.Parse(row["ly"].ToString());
                        item["llj"] = Decimal.Parse(row["llj"].ToString());
                    }
                }
            }

            string[] cols3 = { "cgtype", "vdrno", "vdrna" };
            string[] cols4 = { "ty", "tm", "lm", "ly", "llj" };//厂商中不需要去年同期和去年全年合并
            MergeSameDataRow(ds.Tables["tbvdrno"], cols3, cols4);

        }

        public int MergeSameDataRow(DataTable tbl, String[] comparecolumns, String[] mergecolumns)
        {
            if (tbl.Rows.Count <= 1)
            {
                return 0;
            }
            if (comparecolumns == null || mergecolumns == null)
            {
                return -1;
            }
            bool merged = false;
            bool ret;
            try
            {

            Redo:
                if (tbl.Rows.Count > 1)
                {
                    int i = 0;
                    do
                    {
                        if (tbl.Rows[i].RowState == DataRowState.Deleted)
                        {
                            i++;
                            continue;
                        }

                        int j = i + 1;
                        do
                        {
                            if (tbl.Rows[j].RowState == DataRowState.Deleted)
                            {
                                j++;
                                continue;
                            }

                            ret = true;
                            for (int n = 0; n < comparecolumns.Length; n++)
                            {
                                if ((tbl.Rows[i][comparecolumns[n]] == null) && (tbl.Rows[j][comparecolumns[n]] == null))
                                {
                                    ret = ret && true;
                                    continue;
                                }
                                else if ((tbl.Rows[i][comparecolumns[n]] != null) && (tbl.Rows[j][comparecolumns[n]] == null))
                                {
                                    ret = ret && false;
                                    break;
                                }
                                else if ((tbl.Rows[i][comparecolumns[n]] == null) && (tbl.Rows[j][comparecolumns[n]] != null))
                                {
                                    ret = ret && false;
                                    break;
                                }
                                if (tbl.Rows[i][comparecolumns[n]].ToString() == tbl.Rows[j][comparecolumns[n]].ToString())
                                {
                                    ret = ret && true;
                                }
                                else
                                {
                                    ret = ret && false;
                                    break;
                                }
                            }
                            if (ret)
                            {
                                for (int m = 0; m < mergecolumns.Length; m++)
                                {
                                    tbl.Rows[i][mergecolumns[m]] = decimal.Parse(tbl.Rows[i][mergecolumns[m]].ToString()) + decimal.Parse(tbl.Rows[j][mergecolumns[m]].ToString());
                                }
                                tbl.Rows.RemoveAt(j);
                                merged = true;
                                goto Redo;

                            }
                            j++;
                        }
                        while (j < tbl.Rows.Count);
                        i++;
                    }
                    while (i < tbl.Rows.Count - 1);
                }

            }
            catch (Exception)
            {
                return -1;
            }
            if (merged)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

    }
}
