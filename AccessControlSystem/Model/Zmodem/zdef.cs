using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AccessControlSystem.Model.Zmodem
{
    public class zdef
    {
        public static bool ZDEBUG = false;

        public const byte ZPAD = (byte)'*';	        /* 052 padding character begins frames */
        public const byte ZDLE = (byte)0x18;	        /* ctrl-X ZMODEM escape - `ala BISYNC DLE */
        public const byte ZDLEE = (byte)(ZDLE ^ 0x40);	/* 0x58 escaped ZDLE as transmitted */
        public const byte ZBIN = (byte)'A';	        /* binary frame indicator (CRC-0x10) */
        public const byte ZHEX = (byte)'B';	        /* hex frame indicator */
        public const byte ZBIN32 = (byte)'C';	    /* binary frame with 32 bit FCS */
        public const byte ZBINR32 = (byte)'D';	    /* RLE packed Binary frame with 32 bit FCS */
        public const byte ZVBIN = (byte)'a';	        /* binary frame indicator (CRC-0x10) */
        public const byte ZVHEX = (byte)'b';	        /* hex frame indicator */
        public const byte ZVBIN32 = (byte)'c';	    /* binary frame with 32 bit FCS */
        public const byte ZVBINR32 = (byte)'d';	    /* RLE packed Binary frame with 32 bit FCS */
        public const byte ZRESC = (byte)0x7E;	        /* RLE flag/escape character */

        /* Frame types */
        public const byte ZRQINIT = 0;     /* request receive init */
        public const byte ZRINIT = 1;      /* receive init */
        public const byte ZSINIT = 2;      /* send init sequence (optional) */
        public const byte ZACK = 3;        /* ACK to above */
        public const byte ZFILE = 4;       /* file name from sender */
        public const byte ZSKIP = 5;       /* ro sender: skip this file */
        public const byte ZNAK = 6;        /* last packet was garbled */
        public const byte ZABORT = 7;      /* abort batch transfers */
        public const byte ZFIN = 8;        /* finish session */
        public const byte ZRPOS = 9;       /* resume data trans at this position */
        public const byte ZDATA = 0x0A;      /* data packet(s) follow */
        public const byte ZEOF = 0x0B;       /* end of file */
        public const byte ZFERR = 0x0C;      /* fatal Read or Write error Detected */
        public const byte ZCRC = 0x0D;       /* request for file CRC and response */
        public const byte ZCHALLENGE = 0x0E; /* receiver's Challenge */
        public const byte ZCOMPL = 0x0F;     /* request is complete */
        public const byte ZCAN = 0x10;       /* other end canned session with CAN*5 */
        public const byte ZFREECNT = 17;   /* request for free bytes on filesystem */
        public const byte ZCOMMAND = 18;   /* command from sending program */

        /* ZDLE sequfences */
        public const byte ZCRCE = (byte)'h'; /* CRC next, frame ends, header packet follows */
        public const byte ZCRCG = (byte)'i'; /* CRC next, frame continues nonstop */
        public const byte ZCRCQ = (byte)'j'; /* CRC next, frame continues, ZACK expected */
        public const byte ZCRCW = (byte)'k'; /* CRC next, ZACK expected, end of frame */
        public const byte ZRUB0 = (byte)'l'; /* translate to rubout 0177 */
        public const byte ZRUB1 = (byte)'m'; /* translate to rubout 0377 */

        /* zdlread return values (internal) */
        /* -1 is general error, -2 is timeout */
        public const UInt16 GOTOR = 256;
        public const UInt16 GOTCRCE = (UInt16)(ZCRCE | GOTOR); /* ZDLE-ZCRCE received */
        public const UInt16 GOTCRCG = (UInt16)(ZCRCG | GOTOR); /* ZDLE-ZCRCG received */
        public const UInt16 GOTCRCQ = (UInt16)(ZCRCQ | GOTOR); /* ZDLE-ZCRCQ received */
        public const UInt16 GOTCRCW = (UInt16)(ZCRCW | GOTOR); /* ZDLE-ZCRCW received */
        public const UInt16 GOTCAN = (UInt16)(GOTOR | 0x18);     /* CAN*5 seen */

        /* Byte positions within header array */
        public const byte ZF0 = 3; /* first flags byte */
        public const byte ZF1 = 2;
        public const byte ZF2 = 1;
        public const byte ZF3 = 0;
        public const byte ZP0 = 0; /* low order 8 bits of position */
        public const byte ZP1 = 1;
        public const byte ZP2 = 2;
        public const byte ZP3 = 3; /* high order 8 bits of file position */

        /* parameters for ZRINIT header */
        public const byte ZRPXWN = 8;	    /* 9th byte in header contains window size/256 */
        public const byte ZRPXQQ = 9;	    /* 10th to 14th bytes contain quote mask */
        /* bit Masks for ZRINIT flags byte ZF0 */
        public const byte CANFDX = 0x01;	 /* rx can send and receive true FDX */
        public const byte CANOVIO = 0x02; /* rx can receive data during disk I/O */
        public const byte CANBRK = 0x04;	 /* rx can send a break signal */
        public const byte CANRLE = 0x10;	 /* receiver can decode RLE */
        public const byte CANLZW = 0x20;	 /* receiver can uncompress */
        public const byte CANFC32 = 0x28; /* receiver can use 32 bit Frame Check */
        public const byte ESCCTL = 0x64;	 /* receiver expects ctl chars to be escaped */
        public const byte ESC8 = 0xc8;	 /* receiver expects 8th bit to be escaped */

        /* bit Masks for ZRINIT flags byte ZF1 */
        public const byte CANVHDR = 1;	/* variable headers OK */
        public const byte ZRRQWN = 8;	/* receiver specified window size in ZRPXWN */
        public const byte ZRRQQQ = 0x10;	/* additional control chars to quote in ZRPXQQ	*/
        public const byte ZRQNVH = (byte)(ZRRQWN | ZRRQQQ);	/* variable len hdr reqd to access info */

        /* Parameters for ZSINIT frame */
        public const byte ZATTNLEN = 32;	/* max length of attention string */
        public const byte ALTCOFF = ZF1;	/* offset to alternate canit string, 0 if not used */

        /* Parameters for ZFILE frame */
        /* Conversion options one of these in ZF0 */
        public const byte ZCBIN = 1;	   /* binary transfer - inhibit conversion */
        public const byte ZCNL = 2;	   /* convert NL to local end of line convention */
        public const byte ZCRESUM = 3;  /* resume interrupted file transfer */
        /* management include options, one of these ored in ZF1 */
        public const byte ZMSKNOLOC = 0x80; /* skip file if not present at rx */
        /* management options, one of these ored in ZF1 */
        public const byte ZMMASK = 0x1F;  /* mask for the choices below */
        public const byte ZMNEWL = 1;	   /* transfer if source newer or longer */
        public const byte ZMCRC = 2;	   /* transfer if different file CRC or length */
        public const byte ZMAPND = 3;	   /* append contents to existing file (if any) */
        public const byte ZMCLOB = 4;	   /* replace existing file */
        public const byte ZMNEW = 5;	   /* transfer if source newer */
        /* number 5 is alive ... */
        public const byte ZMDIFF = 6;	   /* transfer if dates or lengths different */
        public const byte ZMPROT = 7;	   /* protect destination file */
        public const byte ZMCHNG = 8;	   /* change filename if destination exists */
        /* transport options, one of these in ZF2 */
        public const byte ZTLZW = 1;	   /* lempel-Ziv compression */
        public const byte ZTRLE = 3;	   /* run Length encoding */
        /* extended options for ZF3, bit encoded */
        public const byte ZXSPARS = 0x40;   /* encoding for sparse file operations */
        public const byte ZCANVHDR = 1;   /* variable headers OK */
        /* receiver window size override */
        public const byte ZRWOVR = 4;	   /* byte position for receive window override/256 */

        /* parameters for ZCOMMAND frame ZF0 (otherwise 0) */
        public const byte ZCACK1 = 1;	   /* acknowledge, then do command */

        /* ward Christensen / CP/M parameters - Don't change these! */
        public const byte ENQ = 5;
        public const byte CAN = ('X' & 0x1F);  /* 0x18 */
        public const byte XOFF = ('s' & 0x1F); /* 0x13 */
        public const byte XON = ('q' & 0x1F);  /* 0x11 */
        public const byte SOH = 1;
        public const byte STX = 2;
        public const byte ETX = 3;
        public const byte SYN = 0x16;
        public const byte ESC = 0x1B;
        public const byte WANTG = 0x47;	/* send G not NAK to get nonstop batch xmsn */
        public const byte EOT = 4;
        public const byte ACK = 6;
        public const byte NAK = 0x15;
        public const byte CPMEOF = 0x1A;
        public const byte WANTCRC = 0x43;	/* send C not NAK to get crc not checksum */
        public const sbyte TIMEOUT = (-2);
        public const sbyte RCDO = (-3);
        public const sbyte GCOUNT = (-4);
        public const byte ERRORMAX = 5;
        public const byte RETRYMAX = 5;
        public const sbyte WCEOT = (-10);

        public static UInt32 BITRATE = 115200;
        public static UInt16 TX_BUFFER_SIZE = 1024;
        public static UInt16 RX_BUFFER_SIZE = 1024;	  /* sender or receiver's max buffer size */

        public struct zfile
        {
            public string fname;
            public FileStream fs;
            public UInt32 ctime;
            public UInt32 mode;
            public UInt32 bytes_total;
            public UInt32 bytes_sent;
            public UInt32 bytes_received;
            public UInt32 file_end;
        };


        public static void rt_kprintf(string str)
        {
            MessageBox.Show(str);
        }
    }
}
