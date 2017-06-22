using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace AccessControlSystem.Model.Zmodem
{
    class rz
    {
        /* start zmodem receive proccess */
        public static void zr_start(string path)
        {
            zdef.zfile zf = new zdef.zfile();
            byte[] ch = new byte[1];
            byte n;
	        Int32 res = -1;

            zf.fname = path;
	        res = zrec_files(ref zf);   

            if (res == 0)
            {		  
                //zdef.rt_kprintf("\b\b\bfile: %s                           \r\n",p);
                //zdef.rt_kprintf("size: %ld bytes\r\n",zf.bytes_received);
		        zdef.rt_kprintf("receive completed.\r\n");
		        zf.fs.Close();
            }
            else
            {
                //zdef.rt_kprintf("\b\b\bfile: %s                           \r\n",p);
		        zdef.rt_kprintf("size: 0 bytes\r\n");
		        zdef.rt_kprintf("receive failed.\r\n");
		        if (null != zf.fs)
		        {
	                zf.fs.Close();
                    File.Delete(zf.fname); /* remove this file */ 
		        }	
            }
	        /* waiting,clear console buffer */
            Thread.Sleep(500);
	        while (true)
	        {
	           n = (byte)zdevice.rt_device_read(0, 0, ref ch, 1);
	           if (n == 0) break;
	        }

	        return ;
        }

        /* receiver init, wait for ack */
        public static Int32 zrec_init(ref byte[] rxbuf,ref zdef.zfile zf)
        {
            UInt32 err_cnt = 0;
	        Int32 res = -1;

	        for (;;) 
	        {
		        zcore.zput_pos(0);
		        zcore.tx_header[zdef.ZF0] = zcore.ZF0_CMD;
		        zcore.tx_header[zdef.ZF1] = zcore.ZF1_CMD;
		        zcore.tx_header[zdef.ZF2] = zcore.ZF2_CMD;
		        zcore.zsend_hex_header(zdef.ZRINIT, zcore.tx_header);
        again:
                res = zcore.zget_header(ref zcore.rx_header);
		        switch(res)
		        {
		        case zdef.ZFILE:						 
			         zcore.ZF0_CMD  = zcore.rx_header[zdef.ZF0];
			         zcore.ZF1_CMD  = zcore.rx_header[zdef.ZF1];
			         zcore.ZF2_CMD  = zcore.rx_header[zdef.ZF2];
			         zcore.ZF3_CMD  = zcore.rx_header[zdef.ZF3];
			         res = zcore.zget_data(ref rxbuf, zdef.RX_BUFFER_SIZE);
			         if (res == zdef.GOTCRCW)
			         {
	                     if ((res = zget_file_info(rxbuf,ref zf))!= 0) 
	                     {
	                         zcore.zsend_hex_header(zdef.ZSKIP, zcore.tx_header);
		                     return (res);
	                     }
			             return 0;; 
			         }     
			         zcore.zsend_hex_header(zdef.ZNAK, zcore.tx_header);
			         goto again;
		        case zdef.ZSINIT:
			         if (zcore.zget_data(ref zcore.Attn, zdef.ZATTNLEN) == zdef.GOTCRCW) /* send zack */
			         {
				        zcore.zsend_hex_header(zdef.ZACK, zcore.tx_header);
				        goto again;
			         }
			         zcore.zsend_hex_header(zdef.ZNAK, zcore.tx_header);		     /* send znak */
			         goto again;
		        case zdef.ZRQINIT:
			         continue;
		        case zdef.ZEOF:
			         continue;
		        case zdef.ZCOMPL:
			         goto again;
		        case zdef.ZFIN:			     /* end file session */
			         zrec_ack_bibi(); 
			         return res;
		         default:
		              if (++err_cnt > 1000) return -1;
		              continue;
		        }
	        }
        }

        /* receive files */
        public static Int32 zrec_files(ref zdef.zfile zf)
        {
	        byte[] rxbuf= new byte[zdef.RX_BUFFER_SIZE*sizeof(byte)];
	        Int32 res = -1;

	        zcore.zinit_parameter();
	        zdef.rt_kprintf("\r\nrz: ready...\r\n"); /* here ready to receive things */
	        if ((res = zrec_init(ref rxbuf,ref zf))!= 0)
	        {
	             zdef.rt_kprintf("\b\b\breceive init failed\r\n");
		         return -1;
	        }
	        res = zrec_file(ref rxbuf,ref zf);
	        if (res == zdef.ZFIN)
	        {	
	            return 0;	     /* if finish session */
	        }
	        else if (res == zdef.ZCAN)
	        {
		        return zdef.ZCAN;        /* cancel by sender */
	        }
	        else
	        {
	           zdevice.zsend_can();
	           return res;
	        }
        }
        /* receive file */
        public static Int32 zrec_file(ref byte[] rxbuf,ref zdef.zfile zf)
        {
	        Int32 res = - 1;
	        UInt16 err_cnt = 0;

	        do 
	        {
		        zcore.zput_pos(zf.bytes_received);
		        zcore.zsend_hex_header(zdef.ZRPOS, zcore.tx_header);
        again:
                res = zcore.zget_header(ref zcore.rx_header);
		        switch (res) 
		        {
		        case zdef.ZDATA:
			         zcore.zget_pos(ref zcore.Rxpos);
			         if (zcore.Rxpos != zf.bytes_received)
			         {
                         zdevice.zsend_break(zcore.Attn);      
				         continue;
			         }
			         err_cnt = 0;
			         res = zrec_file_data(ref rxbuf,ref zf);
			         if (res == -1)
			         {	  
			             zdevice.zsend_break(zcore.Attn);
			             continue;
			         }
			         else if (res == zdef.GOTCAN) return res;	
			         else goto again;	 
		        case zdef.ZRPOS:
		             zcore.zget_pos(ref zcore.Rxpos);
			         continue;
		        case zdef.ZEOF:
		             err_cnt = 0;
		             zcore.zget_pos(ref zcore.Rxpos);
			         if (zcore.Rxpos != zf.bytes_received  || zcore.Rxpos != zf.bytes_total) 
			         {
			             continue;
			         }							 
		             return (zrec_init(ref rxbuf,ref zf));    /* resend zdef.ZRINIT packet,ready to receive next file */
                case zdef.ZFIN:
			         zrec_ack_bibi(); 
			         return zdef.ZCOMPL; 
		        case zdef.ZCAN:
                     if (zdef.ZDEBUG) {
                     zdef.rt_kprintf("error code: sender cancelled \r\n");
                     }
			         zf.bytes_received = 0;		 /* throw the received data */  
		             return res;
		        case zdef.ZSKIP:
			         return res;
		        case -1:             
			         zdevice.zsend_break(zcore.Attn);
			         continue;
		        case zdef.ZNAK:
		        case zdef.TIMEOUT:
		        default: 
			        continue;
		        }
	        } while(++err_cnt < 100);

	        return res;
        }

        /* proccess file infomation */
        public static Int32 zget_file_info (byte[] name,ref zdef.zfile zf)
        {
	        Int32 res  = -1;
            string full_path = "";
            FileInfo fi;
            string data_str = Encoding.Default.GetString(name);
            string[] data_str_array = data_str.Split('\0');
            string[] file_info_array = data_str_array[1].Replace("  ", " ").Split(' ');

	        if (zf.fname == null) 		       /* extract file path  */
            {
                full_path = data_str_array[0];
	        }
	        else
                full_path = zf.fname + data_str_array[0];

            zf.fname = full_path;
            //fi = new FileInfo(full_path);
            ///* check if is a directory */
            //if ((fi.Attributes & FileAttributes.Directory) != 0)
            //{
            //    zdevice.zsend_can();
            //    //zdef.rt_kprintf("\b\b\bcan not open file:%s\r\n",zf.fname+1);
            //    zf.fs.Close();
            //    return res;
            //}
	        /* get fullpath && file attributes */
            zf.bytes_total = Convert.ToUInt32(file_info_array[0], 10);
            zf.ctime = Convert.ToUInt32(file_info_array[1], 8);
            zf.mode = Convert.ToUInt32(file_info_array[2], 8);
            zf.bytes_received = 0;
            if ((zf.fs = File.Open(zf.fname, FileMode.Create)) == null)	 /* create or replace exist file */
            {
                zdevice.zsend_can();
                //zdef.rt_kprintf("\b\b\bcan not create file:%s \r\n",zf.fname);	
                return -1;
            }

	        return 0;
        }

        /* receive file data,continously, no ack */
        public static Int32 zrec_file_data(ref byte[] buf,ref zdef.zfile zf)
        {
            Int32 res = -1;

        more_data:
	        res = zcore.zget_data(ref buf,zdef.RX_BUFFER_SIZE);
	        switch(res)
	        {
	        case zdef.GOTCRCW:						   /* zack received */
		         zwrite_file(buf,zcore.Rxcount,zf);
		         zf.bytes_received += zcore.Rxcount;
		         zcore.zput_pos(zf.bytes_received);
		         zdevice.zsend_line(zdef.XON);
		         zcore.zsend_hex_header(zdef.ZACK, zcore.tx_header);
		         return 0;
	        case zdef.GOTCRCQ:
		         zwrite_file(buf,zcore.Rxcount,zf);
		         zf.bytes_received += zcore.Rxcount;
		         zcore.zput_pos(zf.bytes_received);
		         zcore.zsend_hex_header(zdef.ZACK, zcore.tx_header);
		         goto more_data;
	        case zdef.GOTCRCG:
		         zwrite_file(buf,zcore.Rxcount,zf);
		         zf.bytes_received += zcore.Rxcount;
		         goto more_data;
	        case zdef.GOTCRCE:
		         zwrite_file(buf,zcore.Rxcount,zf);
		         zf.bytes_received += zcore.Rxcount;
		         return 0;
	        case zdef.GOTCAN:
                 if (zdef.ZDEBUG) {
	                zdef.rt_kprintf("error code : zdef.ZCAN \r\n");
                 }
		         return res;
	        case zdef.TIMEOUT:
	             return res;
            case -1:
	             zdevice.zsend_break(zcore.Attn);
	             return res;
	        default:
	             return res;
	        }
        }

        /* write file */
        public static Int32 zwrite_file(byte[] buf,UInt16 size, zdef.zfile zf)
        {
            zf.fs.Write(buf, 0, size);

	        return size;
        }

        /* ack bibi */
        public static void zrec_ack_bibi()
        {
	        byte i;

	        zcore.zput_pos(0);
	        for (i=0;i<3;i++) 
	        {
		        zcore.zsend_hex_header(zdef.ZFIN, zcore.tx_header);
		        switch (zdevice.zread_line(100)) 
		        {
		        case (Int16)'O':
			         zdevice.zread_line(1);	
			         return;
		        case zdef.RCDO:
			         return;
		        case zdef.TIMEOUT:
		        default:
			         break;
		        }
	        }
        }
    }
}
