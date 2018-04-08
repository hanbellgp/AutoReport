using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Purvdrxgb : NotificationContent
    {

        public Purvdrxgb()
        {

        }

        protected override void Init()
        {
            base.Init();

            nc = new PurvdrxgbConfig(Core.DBServerType.MSSQL, "SHBOA");
            //nc = new PurvdrxgbConfig(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }

            string[] title = { "申请日期", "申请人", "姓名", "申请部门", "部门名称", "厂商代号", "厂商名称", "厂商全程", 
                "厂商分类", "单位名称","付款方式","地址","银行名称","银行帐号","税号","联络人","电话1",
            "电话2","传真","其他","修改后单位名称","修改后付款方式","修改后其他付款方式","修改后地址","修改后银行名称",
            "修改后银行帐号","修改后税号","修改后联络人","修改后电话1","修改后电话2","修改后传真","修改后其他","原因说明",};
            int[] width = { 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150
                          ,150, 150, 150, 150, 150, 150, 150, 150, 150, 150,150,150,150};

            //string[] title = { };
            //this.content = GetContent(nc.GetDataTable("tblresult"), title, width);
            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();


            AddNotify(new MailNotify());

        }



    }
}
