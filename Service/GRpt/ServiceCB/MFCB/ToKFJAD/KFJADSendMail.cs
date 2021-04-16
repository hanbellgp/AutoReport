using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Mail;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToKFJAD
{
    public class KFJADSendMail : Hanbell.Common.Mail.MailSenderTemp
    {

        private string changeColor = "#D3E5FD";
        public string ChangeColor
        {
            get
            {
                return changeColor;
            }
            set
            {
                changeColor = value;
            }
        }

        /// <summary>
        /// 当月是否高亮显示
        /// </summary>
        public bool ThisMonthHightLight { get; set; }
        /// <summary>
        /// 高亮显示颜色
        /// </summary>
        public string HightLightColor { get; set; }

        public DataTable TB_R { get; set; }
        public DataTable TB_AA { get; set; }
        public DataTable TB_AH { get; set; }
        public DataTable TB_P { get; set; }
        public DataTable TB_CM { get; set; }
        public DataTable TB_ALL { get; set; }

        private string HTM_R { get; set; }
        private string HTM_AA { get; set; }
        private string HTM_AH { get; set; }
        private string HTM_P { get; set; }
        private string HTM_CM { get; set; }
        private string HTM_ALL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool BulidData()
        {
            HTM_R = this.BuildTB(this.TB_R, "#D3E5FD");
            HTM_AA = this.BuildTB(this.TB_AA, this.ChangeColor);
            HTM_AH = this.BuildTB(this.TB_AH, "#D3E5FD");
            HTM_P = this.BuildTB(this.TB_P, this.ChangeColor);
            HTM_CM = this.BuildTB(this.TB_CM, "#D3E5FD");
            HTM_ALL = this.BuildTB(this.TB_ALL, this.ChangeColor);

            return true;

        }



        /// <summary>
        /// 得到要拼接的HTML
        /// </summary>
        /// <param name="bdtable"></param>
        /// <param name="changecolor"></param>
        /// <returns></returns>
        private string BuildTB(DataTable bdtable, string changecolor)
        {
            StringBuilder sb = new StringBuilder();
            string txtHightLightColor = "black";
            int thisMonth = DateTime.Now.Month;
            if (this.ThisMonthHightLight == true)
            {
                txtHightLightColor = this.HightLightColor;
            }
            if (bdtable != null)
            {
                sb.Append("<table cellpadding='0' cellspacing='0' class='action' style='background-color:" + changecolor + ";'>");
                foreach (DataRow row in bdtable.Rows)
                {
                    sb.Append("<tr>");
                    for (int i = 0; i < bdtable.Columns.Count - 1; i++)
                    {
                        if (Convert.ToInt32(bdtable.Columns[i].ToString()) == thisMonth - 1)//高亮当月前一个月,当月数据还没有
                        {
                            sb.AppendFormat("<td style='color:" + txtHightLightColor + "'>{0}</td>", row[i].ToString());
                        }
                        else if (Convert.ToInt32(bdtable.Columns[i].ToString()) >= thisMonth)//
                        {
                            if (Convert.ToInt32(bdtable.Columns[i].ToString()) == 13)
                            {
                                sb.AppendFormat("<td>{0}</td>", row[i].ToString());
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                        }
                        else
                        {
                            sb.AppendFormat("<td>{0}</td>", row[i].ToString());
                        }
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");

            }
            return sb.ToString().Trim();
        }


        /// <summary>
        /// 自定发送
        /// </summary>
        public bool SendReport2()
        {
            bool flag = true;

            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient();



            //Hanbell.Common.Mail.MailInfo mailInfo = new Hanbell.Common.Mail.MailInfo();
            //mailInfo.Sender = "sys@hanbell.com.cn";

            Hanbell.Common.Xml.XmlHelp xmlHelp = new Hanbell.Common.Xml.XmlHelp();
            xmlHelp.XmlPath = "config\\common\\smtpSetting.xml";
            //mailInfo.Receiver = "C1150@hanbell.com.cn";
            // mailInfo.Receiver = "C0394@hanbell.com.cn;";


            //C0976@hanbell.com.cn;C1163@hanbell.com.cn;

            //mailInfo.Subject = Subject;

            string bodyHtml = Hanbell.Common.Mail.HtmlUtil.ReadHtml(base.TmpName);

            //bodyHtml = bodyHtml.Replace("${qtyTable}", QtyHtml)
            //                   .Replace("${amtTable}", AmtHtml)
            //                   .Replace("${nowdate}", RptDateTime.ToString("yyyy-MM-dd"))
            //                   .Replace("${nowdatetime}", DateTime.Now.ToString("yyyy-MM-dd"))
            //                   .Replace("${tableTiltle}", TableTiltle)
            //                   .Replace("${mark}", Hmark);

            bodyHtml = bodyHtml.Replace("${rptR}", HTM_R)
                                .Replace("${rptAA}", HTM_AA)
                                .Replace("${rptAH}", HTM_AH)
                                .Replace("${rptP}", HTM_P)
                                .Replace("${rptCM}", HTM_CM)
                                .Replace("${rptALL}", HTM_ALL)
                               .Replace("${nowdate}", RptDateTime.ToString("yyyy-MM-dd"))
                               .Replace("${nowdatetime}", DateTime.Now.ToString("yyyy-MM-dd"))
                               .Replace("${tableTiltle}", TableTiltle);
            //.Replace("${mark}", Hmark);


            //mailInfo.Body = bodyHtml;

            //flag = Hanbell.Common.Mail.MailSender.CreateMailSender.SendMail(mailInfo, Hanbell.Common.Mail.BodyType.BodyHtml);


            mail.From = new MailAddress("sys@hanbell.com.cn");//发件人地址
            mail.To.Add(new MailAddress("C1150@hanbell.com.cn"));
            mail.IsBodyHtml = true;
            mail.Body = bodyHtml;
            smtpServer.Host = "172.16.10.18";
            smtpServer.Credentials = new System.Net.NetworkCredential("sys@hanbell.com.cn", "123456");
            smtpServer.Send(mail);
            return flag;
        }




    }
}
