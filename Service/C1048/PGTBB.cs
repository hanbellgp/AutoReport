using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class PGTBB : NotificationContent
    {

      public PGTBB()
      {

 }

      protected override void Init()
      {
          base.Init();

          nc = new PGTBBConfig(Core.DBServerType.MSSQL, "SHBOA");
          //nc = new FddxgbConfig(DBServerType.SybaseASE, "SHBERP");

          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              SetAttachment();
          }

          string[] title = { "表单单号", "申请日期", "申请人", "姓名", "部门", "文件大类", "申请原因", 
                               "申请用途",  "新图面名称", "新图面件号" ,  "存放地址" };
          int[] width = { 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("result"), title, width);
          //this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
       
          AddNotify(new MailNotify());

      }
     


    }
}
