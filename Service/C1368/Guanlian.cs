using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Guanlian : NotificationContent
    {

        public Guanlian()
        {

        }

        protected override void Init()
        {
            base.Init();

            //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc = new GuanlianConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count>0)
            {
                SetAttachment();
            }

            string[] title = { "出货单号", "出货日期", "区域代码", "区域名称", "总公司客户代码", "币别", "汇率", "出货单序号", "订单号", "订单序号", "件号", "品号", "规格", "机型", "总公司单价", "出货数量", "总公司金额", "总公司客户名称", "cp_lcoin", "销售员代码", "销售员", "单位", "备注", "分公司客户代号","分公司客户名称","分公司客户出货试漏","分公司售价","分公司销售总价", "冷媒机型码","产品分类","指配客户地址","发票客户地址","公司地址"};
            int[] width =    { 150, 200, 200, 400, 150, 200, 200, 150, 150, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200,200, 200,200,200,100,100,200,200,200 };

            //string[] title = { };
            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
                
            AddNotify(new MailNotify());

        }



    }
}









