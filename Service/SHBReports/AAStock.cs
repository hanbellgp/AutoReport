using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class AAStock : NotificationContent
    {

        public AAStock()
        {
        }

        protected override void Init()
        {
            base.Init();
            nc = new AAStockConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            string[] t = {"机型", "库存", "在制", "订单", "可接单" };
            int[] w = {150, 100, 100, 100, 100 };
            this.content = GetContent(nc.GetDataTable("tblresult"), t, w);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }




    }
}
