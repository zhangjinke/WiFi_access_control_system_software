using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;

namespace AccessControlSystem.Model
{
    public class AttendanceInfo
    {
        private string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";

        public struct AttInfo
        {
            public byte   deviceID;       /* 设备ID */
            public string deviceName;     /* 设备名称 */
            public string mac;            /* 设备mac地址 */
            public UInt32 uID;            /* 用户号 */
            public string studentID;      /* 学号 */
            public string name;           /* 姓名 */
            public char   state;          /* 状态 0:出门 1:进门 */
            public string time;           /* 时间 */
        }

        public AttendanceInfo()
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
                string sql = "CREATE TABLE IF NOT EXISTS attendanceRecord(" + /* 建表语句 */
                             "deviceID INTEGER," +                    /* 设备ID */
                             "deviceName VARCHAR(14)," +              /* 设备名称 */
                             "mac VARCHAR(14)," +                     /* 设备mac地址 */
                             "uID INTEGER," +                         /* 用户号 */
                             "studentID VARCHAR(20)," +               /* 学号 */
                             "name VARCHAR(14)," +                    /* 姓名 */
                             "state INTEGER," +                       /* 状态 */
                             "time VARCHAR(14));";                    /* 时间 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                   /* 如果表不存在，创建考勤信息表 */

                cmdQ.Dispose();                           /* 释放资源 */
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose(); /* 释放资源 */
            conn.Close();
        }

        public bool AddInfo(AttInfo info)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "insert into attendanceRecord values(@deviceID, @deviceName, @mac, @uID, @studentID, @name, @state, @time)";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                         new SQLiteParameter("@deviceID", info.deviceID),
                                         new SQLiteParameter("@deviceName", info.deviceName),
                                         new SQLiteParameter("@mac", info.mac),
                                         new SQLiteParameter("@uID", info.uID),
                                         new SQLiteParameter("@studentID", info.studentID),
                                         new SQLiteParameter("@name", info.name),
                                         new SQLiteParameter("@state", info.state),
                                         new SQLiteParameter("@time", info.time)
                                         });
                cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                   /* 提交 */
                cmdQ.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */
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
        public bool DelInfo(AttInfo info)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "DELETE FROM attendanceRecord where " + 
                "deviceID = @deviceID and " + 
                "deviceName = @deviceName and " + 
                "mac = @mac and " + 
                "uID = @uID and " + 
                "studentID = @studentID and " + 
                "name = @name and " + 
                "state = @state and " + 
                "time = @time";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                         new SQLiteParameter("@deviceID", info.deviceID),
                                         new SQLiteParameter("@deviceName", info.deviceName),
                                         new SQLiteParameter("@mac", info.mac),
                                         new SQLiteParameter("@uID", info.uID),
                                         new SQLiteParameter("@studentID", info.studentID),
                                         new SQLiteParameter("@name", info.name),
                                         new SQLiteParameter("@state", info.state),
                                         new SQLiteParameter("@time", info.time)
                                        });
                cmdQ.ExecuteNonQuery();                           /* 执行查询 */
                tran.Commit();                                    /* 提交 */
                tran.Dispose();                                   /* 释放资源 */
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
    }
}
