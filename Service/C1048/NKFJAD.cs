using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using System.IO;
using System.Data;

namespace Hanbell.AutoReport.Config
{
  public  class NKFJAD : NotificationContent
    {

      public NKFJAD()
      {

 }

      protected override void Init()
      {
          base.Init();

          nc = new NKFJADConfig(Core.DBServerType.MSSQL, "SHBOA");
          //nc = new NKFJADConfig(DBServerType.SybaseASE, "SHBERP");

          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = { "部门代号", "部门名称", "客服单号", "客服结案单号", "处理责任人工号", "处理责任人姓名", "单据年/月" };
          int[] width = { 150, 150, 200, 200, 150, 150, 150 };

          //string[] title = { };
          this.content = GetContent(nc.GetDataTable("tblresult"), title, width);
          
       
          //AddNotify(new MailNotify());

      }

      protected override void SendAddtionalNotification()
      {
          Hashtable args = new Hashtable();
          args = Base.GetParameter(this.ToString(), nc.ToString());
          //string mailcc;

          string[] title = new string[] { "部门代号", "部门名称", "客服单号", "客服结案单号", "处理责任人工号", "处理责任人姓名", "单据年/月" };
          int[] width = new int[] { 150, 150, 200, 200, 150, 150, 150 };
          string[] p = new string[] { };

          foreach (DataRow row in nc.GetDataTable("tblresult").Rows)
          {

              if (p.Contains(row["resdd007"].ToString())) continue;
              Array.Resize(ref p, p.Length + 1);
              p.SetValue(row["resdd007"].ToString(), p.Length - 1);

              nc.GetDataTable("tblresult").DefaultView.RowFilter = " resdd007='" + row["resdd007"].ToString() + "'";

              NotificationContent msg = new NotificationContent();
              //msg.AddTo--收件人 ；mailcc--抄送收件人主管
              msg.AddTo(row["resdd007"].ToString() + "@" + Base.GetMailAccountDomain());
              //mailcc = GetManagerIdByEmployeeIdFromOA(row["resdd007"].ToString());
              //if (mailcc.ToString() != "")
              //{
              //    mailcc = mailcc + "@" + Base.GetMailAccountDomain();
              //    msg.AddCc(mailcc);
              //}
              //msg.cc = this.cc;
              msg.subject = this.subject;
              msg.content = GetContent(nc.GetDataTable("tblresult").DefaultView.ToTable(), title, width);
              msg.AddNotify(new MailNotify());
              msg.Update();
              msg.Dispose();

          }


      }


    }
}
