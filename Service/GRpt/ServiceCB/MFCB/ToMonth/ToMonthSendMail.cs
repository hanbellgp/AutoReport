using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Data;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToMonth
{
    public class ToMonthSendMail : Hanbell.Common.Mail.MailSenderTemp
    {
        private string changeColor = "#D0D0D0";
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

        public DataTable TB_R_MB { get; set; }
        public DataTable TB_R_SJ { get; set; }
        public DataTable TB_R_KZL { get; set; }

        public DataTable TB_AA_MB { get; set; }
        public DataTable TB_AA_SJ { get; set; }
        public DataTable TB_AA_KZL { get; set; }

        public DataTable TB_AH_MB { get; set; }
        public DataTable TB_AH_SJ { get; set; }
        public DataTable TB_AH_KZL { get; set; }

        public DataTable TB_P_MB { get; set; }
        public DataTable TB_P_SJ { get; set; }
        public DataTable TB_P_KZL { get; set; }

        public DataTable TB_CM_MB { get; set; }
        public DataTable TB_CM_SJ { get; set; }
        public DataTable TB_CM_KZL { get; set; }

        public DataTable TB_ALL_MB { get; set; }
        public DataTable TB_ALL_SJ { get; set; }
        public DataTable TB_ALL_KZL { get; set; }


        private string HTM_R_MB { get; set; }
        private string HTM_R_SJ { get; set; }
        private string HTM_R_KZL { get; set; }

        private string HTM_AA_MB { get; set; }
        private string HTM_AA_SJ { get; set; }
        private string HTM_AA_KZL { get; set; }

        private string HTM_AH_MB { get; set; }
        private string HTM_AH_SJ { get; set; }
        private string HTM_AH_KZL { get; set; }

        private string HTM_P_MB { get; set; }
        private string HTM_P_SJ { get; set; }
        private string HTM_P_KZL { get; set; }

        private string HTM_CM_MB { get; set; }
        private string HTM_CM_SJ { get; set; }
        private string HTM_CM_KZL { get; set; }

        private string HTM_ALL_MB { get; set; }
        private string HTM_ALL_SJ { get; set; }
        private string HTM_ALL_KZL { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool BulidData()
        {
            HTM_R_MB = this.BuildTB(this.TB_R_MB, "white","");
            HTM_R_SJ = this.BuildTB(this.TB_R_SJ, this.ChangeColor, "");
            HTM_R_KZL = this.BuildTB(this.TB_R_KZL, "white", "%");

            HTM_AA_MB = this.BuildTB(this.TB_AA_MB, "white", "");
            HTM_AA_SJ = this.BuildTB(this.TB_AA_SJ, this.ChangeColor, "");
            HTM_AA_KZL = this.BuildTB(this.TB_AA_KZL, "white", "%");

            HTM_AH_MB = this.BuildTB(this.TB_AH_MB, "white", "");
            HTM_AH_SJ = this.BuildTB(this.TB_AH_SJ, this.ChangeColor, "");
            HTM_AH_KZL = this.BuildTB(this.TB_AH_KZL, "white", "%");

            HTM_P_MB = this.BuildTB(this.TB_P_MB, "white", "");
            HTM_P_SJ = this.BuildTB(this.TB_P_SJ, this.ChangeColor, "");
            HTM_P_KZL = this.BuildTB(this.TB_P_KZL, "white", "%");

            HTM_CM_MB = this.BuildTB(this.TB_CM_MB, "white", "");
            HTM_CM_SJ = this.BuildTB(this.TB_CM_SJ, this.ChangeColor, "");
            HTM_CM_KZL = this.BuildTB(this.TB_CM_KZL, "white", "%");

            HTM_ALL_MB = this.BuildTB(this.TB_ALL_MB, "white", "");
            HTM_ALL_SJ = this.BuildTB(this.TB_ALL_SJ, this.ChangeColor, "");
            HTM_ALL_KZL = this.BuildTB(this.TB_ALL_KZL, "white", "%");

            return true;

        }



        /// <summary>
        /// 得到要拼接的HTML
        /// </summary>
        /// <param name="bdtable"></param>
        /// <param name="changecolor"></param>
        /// <returns></returns>
        private string BuildTB(DataTable bdtable, string changecolor,string kzl)
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
                sb.Append("<table cellpadding='0' cellspacing='0' style='background-color:" + changecolor + ";border-bottom:0;border-right:solid 1px gray;'>");
                foreach (DataRow row in bdtable.Rows)
                {
                    sb.Append("<tr>");
                    for (int i = 0; i < bdtable.Columns.Count - 1; i++)
                    {
                        if (Convert.ToInt32(bdtable.Columns[i].ToString()) == thisMonth - 1)//高亮当月前一个月,当月数据还没有
                        {
                            sb.AppendFormat("<td style='color:" + txtHightLightColor + ";text-align:right'>{0}{1}</td>", row[i].ToString(), kzl);
                        }
                        else if (Convert.ToInt32(bdtable.Columns[i].ToString()) >= thisMonth)//
                        {
                            if (Convert.ToInt32(bdtable.Columns[i].ToString()) == 13)
                            {
                                sb.AppendFormat("<td style='text-align:right'>{0}{1}</td>", row[i].ToString(), kzl);
                            }
                            else
                            {
                                sb.Append("<td></td>");
                            }
                        }
                        else
                        {
                            sb.AppendFormat("<td style='text-align:right'>{0}{1}</td>", row[i].ToString(), kzl);
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
            //xmlHelp.XmlDom[
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

            bodyHtml = bodyHtml.Replace("${rptR_MB}", HTM_R_MB).Replace("${rptR_SJ}", HTM_R_SJ).Replace("${rptR_KZL}", HTM_R_KZL)
                                .Replace("${rptAA_MB}", HTM_AA_MB).Replace("${rptAA_SJ}", HTM_AA_SJ).Replace("${rptAA_KZL}", HTM_AA_KZL)
                                .Replace("${rptAH_MB}", HTM_AH_MB).Replace("${rptAH_SJ}", HTM_AH_SJ).Replace("${rptAH_KZL}", HTM_AH_KZL)
                                .Replace("${rptP_MB}", HTM_P_MB).Replace("${rptP_SJ}", HTM_P_SJ).Replace("${rptP_KZL}", HTM_P_KZL)
                                .Replace("${rptCM_MB}", HTM_CM_MB).Replace("${rptCM_SJ}", HTM_CM_SJ).Replace("${rptCM_KZL}", HTM_CM_KZL)
                                .Replace("${rptALL_MB}", HTM_ALL_MB).Replace("${rptALL_SJ}", HTM_ALL_SJ).Replace("${rptALL_KZL}", HTM_ALL_KZL)
                               .Replace("${nowdate}", RptDateTime.ToString("yyyy-MM-dd"))
                               .Replace("${nowdatetime}", DateTime.Now.ToString("yyyy-MM-dd"))
                               .Replace("${tableTiltle}", TableTiltle);
            //.Replace("${mark}", Hmark);


            //mailInfo.Body = bodyHtml;

            //flag = Hanbell.Common.Mail.MailSender.CreateMailSender.SendMail(mailInfo, Hanbell.Common.Mail.BodyType.BodyHtml);

            string From = xmlHelp.XmlDom.DocumentElement.SelectNodes("user")[0].InnerText;//发件人地址
            string Host = xmlHelp.XmlDom.DocumentElement.SelectNodes("server")[0].InnerText;
            string PassWord = xmlHelp.XmlDom.DocumentElement.SelectNodes("password")[0].InnerText;
            string To = xmlHelp.XmlDom.DocumentElement.SelectNodes("To")[0].InnerText;

            mail.From = new MailAddress(From);//发件人地址
            mail.To.Add(new MailAddress(To));
            mail.IsBodyHtml = true;
            mail.Body = bodyHtml;
            smtpServer.Host = Host;
            smtpServer.Credentials = new System.Net.NetworkCredential(From, PassWord);
            smtpServer.Send(mail);
            return flag;
        }


    }
}
