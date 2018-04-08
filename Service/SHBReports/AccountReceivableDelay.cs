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
    public class AccountReceivableDelay : NotificationContent
    {

        public AccountReceivableDelay()
        {
        }

        protected override void Init()
        {
            base.Init();
            string htmlFile = "";
            this.nc = new AccountReceivableDelayConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                nc.ConfigReport();
                string file;
                ExportFormatType type;

                foreach (ReportClass item in nc.GetReportList())
                {
                    if (item.FullResourceName == "Hanbell.AutoReport.Config.AccountReceivableDelayReport.rpt")
                    {
                        Hashtable args = new Hashtable();
                        args = Base.GetParameter(this.ToString(), nc.ToString());
                        int day1, day2, day3, day4, day5;
                        if (args == null || args.Count != 5)
                        {
                            day1 = 1; day2 = 2; day3 = 3; day4 = 4; day5 = 5;
                        }
                        else
                        {
                            day1 = int.Parse(args["day1"].ToString());
                            day2 = day1 + int.Parse(args["day2"].ToString());
                            day3 = day2 + int.Parse(args["day3"].ToString());
                            day4 = day3 + int.Parse(args["day4"].ToString());
                            day5 = day4 + int.Parse(args["day5"].ToString());
                        }
                        item.SetParameterValue("day1", day1);
                        item.SetParameterValue("day2", day2);
                        item.SetParameterValue("day3", day3);
                        item.SetParameterValue("day4", day4);
                        item.SetParameterValue("day5", day5);
                    }
                    file = Base.GetAttachmentFileName(this.ToString(), item.FullResourceName);
                    type = Base.GetAttachmentFileType(this.ToString(), item.FullResourceName);
                    htmlFile = nc.ExportReportToHTML("AccountReceivableDelayReport_" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss"), item);
                    file = nc.ExportReport(type, file, item);

                    AddAtt(file);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetMailHeadAdd(this.ToString()));
            sb.Append("<br><br>");
            foreach (string item in GetHTMLFileContents(htmlFile, "src", "png", 5, 3))
            {
                sb.Append("<img src='");
                sb.Append(Base.GetWebServerAddress());
                sb.Append(item);
                sb.Append("'/>");
            }
            sb.Append(GetMailFooterAdd(this.ToString()));
            this.content = sb.ToString();


            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }

        protected override void SendAddtionalNotification()
        {
            Hashtable args = new Hashtable();
            args = Base.GetParameter(this.ToString(), nc.ToString());
            string mailcc;
            int day1, day2, day3, day4, day5;
            if (args == null || args.Count != 5)
            {
                day1 = 1; day2 = 2; day3 = 3; day4 = 4; day5 = 5;
            }
            else
            {
                day1 = int.Parse(args["day1"].ToString());
                day2 = day1 + int.Parse(args["day2"].ToString());
                day3 = day2 + int.Parse(args["day3"].ToString());
                day4 = day3 + int.Parse(args["day4"].ToString());
                day5 = day4 + int.Parse(args["day5"].ToString());
            }
            string[] title = new string[] {"产品","区域","客户代码","客户简称","业务","姓名","未逾期","逾期款","本月到期","逾期"+day1+"月",
                "逾期"+day2+"月","逾期"+day3+"月","逾期"+day4+"月","逾期"+day5+"月","超过"+day5+"月","账款合计","本月应收","逾期应收","本月总应收"};
            int[] width = new int[] { 40, 40, 70, 80, 60, 60, 80, 80, 80, 80, 80, 80, 80, 80, 80, 90, 80, 80, 80 };
            string[] p = new string[] { };

            foreach (DataRow row in nc.GetDataTable("tblresult").Rows)
            {

                if (p.Contains(row["userno"].ToString())) continue;
                Array.Resize(ref p, p.Length + 1);
                p.SetValue(row["userno"].ToString(), p.Length - 1);

                nc.GetDataTable("tblresult").DefaultView.RowFilter = " userno='" + row["userno"].ToString() + "'";

                NotificationContent msg = new NotificationContent();
                msg.AddTo(GetMailAddressByEmployeeIdFromOA(row["userno"].ToString()));
                //抄送直接主管
                mailcc = GetManagerIdByEmployeeIdFromOA(row["userno"].ToString());

                if (mailcc != null && mailcc.ToString() != "")
                {
                    msg.AddCc(GetMailAddressByEmployeeIdFromOA(mailcc));
                    //获取上上级主管,如果是华东晋能龙经理,就抄送给他
                    mailcc = GetManagerIdByEmployeeIdFromOA(mailcc);
                    if (mailcc != null && mailcc.ToString() == "C0045")
                    {
                        msg.AddCc(GetMailAddressByEmployeeIdFromOA(mailcc));
                    }
                }
                msg.subject = this.subject;
                msg.content = GetContent(nc.GetDataTable("tblresult").DefaultView.ToTable(), title, width);
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();

            }


        }

    }
}
