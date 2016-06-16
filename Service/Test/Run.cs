using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Hanbell.AutoReport.Core;
using System.Xml;
using System.IO;
using System.Collections;
using System.Threading;

namespace DrizzlingTest
{
    public partial class Run : Form
    {
        bool ret = true;
        private ArrayList jobList;

        public Run()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.EventLog.SourceExists("Drizzling"))
            {
                //try
                //{
                //    System.Diagnostics.EventLog.DeleteEventSource("Drizzling");
                //}
                //catch (Exception ex)
                //{

                //    throw ex;
                //}

            }
            DirectoryInfo di = new DirectoryInfo(Base.GetServiceInstallPath() + "\\Data\\");
            if (!di.Exists) di.Create();
            jobList = new ArrayList();
            timer1.Start();
        }

        private  void SendNotification(Object notificationName)
        {
            try
            {
                Notification entity = Base.CreateNotification(notificationName.ToString());
                entity.Update();
                entity.Dispose();
  
                //eventLog.WriteEntry(DateTime.Now.ToString() + "成功发送" + info.subject, EventLogEntryType.Information, 00000001);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
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
                    //eventLog.WriteEntry(ex.ToString());
                }
                //eventLog.WriteEntry(DateTime.Now.ToString() + "准备发送", EventLogEntryType.Information, 00000002);
                //先将任务状态更新为无效,避免多线程更新状态时配置文件独占无法访问
                foreach (string item in Base.GetNotification())
                {
                    jobList.Add(item);
                    try
                    {
                        Base.SetNotificationLastTime(item);
                        Base.SetNotificationOver(item);
                    }
                    catch (Exception ex)
                    {
                        //eventLog.WriteEntry(ex.ToString());
                    }

                }
                foreach (string item in jobList)
                {
                    try
                    {
                        Thread t = new Thread(SendNotification);
                        t.Start(item);
                    }
                    catch (Exception ex)
                    {
                        //eventLog.WriteEntry(ex.ToString());
                    }
                }
                //eventLog.WriteEntry(DateTime.Now.ToString() + "发送结束", EventLogEntryType.Information, 00000002);

                jobList.Clear();
                ret = true;

            }

        }

    }

}

