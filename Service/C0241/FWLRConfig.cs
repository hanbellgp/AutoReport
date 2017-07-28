using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using Hanbell.AutoReport.Config;
using System.Data;

namespace C0241
{
    public class FWLRConfig : NotificationConfig
    {
        public FWLRConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new FWLRDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {

            string sqlstr = "select 'FWLL' as trtype,hzfwlld_d01.hzfwlld_d01002 as dh,hzfwlld_d01.hzfwlld_d01004 as itnbr,hzfwlld_d01.hzfwlld_d01005 as itdsc,hzfwlld_d01.hzfwlld_d01009 as qty" +
            ",hzfwlld.hzfwlld004 as fwdh,hzfwlld.hzfwlld003 as kfdh,'' as ddh,'201605' as mon,0.00 as amts,0.00 as yf,'' as fwzydh  from hzfwlld inner join hzfwlld_d01" +
            " on hzfwlld002=hzfwlld_d01002 and  hzfwlld004 in " +
            "(select distinct hzfwd006 from hzfwd  left join odmhzfwnr  on odmhzfwnr.odmfwnrdm=hzfwd014 and odmhzfwnr.odmfwdl=hzfwd013,hzfwd_d03,hzkfd,resda " +
            "where hzfwd004=hzkfd003 and hzfwd002=hzfwd_d03002 and resda002=hzfwd002 " +
            "and resda001='HZFWD'and resda021<>3 and resda021<>4 and substring(hzkfd003,0,3)<>substring(hzfwd006,0,3)   and resda021 in ('1','2')  " +
            "and year(hzfwd.CREATE_DATE) = 2016 )";
            //年份需要修改
            Fill(sqlstr, this.ds, "tblfwlld");

            sqlstr = "select 'WXLL' as trtype,hzwxlld_d01.hzwxlld_d01002 as dh,hzwxlld_d01.hzwxlld_d01004 as itnbr,hzwxlld_d01.hzwxlld_d01005 as itdsc,hzwxlld_d01.hzwxlld_d01007 as qty,'' as fwdh," +
                "isnull(kfdh,'') as kfdh,isnull(ddh,'') as ddh,'201605' as mon,0.00 as amts,0.00 as yf,'' as fwzydh " +
            " from hzwxlld  inner join hzwxlld_d01 on hzwxlld002=hzwxlld_d01002 where kfdh in (SELECT distinct hzkfjad003 FROM hzkfjad " +
            " where left(MODI_DATE,6)='201604')";
            Fill(sqlstr, this.ds, "tblfwlld");

            sqlstr = "select fy,hzfwzyd_v01002 from hzfwzyd_v01 ,resda WHERE resda002=hzfwzyd_v01002  and " +
                "  zylb<>4 and isnull(fwdh,'')='{0}' and isnull(kfdh,'')='{1}' and isnull(ddh,'')='{2}' ";

            String sqlcost = "select unitavgcst from invpri where itnbr='{0}' and yearmon='{1}'";
            string tmp,zydh;
            DataTable table;
            string[] pk = new string[] { };
            foreach (DataRow item in this.ds.Tables["tblfwlld"].Rows)
            {
                tmp = this.GetQueryString(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBERP"), string.Format(sqlcost, item["itnbr"], item["mon"]));
                if (tmp != null)
                {
                    item["amts"] = Decimal.Parse(tmp);
                }
                else
                {
                    item["amts"] = 0;
                }

                if (pk.Contains(item["fwdh"].ToString() + item["kfdh"].ToString() + item["ddh"].ToString())) continue;

                table = this.GetQueryTable(string.Format(sqlstr, item["fwdh"].ToString(), item["kfdh"].ToString(), item["ddh"].ToString()));

                if (table != null && table.Rows.Count > 0)
                {
                    tmp = table.Rows[0][0].ToString();
                    zydh = table.Rows[0][1].ToString();
                    if (tmp != null && tmp != "")
                    {
                        item["yf"] = Decimal.Parse(tmp);
                    }
                    else
                    {
                        item["yf"] = 0;
                    }
                    item["fwzydh"] = zydh;
                }
              

                Array.Resize(ref pk, pk.Length + 1);
                pk.SetValue(item["fwdh"].ToString()+item["kfdh"].ToString()+item["ddh"].ToString(), pk.Length - 1);


            }


        }

    }
}
