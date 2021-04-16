using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.IO;
namespace Hanbell.AutoReport.Config
{
    class PDMonthlyStatementData : NotificationContent
    {
        public PDMonthlyStatementData() { }

        protected override void Init()
        {
            base.Init();
            this.nc = new PDMonthlyStatementDataConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            try
            {
                //方形件刀具数
                string fileFullName3 = Base.GetServiceInstallPath() + "\\Data\\" + "方形件刀具数表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb3"), fileFullName3, true);

                //方型件钻头丝攻铣刀费用
                string fileFullName4 = Base.GetServiceInstallPath() + "\\Data\\" + "方型件钻头丝攻铣刀费用表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb4"), fileFullName4, true);

                //方型件刀柄类费用
                string fileFullName5 = Base.GetServiceInstallPath() + "\\Data\\" + "方型件刀柄类费用表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb5"), fileFullName5, true);

                //NSM
                string fileFullName6 = Base.GetServiceInstallPath() + "\\Data\\" + "NSM费用表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb6"), fileFullName6, true);

                //NL+CG
                string fileFullName7 = Base.GetServiceInstallPath() + "\\Data\\" + "NL+CG费用表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb7"), fileFullName7, true);

                //KAPP
                string fileFullName8 = Base.GetServiceInstallPath() + "\\Data\\" + "KAPP费用表" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + ".csv";
                DataTableToCSV(nc.GetDataTable("tlb8"), fileFullName8, true);
                //发送
                AddNotify(new MailNotify());
            }
            catch (Exception ex)
            {

            }
          
        }
        //
        protected void DataTableToCSV(System.Data.DataTable dtsource, string fileName, bool flag)
        {
            if (flag)
            {
                if (dtsource!=null)
                {
                    if (dtsource.Rows.Count>0)
                    {
                        //创建文件流(创建文件)
                        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                        //创建流写入对象，并绑定文件流
                        StreamWriter sw = new StreamWriter(fs);

                        string writeTitle = "";
                        for (int i = 0; i < dtsource.Columns.Count; i++)
                        {
                            writeTitle = writeTitle + dtsource.Columns[i].ToString() + ",";
                        }
                        sw.WriteLine(writeTitle.Substring(0, writeTitle.Length - 1));
                        foreach (System.Data.DataRow row in dtsource.Rows)
                        {
                            var rowArray = row.ItemArray;
                            var writeStr = string.Join(",", rowArray.Select(o => o.ToString()).ToArray());//要写入的每一行  1,2,3,4,5
                            //写入
                            sw.WriteLine(writeStr);
                        }
                        AddAtt(fileName); //加入附件中
                        //释放
                        sw.Close();
                        fs.Close();
                     }
                }
            }
        }

    }
}
