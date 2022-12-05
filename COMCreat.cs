using System;
using Lead.Tool.Interface;
using System.Resources;
using System.Drawing;
using Lead.Tool.Resources;

namespace Lead.Tool.COM
{
    public class COMCreat : ICreat
    {
        public ITool GetInstance(string Name, string Path)
        {
             return new COMTool(Name, Path) ;
        }

        public System.Drawing.Image Icon
        {
            get
            {
                return (Image)ImageManager.GetImage("串口");
            }
        }

        public string Name
        {
            get
            {
                return "COM";
            }
        }
    }
}
