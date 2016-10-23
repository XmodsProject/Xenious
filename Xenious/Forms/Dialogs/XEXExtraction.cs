using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious.Forms.Dialogs
{
    partial class XEXExtraction : Form
    {
        public XEXExtraction()
        {
            InitializeComponent();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x = checkedListBox1.SelectedIndex;

            for(int i = 0; i < checkedListBox1.SelectedIndices.Count; i++)
            {
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            }

            checkedListBox1.SetItemCheckState(x, CheckState.Checked);
        }
    }
}
