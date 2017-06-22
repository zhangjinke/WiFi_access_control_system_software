using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessControlSystem.Lib;

namespace AccessControlSystem.Model
{
    class Stm32Sync
    {
        static byte CMD_SERVER_USER_ADD = (0x01);     /**< \brief 添加用户 */
        static byte CMD_SERVER_USER_DEL = (0x02);     /**< \brief 删除用户 */
        static byte CMD_SERVER_CONNECT = (0x03);      /**< \brief 联机 */
        static byte CMD_SERVER_TIMESYNC = (0x04);     /**< \brief 同步时间 */
        static byte CMD_SERVER_ATT_GET = (0x05);      /**< \brief 获取指定条数考勤信息 每次最多18条 */
        static byte CMD_SERVER_DOOR_OPEN = (0x06);    /**< \brief 开门 */
        static byte CMD_SERVER_CONFIG_SET = (0x07);   /**< \brief 下传配置信息 */
        static byte CMD_SERVER_LIST_RELOAD = (0x08);  /**< \brief 重新加载卡号-用户号，指纹号-用户号列表 */
        static byte CMD_SERVER_ATT_DEL = (0x09);      /**< \brief 删除指定条数考勤记录 */
        static byte CMD_SERVER_USER_CRC_GET = (0x0A); /**< \brief 获取指定条数用户CRC 每次最多200条 */

        static byte CMD_SERVER_FILE_START = (0xF0);   /**< \brief 开始文件传输 */
        static byte CMD_SERVER_FILE_DATA = (0xF1);    /**< \brief 文件数据 */
        static byte CMD_SERVER_FILE_END = (0xF2);     /**< \brief 文件传输结束 */

        static byte CMD_SERVER_ACK = (0x55);          /**< \brief ACK */
        static byte CMD_SERVER_NACK = (0xAA);         /**< \brief NACK */

        static UInt32 USER_PACK_SIZE = 1058;          /**< \brief 下位机人员结构大小 */

        public delegate void Output(string str);
        public Output output_dlg;

        public void output(string str)
        {
            if (null != output_dlg)
            {
                output_dlg(str);
            }
        }
        /// <summary>
        /// 计算用户crc
        /// </summary>
        /// <param name="PersonInfo"></param>
        /// <returns></returns>
        public UInt32 user_crc_calc(PersonnelManagement.PersonInfo PersonInfo)
        {
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] user_data = new byte[1 + USER_PACK_SIZE + 4];
            string temp_str;
            byte[] temp;

            /* 有效期 */
            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy年MM月dd日 HH:mm:ss";
            DateTime limitTime = Convert.ToDateTime(PersonInfo.limitTime, dtFormat);
            int year = limitTime.Year;
            int month = limitTime.Month;
            int day = limitTime.Day;
            int hour = limitTime.Hour;
            int minute = limitTime.Minute;
            int second = limitTime.Second;

            /* 用户号 */
            user_data[1] = (byte)((PersonInfo.uID & 0x00FF) >> 0);
            user_data[2] = (byte)((PersonInfo.uID & 0xFF00) >> 8);

            /* RFID卡号 */
            user_data[3] = (byte)((PersonInfo.cardID & 0x000000FF) >> 0);
            user_data[4] = (byte)((PersonInfo.cardID & 0x0000FF00) >> 8);
            user_data[5] = (byte)((PersonInfo.cardID & 0x00FF0000) >> 16);
            user_data[6] = (byte)((PersonInfo.cardID & 0xFF000000) >> 24);

            /* 激活状态 */
            user_data[7] = (byte)PersonInfo.activeState;

            /* 学号 */
            temp_str = PersonInfo.studentID;
            temp = Encoding.Default.GetBytes(temp_str);
            Buffer.BlockCopy(temp, 0, user_data, 8, temp.Length);

            /* 姓名 */
            temp_str = PersonInfo.name;
            temp = Encoding.GetEncoding("utf-8").GetBytes(temp_str);
            //temp = Encoding.Default.GetBytes(temp_str);
            Buffer.BlockCopy(temp, 0, user_data, 24, temp.Length);

            /* 权限 */
            DeviceManagement deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 设备数量 */

            temp = new byte[16];
            temp_str = PersonInfo.authority;
            string[] split_array = temp_str.Replace(" ", "").Split(',');
            for (int i = 0; i < split_array.Length; i++)
            {
                if (split_array[i] == "超级管理员")
                {
                    temp[15] |= (byte)(1 << 7);
                }
                else if (split_array[i] == "管理员")
                {
                    temp[15] |= (byte)(1 << 6);
                }
                else
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (split_array[i] == deviceManagement.DeviceList[j].name)
                        {
                            Int32 id = (Int32)deviceManagement.DeviceList[j].ID;
                            temp[id / 8] |= (byte)(1 << (id % 8));
                        }
                    }
                }
            }
            Buffer.BlockCopy(temp, 0, user_data, 40, temp.Length);

            /* 是否有时间限制 */
            user_data[56] = (byte)PersonInfo.isLimitTime;

            /* 有效期 */
            user_data[57] = (byte)(year & 0xFF);
            user_data[58] = (byte)((year & 0xFF00) >> 8);
            user_data[59] = (byte)month;
            user_data[60] = (byte)day;
            user_data[61] = (byte)hour;
            user_data[62] = (byte)minute;
            user_data[63] = (byte)second;

            /* 指纹号 */
            for (int i = 0; i < 5; i++)
            {
                user_data[64 + i * 2] = (byte)((PersonInfo.eigenNum[i] & 0x00FF) >> 0);
                user_data[65 + i * 2] = (byte)((PersonInfo.eigenNum[i] & 0xFF00) >> 8);
            }

            /* 指纹特征值 */
            for (int i = 0; i < 5; i++)
            {
                temp = String_Byte.strToHexByte(PersonInfo.eigen[i]);
                Buffer.BlockCopy(temp, 0, user_data, 74 + i * 193, temp.Length);
            }

            /* crc */
            crc = stm32_crc.block_crc_calc(user_data, 1, USER_PACK_SIZE - 4 - 16);

            return crc;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <param name="PersonInfo"></param>
        public bool user_add(byte[] dst_addr, PersonnelManagement.PersonInfo PersonInfo)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[1 + USER_PACK_SIZE + 4];
            string temp_str;
            byte[] temp;

            /* 有效期 */
            System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy年MM月dd日 HH:mm:ss";
            DateTime limitTime = Convert.ToDateTime(PersonInfo.limitTime, dtFormat);
            int year = limitTime.Year;
            int month = limitTime.Month;
            int day = limitTime.Day;
            int hour = limitTime.Hour;
            int minute = limitTime.Minute;
            int second = limitTime.Second;

            /* CMD */
            user_data[0] = CMD_SERVER_USER_ADD;

            /* 用户号 */
            user_data[1] = (byte)((PersonInfo.uID & 0x00FF) >> 0);
            user_data[2] = (byte)((PersonInfo.uID & 0xFF00) >> 8);

            /* RFID卡号 */
            user_data[3] = (byte)((PersonInfo.cardID & 0x000000FF) >> 0);
            user_data[4] = (byte)((PersonInfo.cardID & 0x0000FF00) >> 8);
            user_data[5] = (byte)((PersonInfo.cardID & 0x00FF0000) >> 16);
            user_data[6] = (byte)((PersonInfo.cardID & 0xFF000000) >> 24);

            /* 激活状态 */
            user_data[7] = (byte)PersonInfo.activeState;

            /* 学号 */
            temp_str = PersonInfo.studentID;
            temp = Encoding.Default.GetBytes(temp_str);
            Buffer.BlockCopy(temp, 0, user_data, 8, temp.Length);

            /* 姓名 */
            temp_str = PersonInfo.name;
            temp = Encoding.GetEncoding("utf-8").GetBytes(temp_str);
            //temp = Encoding.Default.GetBytes(temp_str);
            Buffer.BlockCopy(temp, 0, user_data, 24, temp.Length);

            /* 权限 */
            DeviceManagement deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 设备数量 */

            temp = new byte[16];
            temp_str = PersonInfo.authority;
            string[] split_array = temp_str.Replace(" ", "").Split(',');
            for (int i = 0; i < split_array.Length; i++)
            {
                if (split_array[i] == "超级管理员")
                {
                    temp[15] |= (byte)(1 << 7);
                }
                else if (split_array[i] == "管理员")
                {
                    temp[15] |= (byte)(1 << 6);
                }
                else
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (split_array[i] == deviceManagement.DeviceList[j].name)
                        {
                            Int32 id = (Int32)deviceManagement.DeviceList[j].ID;
                            temp[id / 8] |= (byte)(1 << (id % 8));
                        }
                    }
                }
            }
            Buffer.BlockCopy(temp, 0, user_data, 40, temp.Length);

            /* 是否有时间限制 */
            user_data[56] = (byte)PersonInfo.isLimitTime;

            /* 有效期 */
            user_data[57] = (byte)(year & 0xFF);
            user_data[58] = (byte)((year & 0xFF00) >> 8);
            user_data[59] = (byte)month;
            user_data[60] = (byte)day;
            user_data[61] = (byte)hour;
            user_data[62] = (byte)minute;
            user_data[63] = (byte)second;

            /* 指纹号 */
            for (int i = 0; i < 5; i++)
            {
                user_data[64 + i * 2] = (byte)((PersonInfo.eigenNum[i] & 0x00FF) >> 0);
                user_data[65 + i * 2] = (byte)((PersonInfo.eigenNum[i] & 0xFF00) >> 8);
            }

            /* 指纹特征值 */
            for (int i = 0; i < 5; i++)
            {
                temp = String_Byte.strToHexByte(PersonInfo.eigen[i]);
                Buffer.BlockCopy(temp, 0, user_data, 74 + i * 193, temp.Length);
            }

            /* crc */
            crc = stm32_crc.block_crc_calc(user_data, 1, USER_PACK_SIZE - 4 - 16);
            user_data[1039] = (byte)((crc & 0x000000FF) >> 0);
            user_data[1040] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[1041] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[1042] = (byte)((crc & 0xFF000000) >> 24);

            /* 进出状态 */
            temp = new byte[16];
            Buffer.BlockCopy(temp, 0, user_data, 1043, temp.Length);

            /* crc */
            crc = stm32_crc.block_crc_calc(user_data, 0, 1 + USER_PACK_SIZE);
            user_data[user_data.Length - 4] = (byte)((crc & 0x000000FF) >> 0);
            user_data[user_data.Length - 3] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[user_data.Length - 2] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[user_data.Length - 1] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_USER_ADD != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("添加" + PersonInfo.uID.ToString() + "号用户失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("添加" + PersonInfo.uID.ToString() + "号用户成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <param name="user_id"></param>
        public bool user_del(byte[] dst_addr, UInt32 user_id)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[7];

            /* cmd */
            user_data[0] = CMD_SERVER_USER_DEL;

            /* 用户号 */
            user_data[1] = (byte)((user_id & 0x00FF) >> 0);
            user_data[2] = (byte)((user_id & 0xFF00) >> 8);

            /* crc */
            crc = stm32_crc.block_crc_calc(user_data, 0, 3);
            user_data[3] = (byte)((crc & 0x000000FF) >> 0);
            user_data[4] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[5] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[6] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_USER_DEL != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("删除" + user_id.ToString() + "号用户失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("删除" + user_id.ToString() + "号用户成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <returns></returns>
        public bool connect (byte[] dst_addr)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[5];

            user_data[0] = CMD_SERVER_CONNECT;

            crc = stm32_crc.block_crc_calc(user_data, 0, 1);
            user_data[1] = (byte)((crc & 0x000000FF) >> 0);
            user_data[2] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[3] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[4] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_CONNECT != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("联机失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("联机成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 同步时间
        /// </summary>
        /// <param name="dst_addr"></param>
        public bool time_sync(byte[] dst_addr)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[13];

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            int week = (int)DateTime.Now.DayOfWeek;

            user_data[0] = CMD_SERVER_TIMESYNC;
            user_data[1] = (byte)(year & 0xFF);
            user_data[2] = (byte)((year & 0xFF00) >> 8);
            user_data[3] = (byte)month;
            user_data[4] = (byte)day;
            user_data[5] = (byte)hour;
            user_data[6] = (byte)minute;
            user_data[7] = (byte)second;
            user_data[8] = (byte)week;

            crc = stm32_crc.block_crc_calc(user_data, 0, 9);
            user_data[9] = (byte)((crc & 0x000000FF) >> 0);
            user_data[10] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[11] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[12] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_TIMESYNC != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("时间同步失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("时间同步成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 获取考勤记录
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <param name="num"></param>
        /// <param name="att_data"></param>
        /// <returns></returns>
        public bool att_get(byte[] dst_addr, UInt16 num, out UInt32 total, out UInt16 count, out byte[] att_data)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[7];

            att_data = new byte[0];
            total = 0;
            count = 0;

            user_data[0] = CMD_SERVER_ATT_GET;

            user_data[1] = (byte)((num & 0x00FF) >> 0);
            user_data[2] = (byte)((num & 0xFF00) >> 8);

            crc = stm32_crc.block_crc_calc(user_data, 0, 3);
            user_data[3] = (byte)((crc & 0x000000FF) >> 0);
            user_data[4] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[5] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[6] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(2000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_ATT_GET != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[7]))
            {
                output("获取考勤记录失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                total = (UInt32)((header.user_data[8] << 0) +
                                 (header.user_data[9] << 8) +
                                 (header.user_data[10] << 16) +
                                 (header.user_data[11] << 24));
                count = (UInt16)((header.user_data[12] << 0) +
                                 (header.user_data[13] << 8));

                crc = (UInt32)((header.user_data[14 + count * 53 + 0] << 0) +
                               (header.user_data[14 + count * 53 + 1] << 8) +
                               (header.user_data[14 + count * 53 + 2] << 16) +
                               (header.user_data[14 + count * 53 + 3] << 24));

                if (crc != stm32_crc.block_crc_calc(header.user_data, 0, (UInt32)(14 + count * 53)))
                {
                    output("考勤记录crc校验失败 " + mac_addr.ToString());
                    return false;
                }

                att_data = new byte[count * 53];

                Buffer.BlockCopy(header.user_data, 14, att_data, 0, att_data.Length);

                output("获取考勤记录成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 重新加载卡号-用户号，指纹号-用户号列表
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <returns></returns>
        public bool list_reload(byte[] dst_addr)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[5];

            user_data[0] = CMD_SERVER_LIST_RELOAD;

            crc = stm32_crc.block_crc_calc(user_data, 0, 1);
            user_data[1] = (byte)((crc & 0x000000FF) >> 0);
            user_data[2] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[3] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[4] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_LIST_RELOAD != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("重新加载卡号-用户号，指纹号-用户号列表失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("重新加载卡号-用户号，指纹号-用户号列表成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 删除指定条数考勤记录
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool att_del (byte[] dst_addr, UInt16 num)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[7];

            user_data[0] = CMD_SERVER_ATT_DEL;

            user_data[1] = (byte)((num & 0x00FF) >> 0);
            user_data[2] = (byte)((num & 0xFF00) >> 8);

            crc = stm32_crc.block_crc_calc(user_data, 0, 3);
            user_data[3] = (byte)((crc & 0x000000FF) >> 0);
            user_data[4] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[5] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[6] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(1000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_ATT_DEL != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[8]))
            {
                output("删除" + num.ToString() + "条考勤记录失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                output("删除" + num.ToString() + "条考勤记录成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 获取指定条数用户CRC
        /// </summary>
        /// <param name="dst_addr"></param>
        /// <param name="start"></param>
        /// <param name="num"></param>
        /// <param name="total"></param>
        /// <param name="count"></param>
        /// <param name="user_crc"></param>
        /// <returns></returns>
        public bool user_crc_get (byte[] dst_addr, UInt16 start, UInt16 num, out UInt16 total, out UInt16 count, out byte[] user_crc)
        {
            MacAddr mac_addr = new MacAddr(dst_addr);
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] head;
            byte[] user_data = new byte[9];

            user_crc = new byte[0];
            total = 0;
            count = 0;

            user_data[0] = CMD_SERVER_USER_CRC_GET;

            user_data[1] = (byte)((start & 0x00FF) >> 0);
            user_data[2] = (byte)((start & 0xFF00) >> 8);

            user_data[3] = (byte)((num & 0x00FF) >> 0);
            user_data[4] = (byte)((num & 0xFF00) >> 8);

            crc = stm32_crc.block_crc_calc(user_data, 0, 5);
            user_data[5] = (byte)((crc & 0x000000FF) >> 0);
            user_data[6] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[7] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[8] = (byte)((crc & 0xFF000000) >> 24);

            head = Mesh.create_user_data(dst_addr, user_data);

            Global.data_recv = null;
            if (false == Global.netTCPServer.SendData(head))
            {
                return false;
            }
            if (Global.wait_data(3000) == false)
            {
                output("连接超时 " + mac_addr.ToString());
                return false;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);
            if ((CMD_SERVER_USER_CRC_GET != header.user_data[6]) || (CMD_SERVER_ACK != header.user_data[7]))
            {
                output("获取用户crc失败 " + mac_addr.ToString());
                return false;
            }
            else
            {
                total = (UInt16)((header.user_data[8] << 0) +
                                 (header.user_data[9] << 8));
                count = (UInt16)((header.user_data[10] << 0) +
                                 (header.user_data[11] << 8));

                crc = (UInt32)((header.user_data[12 + count * 4 + 0] << 0) +
                               (header.user_data[12 + count * 4 + 1] << 8) +
                               (header.user_data[12 + count * 4 + 2] << 16) +
                               (header.user_data[12 + count * 4 + 3] << 24));

                if (crc != stm32_crc.block_crc_calc(header.user_data, 0, (UInt32)(12 + count * 4)))
                {
                    output("获取用户crc crc校验失败 " + mac_addr.ToString());
                    return false;
                }

                user_crc = new byte[count * 4];

                Buffer.BlockCopy(header.user_data, 12, user_crc, 0, user_crc.Length);

                output("获取用户crc成功 " + mac_addr.ToString());
                return true;
            }
        }
        /// <summary>
        /// 将人员信息表(Member.txt)传入下位机
        /// </summary>
        private void DownloadAll()
        {
            //btnDownloadAll.Enabled = false;
            //if (uart.serialPort.IsOpen == false)
            //{
            //    MessageBox.Show("请打开串口");
            //    btnDownloadAll.Enabled = true;
            //    return;
            //}
            //try
            //{
            //    //获取需要更新的设备数
            //    int deviceNum = 1;
            //    if (cbDeviceList.Text == "全部设备")
            //    {
            //        deviceNum = cbDeviceList.Items.Count - 1;
            //        if (deviceNum == 0) { output("未连接任何设备！"); }
            //    }
            //    for (int j = 0; j < deviceNum; j++)
            //    {
            //        string DeviceInfo = cbDeviceList.Items[j].ToString();
            //        byte address = Convert.ToByte(DeviceInfo.Substring(DeviceInfo.LastIndexOf("：") + 1));//设备地址
            //        string deviceName = DeviceInfo.Substring(0, (DeviceInfo.LastIndexOf("：") - 4));//设备名称
            //        //将设备中的人员信息存到数据库中

            //        downloadToOneDevice(address);
            //        output("正在下传人员头像至设备：" + deviceName);
            //        syncPicture(address);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //btnDownloadAll.Enabled = true;
        }

        private byte downloadToOneDevice(byte address)
        {
            //string path = Environment.CurrentDirectory + @"\dataBase\Member.txt";
            //FileStream fs = new FileStream(path, FileMode.Create);
            //int rowsCount = dgvPerson.Rows.Count;
            //byte[] data = System.Text.Encoding.Default.GetBytes("用户号\t卡号\t学号\t姓名\t权限\t状态\t指纹\t总人数\t" + rowsCount.ToString().PadLeft(4, '0') + "\r\n");
            //fs.Write(data, 0, data.Length);
            //fs.Flush();
            //fs.Close();
            //for (int i = 0; i < rowsCount; i++)
            //{
            //    string UID = dgvPerson.Rows[i].Cells[0].Value.ToString();
            //    string ID = dgvPerson.Rows[i].Cells[11].Value.ToString();
            //    string StudentID = dgvPerson.Rows[i].Cells[3].Value.ToString();
            //    string Name = dgvPerson.Rows[i].Cells[1].Value.ToString();
            //    int AuthorityInt = Convert.ToInt32(dgvPerson.Rows[i].Cells[10].Value.ToString(),16);
            //    string Authority = AuthorityInt.ToString("X");
            //    string Eigen = dgvPerson.Rows[i].Cells[12].Value.ToString();
            //    writeInfo(UID, ID, StudentID, Name, Authority, "1", Eigen);
            //}
            //StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            //sw.Write("OVER\r\n");
            //sw.Flush();
            //sw.Close();
            //fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //byte[] buffur = new byte[fs.Length];
            //fs.Read(buffur, 0, (int)fs.Length);

            //output("正在将人员信息表下传至设备：" + address);
            //byte state = uart.sendNByte(buffur, 0x0B, address, (uint)buffur.Length, 300);
            //uart.cleanReceiveData();//清除接收器
            //switch (state)
            //{
            //    case 0: { output("下传人员信息表命令发送成功!"); } break;
            //    case 1: { output("下传人员信息表命令发送超时!"); } break;
            //    case 2: { output("下传人员信息表命令超重发次数!"); } break;
            //    case 3: { output("下传人员信息表命令通信错误!"); } break;
            //}
            //if (state != 0) { return 0xff; }
            //int outTimeX = 50000;
            //while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //{
            //    outTimeX--;
            //    Thread.Sleep(1);
            //}
            //uart.exchangeOrder(6, 7);
            //uart.exchangeOrder(9, 12);
            //uart.exchangeOrder(13, 16);
            //uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //uart.transformDataShort();
            //if (outTimeX == 0) { output("下传人员信息表命令应答超时!"); return 0xff; }//如果超时，中断发送
            //if ((uart.HEAD != uart.Z_HEAD)
            //    || (uart.ADDRESS != address)
            //    || (uart.CMD != 0x0B)
            //    || (uart.BLOCK != 0x0000)
            //    || (uart.TOTAL != 0x00000000))	//判断应答是否正确
            //{ output("下传人员信息表失败!"); return 0xff; }
            //switch (uart.BYTE)
            //{
            //    case 0: { output("下传人员信息表成功!"); } break;
            //    case 1: { output("下传人员信息表失败!"); } break;
            //}
            //if (uart.BYTE != 0) { return 0xff; }
            //return 0x00;
            return 0x00;
        }
        private byte syncPicture(byte address)
        {
            //int RowCount = dgvPerson.RowCount;
            //for (int i = 0; i < RowCount; i++)
            //{
            //    string uID = dgvPerson.Rows[i].Cells[0].Value.ToString().PadLeft(4, '0');
            //    string name = dgvPerson.Rows[i].Cells[1].Value.ToString();
            //    if (name.Length == 2) { name = name.Insert(1, "  "); }
            //    name = name.PadLeft(3); //姓名（6字节）          

            //    String Path = Environment.CurrentDirectory + @"\picture\" + uID.Replace(" ", "") + "_" + name.Replace(" ", "") + ".jpg";
            //    if (!File.Exists(Path)) { output(uID + "号用户" + name + "头像不存在"); continue; }
            //    FileStream fs = File.OpenRead(Path);
            //    if (fs.Length == 17890) { continue; }
            //    byte[] data = System.Text.Encoding.Default.GetBytes(uID + "_" + name + ".jpg");
            //    byte[] imagebu = new byte[fs.Length + data.Length];
            //    for (int j = 0; j < data.Length; j++)
            //    {
            //        imagebu[j] = data[j];
            //    }
            //    fs.Read(imagebu, data.Length, (int)fs.Length);
            //    fs.Close();
            //    if (getPictureState(address, uID, name) != 0x00) { continue; }
            //    output("正在下传" + uID + "号用户" + name + "头像");
            //    byte state = uart.sendNByte(imagebu, 0x09, address, (uint)imagebu.Length, 400);
            //    uart.cleanReceiveData();//清除接收器
            //    switch (state)
            //    {
            //        case 0: { output("下传头像命令发送成功!"); } break;
            //        case 1: { output("下传头像命令发送超时!"); } break;
            //        case 2: { output("下传头像命令超重发次数!"); } break;
            //        case 3: { output("下传头像命令通信错误!"); } break;
            //    }
            //    if (state != 0) { return 0xff; }
            //    int outTimeX = 5000;
            //    while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //    {
            //        outTimeX--;
            //        Thread.Sleep(1);
            //    }
            //    uart.exchangeOrder(6, 7);
            //    uart.exchangeOrder(9, 12);
            //    uart.exchangeOrder(13, 16);
            //    uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //    uart.transformDataShort();
            //    if (outTimeX == 0) { output("下传头像命令应答超时!"); return 0xff; }//如果超时，中断发送
            //    if ((uart.HEAD != uart.Z_HEAD)
            //        || (uart.ADDRESS != address)
            //        || (uart.CMD != 0x09)
            //        || (uart.BYTE != 0xff)
            //        || (uart.BLOCK != 0x0000)
            //        || (uart.TOTAL != 0x00000000))	//判断应答是否正确
            //    { output("下传头像失败!"); return 0xff; }
            //    else
            //    {
            //        output("下传头像成功!");
            //    }
            //}
            //return 0x00;
            return 0x00;
        }
        private byte getPictureState(byte address, string uID, string name)
        {
            //String Path = Environment.CurrentDirectory + @"\picture\" + uID.Replace(" ", "") + "_" + name.Replace(" ", "") + ".jpg";
            //if (!File.Exists(Path)) { output(uID + "号用户" + name + "头像不存在"); return 0xff; }
            //FileStream fs = File.OpenRead(Path);
            //long lenth = fs.Length;
            //string sendDataString;
            //sendDataString = uID.PadLeft(4, '0') + ".jpg"+fs.Length.ToString().PadLeft(8,'0');
            //fs.Close();
            //byte[] sendData = System.Text.Encoding.Default.GetBytes(sendDataString);

            //output("正在查询" + uID + "号用户" + name + "头像是否存在下位机");
            //byte state = uart.sendNByte(sendData, 0x0C, address, (uint)sendData.Length, 500);
            //uart.cleanReceiveData();//清除接收器
            //switch (state)
            //{
            //    case 0: { output("下传查询命令发送成功!"); } break;
            //    case 1: { output("下传查询命令发送超时!"); } break;
            //    case 2: { output("下传查询命令超重发次数!"); } break;
            //    case 3: { output("下传查询命令通信错误!"); } break;
            //}
            //if (state != 0) { return 0xff; }
            //int outTimeX = 5000;
            //while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //{
            //    outTimeX--;
            //    Thread.Sleep(1);
            //}
            //uart.exchangeOrder(6, 7);
            //uart.exchangeOrder(9, 12);
            //uart.exchangeOrder(13, 16);
            //uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //uart.transformDataShort();
            //if (outTimeX == 0) { output("下传查询命令应答超时!"); return 0xff; }//如果超时，中断发送
            //if ((uart.HEAD != uart.Z_HEAD)
            //    || (uart.ADDRESS != address)
            //    || (uart.CMD != 0x0C)
            //    || (uart.BLOCK != 0x0000)
            //    || (uart.TOTAL != 0x00000000))	//判断应答是否正确
            //{ output("下传查询命令失败!"); return 0xff; }
            //switch (uart.BYTE)
            //{
            //    case 0: { output(uID + "号用户" + name + "头像不存在下位机"); return 0x00; }
            //    case 1: { output(uID + "号用户" + name + "头像存在下位机"); return 0x01; }
            //    default: { return 0x03; }
            //}
            return 0xff;
        }
        /// <summary>
        /// 将人员信息传入下位机
        /// </summary>
        private void Download()
        {
            //btnDownload.Enabled = false;
            //if (uart.serialPort.IsOpen == false)
            //{
            //    MessageBox.Show("请打开串口");
            //    btnDownload.Enabled = true;
            //    return;
            //}
            //try
            //{
            //    //获取需要更新的设备数
            //    int deviceNum = 1;
            //    if(cbDeviceList.Text=="全部设备")
            //    {
            //       deviceNum = cbDeviceList.Items.Count - 1;
            //       if (deviceNum == 0) { output("未连接任何设备！"); }
            //    }
            //    for(int j=0;j<deviceNum;j++)
            //    {                    
            //        string DeviceInfo = cbDeviceList.Items[j].ToString();
            //        byte address = Convert.ToByte(DeviceInfo.Substring(DeviceInfo.LastIndexOf("：") + 1));//设备地址
            //        string deviceName = DeviceInfo.Substring(0, (DeviceInfo.LastIndexOf("：") - 4));//设备名称
            //        //将设备中的人员信息存到数据库中
            //        if (getUserInfo(address) != 0x00) { btnDownload.Enabled = true; break; }
            //        output("正在与设备：" + deviceName + "同步人员信息");
            //        if (synchronization(address) != 0x00) { btnDownload.Enabled = true; break; }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //btnDownload.Enabled = true;
        }
        /// <summary>
        /// 将指定设备与上位机的数据库同步
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte synchronization(byte address)
        {
            //SQLiteConnection conn = null;
            //string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            //conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置 

            //try
            //{
            //    conn.Open();//打开数据库，若文件不存在会自动创建  
            //    string sql = "SELECT COUNT(*) FROM student";
            //    SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
            //    int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
            //    cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

            //    sql = "SELECT * FROM student";
            //    cmdQ = new SQLiteCommand(sql, conn);
            //    SQLiteDataReader readerStudent = cmdQ.ExecuteReader();//读取上位机数据库内人员信息
            //    while (readerStudent.Read())
            //    {
            //        int sourceUID = readerStudent.GetInt32(0);  //用户号
            //        string sourceId = readerStudent.GetString(10);//卡号
            //        string sourceStudentID = readerStudent.GetString(3); //学号
            //        string sourceName = readerStudent.GetString(1); //姓名
            //        sourceName = sourceName.Replace(" ", ""); //删除字符串中的空格
            //        byte sourceAuthority = readerStudent.GetByte(9);  //权限
            //        string sourceEigen = readerStudent.GetString(11);   //特征值
            //        string sqlStudentTemp = "SELECT * FROM studentTemp WHERE uID=" + sourceUID.ToString();
            //        SQLiteCommand cmdStudentTemp = new SQLiteCommand(sqlStudentTemp, conn);
            //        SQLiteDataReader readerStudentTemp = cmdStudentTemp.ExecuteReader();//在下位机数据库中搜索指定用户号的用户
            //        if (readerStudentTemp.Read() == true)
            //        {
            //            int tempUID = readerStudentTemp.GetInt32(0);//用户号
            //            string tempId = readerStudentTemp.GetString(1);//卡号
            //            string tempStudentID = readerStudentTemp.GetString(2);//学号
            //            string tempName = readerStudentTemp.GetString(3);//姓名
            //            tempName = tempName.Replace(" ", ""); //删除字符串中的空格
            //            byte tempAuthority = readerStudentTemp.GetByte(4);//权限
            //            //while (readerStudentTemp.Read())
            //            //如果下位机中存在此用户号用户，且其它信息与上位机内数据不用，删除此用户重新添加
            //            if ((tempId != sourceId) || (tempStudentID != sourceStudentID) || (tempName != sourceName) || (tempAuthority != sourceAuthority))
            //            {
            //                if (deleteOnePerson(tempUID.ToString(), tempName, address) != 0x00) { break; }
            //                if (addUser(sourceUID.ToString(), sourceId, sourceStudentID, sourceName,
            //                            sourceAuthority.ToString(), "0", sourceEigen, RowCount, address) != 0x00) { break; }
            //            }

            //        }
            //        else
            //        {
            //            if (addUser(sourceUID.ToString(), sourceId, sourceStudentID, sourceName,
            //                        sourceAuthority.ToString(), "0", sourceEigen, RowCount, address) != 0x00) { break; }
            //        }
            //        readerStudentTemp.Dispose();
            //    }
            //    readerStudent.Dispose();
            //    cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

            //    sql = "SELECT * FROM studentTemp";
            //    cmdQ = new SQLiteCommand(sql, conn);
            //    SQLiteDataReader readerStudentTempReRead = cmdQ.ExecuteReader();//读取下位机数据库内人员信息
            //    while (readerStudentTempReRead.Read())
            //    {
            //        int tempUID = readerStudentTempReRead.GetInt32(0);  //用户号
            //        string tempId = readerStudentTempReRead.GetString(1);//卡号
            //        string tempStudentID = readerStudentTempReRead.GetString(2); //学号
            //        string tempName = readerStudentTempReRead.GetString(3); //姓名
            //        byte tempAuthority = readerStudentTempReRead.GetByte(4);  //权限
            //        string sqlStudentTemp = "SELECT * FROM student WHERE uID=" + tempUID.ToString();
            //        SQLiteCommand cmdStudentTemp = new SQLiteCommand(sqlStudentTemp, conn);
            //        SQLiteDataReader readerStudentReRead = cmdStudentTemp.ExecuteReader();//在上位机数据库中搜索指定用户号的用户
            //        if (readerStudentReRead.Read() == false)//若在上位机数据库中没搜索到指定用户，代表此用户已被删除
            //        {
            //            deleteOnePerson(tempUID.ToString(), tempName, address);
            //        }
            //        readerStudentReRead.Dispose();
            //    }
            //    readerStudentTempReRead.Dispose();
            //    cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生
            //    output("人员信息同步完成。");
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            //conn.Close();
            //conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
            return 0x00;
        }
        /// <summary>
        /// 获取下位机总人数 人数储存在uart.TOTAL中
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte getUserCount(byte address)
        {
            //try
            //{
            //    byte[] sendData = new byte[1];
            //    output("正在获取设备总人数!");
            //    byte state = uart.sendNByte(sendData, 0x06, address, 1, 500);
            //    uart.cleanReceiveData();//清除接收器
            //    switch (state)
            //    {
            //        case 0: { output("获取设备总人数命令发送成功!"); } break;
            //        case 1: { output("获取设备总人数命令发送超时!"); } break;
            //        case 2: { output("获取设备总人数命令超重发次数!"); } break;
            //        case 3: { output("获取设备总人数命令通信错误!"); } break;
            //    }
            //    if (state != 0) { return 0xff; }
            //    int outTimeX = 5000;
            //    while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //    {
            //        outTimeX--;
            //        Thread.Sleep(1);
            //    }
            //    uart.exchangeOrder(6, 7);
            //    uart.exchangeOrder(9, 12);
            //    uart.exchangeOrder(13, 16);
            //    uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //    uart.transformDataShort();
            //    if (outTimeX == 0) { output("获取设备总人数命令应答超时!"); return 0xff; }//如果超时，中断发送
            //    if ((uart.HEAD != uart.Z_HEAD)
            //        || (uart.ADDRESS != address)
            //        || (uart.CMD != 0x06)
            //        || (uart.BYTE != 0xff)
            //        || (uart.BLOCK != 0x0000))	//判断应答是否正确
            //    { output("获取设备总人数失败!"); return 0xff; }
            //    else
            //    {
            //        output("获取设备总人数成功!");
            //        output("设备总人数：" + uart.TOTAL.ToString());
            //    }
            //    return 0x00;
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            return 0xff;
        }
        /// <summary>
        /// 获取指定设备用户列表  address：设备地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte getUserInfo(byte address)
        {
            //SQLiteConnection conn = null;
            //string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            //conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置 
            //try
            //{ 
            //    conn.Open();//打开数据库，若文件不存在会自动创建
            //    SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
            //    cmd.CommandText = "DELETE FROM studentTemp";//删除数据库中的临时数据  
            //    cmd.ExecuteNonQuery();//执行查询
            //    cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生

            //    if (getUserCount(address) != 0x00) { return 0xff; }//获取下位机总人数
            //    uint userCount = uart.TOTAL;
            //    uint times = 1 + userCount / 4;//每次获取4人信息，计算需要几次才能获取完成
            //    uint lastTimes = userCount % 4;//计算最后一次需要获取几人
            //    if (lastTimes == 0) 
            //    { 
            //        times--;
            //        if (times != 0)
            //        { 
            //            lastTimes = 4; 
            //        }
            //    }
            //    byte[] sendData = new byte[8];
            //    for (int j = 0; j < times; j++)
            //    {
            //        string sendDataString = "";

            //        sendDataString += ((j * 4)+ 1 ).ToString().PadLeft(4, '0');//从第几人开始
            //        uint personNum;
            //        if (j != times - 1)
            //        {
            //            sendDataString += 4.ToString().PadLeft(4, '0');//获取几人的信息 
            //            personNum = 4;
            //        }
            //        else
            //        {
            //            sendDataString += lastTimes.ToString().PadLeft(4, '0');//获取几人的信息
            //            personNum = lastTimes;
            //        }
            //        int i = 0;
            //        foreach (char temp in sendDataString)
            //        {
            //            char[] tempChar = new char[1] { temp };
            //            byte[] tempByte = Encoding.GetEncoding("gbk").GetBytes(tempChar);
            //            if (temp <= 255)
            //            {
            //                sendData[i++] = tempByte[0];
            //            }
            //            else
            //            {
            //                sendData[i++] = tempByte[0];
            //                sendData[i++] = tempByte[1];
            //            }
            //        }
            //        output("正在获取下位机用户列表!");
            //        byte state = uart.sendNByte(sendData, 0x04, address, 8, 500);//获取下位机用户列表
            //        uart.cleanReceiveData();//清除接收器
            //        switch (state)
            //        {
            //            case 0: { output("获取用户列表指令发送成功!"); } break;
            //            case 1: { output("获取用户列表指令发送超时!"); } break;
            //            case 2: { output("获取用户列表指令超重发次数!"); } break;
            //            case 3: { output("获取用户列表指令通信错误!"); } break;
            //        }
            //        if (state != 0) { return 0xff; }
            //        int outTimeX = 5000;
            //        while ((uart.receiveNByte_ok != 1) && (outTimeX != 0))//等待应答数据
            //        {
            //            outTimeX--;
            //            Thread.Sleep(1);
            //        }
            //        if (outTimeX == 0) { output("获取用户列表指令应答超时!"); return 0xff; }//如果超时，中断发送
            //        else
            //        {
            //            uint personNumGet = Convert.ToUInt16(new string(uart.data, 0, 4));
            //            if (personNum != personNumGet) { output("人数校验错误!"); return 0xff; }
            //            SQLiteTransaction tran = conn.BeginTransaction();//实例化事务
            //            for (i = 0; i < personNumGet; i++)
            //            {
            //                string uID = new string(uart.data, i * 31 + 4, 4);
            //                string ID = new string(uart.data, i * 31 + 8, 8);
            //                string studentID = new string(uart.data, i * 31 + 16, 11);
            //                //将GBK编码的汉字转换为字符串
            //                byte[] bs = new byte[2];
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 27]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 28]).ToString("X"), 16);
            //                string neme = Encoding.GetEncoding("GBK").GetString(bs);
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 29]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 30]).ToString("X"), 16);
            //                neme += Encoding.GetEncoding("GBK").GetString(bs);
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 31]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[i * 31 + 32]).ToString("X"), 16);
            //                neme += Encoding.GetEncoding("GBK").GetString(bs);

            //                string authority = new string(uart.data, i * 31 + 33, 1);

            //                output("获取：" + uID + "号用户：" + neme + " 成功");
            //                cmd = new SQLiteCommand(conn);//实例化SQL命令  
            //                cmd.Transaction = tran;
            //                cmd.CommandText = "insert into studentTemp values(@uID, @id, @studentID, @name, @authority)";//设置带参SQL语句  
            //                cmd.Parameters.AddRange(new[] {//添加参数  
            //                                        new SQLiteParameter("@uID", int.Parse(uID)),  //用户号
            //                                        new SQLiteParameter("@id", ID),//卡号
            //                                        new SQLiteParameter("@studentID", studentID),//学号
            //                                        new SQLiteParameter("@name", neme),  //姓名
            //                                        new SQLiteParameter("@authority", authority),//权限
            //                                        });
            //                cmd.ExecuteNonQuery();//执行查询                                
            //            }
            //            tran.Commit();//提交
            //            tran.Dispose();//释放reader使用的资源，防止database is lock异常产生
            //            cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
            //        }         
            //    }
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            //conn.Close();
            //conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
            //return 0x00;
            return 0x00;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="ID"></param>
        /// <param name="studentID"></param>
        /// <param name="name"></param>
        /// <param name="authority"></param>
        /// <param name="status"></param>
        /// <param name="eigen"></param>
        /// <param name="personNum"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte addUser(string uID, string ID, string studentID,
                             string name, string authority, string status,
                             string eigen, int personNum, byte address)
        {
            //try 
            //{
            //    string sendDataString = string.Empty;   //要发送的字符串
            //    string eigenString = string.Empty;      //指纹特征值字符串
            //    byte[] sendData = new byte[228];        //串口发送的数据

            //    sendDataString += uID.PadLeft(4, '0');//添加用户号（4字节）
            //    sendDataString += ID.PadLeft(8, '0');//添加ID卡号（8字节）
            //    sendDataString += studentID.PadLeft(11, '0');//添加学号（11字节）
            //    if (name.Length == 2) { name = name.Insert(1, "  "); }
            //    sendDataString += name.PadLeft(3); //姓名（6字节）          
            //    sendDataString += authority.PadLeft(1, '0');//权限（1字节）
            //    sendDataString += "1";//状态（1字节）

            //    int i = 0;
            //    foreach (char temp in sendDataString)
            //    {
            //        char[] tempChar = new char[1] { temp };
            //        byte[] tempByte = Encoding.GetEncoding("gbk").GetBytes(tempChar);
            //        if (temp <= 255)
            //        {
            //            sendData[i++] = tempByte[0];
            //        }
            //        else
            //        {
            //            sendData[i++] = tempByte[0];
            //            sendData[i++] = tempByte[1];
            //        }
            //    }
            //    eigenString = eigen.Replace(" ", ""); //删除字符串中的空格
            //    for (i = 31; i < 224; i++)//添加指纹特征值
            //    {
            //        string str = eigenString.Substring((i - 31) * 2, 2);
            //        sendData[i] = Convert.ToByte(str, 16);
            //    }
            //    foreach (char temp in personNum.ToString().PadLeft(4, '0'))
            //    {
            //        char[] tempChar = new char[1] { temp };
            //        byte[] tempByte = Encoding.GetEncoding("gbk").GetBytes(tempChar);
            //        if (temp <= 255)
            //        {
            //            sendData[i++] = tempByte[0];
            //        }
            //        else
            //        {
            //            sendData[i++] = tempByte[0];
            //            sendData[i++] = tempByte[1];
            //        }
            //    }
            //    output("正在下传" + uID.PadLeft(4, '0') + "号用户" + name.PadLeft(3, '0'));
            //    byte state = uart.sendNByte(sendData, 0x01, address, 228, 500);//发送数据至下位机
            //    uart.cleanReceiveData();//清除接收器
            //    switch (state)
            //    {
            //        case 0: { output("发送成功!"); } break;
            //        case 1: { output("发送超时!"); } break;
            //        case 2: { output("超重发次数!"); } break;
            //        case 3: { output("通信错误!"); } break;
            //    }
            //    if (state != 0) { return 0xff; }
            //    int outTimeX = 5000;
            //    while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //    {
            //        outTimeX--;
            //        Thread.Sleep(1);
            //    }
            //    uart.exchangeOrder(6, 7);
            //    uart.exchangeOrder(9, 12);
            //    uart.exchangeOrder(13, 16);
            //    uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //    uart.transformDataShort();
            //    if (outTimeX == 0) { output("发送超时!"); return 0xff; }//如果超时，中断发送
            //    if ((uart.HEAD != uart.Z_HEAD)
            //        || (uart.ADDRESS != address)
            //        || (uart.CMD != 0x01)
            //        || (uart.BLOCK != 0x0000)
            //        || (uart.TOTAL != 0xFFFFFFFF))	//判断应答是否正确
            //    { output("添加指纹失败!"); return 0xff; }
            //    switch (uart.BYTE)
            //    {
            //        case 0: { output("添加指纹成功!"); } break;
            //        case 1: { output("添加指纹失败!"); } break;
            //        case 5: { output("添加指纹失败!"); } break;
            //        case 0x55: { output("打开文件失败!"); } break;
            //    }
            //    if (uart.BYTE != 0) { return 0xff; }
            //    return 0x00;
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            return 0xff;
        }
        /// <summary>
        /// 删除指定用户 uID：用户号 name：姓名 address：下位机地址
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte deleteOnePerson(string uID, string name, byte address)
        {
            //try
            //{ 
            //    uID = uID.PadLeft(4, '0');
            //    output("正在删除" + uID + "号用户" + name);
            //    string sendDataString = "";
            //    sendDataString += uID.PadLeft(4, '0');//添加用户号（4字节）
            //    if (name.Length == 2) { name = name.Insert(1, "  "); }
            //    sendDataString += name.PadLeft(3); //姓名（6字节） 
            //    int i = 0;
            //    byte[] sendData = new byte[10];
            //    foreach (char temp in sendDataString)
            //    {
            //        char[] tempChar = new char[1] { temp };
            //        byte[] tempByte = Encoding.GetEncoding("gbk").GetBytes(tempChar);
            //        if (temp <= 255)
            //        {
            //            sendData[i++] = tempByte[0];
            //        }
            //        else
            //        {
            //            sendData[i++] = tempByte[0];
            //            sendData[i++] = tempByte[1];
            //        }
            //    }
            //    byte state = uart.sendNByte(sendData, 0x02, address, 10, 500);
            //    uart.cleanReceiveData();//清除接收器
            //    switch (state)
            //    {
            //        case 0: { output("删除人员命令发送成功!"); } break;
            //        case 1: { output("删除人员命令发送超时!"); } break;
            //        case 2: { output("删除人员命令超重发次数!"); } break;
            //        case 3: { output("删除人员命令通信错误!"); } break;
            //    }
            //    if (state != 0) { return 0xff; }
            //    int outTimeX = 5000;
            //    while ((uart.receive_len < 20) && (outTimeX != 0))//等待应答数据
            //    {
            //        outTimeX--;
            //        Thread.Sleep(1);
            //    }
            //    uart.exchangeOrder(6, 7);
            //    uart.exchangeOrder(9, 12);
            //    uart.exchangeOrder(13, 16);
            //    uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //    uart.transformDataShort();
            //    if (outTimeX == 0) { output("删除人员命令应答超时!"); return 0xff; }//如果超时，中断发送
            //    if ((uart.HEAD != uart.Z_HEAD)
            //        || (uart.ADDRESS != address)
            //        || (uart.CMD != 0x02)
            //        || (uart.BLOCK != 0x0000)
            //        || (uart.TOTAL != 0xFFFFFFFF))	//判断应答是否正确
            //    { output("删除人员失败!"); return 0xff; }
            //    switch (uart.BYTE)
            //    {
            //        case 0: { output("删除人员成功!"); } break;
            //        case 1: { output("删除人员失败!"); } break;
            //        case 0x55: { output("打开文件失败!"); } break;
            //    }
            //    if (uart.BYTE != 0) { return 0xff; }
            //    return 0x00;
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            return 0xff;
        }
    }
}
