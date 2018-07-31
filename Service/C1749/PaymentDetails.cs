using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class PaymentDetails:NotificationContent
    {
        public PaymentDetails() { 
        
        }
        protected override void Init()
        {
            base.Init();
            nc = new PaymentDetailsConfig(DBServerType.SybaseASE,"SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "申请单号", "申请日期", "输入日期", "申请部门", "部门名称", "部门名称预算中心", "预算中心", "预算中心名称", "申请人员", "申请人员姓名", "输入人员", "输入人员姓名", "申请内容", "借货交易类别", "单价", "数量", "币别", "汇率", "费用金额", "预算科目", "科目名称" };
            int[] width = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 100 };
            this.content = GetContent(nc.GetDataTable("tlbpayment"), title, width);

            if (nc.GetDataTable("tlbpayment").Rows.Count > 0)
            {
                //AddNotify(new MailNotify());
            }
        }


        protected override void SendAddtionalNotification()
        {
            string table;
            string[] p = new string[] { };
            NotificationContent msg = new NotificationContent();
            string[] title = { "申请单号", "申请日期", "输入日期", "申请部门", "部门名称", "部门名称预算中心", "预算中心", "预算中心名称", "申请人员", "申请人员姓名", "输入人员", "输入人员姓名", "申请内容", "借货交易类别", "单价", "数量", "币别", "汇率", "费用金额", "预算科目", "科目名称" };
            int[] width = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 100 };

            foreach (DataRow row in nc.GetDataTable("tlbpayment").Rows)
            {
                if (!p.Contains(row["apusrno"].ToString()))
                {
                    Array.Resize(ref p, p.Length + 1);
                    p.SetValue(row["apusrno"].ToString(), p.Length - 1);
                    msg.AddTo(GetMailAddressByEmployeeIdFromOA(row["apusrno"].ToString()));
                    //msg.AddTo(GetMailAddressByEmployeeIdFromOA("C1749"));
                }
                if (row["apusrno"].ToString() != "" && row["apusrno"].ToString().Equals("C0002"))
                {
                    msg.AddCc("C0616@hanbell.com.cn");
                    //msg.AddCc("C1749@hanbell.com.cn");
                }
                
            }
            table = GetHTMLTable(nc.GetDataTable("tlbpayment").DefaultView.ToTable(), title, width);
            if (nc.GetDataTable("tlbpayment").DefaultView.ToTable().Rows.Count > 0)
            {
                table = "以下是跨部门请款明细表,请核对 <br/>" + table;
            }
            msg.subject = this.subject;
            msg.content = GetContentHead() + table + GetContentFooter();
            msg.AddNotify(new MailNotify());
            msg.Update();
            msg.Dispose();
        }

    }
}
