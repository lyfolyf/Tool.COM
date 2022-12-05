using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lead.Tool.Interface;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Lead.Tool.XML;

namespace Lead.Tool.COM
{
    public delegate void DataRecived(string Data);
    public  class COMTool : ITool
    {
        public event DataRecived DataRecivedEvent;
        public COMConfig _Config = null;

        private SerialPort _SerialInPort = null;
        private String StrTemp = "";
        private ConfigUI _ConfigControl = null;
        private DebugUI _DebugControl = null;
        private IToolState _State = IToolState.ToolMin;
        public string _ConfigPath = "";

        private COMTool()
        {

        }

        public COMTool(string Name, string Path)
        {
            _ConfigPath = Path;
            if (File.Exists(Path))
            {
                _Config = (COMConfig)XmlSerializerHelper.ReadXML(Path, typeof(COMConfig));
            }
            else
            {
                _Config = new COMConfig();
            }
            _ConfigControl = new ConfigUI(this);
            _DebugControl = new DebugUI(this);
        }

        public Control ConfigUI
        {
            get
            {
                return _ConfigControl;
            }
        }

        public Control DebugUI
        {
            get
            {
                return _DebugControl;
            }
        }

        public IToolState State
        {
            get
            {
                return _State;
            }
        }

        public void Init()
        {
            if (_SerialInPort == null)
            {
                _SerialInPort = new SerialPort();
                _SerialInPort.PortName = _Config.COM;
                _SerialInPort.Parity = _Config.CheckBit;
                _SerialInPort.DataBits = _Config.DataBit;
                _SerialInPort.BaudRate = _Config.BaudRate;
                _SerialInPort.StopBits = _Config.StopBit;
                _SerialInPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataRecived);
            }
            _State = IToolState.ToolInit;
        }
        
        private void DataRecived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_Config.ReadMode ==  ReadModeEnum.Readline)
                {
                    StrTemp += _SerialInPort.ReadLine();
                }
                else if (_Config.ReadMode == ReadModeEnum.ReadExsting)
                {
                    StrTemp += _SerialInPort.ReadExisting();
                }
                else
                {
                    throw new Exception("未在配置界面配置数据接收方式！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_Config.Name+"接收结果出问题:" + ex.Message);
            }

            if (DataRecivedEvent != null)
            {
                if (_Config.ReadMode == ReadModeEnum.Readline)
                {
                    DataRecivedEvent(StrTemp);
                    StrTemp = "";
                }
                else if (_Config.ReadMode == ReadModeEnum.ReadExsting)
                {
                    if (StrTemp.Contains("\r"))
                    {
                        DataRecivedEvent(StrTemp);
                        StrTemp = "";
                    }
                }
                else
                {
                    throw new Exception("未在配置界面配置数据接收方式！");
                }
            }
        }

        public void Start()
        {
            while (_SerialInPort.IsOpen == false)
            {
                _SerialInPort.Open();
                Thread.Sleep(30);
            }
            _State = IToolState.ToolRunning;

        }

        public void Terminate()
        {
            if (_SerialInPort.IsOpen)
            {
                _SerialInPort.Close();
            }
            _State = IToolState.ToolTerminate;
        }

        public void SendData(string str, bool IsHex = false)
        {
            try
            {
                if (!IsHex)
                {
                    _SerialInPort.Write(str);
                }
                else
                {
                    if (str.Length % 2 != 0)
                    {
                        str += " ";
                    }

                    byte[] temp = new byte[str.Length / 2];

                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
                    }

                    _SerialInPort.Write(temp, 0, temp.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
