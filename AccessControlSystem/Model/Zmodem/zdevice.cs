using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace AccessControlSystem.Model.Zmodem
{
    class zdevice
    {
        public static SerialPort serialPort;

        public static UInt32 Line_left = 0;		      /* left number of data in the read line buffer*/
        public static UInt32 Left_sizes = 0;		  /* left file sizes */
        public static UInt32 Baudrate = zdef.BITRATE; /* console baudrate */

        public static UInt32 get_device_baud()
        {
            return (Baudrate);
        }

        public static UInt32 get_sys_time()
        {
            return (0);
        }

        public static void zsend_byte(UInt16 ch)
        {
            byte[] data = {(byte)ch};

            serialPort.Write(data, 0, 1);
        }
        public static void zsend_line(UInt16 c)
        {
            byte[] data = { (byte)c };

            serialPort.Write(data, 0, 1);
        }
        public static Int16 zread_line(UInt16 timeout)
        {
	        byte[] buf = new byte[1];

            while (0 != --timeout)
            {
                if (serialPort.BytesToRead > 0)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            if (0 == timeout) {
                return zdef.TIMEOUT;
            }
            Line_left = (UInt32)serialPort.Read(buf, 0, 1);

            return buf[0];
        }
        /*
         * send a string to the modem, processing for \336 (sleep 1 sec)
         *   and \335 (break signal)
         */
        public static void zsend_break(byte[] cmd)
        {
            UInt32 i = 0;

	        while (0 != cmd[i++]) 
	        {
		        switch (cmd[i]) 
		        {
		        case 0xDE:
			         continue;
		        case 0xDD:
		             Thread.Sleep(1000);
			         continue;
		        default:
			         zsend_line(cmd[i]);
			         break;
		        }
	        }
        }
        /* send cancel string to get the other end to shut up */
        public static void zsend_can()
        {
	        byte[] cmd = {24,24,24,24,24,24,24,24,24,24,0};

	        zsend_break(cmd);
	        zdef.rt_kprintf("\x0d");
	        Line_left=0;	       /* clear Line_left */

	        return;
        }
        public static Int32 rt_device_read(Int32 dev, Int32 pos, ref byte[] buffer, UInt32 size)
        {
            return 0;
        }
    }
}
