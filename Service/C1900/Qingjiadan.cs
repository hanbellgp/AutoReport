﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Qingjiadan : NotificationContent
    {
        public Qingjiadan()
        {
        }

        protected override void Init()
        {
            base.Init();
            nc = new QingjiadanConfig(DBServerType.MSSQL, "EFGP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("qingjiadantbl").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "OA请假单明细" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel2(nc.GetDataTable("qingjiadantbl"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }

        protected void DataTableToExcel2(System.Data.DataTable qingjiadantbl, string fileName, bool flag)
        {
            if (qingjiadantbl == null)
                return;
            int rowNum = qingjiadantbl.Rows.Count;
            int columnNum = qingjiadantbl.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;

            var excelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook newBook = excelApp.Workbooks.Add();
            //newBook.SaveAs(fileName);
            //将DataTable的列名导入Excel表第一行
            foreach (DataColumn col in qingjiadantbl.Columns)
            {
                columnIndex++;
                excelApp.Cells[rowIndex, columnIndex] = col.Caption;
            }

            //将DataTable中的数据导入Excel中
            Microsoft.Office.Interop.Excel.Range r = excelApp.get_Range(excelApp.Cells[1, 3], excelApp.Cells[rowNum + 1, columnIndex]);
            r.NumberFormat = "@";
            r.NumberFormatLocal = "@";
            for (int i = 0; i < rowNum; i++)
            {
                rowIndex++;//数据从第二行开始
                columnIndex = 0;
                for (int j = 0; j < columnNum; j++)
                {
                    columnIndex++;
                    //if (columnIndex == 3||columnIndex == 18)
                    //{
                    //    Microsoft.Office.Interop.Excel.Range r = excelApp.get_Range(excelApp.Cells[rowIndex, columnIndex], excelApp.Cells[rowIndex, columnIndex]);
                    //    r.NumberFormat = "@";
                    //    r.NumberFormatLocal = "@";
                    //}
                    excelApp.Cells[rowIndex, columnIndex] = qingjiadantbl.Rows[i][j].ToString();
                }
            }
            excelApp.Cells.Columns.AutoFit();

            newBook.SaveCopyAs(fileName);
            newBook.Close();
            excelApp.Workbooks.Close();
            excelApp.Quit();
            //Kill打开的Excel进程
            //Process[] excelApps;
            //excelApps = Process.GetProcessesByName("EXCEL");
            //foreach (Process p in excelApps)`1q   
            //{
            //    p.Kill();
            //}//End
            if (flag)
            {
                AddAtt(fileName); //加入附件中
            }

        }

    }
}
