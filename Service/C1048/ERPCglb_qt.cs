using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ERPCglb_qt:ERPCglb_zj
    {
        public ERPCglb_qt()
        { 
        }

        protected override void Init()
        {
            SetMailHead();


            //nc = new ERPCglb_qtConfig(DBServerType.SybaseASE, "SHBERP");
            nc = new ERPCglb_qtConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }


            string[] title = { "采购类别", "厂商代号", "厂商简称","本年占比%" ,"本月占比%","今年累计", "本月金额",
                                 "去年同期金额", "去年全年金额","去年累计金额","去年同期%","去年全年%","月成长比%","年累计成长比%","小类占比%"};
            int[] width = { 100, 70, 70, 80, 80, 80, 80, 90, 90, 90, 80, 80, 80, 80, 70 };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width); 

            AddNotify(new MailNotify());

        }
    }
}
