using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    public class CloudEquipmentReserve : Hanbell.AutoReport.Config.NotificationContent
    {

        public CloudEquipmentReserve()
        {

        }

        protected override void Init()
        {
            base.Init();


            nc = new C1491.CloudEquipmentReserveConfig(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            //string[] title = {  "厂商代号", "厂商简称", "采购类别", "本年采购金额", "本月采购金额",
            //                     "去年同期采购金额", "去年全年采购金额","本年占比%" ,"本月占比%","同期去年占比%","同期去年全年占比%"};


            string[] title = { "品号", "品名", "库号", "库名", "库存数" };
            int[] width = { 150, 200, 150, 200, 150};
            this.content = GetContent(nc.GetDataTable("tbresult"), title, width);  

            AddNotify(new MailNotify());

        }


         
    }
}
