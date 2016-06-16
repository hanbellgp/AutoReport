namespace DrizzlingTest
{
    partial class SetAttachments
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelExBar = new DevComponents.DotNetBar.PanelEx();
            this.btnQuit = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.dataGridView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Colid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Colvalue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coltype = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panelExBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelExBar
            // 
            this.panelExBar.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBar.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBar.Controls.Add(this.btnQuit);
            this.panelExBar.Controls.Add(this.btnSave);
            this.panelExBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExBar.Location = new System.Drawing.Point(0, 0);
            this.panelExBar.Name = "panelExBar";
            this.panelExBar.Size = new System.Drawing.Size(761, 41);
            this.panelExBar.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBar.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelExBar.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelExBar.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBar.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExBar.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBar.Style.GradientAngle = 90;
            this.panelExBar.TabIndex = 32;
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuit.Location = new System.Drawing.Point(706, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(45, 23);
            this.btnQuit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(652, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Colid,
            this.Colvalue,
            this.Coltype});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridView.Location = new System.Drawing.Point(0, 41);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(761, 221);
            this.dataGridView.TabIndex = 33;
            // 
            // Colid
            // 
            this.Colid.HeaderText = "报表类";
            this.Colid.Name = "Colid";
            this.Colid.Width = 400;
            // 
            // Colvalue
            // 
            this.Colvalue.HeaderText = "附件名称";
            this.Colvalue.Name = "Colvalue";
            this.Colvalue.Width = 200;
            // 
            // Coltype
            // 
            this.Coltype.HeaderText = "类型";
            this.Coltype.Items.AddRange(new object[] {
            "doc",
            "html",
            "pdf",
            "rpt",
            "xls"});
            this.Coltype.Name = "Coltype";
            this.Coltype.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Coltype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SetAttachments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 262);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelExBar);
            this.DoubleBuffered = true;
            this.Name = "SetAttachments";
            this.Text = "SetAttachments";
            this.Load += new System.EventHandler(this.SetAttachments_Load);
            this.panelExBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelExBar;
        private DevComponents.DotNetBar.ButtonX btnQuit;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Colid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Colvalue;
        private System.Windows.Forms.DataGridViewComboBoxColumn Coltype;
    }
}