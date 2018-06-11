using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class CDR643Prompt : NotificationContent
    {
        public CDR643Prompt() 
        {

        }
        protected override void Init()
        {
            base.Init();
            nc = new CDRX643PromptConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            
            string[] title = { " 公司别", "异常时间", "异常栏位名称","单据编号","序号","异常类别","程式编号","异常栏位说明","现有资料","新的资料","异动人员","是否已送出Email"};
            int[] width = { 150, 150, 150, 150, 100, 150,100,150,100,100,100,150 };
            this.content = GetContent(nc.GetDataTable("cdr643"), title, width);
            if (nc.GetDataTable("cdr643").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
