using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderDelayConfig:SpecOrderNotificationConfig
    {

        public SpecOrderDelayConfig()
        {
        }

        public SpecOrderDelayConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSSpecOrderNotification();
            this.reportList.Add(new SpecOrderNotificationReport());
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {

            ResetSpecialOrderState();
            //被SpecOrderUpdateState取代,执行此程序前需先执行SpecOrderUpdateState,加快速度
            //ResetSpecialOrderDetailState();

            string sqlstr = "SELECT cdrspec.id,cdrspec.projname as project,cdrspec.itnbr as prod,cdrspec.itdsc as prodname,cdrspec.shipday1,cdrspec.shipday2,cdrspec.shipid," +
                "cdrspec.manday1,cdrspec.manday2,cdrspec.finday1,cdrspec.finday2,cdrspec.manid," +
                "cdrspecdetail.seq,cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty,cdrspecdetail.recqty,cdrspecdetail.rdman,cdrspecdetail.rdday1,cdrspecdetail.rdday2," +
                "cdrspecdetail.prman,cdrspecdetail.prday1,cdrspecdetail.prday2,cdrspecdetail.prno,cdrspecdetail.purman,cdrspecdetail.purday1,cdrspecdetail.purday2," +
                "cdrspecdetail.accday2,cdrspecdetail.recday2,cdrspecdetail.acceptno,cdrspecdetail.remark FROM cdrspec,cdrspecdetail   " +
                "WHERE ( cdrspec.facno = cdrspecdetail.facno ) and  ( cdrspec.id = cdrspecdetail.id ) and (cdrspec.status='Y') and (cdrspecdetail.state=0) " +
                "and ((datediff(day,getdate(),rdday1)<0 and (rdday2 is null)) or (datediff(day,getdate(),prday1)<0 and (prday2 is null))) " + 
                "and cdrspec.kindcode='{0}' ORDER BY cdrspec.facno ASC, cdrspec.id ASC    ";
            Fill(String.Format(sqlstr,args["kindcode"]), ds, "tblcdrspec");

            Fill("select userno,username from secuser ", ds, "tbluser");

        }

    }
}
