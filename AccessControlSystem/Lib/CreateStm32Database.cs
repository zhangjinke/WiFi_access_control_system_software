using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AccessControlSystem.Model;

namespace AccessControlSystem.Lib
{
    class CreateStm32Database
    {
        static UInt32 USER_PACK_SIZE = 1058; /**< \brief 下位机人员结构大小 */

        Stm32_crc stm32_crc = new Stm32_crc();
        public void CreateMemberBin()
        {
            Stm32_crc stm32_crc = new Stm32_crc();
            PersonnelManagement personnelManagement = new PersonnelManagement();
            PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);
            string path = Environment.CurrentDirectory + @"\dataBase\member.bin";
            FileStream fs = new FileStream(path, FileMode.Create);
            UInt32 crc = 0;

            /* 获取总人数 */
            ushort rowsCount = (ushort)personnelManagement.PersonList.Count;

            /* 获取最大用户号 */
            ushort user_num_max = (ushort)personnelManagement.max_user_id_get();
            uint one_user_lenth = USER_PACK_SIZE;

            byte[] data = new byte[8 + user_num_max * one_user_lenth];
            byte[] person_array;
            data[0] = (byte)(rowsCount & 0xff);
            data[1] = (byte)((rowsCount & 0xff00) >> 8);
            data[2] = (byte)(user_num_max & 0xff);
            data[3] = (byte)((user_num_max & 0xff00) >> 8);

            crc = stm32_crc.block_crc_calc(data, 0, 4);
            data[4] = (byte)((crc & 0x000000FF) >> 0);
            data[5] = (byte)((crc & 0x0000FF00) >> 8);
            data[6] = (byte)((crc & 0x00FF0000) >> 16);
            data[7] = (byte)((crc & 0xFF000000) >> 24);

            for (ushort i = 0; i < rowsCount; i++)
            {
                person = personnelManagement.PersonList[i];
                person_array = person_array_get(person);
                Buffer.BlockCopy(person_array, 0, data, (int)((person.uID - 1) * one_user_lenth + 8), person_array.Length);
            }
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        public byte[] person_array_get(PersonnelManagement.PersonInfo PersonInfo)
        {
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            byte[] user_data = new byte[1 + USER_PACK_SIZE];
            byte[] data = new byte[USER_PACK_SIZE];
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
            user_data[1039] = (byte)((crc & 0x000000FF) >> 0);
            user_data[1040] = (byte)((crc & 0x0000FF00) >> 8);
            user_data[1041] = (byte)((crc & 0x00FF0000) >> 16);
            user_data[1042] = (byte)((crc & 0xFF000000) >> 24);

            /* 进出状态 */
            temp = new byte[16];
            Buffer.BlockCopy(temp, 0, user_data, 1043, temp.Length);

            Buffer.BlockCopy(user_data, 1, data, 0, data.Length);

            return data;
        }
    }
}
