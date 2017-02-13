using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessControlSystem.Lib
{
    /// <summary>
    /// MAC地址类
    /// </summary>
    class MacAddr
    {
        private byte[] mac;

        /// <summary>
        /// MAC地址
        /// </summary>
        public byte[] Mac
        {
            get { return mac; }
            set
            {
                if (value.Length == 6)
                {
                    mac = value;
                }
                else
                { 
                    mac = new byte[6];
                }
            }
        }
        public MacAddr()
        {
            mac = new byte[6];
        }
        /// <summary>
        /// 使用字符串的构造函数
        /// </summary>
        /// <param name="str"></param>
        public MacAddr(string str)
        {
            mac = new byte[6];
            str = str.Replace(" ", "");
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    mac[i] = Convert.ToByte(str.Substring(i * 2 + i, 2), 16);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string ToString()
        {
            return mac[0].ToString("x2") + ":" + mac[1].ToString("x2") + ":" + 
                   mac[2].ToString("x2") + ":" + mac[3].ToString("x2") + ":" + 
                   mac[4].ToString("x2") + ":" + mac[5].ToString("x2");
        }
    }
}
