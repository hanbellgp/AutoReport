﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using Hanbell.AutoReport.Config;


namespace KNB.config
{
    class NianDuCaiGouPaiMing : NotificationContent
    {
        public NianDuCaiGouPaiMing() { 
        
        }

        protected override void Init()
        {
            base.Init();

            nc = new NianDuCaiGouPaiMingConfig(DBServerType.SybaseASE, "KNBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            string[] title = { "年度", "厂商代号", "厂商简称", "上月采购金额(万元)", "上月排名", "年度采购金额(万元)", "年度排名", "同期采购金额(万元)", "同期排名", "去年采购金额(万元)", "去年排名", "較去年成長金額", "成长率" };
            int[] width = { 80, 150, 150, 100, 70, 150, 70, 150, 70, 150, 70,100,60 };
            DataTable dt =nc.GetDataTable("ndcgpm");
            //获取較去年成長金額
            foreach (DataRow item in dt.Rows)
            {
                item["ly_grow"] = (Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())).ToString("0.00");
                item["ly_grower"] = ((Convert.ToDouble(item["y_puramt"].ToString()) - Convert.ToDouble(item["lm_puramt"].ToString())) / Convert.ToDouble(item["ly_puramt"].ToString())).ToString("0.00%");
            }

            foreach (DataRow item in dt.Rows)
            {
                item["ly_grower"] = "<p align=right>" + item["ly_grower"] + "</p>";
                if (item["ly_grower"].ToString().Contains("-")) 
                {
                    item["ly_grower"] = "<font color=red>" + item["ly_grower"] + "</font>";
                }
            }
            dt.AcceptChanges();
            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
            this.content = GetContent(dt, title, width);
            if (nc.GetDataTable("ndcgpm").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
