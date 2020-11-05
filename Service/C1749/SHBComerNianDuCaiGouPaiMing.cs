using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class SHBComerNianDuCaiGouPaiMing : NotificationContent
    {
        public SHBComerNianDuCaiGouPaiMing() { 
            
        }
        protected override void Init() {
            base.Init();

            nc = new SHBNianDuCaiGouPaiMingConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            NotificationConfig ncComer = new ComerNainDuCaiGouMaiMingConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
            ncComer.InitData();
            ncComer.ConfigData();

            NotificationConfig ncHanson = new NianDuCaiGouPaiMingConfig_HS(DBServerType.SybaseASE, "HansonERP", this.ToString());
            ncHanson.InitData();
            ncHanson.ConfigData();

            string[] title = { "年度", "厂商代号", "厂商简称", "上月采购金额(万元)", "上月排名", "年度累计采购金额(万元)", "年度排名", "同期累计采购金额(万元)", "同期排名", "去年总采购金额(万元)", "去年排名", "較去年成長金額", "成长率" };
            int[] width = { 80, 150, 150, 100, 70, 150, 70, 200, 70, 150, 70,100,60 };
            DataTable dt1 = nc.GetDataTable("ndcgpm");
            DataTable dt2 = ncComer.GetDataTable("ndcgpm");
            DataTable dt3 = ncHanson.GetDataTable("ndcgpm");
            //获取較去年成長金額
            foreach (DataRow item in dt1.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())) / Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in dt1.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-"))
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }

            foreach (DataRow item in dt2.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["ly_puramt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["ly_puramt"].ToString())) / Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in dt2.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-"))
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }

            foreach (DataRow item in dt3.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["ly_puramt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["ly_puramt"].ToString())) / Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in dt3.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-"))
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }

            dt1.AcceptChanges();
            dt2.AcceptChanges();
            dt3.AcceptChanges();
            this.content = GetContent(dt1, dt2, dt3, title, width);
            if (nc.GetDataTable("ndcgpm").Rows.Count > 0 && ncComer.GetDataTable("ndcgpm").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected string GetContent(DataTable tbl, DataTable tbl2, DataTable tbl3, string[] title, int[] width)
        {
            if (title != null && width != null && title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            //总部
            sb.Append("<p class='subject'>汉钟本年度采购厂商进货排名前30</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl, title, width));
            //上海柯茂
            sb.Append("<p class='subject'>柯茂本年度采购厂商进货排名前5</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            while (tbl2.Rows.Count > 5)
            {
                tbl2.Rows.RemoveAt(tbl2.Rows.Count - 1);
            }
            sb.Append(GetHTMLTable(tbl2, title, width));

            //浙江汉声
            sb.Append("<p class='subject'>汉声本年度采购厂商进货排名前15</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            while (tbl3.Rows.Count > 15)
            {
                tbl3.Rows.RemoveAt(tbl3.Rows.Count - 1);
            }
            sb.Append(GetHTMLTable(tbl3, title, width));
            sb.Append(GetContentFooter());
            return sb.ToString();
        }

    }
}
