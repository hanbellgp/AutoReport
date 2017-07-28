using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class CHU : NotificationContent
    {

      public CHU()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new CHUConfig(DBServerType.SybaseASE, "SHBERP");
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = { "出货单号", "出货日期", "区域名称", "区域代码", "客户代码", "币别", "汇率", "出货序号", "订单号", "订单序号", "件号", "品号", "规格", "机型", "单价",  "出货数量", "总金额", "客户名称", "数量单位", "销售员代码", "销售员", "单位", "备注", "总公司订单号", "冷媒机型码" };
          int[] width = { 150, 200, 400, 200, 150, 200, 200, 150, 150, 200, 200, 200, 200, 200, 200, 200, 200, 300, 100, 200, 200, 200, 200, 200, 200 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("CHU"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}









