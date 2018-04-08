using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;


namespace Hanbell.AutoReport.Config
{
    class NianDuCaiGouPaiMing : NotificationContent
    {
        public NianDuCaiGouPaiMing() { 
        
        }

        protected override void Init()
        {
            base.Init();

            nc = new NianDuCaiGouPaiMingConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();


            string[] title = { "年度", "厂商代号", "厂商简称", "上月采购金额(万元)", "上月排名", "年度采购金额(万元)", "年度排名", "同期采购金额(万元)", "同期排名", "去年采购金额(万元)", "去年排名" };
            int[] width = { 80, 150, 150, 100, 70, 150, 70, 150, 70, 150, 70 };
            this.content = GetContent(nc.GetDataTable("ndcgpm"), title, width);
            
            if (nc.GetDataTable("ndcgpm").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
