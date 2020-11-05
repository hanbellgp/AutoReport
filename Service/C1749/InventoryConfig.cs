using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class InventoryConfig : NotificationConfig
    {
        public InventoryConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new InventoryDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            //0-6月库存
            sb.Append(" select c.facno, c.protype as  protype, 0 as trnqys1, c.mb , 0 as cy , ");
            sb.Append(" c.trnqys1 as  trnqys2,0 as trnqys3 ,0 as trnqys4,0 as trnqys5,0 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno, b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20 when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) and convert(VARCHAR(8),lindate,112)  <= convert(VARCHAR(8),getdate(),112) ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT  LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112) >= convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) and convert(VARCHAR(8),lindate,112)  <= convert(varchar(8),getdate(),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr NOT  LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where  convert(VARCHAR(8),lindate,112) >= convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112)  ");
            sb.Append(" and convert(VARCHAR(8),lindate,112)  <= convert(varchar(8),getdate(),112)  ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  WHERE  convert(VARCHAR(8),lindate,112) >= convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112)  ");
            sb.Append(" and convert(VARCHAR(8),lindate,112)  <= convert(varchar(8),getdate(),112)  ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select  '柯茂' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) and convert(VARCHAR(8),lindate,112)  <= convert(varchar(8),getdate(),112) ");
            sb.Append(" and wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(),ds,"zbtlb1");

            sb.Length = 0;//清除sb的长度
            //现库存
            sb.Append(" select c.facno, c.protype as  protype, c.trnqys1 as trnqys1, c.mb , (convert(int,c.trnqys1,0) - convert(int,c.mb,0)) as cy , ");
            sb.Append(" 0 as  trnqys2,0 as trnqys3 ,0 as trnqys4,0 as trnqys5,0 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno, b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20  when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select  '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where   ");
            sb.Append("  wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT  LIKE '39%' ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select  '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where  ");
            sb.Append("  wareh in ('W01' ,'ASRS03') AND itnbr NOT  LIKE '39%' ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where  ");
            sb.Append(" wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  WHERE ");
            sb.Append(" wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select  '柯茂' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where  ");
            sb.Append("  wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(), ds, "zbtlb2");

            sb.Length = 0;//清除sb的长度
            //7-12月库存
            sb.Append(" select c.facno, c.protype as  protype, 0 as trnqys1, c.mb , 0 as cy , ");
            sb.Append(" 0 as  trnqys2,c.trnqys1 as trnqys3 ,0 as trnqys4,0 as trnqys5,0 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno, b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20  when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT  LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr NOT  LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where  convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112)  ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select '柯茂' as facno,  a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-6,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(), ds, "zbtlb3");

            sb.Length = 0;//清除sb的长度
            //1-2年库存
            sb.Append(" select c.facno, c.protype as  protype, 0 as trnqys1, c.mb , 0 as cy , ");
            sb.Append(" 0 as  trnqys2,0 as trnqys3 ,c.trnqys1 as trnqys4,0 as trnqys5,0 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno, b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20  when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT LIKE '39%' ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr NOT LIKE '39%' ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112)  ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal  where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select '柯茂' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-12,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(), ds, "zbtlb4");

            sb.Length = 0;//清除sb的长度
            //2-3年库存
            sb.Append(" select c.facno,  c.protype as  protype, 0 as trnqys1, c.mb , 0 as cy , ");
            sb.Append(" 0 as  trnqys2,0 as trnqys3 ,0 as trnqys4,c.trnqys1 as trnqys5,0 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno,  b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20  when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr NOT LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112)  ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select '柯茂' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  >= convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) and convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-24,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(), ds, "zbtlb5");

            sb.Length = 0;//清除sb的长度
            //3年以上库存
            sb.Append(" select c.facno, c.protype as  protype, 0 as trnqys1, c.mb , 0 as cy , ");
            sb.Append(" 0 as  trnqys2,0 as trnqys3 ,0 as trnqys4,0 as trnqys5,c.trnqys1 as trnqys6,'台' as dw from ( ");
            sb.Append(" select b.facno, b.protype, b.trnqys1, ");
            sb.Append(" (CASE b.protype when 'R制冷' then 450 when 'A机体' then 2000 when 'A机组' then 230 when 'P机体' then 240 when 'P机组' then 100 WHEN '涡旋' THEN 20 when '代理品' then 20  when '离心机组' then 5 when '螺杆机组' then 12 when 'ORC' then 5 else 1 END ) as mb ");
            sb.Append(" from ( select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280') then 'R制冷' ");
            sb.Append(" when itcls in ('3576','3579','3580','3676','3679','3680','3586','3589','3590') then 'A机组' ");
            sb.Append(" when itcls in ('3376','3379','3380','3476','3479','3480') then 'P机体' ");
            sb.Append(" when itcls in ('3776','3779','3780','3A76','3A79','3A80') then 'P机组' ");
            //<-- 20191227增加代理品
            sb.Append(" when itcls in ('6053') then '代理品' ");
            //-->
            sb.Append(" else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr NOT LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then 'A机体' else '其他' end )  as protype ");
            sb.Append(" from invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr NOT LIKE '39%'  ");
            sb.Append(" group by itcls )as a where a.protype <>'其他'  GROUP BY a.protype ");
            //涡旋
            sb.Append(" UNION ALL ");
            sb.Append(" select '汉钟' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590', ");
            sb.Append(" '3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal where convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112)  ");
            sb.Append(" and wareh in ('W01' ,'EW01','FTW01') AND itnbr LIKE '39%' ");
            sb.Append(" group by itcls  ");
            sb.Append(" UNION ALL ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls,   ");
            sb.Append(" (case when itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') then '涡旋' else '其他' end )  as protype  ");
            sb.Append(" from invbal where convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'ASRS03') AND itnbr LIKE '39%'  ");
            sb.Append(" group by itcls  ");
            sb.Append(" )as a where a.protype <>'其他' ");
            sb.Append(" GROUP BY a.protype ");
            //柯茂
            sb.Append(" UNION ALL ");
            sb.Append(" select '柯茂' as facno, a.protype,isnull(sum(a.trnqys1),0) as trnqys1 from ( ");
            sb.Append(" select isnull(sum(onhand1),0) as trnqys1,itcls, ");
            sb.Append(" (case when itcls in ('3H76','3H79','3H80') then '离心机组' ");
            sb.Append(" when itcls in ('3W76','3W79','3W80') then '螺杆机组' ");
            sb.Append(" when itcls in('3B76','3B79','3B80') then 'ORC' else '其他' end )  as protype ");
            sb.Append(" from [comererp].[dbo].invbal ");
            sb.Append(" where convert(VARCHAR(8),lindate,112)  < convert(VARCHAR(8),DATEADD(MONTH,-36,getdate()),112) ");
            sb.Append(" and wareh in ('W01' ,'FTW01') ");
            sb.Append(" group by itcls ");
            sb.Append(" )as a where a.protype <>'其他'  GROUP BY a.protype ");
            sb.Append(" ) as b ");
            sb.Append(" ) as c ORDER BY c.facno,c.protype ");
            Fill(sb.ToString(), ds, "zbtlb6");

            //添加到zbtlb1表中
            foreach (DataRow item in ds.Tables["zbtlb1"].Rows)
            {
                foreach (DataRow row in ds.Tables["zbtlb2"].Rows)
                {
                    if (item["protype"].ToString() == row["protype"].ToString())
                    {
                        item["trnqys1"] = row["trnqys1"];
                        item["cy"] = row["cy"];
                    }
                }
            }
            ds.Tables["zbtlb1"].AcceptChanges();

            //添加到zbtlb1表中
            foreach (DataRow item in ds.Tables["zbtlb1"].Rows)
            {
                foreach (DataRow row in ds.Tables["zbtlb3"].Rows)
                {
                    if (item["protype"].ToString() == row["protype"].ToString())
                    {
                        item["trnqys3"] = row["trnqys3"];
                    }
                }
            }
            ds.Tables["zbtlb1"].AcceptChanges();
            //添加到zbtlb1表中
            foreach (DataRow item in ds.Tables["zbtlb1"].Rows)
            {
                foreach (DataRow row in ds.Tables["zbtlb4"].Rows)
                {
                    if (item["protype"].ToString() == row["protype"].ToString())
                    {
                        item["trnqys4"] = row["trnqys4"];
                    }
                }
            }
            ds.Tables["zbtlb1"].AcceptChanges();
            //添加到zbtlb1表中
            foreach (DataRow item in ds.Tables["zbtlb1"].Rows)
            {
                foreach (DataRow row in ds.Tables["zbtlb5"].Rows)
                {
                    if (item["protype"].ToString() == row["protype"].ToString())
                    {
                        item["trnqys5"] = row["trnqys5"];
                    }
                }
            }
            ds.Tables["zbtlb1"].AcceptChanges();
            //添加到zbtlb1表中
            foreach (DataRow item in ds.Tables["zbtlb1"].Rows)
            {
                foreach (DataRow row in ds.Tables["zbtlb6"].Rows)
                {
                    if (item["protype"].ToString() == row["protype"].ToString())
                    {
                        item["trnqys6"] = row["trnqys6"];
                    }
                }
            }
            ds.Tables["zbtlb1"].AcceptChanges();


            //附件明细
            sb.Length = 0;
            sb.Append(" SELECT a.wareh,a.whdsc,a.itcls,a.itnbr,a.itdsc,a.varnr,a.onhand1,a.unmsr1,a.lindate, a.zlts FROM ( ");
            sb.Append(" select b.wareh,w.whdsc,b.itcls,b.itnbr,s.itdsc,t.varnr,t.onhand1,s.unmsr1,b.lindate, datediff(day,b.lindate,getdate()) as zlts from invbal b ");
            sb.Append(" LEFT JOIN invbat t on b.wareh = t.wareh and b.itnbr = t.itnbr ");
            sb.Append(" LEFT JOIN invwh w on b.wareh = w.wareh and b.facno=w.facno and b.prono = w.prono ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr and b.itcls = s.itcls ");
            sb.Append(" where b.wareh  in ('W01' ,'EW01','FTW01') ");
            sb.Append(" and b.itcls in ('3176','3177','3179','3180','3276','3279','3280','3576','3579','3580','3676','3679','3680','3586','3589','3590','3376','3379','3380','3476','3479','3480','3776','3779','3780','3A76','3A79','3A80','6053') ");
            sb.Append(" and t.onhand1>0 ");
            sb.Append(" UNION ALL ");
            sb.Append(" select b.wareh,w.whdsc,b.itcls,b.itnbr,s.itdsc,t.varnr,t.onhand1,s.unmsr1,b.lindate, datediff(day,b.lindate,getdate()) as zlts from invbal b ");
            sb.Append(" LEFT JOIN invbat t on b.wareh = t.wareh and b.itnbr = t.itnbr ");
            sb.Append(" LEFT JOIN invwh w on b.wareh = w.wareh and b.facno=w.facno and b.prono = w.prono ");
            sb.Append(" LEFT JOIN invmas s on b.itnbr = s.itnbr and b.itcls = s.itcls ");
            sb.Append(" where b.wareh  in ('W01','ASRS03') ");
            sb.Append(" and b.itcls in ('3886','3889','3890','3876','3879','3880','3976','3979','3980') ");
            sb.Append(" and t.onhand1>0 ");
            sb.Append(" UNION ALL ");
            sb.Append(" select b.wareh,w.whdsc,b.itcls,b.itnbr,s.itdsc,t.varnr,t.onhand1,s.unmsr1,b.lindate, datediff(day,b.lindate,getdate()) as zlts from [comererp].[dbo].invbal b ");
            sb.Append(" LEFT JOIN [comererp].[dbo].invbat t on b.wareh = t.wareh and b.itnbr = t.itnbr ");
            sb.Append(" LEFT JOIN [comererp].[dbo].invwh w on b.wareh = w.wareh and b.facno=w.facno and b.prono = w.prono ");
            sb.Append(" LEFT JOIN [comererp].[dbo].invmas s on b.itnbr = s.itnbr and b.itcls = s.itcls ");
            sb.Append(" where b.wareh  in ('W01' ,'FTW01') ");
            sb.Append(" and b.itcls in ('3H76','3H79','3H80','3W76','3W79','3W80','3B76','3B79','3B80') ");
            sb.Append(" and t.onhand1>0 ");
            sb.Append(" ) AS a ORDER BY a.wareh, zlts DESC  ");
            Fill(sb.ToString(),ds,"mxtlb");
            
        }
    }
}
