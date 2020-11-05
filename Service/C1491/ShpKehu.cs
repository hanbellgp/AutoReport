using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class ShpKehu : NotificationContent
    {

        public ShpKehu()
        {

        }

        protected override void Init()
        {
            base.Init();

            //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc = new ShpKehuConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            string[] title = { "销售员代码", "销售员", "销售助理", "客户名称", "接单日期", "机型", "件号", "品号", "本次出货数量", "报价数量", "销售单价", "越权签核人", "合约价", "合约占比", "审批代码", "审批原因", "销售别", "客户代码",  "报价单", "产品别", "订单号", "出货单号", "序号1", "冷媒机型码", "dmark2", "序号2", "基准价", "占比", "标准定价单号" };
            int[] width = { 150, 300, 300, 200, 150, 200, 200, 200, 150, 150, 200, 300, 150, 150, 200, 200, 200, 200, 200, 200, 150, 200, 200, 150, 150, 100, 150, 200, 200, 200 };

            //string[] title = { };
            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();


            AddNotify(new MailNotify());

        }



    }
}