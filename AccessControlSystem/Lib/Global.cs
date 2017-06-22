using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessControlSystem.Lib
{
    class Global
    {
        public static LeafSoft.Units.NetTCPServer netTCPServer = null;
        public static byte[][] device_list; /* 连接上的设备列表 */
        public static byte[] data_recv;     /* 接收到的数据 */


        public static bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Milliseconds;
                //Application.DoEvents();
            }
            while (s < delayTime);
            return true;
        }
        public static bool wait_data(int ms)
        {
            while ((data_recv == null) && (ms != 0))
            {
                Delay(1);
                ms--;
            }

            return ms == 0 ? false : true;
        }
    }
}
