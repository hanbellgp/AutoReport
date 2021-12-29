using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class StockAndInProcess_S:NotificationContent
    {
        public StockAndInProcess_S() { }

        protected override void Init()
        {
            base.Init();
            nc = new StockAndInProcessConfig_S(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "机型码", "件号", "FSFL涡旋领料站", "FSSC涡旋试车站", "FSJY涡旋入库检验站", "EW01", "合计"};
            int[] width = { 120, 120, 150, 150, 150, 80, 80 };
            DataTable dataTable = base.nc.GetDataTable("tbl");
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["FSFL"] != null)
                {
                    num += Convert.ToDouble(row["FSFL"].ToString());
                }
                if (row["FSSC"] != null)
                {
                    num2 += Convert.ToDouble(row["FSSC"].ToString());
                }
                if (row["FSJY"] != null)
                {
                    num3 += Convert.ToDouble(row["FSJY"].ToString());
                }
                if (row["EW01"] != null)
                {
                    num4 += Convert.ToDouble(row["EW01"].ToString());
                }
                if (row["total"] != null)
                {
                    num5 += Convert.ToDouble(row["total"].ToString());
                }
            }
            DataRow dataRow2 = dataTable.NewRow();
            dataRow2["cmcmodel"] = "合计";
            dataRow2["itnbr"] = "";
            dataRow2["FSFL"] = num;
            dataRow2["FSSC"] = num2;
            dataRow2["FSJY"] = num3;
            dataRow2["EW01"] = num4;
            dataRow2["total"] = num5;
            dataTable.Rows.Add(dataRow2);
            dataTable.AcceptChanges();

            this.content = GetContent(nc.GetDataTable("tbl"), title, width);
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
