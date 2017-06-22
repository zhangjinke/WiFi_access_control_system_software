using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessControlSystem.Model.Zmodem
{
    class zcore
    {
        public static byte ZF0_CMD;		          /* file conversion request */
        public static byte ZF1_CMD;	 	          /* file management request */
        public static byte ZF2_CMD;		          /* file transport request */
        public static byte ZF3_CMD;
        public static byte Rxframeind;		      /* zdef.ZBIN zdef.ZBIN32, or zdef.ZHEX type of frame */
        public static UInt16 Rxcount;		          /* received count*/
        public static byte header_type;	          /* header type */
        public static byte[] rx_header = new byte[4]; /* received header */
        public static byte[] tx_header = new byte[4]; /* transmitted header */
        public static UInt32 Rxpos;		              /* received file position */
        public static UInt32 Txpos;		              /* transmitted file position */
        public static byte Txfcs32;		          /* TURE means send binary frames with 32 bit FCS */
        public static byte TxCRC;		              /* controls 32 bit CRC being sent */
        public static byte RxCRC;		              /* indicates/controls 32 bit CRC being received */
        /* 0 == CRC16,  1 == CRC32,  2 == CRC32 + RLE */
        public static byte ZATTNLEN = 32;	          /* max length of attention string */
        public static byte[] Attn = new byte[ZATTNLEN + 1];  /* attention string rx sends to tx on err */

        public static void zinit_parameter()
        {
            byte i;

            /* not chose CANFC32,CANRLE,although it have been supported */
            ZF0_CMD = (byte)(zdef.CANFC32 | zdef.CANFDX | zdef.CANOVIO);
            ZF1_CMD = 0; /* fix header length,not support CANVHDR */
            ZF2_CMD = 0;
            ZF3_CMD = 0;
            Rxframeind = 0;
            header_type = 0;
            Rxcount = 0;
            for (i = 0; i < 4; i++) rx_header[i] = tx_header[i] = 0;
            Rxpos = Txpos = 0;
            RxCRC = 0;
            Txfcs32 = 0;

            return;
        }

        /* send binary header */
        public static void zsend_bin_header(byte type, byte[] hdr)
        {
            byte i;
            UInt32 crc;

            zdevice.zsend_byte(zdef.ZPAD);
            zdevice.zsend_byte(zdef.ZDLE);
            TxCRC = Txfcs32;
            if (TxCRC == 0)
            {
                zdevice.zsend_byte(zdef.ZBIN);
                zsend_zdle_char(type);
                /* add 16bits crc */
                crc = 0;
                crc = zcrc.updcrc16(type, 0);
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char(hdr[i]);
                    crc = zcrc.updcrc16((UInt16)(0xFF & hdr[i]), (UInt16)crc);
                }
                crc = zcrc.updcrc16(0, zcrc.updcrc16((UInt16)0, (UInt16)crc));
                zsend_zdle_char(((UInt16)(crc >> 8)));
                zsend_zdle_char((UInt16)crc);
            }
            else if (TxCRC == 1)
            {
                zdevice.zsend_byte(zdef.ZBIN32);
                zsend_zdle_char(type);
                /* add 32bits crc */
                crc = 0xffffffff;
                crc = zcrc.updcrc32(type, crc);
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char(hdr[i]);
                    crc = zcrc.updcrc32((UInt32)(0xFF & hdr[i]), crc);
                }
                crc = ~crc;
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char((UInt16)crc);
                    crc >>= 8;
                }
            }
            else if (TxCRC == 2)
            {
                zdevice.zsend_byte(zdef.ZBINR32);
                zsend_zdle_char(type);
                /* add 32bits crc */
                crc = 0xffffffff;
                crc = zcrc.updcrc32(type, crc);
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char(hdr[i]);
                    crc = zcrc.updcrc32((UInt32)(0xFF & hdr[i]), crc);
                }
                crc = ~crc;
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char((UInt16)crc);
                    crc >>= 8;
                }
            }

            return;
        }

        /* send hex header */
        public static void zsend_hex_header(byte type, byte[] hdr)
        {
            byte i;
            UInt16 crc;

            zdevice.zsend_line(zdef.ZPAD); 
            zdevice.zsend_line(zdef.ZPAD); 
            zdevice.zsend_line(zdef.ZDLE);
            zdevice.zsend_line(zdef.ZHEX);
            zsend_ascii(type);
            crc = zcrc.updcrc16(type, 0);
            for (i = 0; i < 4; i++)
            {
                zsend_ascii(hdr[i]);
                crc = zcrc.updcrc16((UInt16)(0xFF & hdr[i]), crc);
            }
            crc = zcrc.updcrc16(0, zcrc.updcrc16(0, crc));
            zsend_ascii((byte)(crc >> 8));
            zsend_ascii((byte)crc);
            /* send display control cmd */
            zdevice.zsend_line(0x0D); zdevice.zsend_line(0x8A);
            if (type != zdef.ZFIN && type != zdef.ZACK)
                zdevice.zsend_line(0x11);
            TxCRC = 0;               /* clear tx crc type */

            return;
        }

        /* send binary data,with frameend */
        public static void zsend_bin_data(byte[] buf, Int16 len, byte frameend)
        {
            Int16 i, c, tmp;
            UInt32 crc;

            if (TxCRC == 0)         /* send binary data with 16bits crc check */
            {
                crc = 0x0;
                for (i = 0; i < len; i++)
                {
                    zsend_zdle_char(buf[i]);
                    crc = zcrc.updcrc16((UInt16)(0xFF & buf[i]), (UInt16)crc);
                }
                zdevice.zsend_byte(zdef.ZDLE); zdevice.zsend_byte(frameend);
                crc = zcrc.updcrc16((UInt16)frameend, (UInt16)crc);
                crc = zcrc.updcrc16(0, zcrc.updcrc16(0, (UInt16)crc));
                zsend_zdle_char((UInt16)(crc >> 8));
                zsend_zdle_char((UInt16)crc);
            }
            else if (TxCRC == 1)   /* send binary data with 32 bits crc check */
            {
                crc = 0xffffffff;
                for (i = 0; i < len; i++)
                {
                    c = (Int16)(buf[i] & 0xFF);
                    zsend_zdle_char((UInt16)c);
                    crc = zcrc.updcrc32((UInt32)c, crc);
                }
                zdevice.zsend_byte(zdef.ZDLE); zdevice.zsend_byte(frameend);
                crc = zcrc.updcrc32(frameend, crc);
                crc = ~crc;
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char((UInt16)crc); crc >>= 8;
                }
            }
            else if (TxCRC == 2)   /* send binary data with 32bits crc check,RLE encode */
            {
                crc = 0xffffffff;
                tmp = (Int16)(buf[0] & 0xFF);
                for (i = 0; --len >= 0; )
                {
                    if ((c = (Int16)(buf[i + 1] & 0xFF)) == tmp && i < 126 && len > 0)
                    {
                        ++i; continue;
                    }
                    if (i == 0)
                    {
                        zsend_zdle_char((UInt16)tmp);
                        crc = zcrc.updcrc32((UInt32)tmp, crc);
                        if (tmp == zdef.ZRESC)
                        {
                            zsend_zdle_char(0x40); crc = zcrc.updcrc32(0x40, crc);
                        }
                        tmp = c;
                    }
                    else if (i == 1)
                    {
                        if (tmp != zdef.ZRESC)
                        {
                            zsend_zdle_char((UInt16)tmp); zsend_zdle_char((UInt16)tmp);
                            crc = zcrc.updcrc32((UInt32)tmp, crc);
                            crc = zcrc.updcrc32((UInt32)tmp, crc);
                            i = 0; tmp = c;
                        }

                    }
                    else
                    {
                        zsend_zdle_char(zdef.ZRESC); crc = zcrc.updcrc32(zdef.ZRESC, crc);
                        if (tmp == 0x20 && i < 34)
                        {
                            i += 0x1E;
                            zsend_zdle_char((UInt16)i);
                            crc = zcrc.updcrc32((UInt32)i, crc);
                        }
                        else
                        {
                            i += 0x41;
                            zsend_zdle_char((UInt16)i); crc = zcrc.updcrc32((UInt32)i, crc);
                            zsend_zdle_char((UInt16)tmp); crc = zcrc.updcrc32((UInt32)tmp, crc);
                        }
                        i = 0; tmp = c;
                    }
                }
                zdevice.zsend_byte(zdef.ZDLE); zdevice.zsend_byte(frameend);
                crc = zcrc.updcrc32(frameend, crc);
                crc = ~crc;
                for (i = 0; i < 4; i++)
                {
                    zsend_zdle_char((UInt16)crc);
                    crc >>= 8;
                }
            }
            if (frameend == zdef.ZCRCW)
                zdevice.zsend_byte(zdef.XON);

            return;
        }

        /* receive data,with 16bits CRC check */
        public static Int16 zrec_data16(ref byte[] buf, UInt16 len)
        {
            Int16 c = 0;
            Int16 crc_cnt;
            UInt16 crc;
            Int32 res = -1;
            byte flag = 0;
            UInt32 idx = 0;

            crc_cnt = 0; crc = 0;
            Rxcount = 0;
            while (idx < len)
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                {
                    if (res == zdef.GOTCRCE || res == zdef.GOTCRCG ||
                        res == zdef.GOTCRCQ || res == zdef.GOTCRCW)
                    {
                        c = (Int16)res;
                        c = (Int16)res;
                        crc = zcrc.updcrc16((UInt16)(res & 0xFF), (UInt16)crc);
                        flag = 1;
                        continue;
                    }
                    else if (res == zdef.GOTCAN) return zdef.ZCAN;
                    else if (res == zdef.TIMEOUT) return zdef.TIMEOUT;
                    else return (Int16)res;

                }
                else
                {
                    if (0 != flag)
                    {
                        crc = zcrc.updcrc16((UInt16)res, crc);
                        crc_cnt++;
                        if (crc_cnt < 2) continue;
                        if (0 != (crc & 0xffff))
                        {
                            if (zdef.ZDEBUG)
                            {
                                zdef.rt_kprintf("error code: CRC16 error \r\n");
                            }
                            return -1;
                        }
                        return c;
                    }
                    else
                    {
                        buf[idx++] = (byte)res;
                        Rxcount++;
                        crc = zcrc.updcrc16((UInt16)res, crc);
                    }
                }
            }

            return -1;
        }

        /* receive data,with 32bits CRC check */
        public static Int16 zrec_data32(ref byte[] buf, Int16 len)
        {
            Int16 c = 0;
            Int16 crc_cnt;
            UInt32 crc;
            Int32 res = -1;
            byte flag = 0;
            UInt32 idx = 0;

            crc_cnt = 0; crc = 0xffffffff;
            Rxcount = 0;
            while (idx <= len)
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                {
                    if (res == zdef.GOTCRCE || res == zdef.GOTCRCG ||
                        res == zdef.GOTCRCQ || res == zdef.GOTCRCW)
                    {
                        c = (Int16)res;
                        crc = zcrc.updcrc32((UInt32)(res & 0xFF), crc);
                        flag = 1;
                        continue;
                    }
                    else if (res == zdef.GOTCAN) return zdef.ZCAN;
                    else if (res == zdef.TIMEOUT) return zdef.TIMEOUT;
                    else return (Int16)res;

                }
                else
                {
                    if (0 != flag)
                    {
                        crc = zcrc.updcrc32((UInt32)res, crc);
                        crc_cnt++;
                        if (crc_cnt < 4) continue;
                        if (0 != (crc & 0xDEBB20E3))
                        {
                            if (zdef.ZDEBUG)
                            {
                                zdef.rt_kprintf("error code: CRC32 error \r\n");
                            }
                            return -1;
                        }
                        return c;
                    }
                    else
                    {
                        buf[idx++] = (byte)res;
                        Rxcount++;
                        crc = zcrc.updcrc32((UInt32)res, crc);
                    }
                }
            }

            return -1;
        }
        /* receive data,with RLE encoded,32bits CRC check */
        public static Int16 zrec_data32r(ref byte[] buf, Int16 len)
        {
            Int16 c = 0;
            Int16 crc_cnt;
            UInt32 crc;
            Int32 res = -1;
            byte flag = 0;
            UInt32 idx = 0;

            crc_cnt = 0; crc = 0xffffffff;
            Rxcount = 0;
            while (idx <= len)
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                {
                    if (res == zdef.GOTCRCE || res == zdef.GOTCRCG ||
                        res == zdef.GOTCRCQ || res == zdef.GOTCRCW)
                    {
                        c = (Int16)res;
                        crc = zcrc.updcrc32((UInt32)(res & 0xFF), crc);
                        flag = 1;
                        continue;
                    }
                    else if (res == zdef.GOTCAN) return zdef.ZCAN;
                    else if (res == zdef.TIMEOUT) return zdef.TIMEOUT;
                    else return (Int16)res;

                }
                else
                {
                    if (0 != flag)
                    {
                        crc = zcrc.updcrc32((UInt32)res, crc);
                        crc_cnt++;
                        if (crc_cnt < 4) continue;
                        if (0 != (crc & 0xDEBB20E3))
                        {
                            if (zdef.ZDEBUG)
                            {
                                zdef.rt_kprintf("error code: CRC32 error \r\n");
                            }
                            return -1;
                        }
                        return c;
                    }
                    else
                    {
                        crc = zcrc.updcrc32((UInt32)res, crc);
                        switch (c)
                        {
                            case 0:
                                if (res == zdef.ZRESC)
                                {
                                    c = -1; continue;
                                }
                                buf[idx++] = (byte)res;
                                Rxcount++;
                                continue;
                            case -1:
                                if (res >= 0x20 && res < 0x40)
                                {
                                    c = (Int16)(res - 0x1D); res = 0x20;
                                    goto spaces;
                                }
                                if (res == 0x40)
                                {
                                    c = 0;
                                    buf[idx++] = zdef.ZRESC;
                                    Rxcount++;
                                    continue;
                                }
                                c = (Int16)res; continue;
                            default:
                                c -= 0x40;
                                if (c < 1)
                                    goto end;
                            spaces:
                                if ((idx + c) > len)
                                    goto end;
                                while (--res >= 0)
                                {
                                    buf[idx++] = (byte)res;
                                    Rxcount++;
                                }
                                c = 0; continue;
                        }
                    }
                }	// if -else

            }
        end:
            return -1;
        }
        public static Int16 zget_data(ref byte[] buf, UInt16 len)
        {
            Int16 res = -1;

            if (RxCRC == 0)
            {
                res = zrec_data16(ref buf, len);
            }
            else if (RxCRC == 1)
            {
                res = zrec_data32(ref buf, (Int16)len);
            }
            else if (RxCRC == 2)
            {
                res = zrec_data32r(ref buf, (Int16)len);
            }

            return res;
        }
        /* get type and cmd of header, fix lenght */
        public static Int16 zget_header(ref byte[] hdr)
        {
            Int16 c, prev_char;
            UInt32 bit;
            UInt16 get_can = 0;
            UInt16 step_out;
            bool is_get_can = false;
            bool is_start_again = false;

            bit = zdevice.get_device_baud(); /* get console baud rate */
            Rxframeind = header_type = 0;
            step_out = 0;
            prev_char = 0xff;
        start:
            for (; ; )
            {
                if (is_get_can)
                {
                    is_get_can = false;
                    c = zdef.CAN;
                    prev_char = zdef.ZCAN;
                }
                else if (is_start_again)
                {
                    is_start_again = false;
                    c = 0xFF;
                    prev_char = zdef.ZPAD;
                }
                else
                {
                    c = zdevice.zread_line(100);
                }
                switch (c)
                {
                    case 0x11:
                    case 0x91:
                        if (prev_char == zdef.CAN) break;
                        if (prev_char == zdef.ZCRCW) goto start_again;
                        break;
                    case zdef.RCDO:
                        goto end;
                    case zdef.TIMEOUT:
                        if (prev_char == zdef.CAN) break;
                        if (prev_char == zdef.ZCRCW)
                        {
                            c = -1; goto end;
                        }
                        goto end;
                    case zdef.ZCRCW:
                        if (prev_char == zdef.CAN) goto start_again;
                        break;
                    case zdef.CAN:
                        if (++get_can > 5)
                        {
                            c = zdef.ZCAN; goto end;
                        }
                        break;
                    case zdef.ZPAD:
                        if (prev_char == zdef.CAN) break;
                        if (prev_char == zdef.ZCRCW) goto start_again;
                        step_out = 1;
                        break;
                    default:
                        if (prev_char == zdef.CAN) break;
                        if (prev_char == zdef.ZCRCW) goto start_again;
                    start_again:
                        if (--bit == 0)
                        {
                            c = zdef.GCOUNT; goto end;
                        }
                        get_can = 0;
                        break;
                }
                prev_char = c;
                if (0 != step_out) break;    /* exit loop */
            }
            step_out = get_can = 0;
            for (; ; )
            {
                c = zxor_read();
                switch (c)
                {
                    case zdef.ZPAD:
                        break;
                    case zdef.RCDO:
                    case zdef.TIMEOUT:
                        goto end;
                    case zdef.ZDLE:
                        step_out = 1;
                        break;
                    default:
                        is_start_again = true;
                        break;
                }
                if (is_start_again) break;
                if (0 != step_out) break;
            }
            if (is_start_again)
            {
                goto start;
            }
            c = zxor_read();
            Rxframeind = (byte)c;
            switch (c)
            {
                case zdef.ZBIN32:
                    RxCRC = 1; c = zget_bin_fcs(ref hdr); break;
                case zdef.ZBINR32:
                    RxCRC = 2; c = zget_bin_fcs(ref hdr); break;
                case zdef.ZBIN:
                    RxCRC = 0; c = zget_bin_header(ref hdr); break;
                case zdef.ZHEX:
                    RxCRC = 0; c = zget_hex_header(ref hdr); break;
                case zdef.CAN:
                    is_get_can = true; break;
                case zdef.RCDO:
                case zdef.TIMEOUT:
                    goto end;
                default:
                    is_start_again = true; break;
            }
            if ((is_get_can) || (is_start_again))
            {
                goto start;
            }
        end:
            return c;
        }

        /* receive a binary header */
        public static Int16 zget_bin_header(ref byte[] hdr)
        {
            Int16 res, i;
            UInt16 crc;

            if (0 != ((res = zread_byte()) & ~0xFF))
                return res;
            header_type = (byte)res;
            crc = zcrc.updcrc16((UInt16)res, 0);

            for (i = 0; i < 4; i++)
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                    return res;
                crc = zcrc.updcrc16((UInt16)res, crc);
                hdr[i] = (byte)res;
            }
            if (0 != ((res = zread_byte()) & ~0xFF))
                return res;
            crc = zcrc.updcrc16((UInt16)res, crc);
            if (0 != ((res = zread_byte()) & ~0xFF))
                return res;
            crc = zcrc.updcrc16((UInt16)res, crc);
            if (0 != (crc & 0xFFFF))
            {
                zdef.rt_kprintf("CRC error\n");
                return -1;
            }

            return header_type;
        }

        /* receive a binary header,with 32bits FCS */
        public static Int16 zget_bin_fcs(ref byte[] hdr)
        {
            Int16 res, i;
            UInt32 crc;

            if (0 != ((res = zread_byte()) & ~0xFF))
                return res;
            header_type = (byte)res;
            crc = 0xFFFFFFFF;
            crc = zcrc.updcrc32((UInt32)res, crc);

            for (i = 0; i < 4; i++)    /* 4headers */
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                    return res;
                crc = zcrc.updcrc32((UInt32)res, crc);
                hdr[i] = (byte)res;

            }
            for (i = 0; i < 4; i++) 	/* 4bytes crc */
            {
                if (0 != ((res = zread_byte()) & ~0xFF))
                    return res;
                crc = zcrc.updcrc32((UInt32)res, crc);

            }
            if (crc != 0xDEBB20E3)
            {
                if (zdef.ZDEBUG)
                {
                    zdef.rt_kprintf("CRC error\n");
                }
                return -1;
            }

            return header_type;
        }


        /* receive a hex style header (type and position) */
        public static Int16 zget_hex_header(ref byte[] hdr)
        {
            Int16 res, i;
            UInt16 crc;

            if ((res = zget_hex()) < 0)
                return res;
            header_type = (byte)res;
            crc = zcrc.updcrc16((UInt16)res, 0);

            for (i = 0; i < 4; i++)
            {
                if ((res = zget_hex()) < 0)
                    return res;
                crc = zcrc.updcrc16((UInt16)res, crc);
                hdr[i] = (byte)res;
            }
            if ((res = zget_hex()) < 0)
                return res;
            crc = zcrc.updcrc16((UInt16)res, crc);
            if ((res = zget_hex()) < 0)
                return res;
            crc = zcrc.updcrc16((UInt16)res, crc);
            if (0 != (crc & 0xFFFF))
            {
                if (zdef.ZDEBUG)
                {
                    zdef.rt_kprintf("error code : CRC error\r\n");
                }
                return -1;
            }
            res = zdevice.zread_line(100);
            if (res < 0)
                return res;
            res = zdevice.zread_line(100);
            if (res < 0)
                return res;

            return header_type;
        }

        /* convert to ascii */
        public static void zsend_ascii(byte c)
        {
            byte[] hex = Encoding.Default.GetBytes("0123456789abcdef");

            zdevice.zsend_line(hex[(c & 0xF0) >> 4]);
            zdevice.zsend_line(hex[(c) & 0x0F]);

            return;
        }

        /*
         * aend character c with ZMODEM escape sequence encoding.
         */
        public static void zsend_zdle_char(UInt16 ch)
        {
            UInt16 res;

            res = (UInt16)(ch & 0xFF);
            switch (res)
            {
                case 0xFF:
                    zdevice.zsend_byte(res);
                    break;
                case zdef.ZDLE:
                    zdevice.zsend_byte(zdef.ZDLE);
                    res ^= 0x40;
                    zdevice.zsend_byte(res);
                    break;
                case 0x11:
                case 0x13:
                case 0x91:
                case 0x93:
                    zdevice.zsend_byte(zdef.ZDLE);
                    res ^= 0x40;
                    zdevice.zsend_byte(res);
                    break;
                default:
                    zdevice.zsend_byte(res);
                    break;
            }
        }

        /* decode two lower case hex digits into an 8 bit byte value */
        public static Int16 zget_hex()
        {
            Int16 res, n;

            if ((res = zxor_read()) < 0)
                return res;
            n = (Int16)(res - '0');
            if (n > 9)
                n -= ('a' - ':');
            if (0 != (n & ~0x0f))
                return -1;
            if ((res = zxor_read()) < 0)
                return res;
            res -= (Int16)'0';
            if (res > 9)
                res -= ('a' - ':');
            if (0 != (res & ~0x0f))
                return -1;
            res += (Int16)(n << 4);

            return res;
        }


        /*
         * read a byte, checking for ZMODEM escape encoding
         *  including zdef.CAN*5 which represents a quick abort
         */
        public static Int16 zread_byte()
        {
            int res;

        again:
            /* Quick check for non control characters */
            if (0 != ((res = zdevice.zread_line(100)) & 0x60))
                return (Int16)res;
            switch (res)
            {
                case zdef.ZDLE:
                    break;
                case 0x13:
                case 0x93:
                case 0x11:
                case 0x91:
                    goto again;
                default:
                    return (Int16)res;
            }
        again2:
            if ((res = zdevice.zread_line(100)) < 0)
                return (Int16)res;
            if (res == zdef.CAN && (res = zdevice.zread_line(100)) < 0)
                return (Int16)res;
            if (res == zdef.CAN && (res = zdevice.zread_line(100)) < 0)
                return (Int16)res;
            if (res == zdef.CAN && (res = zdevice.zread_line(100)) < 0)
                return (Int16)res;
            switch (res)
            {
                case zdef.CAN:
                    return (Int16)zdef.GOTCAN;
                case zdef.ZCRCE:
                case zdef.ZCRCG:
                case zdef.ZCRCQ:
                case zdef.ZCRCW:
                    return (Int16)(res | zdef.GOTOR);
                case zdef.ZRUB0:
                    return 0x7F;
                case zdef.ZRUB1:
                    return 0xFF;
                case 0x13:
                case 0x93:
                case 0x11:
                case 0x91:
                    goto again2;
                default:
                    if ((res & 0x60) == 0x40)
                        return (Int16)(res ^ 0x40);
                    break;
            }

            return -1;
        }

        /*
         * @read a character from the modem line with timeout.
         * @eat parity, zdef.XON and zdef.XOFF characters.
         */
        public static Int16 zxor_read()
        {
            Int16 res;

            for (; ; )
            {
                if ((res = zdevice.zread_line(100)) < 0)
                    return res;
                switch ((byte)(res &= 0x7F))
                {
                    case zdef.XON:
                    case zdef.XOFF:
                        continue;
                    case (byte)'\r':
                    case (byte)'\n':
                    case zdef.ZDLE:
                    default:
                        return res;
                }
            }
        }

        /* put file posistion into the header*/
        public static void zput_pos(UInt32 pos)
        {
            tx_header[zdef.ZP0] = (byte)(pos);
            tx_header[zdef.ZP1] = (byte)(pos >> 8);
            tx_header[zdef.ZP2] = (byte)(pos >> 16);
            tx_header[zdef.ZP3] = (byte)(pos >> 24);

            return;
        }

        /* Recover a long integer from a header */
        public static void zget_pos(ref UInt32 pos)
        {
            Rxpos = (UInt32)(rx_header[zdef.ZP3] & 0xFF);
            Rxpos = (UInt32)((Rxpos << 8) | (rx_header[zdef.ZP2] & 0xFF));
            Rxpos = (UInt32)((Rxpos << 8) | (rx_header[zdef.ZP1] & 0xFF));
            Rxpos = (UInt32)((Rxpos << 8) | (rx_header[zdef.ZP0] & 0xFF));

            return;
        }

    }
}
