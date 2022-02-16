using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class HZ_JS014 : NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            this.nc = new HZ_JS014Config(DBServerType.MSSQL, "EFGP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                this.content = this.GetContent(nc.GetDataTable("tbl"), null, null);
                AddNotify(new MailNotify());
            }

        }

        protected override void SendAddtionalNotification()
        {
            string dept = null;
            String[] title = { "申请人","通报编号", "主题", "距有效日期剩余天数", "分发部门" };
            int[] width = { 100, 100, 100, 100 };
            string table;
            string deptno = "";
            //OA表单的集合 ->
            foreach (DataRow row in nc.GetDataTable("tbl").Rows)
            {
                //找到每一张表单上的部门
                dept = "";
                dept = row["depat"].ToString(); 
                //按分号拆分部门
                String[] arrDept = dept.Replace("\n", "").Trim().Split(';');
                
                nc.GetDataTable("tbl").DefaultView.RowFilter = " canu='" + row["canu"].ToString() + "'";
                if (arrDept != null) 
                {
                    foreach (string item in arrDept)
                    {
                        NotificationContent msg = new NotificationContent();
                        //截取部门代号
                        if (item != "" && item != null)
                        {
                            deptno = item != "" ? item.Substring(0, 5) : "";
                            //直接发送
                            if (deptno != "" && deptno != null)
                            {
                                msg.AddTo(GetMailAddressByDeptIdFromOA(deptno));
                              // string aaa = GetMailAddressByDeptIdFromOA(deptno);
                              //  msg.AddTo("C1900@Hanbell.com.cn");
                            }
                            msg.subject = this.subject;
                            table = GetHTMLTable(nc.GetDataTable("tbl").DefaultView.ToTable(), title, width);
                            msg.content = GetContentHead() + table + GetContentFooter();
                            msg.AddNotify(new MailNotify());
                            msg.Update();
                            msg.Dispose();
                        }
                    }
                }
            }

        }

        protected string GetMailAddressByDeptIdFromOA(string deptid)
        {
            if (nc == null) return "";
            string sqlstr = "SELECT mailAddress FROM Users WHERE OID =(SELECT managerOID FROM OrganizationUnit WHERE id = '{0}')";
            return nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr, deptid));
        }

    }
}
