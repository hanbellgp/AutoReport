using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace C1491
{
    class ERPPUR530 : NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            this.nc = new ERPPUR530Config(DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tbresult").Rows.Count > 0)
            {
                this.content = this.GetContent(nc.GetDataTable("tbresult"), null, null);
                //AddNotify(new MailNotify());
            }

        }

        protected override void SendAddtionalNotification()
        {
            string table1;
            string[] p = new string[] { };
            string[] d = new string[]{ };
            NotificationContent msg = new NotificationContent();
            foreach (DataRow row in nc.GetDataTable("tbresult").Rows)
            {

                if (!p.Contains(row["userno"].ToString())) {
                    Array.Resize(ref p, p.Length + 1);
                    p.SetValue(row["userno"].ToString(), p.Length - 1);
                    //string userno = row["userno"].ToString();
                    msg.AddTo(GetMailAddressByEmployeeIdFromOA(row["userno"].ToString()));
                }
            }
            table1 = GetHTMLTable(nc.GetDataTable("tbresult").DefaultView.ToTable(), null, null);

            if (nc.GetDataTable("tbresult").DefaultView.ToTable().Rows.Count > 0)
            {
                table1 = "以下已点收GB 物料 ，请技术人员配合检验 <br/>" + table1;
            }
            msg.AddCc("C0733@hanbell.com.cn");
            msg.AddCc("1M110@hanbell.com.cn");
            msg.AddBcc("C1491@hanbell.com.cn");
            msg.subject = this.subject;
            msg.content = GetContentHead() + table1 + GetContentFooter();
            msg.AddNotify(new MailNotify());
            msg.Update();
            msg.Dispose();
        }


    }
}
