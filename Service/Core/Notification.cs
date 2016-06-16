using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;

namespace Hanbell.AutoReport.Core
{
    public abstract class Notification : IDisposable
    {
        protected List<Notify> notifyList;
        public Hashtable to;
        public Hashtable cc;
        public Hashtable bcc;
        public Hashtable att;
        public String subject { get; set; }
        public String content { get; set; }

        public Notification()
        {
            notifyList = new List<Notify>();
            to = new Hashtable();
            cc = new Hashtable();
            bcc = new Hashtable();
            att = new Hashtable();
            Init();
        }

        protected virtual void Init()
        {
            SetMailHead();
        }

        protected virtual void SendAddtionalNotification()
        {
        }

        protected void SetMailHead()
        {
            this.subject = GetMailSubject(this.ToString());
            foreach (string item in GetMailTo(this.ToString()))
            {
                if (item != "") AddTo(item);
            }
            foreach (string item in GetMailCc(this.ToString()))
            {
                if (item != "") AddCc(item);
            }
            foreach (string item in GetMailBcc(this.ToString()))
            {
                if (item != "") AddBcc(item);
            }
        }

        protected string GetMailSubject(string name)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name)
                    {
                        try
                        {
                            XmlNode xe = item.SelectSingleNode("subject");
                            return xe.InnerText;
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

        protected string GetMailHeadAdd(string name)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name)
                    {
                        try
                        {
                            XmlNode node = item.SelectSingleNode("headAdd");
                            return node.InnerXml;
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

        protected string GetMailFooterAdd(string name)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name)
                    {
                        try
                        {
                            XmlNode xe = item.SelectSingleNode("footerAdd");
                            return xe.InnerXml;
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

        protected string[] GetMailTo(string name)
        {
            string[] to = new string[] { };
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name && item.HasChildNodes)
                    {
                        XmlNode mailto = item.SelectSingleNode("to");

                        if (!mailto.HasChildNodes) break;

                        foreach (XmlNode node in mailto.ChildNodes)
                        {
                            Array.Resize(ref to, to.Length + 1);
                            to.SetValue(node.InnerText, to.Length - 1);
                        }
                        return to;
                    }
                }
            }
            return to;
        }

        protected string[] GetMailCc(string name)
        {
            string[] to = new string[] { };
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name && item.HasChildNodes)
                    {
                        XmlNode mailto = item.SelectSingleNode("cc");

                        if (!mailto.HasChildNodes) break;

                        foreach (XmlNode node in mailto.ChildNodes)
                        {
                            Array.Resize(ref to, to.Length + 1);
                            to.SetValue(node.InnerText, to.Length - 1);
                        }
                        return to;
                    }
                }
            }
            return to;
        }

        protected string[] GetMailBcc(string name)
        {
            string[] to = new string[] { };
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Base.GetServiceInstallPath() + "\\NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    if (item.Name == name && item.HasChildNodes)
                    {
                        XmlNode mailto = item.SelectSingleNode("bcc");

                        if (!mailto.HasChildNodes) break;

                        foreach (XmlNode node in mailto.ChildNodes)
                        {
                            Array.Resize(ref to, to.Length + 1);
                            to.SetValue(node.InnerText, to.Length - 1);
                        }
                        return to;
                    }
                }
            }
            return to;
        }

        public void AddNotify(Notify n)
        {
            this.notifyList.Add(n);
        }

        public void AddTo(string receiver)
        {
            this.to.Add(this.to.Count, receiver);
        }

        public void AddCc(string receiver)
        {
            this.cc.Add(this.cc.Count, receiver);
        }

        public void AddBcc(string receiver)
        {
            this.bcc.Add(this.bcc.Count, receiver);
        }

        public void AddAtt(string file)
        {
            this.att.Add(this.att.Count, file);
        }

        public void ClearTo()
        {
            this.to.Clear();
        }

        public void ClearCc()
        {
            this.cc.Clear();
        }

        public void ClearBcc()
        {
            this.bcc.Clear();
        }

        public void ClearAtt()
        {
            this.att.Clear();
        }

        public void RemoveNotify(Notify n)
        {
            this.notifyList.Remove(n);
        }

        public void Update()
        {
            try
            {
                foreach (Notify item in notifyList)
                {
                    item.SendInfo(this);
                }
                SendAddtionalNotification();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public virtual void Dispose()
        {
            if (notifyList != null)
            {
                notifyList.Clear();
                notifyList = null;
            }
            if (to != null) ClearTo();
            if (cc != null) ClearCc();
            if (bcc != null) ClearBcc();
            if (att != null) ClearAtt();
            if (subject != null) subject = null;
            if (content != null) content = null;
        }

    }
}
