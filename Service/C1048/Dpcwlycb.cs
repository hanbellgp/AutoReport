using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class Dpcwlycb : NotificationContent
    {

      public Dpcwlycb()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new DpcwlycbConfig(DBServerType.SybaseASE, "SHBERP");
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList().Count>0)
          {
              SetAttachment();
          }

          string[] title = { "库号", "库名", "件号", "品名",  "期末库存数量", "交易数量","库存数量", "单位", "入库日期",  "异常天数",
                };
          int[] width = { 100, 100, 100, 100, 100, 100, 100, 80, 100, 80 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("Dpcwlycb"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}
