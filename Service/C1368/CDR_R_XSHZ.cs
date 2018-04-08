using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    
     public  class CDR_R_XSHZ : NotificationContent
    {

      public CDR_R_XSHZ()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new CDR_R_XSHZConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = {"分类" };
          int[] width = { 50 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("CDR_R_XSHZ"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}