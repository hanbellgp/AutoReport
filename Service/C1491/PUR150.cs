using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    public class PUR150 : Hanbell.AutoReport.Config.NotificationContent
    {

        public PUR150()
        {

        }

        protected override void Init()
        {
            base.Init();


            nc = new C1491.ERPPUR150Config(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            //string[] title = {  "厂商代号", "厂商简称", "采购类别", "本年采购金额", "本月采购金额",
            //                     "去年同期采购金额", "去年全年采购金额","本年占比%" ,"本月占比%","同期去年占比%","同期去年全年占比%"};


            string[] title = { "公司别", "厂商代号", "厂商简称","品号" ,"品名","到期时间"};
            int[] width = { 100, 120, 150, 140, 150, 120 };
            this.content = GetContent(nc.GetDataTable("tbresult"), title, width);  

            AddNotify(new MailNotify());

        }


         
    }
}
