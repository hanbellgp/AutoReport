using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class AccountReceivableDelay_NJ : AccountReceivableDelay
    {

        public AccountReceivableDelay_NJ()
        {
        }

        protected override void Init()
        {
            SetMailHead();
            string htmlFile = "";
            this.nc = new AccountReceivableDelayConfig(Core.DBServerType.SybaseASE, "NJERP", this.ToString());
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
                    htmlFile = nc.ExportReportToHTML("AccountReceivableDelayReport_NJ_" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss"), item);
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
    }
}
