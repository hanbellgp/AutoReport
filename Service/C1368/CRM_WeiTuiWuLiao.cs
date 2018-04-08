using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class CRM_WeiTuiWuLiao:NotificationContent
    {
        public CRM_WeiTuiWuLiao()
      {

 }

      protected override void Init()
      {
          base.Init();

          nc = new CRM_WeiTuiWuLiaoConfig(Core.DBServerType.MSSQL, "CRM", this.ToString());
          //nc = new CRM_WeiTuiWuLiaoConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
          nc.InitData();
          nc.ConfigData();

          if (nc.GetReportList() != null)
          {
              //SetAttachment();
          }

          string[] title = { "案件代號", "維修單別", "維修單號", "維修序號", "客戶代號", "客戶全名", "產品品號", "產品品名", "產品規格", "送修", "送修部门", "维修人员", "维修部门", "产品序号", "产品别", "区域别", "需核銷", "已核銷", "強制結案", "部門代號", "ERP服務領料單號", "ERP服務退料單號", "ERP借出單號", "ERP歸還單號", "品號", "品名", "實領數量" };
          int[] width = { 150, 50, 150, 30, 100, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 50 };

          //string[] title = { };
          //this.content = GetContent(nc.GetDataTable("CRM_WeiTuiWuLiao"), title, width);
          this.content = GetContentHead() + GetContentFooter();

          if (nc.GetDataTable("CRM_WeiTuiWuLiao").Rows.Count > 0)
          {
              string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "未退物料明细表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xls";
              DataTableToExcel(nc.GetDataTable("CRM_WeiTuiWuLiao"), fileFullName, true);
              AddNotify(new MailNotify());
          }
       

      }
    }
}
