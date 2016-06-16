using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using CrystalDecisions.Shared;

namespace Hanbell.AutoReport.Core
{
    public class Base
    {
        public static string notificationConfigFileName = "NotificationConfig.xml";

        /// <summary>
        /// 创建消息对象
        /// </summary>
        /// <param name="objectname">消息类名称</param>
        /// <returns>Notification实例</returns>
        public static Notification CreateNotification(string objectname)
        {

            try
            {
                Type myType = Assembly.LoadFrom(GetServiceInstallPath() + "\\" + GetObjectFile(objectname)).GetType(objectname);
                if (myType == null)
                {
                    throw new NullReferenceException();
                }
                Notification myInstance = (Notification)Activator.CreateInstance(myType);
                return myInstance;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 将日期转换为指定的格式
        /// </summary>
        /// <param name="dateTime">以字符串表示的日期</param>
        /// <param name="format">日期格式</param>
        /// <returns></returns>
        public static string Format(string dateTime, string format)
        {
            try
            {
                return dateTime != "" ? DateTime.Parse(dateTime).ToString(format) : dateTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取消息附件名称
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        /// <param name="reportName">报表类名称</param>
        /// <returns></returns>
        public static string GetAttachmentFileName(string notificationName, string reportName)
        {
            XmlDocument xmldoc = GetXmlDocument("NotificationConfig.xml");
            XmlNode root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null && item.Name == notificationName)
                {
                    XmlNode node = item.SelectSingleNode("attachments");
                    foreach (XmlNode att in node.ChildNodes)
                    {
                        if (att != null && att.Name == reportName)
                        {
                            return att.Attributes["attName"].Value;
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取消息附件类型
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        /// <param name="reportName">报表类名称</param>
        /// <returns></returns>
        public static ExportFormatType GetAttachmentFileType(string notificationName, string reportName)
        {
            string type = "";
            XmlDocument xmldoc = GetXmlDocument("NotificationConfig.xml");
            XmlNode root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null && item.Name == notificationName)
                {
                    XmlNode node = item.SelectSingleNode("attachments");
                    foreach (XmlNode att in node.ChildNodes)
                    {
                        if (att != null && att.Name == reportName)
                        {
                            type = att.Attributes["attType"].Value;
                            break;
                        }
                    }
                    break;
                }
            }
            switch (type)
            {
                case "xls":
                    return ExportFormatType.Excel;
                case "doc":
                    return ExportFormatType.WordForWindows;
                case "pdf":
                    return ExportFormatType.PortableDocFormat;
                case "htm":
                    return ExportFormatType.HTML32;
                case "html":
                    return ExportFormatType.HTML40;
                case "rpt":
                    return ExportFormatType.CrystalReport;
                case "txt":
                    return ExportFormatType.TabSeperatedText;
                default:
                    return ExportFormatType.PortableDocFormat;
            }
        }

        /// <summary>
        /// 获取指定名称的数据库连结字符串
        /// </summary>
        /// <param name="connName">连接名称</param>
        /// <returns></returns>
        public static string GetDBConnectionString(string connName)
        {
            XmlDocument xmldoc = GetXmlDocument("Database.xml");
            XmlNode root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == connName)
                    {
                        try
                        {
                            return item.InnerText;
                        }
                        catch (Exception)
                        {
                            return "";
                        }

                    }
                }

            }
            return "";
        }

        /// <summary>
        /// 获取邮件发送帐户
        /// </summary>
        /// <returns></returns>
        public static string GetMailAccount()
        {
            return GetXmlNode("ServiceConfig.xml", "account").InnerText;
        }

        /// <summary>
        /// 获取邮件发送帐户域
        /// </summary>
        /// <returns></returns>
        public static string GetMailAccountDomain()
        {
            return GetXmlNode("ServiceConfig.xml", "domain").InnerText;
        }

        /// <summary>
        /// 获取邮件发送帐户密码
        /// </summary>
        /// <returns></returns>
        public static string GetMailAccountPassword()
        {
            return GetXmlNode("ServiceConfig.xml", "pwd").InnerText;
        }

        /// <summary>
        /// 获取邮件发送帐户密码
        /// </summary>
        /// <returns></returns>
        public static string GetMailFrom()
        {
            return GetXmlNode("ServiceConfig.xml","from").InnerText;
        }

        /// <summary>
        /// 获取邮件服务器地址
        /// </summary>
        /// <returns></returns>
        public static string GetMailServerAddress()
        {
            return GetXmlNode("ServiceConfig.xml", "smtp").InnerText;
        }

        /// <summary>
        /// 获取邮件服务器端口
        /// </summary>
        /// <returns></returns>
        public static string GetMailServerPort()
        {
            return GetXmlNode("ServiceConfig.xml", "port").InnerText;
        }

        /// <summary>
        /// 获取Web服务器地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebServerAddress()
        {
            XmlDocument xmldoc = GetXmlDocument("ServiceConfig.xml");
            XmlNode root = xmldoc.DocumentElement;
            return root.SelectSingleNode("webserver").InnerText;
        }

        /// <summary>
        /// 获取需要发送的消息列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetNotification()
        {
            string[] notification = new string[] { };
            string status;
            DateTime nexttime;
            DateTime date = DateTime.Now;
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load(GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    XmlNode node = item.SelectSingleNode("notify");
                    {

                        nexttime = DateTime.Parse(node.SelectSingleNode("nexttime").InnerText);
                        status = node.SelectSingleNode("status").InnerText;
                        if (nexttime.Date == date.Date && nexttime.Hour == date.Hour && nexttime.Minute == date.Minute && status == "V")
                        {
                            Array.Resize(ref notification, notification.Length + 1);
                            notification.SetValue(item.Name, notification.Length - 1);
                        }

                    }
                }
            }
            return notification;
        }

        /// <summary>
        /// 获取报表程序集名称
        /// </summary>
        /// <returns></returns>
        public static string GetObjectFile()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(GetServiceInstallPath() + "\\ServiceConfig.xml");
            XmlNode root = xmldoc.DocumentElement;
            return root.SelectSingleNode("objectfile").InnerText;
        }

        /// <summary>
        /// 获取报表程序集名称
        /// </summary>
        /// <returns></returns>
        public static string GetObjectFile(string notificationName)
        {
            string objectfile = "";
            XmlDocument xmldoc = GetXmlDocument(notificationConfigFileName);
            XmlNode root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null && item.Name == notificationName)
                {
                    if (item.Attributes["fileName"] != null)
                    {
                        objectfile = item.Attributes["fileName"].Value;
                    }
                    break;
                }
            }
            if (objectfile == "") objectfile = GetObjectFile();
            return objectfile;
        }

        /// <summary>
        /// 获取报表运行需要的参数及默认值
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static Hashtable GetParameter(string configName)
        {
            Hashtable param = new Hashtable();
            XmlDocument xmldoc = GetXmlDocument(notificationConfigFileName);
            XmlNode root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {

                XmlNode node = item.SelectSingleNode("param");
                if (node.Attributes["name"].Value == configName)
                {
                    foreach (XmlNode p in node.ChildNodes)
                    {
                        param.Add(p.Name, p.InnerText);
                    }
                }

            }
            return param;
        }

        /// <summary>
        /// 获取报表运行需要的参数及默认值
        /// </summary>
        /// <param name="notificationName">消息类</param>
        /// <param name="configName">配置类</param>
        /// <returns></returns>
        public static Hashtable GetParameter(string notificationName,string configName)
        {
            Hashtable param = new Hashtable();
            XmlNode root = GetXmlNode(notificationConfigFileName, notificationName);
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item.Name=="param" && item.Attributes["name"].Value == configName)
                {
                    foreach (XmlNode p in item.ChildNodes)
                    {
                        param.Add(p.Name, p.InnerText);
                    }
                }

            }
            return param;
        }

        /// <summary>
        /// 获取报表发送服务安装路径
        /// </summary>
        /// <returns></returns>
        public static string GetServiceInstallPath()
        {
            //return @"G:\myWorkSpace\SHB\AutoReport\Service\Test\bin\Debug";
            string key = @"SYSTEM\CurrentControlSet\Services\" + "HanbellAutoReport";
            string path = Registry.LocalMachine.OpenSubKey(key).GetValue("ImagePath").ToString();
            path = path.Replace("\"", string.Empty);
            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }

        /// <summary>
        /// 将字符串按指定的分割符分割成字符串数组
        /// </summary>
        /// <param name="value">需要分割的字符串</param>
        /// <param name="separator">分割符</param>
        /// <returns></returns>
        public static string[] GetSeparatedContent(string value, string separator)
        {
            string[] separators = new string[] { separator };
            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 枚举一周中的某天
        /// </summary>
        /// <param name="value">0～6字符表示的星期</param>
        /// <returns>System.DayOfWeek枚举值</returns>
        public static DayOfWeek GetWeek(string value)
        {
            switch (value)
            {
                case "0":
                    return DayOfWeek.Sunday;
                case "1":
                    return DayOfWeek.Monday;
                case "2":
                    return DayOfWeek.Tuesday;
                case "3":
                    return DayOfWeek.Wednesday;
                case "4":
                    return DayOfWeek.Thursday;
                case "5":
                    return DayOfWeek.Friday;
                case "6":
                    return DayOfWeek.Saturday;
                default:
                    return DayOfWeek.Monday;
            }
        }

        /// <summary>
        ///  获取服务运行路径下的Xml文件并载入
        /// </summary>
        /// <param name="xmlFileName">文件名</param>
        /// <returns></returns>
        public static XmlDocument GetXmlDocument(string xmlFileName)
        {
            return GetXmlDocument(Base.GetServiceInstallPath(), xmlFileName);
        }

        /// <summary>
        /// 获取指定路径下的Xml文件并载入
        /// </summary>
        /// <param name="runPath">路径</param>
        /// <param name="xmlFileName">文件名</param>
        /// <returns></returns>
        public static XmlDocument GetXmlDocument(string runPath, string xmlFileName)
        {
            string appPath = runPath + "\\" + xmlFileName;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(appPath);
            return xmldoc;
        }

        /// <summary>
        /// 获取Xml文件中第一个匹配的节点
        /// </summary>
        /// <param name="xmlFileName">文件名</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static XmlNode GetXmlNode(string xmlFileName, string nodeName)
        {
            XmlDocument xmldoc = GetXmlDocument(xmlFileName);
            XmlNode root = xmldoc.DocumentElement;
            XmlNode node = GetXmlNode(root, nodeName);
            return node;
        }

        /// <summary>
        /// 获取Xml文件某个节点下的第一个匹配的节点，遍历子节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="nodename">查找的节点名称</param>
        /// <returns></returns>
        public static XmlNode GetXmlNode(XmlNode parent, string nodeName)
        {
            XmlNode node;
            foreach (XmlNode item in parent.ChildNodes)
            {
                if ((item != null) && (item.Name == nodeName))
                {
                    return item;
                }
                node = GetXmlNode(item, nodeName);
                if (node != null) return node;
            }
            return null;
        }

        /// <summary>
        /// 设置下一次通知时间
        /// </summary>
        public static void ResetNotify()
        {
            bool ret = false;
            string period, week, day, lasttime, nexttime, status;
            TimeSpan time;
            DateTime date;
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load(GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    date = DateTime.Now;
                    bool flag;
                    XmlNode node = item.SelectSingleNode("notify");

                    period = node.SelectSingleNode("period").InnerText;
                    week = node.SelectSingleNode("week").InnerText;
                    day = node.SelectSingleNode("day").InnerText;
                    time = TimeSpan.Parse(node.SelectSingleNode("time").InnerText);
                    lasttime = node.SelectSingleNode("lasttime").InnerText;
                    nexttime = node.SelectSingleNode("nexttime").InnerText;
                    status = node.SelectSingleNode("status").InnerText;
                    switch (period)
                    {
                        //每天发送
                        case "D":
                            if ((nexttime == "") || (date.Date > DateTime.Parse(nexttime).Date && status == "V"))
                            {
                                node.SelectSingleNode("nexttime").InnerText = date.ToString("yyyy/MM/dd") + " " + time.ToString();
                                node.SelectSingleNode("status").InnerText = "V";
                                ret = true;
                            }
                            else if (date.Date == DateTime.Parse(nexttime).Date && date.TimeOfDay > time && status == "X")
                            {
                                date = date.Date.AddDays(1);
                                node.SelectSingleNode("nexttime").InnerText = date.ToString("yyyy/MM/dd") + " " + time.ToString();
                                node.SelectSingleNode("status").InnerText = "V";
                                ret = true;
                            }
                            break;
                        //每周指定星期几发送
                        case "W":
                            string[] weekSeparators = new string[] { "," };
                            string[] weeks = week.Split(weekSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (weeks == null || weeks.Length == 0) break;
                            int n = 0;
                            flag = true;
                            foreach (string w in weeks)
                            {
                                if (date.DayOfWeek < GetWeek(w))
                                {
                                    n = GetWeek(w) - date.DayOfWeek;
                                    flag = false;
                                    break;
                                }
                                else if (date.DayOfWeek == GetWeek(w) && date.Hour <= time.Hours && date.Minute <= time.Minutes)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                n = GetWeek(weeks[0]) - date.DayOfWeek + 7;
                            }
                            if ((nexttime == "") || (date.Date > DateTime.Parse(nexttime).Date && (status == "V" || status == "X")))
                            {
                                node.SelectSingleNode("nexttime").InnerText = date.AddDays(n).ToString("yyyy/MM/dd") + " " + time.ToString();
                                node.SelectSingleNode("status").InnerText = "V";
                                ret = true;
                            }
                            break;
                        //每月指定某几天发送,L表示最后一天
                        case "M":
                            string[] daySeparators = new string[] { "," };
                            string[] days = day.Split(daySeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (days == null || days.Length == 0) break;
                            int m = 0;
                            flag = true;
                            foreach (string d in days)
                            {
                                if (d != "L")
                                {
                                    m = Int16.Parse(d);
                                }
                                else
                                {
                                    m = date.AddMonths(1).AddDays(0 - date.Day).Day;
                                }
                                if (date.Day < m)
                                {
                                    date = date.AddDays(m - date.Day);
                                    flag = false;
                                    break;
                                }
                                else if (date.Day == m && date.Hour <= time.Hours && date.Minute < time.Minutes)
                                {
                                    date = date.AddDays(m - date.Day);
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                //m = Int16.Parse(days[0]);
                                date = date.AddDays(m - date.Day).AddMonths(1);
                            }
                            if ((nexttime == "") || ((DateTime.Now.Date > DateTime.Parse(nexttime).Date)))
                            {
                                node.SelectSingleNode("nexttime").InnerText = date.ToString("yyyy/MM/dd") + " " + time.ToString();
                                node.SelectSingleNode("status").InnerText = "V";
                                ret = true;
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            if (ret)
            {
                xmldoc.Save(GetServiceInstallPath() + "\\NotificationConfig.xml");
            }
        }

        /// <summary>
        /// 设置消息状态为已过期
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        public static void SetNotificationOver(string notificationName)
        {
            bool ret = false;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null && item.Name == notificationName)
                {
                    XmlNode node = item.SelectSingleNode("notify");
                    node.SelectSingleNode("status").InnerText = "X";
                    ret = true;
                }
            }
            if (ret)
            {
                xmldoc.Save(GetServiceInstallPath() + "\\NotificationConfig.xml");
            }
        }

        /// <summary>
        /// 设置消息上一次成功执行的时间
        /// </summary>
        /// <param name="notificationName">消息名称</param>
        public static void SetNotificationLastTime(string notificationName)
        {
            bool ret = false;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null && item.Name == notificationName)
                {
                    XmlNode node = item.SelectSingleNode("notify");
                    node.SelectSingleNode("lasttime").InnerText = DateTime.Now.ToString();
                    ret = true;
                }
            }
            if (ret)
            {
                xmldoc.Save(GetServiceInstallPath() + "\\NotificationConfig.xml");
            }
        }

    }

    public enum DBServerType
    {
        MSSQL, OLEDB, SybaseASE
    }
}
