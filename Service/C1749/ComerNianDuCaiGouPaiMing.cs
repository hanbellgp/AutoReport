using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ComerNianDuCaiGouPaiMing : NotificationContent
    {
        public ComerNianDuCaiGouPaiMing()
        {

        }
        protected override void Init()
        {
            base.Init();

            nc = new ComerNainDuCaiGouMaiMingConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            string[] title = { "年度", "厂商代号", "厂商简称", "上月采购金额(万元)", "上月排名", "年度累计采购金额(万元)", "年度排名", "同期累计采购金额(万元)", "同期排名", "去年总采购金额(万元)", "去年排名", "較去年成長金額", "成長率", "厂商类别" };
            int[] width = { 80, 150, 150,100, 70, 150, 70, 150, 70, 150, 70,100,60,120 };
            DataTable dt = nc.GetDataTable("ndcgpm");
            //获取較去年成長金額 
            foreach (DataRow item in dt.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())) / Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in dt.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-"))
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }
            //<--2021/1/26增加厂商类别
            dt.Columns.Add("type", System.Type.GetType("System.String"));//插入新列
            DataTable dtType = getVdrType();//拿到有厂商类别的dt
            foreach (DataRow item in dt.Rows)
            {
                DataRow[] typeRow = dtType.Select("vdrno='" + item["vdrno"].ToString() + "'");//拿到这个厂商的类型集合
                if (typeRow != null)
                {
                    string[] a = typeRow.Select(row => row["Name"].ToString()).ToArray();//DataRow[] 转成 string[]
                    string type = string.Join(";", a);//转成string字符串
                    item["type"] = type;
                }
            }
            //-->

            dt.AcceptChanges();


            this.content = GetContent(dt, title, width);

            if (nc.GetDataTable("ndcgpm").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected DataTable getVdrType()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT DISTINCT a.Vdrno,c.Name FROM EC_SupErpNo a LEFT JOIN EC_STypeVdr b ");
            sb.Append(" ON a.SID=b.SID ");
            sb.Append(" LEFT JOIN EC_STypeA c ON b.STAID=c.STAID ");
            sb.Append(" WHERE a.Facno='K' AND a.Vdrno IS NOT NULL and c.Name is not null ");
            return nc.GetQueryTable(DBServerType.MSSQL, Base.GetDBConnectionString("SHBTMS"), sb.ToString());
        }
    }
}
