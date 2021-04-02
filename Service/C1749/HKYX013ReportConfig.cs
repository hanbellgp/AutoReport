using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class HKYX013ReportConfig:NotificationConfig
    {
        public HKYX013ReportConfig(DBServerType dbType, String connName, String notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new HKYX013ReportDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        //SQL 数据获取
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select h.facno,h.applyUser,u.userName,h.appDept,o.organizationUnitName, ");
            sb.Append(" (case d.invoiceType when '1' then N'订单' when '2' then N'出货单' when '3' then N'销退单' end ) as type, ");
            sb.Append(" d.singleNumber,h.applyDate, ");
            sb.Append(" (case when d.newMancode <> '' then '√' else ''  end ) as newMancode, ");
            sb.Append(" (case when d.newDeptno <> '' then '√' else ''  end ) as newDeptno, ");
            sb.Append(" (case when d.ncodeDA <> '' then '√' else ''  end ) as ncodeDA, ");
            sb.Append(" (case when d.ncodeCD  <> '' then '√' else '' end ) as ncodeCD, ");
            sb.Append(" (case when d.ncodeDC <> '' then '√' else '' end ) as ncodeDC, ");
            sb.Append(" (case when (d.ncodeDD <> '' and d.ncodeDD <> '0') then '√' else '' end ) as ncodeDD, ");
            sb.Append(" (case when (d.issevdta <> '' and d.issevdta <> '0') then '√' else '' end ) as issevdta, ");
            sb.Append(" d.differenceAmount ");
            sb.Append(" from HK_YX013 h,HK_YX013_Detail d,OrganizationUnit o,Users u ");
            sb.Append(" where h.formSerialNumber = d.formSerialNumber ");
            sb.Append(" and h.appDept = o.id and h.applyUser = u.id ");
            sb.Append(" and h.applyDate>='20201001' ");
            sb.Append(" ORDER BY h.appDept ");
 
            Fill(sb.ToString(), ds, "tbl");

        }
    }
}
