using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.EFGP
{
    public class WorkItemNeedSignConfig : NotificationConfig
    {

        public WorkItemNeedSignConfig()
        {
        }

        public WorkItemNeedSignConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new WorkItemNeedSignDataSet();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ProcessInstance.processDefinitionId,ProcessInstance.processInstanceName,ProcessInstance.serialNumber,ProcessInstance.subject,Users.id,Users.userName,");
            sb.Append("WorkItem.createdTime,N'未签核' as status,DateDiff(DAY, WorkItem.createdTime, getdate()) AS delaydays,OrganizationUnit.id as deptno,OrganizationUnit.organizationUnitName as dept ");
            sb.Append("FROM ProcessInstance,ParticipantActivityInstance,WorkItem,WorkAssignment,Users,Functions,OrganizationUnit ");
            sb.Append("WHERE ParticipantActivityInstance.contextOID = ProcessInstance.contextOID AND (ParticipantActivityInstance.currentState = 0 OR ParticipantActivityInstance.currentState = 1 OR  ParticipantActivityInstance.currentState = 6)  AND ");
            sb.Append("WorkItem.containerOID = ParticipantActivityInstance.OID AND  (WorkItem.currentState = 0 OR WorkItem.currentState = 1) AND  WorkAssignment.workItemOID = WorkItem.OID AND ");
            sb.Append("WorkAssignment.isNotice = 0 AND WorkAssignment.assigneeOID = Users.OID AND DATEDIFF(DAY, CONVERT(DATETIME, WorkItem.createdTime), GETDATE()) > 0 AND ");
            sb.Append("Users.OID = Functions.occupantOID AND Functions.isMain=1 AND Functions.organizationUnitOID = OrganizationUnit.OID AND ");
            sb.Append("ProcessInstance.processDefinitionId NOT IN {0} ORDER BY OrganizationUnit.id,Users.id,delaydays DESC ");
            Fill(String.Format(sb.ToString(), args["processDefinitionId"]), this.ds, "tblprocess");

        }

    }
}
