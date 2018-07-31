using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class Fddxgb : NotificationContent
    {

      public Fddxgb()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new FddxgbConfig(DBServerType.SybaseASE, "SHBERP");

          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = { "订单编号", "部门代号", "部门名称", "客户", "客户名称", "品号", "客户品号", "接单数量", 
                "变更次数", "输入人员","业务人员","变更原因" };
          int[] width = { 150, 200, 300, 200, 150, 200, 200, 200, 150,100,100, 200 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("Fddxgb"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}
