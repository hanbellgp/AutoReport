using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class CDR680qlb : NotificationContent
    {

      public CDR680qlb()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new CDR680qlbConfig(DBServerType.SybaseASE, "SHBERP");
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList().Count>0)
          {
              SetAttachment();
          }

          string[] title = { "客户代号", "客户名称", "出货日期", "出货单号", "件号", "品名", "欠料数量", 
                };
          int[] width = { 100, 100, 100, 100, 150, 200, 80 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("CDR680qlb"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}
