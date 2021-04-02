using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class GuestComplaints:NotificationContent
    {
        public GuestComplaints() { 
        }

        protected override void Init()
        {
            base.Init();
            nc = new GuestComplaintsConfig(DBServerType.MSSQL, "CRM", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "录入人员", "人员姓名", "责任部门", "部门简称", "区域别", "产品别", "案件编号", "客户代号", "客户简称", "产品序号", "客诉类别", "责任判定", "是否客诉", "收费否", "是否在原厂保固期", "问题描述", "接案日期", "原因分析说明", "结案码", "结案日期" };
            int[] width = { 80, 80, 80, 80, 120, 120, 120, 80, 80, 80, 80, 80, 80, 80, 80, 100, 80, 80, 80, 80 };
            DataTable dt = nc.GetDataTable("tlbGuest");
            content = GetContent(dt, title, width);
            //if (dt.Rows.Count > 0)
            //{
            //    AddNotify(new MailNotify());
            //}
        }

        protected override void SendAddtionalNotification()
        {
            string[] title = { "录入人员", "人员姓名", "责任部门", "部门简称", "区域别", "产品别", "案件编号", "客户代号", "客户简称", "产品序号", "客诉类别", "责任判定", "是否客诉", "收费否", "是否在原厂保固期", "问题描述", "接案日期", "原因分析说明", "结案码", "结案日期" };
            int[] width = { 80, 80, 80, 80, 120, 120, 120, 80, 80, 80, 80, 80, 80, 80, 80, 100, 80, 80, 80, 80 };
            this.content = GetContent(nc.GetDataTable("tlbGuest"), title, width);
            string[] optstr = new string[] { };
            foreach (DataRow item in this.nc.GetDataTable("tlbGuest").Rows)
            {
                if (optstr.Contains(item["CREATOR"].ToString())) continue;//跳出重复值
                Array.Resize(ref optstr, optstr.Length + 1);
                optstr.SetValue(item["CREATOR"].ToString(), optstr.Length - 1);
                this.nc.GetDataTable("tlbGuest").DefaultView.RowFilter = "CREATOR='" + item["CREATOR"].ToString() + "'";
                NotificationContent msg = new NotificationContent();
                msg.content = GetContent(nc.GetDataTable("tlbGuest").DefaultView.ToTable(), title, width);
                msg.subject = this.subject;
                String bb = item["CREATOR"].ToString().Trim() + "@" + Base.GetMailAccountDomain();
                msg.AddTo(item["CREATOR"].ToString() + "@" + Base.GetMailAccountDomain());
                //msg.AddTo("C1749" + "@" + Base.GetMailAccountDomain());
                String deptno = item["BQ017"].ToString().Substring(0, 2) + "000";
                msg.AddCc(GetManagerOIDByDeptnoFromOA(deptno) + "@" + Base.GetMailAccountDomain());
                msg.AddCc(GetManagerIdByEmployeeIdFromOA(item["CREATOR"].ToString().Trim()) + "@" + Base.GetMailAccountDomain());
                msg.AddCc("C0201" + "@" + Base.GetMailAccountDomain());
                msg.AddCc("C0005" + "@" + Base.GetMailAccountDomain());
                msg.AddNotify(new MailNotify());
                msg.Update();
                msg.Dispose();
            }
        }

        //部门主管
        protected string GetManagerOIDByDeptnoFromOA(string deptno)
        {
            if (nc == null) return "";
            string sqlstr = "select Users.id from OrganizationUnit,Users where Users.OID = OrganizationUnit.managerOID and OrganizationUnit.id='{0}'";
            return nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr, deptno));
        }
    }
}
