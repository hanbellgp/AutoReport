using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Threading;

namespace Hanbell.AutoReport.Config
{
   public class PDMonthlyStatementDataConfig : NotificationConfig
    {
        public PDMonthlyStatementDataConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new PDMonthlyStatementDataDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        
        public override void InitData()
        {
            /*添加到datatable里*/
            string aa = args["param"].ToString();
            if(aa.Equals("FX"))
            {
                //方形件产出工时
                String sql1 = setSQL("工时", "方型");
                Fill(sql1, ds, "tlb1");
            }
            else if (aa.Equals("YX"))
            {
                //圆形件产出工时
                String sql2 = setSQL("工时", "圆型");
                Fill(sql2, ds, "tlb2");
            }
            else {
                //方形件刀具数
                String sql3 = setSQL("成本", "刀具");
                Fill(sql3, ds, "tlb3");
                getCost(ds.Tables["tlb3"]);
                ds.AcceptChanges();

                //方型件钻头丝攻铣刀费用
                String sql4 = setSQL("成本", "钻头丝攻铣刀");
                Fill(sql4, ds, "tlb4");
                getCost(ds.Tables["tlb4"]);
                ds.AcceptChanges();

                //方型件刀柄类费用
                String sql5 = setSQL("成本", "刀柄类");
                Fill(sql5, ds, "tlb5");
                getCost(ds.Tables["tlb5"]);
                ds.AcceptChanges();

                //NSM
                String sql6 = setSQL("成本", "NSM");
                Fill(sql6, ds, "tlb6");
                getCost(ds.Tables["tlb6"]);
                ds.AcceptChanges();

                //NL+CG
                String sql7 = setSQL("成本", "NL+CG");
                Fill(sql7, ds, "tlb7");
                getCost(ds.Tables["tlb7"]);
                ds.AcceptChanges();

                //KAPP
                String sql8 = setSQL("KAPP", "KAPP");
                Fill(sql8, ds, "tlb8");
            }

        }

        //获取金额
        private void getCost(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    //传入件号获取ERP的成本
                    sb.Append(" SELECT TOP 1 round(unittotcst / unittotqy,2) FROM invpri WHERE facno = 'C' AND itnbr ='").Append(item["itnbr"].ToString()).Append("' ORDER BY yearmon desc ");
                    //建立连接 得到数据
                    String unitavgcst = GetQueryString(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBERP"), sb.ToString());
                    //清空StringBuilder数据 
                    sb.Length = 0;
                    //乘上数量 获取总金额
                    double trnqy = item["trnqy"] == null ? 0 : double.Parse(item["trnqy"].ToString());
                    //赋值
                    item["cost"] = trnqy * double.Parse(unitavgcst == null ? "0" : unitavgcst);
                }
                //刷新数据
                dt.AcceptChanges();
            }
        }

        //获取各报表的SQL
        public String setSQL(String type, String parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            try
            {
                if (!"".Equals(type))
                {
                    switch (type)
                    {
                        case "工时":
                            sb.Append(" SELECT sfcfsd.fshno,sfcfsd.manno,sfcfsd.itnbr, round(sum(sfcfsd.godqty * manbor.manstdtm),2) AS ccgs ");
                            sb.Append(" FROM sfcfsd,sfcfsh, manbor  ");
                            sb.Append(" WHERE  sfcfsd.facno = sfcfsh.facno     ");
                            sb.Append(" and sfcfsd.prono = sfcfsh.prono   ");
                            sb.Append(" and sfcfsd.fshno = sfcfsh.fshno   ");
                            sb.Append(" AND sfcfsd.nwrcode = sfcfsh.nwrcode   ");
                            sb.Append(" AND sfcfsd.facno = manbor.facno   ");
                            sb.Append(" AND sfcfsd.prono = manbor.prono ");
                            sb.Append(" AND sfcfsd.manno=manbor.manno   ");
                            sb.Append(" AND  sfcfsd.prosscode=manbor.prosscode   ");
                            sb.Append(" AND sfcfsd.itnbr = manbor.itnbrf  ");
                            sb.Append(" AND sfcfsd.wrcode = manbor.wrcode  ");
                            sb.Append(" AND sfcfsh.facno = manbor.facno  ");
                            sb.Append(" AND sfcfsh.prono = manbor.prono  ");
                            sb.Append(" AND sfcfsd.proseq = manbor.proseq  ");
                            sb.Append(" AND sfcfsh.facno = 'C' AND sfcfsh.prono = '1' ");
                            sb.Append(" AND sfcfsd.facno = 'C' AND sfcfsd.prono = '1' ");
                            sb.Append(" AND manbor.facno = 'C' AND manbor.prono = '1' ");
                            if (!"".Equals(parameters) && "方型".Equals(parameters))
                            {
                                sb.Append(" and (sfcfsd.prosscode = 'F3' OR sfcfsd.prosscode = 'F2' ) ");
                            }
                            else
                            {
                                sb.Append(" and sfcfsd.prosscode ='Y9'  ");
                            }
                            sb.Append(" and sfcfsh.assyn = 'N' ");
                            sb.Append(" and  (sfcfsd.stats = '2' OR sfcfsd.stats = '3')  ");
                            sb.Append(" and year(sfcfsh.fshdat)=year(dateadd(month,-1,getdate())) and month(sfcfsh.fshdat)=month(dateadd(month,-1,getdate()))   ");
                            sb.Append(" group by sfcfsd.fshno,sfcfsd.manno,sfcfsd.itnbr ");
                            break;
                        case "成本":
                            sb.Append(" select itnbr ,sum(trnqy1) as trnqy ,0 as cost from invtrnh  ");
                            sb.Append(" where  facno='C'  AND prono = '1'  ");
                            switch (parameters)
                            {
                                case "刀具":
                                    sb.Append(" and depno='1P100' and itnbr like 'BS101%' ");
                                    break;
                                case "钻头丝攻铣刀":
                                    sb.Append(" and depno='1P100' and (itnbr like 'BS102%'  or itnbr like 'BS103%'  or itnbr like 'BS104%'  or  itnbr like 'BS105%' )  ");
                                    break;
                                case "刀柄类":
                                    sb.Append(" and depno='1P100'  ");
                                    sb.Append(" and (itnbr like 'BS106%' or itnbr like 'BS151%'  or itnbr like 'BS152%'  or  itnbr like 'BS153%' or  ");
                                    sb.Append(" itnbr like 'BS154%' or  itnbr like 'BS155%'  or  itnbr like 'BS156%'  or  itnbr like 'BS158%')  ");
                                    break;
                                case "NSM":
                                    sb.Append(" and depno in('1P121','1P122') and itnbr  like 'BS101-01%'   ");
                                    break;
                                case "NL+CG":
                                    sb.Append(" and  depno in('1P121','1P122')   ");
                                    sb.Append(" and (( itnbr like 'BS101-06%'  or itnbr like 'BS101-07%'  or itnbr like 'BS101-08%' or itnbr like 'BS101-09%' ");
                                    sb.Append("  or  itnbr like 'BS108-01%'  or  itnbr like 'BS104-03%'  or  itnbr like 'BS109%'    or  itnbr like 'BS110%')  ");
                                    sb.Append(" and (  itnbr not like 'BS101-01%' and itnbr not like '52%'))  ");
                                    break;
                                default:
                                    break;
                            }
                            sb.Append(" and iocode='2' and trtype='IAB' ");
                            sb.Append(" and year(trdate)= year(dateadd(month,-1,getdate())) AND month(trdate)=month(dateadd(month,-1,getdate())) ");
                            sb.Append(" group by itnbr  ");



                            break;
                        case "KAPP":
                            sb.Append(" SELECT purdta.itnbr,round(sum(purdta.unpris),2)  AS unpris ");
                            sb.Append(" FROM purdta, purhad      ");
                            sb.Append(" WHERE purhad.facno = purdta.facno  ");
                            sb.Append(" AND purdta.prono = purhad.prono ");
                            sb.Append(" AND purhad.pono = purdta.pono  ");
                            sb.Append(" AND (purdta.dposta <> '99' OR purhad.hposta <> 'W')   ");
                            sb.Append(" AND purhad.vdrno='SZJ00100'  ");
                            sb.Append(" and purdta.itnbr like 'CS401%'  ");
                            sb.Append(" and year(purhad.podate)=year(dateadd(month,-1,getdate())) AND month(purhad.podate)=month(dateadd(month,-1,getdate()))    ");
                            sb.Append(" group by purdta.itnbr ");
                            break;
                        default:
                            break;
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return "";
        }
    }
}
