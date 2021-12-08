using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CamliCafeOtomasyon
{
    public partial class FaturaForm : DevExpress.XtraEditors.XtraForm
    {
        public FaturaForm()
        {
            InitializeComponent();
        }

        private void FaturaForm_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

        }

        private void FaturaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            listBoxControl3.Items.Clear();
            this.Hide();
        }
    }
}