using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Lead.Tool.XML;

namespace Lead.Tool.COM
{
    public partial class ConfigUI : UserControl
    {
        COMTool _Porxy = null;
        public ConfigUI(COMTool proxy)
        {
            InitializeComponent();
            _Porxy = proxy;

            comboBoxCheck.Items.Add(Parity.Even);
            comboBoxCheck.Items.Add(Parity.Mark);
            comboBoxCheck.Items.Add(Parity.None);
            comboBoxCheck.Items.Add(Parity.Odd);
            comboBoxCheck.Items.Add(Parity.Space);

            comboBoxData.Items.Add(5);
            comboBoxData.Items.Add(6);
            comboBoxData.Items.Add(7);
            comboBoxData.Items.Add(8);

            comboBoxStop.Items.Add(StopBits.None);
            comboBoxStop.Items.Add(StopBits.One);
            comboBoxStop.Items.Add(StopBits.OnePointFive);
            comboBoxStop.Items.Add(StopBits.Two);

            comboBoxReadMode.Items.Add(ReadModeEnum.ReadExsting);
            comboBoxReadMode.Items.Add(ReadModeEnum.Readline);

            textBoxCOM.Text = _Porxy._Config.COM;
            comboBoxPort.Text = _Porxy._Config.BaudRate.ToString();
            comboBoxCheck.Text = _Porxy._Config.CheckBit.ToString();
            comboBoxData.Text = _Porxy._Config.DataBit.ToString();
            comboBoxStop.Text = _Porxy._Config.StopBit.ToString();
            comboBoxReadMode.Text = _Porxy._Config.ReadMode.ToString();
            textBoxSendSize.Text = _Porxy._Config.SendSize.ToString();
            textBoxReciveSize.Text = _Porxy._Config.ReciveSize.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _Porxy._Config.COM = textBoxCOM.Text;
                _Porxy._Config.BaudRate = Convert.ToInt32(comboBoxPort.Text);
                _Porxy._Config.CheckBit = comboBoxCheck.Text == Parity.Even.ToString() ? Parity.Even :
                                            comboBoxCheck.Text == Parity.Mark.ToString() ? Parity.Mark :
                                            comboBoxCheck.Text == Parity.None.ToString() ? Parity.None :
                                            comboBoxCheck.Text == Parity.Odd.ToString() ? Parity.Odd : Parity.Space;

                _Porxy._Config.DataBit = Convert.ToInt32(comboBoxData.Text);

                _Porxy._Config.StopBit = comboBoxStop.Text == StopBits.None.ToString() ? StopBits.None :
                                            comboBoxStop.Text == StopBits.One.ToString() ? StopBits.One :
                                            comboBoxStop.Text == StopBits.OnePointFive.ToString() ? StopBits.OnePointFive : StopBits.Two;

                _Porxy._Config.ReadMode =
                    comboBoxStop.Text == ReadModeEnum.ReadExsting.ToString() ? ReadModeEnum.ReadExsting : ReadModeEnum.Readline;

                _Porxy._Config.SendSize = Convert.ToUInt32(textBoxSendSize.Text.Trim());
                _Porxy._Config.ReciveSize = Convert.ToUInt32(textBoxReciveSize.Text.Trim());


                var re = XmlSerializerHelper.WriteXML(_Porxy._Config, _Porxy._ConfigPath, typeof(COMConfig));

                if (!re)
                {
                    throw new Exception("保存至文件失败");

                }

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:"+ex.Message);
            }
        }
    }
}
