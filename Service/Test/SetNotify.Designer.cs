namespace DrizzlingTest
{
    partial class SetNotify
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelExTree = new DevComponents.DotNetBar.PanelEx();
            this.GroupPanel = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.AdvTree = new DevComponents.AdvTree.AdvTree();
            this.ColId = new DevComponents.AdvTree.ColumnHeader();
            this.ColName = new DevComponents.AdvTree.ColumnHeader();
            this.root = new DevComponents.AdvTree.Node();
            this.ElementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.panelExBody = new DevComponents.DotNetBar.PanelEx();
            this.txtNexttime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblnext = new System.Windows.Forms.Label();
            this.lbllast = new System.Windows.Forms.Label();
            this.txtLasttime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblParam = new System.Windows.Forms.Label();
            this.txtParam = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblReportName = new System.Windows.Forms.Label();
            this.txtTime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.txtDay = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkSat = new System.Windows.Forms.CheckBox();
            this.chkFri = new System.Windows.Forms.CheckBox();
            this.chkThu = new System.Windows.Forms.CheckBox();
            this.chkWed = new System.Windows.Forms.CheckBox();
            this.chkTue = new System.Windows.Forms.CheckBox();
            this.chkMon = new System.Windows.Forms.CheckBox();
            this.cmbPeriod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblFooterAdd = new System.Windows.Forms.Label();
            this.lblHeadAdd = new System.Windows.Forms.Label();
            this.txtFooterAdd = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblBcc = new System.Windows.Forms.Label();
            this.txtBcc = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblCc = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.txtCc = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSubject = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelExBar = new DevComponents.DotNetBar.PanelEx();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnPara = new DevComponents.DotNetBar.ButtonX();
            this.btnAtt = new DevComponents.DotNetBar.ButtonX();
            this.btnQuit = new DevComponents.DotNetBar.ButtonX();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.btnEdit = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.txtHeadAdd = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkSun = new System.Windows.Forms.CheckBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtReportName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtFullName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnExec = new DevComponents.DotNetBar.ButtonX();
            this.PanelExTree.SuspendLayout();
            this.GroupPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdvTree)).BeginInit();
            this.panelExBody.SuspendLayout();
            this.panelExBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelExTree
            // 
            this.PanelExTree.CanvasColor = System.Drawing.SystemColors.Control;
            this.PanelExTree.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.PanelExTree.Controls.Add(this.GroupPanel);
            this.PanelExTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelExTree.Location = new System.Drawing.Point(0, 0);
            this.PanelExTree.Name = "PanelExTree";
            this.PanelExTree.Size = new System.Drawing.Size(391, 502);
            this.PanelExTree.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.PanelExTree.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.PanelExTree.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.PanelExTree.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.PanelExTree.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.PanelExTree.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.PanelExTree.Style.GradientAngle = 90;
            this.PanelExTree.TabIndex = 3;
            this.PanelExTree.Text = "PanelEx4";
            // 
            // GroupPanel
            // 
            this.GroupPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel.Controls.Add(this.AdvTree);
            this.GroupPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupPanel.Location = new System.Drawing.Point(0, 0);
            this.GroupPanel.Name = "GroupPanel";
            this.GroupPanel.Size = new System.Drawing.Size(391, 502);
            // 
            // 
            // 
            this.GroupPanel.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.GroupPanel.Style.BackColorGradientAngle = 90;
            this.GroupPanel.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.GroupPanel.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel.Style.BorderBottomWidth = 1;
            this.GroupPanel.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.GroupPanel.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel.Style.BorderLeftWidth = 1;
            this.GroupPanel.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel.Style.BorderRightWidth = 1;
            this.GroupPanel.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel.Style.BorderTopWidth = 1;
            this.GroupPanel.Style.Class = "";
            this.GroupPanel.Style.CornerDiameter = 5;
            this.GroupPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.GroupPanel.Style.MarginTop = 10;
            this.GroupPanel.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.GroupPanel.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.GroupPanel.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.GroupPanel.StyleMouseDown.Class = "";
            this.GroupPanel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.GroupPanel.StyleMouseOver.Class = "";
            this.GroupPanel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GroupPanel.TabIndex = 1;
            this.GroupPanel.Text = "报表列表";
            // 
            // AdvTree
            // 
            this.AdvTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.AdvTree.AllowDrop = true;
            this.AdvTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.AdvTree.BackgroundStyle.Class = "TreeBorderKey";
            this.AdvTree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.AdvTree.Columns.Add(this.ColId);
            this.AdvTree.Columns.Add(this.ColName);
            this.AdvTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdvTree.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle;
            this.AdvTree.ExpandWidth = 14;
            this.AdvTree.Location = new System.Drawing.Point(0, 0);
            this.AdvTree.Name = "AdvTree";
            this.AdvTree.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.root});
            this.AdvTree.NodeStyle = this.ElementStyle1;
            this.AdvTree.PathSeparator = ";";
            this.AdvTree.Size = new System.Drawing.Size(385, 468);
            this.AdvTree.Styles.Add(this.ElementStyle1);
            this.AdvTree.TabIndex = 0;
            this.AdvTree.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.AdvTree_NodeClick);
            // 
            // ColId
            // 
            this.ColId.Name = "ColId";
            this.ColId.Text = "编号";
            this.ColId.Width.Absolute = 400;
            // 
            // ColName
            // 
            this.ColName.Name = "ColName";
            this.ColName.Text = "名称";
            this.ColName.Width.Absolute = 180;
            // 
            // root
            // 
            this.root.Expanded = true;
            this.root.Name = "root";
            this.root.Text = "Root";
            // 
            // ElementStyle1
            // 
            this.ElementStyle1.Class = "";
            this.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ElementStyle1.Name = "ElementStyle1";
            this.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelExBody
            // 
            this.panelExBody.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBody.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelExBody.Controls.Add(this.txtNexttime);
            this.panelExBody.Controls.Add(this.lblnext);
            this.panelExBody.Controls.Add(this.lbllast);
            this.panelExBody.Controls.Add(this.txtLasttime);
            this.panelExBody.Controls.Add(this.lblDay);
            this.panelExBody.Controls.Add(this.lblParam);
            this.panelExBody.Controls.Add(this.txtParam);
            this.panelExBody.Controls.Add(this.lblTime);
            this.panelExBody.Controls.Add(this.txtFileName);
            this.panelExBody.Controls.Add(this.lblReportName);
            this.panelExBody.Controls.Add(this.txtTime);
            this.panelExBody.Controls.Add(this.lblPeriod);
            this.panelExBody.Controls.Add(this.txtDay);
            this.panelExBody.Controls.Add(this.chkSat);
            this.panelExBody.Controls.Add(this.chkFri);
            this.panelExBody.Controls.Add(this.chkThu);
            this.panelExBody.Controls.Add(this.chkWed);
            this.panelExBody.Controls.Add(this.chkTue);
            this.panelExBody.Controls.Add(this.chkMon);
            this.panelExBody.Controls.Add(this.cmbPeriod);
            this.panelExBody.Controls.Add(this.lblFooterAdd);
            this.panelExBody.Controls.Add(this.lblHeadAdd);
            this.panelExBody.Controls.Add(this.txtFooterAdd);
            this.panelExBody.Controls.Add(this.lblBcc);
            this.panelExBody.Controls.Add(this.txtBcc);
            this.panelExBody.Controls.Add(this.lblCc);
            this.panelExBody.Controls.Add(this.lblTo);
            this.panelExBody.Controls.Add(this.lblSubject);
            this.panelExBody.Controls.Add(this.txtCc);
            this.panelExBody.Controls.Add(this.txtTo);
            this.panelExBody.Controls.Add(this.txtSubject);
            this.panelExBody.Controls.Add(this.panelExBar);
            this.panelExBody.Controls.Add(this.txtHeadAdd);
            this.panelExBody.Controls.Add(this.chkSun);
            this.panelExBody.Controls.Add(this.lblFileName);
            this.panelExBody.Controls.Add(this.lblFullName);
            this.panelExBody.Controls.Add(this.txtReportName);
            this.panelExBody.Controls.Add(this.txtFullName);
            this.panelExBody.Controls.Add(this.PanelExTree);
            this.panelExBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExBody.Location = new System.Drawing.Point(0, 0);
            this.panelExBody.Name = "panelExBody";
            this.panelExBody.Size = new System.Drawing.Size(930, 502);
            this.panelExBody.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBody.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExBody.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelExBody.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBody.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExBody.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBody.Style.GradientAngle = 90;
            this.panelExBody.TabIndex = 0;
            // 
            // txtNexttime
            // 
            // 
            // 
            // 
            this.txtNexttime.Border.Class = "TextBoxBorder";
            this.txtNexttime.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtNexttime.Location = new System.Drawing.Point(636, 355);
            this.txtNexttime.Name = "txtNexttime";
            this.txtNexttime.Size = new System.Drawing.Size(110, 21);
            this.txtNexttime.TabIndex = 37;
            // 
            // lblnext
            // 
            this.lblnext.Location = new System.Drawing.Point(575, 355);
            this.lblnext.Name = "lblnext";
            this.lblnext.Size = new System.Drawing.Size(60, 23);
            this.lblnext.TabIndex = 36;
            this.lblnext.Text = "下次发送";
            this.lblnext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbllast
            // 
            this.lbllast.Location = new System.Drawing.Point(748, 355);
            this.lbllast.Name = "lbllast";
            this.lbllast.Size = new System.Drawing.Size(60, 23);
            this.lbllast.TabIndex = 35;
            this.lbllast.Text = "最近发送";
            this.lbllast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLasttime
            // 
            this.txtLasttime.BackColor = System.Drawing.SystemColors.Info;
            // 
            // 
            // 
            this.txtLasttime.Border.Class = "TextBoxBorder";
            this.txtLasttime.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtLasttime.Location = new System.Drawing.Point(808, 355);
            this.txtLasttime.Name = "txtLasttime";
            this.txtLasttime.ReadOnly = true;
            this.txtLasttime.Size = new System.Drawing.Size(110, 21);
            this.txtLasttime.TabIndex = 34;
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(399, 409);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(100, 23);
            this.lblDay.TabIndex = 33;
            this.lblDay.Text = "发送日期";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParam
            // 
            this.lblParam.Location = new System.Drawing.Point(399, 328);
            this.lblParam.Name = "lblParam";
            this.lblParam.Size = new System.Drawing.Size(100, 23);
            this.lblParam.TabIndex = 32;
            this.lblParam.Text = "报表数据配置类";
            this.lblParam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtParam
            // 
            this.txtParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtParam.Border.Class = "TextBoxBorder";
            this.txtParam.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtParam.Location = new System.Drawing.Point(500, 328);
            this.txtParam.Name = "txtParam";
            this.txtParam.Size = new System.Drawing.Size(418, 21);
            this.txtParam.TabIndex = 31;
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(439, 355);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(60, 23);
            this.lblTime.TabIndex = 29;
            this.lblTime.Text = "发送时间";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.Border.Class = "TextBoxBorder";
            this.txtFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileName.Location = new System.Drawing.Point(500, 86);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(164, 21);
            this.txtFileName.TabIndex = 1;
            // 
            // lblReportName
            // 
            this.lblReportName.Location = new System.Drawing.Point(668, 86);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Size = new System.Drawing.Size(60, 23);
            this.lblReportName.TabIndex = 21;
            this.lblReportName.Text = "报表名称";
            this.lblReportName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTime
            // 
            // 
            // 
            // 
            this.txtTime.Border.Class = "TextBoxBorder";
            this.txtTime.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTime.Location = new System.Drawing.Point(500, 355);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(60, 21);
            this.txtTime.TabIndex = 10;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Location = new System.Drawing.Point(399, 382);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(100, 23);
            this.lblPeriod.TabIndex = 28;
            this.lblPeriod.Text = "发送间隔";
            this.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDay
            // 
            // 
            // 
            // 
            this.txtDay.Border.Class = "TextBoxBorder";
            this.txtDay.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDay.Location = new System.Drawing.Point(500, 409);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(120, 21);
            this.txtDay.TabIndex = 11;
            this.txtDay.Visible = false;
            // 
            // chkSat
            // 
            this.chkSat.AutoSize = true;
            this.chkSat.Location = new System.Drawing.Point(860, 413);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(60, 16);
            this.chkSat.TabIndex = 18;
            this.chkSat.Text = "星期六";
            this.chkSat.UseVisualStyleBackColor = true;
            this.chkSat.Visible = false;
            // 
            // chkFri
            // 
            this.chkFri.AutoSize = true;
            this.chkFri.Location = new System.Drawing.Point(800, 413);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(60, 16);
            this.chkFri.TabIndex = 17;
            this.chkFri.Text = "星期五";
            this.chkFri.UseVisualStyleBackColor = true;
            this.chkFri.Visible = false;
            // 
            // chkThu
            // 
            this.chkThu.AutoSize = true;
            this.chkThu.Location = new System.Drawing.Point(740, 413);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(60, 16);
            this.chkThu.TabIndex = 16;
            this.chkThu.Text = "星期四";
            this.chkThu.UseVisualStyleBackColor = true;
            this.chkThu.Visible = false;
            // 
            // chkWed
            // 
            this.chkWed.AutoSize = true;
            this.chkWed.Location = new System.Drawing.Point(680, 413);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(60, 16);
            this.chkWed.TabIndex = 15;
            this.chkWed.Text = "星期三";
            this.chkWed.UseVisualStyleBackColor = true;
            this.chkWed.Visible = false;
            // 
            // chkTue
            // 
            this.chkTue.AutoSize = true;
            this.chkTue.Location = new System.Drawing.Point(620, 413);
            this.chkTue.Name = "chkTue";
            this.chkTue.Size = new System.Drawing.Size(60, 16);
            this.chkTue.TabIndex = 14;
            this.chkTue.Text = "星期二";
            this.chkTue.UseVisualStyleBackColor = true;
            this.chkTue.Visible = false;
            // 
            // chkMon
            // 
            this.chkMon.AutoSize = true;
            this.chkMon.Location = new System.Drawing.Point(560, 413);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(60, 16);
            this.chkMon.TabIndex = 13;
            this.chkMon.Text = "星期一";
            this.chkMon.UseVisualStyleBackColor = true;
            this.chkMon.Visible = false;
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DisplayMember = "Text";
            this.cmbPeriod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.ItemHeight = 15;
            this.cmbPeriod.Location = new System.Drawing.Point(500, 382);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(121, 21);
            this.cmbPeriod.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbPeriod.TabIndex = 9;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblFooterAdd
            // 
            this.lblFooterAdd.Location = new System.Drawing.Point(399, 274);
            this.lblFooterAdd.Name = "lblFooterAdd";
            this.lblFooterAdd.Size = new System.Drawing.Size(100, 23);
            this.lblFooterAdd.TabIndex = 27;
            this.lblFooterAdd.Text = "底部附加内容";
            this.lblFooterAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeadAdd
            // 
            this.lblHeadAdd.Location = new System.Drawing.Point(399, 220);
            this.lblHeadAdd.Name = "lblHeadAdd";
            this.lblHeadAdd.Size = new System.Drawing.Size(100, 23);
            this.lblHeadAdd.TabIndex = 26;
            this.lblHeadAdd.Text = "头部附加内容";
            this.lblHeadAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFooterAdd
            // 
            this.txtFooterAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtFooterAdd.Border.Class = "TextBoxBorder";
            this.txtFooterAdd.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFooterAdd.Location = new System.Drawing.Point(500, 274);
            this.txtFooterAdd.Multiline = true;
            this.txtFooterAdd.Name = "txtFooterAdd";
            this.txtFooterAdd.Size = new System.Drawing.Size(418, 48);
            this.txtFooterAdd.TabIndex = 8;
            // 
            // lblBcc
            // 
            this.lblBcc.Location = new System.Drawing.Point(399, 165);
            this.lblBcc.Name = "lblBcc";
            this.lblBcc.Size = new System.Drawing.Size(100, 23);
            this.lblBcc.TabIndex = 24;
            this.lblBcc.Text = "密件抄送";
            this.lblBcc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBcc
            // 
            this.txtBcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtBcc.Border.Class = "TextBoxBorder";
            this.txtBcc.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtBcc.Location = new System.Drawing.Point(500, 165);
            this.txtBcc.Name = "txtBcc";
            this.txtBcc.Size = new System.Drawing.Size(418, 21);
            this.txtBcc.TabIndex = 5;
            // 
            // lblCc
            // 
            this.lblCc.Location = new System.Drawing.Point(399, 139);
            this.lblCc.Name = "lblCc";
            this.lblCc.Size = new System.Drawing.Size(100, 23);
            this.lblCc.TabIndex = 23;
            this.lblCc.Text = "抄送";
            this.lblCc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(399, 112);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(100, 23);
            this.lblTo.TabIndex = 22;
            this.lblTo.Text = "收件人";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubject
            // 
            this.lblSubject.Location = new System.Drawing.Point(399, 191);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(100, 23);
            this.lblSubject.TabIndex = 25;
            this.lblSubject.Text = "主旨";
            this.lblSubject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCc
            // 
            this.txtCc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtCc.Border.Class = "TextBoxBorder";
            this.txtCc.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCc.Location = new System.Drawing.Point(500, 139);
            this.txtCc.Name = "txtCc";
            this.txtCc.Size = new System.Drawing.Size(418, 21);
            this.txtCc.TabIndex = 4;
            // 
            // txtTo
            // 
            this.txtTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtTo.Border.Class = "TextBoxBorder";
            this.txtTo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTo.Location = new System.Drawing.Point(500, 112);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(418, 21);
            this.txtTo.TabIndex = 3;
            // 
            // txtSubject
            // 
            this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSubject.Border.Class = "TextBoxBorder";
            this.txtSubject.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSubject.Location = new System.Drawing.Point(500, 192);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(418, 21);
            this.txtSubject.TabIndex = 6;
            // 
            // panelExBar
            // 
            this.panelExBar.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBar.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBar.Controls.Add(this.btnExec);
            this.panelExBar.Controls.Add(this.btnCancel);
            this.panelExBar.Controls.Add(this.btnPara);
            this.panelExBar.Controls.Add(this.btnAtt);
            this.panelExBar.Controls.Add(this.btnQuit);
            this.panelExBar.Controls.Add(this.btnAdd);
            this.panelExBar.Controls.Add(this.btnEdit);
            this.panelExBar.Controls.Add(this.btnDel);
            this.panelExBar.Controls.Add(this.btnSave);
            this.panelExBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExBar.Location = new System.Drawing.Point(391, 0);
            this.panelExBar.Name = "panelExBar";
            this.panelExBar.Size = new System.Drawing.Size(539, 41);
            this.panelExBar.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBar.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExBar.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelExBar.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBar.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExBar.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBar.Style.GradientAngle = 90;
            this.panelExBar.TabIndex = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(298, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPara
            // 
            this.btnPara.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPara.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPara.Enabled = false;
            this.btnPara.Location = new System.Drawing.Point(247, 9);
            this.btnPara.Name = "btnPara";
            this.btnPara.Size = new System.Drawing.Size(45, 23);
            this.btnPara.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPara.TabIndex = 6;
            this.btnPara.Text = "参数";
            this.btnPara.Click += new System.EventHandler(this.btnPara_Click);
            // 
            // btnAtt
            // 
            this.btnAtt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAtt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAtt.Enabled = false;
            this.btnAtt.Location = new System.Drawing.Point(196, 9);
            this.btnAtt.Name = "btnAtt";
            this.btnAtt.Size = new System.Drawing.Size(45, 23);
            this.btnAtt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAtt.TabIndex = 5;
            this.btnAtt.Text = "附件";
            this.btnAtt.Click += new System.EventHandler(this.btnAtt_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuit.Location = new System.Drawing.Point(482, 9);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(45, 23);
            this.btnQuit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAdd.Location = new System.Drawing.Point(43, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(45, 23);
            this.btnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEdit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEdit.Location = new System.Drawing.Point(94, 9);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(45, 23);
            this.btnEdit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "修改";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(145, 9);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(45, 23);
            this.btnDel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(431, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtHeadAdd
            // 
            this.txtHeadAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtHeadAdd.Border.Class = "TextBoxBorder";
            this.txtHeadAdd.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtHeadAdd.Location = new System.Drawing.Point(500, 220);
            this.txtHeadAdd.Multiline = true;
            this.txtHeadAdd.Name = "txtHeadAdd";
            this.txtHeadAdd.Size = new System.Drawing.Size(418, 48);
            this.txtHeadAdd.TabIndex = 7;
            // 
            // chkSun
            // 
            this.chkSun.AutoSize = true;
            this.chkSun.Location = new System.Drawing.Point(500, 413);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(60, 16);
            this.chkSun.TabIndex = 12;
            this.chkSun.Text = "星期日";
            this.chkSun.UseVisualStyleBackColor = true;
            this.chkSun.Visible = false;
            // 
            // lblFileName
            // 
            this.lblFileName.Location = new System.Drawing.Point(399, 86);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(100, 23);
            this.lblFileName.TabIndex = 20;
            this.lblFileName.Text = "文件名称";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFullName
            // 
            this.lblFullName.Location = new System.Drawing.Point(399, 60);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(100, 23);
            this.lblFullName.TabIndex = 19;
            this.lblFullName.Text = "类完整名称";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReportName
            // 
            this.txtReportName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtReportName.Border.Class = "TextBoxBorder";
            this.txtReportName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtReportName.Location = new System.Drawing.Point(730, 86);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(188, 21);
            this.txtReportName.TabIndex = 2;
            // 
            // txtFullName
            // 
            this.txtFullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtFullName.Border.Class = "TextBoxBorder";
            this.txtFullName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFullName.Location = new System.Drawing.Point(500, 60);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(418, 21);
            this.txtFullName.TabIndex = 0;
            // 
            // btnExec
            // 
            this.btnExec.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExec.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExec.Location = new System.Drawing.Point(349, 9);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(45, 23);
            this.btnExec.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExec.TabIndex = 8;
            this.btnExec.Text = "运行";
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // SetNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 502);
            this.Controls.Add(this.panelExBody);
            this.DoubleBuffered = true;
            this.Name = "SetNotify";
            this.Text = "报表排程设定";
            this.Load += new System.EventHandler(this.SetNotify_Load);
            this.PanelExTree.ResumeLayout(false);
            this.GroupPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AdvTree)).EndInit();
            this.panelExBody.ResumeLayout(false);
            this.panelExBody.PerformLayout();
            this.panelExBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.PanelEx PanelExTree;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel;
        internal DevComponents.AdvTree.AdvTree AdvTree;
        internal DevComponents.AdvTree.ColumnHeader ColId;
        internal DevComponents.AdvTree.ColumnHeader ColName;
        internal DevComponents.AdvTree.Node root;
        internal DevComponents.DotNetBar.ElementStyle ElementStyle1;
        private DevComponents.DotNetBar.PanelEx panelExBody;
        private DevComponents.DotNetBar.ButtonX btnQuit;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnEdit;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblFullName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtReportName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFullName;
        private DevComponents.DotNetBar.PanelEx panelExBar;
        private DevComponents.DotNetBar.Controls.TextBoxX txtHeadAdd;
        private System.Windows.Forms.CheckBox chkSun;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSubject;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFooterAdd;
        private System.Windows.Forms.Label lblBcc;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBcc;
        private System.Windows.Forms.Label lblCc;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblSubject;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCc;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTo;
        private DevComponents.DotNetBar.ButtonX btnPara;
        private DevComponents.DotNetBar.ButtonX btnAtt;
        private System.Windows.Forms.Label lblFooterAdd;
        private System.Windows.Forms.Label lblHeadAdd;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbPeriod;
        private System.Windows.Forms.CheckBox chkSat;
        private System.Windows.Forms.CheckBox chkFri;
        private System.Windows.Forms.CheckBox chkThu;
        private System.Windows.Forms.CheckBox chkWed;
        private System.Windows.Forms.CheckBox chkTue;
        private System.Windows.Forms.CheckBox chkMon;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTime;
        private System.Windows.Forms.Label lblPeriod;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDay;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFileName;
        private System.Windows.Forms.Label lblReportName;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblParam;
        private DevComponents.DotNetBar.Controls.TextBoxX txtParam;
        private System.Windows.Forms.Label lblDay;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.Label lbllast;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLasttime;
        private DevComponents.DotNetBar.Controls.TextBoxX txtNexttime;
        private System.Windows.Forms.Label lblnext;
        private DevComponents.DotNetBar.ButtonX btnExec;
    }
}