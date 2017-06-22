using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessControlSystem.Model
{
    class Mesh
    {
        const int MESH_VER = 0;          /* mesh版本 */
        const int ESP_MESH_ADDR_LEN = 6; /* mesh地址长度 */
        const int MESH_HEADER_SIZE = 16; /* mesh头字节数 */
        public enum mesh_option_type
        {
            M_O_CONGEST_REQ = 0,        // congest request option
            M_O_CONGEST_RESP,           // congest response option
            M_O_ROUTER_SPREAD,          // router information spread option
            M_O_ROUTE_ADD,              // route table update (node joins mesh) option
            M_O_ROUTE_DEL,              // route table update (node leaves mesh) option
            M_O_TOPO_REQ,               // topology request option
            M_O_TOPO_RESP,              // topology response option
            M_O_MCAST_GRP,              // group list of mcast
            M_O_MESH_FRAG,              // mesh management fragment option
            M_O_USR_FRAG,               // user data fragment
            M_O_USR_OPTION,             // user option
        }

        public enum mesh_usr_proto_type
        {
            M_PROTO_NONE = 0,           // used to delivery mesh management packet
            M_PROTO_HTTP,               // user data formated with HTTP protocol
            M_PROTO_JSON,               // user data formated with JSON protocol
            M_PROTO_MQTT,               // user data formated with MQTT protocol
            M_PROTO_BIN,                // user data is binary stream
        }
        public struct mesh_header_option_format
        {
            public byte otype;          // option type
            public byte olen;           // current option length
            public byte[] ovalue;       // option value
        }
        public struct mesh_header_option_header_type
        {
            public UInt16 ot_len;                     // option total length;
            public mesh_header_option_format[] olist; // option list
        }
        public struct proto_struct
        {
            public byte d;              //:1 direction, 1:upwards, 0:downwards
            public byte p2p;            //:1 node to node packet
            public byte protocol;       //:6 protocol used by user data;
        }
        public struct mesh_header_format
        {
            public byte ver;            //:2 version of mesh
            public byte oe;             //:1 option exist flag
            public byte cp;             //:1 piggyback congest permit in packet
            public byte cr;             //:1 piggyback congest request in packet
            public byte rsv;            //:3 reserve for fulture;
            public proto_struct proto;
            public UInt16 len;          // packet total length (include mesh header)
            public byte[] dst_addr;     // destiny address
            public byte[] src_addr;     // source address
            public byte[] user_data;    // user data
            public mesh_header_option_header_type[] option; // mesh option
        }
        /// <summary>
        /// 创建mesh包
        /// </summary>
        /// <param name="dst_addr">destination address (6 Bytes)</param>
        /// <param name="src_addr">source address (6 Bytes)</param>
        /// <param name="p2p">node-to-node packet</param>
        /// <param name="piggyback_cr">piggyback flow request</param>
        /// <param name="proto">protocol used by user data</param>
        /// <param name="data_len">length of user data</param>
        /// <param name="option">option flag</param>
        /// <param name="ot_len">option total length</param>
        /// <param name="frag">fragmentation flag</param>
        /// <param name="frag_type">fragmentation type</param>
        /// <param name="mf">fragmentation</param>
        /// <param name="frag_idx">fragmentation index</param>
        /// <param name="frag_id">fragmentation id</param>
        /// <returns>NULL: create mesh packet fail. mesh_header_format: mesh packet.</returns>
        public static mesh_header_format? espconn_mesh_create_packet(byte[] dst_addr, byte[] src_addr, bool p2p,
                                  bool piggyback_cr, mesh_usr_proto_type proto,
                                  UInt16 data_len, bool option, UInt16 ot_len,
                                  bool frag, mesh_option_type frag_type,
                                  bool mf, UInt16 frag_idx, UInt16 frag_id)
        {
            mesh_header_format header = new mesh_header_format();
            mesh_header_option_header_type[] option_header = new mesh_header_option_header_type[option ? 1 : 0];

            if (dst_addr.Length < 6) { return null; }
            if (src_addr.Length < 6) { return null; }
            if (src_addr.Length < 6) { return null; }
            if ((byte)proto > (byte)mesh_usr_proto_type.M_PROTO_BIN) { return null; }
            if ((byte)frag_type > (byte)mesh_option_type.M_O_USR_OPTION) { return null; }
            if ((MESH_HEADER_SIZE + ot_len + data_len) > 1300) { return null; }

            header.ver = (byte)MESH_VER;                /* mesh版本 */
            header.oe = (byte)(option ? 1 : 0);         /* 选项标志 */
            header.cp = 0;                              /* 数据包捎带流量应答 */
            header.cr = (byte)(piggyback_cr ? 1 : 0);   /* 数据包捎带流量请求 */
            header.rsv = 0;                             /* 保留字段 */
            header.proto.d = 0;                         /* 数据包流向 1:向上, 0:向下 */
            header.proto.p2p = (byte)(p2p ? 1 : 0);     /* 节点到节点的数据包 */
            header.proto.protocol = (byte)proto;        /* 用户数据格式 */
            header.len = (UInt16)(MESH_HEADER_SIZE + data_len + ot_len); /* mesh的数据包长度(包含mesh头信息) */
            header.dst_addr = dst_addr;                 /* 目的地址 */
            header.src_addr = src_addr;                 /* 源地址 */
            if (option)
            {
                header.option = option_header;          /* 选项头 */
                header.option[0].ot_len = ot_len;       /* 选项总长度 */
            }

            return header;
        }
        /// <summary>
        /// 解析mesh包
        /// </summary>
        /// <param name="data">mesh包数组</param>
        /// <returns>返回一个mesh_header_format结构</returns>
        public static mesh_header_format? espconn_mesh_analysis_packet(byte[] data)
        {
            mesh_header_format header = new mesh_header_format();
            mesh_header_option_header_type[] option_header;
            mesh_header_option_format[] options;
            byte[] dst_addr = { data[4], data[5], data[6], data[7], data[8], data[9] };
            byte[] src_addr = { data[10], data[11], data[12], data[13], data[14], data[15] };
            UInt16 len = (UInt16)(data[2] + (data[3] << 8));             /* 整包长度 */
            UInt16 ot_len = 0;                                           /* 选项长度 */
            byte op_count = 0;                                           /* 选项个数 */
            int index = 0;                                               /* 数组索引 */

            if (data.Length < 16) { return null; }
            if (data.Length > 1284) { return null; }
            if (data.Length != len) { return null; }                     /* 安全检查 */
            header.ver = (byte)((data[index] & 0x03) >> 0);              /* mesh版本 */
            header.oe = (byte)((data[index] & 0x04) >> 2);               /* 选项标志 */
            header.cp = (byte)((data[index] & 0x08) >> 3);               /* 数据包捎带流量应答 */
            header.cr = (byte)((data[index] & 0x10) >> 4);               /* 数据包捎带流量请求 */
            header.rsv = (byte)((data[index++] & 0xE0) >> 5);            /* 保留字段 */
            header.proto.d = (byte)((data[index] & 0x01) >> 0);          /* 数据包流向 1:向上, 0:向下 */
            header.proto.p2p = (byte)((data[index] & 0x02) >> 1);        /* 节点到节点的数据包 */
            header.proto.protocol = (byte)((data[index++] & 0xFC) >> 2); /* 数据协议 */
            header.len = len;                                            /* mesh的数据包长度(包含mesh头信息) */
            header.dst_addr = dst_addr;                                  /* 目的地址 */
            header.src_addr = src_addr;                                  /* 源地址 */
            if (header.oe == 1)
            {
                ot_len = (UInt16)(data[MESH_HEADER_SIZE] + (data[MESH_HEADER_SIZE + 1] << 8)); /* 选项字节数 */
                index = MESH_HEADER_SIZE + 2/* ot_len */ + 1/* otype */;
                while (true) /* 计算选项个数 */
                {
                    byte olen = data[index];
                    index += olen;
                    op_count++;
                    if (index >= MESH_HEADER_SIZE + ot_len - 1)
                    {
                        break;
                    }
                }
                option_header = new mesh_header_option_header_type[1];
                options = new mesh_header_option_format[op_count];
                index = MESH_HEADER_SIZE + 2/* ot_len */;
                for (int i = 0; i < op_count; i++)
                {
                    byte otype = data[index++];
                    byte olen = data[index++];
                    byte[] ovalue = new byte[olen - 2];
                    for (int j = 0; j < olen - 2; j++)
                    {
                        ovalue[j] = data[index++];
                    }
                    options[i].otype = otype;
                    options[i].olen = olen;
                    options[i].ovalue = ovalue;
                }
                option_header[0].ot_len = ot_len;
                option_header[0].olist = options;
                header.option = option_header;
            }

            UInt16 data_len = (UInt16)(len - MESH_HEADER_SIZE - ot_len); /* 用户数据长度 */
            if (data_len > 0)
            {
                byte[] user_data = new byte[data_len];
                for (int i = 0; i < data_len; i++)
                {
                    user_data[i] = data[i + MESH_HEADER_SIZE + ot_len];
                }
                header.user_data = user_data;
            }

            return header;
        }
        /// <summary>
        /// 创建选项包
        /// </summary>
        /// <param name="otype">选项类型</param>
        /// <param name="ovalue">数据</param>
        /// <param name="val_len">包长</param>
        /// <returns>返回一个mesh_header_option_format结构</returns>
        public static mesh_header_option_format? espconn_mesh_create_option(byte otype, byte[] ovalue, byte val_len)
        {
            mesh_header_option_format option = new mesh_header_option_format();

            if (ovalue.Length != val_len)
            {
                return null;
            }
            option.otype = otype;
            option.olen = (byte)(val_len + 2);
            option.ovalue = ovalue;

            return option;
        }
        /// <summary>
        /// 向mesh包中添加一个选项包
        /// </summary>
        /// <param name="head">mesh包头</param>
        /// <param name="option">选项包</param>
        /// <returns>返回一个添加好option的mesh包</returns>
        public static mesh_header_format? espconn_mesh_add_option(mesh_header_format head, mesh_header_option_format option)
        {
            if (head.option[0].olist == null)
            {
                mesh_header_option_format[] options = new mesh_header_option_format[1];
                options[0] = option;
                head.option[0].olist = options;
            }
            else
            {
                int op_count = head.option[0].olist.Length; /* mesh头中的选项个数 */
                mesh_header_option_format[] options = new mesh_header_option_format[op_count + 1];
                head.option[0].olist.CopyTo(options, 0);    /* 将mesh头中的选项复制出来 */
                options[op_count] = option;
                head.option[0].olist = options;
            }

            return head;
        }
        /// <summary>
        /// 向mesh包中添加用户数据
        /// </summary>
        /// <param name="head">mesh包头</param>
        /// <param name="usr_data">用户数据</param>
        /// <param name="data_len">数据长度</param>
        /// <returns>true or false</returns>
        public static mesh_header_format? espconn_mesh_set_usr_data(mesh_header_format head, byte[] usr_data, UInt16 data_len)
        {
            if (usr_data.Length != data_len) { return null; }
            head.user_data = usr_data;

            return head;
        }
        /// <summary>
        /// 生成需要通过网络发送的mesh包数组
        /// </summary>
        /// <param name="header"></param>
        /// <param name="len"></param>
        /// <returns>返回mesh包数组</returns>
        public static byte[] espconn_mesh_sent(mesh_header_format header, UInt16 len)
        {
            int index = 0;

            if (header.len != len) { return null; }

            byte[] packet = new byte[header.len]; /* 申请内存 */
            packet[index++] = (byte)((((byte)header.ver) << 0) | (((byte)header.oe) << 2) | (((byte)header.cp) << 3) | (((byte)header.cr) << 4));
            packet[index++] = (byte)((((byte)header.proto.protocol) << 2) | ((header.proto.p2p != 0 ? 1 : 0)));
            packet[index++] = (byte)((packet.Length >> 0) & 0xFF);
            packet[index++] = (byte)((packet.Length >> 8) & 0xFF);
            header.dst_addr.CopyTo(packet, index);
            index += 6;
            header.src_addr.CopyTo(packet, index);
            index += 6;
            if (header.option != null)
            {
                packet[index++] = (byte)((header.option[0].ot_len << 0) & 0xFF);
                packet[index++] = (byte)((header.option[0].ot_len << 8) & 0xFF);
                for (int i = 0; i < header.option[0].olist.Length; i++)
                {
                    packet[index++] = header.option[0].olist[i].otype;
                    packet[index++] = header.option[0].olist[i].olen;
                    header.option[0].olist[i].ovalue.CopyTo(packet, index);
                    index += header.option[0].olist[i].ovalue.Length;
                }
            }
            if (header.user_data != null)
            {
                header.user_data.CopyTo(packet, index);
                index += header.user_data.Length;
            }

            return packet;
        }
        /// <summary>
        /// 创建拓扑请求包
        /// </summary>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static byte[] create_topo_req(byte[] dst)
        {
            Mesh.mesh_header_format header;
            Mesh.mesh_header_option_format topo_option;
            byte[] src = { 0, 0, 0, 0, 0, 0 };
            byte[] dev = { 0, 0, 0, 0, 0, 0 };

            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_create_packet(
                    dst,    /* 目标地址 */
                    src,    /* 源地址 */
                    false,  /* 不是p2p包 */
                    true,   /* piggyback congest request */
                    Mesh.mesh_usr_proto_type.M_PROTO_NONE, /* 包格式 */
                    0,      /* 用户数据长度 */
                    true,   /* 选项字节 */
                    10,     /* 选项字节长度 */
                    false,  /* 分片选项 */
                    0,      /* 分片类型 */
                    false,  /* 更多分片 */
                    0,      /* 分片index */
                    0      /* 分片id */
                );
            topo_option = (Mesh.mesh_header_option_format)Mesh.espconn_mesh_create_option(
                          (byte)Mesh.mesh_option_type.M_O_TOPO_REQ, 
                          dev, 
                          (byte)dev.Length);

            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_add_option(header, topo_option);
            return Mesh.espconn_mesh_sent(header, header.len);
        }
        public static byte[] create_user_data(byte[] dst, byte[] user_data)
        {
            mesh_header_format header;
            byte[] src = { 0, 0, 0, 0, 0, 0 };

            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_create_packet(
                    dst,     /* 目标地址 */
                    src,     /* 源地址 */
                    true,    /* 不是p2p包 */
                    true,    /* piggyback congest request */
                    Mesh.mesh_usr_proto_type.M_PROTO_BIN, /* 包格式 */
                    (UInt16)user_data.Length,             /* 用户数据长度 */
                    false,   /* 选项字节 */
                    0,       /* 选项字节长度 */
                    false,   /* 分片选项 */
                    0,       /* 分片类型 */
                    false,   /* 更多分片 */
                    0,       /* 分片index */
                    0        /* 分片id */
                );
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_set_usr_data(
                                                       header, 
                                                       user_data, 
                                                       (UInt16)user_data.Length);

            return Mesh.espconn_mesh_sent(header, header.len);
        }
    }
}
