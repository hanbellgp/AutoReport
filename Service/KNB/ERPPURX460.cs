using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace KNB.config
{
    class ERPPURX460 : NotificationContent
    {
        public ERPPURX460() { }

        protected override void Init()
        {
            base.Init();
            this.nc = new ERPPURX460Config(DBServerType.SybaseASE, "KNBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tbresult").Rows.Count > 0)
            {
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
                this.content = this.GetContent(nc.GetDataTable("tbresult"), null, null);
                AddNotify(new MailNotify());
            }

        }
        //《--发送给采购人员
        //protected override void SendAddtionalNotification()
        //{
        //    string table1;
        //    string[] p = new string[] { };

        //    foreach (DataRow row in nc.GetDataTable("tbresult").Rows)
        //    {
        //        if (p.Contains(row["userno"].ToString())) continue;
        //        Array.Resize(ref p, p.Length + 1);
        //        p.SetValue(row["userno"].ToString(), p.Length - 1);
        //        nc.GetDataTable("tbresult").DefaultView.RowFilter = " userno ='" + row["userno"].ToString() + "'";
        //        table1 = GetHTMLTable(nc.GetDataTable("tbresult").DefaultView.ToTable(), null, null);

        //        if (nc.GetDataTable("tbresult").DefaultView.ToTable().Rows.Count > 0)
        //        {
        //            table1 = "以下采购单未交货明细汇总表<br/>" + table1;
        //        }
        //        NotificationContent msg = new NotificationContent();
        //        msg.AddTo(GetMailAddressByEmployeeIdFromOA(row["userno"].ToString()));
        //        //string str1 = "</br>" + userno;
        //        //msg.AddCc(GetMailAddressByEmployeeIdFromOA("C1749"));
        //        msg.subject = this.subject;
        //        msg.content = GetContentHead() + table1 + GetContentFooter();
        //        msg.AddNotify(new MailNotify());
        //        msg.Update();
        //        msg.Dispose();
        //    }
        //}
        //--》


    }
}
