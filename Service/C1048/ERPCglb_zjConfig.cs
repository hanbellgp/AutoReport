using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Globalization;

namespace Hanbell.AutoReport.Config
{
    public class ERPCglb_zjConfig : NotificationConfig
    {
        public ERPCglb_zjConfig()
        { 
        }
        public ERPCglb_zjConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSERPCglb_zj();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = @"SELECT A.facno,A.prono,A.cgtype,sum(A.ty) as ty,sum(A.tm) as tm,sum(A.ly) as ly,sum(A.lm) as lm,sum(A.llj) as llj
            FROM 
            (SELECT  apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype, 
            sum(acpamt) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6) 
            then acpamt else 0 end)as tm, 
            0 as lm, 
            0 as ly, 
            0 as llj
            FROM apmpyh , invmas ,  purhad ,wlfl_db  
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1'  and  purhad.facno = apmpyh.facno  
            and    purhad.prono = apmpyh.prono and   purhad.pono = apmpyh.pono 
            and  apmpyh.itnbr=invmas.itnbr   and invmas.itcls = wlfl_db.itcls   
            AND  year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))  and apmpyh.pyhkind = '1'and wlfl_db.cgtype='铸件类'
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype         

            union all

            SELECT  apmpyh.facno ,apmpyh.prono ,'铸件类', sum(puritnbrdz.poprice*apmpyh.payqty) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112) ,6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as tm, 
            0 as lm, 
            0 as ly , 
            0 as llj
            FROM apmpyh , invmas a,  purhad  ,puritnbrdz ,wlfl_db ,invmas b
            WHERE
            apmpyh.itnbr=puritnbrdz.initnbr  and 
            a.itnbr=puritnbrdz.poitnbr and
            a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and wlfl_db.cgtype <> '铸件类' and apmpyh.vdrno='SZJ00065' and 
            apmpyh.facno = 'C' and apmpyh.prono = '1' and  purhad.facno = apmpyh.facno and 
            purhad.prono = apmpyh.prono and  purhad.pono = apmpyh.pono and apmpyh.itnbr=b.itnbr and wlfl_db.itcls=b.itcls  and  
            year(apmpyh.trdat)= year(dateadd(month,-1,getdate())) and  apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono 

            union all

            SELECT  apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype, sum(puritnbrdz.poprice*apmpyh.payqty*-1) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as tm, 
            0 as lm, 
            0 as ly, 
            0 as llj 
            FROM apmpyh , invmas a,  purhad ,wlfl_db ,puritnbrdz ,invmas b
            WHERE 
            apmpyh.itnbr=puritnbrdz.initnbr  and 
            a.itnbr=puritnbrdz.poitnbr and
            a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and wlfl_db.cgtype <> '铸件类' and apmpyh.vdrno='SZJ00065' and 
            apmpyh.facno = 'C' and apmpyh.prono = '1' and  purhad.facno = apmpyh.facno and
            purhad.prono = apmpyh.prono and purhad.pono = apmpyh.pono 
            and apmpyh.itnbr=b.itnbr and wlfl_db.itcls=b.itcls and
            year(apmpyh.trdat)= year(dateadd(month,-1,getdate())) and apmpyh.pyhkind = '1'
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype) A
            group by A.facno,A.prono,A.cgtype ";

            string sqlstr4 = @"select A.facno ,A.prono,A.cgtype,sum(A.lm) as lm,sum(A.ly) as ly,sum(A.llj) as llj
            from 
            (
            SELECT  apmpyh.facno ,apmpyh.prono ,
            wlfl_db.cgtype, sum(acpamt) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)as lm,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)as llj
            FROM apmpyh , invmas ,  wlfl_db    
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1' and  wlfl_db.itcls=invmas.itcls and wlfl_db.cgtype='铸件类' 
            and apmpyh.itnbr=invmas.itnbr 
            and year(apmpyh.trdat)= year(dateadd(month,-13,getdate()))
            and apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype

            union all

            SELECT  apmpyh.facno ,apmpyh.prono  , 
            '铸件类', sum(puritnbrdz.poprice*apmpyh.payqty) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as lm ,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as llj
            FROM apmpyh , invmas a,wlfl_db ,puritnbrdz  ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1'  
            and a.itnbr=puritnbrdz.poitnbr
            and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and  apmpyh.vdrno='SZJ00065' and 
            apmpyh.itnbr=puritnbrdz.initnbr  and apmpyh.itnbr=b.itnbr and wlfl_db.itcls=b.itcls and wlfl_db.cgtype <> '铸件类'
            and   year(apmpyh.trdat)= year(dateadd(month,-13,getdate())) 
            and apmpyh.pyhkind = '1'  
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype

            union all

            SELECT  apmpyh.facno ,apmpyh.prono  , wlfl_db.cgtype, sum(puritnbrdz.poprice*apmpyh.payqty*(-1)) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as lm,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as llj
            FROM apmpyh , invmas a ,wlfl_db ,puritnbrdz  ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1'   and  apmpyh.vdrno='SZJ00065' and 
            a.itnbr=puritnbrdz.poitnbr and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) 
            and apmpyh.itnbr=puritnbrdz.initnbr and 
            wlfl_db.itcls=b.itcls and apmpyh.itnbr=b.itnbr and wlfl_db.cgtype <> '铸件类'
            and apmpyh.itnbr=b.itnbr and   year(apmpyh.trdat)= year(dateadd(month,-13,getdate()))
            and  apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype) A
            GROUP BY  A.facno ,A.prono,A.cgtype ";

                        Fill(sqlstr, ds, "tbcglb");
                        Fill(sqlstr4, ds, "tbl3");

            foreach (DataRow item in ds.Tables["tbcglb"].Rows)
            {
                foreach (DataRow row in ds.Tables["tbl3"].Rows)
                {
                    if (item["cgtype"].ToString() == row["cgtype"].ToString() )
                    {
                        item["lm"] = row["lm"];
                        item["ly"] = row["ly"];
                        item["llj"] = row["llj"];
                    }
                }
            }


            string sqlstr2 = @"select A.vdrno,A.vdrna,A.cgtype,sum(A.ty) as ty ,sum(A.tm) as tm,sum(A.lm) as lm,sum(A.ly) as ly,sum(A.llj) as llj
            from 
            (SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            wlfl_db.cgtype, sum(acpamt) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6) 
            then acpamt else 0 end)as tm,  0 as lm, 0 as ly, 0 as llj
            FROM apmpyh , invmas ,  wlfl_db ,purvdr   
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1' 
            and apmpyh.vdrno = purvdr.vdrno and  wlfl_db.itcls=invmas.itcls and wlfl_db.cgtype='铸件类'
            and apmpyh.itnbr=invmas.itnbr 
            and year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))
            and apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,apmpyh.vdrno , purvdr.vdrna,wlfl_db.cgtype


            union all

            SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            '铸件类', sum(puritnbrdz.poprice*apmpyh.payqty) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as tm, 
            0 as lm, 0 as ly , 0 as llj
            FROM apmpyh , invmas a,wlfl_db ,purvdr ,puritnbrdz ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1' and a.itnbr=puritnbrdz.poitnbr
            and  apmpyh.vdrno = purvdr.vdrno and wlfl_db.itcls=b.itcls 
            and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and apmpyh.vdrno='SZJ00065' and 
            apmpyh.itnbr=puritnbrdz.initnbr  and apmpyh.itnbr=b.itnbr and wlfl_db.cgtype <> '铸件类'
            and   year(apmpyh.trdat)= year(dateadd(month,-1,getdate())) 
            and apmpyh.pyhkind = '1'  
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype,apmpyh.vdrno , purvdr.vdrna


            union all

            SELECT  apmpyh.vdrno ,purvdr.vdrna , wlfl_db.cgtype, sum(puritnbrdz.poprice*apmpyh.payqty*(-1)) as ty ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-1,getdate()),112),6) 
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as tm, 
            0 as lm, 0 as ly  , 0 as llj 
            FROM apmpyh , invmas a,wlfl_db ,purvdr ,puritnbrdz  ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1' and apmpyh.vdrno = purvdr.vdrno  and a.itnbr=puritnbrdz.poitnbr 
            and apmpyh.vdrno='SZJ00065' 
            and wlfl_db.itcls=b.itcls and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and 
            apmpyh.itnbr=puritnbrdz.initnbr and wlfl_db.cgtype <> '铸件类'
            and apmpyh.itnbr=b.itnbr and   year(apmpyh.trdat)= year(dateadd(month,-1,getdate()))
            and  apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype,apmpyh.vdrno , purvdr.vdrna) A
            GROUP BY  A.vdrno,A.vdrna,A.cgtype";


            string sqlstr3 = @"select A.vdrno,A.vdrna,A.cgtype,sum(A.lm) as lm,sum(A.ly) as ly,sum(A.llj) as llj
            from 
            (
            SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            wlfl_db.cgtype, sum(acpamt) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)as lm ,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then acpamt else 0 end)as llj
            FROM apmpyh , invmas ,  wlfl_db ,purvdr   
            WHERE apmpyh.facno = 'C' and apmpyh.prono = '1' 
            and apmpyh.vdrno = purvdr.vdrno and  wlfl_db.itcls=invmas.itcls and wlfl_db.cgtype='铸件类' 
            and apmpyh.itnbr=invmas.itnbr 
            and year(apmpyh.trdat)= year(dateadd(month,-13,getdate()))
            and apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,apmpyh.vdrno , purvdr.vdrna,wlfl_db.cgtype

            union all

            SELECT  apmpyh.vdrno ,purvdr.vdrna , 
            '铸件类', sum(puritnbrdz.poprice*apmpyh.payqty) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as lm,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6)
            then puritnbrdz.poprice*apmpyh.payqty else 0 end)as llj
            FROM apmpyh , invmas a,wlfl_db ,purvdr ,puritnbrdz  ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1' and  apmpyh.vdrno = purvdr.vdrno 
            and a.itnbr=puritnbrdz.poitnbr
            and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) and  apmpyh.vdrno='SZJ00065' and 
            apmpyh.itnbr=puritnbrdz.initnbr  and apmpyh.itnbr=b.itnbr and wlfl_db.itcls=b.itcls and wlfl_db.cgtype <> '铸件类'
            and   year(apmpyh.trdat)= year(dateadd(month,-13,getdate())) 
            and apmpyh.pyhkind = '1'  
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype,apmpyh.vdrno , purvdr.vdrna

            union all

            SELECT  apmpyh.vdrno ,purvdr.vdrna , wlfl_db.cgtype, sum(puritnbrdz.poprice*apmpyh.payqty*(-1)) as ly ,  
            sum(case when left(convert (varchar(8),trdat,112),6)= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as lm,  
            sum(case when left(convert (varchar(8),trdat,112),6)<= left(convert(varchar(8),dateadd(month,-13,getdate()),112),6) 
            then puritnbrdz.poprice*apmpyh.payqty*(-1) else 0 end)as llj
            FROM apmpyh , invmas a ,wlfl_db ,purvdr ,puritnbrdz  ,invmas b
            WHERE  apmpyh.facno = 'C' and apmpyh.prono = '1' and apmpyh.vdrno = purvdr.vdrno  and  apmpyh.vdrno='SZJ00065' and 
            a.itnbr=puritnbrdz.poitnbr and a.itcls in ( select itcls from wlfl_db where cgtype='铸件类' ) 
            and apmpyh.itnbr=puritnbrdz.initnbr and 
            wlfl_db.itcls=b.itcls and apmpyh.itnbr=b.itnbr and wlfl_db.cgtype <> '铸件类'
            and apmpyh.itnbr=b.itnbr and   year(apmpyh.trdat)= year(dateadd(month,-13,getdate()))
            and  apmpyh.pyhkind = '1' 
            group by apmpyh.facno ,apmpyh.prono ,wlfl_db.cgtype,apmpyh.vdrno , purvdr.vdrna) A
            GROUP BY  A.vdrno,A.vdrna,A.cgtype";

            Fill(sqlstr2, ds, "tbvdrno");

            Fill(sqlstr3, ds, "tbl2");


            foreach (DataRow item in ds.Tables["tbvdrno"].Rows)
            {
                foreach (DataRow row in ds.Tables["tbl2"].Rows)
                {
                    if (item["cgtype"].ToString() == row["cgtype"].ToString() && item["vdrno"].ToString() == row["vdrno"].ToString())
                    {
                        item["lm"] = row["lm"];
                        item["ly"] = row["ly"];
                        item["llj"] = row["llj"];
                    }
                }
            }



        }






        public override void ConfigData()
        {
            DataRow newrow;
            decimal puramtstotal = 0;

            foreach (DataRow row in ds.Tables["tbcglb"].Rows)
            {
                if (decimal.Parse(row["ty"].ToString()) != 0)
                {
                    puramtstotal += decimal.Parse(row["ty"].ToString());
                }
            }


            foreach (DataRow item in ds.Tables["tbcglb"].Rows)
            {
                foreach (DataRow item2 in ds.Tables["tbvdrno"].Rows)
                {
                    if (item["cgtype"].Equals(item2["cgtype"]))
                    {
                        if (decimal.Parse(item["ty"].ToString()) != 0)
                        {
                            item2["tyzb"] = decimal.Parse(String.Format("{0:N2}", 100 * decimal.Parse(item2["ty"].ToString()) / decimal.Parse(item["ty"].ToString())));
                        }
                        if (decimal.Parse(item["tm"].ToString()) != 0)
                        {
                            item2["tmzb"] = decimal.Parse(String.Format("{0:N2}", 100 * decimal.Parse(item2["tm"].ToString()) / decimal.Parse(item["tm"].ToString())));
                        }
                        if (decimal.Parse(item["lm"].ToString()) != 0)
                        {
                            item2["lmzb"] = decimal.Parse(String.Format("{0:N2}", 100 * decimal.Parse(item2["lm"].ToString()) / decimal.Parse(item["lm"].ToString())));
                        }
                        if (decimal.Parse(item["ly"].ToString()) != 0)
                        {
                            item2["lyzb"] = decimal.Parse(String.Format("{0:N2}", 100 * decimal.Parse(item2["ly"].ToString()) / decimal.Parse(item["ly"].ToString())));
                        }

                        newrow = ds.Tables["tblresult"].NewRow();
                        newrow[0] = item2[0];
                        newrow[1] = item2[1];
                        newrow[2] = item2[2];
                        newrow[3] = item2[3];
                        newrow[4] = item2[4];
                        newrow[5] = item2[5];
                        newrow[6] = item2[6];
                        newrow[7] = item2[7];
                        newrow[8] = item2[8];
                        newrow[9] = item2[9];
                        newrow[10] = item2[10];
                        newrow[11] = item2[11];
                        newrow[12] = 0;
                        newrow[13] = 0;
                        newrow[14] = "";
                        ds.Tables["tblresult"].Rows.Add(newrow);
                    }

                }
                newrow = ds.Tables["tblresult"].NewRow();
                newrow[0] = "小计";
                newrow[1] = "";
                newrow[2] = "";
                newrow[3] = 100;
                newrow[4] = 100;
                newrow[5] = item["ty"];
                newrow[6] = item["tm"];
                newrow[7] = item["lm"];
                newrow[8] = item["ly"];
                newrow[9] = item["llj"];
                newrow[10] = 100;
                newrow[11] = 100;
                newrow[12] = 0;
                newrow[13] = 0;
                if (decimal.Parse(item["ty"].ToString()) != 0)
                {
                    newrow[14] = String.Format("{0:N2}", 100 * decimal.Parse(item["ty"].ToString()) / puramtstotal);
                }
                else
                {
                    newrow[14] = "";
                }
                ds.Tables["tblresult"].Rows.Add(newrow);
            }


            foreach (DataRow item3 in ds.Tables["tblresult"].Rows)
            {

                if (decimal.Parse(item3["lm"].ToString()) != 0)
                {

                    item3["lmcz"] = decimal.Parse(String.Format("{0:N2}", 100 * (decimal.Parse(item3["tm"].ToString()) - decimal.Parse(item3["lm"].ToString())) / decimal.Parse(item3["lm"].ToString())));
                }
                if (decimal.Parse(item3["llj"].ToString()) != 0)
                {
                    item3["lycz"] = decimal.Parse(String.Format("{0:N2}", 100 * (decimal.Parse(item3["ty"].ToString()) - decimal.Parse(item3["llj"].ToString())) / decimal.Parse(item3["llj"].ToString())));
                }

            }





            ds.AcceptChanges();
        }


    }
}
