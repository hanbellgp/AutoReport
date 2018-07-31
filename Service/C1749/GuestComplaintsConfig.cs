using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class GuestComplaintsConfig:NotificationConfig
    {
        public GuestComplaintsConfig(DBServerType dbType, String connName, String notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new GuestComplaintsDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        //SQL 数据获取
        public override void InitData()
        {

            StringBuilder sqlStr = new StringBuilder();
            //sqlStr.Append("select BQ037 as 'endtime',BQ001 as 'jabh',BQ002 as 'cusno',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as 'cusna',");
            //sqlStr.Append(" BQ024 as 'hmark',BQ197 as 'nda',BQ198 as 'ndc',BQ129  as 'kslb',BQ130 as 'ischarge',BQ131 as 'zrpd',");
            //sqlStr.Append(" BQ035 as 'jam',BQ110 as 'isks',BQ132 as 'nwhy', BQ021 as 'begintime',x.MI017 as 'issbg'  from SERBQ,SERBR,(SELECT SERCA.CA001,");
            //sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017 ");
            //sqlStr.Append(" FROM SERCA AS SERCA ");
            //sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003 ");
            //sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1' ");
            //sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019 ");
            //sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011 ");
            //sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009 ) AS x ");
            //sqlStr.Append(" where SERBQ.BQ001=SERBR.BR001 and x.CA001=SERBQ.BQ001 AND BQ130 ='N' AND BQ131 like '%MF%' AND BQ035='Y' AND BQ110='Y' and BQ037>=DATEADD(day, -7, getdate())");
            //sqlStr.Append(" UNION ALL ");
            //sqlStr.Append(" select BQ037 as 'endtime',BQ001 as 'jabh',BQ002 as 'cusno',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as 'cusna',");
            //sqlStr.Append(" BQ024 as 'hmark',BQ197 as 'nda',BQ198 as 'ndc',BQ129  as 'kslb',BQ130 as 'ischarge',BQ131 as 'zrpd',");
            //sqlStr.Append(" BQ035 as 'jam',BQ110 as 'isks',BQ132 as 'nwhy', BQ021 as 'begintime',x.MI017 as 'issbg'  from SERBQ,SERBR,(SELECT SERCA.CA001,");
            //sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017 ");
            //sqlStr.Append(" FROM SERCA AS SERCA ");
            //sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003 ");
            //sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1' ");
            //sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019 ");
            //sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011 ");
            //sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009 ) AS x ");
            //sqlStr.Append(" where    SERBQ.BQ001=SERBR.BR001 and x.CA001=SERBQ.BQ001  AND BQ130 ='N'    AND BQ131 NOT LIKE  '%MF%' ");
            //sqlStr.Append(" AND MI017='N' AND BQ035='Y' AND BQ110='Y' and BQ037>=DATEADD(day, -7, getdate());");

            sqlStr.Append("select (SELECT ME002 FROM CMSME WHERE ME001=BQ017) as '责任部门', BQ001 as '案件编号',BQ002 as '客户代号',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as '客户简称',");
            sqlStr.Append(" BQ024 as '问题描述',(SELECT EL002 FROM SALEL WHERE EL001=BQ197) as '产品别',BQ198 as '区域别',");
            sqlStr.Append(" CASE WHEN BQ129=1 THEN '客户投诉'");
            sqlStr.Append(" WHEN BQ129=2 THEN '赠送'");
            sqlStr.Append(" WHEN BQ129=3 THEN '技术支持'");
            sqlStr.Append(" WHEN BQ129=4 THEN '统包服务'");
            sqlStr.Append(" WHEN BQ129=5 THEN '例行巡检'");
            sqlStr.Append(" WHEN BQ129=6 THEN '客户财产维修'");
            sqlStr.Append(" WHEN BQ129=7 THEN '收费服务'");
            sqlStr.Append(" WHEN BQ129=8 THEN '新机调试'");
            sqlStr.Append(" ELSE '其他' END AS '客诉类别',");
            sqlStr.Append(" CASE WHEN BQ131 ='HZMF' THEN '汉钟原因免费'");
            sqlStr.Append(" WHEN BQ131 ='HZSF' THEN '汉钟原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KHMF' THEN '客户原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KHSF' THEN '客户原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KMKHMF' THEN '柯茂客户原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KMKHSF' THEN '柯茂客户原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KMMF' THEN '柯茂原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KMSF' THEN '柯茂原因收费'");
            sqlStr.Append(" ELSE '其他' END AS '责任判定',BQ130 as '收费否',");
            sqlStr.Append(" BQ035 as '结案码',BQ037 as '结案日期',BQ110 as '是否客诉',BQ132 as '原因分析说明', BQ021 as '接案日期',x.MI017 as '是否在原厂保固期'  from SERBQ,SERBR,(SELECT SERCA.CA001,");
            sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017 ");
            sqlStr.Append(" FROM SERCA AS SERCA ");
            sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003 ");
            sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1' ");
            sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019 ");
            sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011 ");
            sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009  ) AS x");
            sqlStr.Append(" where SERBQ.BQ001=SERBR.BR001 and x.CA001=SERBQ.BQ001 AND BQ130 ='N' AND BQ131 like '%KH%'  AND MI017='Y' AND BQ035='Y' AND BQ110='Y' and BQ037>=DATEADD(day, -7, getdate())");
            sqlStr.Append(" UNION ALL ");
            sqlStr.Append(" select (SELECT ME002 FROM CMSME WHERE ME001=BQ017) as '责任部门',BQ001 as '案件编号',BQ002 as '客户代号',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as '客户简称',");
            sqlStr.Append(" BQ024 as '问题描述',(SELECT EL002 FROM SALEL WHERE EL001=BQ197) as '产品别',BQ198 as '区域别',");
            sqlStr.Append(" CASE WHEN BQ129=1 THEN '客户投诉'");
            sqlStr.Append(" WHEN BQ129=2 THEN '赠送'");
            sqlStr.Append(" WHEN BQ129=3 THEN '技术支持'");
            sqlStr.Append(" WHEN BQ129=4 THEN '统包服务'");
            sqlStr.Append(" WHEN BQ129=5 THEN '例行巡检'");
            sqlStr.Append(" WHEN BQ129=6 THEN '客户财产维修'");
            sqlStr.Append(" WHEN BQ129=7 THEN '收费服务'");
            sqlStr.Append(" WHEN BQ129=8 THEN '新机调试'");
            sqlStr.Append(" ELSE '其他' END AS '客诉类别',");
            sqlStr.Append(" CASE WHEN BQ131 ='HZMF' THEN '汉钟原因免费' ");
            sqlStr.Append(" WHEN BQ131 ='HZSF' THEN '汉钟原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KHMF' THEN '客户原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KHSF' THEN '客户原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KMKHMF' THEN '柯茂客户原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KMKHSF' THEN '柯茂客户原因收费'");
            sqlStr.Append(" WHEN BQ131 ='KMMF' THEN '柯茂原因免费'");
            sqlStr.Append(" WHEN BQ131 ='KMSF' THEN '柯茂原因收费'");
            sqlStr.Append(" ELSE '其他' END AS '责任判定',BQ130 as '收费否',");
            sqlStr.Append(" BQ035 as '结案码',BQ037 as '结案日期',BQ110 as '是否客诉',BQ132 as '原因分析说明', BQ021 as '接案日期',x.MI017 as '是否在原厂保固期'  from SERBQ,SERBR,(SELECT SERCA.CA001,");
            sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017");
            sqlStr.Append(" FROM SERCA AS SERCA");
            sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003");
            sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1'");
            sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019");
            sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011");
            sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009 ) AS x");
            sqlStr.Append(" where SERBQ.BQ001=SERBR.BR001 and x.CA001=SERBQ.BQ001  AND BQ130 ='N' AND MI017='N' AND BQ035='Y' AND BQ110='Y' and BQ037>=DATEADD(day, -7, getdate())");


            Fill(sqlStr.ToString(), ds, "tlbGuest");
        }
       
    }
}
