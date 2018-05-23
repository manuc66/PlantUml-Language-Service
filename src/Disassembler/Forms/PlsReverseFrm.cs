using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disassembler.Forms
{
    public partial class PlsReverseFrm : Form
    {
        public static string Input { get; set; }
        public static string OutPut { get; set; }
        public static string Root { get; set; }

        public PlsReverseFrm()
        {
            InitializeComponent();
        }

        private void tboxInput_TextChanged(object sender, EventArgs e)
        {
            Input = tboxInput.Text;
        }

        private void tboxOutput_TextChanged(object sender, EventArgs e)
        {
            OutPut = tboxOutput.Text;
        }

        private void tboxRoot_TextChanged(object sender, EventArgs e)
        {
            Root = tboxRoot.Text;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
