using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class SEIASBillConfig : NotificationConfig
    {
        public SEIASBillConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new SEIASDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            //StringBuilder sqlstr = new StringBuilder();
            //sqlstr.Append("  select h.fshdat as fshdat,h.fshno as fshno,d.inspno as inspno,d.initnbr as itnbrf,z.itnbr as itnbr,'' as zzjh, i.itdsc as itdsc,c.vdrna as vdrna ,d.godqty as godqty, ");
            //sqlstr.Append("'' as zrcs , '' as pric1 , '' as kkje1 , '' as xgcs , '' as jgc , '' as pric2 , '' as kkje2  ");
            //sqlstr.Append("from sfcwph h,sfcwpd d LEFT JOIN bomasd z on d.initnbr = z.itnbrf,manmas m,purvdr c,invmas i  ");
            //sqlstr.Append("where h.facno = d.facno and h.prono = d.prono and h.fshno = d.fshno  ");
            //sqlstr.Append("and d.facno = m.facno and d.prono = m.prono and d.manno = m.manno  ");
            //sqlstr.Append("and d.initnbr = i.itnbr and d.nwrcode = c.vdrno and h.facno = 'C' and h.prono = '1' and h.stats = '2' and  DateDiff(month,h.fshdat,getdate())<=2  ");
            //sqlstr.Append("UNION all ");
            //sqlstr.Append("SELECT  b.trdate  as fshdat,  b.trno as fshno,  b.dmark1 as inspno, b.itnbr as itnbrf,  m.itnbr as itnbr, '' as zzjh,   a.itdsc as itdsc,  c.vdrna as vdrna,  b.trnqy1 as godqty ,");
            //sqlstr.Append("  '' as zrcs , '' as pric1 , '' as kkje1 , '' as xgcs , '' as jgc , '' as pric2 , '' as kkje2 ");
            //sqlstr.Append("    FROM invmas a,invtrnh b LEFT JOIN purvdr c on b.depno = c.vdrno,bomasd m ");
            //sqlstr.Append(" WHERE  b.itnbr = a.itnbr and a.itcls = b.itcls and b.itnbr = m.itnbr ");
            //sqlstr.Append(" and (b.trtype='IAS' or b.trtype='IJT') and b.facno = 'C' and b.prono = '1' and   DateDiff(month,b.trdate,getdate())<=2 ");
            //Fill(sqlstr.ToString(), ds, "SEIASTlb");

            string sql = @"   select h.fshdat as fshdat,h.fshno as fshno,d.inspno as inspno,d.initnbr as itnbrf,z.itnbr as itnbr,'' as zzjh, i.itdsc as itdsc,c.vdrna as vdrna ,d.godqty as godqty,
                        '' as zrcs , '' as pric1 , '' as kkje1 , '' as xgcs , '' as jgc , '' as pric2 , '' as kkje2
                        from sfcwph h,sfcwpd d LEFT JOIN bomasd z on d.initnbr = z.itnbrf,manmas m,purvdr c,invmas i
                        where h.facno = d.facno and h.prono = d.prono and h.fshno = d.fshno
                        and d.facno = m.facno and d.prono = m.prono and d.manno = m.manno
                        and d.initnbr = i.itnbr and d.nwrcode = c.vdrno and h.facno = 'C' and h.prono = '1' and h.stats = '2' and  (DateDiff(month,h.fshdat,getdate())<=2) 
                        UNION all 
                        SELECT  b.trdate  as fshdat,  b.trno as fshno,  b.dmark1 as inspno, b.itnbr as itnbrf,  m.itnbr as itnbr, '' as zzjh,   a.itdsc as itdsc,  c.vdrna as vdrna,  b.trnqy1 as godqty ,
                         '' as zrcs , '' as pric1 , '' as kkje1 , '' as xgcs , '' as jgc , '' as pric2 , '' as kkje2
                        FROM invmas a,invtrnh b LEFT JOIN purvdr c on b.depno = c.vdrno,bomasd m
                        WHERE  b.itnbr = a.itnbr and a.itcls = b.itcls and b.itnbr = m.itnbr
                         and (b.trtype='IAS' or b.trtype='IJT') and b.facno = 'C' and b.prono = '1' and   (DateDiff(month,b.trdate,getdate())<=2) ";
            Fill(sql, ds, "SEIASTlb");

            //StringBuilder sqlstr1 = new StringBuilder();
            //sqlstr1.Append("select bomasd.itnbrf as itnbrf1,bomash.itnbrf as itnbrf2,bomasd.itnbr FROM bomash LEFT JOIN  ");
            //sqlstr1.Append("bomasd ON bomasd.itnbrf=bomash.itnbrf   WHERE bomasd.itnbr<>'' and bomasd.invaldate > getdate() ");
            //Fill(sqlstr1.ToString(), ds, "bomitnbr");





        }

        #region 用for循环判断那个bom表里是否有资料，如果有就继续找，没有就返回当前传进取得值
        public override void ConfigData()
        {
            addERPdata();
            //string a = Getitnbr("27312-1132PD-A-9");

            DataTable zb = ds.Tables["SEIASTlb"];
            for (int i = 0; i < zb.Rows.Count; i++)
            {
                string jh = zb.Rows[i]["itnbrf"].ToString();//取到主表的父件号
                zb.Rows[i]["zzjh"] = Getitnbr(jh);//调用下面的方法执行
            }
            this.ds.AcceptChanges();
        }


        private string Getitnbr(string itnbrf)//递归方法寻找最后的子子件号
        {

            DataTable tbl = GetQueryTable("select itnbrf,itnbr from bomasd where bomasd.invaldate > getdate() and itnbrf='" + itnbrf + "'");
            if (tbl.Rows.Count > 0)//如果table不为空
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    return Getitnbr(tbl.Rows[0]["itnbr"].ToString());
                }
            }
            return itnbrf;
        }
        #endregion

        #region 把ERP的责任厂商相关厂商添加到主表中
        protected DataTable addERPdata()
        {
            string mesSql = "select RESPONSIBILITYCOMPANY,RESPONSIBILITYCOMPANYID,RELATEDCOMPANY,RELATEDCOMPANYID,PROJECTID from FLOW_FORM_UQF_S_NOW where  PROJECTID = '{0}' ";
            DataTable erpDt = ds.Tables["SEIASTlb"];
            DataTable mesDt = getManagerOIDByEmployeeIdFromMES();
            DataTable mesDt2;
            try
            {
                for (int i = 0; i < erpDt.Rows.Count; i++)
                {
                    string inspno = erpDt.Rows[i]["inspno"].ToString();
                    mesDt2 = GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBMES"), String.Format(mesSql,inspno));

                    if (mesDt2 != null && mesDt2.Rows.Count > 0)
                    {
                        erpDt.Rows[i]["zrcs"] = mesDt2.Rows[0]["RESPONSIBILITYCOMPANY"];
                        erpDt.Rows[i]["xgcs"] = mesDt2.Rows[0]["RELATEDCOMPANY"];
                    }

                    //for (int j = 0; j < mesDt.Rows.Count; j++)
                    //{
                    //    string PROJECTID = mesDt.Rows[j]["PROJECTID"].ToString();
                    //    if (inspno.Equals(PROJECTID))
                    //    {
                    //        erpDt.Rows[j]["zrcs"] = mesDt.Rows[i]["RESPONSIBILITYCOMPANY"];
                    //        erpDt.Rows[j]["xgcs"] = mesDt.Rows[i]["RELATEDCOMPANY"];
                    //        erpDt.AcceptChanges();

                    //    }
                    //}
                }
                erpDt.AcceptChanges();

                return erpDt;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        protected DataTable getManagerOIDByEmployeeIdFromMES()
        {
            string sql001 = "select RESPONSIBILITYCOMPANYID,RESPONSIBILITYCOMPANY,RELATEDCOMPANYID,RELATEDCOMPANY,PROJECTID from FLOW_FORM_UQF_S_NOW where  DateDiff(month,PROJECTCREATETIME,getdate())<=3 order by PROJECTID";
            return GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBMES"), String.Format(sql001));
        }

        #endregion

        #region 循环主表判断责任厂商对应件号的价格，找到价格添加到主表（SEIASTlb）去
        public void addPrice()
        {
            DataTable zhub = ds.Tables["SEIASTlb"];
            //①1字头件号 就责任厂商对应件号的价格
            for (int i = 0; i < zhub.Rows.Count; i++)
            {
                string itn = zhub.Rows[i]["itnbrf"].ToString().Substring(1);
                //②2字头件号 就责任厂商对应件号的价格
                if (itn.Equals("1") || itn.Equals("2"))
                {
                    zhub.Rows[i]["pric1"] = GetPrice(zhub.Rows[i]["itnbrf"].ToString());
                }
                //③如果责任厂商=相关厂商就取2字头件号的价格
                //只有责任厂商就去2字头件号的价格 

                else if (itn.Equals("3") && zhub.Rows[i]["zrcs"] == zhub.Rows[i]["xgcs"] || zhub.Rows[i]["zrcs"] != null)
                {
                    zhub.Rows[i]["pric1"] = GetPrice(zhub.Rows[i]["itnbr"].ToString());
                }
                //④只有相关厂商就取2字头件号的价格
            }


        }

        private string GetPrice(string itnbr)
        {
            DataTable tbl = GetQueryTable("select top 1 itnbr,listunpri,* from purcontract where facno = 'C' and prono = '1' and enddate >= getdate()  and itnbr = '" + itnbr + "' ORDER BY pcdate DESC;");
            if ((tbl != null) && (tbl.Rows.Count > 0))
            {
                return tbl.Rows[0]["listunpri"].ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion


    }
}
