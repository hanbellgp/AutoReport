using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Qinggousantianweipaozhuan_P3_K : Qinggousantianweipaozhuan_K
    {
        public Qinggousantianweipaozhuan_P3_K()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new QinggousantianweipaozhuanConfig_P3(Core.DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            this.content = GetContent(nc.GetDataTable("tblprocess"), null);

            if (nc.GetDataTable("tblprocess").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
