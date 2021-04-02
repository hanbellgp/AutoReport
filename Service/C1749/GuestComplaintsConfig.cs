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
            sqlStr.Append(" select SERBQ.CREATOR,(select MV002 from CMSMV where MV001= SERBQ.CREATOR) as CREATORNAME, ");
            sqlStr.Append("  BQ017,(SELECT ME002 FROM CMSME WHERE ME001=BQ017) as '部门简称',BQ198 as '区域别',(SELECT EL002 FROM SALEL WHERE EL001=BQ197) as '产品别',");
            sqlStr.Append("  BQ001 as '案件编号',BQ002 as '客户代号',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as '客户简称',");
            sqlStr.Append("  CA009 as '产品序号',");
            sqlStr.Append(" CASE WHEN BQ500=1 THEN '客户投诉'");
            sqlStr.Append("  WHEN BQ500=2 THEN '赠送'");
            sqlStr.Append("  WHEN BQ500=3 THEN '技术支持'");
            sqlStr.Append("  WHEN BQ500=4 THEN '统包服务'");
            sqlStr.Append("  WHEN BQ500=5 THEN '例行巡检'");
            sqlStr.Append("  WHEN BQ500=6 THEN '客户财产维修'");
            sqlStr.Append("  WHEN BQ500=7 THEN '收费服务'");
            sqlStr.Append("  WHEN BQ500=8 THEN '新机调试'");
            sqlStr.Append("  ELSE '其他' END AS '客诉类别',");
            sqlStr.Append(" CASE WHEN rtrim(BQ502)='HZZR' THEN '汉钟责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='HZKFZR' THEN '汉钟客户责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='KMKFZR' THEN '柯茂客户责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='KMZR' THEN '柯茂责任'");    
            sqlStr.Append("  ELSE '其他' END AS '责任判定',BQ110 as '是否客诉',BQ130 as '收费否',x.MI017 as '是否在原厂保固期',BQ024 as '问题描述',");
            sqlStr.Append("  BQ021 as '接案日期',BQ132 as '原因分析说明',BQ035 as '结案码',BQ037 as '结案日期'  from SERBQ,SERBR,(SELECT SERCA.CA001,CA009,");
            sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017 ");
            sqlStr.Append(" FROM SERCA AS SERCA ");
            sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003 ");
            sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1' ");
            sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019 ");
            sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011 ");
            sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009");
            sqlStr.Append(" ) AS x ");
            sqlStr.Append(" where    SERBQ.BQ001=SERBR.BR001 ");
            sqlStr.Append(" and x.CA001=SERBQ.BQ001 AND BQ130 ='N'");
            sqlStr.Append(" AND BQ502 like '%KF%'  AND MI017='Y'");
            sqlStr.Append(" AND BQ035='Y' AND BQ110='Y' and BQ037>=");
            sqlStr.Append(" DATEADD(day, -7, getdate())");

            sqlStr.Append(" UNION ALL");
            sqlStr.Append(" select SERBQ.CREATOR,(select MV002 from CMSMV where MV001= SERBQ.CREATOR) as CREATORNAME, ");
            sqlStr.Append(" BQ017,(SELECT ME002 FROM CMSME WHERE ME001=BQ017) as '部门简称',BQ198 as '区域别',(SELECT EL002 FROM SALEL WHERE EL001=BQ197) as '产品别',");
            sqlStr.Append(" BQ001 as '案件编号',BQ002 as '客户代号',(SELECT GG003 FROM CRMGG WHERE GG001=BQ002) as '客户简称',");
            sqlStr.Append(" CA009 as '产品序号',");
            sqlStr.Append(" CASE WHEN BQ500=1 THEN '客户投诉'");
            sqlStr.Append("  WHEN BQ500=2 THEN '赠送'");
            sqlStr.Append("  WHEN BQ500=3 THEN '技术支持'");
            sqlStr.Append("  WHEN BQ500=4 THEN '统包服务'");
            sqlStr.Append("  WHEN BQ500=5 THEN '例行巡检'");
            sqlStr.Append("  WHEN BQ500=6 THEN '客户财产维修'");
            sqlStr.Append("  WHEN BQ500=7 THEN '收费服务'");
            sqlStr.Append("  WHEN BQ500=8 THEN '新机调试'");
            sqlStr.Append("  ELSE '其他' END AS '客诉类别',");
            sqlStr.Append(" CASE WHEN rtrim(BQ502)='HZZR' THEN '汉钟责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='HZKFZR' THEN '汉钟客户责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='KMKFZR' THEN '柯茂客户责任'");
            sqlStr.Append("  WHEN rtrim(BQ502)='KMZR' THEN '柯茂责任'");  
            sqlStr.Append("  ELSE '其他' END AS '责任判定',BQ110 as '是否客诉',BQ130 as '收费否',x.MI017 as '是否在原厂保固期',BQ024 as '问题描述',");
            sqlStr.Append("  BQ021 as '接案日期',BQ132 as '原因分析说明',BQ035 as '结案码',BQ037 as '结案日期'  from SERBQ,SERBR,(SELECT SERCA.CA001,CA009,");
            sqlStr.Append(" CASE WHEN datediff(DAY,REPMI.MI009,CONVERT(VARCHAR(8),GETDATE(),112))>0 THEN 'N' ELSE 'Y' END AS MI017 ");
            sqlStr.Append(" FROM SERCA AS SERCA ");
            sqlStr.Append(" LEFT JOIN WARMB WARMB ON MB001=CA003 ");
            sqlStr.Append(" LEFT JOIN REPMQ REPMQ ON MQ001=CA010 AND MQ003='a1' ");
            sqlStr.Append(" LEFT JOIN SERAC SERAC ON AC001=CA019 ");
            sqlStr.Append(" LEFT JOIN REPTA REPTA ON REPTA.TA001 = SERCA.CA010 AND REPTA.TA002 = SERCA.CA011 ");
            sqlStr.Append(" LEFT JOIN REPMI REPMI ON MI002=CA009");
            sqlStr.Append(" ) AS x ");
            sqlStr.Append(" where SERBQ.BQ001=SERBR.BR001 ");
            sqlStr.Append(" and x.CA001=SERBQ.BQ001  AND BQ130 ='N'");
            sqlStr.Append(" AND MI017='N'");
            sqlStr.Append(" AND BQ035='Y' AND BQ110='Y'");
            sqlStr.Append(" and BQ037>=DATEADD(day, -7, getdate())");
            Fill(sqlStr.ToString(), ds, "tlbGuest");

        }
       
    }
}
