using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lead.Tool.COM
{
    public partial class DebugUI : UserControl
    {
        COMTool _Proxy = null;
        public DebugUI(COMTool proxy)
        {
            InitializeComponent();
            _Proxy = proxy;
            _Proxy.DataRecivedEvent += DataRecived;
        }

        private void DataRecived(string Data)
        {
            richTextBoxRecive.Text = Data;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            _Proxy.SendData(richTextBoxSend.Text);
        }
    }
}
