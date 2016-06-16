using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Collections;

namespace DrizzlingTest
{
    public partial class SetParameters : Office2007Form
    {
        public Hashtable parameters { get; set; }

        public SetParameters()
        {
            InitializeComponent();
            parameters = new Hashtable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.parameters.Clear();
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if ((item.Cells["Colid"].Value != null) && (item.Cells["Colvalue"].Value != null))
                {
                    this.parameters.Add(item.Cells["Colid"].Value, item.Cells["Colvalue"].Value);
                }
            }
            this.Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetParameters_Load(object sender, EventArgs e)
        {
            if (this.parameters.Count > 0)
            {
                foreach (DictionaryEntry item in this.parameters)
                {
                    dataGridView.Rows.Add(new object[] { item.Key, item.Value });
                }
            }
        }

    }
}
