﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Cglb_zj : NotificationContent
    {

        public Cglb_zj()
        {

        }

        protected override void Init()
        {
            base.Init();


            nc = new Cglb_zjConfig(DBServerType.SybaseASE, "SHBERP");
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            //string[] title = {  "厂商代号", "厂商简称", "采购类别", "本年采购金额", "本月采购金额",
            //                     "去年同期采购金额", "去年全年采购金额","本年占比%" ,"本月占比%","同期去年占比%","同期去年全年占比%"};


            string[] title = { "采购类别", "厂商代号", "厂商简称","本年占比%" ,"本月占比%","本年金额", "本月金额",
                                 "去年同期金额", "去年全年金额","同期去年%","去年全年%","小类占比%"};
            int[] width = { 100, 70, 70, 80, 80, 80, 80, 90, 90, 80, 80, 70 };
            this.content = GetContent(nc.GetDataTable("tblresult"), title, width);

            AddNotify(new MailNotify());

        }



    }
}
