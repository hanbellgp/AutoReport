using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class PCDBB : NotificationContent
    {

      public PCDBB()
      {

 }

      protected override void Init()
      {
          base.Init();

          nc = new PCDBBConfig(Core.DBServerType.MSSQL, "SHBOA");
          //nc = new FddxgbConfig(DBServerType.SybaseASE, "SHBERP");

          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              SetAttachment();
          }

          string[] title = { "执行回报单单号", "预订申请单号", "日期", "地点", "事由", "申请人", "姓名", 
                               "部门",  "部门名称", "起点里程" ,  "讫点里程",  "回厂日期", "里程", 
                               "车牌号" ,  "车辆编号" };
          int[] width = { 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("DSpcdbb"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}
