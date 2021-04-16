using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class EFGPForHK_CG017Config:NotificationConfig
    {
        public EFGPForHK_CG017Config() 
        {
            
        }

        public EFGPForHK_CG017Config(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new EFGPForHK_CG017DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder addSql = new StringBuilder();
            addSql.Append(" select pi.createdTime,h.SerialNumber,h.facno,h.vdrno,h.vdrna, ");
            addSql.Append(" (case vdrsta when '1' then '正式厂商' when '2' then '询价中厂商' when '3' then '拒绝' when '4' then '转口厂商' when '5' then '送样中厂商' else '' end )as vdrsta, ");
            addSql.Append("  h.vdrds,h.address,h.contactman,h.boss,h.tel1,h.fax1,h.purkind,h.ttbankna,h.ttname,h.uniform, ");
            addSql.Append(" (case fkfs when '01' then N'预付款' when '02' then N'货到付款' when '03' then N'当月结' when '04' then N'月结30天' when '05' then N'月结60天' when '06' then N'月结90天' ");
            addSql.Append(" when 'A1' then N'预付10~30%，验收60~80%，质保金5~10%' when 'A2' then N'预付10~50%，余款验收后付清' when 'A3' then N'预付50%以上，验收后付清' when 'QT' then N'其他' ");
            addSql.Append(" else '' end ) as fkfs,h.tickdays,h.taxrate,h.coin,h.begdate,h.cuycode,h.mark1, ");
            addSql.Append(" (case fj1 when '1' then N'营业执照' when '2' then N'开户银行许可证' when '3' then N'相关资质' when '4' then N'税务登记证' when '5' then N'其他' else '' end ) as fj1, ");
            addSql.Append(" Users.userName ");
            addSql.Append(" from  HK_CG016 h INNER JOIN  ProcessInstance pi on h.processSerialNumber=pi.serialNumber ");
            //addSql.Append(" INNER JOIN WorkItem wi on pi.contextOID=wi.contextOID ");
            addSql.Append(" INNER JOIN Users on h.userno=Users.id ");
            addSql.Append(" where convert(VARCHAR(4),pi.createdTime,112)>=convert(VARCHAR(4),getdate(),112) ORDER BY pi.createdTime ");

            Fill(addSql.ToString(),this.ds, "addtlb");

            StringBuilder updateSql = new StringBuilder();
            updateSql.Append(" select pi.createdTime,h.SerialNumber,h.facno,Users.userName,h.dept,h.vdrno,h.vdrna,h.vdrd, ");
            updateSql.Append(" (CASE h.purkind when '01' then N'原料类厂商'  when '02' then N'费用类厂商' when '03' then N'工程类厂商' when '04' then N'设备类厂商' when '9' then N'未定义'  when 'ZJ' then N'铸件' else h.purkind end) as purkind, ");
            updateSql.Append(" h.bvdrds1,h.btickdays,h.bfkfs1,h.baddress,h.bttbankno,h.bttbanknum,h.buniform,h.bcontactman,h.btel1,h.btel2,h.bfax1,h.belse, ");
            updateSql.Append(" h.vdrds,h.tickdays,h.fktype,h.address,h.ttbankno,h.ttbanknum,h.uniform,h.contactman,h.tel1,h.tel2,h.fax1,h.alse ");
            updateSql.Append(" from  HK_CG017 h INNER JOIN  ProcessInstance pi on h.processSerialNumber=pi.serialNumber ");
            //updateSql.Append(" INNER JOIN WorkItem wi on pi.contextOID=wi.contextOID ");
            updateSql.Append(" INNER JOIN Users on h.applyuser=Users.id ");
            updateSql.Append(" where convert(VARCHAR(4),pi.createdTime,112)>=convert(VARCHAR(4),getdate(),112) ORDER BY pi.createdTime ");

            Fill(updateSql.ToString(), this.ds, "updatetlb");
        }
    }
}
