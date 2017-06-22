using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;

namespace AccessControlSystem.Model
{
    public class DeviceManagement
    {
        private string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\DeviceInfo.db";
        private DeviceInfo deviceInfo = new DeviceInfo();
        private List<DeviceInfo> deviceList = new List<DeviceInfo>();
        private TcpServer tcpServerInfo;

        /// <summary>
        /// 保存的tcp服务器的IP和端口
        /// </summary>
        public TcpServer TcpServerInfo
        {
            get { return tcpServerInfo; }
            set { tcpServerInfo = value; }
        }
        /// <summary>
        /// 设备列表
        /// </summary>
        public List<DeviceInfo> DeviceList
        {
            get { return deviceList; }
            set { deviceList = value; }
        }
        public struct DeviceInfo
        {
            public UInt32 ID;
            public string name;
            public string mac;
        }
        public struct TcpServer
        {
            public string serverIp;
            public UInt16 port;
        }
        public DeviceManagement()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"\dataBase"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\dataBase");/* 目录不存在，建立目录 */
            }
            SQLiteCommand cmdQ = new SQLiteCommand();
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            conn.Open();/* 打开数据库，若文件不存在会自动创建 */
            try
            {
                #region 初始化数据库文件并建表
                string sql = "CREATE TABLE IF NOT EXISTS device(" + /* 建表语句 */
                                "ID INTEGER," +                     /* 设备ID */
                                "mac VARCHAR(18)," +                /* MAC地址 */
                                "name VARCHAR(20));";               /* 设备名称 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                   /* 如果表不存在，创建设备列表 */

                sql = "CREATE TABLE IF NOT EXISTS tcpServer(" +     /* 建表语句 */
                                "serverIp VARCHAR(16)," +           /* 服务器IP */
                                "port INTEGER);";                   /* 端口 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                   /* 如果表不存在，创建串口配置信息表 */
                cmdQ.Dispose();                           /* 释放资源 */
                #endregion
                #region 初始化tcp server参数
                sql = "SELECT COUNT(*) FROM tcpServer";
                cmdQ = new SQLiteCommand(sql, conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());/* 测试数据库内有没有保存串口的数据 */
                if (RowCount == 0)
                {
                    SQLiteTransaction tran = conn.BeginTransaction();
                    cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                    cmdQ.Transaction = tran;
                    cmdQ.CommandText = "insert into tcpServer values(@serverIp, @port)";/* 设置带参SQL语句 */
                    cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@serverIp", "192.168.1.1"),
                                        new SQLiteParameter("@port", 2576)
                                        });
                    cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                    tran.Commit();                                   /* 提交 */
                    tran.Dispose();                                  /* 释放资源 */
                }
                #endregion
                InitList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose(); /* 释放资源 */
            conn.Close();
        }

        /// <summary>
        /// 将数据库中的数据读取到列表中
        /// </summary>
        private void InitList()
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath); /* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            deviceList.Clear();                                   /* 清空设备列表 */
            try
            {
                #region 初始化设备列表
                conn.Open();                                      /* 打开数据库，若文件不存在会自动创建 */
                string sql = "SELECT * FROM device";
                cmdQ = new SQLiteCommand(sql, conn);

                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    deviceInfo.ID = (UInt32)reader.GetInt32(0);   /* 设备ID */
                    deviceInfo.mac = reader.GetString(1);         /* MAC地址 */
                    deviceInfo.name = reader.GetString(2);        /* 设备名称 */
                    deviceList.Add(deviceInfo);
                }
                reader.Dispose();                                 /* 释放资源 */
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose();   /* 释放资源 */
            conn.Close();
        }
        public bool AddDevice(DeviceInfo device)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */

                string sql = "SELECT * FROM device";
                cmdQ = new SQLiteCommand(sql, conn);

                SQLiteDataReader reader = cmdQ.ExecuteReader();
                if (reader.Read())
                {

                }
                reader.Dispose();                                 /* 释放资源 */

                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "insert into device values(@ID, @mac, @name)";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                        new SQLiteParameter("@ID", device.ID),
                                        new SQLiteParameter("@mac", device.mac),
                                        new SQLiteParameter("@name", device.name)
                                        });
                cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                   /* 提交 */
                cmdQ.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */
                InitList();                                      /* 更新列表 */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmdQ.Dispose();                                 /* 释放资源 */
                conn.Close();
                return false;
            }
            cmdQ.Dispose(); /* 释放资源 */
            conn.Close();
            return true;
        }
        public bool DelDevice(DeviceInfo device)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "DELETE FROM device where ID = @ID and name = @name and mac = @mac";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@ID", device.ID),
                                        new SQLiteParameter("@mac", device.mac),
                                        new SQLiteParameter("@name", device.name)
                                        });
                cmdQ.ExecuteNonQuery();                           /* 执行查询 */
                tran.Commit();                                    /* 提交 */
                tran.Dispose();                                   /* 释放资源 */
                InitList();                                       /* 更新列表 */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmdQ.Dispose();                                   /* 释放资源 */
                conn.Close();
                return false;
            }
            cmdQ.Dispose(); /* 释放资源 */
            conn.Close();

            return true;
        }
        /// <summary>
        /// 由设备id获取设备名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string device_name_get (byte id)
        {
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (id == deviceList[i].ID)
                {
                    return deviceList[i].name;
                }
            }

            return "";
        }
    }
}
