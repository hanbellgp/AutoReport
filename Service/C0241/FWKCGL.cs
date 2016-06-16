using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0241
{
    public class FWKCGL : NotificationContent
    {

        public FWKCGL()
        {
        }

        protected override void Init()
        {

            base.Init();
            nc = new FWKCGLConfig(Hanbell.AutoReport.Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();

            if (nc.GetReportList().Count>0)
            {
                SetAttachment();
            }

            this.content = GetContentHead() + "<br/><br/><br/><br/>" +  GetContentFooter() ;
            AddNotify(new MailNotify());

        }


    }
}
