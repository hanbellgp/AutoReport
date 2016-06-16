using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Windows.Forms;
using System.IO;


namespace Hanbell.AutoReport.Service
{
    public partial class Jobs : ServiceBase
    {
        bool ret = true;

        public Jobs()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("Drizzling"))
            {
                System.Diagnostics.EventLog.CreateEventSource("Drizzling", "LightShell");
            }
            eventLog.Source = "Drizzling";
            eventLog.Log = "";
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Drizzling服务启动了");
            DirectoryInfo di = new DirectoryInfo(Base.GetServiceInstallPath() + "\\Data\\");
            if (!di.Exists) di.Create();
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Drizzling服务停止了");
        }

        private void SendNotification(Notification info)
        {
            try
            {
                info.Update();
                Base.SetNotificationLastTime(info.ToString());
                eventLog.WriteEntry(DateTime.Now.ToString() + "成功发送" + info.subject, EventLogEntryType.Information, 00000001);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (ret)
            {

                ret = false;
                try
                {
                    Base.ResetNotify();
                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry(ex.ToString());
                }
                //eventLog.WriteEntry(DateTime.Now.ToString() + "准备发送", EventLogEntryType.Information, 00000002);
                foreach (string item in Base.GetNotification())
                {
                    
                    try
                    {
                        Notification entity = Base.CreateNotification(item);
                        SendNotification(entity);
                        Base.SetNotificationOver(item);
                        entity.Dispose();
                    }
                    catch (Exception ex)
                    {
                        eventLog.WriteEntry(ex.ToString());
                    }
                    
                }
                //eventLog.WriteEntry(DateTime.Now.ToString() + "发送结束", EventLogEntryType.Information, 00000002);
                ret = true;

            }

        }


    }
}
