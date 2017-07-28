using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class Bxdmx : NotificationContent
    {

      public Bxdmx()
      {

 }

      protected override void Init()
      {
          base.Init();

          nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          //nc = new BxdmxConfig(DBServerType.SybaseASE, "SHBERP");
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = { "公司别", "申请部门", "请款部门名称", "请款日期", "请款人", "姓名", "预算部门", "部门名称", 
                "金额", "招待对象", "招待日期", "招待人数", "招待原因", "备注说明", "表单单号", "预算科目", };
          int[] width = { 150, 200, 300, 200, 150, 200, 200, 200, 150, 200, 200, 200, 200, 200, 200, 200 };
          this.content = GetContent(nc.GetDataTable("Bxdmx"), title,width);

          AddNotify(new MailNotify());

      }
     


    }
}
