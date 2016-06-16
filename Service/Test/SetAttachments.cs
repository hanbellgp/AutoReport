using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DrizzlingTest
{
    public partial class SetAttachments : Office2007Form
    {
        public Attachment[] attachments { get; set; }

        public SetAttachments()
        {
            InitializeComponent();
            attachments = new Attachment[] { };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Attachment[] atts = new Attachment[] { };
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if ((item.Cells["Colid"].Value != null) && (item.Cells["Colvalue"].Value != null) && (item.Cells["Coltype"].Value != null))
                {
                    Attachment att = new Attachment(item.Cells["Colid"].Value.ToString(), item.Cells["Colvalue"].Value.ToString(), item.Cells["Coltype"].Value.ToString());
                    Array.Resize(ref atts, atts.Length + 1);
                    atts.SetValue(att, atts.Length - 1);
                }
            }
            if (atts.Length > 0) this.attachments = atts;
            this.Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetAttachments_Load(object sender, EventArgs e)
        {
            if (this.attachments.Length > 0)
            {
                foreach (Attachment  item in this.attachments)
                {
                    dataGridView.Rows.Add(new object[] { item.reportClass, item.attName,item.attType });
                }
            }
        }
    }
}
