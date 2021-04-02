using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class HKYX013Report:NotificationContent
    {
        public HKYX013Report() { }

        protected override void Init()
        {
            base.Init();
            nc = new HKYX013ReportConfig(DBServerType.MSSQL, "EFGP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "公司别", "修改人员", "姓名", "部门代号", "部门名称", "单据类别", "主要影响单据", "修改日期", "业务员", "部门", "事业部", "区域别", "产品别", "整机/零件", "纳入服务", "差异金额" };
            int[] width = { 60, 70, 70, 70, 140, 80, 120, 100, 80, 80, 80, 80, 80, 80, 80, 80 };
            this.content = this.GetContent(nc.GetDataTable("tbl"), title, width);
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected override void SendAddtionalNotification()
        {
            string[] title = { "公司别", "修改人员", "姓名", "部门代号", "部门名称", "单据类别", "主要影响单据", "修改日期", "业务员", "部门", "事业部", "区域别", "产品别", "整机/零件", "纳入服务", "差异金额" };
            int[] width = { 80, 80, 80, 80, 120, 120, 120, 80, 70, 70, 70, 70, 70, 80, 80, 80 };
            DataTable dt = nc.GetDataTable("tbl");
            this.content = this.GetContent(dt, title, width);
            string[] optstr = new string[] { };
            foreach (DataRow item in dt.Rows)
            {
                if (optstr.Contains(item["applyUser"].ToString())) continue;//跳出重复值
                Array.Resize(ref optstr, optstr.Length + 1);
                optstr.SetValue(item["applyUser"].ToString(), optstr.Length - 1);
                dt.DefaultView.RowFilter = "applyUser='" + item["applyUser"].ToString() + "'";
                NotificationContent msg = new NotificationContent();
                var sndDT = dt.DefaultView.ToTable();
                this.setTableSum(sndDT);
                msg.content = this.GetContent(sndDT, title, width);
                msg.subject = this.subject;
                String bb = item["applyUser"].ToString().Trim() + "@" + Base.GetMailAccountDomain();
                msg.AddTo(item["applyUser"].ToString() + "@" + Base.GetMailAccountDomain());
                //msg.AddTo("C1749" + "@" + Base.GetMailAccountDomain());
                String deptno = item["appDept"].ToString().Substring(0, 2) + "000";
                msg.AddCc(GetManagerOIDByDeptnoFromOA(deptno));
                msg.AddCc(GetManagerIdByEmployeeIdFromOA(item["applyUser"].ToString().Trim()));
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

        private void setTableSum(DataTable dt) 
        {
            DataRow newRow = dt.NewRow();
            var a = dt.AsEnumerable();
            var sum = a.Sum(o => string.IsNullOrEmpty(o["differenceAmount"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["differenceAmount"]), 4));
            newRow["facno"] = "合计金额";
            newRow["differenceAmount"] = sum;
            dt.Rows.Add(newRow);
        }

        //重写表单内容
        protected override string GetContent(DataTable tbl, string[] title, int[] width)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append("<TABLE>");
            sb.Append("<tr valign='middle'>");
            sb.Append("<th rowspan='2' width='60'><span>公司别</span></th>");
            sb.Append("<th rowspan='2' width='70'><span>修改人员</span></th>");
            sb.Append("<th rowspan='2' width='70'><span>姓名</span></th>");
            sb.Append("<th rowspan='2' width='70'><span>部门代号</span></th>");
            sb.Append("<th rowspan='2' width='140'><span>部门名称</span></th>");
            sb.Append("<th rowspan='2' width='80'><span>单据类别</span></th>");
            sb.Append("<th rowspan='2' width='120'><span>主要影响单据</span></th>");
            sb.Append("<th rowspan='2' width='80'><span>修改日期</span></th>");
            sb.Append("<th colspan='7' width='70'><span>修改内容</span></th>");
            sb.Append("<th rowspan='2' width='70'><span>差异金额</span></th>");
            sb.Append("</tr>");
            sb.Append("<tr valign='middle'>");
            sb.Append("<th width='70'><span>业务员</span></th>");
            sb.Append("<th width='70'><span>部门</span></th>");
            sb.Append("<th width='70'><span>事业部</span></th>");
            sb.Append("<th width='70'><span>区域别</span></th>");
            sb.Append("<th width='70'><span>产品别</span></th>");
            sb.Append("<th width='80'><span>整机/零件</span></th>");
            sb.Append("<th width='80'><span>纳入服务</span></th>");
            sb.Append("</tr>");
            int colsCount = tbl.Columns.Count;
            int rowsCount = tbl.Rows.Count;
            object obj;
            for (int j = 0; j < rowsCount; j++)
            {
                sb.Append("<tr>");
                for (int k = 0; k < colsCount; k++)
                {
                    if ((tbl.Columns[k].DataType == System.Type.GetType("System.Double")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Decimal")) ||
                       (tbl.Columns[k].DataType == System.Type.GetType("System.Int16")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Int32")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Int64")))
                    {
                        //数字格式右对齐
                        sb.Append("<td class='number'>");
                    }
                    else
                    {
                        sb.Append("<td>");
                    }
                    obj = tbl.Rows[j][k];
                    if (obj == DBNull.Value || obj.ToString() == "")
                    {
                        // 如果是NULL则在HTML里面使用一个空格替换之  
                        obj = "&nbsp;";
                    }
                    if (tbl.Columns[k].DataType == System.Type.GetType("System.DateTime"))
                    {
                        if (obj.ToString() != "&nbsp;") sb.Append("<span>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                    }
                    else if (tbl.Columns[k].DataType == System.Type.GetType("System.Decimal"))
                    {
                        sb.Append("<span>" + String.Format("{0:N}", Decimal.Parse(obj.ToString())) + "</span>");
                    }
                    else
                    {
                        sb.Append("<span>" + obj.ToString().Trim() + "</span>");
                    }
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");
            sb.Append(GetContentFooter());
            return sb.ToString();
        }
    }

    
    
}
