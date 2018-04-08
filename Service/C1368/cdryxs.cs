using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
  public  class cdryxs : NotificationContent
    {

      public cdryxs()
      {

 }

      protected override void Init()
      {
          base.Init();

          //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
          nc = new cdryxsConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = {"类别","机型" };
          int[] width = {50,150  };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("cdryxs"), title, width);
          
       
          AddNotify(new MailNotify());

      }
     


    }
}