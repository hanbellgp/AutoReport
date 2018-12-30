using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Xml;
using System.Collections;
using System.Xml.Linq;
using Hanbell.AutoReport.Core;

namespace DrizzlingTest
{
    public partial class SetNotify : Office2007Form
    {
        DevComponents.AdvTree.Node selectedNote;
        ArrayList period = new ArrayList();
        Hashtable parameters = new Hashtable();
        Attachment[] attachments = new Attachment[] { };

        public SetNotify()
        {
            InitializeComponent();
            period.Add(new PeriodType("H", "小时间隔"));
            period.Add(new PeriodType("D", "每天"));
            period.Add(new PeriodType("W", "每周"));
            period.Add(new PeriodType("M", "每月"));
            period.Add(new PeriodType("Q", "每季"));
          //  period.Add(new PeriodType("TD", "每两天"));
        }

        private void SetNotify_Load(object sender, EventArgs e)
        {
            this.cmbPeriod.DataSource = period;
            this.cmbPeriod.ValueMember = "Type";
            this.cmbPeriod.DisplayMember = "Name";

            FillTree();

            ControlUI(true);
        }

        #region UIEvents

        private void btnAdd_Click(object sender, EventArgs e)
        {
            selectedNote = null;
            FillUI();
            ControlToolButton(false, false, false, true, true, true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedNote != null) ControlUI(false);
            ControlToolButton(false, false, false, true, true, true);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (selectedNote == null)
            {
                ShowMessage("没有需要删除的资料");
                return;
            }
            XmlDocument xmldoc = Base.GetXmlDocument("NotificationConfig.xml");
            XmlNode root = xmldoc.DocumentElement;
            XmlNode node = Base.GetXmlNode(root, selectedNote.Name);
            if (node != null)
            {
                root.RemoveChild(node);
                xmldoc.Save("NotificationConfig.xml");
                FillTree();
                ControlToolButton(true, true, true, false, false, false);
            }
        }

        private void btnAtt_Click(object sender, EventArgs e)
        {
            SetAttachments setAtt = new SetAttachments();
            setAtt.StartPosition = FormStartPosition.CenterScreen;
            setAtt.ShowInTaskbar = false;
            if (this.attachments != null && this.attachments.Length > 0)
            {
                setAtt.attachments = this.attachments;
            }
            setAtt.ShowDialog();
            this.attachments = setAtt.attachments;
        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            SetParameters setParam = new SetParameters();
            setParam.StartPosition = FormStartPosition.CenterScreen;
            setParam.ShowInTaskbar = false;
            if (this.parameters != null && this.parameters.Count > 0)
            {
                setParam.parameters = this.parameters;
            }
            setParam.ShowDialog();
            this.parameters = setParam.parameters;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ControlUI(true);
            ControlToolButton(true, true, true, false, false, false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool ret = true;
            if ((txtFullName.Text.Length == 0) || txtFullName.Text.Equals(""))
            {
                ret = ret && false;
                ShowMessage("类完整名称不能空白");
            }
            if ((txtFileName.Text.Length == 0) || txtFileName.Text.Equals(""))
            {
                ret = ret && false;
                ShowMessage("类库名称不能空白");
            }
            if ((txtSubject.Text.Length == 0) || txtSubject.Text.Equals(""))
            {
                ret = ret && false;
                ShowMessage("主旨不能空白");
            }
            if (cmbPeriod.SelectedIndex == -1)
            {
                ret = ret && false;
                ShowMessage("推送周期不能空白");
            }
            else
            {
                switch (cmbPeriod.SelectedValue.ToString())
                {
                    case "W":
                        if (GetWeeks() == "")
                        {
                            ret = ret && false;
                            ShowMessage("请指定需要发送的星期");
                        }
                        break;
                    case "M":
                        if (GetDays() == "")
                        {
                            ret = ret && false;
                            ShowMessage("请指定需要发送的日期,（L）代表最后一天");
                        }
                        break;
                    default:
                        break;
                }
            }
            if ((txtTime.Text.Length == 0) || txtTime.Text.Equals(""))
            {
                ret = ret && false;
                ShowMessage("推送时间不能空白");
            }
            else
            {
                try
                {
                    TimeSpan.Parse(txtTime.Text);
                }
                catch (Exception)
                {
                    ret = ret && false;
                    ShowMessage("推送时间格式不正确");
                }
            }
            if (txtNexttime.Text.Length != 0)
            {
                try
                {
                    DateTime.Parse(txtNexttime.Text);
                }
                catch (Exception)
                {
                    ret = ret && false;
                    ShowMessage("下次发送时间格式不正确");
                }
            }
            if (ret)
            {
                SaveXmlDocument();
                FillTree();
                ControlToolButton(true, true, true, false, false, false);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPeriod.SelectedIndex != -1)
            {
                if (cmbPeriod.SelectedValue != null)
                {
                    ControlPeriod(cmbPeriod.SelectedValue.ToString());
                }
            }
        }

        private void AdvTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Node.Name != "Root")
            {
                selectedNote = e.Node;
                FillUI();
                ControlUI(true);
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            if (selectedNote != null)
            {
                try
                {
                    Notification entity = Base.CreateNotification(selectedNote.Name);
                    entity.Update();
                    Base.SetNotificationLastTime(selectedNote.Name);
                    Base.SetNotificationOver(selectedNote.Name);
                    entity.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        #endregion

        #region UIControl

        private void ControlToolButton(bool add, bool edit, bool del, bool att, bool param, bool save)
        {
            btnAdd.Enabled = add;
            btnEdit.Enabled = edit;
            btnDel.Enabled = del;
            btnAtt.Enabled = att;
            btnPara.Enabled = param;
            btnSave.Enabled = save;
            btnExec.Enabled = !btnSave.Enabled;
        }

        private void ControlUI(bool flag)
        {
            switch (flag)
            {
                case true:
                    txtFullName.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtFullName.ReadOnly = true;
                    txtFileName.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtFileName.ReadOnly = true;
                    txtReportName.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtReportName.ReadOnly = true;
                    txtSubject.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtSubject.ReadOnly = true;
                    txtTo.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtTo.ReadOnly = true;
                    txtCc.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtCc.ReadOnly = true;
                    txtBcc.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtBcc.ReadOnly = true;
                    txtHeadAdd.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtHeadAdd.ReadOnly = true;
                    txtFooterAdd.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtFooterAdd.ReadOnly = true;
                    txtParam.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtParam.ReadOnly = true;
                    cmbPeriod.Enabled = false;
                    txtTime.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtTime.ReadOnly = true;
                    txtDay.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtDay.ReadOnly = true;
                    txtNexttime.BackColor = Color.FromKnownColor(KnownColor.Info);
                    txtNexttime.ReadOnly = true;
                    chkSun.Enabled = false;
                    chkMon.Enabled = false;
                    chkTue.Enabled = false;
                    chkWed.Enabled = false;
                    chkThu.Enabled = false;
                    chkFri.Enabled = false;
                    chkSat.Enabled = false;
                    break;
                default:
                    txtFullName.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtFullName.ReadOnly = false;
                    txtFileName.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtFileName.ReadOnly = false;
                    txtReportName.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtReportName.ReadOnly = false;
                    txtSubject.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtSubject.ReadOnly = false;
                    txtTo.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtTo.ReadOnly = false;
                    txtCc.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtCc.ReadOnly = false;
                    txtBcc.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtBcc.ReadOnly = false;
                    txtHeadAdd.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtHeadAdd.ReadOnly = false;
                    txtFooterAdd.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtFooterAdd.ReadOnly = false;
                    txtParam.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtParam.ReadOnly = false;
                    cmbPeriod.Enabled = true;
                    txtTime.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtTime.ReadOnly = false;
                    txtDay.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtDay.ReadOnly = false;
                    txtNexttime.BackColor = Color.FromKnownColor(KnownColor.Window);
                    txtNexttime.ReadOnly = false;
                    chkSun.Enabled = true;
                    chkMon.Enabled = true;
                    chkTue.Enabled = true;
                    chkWed.Enabled = true;
                    chkThu.Enabled = true;
                    chkFri.Enabled = true;
                    chkSat.Enabled = true;
                    break;
            }

            ControlPeriod(cmbPeriod.SelectedValue.ToString());

        }

        private void ControlPeriod(string period)
        {
            switch (period)
            {
                case "W":
                    lblDay.Visible = true;
                    txtDay.Visible = false;
                    chkSun.Visible = true;
                    chkMon.Visible = true;
                    chkTue.Visible = true;
                    chkWed.Visible = true;
                    chkThu.Visible = true;
                    chkFri.Visible = true;
                    chkSat.Visible = true;
                    break;
                case "M":
                    lblDay.Visible = true;
                    txtDay.Visible = true;
                    chkSun.Visible = false;
                    chkMon.Visible = false;
                    chkTue.Visible = false;
                    chkWed.Visible = false;
                    chkThu.Visible = false;
                    chkFri.Visible = false;
                    chkSat.Visible = false;
                    break;
                default:
                    lblDay.Visible = false;
                    txtDay.Visible = false;
                    chkSun.Visible = false;
                    chkMon.Visible = false;
                    chkTue.Visible = false;
                    chkWed.Visible = false;
                    chkThu.Visible = false;
                    chkFri.Visible = false;
                    chkSat.Visible = false;
                    break;
            }
        }

        private void FillTree()
        {
            this.root.Nodes.Clear();
            XmlDocument xmldoc = Base.GetXmlDocument("NotificationConfig.xml");
            XmlNode root;
            root = xmldoc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item != null)
                {
                    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                    node.Name = item.Name;
                    node.Text = item.Name;
                    if (item.Attributes["reportName"] != null)
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(item.Attributes["reportName"].Value));
                    }
                    else
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(""));
                    }
                    this.root.Nodes.Add(node);
                    selectedNote = node;
                }
            }
            FillUI();
        }

        private void FillUI()
        {
            if (selectedNote != null)
            {
                txtFullName.Text = selectedNote.Name;
                XmlNode root, node;
                root = Base.GetXmlNode("NotificationConfig.xml", selectedNote.Name);
                if (root.Attributes["fileName"] != null) txtFileName.Text = root.Attributes["fileName"].Value.ToString();
                if (root.Attributes["reportName"] != null) txtReportName.Text = root.Attributes["reportName"].Value.ToString();

                txtTo.Text = this.GetContent(GetChildXmlNodesInnerText(root, "to"), ";");
                txtCc.Text = this.GetContent(GetChildXmlNodesInnerText(root, "cc"), ";");
                txtBcc.Text = this.GetContent(GetChildXmlNodesInnerText(root, "bcc"), ";");

                node = Base.GetXmlNode(root, "subject");
                if (node != null)
                {
                    txtSubject.Text = node.InnerText;
                }
                else
                {
                    txtSubject.Text = "";
                }
                node = Base.GetXmlNode(root, "headAdd");
                if (node != null)
                {
                    txtHeadAdd.Text = node.InnerXml;
                }
                else
                {
                    txtHeadAdd.Text = "";
                }
                node = Base.GetXmlNode(root, "footerAdd");
                if (node != null)
                {
                    txtFooterAdd.Text = node.InnerXml;
                }
                else
                {
                    txtFooterAdd.Text = "";
                }
                node = Base.GetXmlNode(root, "param");
                if (node != null)
                {
                    txtParam.Text = node.Attributes["name"].Value.ToString();
                }
                else
                {
                    txtFooterAdd.Text = "";
                }
                node = Base.GetXmlNode(root, "period");
                if (node != null)
                {
                    cmbPeriod.SelectedValue = node.InnerText;
                }
                node = Base.GetXmlNode(root, "week");
                if (node != null)
                {
                    SetWeeks(GetSeparatedContent(node.InnerText, ","));
                }
                node = Base.GetXmlNode(root, "day");
                if (node != null)
                {
                    txtDay.Text = GetContent(GetSeparatedContent(node.InnerText, ","), ";");
                }
                node = Base.GetXmlNode(root, "time");
                if (node != null)
                {
                    txtTime.Text = node.InnerText;
                }
                node = Base.GetXmlNode(root, "lasttime");
                if (node != null)
                {
                    txtLasttime.Text = node.InnerText;
                }
                node = Base.GetXmlNode(root, "nexttime");
                if (node != null)
                {
                    txtNexttime.Text = node.InnerText;
                }
                else
                {
                    txtTime.Text = "";
                }
                this.parameters.Clear();
                foreach (XmlNode item in GetChildXmlNodes(root, "param"))
                {
                    this.parameters.Add(item.Name, item.InnerText);
                }
                Attachment[] atts = new Attachment[] { };
                foreach (XmlNode item in GetChildXmlNodes(root, "attachments"))
                {
                    Array.Resize(ref atts, atts.Length + 1);
                    atts.SetValue(new Attachment(item.Name, item.Attributes["attName"].Value, item.Attributes["attType"].Value), atts.Length - 1);
                }
                this.attachments = atts;
            }
            else
            {
                txtFullName.Text = "";
                txtFileName.Text = "";
                txtReportName.Text = "";
                txtTo.Text = "";
                txtCc.Text = "";
                txtBcc.Text = "";
                txtSubject.Text = "";
                txtHeadAdd.Text = "";
                txtFooterAdd.Text = "";
                txtParam.Text = "";
                txtDay.Text = "";
                txtTime.Text = "";
                ControlPeriod("");
                ControlUI(false);
            }
        }

        #endregion

        /// <summary>
        /// 向字符串数组添加值
        /// </summary>
        /// <param name="values">符串数组</param>
        /// <param name="value">值</param>
        private void AddStringArrayValue(ref string[] values, object value)
        {
            if (values == null)
            {
                throw new NullReferenceException();
            }
            Array.Resize(ref values, values.Length + 1);
            values.SetValue(value, values.Length - 1);
        }

        /// <summary>
        /// 创建新的排程节点
        /// </summary>
        /// <returns></returns>
        private XElement CreateXmlNode()
        {
            string nexttime, status;
            if (txtNexttime.Text.Length != 0)
            {
                nexttime = txtNexttime.Text;
                status = "V";
            }
            else
            {
                nexttime = "";
                status = "X";
            }
            XElement notification =
            new XElement(txtFullName.Text, new XAttribute("fileName", txtFileName.Text), new XAttribute("reportName", txtReportName.Text),
                new XElement("subject", txtSubject.Text),
                new XElement("to", GetXElementObjects(txtTo.Text, ";", "address")),
                new XElement("cc", GetXElementObjects(txtCc.Text, ";", "address")),
                new XElement("bcc", GetXElementObjects(txtBcc.Text, ";", "address")),
                new XElement("attachments", GetXElementObjects(this.attachments)),
                new XElement("headAdd", txtHeadAdd.Text),
                new XElement("footerAdd", txtFooterAdd.Text),
                new XElement("notify",
                       new XElement("period", cmbPeriod.SelectedValue),
                       new XElement("week", GetWeeks()),
                       new XElement("day", GetDays()),
                       new XElement("time", txtTime.Text),
                       new XElement("lasttime"),
                       new XElement("nexttime", nexttime),
                       new XElement("status", status)
                   ),
                new XElement("param", new XAttribute("name", txtParam.Text), GetXElementObjects(this.parameters))
            );
            return notification;
        }

        /// <summary>
        /// 获取某个节点下的所有下一级子节点
        /// </summary>
        /// <param name="parent">开始节点</param>
        /// <param name="nodeName">查找的节点名称</param>
        /// <returns></returns>
        private XmlNode[] GetChildXmlNodes(XmlNode parent, string nodeName)
        {
            XmlNode node;
            XmlNode[] nodes = new XmlNode[] { };
            foreach (XmlNode item in parent.ChildNodes)
            {
                if ((item != null) && (item.Name == nodeName))
                {
                    node = item;
                }
                else
                {
                    node = Base.GetXmlNode(item, nodeName);
                }
                if (node != null)
                {
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        Array.Resize(ref nodes, nodes.Length + 1);
                        nodes.SetValue(n, nodes.Length - 1);
                    }
                    break;
                }
            }
            return nodes;
        }

        /// <summary>
        /// 获取某个节点下的所有子节点的内容(InnerText)
        /// </summary>
        /// <param name="parent">开始节点</param>
        /// <param name="nodeName">查找的节点名称</param>
        /// <returns></returns>
        private string[] GetChildXmlNodesInnerText(XmlNode parent, string nodeName)
        {
            XmlNode node;
            string[] content = new string[] { };
            foreach (XmlNode item in parent.ChildNodes)
            {
                if ((item != null) && (item.Name == nodeName))
                {
                    node = item;
                }
                else
                {
                    node = Base.GetXmlNode(item, nodeName);
                }
                if (node != null)
                {
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        AddStringArrayValue(ref content, n.InnerText);
                    }
                    break;
                }
            }
            return content;
        }

        /// <summary>
        /// 将字符串数组用指定的分隔符组成一个字符串
        /// </summary>
        /// <param name="args">字符串数组</param>
        /// <param name="separetor">分隔符</param>
        /// <returns></returns>
        private string GetContent(string[] args, string separetor)
        {
            if ((args == null) || args.Length == 0) return "";
            string result = "";
            foreach (string item in args)
            {
                result += item + separetor;
            }
            if (result.Substring(result.Length - separetor.Length) == separetor)
            {
                result = result.Remove(result.Length - separetor.Length);
            }
            return result;
        }

        private string GetDays()
        {
            if (txtDay.Text.Length == 0) return null;
            return GetContent(GetSeparatedContent(txtDay.Text, ";"), ",");
        }

        private string[] GetSeparatedContent(string value, string separator)
        {
            string[] separators = new string[] { separator };
            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetWeeks()
        {
            string[] weeks = new string[] { };
            if (chkSun.Checked) AddStringArrayValue(ref weeks, "0");
            if (chkMon.Checked) AddStringArrayValue(ref weeks, "1");
            if (chkTue.Checked) AddStringArrayValue(ref weeks, "2");
            if (chkWed.Checked) AddStringArrayValue(ref weeks, "3");
            if (chkThu.Checked) AddStringArrayValue(ref weeks, "4");
            if (chkFri.Checked) AddStringArrayValue(ref weeks, "5");
            if (chkSat.Checked) AddStringArrayValue(ref weeks, "6");
            return GetContent(weeks, ",");
        }

        private void SetWeeks(string[] weeks)
        {
            chkSun.Checked = false;
            chkMon.Checked = false;
            chkTue.Checked = false;
            chkWed.Checked = false;
            chkThu.Checked = false;
            chkFri.Checked = false;
            chkSat.Checked = false;
            if (weeks.Length == 0) return;
            foreach (string item in weeks)
            {
                switch (item)
                {
                    case "0":
                        chkSun.Checked = true;
                        break;
                    case "1":
                        chkMon.Checked = true;
                        break;
                    case "2":
                        chkTue.Checked = true;
                        break;
                    case "3":
                        chkWed.Checked = true;
                        break;
                    case "4":
                        chkThu.Checked = true;
                        break;
                    case "5":
                        chkFri.Checked = true;
                        break;
                    case "6":
                        chkSat.Checked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private object[] GetXElementObjects(string value, string separator, string elementName)
        {
            if ((value == null) || (value.Equals("")))
            {
                return null;
            }
            object[] objects = new object[] { };
            string[] results = GetSeparatedContent(value, separator);
            if ((results != null) && (results.Length > 0))
            {
                foreach (string item in results)
                {
                    Array.Resize(ref objects, objects.Length + 1);
                    objects.SetValue(new XElement(elementName, item), objects.Length - 1);
                }
            }
            return objects;
        }

        private object[] GetXElementObjects(string[] values, string elementName)
        {
            if ((values == null) || (values.Length == 0))
            {
                return null;
            }
            object[] objects = new object[] { };
            foreach (string item in values)
            {
                Array.Resize(ref objects, objects.Length + 1);
                objects.SetValue(new XElement(elementName, item), objects.Length - 1);
            }
            return objects;
        }

        private object[] GetXElementObjects(Hashtable values)
        {
            if ((values == null) || (values.Count == 0))
            {
                return null;
            }
            object[] objects = new object[] { };
            foreach (DictionaryEntry item in values)
            {
                Array.Resize(ref objects, objects.Length + 1);
                objects.SetValue(new XElement(item.Key.ToString(), item.Value.ToString()), objects.Length - 1);
            }
            return objects;
        }

        private object[] GetXElementObjects(Attachment[] attachments)
        {
            if ((attachments == null) || (attachments.Length == 0))
            {
                return null;
            }
            object[] objects = new object[] { };
            foreach (Attachment item in attachments)
            {
                Array.Resize(ref objects, objects.Length + 1);
                objects.SetValue(new XElement(item.reportClass, new XAttribute("attName", item.attName), new XAttribute("attType", item.attType)), objects.Length - 1);
            }
            return objects;
        }

        private void SaveXmlDocument()
        {
            string appPath = Base.GetServiceInstallPath() + "\\NotificationConfig.xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(appPath);
            XmlNode root = xmldoc.DocumentElement;
            XmlNode node = Base.GetXmlNode(root, txtFullName.Text);
            XmlNode newnode = xmldoc.ReadNode(CreateXmlNode().CreateReader());
            if (node == null)
            {
                root.AppendChild(newnode);
            }
            else
            {
                root.ReplaceChild(newnode, node);
            }
            xmldoc.Save(appPath);
            ControlUI(true);
        }

        private void ShowMessage(string msg)
        {
            MessageBoxEx.Show(msg, "系统消息", MessageBoxButtons.OK);
        }



    }

}
