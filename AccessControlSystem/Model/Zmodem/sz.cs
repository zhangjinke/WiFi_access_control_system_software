using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AccessControlSystem.Model.Zmodem
{
    class sz
    {
        public static byte[] TX_BUFFER = new byte[zdef.TX_BUFFER_SIZE]; /* sender buffer */
        public static byte file_cnt = 0;		                         /* count of number of files opened */
        public static byte Rxflags  = 0;	                  	         /* rx parameter flags */
        public static byte ZF2_OP;		                  	             /* file transfer option */ 

        /* start zmodem send process */
        public static void zs_start(string path)
        {
            zdef.zfile zf = new zdef.zfile();
	        Int32 res = 1;

            zdef.rt_kprintf("\r\nsz: ready...\r\n");	   /* here ready to send things */

            zf.fname = path;
	        res = zsend_files(zf);
            if (res == 0)
            {
                //zdef.rt_kprintf("\r\nfile: %s \r\nsize: %ld bytes\r\nsend completed.\r\n",
                //          p,zf.bytes_received);
            }
            else
            {
                //zdef.rt_kprintf("\r\nfile: %s \r\nsize: 0 bytes\r\nsend failed.\r\n",p);
            }

	        return;
        }

        /* init the parameters */
        public static void zsend_init()
        {
	        Int32 res = -1;

	        zcore.zinit_parameter();
            //for(;;)          /* wait zdef.ZPAD */
            //{
            //    res = zdevice.zread_line(800);
            //    if (res == zdef.ZPAD) break;
            //}
            //for (;;) 
            //{
            //    res = zcore.zget_header(ref zcore.rx_header);
            //    if (res == zdef.ZRINIT) break;
            //}
            //if (0 != (zcore.rx_header[zdef.ZF1] & zdef.ZRQNVH))
            //{
            //    zcore.zput_pos(0x80);	/* Show we can var header */
            //    zcore.zsend_hex_header(zdef.ZRQINIT, zcore.tx_header);
            //}
            //Rxflags = (byte)(zcore.rx_header[zdef.ZF0] & 0xFF);
            //if (0 != (Rxflags & zdef.CANFC32)) zcore.Txfcs32 = 1;    /* used 32bits CRC check */

            //if ((ZF2_OP == zdef.ZTRLE) && (0 != (Rxflags & zdef.CANRLE)))	  /* for RLE packet */
            //     zcore.Txfcs32 = 2;
            //else
            //    ZF2_OP = 0;
            /* send SINIT cmd */

            zcore.zsend_hex_header(zdef.ZRQINIT, zcore.tx_header);

	        return;
        }

        /* send files */
        public static Int32 zsend_files(zdef.zfile zf)
        {
            //char *p,*q;
            //char *str = "/";
            //stat finfo;
	        Int32 res = -1;
            FileInfo fi;

	        if (zf.fname == null)
	        {
	            zdef.rt_kprintf("\r\nerror: no file to be send.\r\n");
		        return res;
	        }
            ;
            if ((zf.fs = File.Open(zf.fname, FileMode.Open)) == null)
	        {
                //zdef.rt_kprintf("\r\ncan not open file:%s\r\n",zf.fname+1);
	            return res;
	        }

            fi = new FileInfo(zf.fname);

	        zf.file_end = 0;
	        ++file_cnt;	 
	        /* extract file name */
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(fi.LastWriteTime - startTime).TotalSeconds; // 相差秒数

            zdevice.Left_sizes += (UInt32)fi.Length;
            string info = Convert.ToString(fi.Length, 10) + " " +
                          Convert.ToString(timeStamp, 8) + " " +
                          "0 " +
                          "3 " +
                          Convert.ToString(file_cnt, 10) + " " +
                          Convert.ToString(zdevice.Left_sizes, 10);
            zdevice.Left_sizes -= (UInt32)fi.Length;
            TX_BUFFER[127] = (byte)((fi.Length + 127) >> 7);
            TX_BUFFER[126] = (byte)((fi.Length + 127) >> 15);

	        zsend_init();
	        /* start sending files */
            res = zsend_file(ref zf, TX_BUFFER, (UInt16)fi.Length);
	        zsay_bibi();
            zf.fs.Close();
 
	        return res;
        }

        /* send file name and related info */
        public static Int32 zsend_file(ref zdef.zfile zf, byte[] buf, UInt16 len)
        {
	        byte cnt;
	        Int32 res = -1;

	        for (cnt=0;cnt<5;cnt++) 
	        {  
		        zcore.tx_header[zdef.ZF0] = zcore.ZF0_CMD;	              /* file conversion option */
		        zcore.tx_header[zdef.ZF1] = zcore.ZF1_CMD;                /* file management option */
		        zcore.tx_header[zdef.ZF2] = (byte)(zcore.ZF3_CMD|ZF2_OP); /* file transfer option   */
		        zcore.tx_header[zdef.ZF3] = zcore.ZF3_CMD;
		        zcore.zsend_bin_header(zdef.ZFILE, zcore.tx_header);
		        zcore.zsend_bin_data(buf, (Int16)len, zdef.ZCRCW);
        loop:
		        res = zcore.zget_header(ref zcore.rx_header);
		        switch (res) 
		        {
		        case zdef.ZRINIT:
			         while ((res = zdevice.zread_line(50)) > 0)
			         {
				         if (res == zdef.ZPAD) 
				         {
					        goto loop;
				         }
			         }
			         break;
		        case zdef.ZCAN:
		        case zdef.TIMEOUT:
		        case zdef.ZABORT:
		        case zdef.ZFIN:
                     break;
		        case -1:
		        case zdef.ZNAK:
			         break;
		        case zdef.ZCRC:	                         /* no CRC request */
			         goto loop;
		        case zdef.ZFERR:
		        case zdef.ZSKIP:
			         break;
		        case zdef.ZRPOS:		           	         /* here we want */
		             zcore.zget_pos(ref zcore.Rxpos);
			         zcore.Txpos = zcore.Rxpos;
                     return (zsend_file_data(ref zf));
		        default:
			         break;
		        } 
	        }

	        return res;
        }

        /* send the file data */
        public static Int32 zsend_file_data(ref zdef.zfile zf)
        {
	        Int16 cnt;
	        byte cmd;
	        Int32 res = -1;
            bool is_get_syn1 = false;

	        /* send zdef.ZDATA packet, start to send data */
        start_send:
	        zcore.zput_pos(zcore.Txpos);
	        zcore.zsend_bin_header(zdef.ZDATA, zcore.tx_header);
	        do
	        {
                cnt = (Int16)zfill_buffer(ref zf, ref TX_BUFFER, zdef.RX_BUFFER_SIZE);
		        if (cnt < zdef.RX_BUFFER_SIZE )
			        cmd = zdef.ZCRCE;				
		        else
			        cmd = zdef.ZCRCG;
		        zcore.zsend_bin_data(TX_BUFFER, cnt, cmd);
		        zf.bytes_received= zcore.Txpos += (UInt32)cnt;
                if (cmd == zdef.ZCRCW)
                { 
                    is_get_syn1 = true;
                    break;
                }
	        } while (cnt == zdef.RX_BUFFER_SIZE);
	        for (;;) 	                     /*  get ack and check if send finish */
	        {
                if (is_get_syn1)
                {
                    is_get_syn1 = false;
                    goto get_syn1;
                }
		        zcore.zput_pos(zcore.Txpos);
		        zcore.zsend_bin_header(zdef.ZEOF, zcore.tx_header);
        get_syn1:
                res = zget_sync();
		        switch (res) 
		        {
		        case zdef.ZACK:
			         goto get_syn1;
		        case zdef.ZNAK:
			         continue;
		        case zdef.ZRPOS:				        /* resend here */
                     zf.fs.Seek(zcore.Txpos, SeekOrigin.Begin);
			         goto start_send;
		        case zdef.ZRINIT:		           /*  send finish,then begin to send next file */
			         return 0;
		        case zdef.ZSKIP:
		        case -1:
		             return res;
		        default:
                     return res;
		        }
	        }
        }

        /* fill file data to buffer*/
        public static UInt16 zfill_buffer(ref zdef.zfile zf, ref byte[] buf, UInt16 size)
        {
            return (UInt16)zf.fs.Read(buf, 0, size);
        }

        /* wait sync(ack) from the receiver */
        public static Int32 zget_sync()
        {
            Int32 res = -1;

	        for (;;) 
	        {
		        res = zcore.zget_header(ref zcore.rx_header);
		        switch (res) 
		        {
		        case zdef.ZCAN:
		        case zdef.ZABORT:
		        case zdef.ZFIN:
		        case zdef.TIMEOUT:
			         return -1;
		        case zdef.ZRPOS:		             /* get pos, need to resend */
		             zcore.zget_pos(ref zcore.Rxpos);
			         zcore.Txpos = zcore.Rxpos;
			         return res;
		        case zdef.ZACK:
			         return res;
		        case zdef.ZRINIT:		         /* get zdef.ZRINIT indicate that the prev file send completed */
			         return res;
		        case zdef.ZSKIP:
			         return res;
		        case -1:
		        default:
			         zcore.zsend_bin_header(zdef.ZNAK, zcore.tx_header);
			         continue;
		        }
	        }
        }

        /* say "bibi" to the receiver */
        public static void zsay_bibi()
        {
	        for (;;) 
	        {
		        zcore.zput_pos(0);	            	      /* reninit position of next file*/
		        zcore.zsend_hex_header(zdef.ZFIN, zcore.tx_header);	  /* send finished session cmd */
		        switch (zcore.zget_header(ref zcore.rx_header)) 
		        {
		        case zdef.ZFIN:
			        zdevice.zsend_line('O'); 
			        zdevice.zsend_line('O'); 
                    goto case zdef.TIMEOUT;
		        case zdef.ZCAN:
                    goto case zdef.TIMEOUT;
		        case zdef.TIMEOUT:
			        return;
		        }
	        }
        }
    }
}
