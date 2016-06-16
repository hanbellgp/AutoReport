using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class MisWork:NotificationContent
    {

        public MisWork()
        {
        }

        protected override void Init()
        {
            //base.Init();

            SetMailHead();

            nc = new MisWorkConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc.InitData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }

            string[] title = { "编号", "部门", "部门" };
            int[] width = { 60, 200, 200 };
            this.content = GetContent(nc.GetDataTable("tbl"),null);

            //AddNotify(new MailNotify());

        }

        protected override void SendAddtionalNotification()
        {

            string[] p = new string[] { };

            foreach (DataRow row in nc.GetDataTable("tbl").Rows)
            {
                if (!p.Contains(row["配合人"].ToString()))
                {
                    Array.Resize(ref p, p.Length + 1);
                    p.SetValue(row["配合人"].ToString(), p.Length - 1);
                }
                else
                {
                    continue;
                }

                nc.GetDataTable("tbl").DefaultView.RowFilter=" 配合人='" + row["配合人"].ToString() + "'";

                NotificationContent msg = new NotificationContent();
                msg.subject = this.subject;
                msg.AddTo("C0160" + "@hanbell.com.cn");
                msg.content = GetContent(nc.GetDataTable("tbl").DefaultView.ToTable(), null);
                msg.AddNotify(new MailNotify());
                msg.Update();
            }



        }

    }
}
