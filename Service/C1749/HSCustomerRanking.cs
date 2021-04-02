using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class HSCustomerRanking : NotificationContent
    {
        public HSCustomerRanking()
        {
        }
        protected override void Init()
        {
            base.Init();

            nc = new HSCustomerRankingConfig(DBServerType.SybaseASE, "HansonERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            int topItem = 20;

            string[] title = { "年度", "客户代号", "客户简称", "上月销售金额(万元)", "上月排名", "年度累计销售金额(万元)", "年度排名", "同期累计销售金额(万元)", "同期排名", "去年总销售金额(万元)", "去年排名", "較去年成長金額", "成長率" };
            int[] width = { 80, 150, 150, 100, 70, 150, 70, 150, 70, 150, 70, 100, 60 };
            DataTable dt = nc.GetDataTable("tbl");

            //取前20行 
            DataTable newTable;
            if (dt.Rows.Count > topItem)
            {
                newTable = dt.Clone();
                DataRow[] rows = dt.Select("1=1");
                for (int i = 0; i < topItem; i++)
                {
                    newTable.ImportRow((DataRow)rows[i]);
                }

                var a = dt.AsEnumerable();

                var b = a.ToList().Skip(20).Take(10000);

                //合并剩余行的数据
                var sum1 = b.Sum(o => string.IsNullOrEmpty(o["m_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["m_amt"]), 4));
                var sum2 = b.Sum(o => string.IsNullOrEmpty(o["y_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["y_amt"]), 4));
                var sum3 = b.Sum(o => string.IsNullOrEmpty(o["lm_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["lm_amt"]), 4));
                var sum4 = b.Sum(o => string.IsNullOrEmpty(o["ly_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["ly_amt"]), 4));
                var sum5 = b.Sum(o => string.IsNullOrEmpty(o["ly_grow"].ToString()) ? 0 : Convert.ToDecimal(o["ly_grow"]));
                DataRow newRow = newTable.NewRow();
                newRow[0] = newTable.Rows[0][0];
                newRow["cusno"] = "其他";
                newRow["cusna"] = "";
                newRow["m_amt"] = sum1;
                newRow["m_order"] = 0;
                newRow["y_amt"] = sum2;
                newRow["y_order"] = 0;
                newRow["lm_amt"] = sum3;
                newRow["lm_order"] = 0;
                newRow["ly_amt"] = sum4;
                newRow["ly_order"] = 0;
                newRow["ly_grow"] = sum5;
                newRow["ly_grower"] = 0;
                newTable.Rows.Add(newRow);
            }
            else 
            {
                newTable = dt.Copy();
            }
            //增加合计项
            var data = newTable.AsEnumerable();
            var tsum1 = data.Sum(o => string.IsNullOrEmpty(o["m_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["m_amt"]), 4));
            var tsum2 = data.Sum(o => string.IsNullOrEmpty(o["y_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["y_amt"]), 4));
            var tsum3 = data.Sum(o => string.IsNullOrEmpty(o["lm_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["lm_amt"]), 4));
            var tsum4 = data.Sum(o => string.IsNullOrEmpty(o["ly_amt"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["ly_amt"]), 4));
            var tsum5 = data.Sum(o => string.IsNullOrEmpty(o["ly_grow"].ToString()) ? 0 : Convert.ToDecimal(o["ly_grow"]));
           
            DataRow newTotalRow = newTable.NewRow();
            newTotalRow["yer"] = newTable.Rows[0][0];
            newTotalRow["cusno"] = "合计";
            newTotalRow["cusna"] = "";
            newTotalRow["m_amt"] = tsum1;
            newTotalRow["m_order"] = 0;
            newTotalRow["y_amt"] = tsum2;
            newTotalRow["y_order"] = 0;
            newTotalRow["lm_amt"] = tsum3;
            newTotalRow["lm_order"] = 0;
            newTotalRow["ly_amt"] = tsum4;
            newTotalRow["ly_order"] = 0;
            newTotalRow["ly_grow"] = tsum5;
            newTotalRow["ly_grower"] = 0;
            newTable.Rows.Add(newTotalRow);
            newTable.AcceptChanges();
 
            //获取較去年成長金額 
            foreach (DataRow item in newTable.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_amt"].ToString()) - Convert.ToDouble(item["lm_amt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_amt"].ToString()) - Convert.ToDouble(item["lm_amt"].ToString())) / Convert.ToDouble(item["lm_amt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in newTable.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-"))
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }
            newTable.AcceptChanges();


            this.content = GetContent(newTable, title, width);

            if (newTable.Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
