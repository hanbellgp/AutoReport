using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class MisWorkConfig:NotificationConfig
    {
        public MisWorkConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSMisWork();
            this.reportList.Add(new MisWorkReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            Fill("select  dept1 as'需求部门',dept1C as '部门名称',empl2C as '需求人',empl1 as '配合人'," +
        "empl1C as '配合人姓名',datetime1 as '需求日期',datetime2 as '预计日期',datetime3 as '完成日期'," +
        "textarea2 as '工作内容',DateDiff (day,getdate(),datetime1)as '延迟天数'" +
        "from  zyd01,resda where  dept2='13000' and datetime1< getdate()and datetime3 is null and resda021=1 " +
        "and resda001='ZYD01'and resda002=zyd01002;",ds,"tbl");

            //Fill(DBServerType.MSSQL,Base.GetDBConnectionString("SHBOA2"),"select resal001,resal002,resal003 from resal ", ds, "tbl");
        }
    }
}
