using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AccessControlSystem.Model
{
    class SchoolInfo
    {
        private string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\SchoolInfo.db";
        private BuildingInfo buildingInfo = new BuildingInfo();
        private List<BuildingInfo> buildingList = new List<BuildingInfo>();
        private InstituteInfo instituteInfo = new InstituteInfo();
        private List<InstituteInfo> instituteList = new List<InstituteInfo>();

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<BuildingInfo> BuildingList
        {
            get { return buildingList; }
            set { buildingList = value; }
        }
        /// <summary>
        /// 设备列表
        /// </summary>
        public List<InstituteInfo> InstituteList
        {
            get { return instituteList; }
            set { instituteList = value; }
        }
        public struct BuildingInfo
        {
            public UInt32 num;     /* 宿舍楼号 */
            public string name;    /* 宿舍名称 */
            public string[] floor; /* 宿舍楼对应单元 */
        }
        public struct InstituteInfo
        {
            public UInt32 num;     /* 学院序号 */
            public string name;    /* 学院名称 */
            public string[] major; /* 学院的专业 */
        }
        public SchoolInfo()
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
                string sql = "CREATE TABLE IF NOT EXISTS building(" + /* 建表语句 */
                                "num INTEGER," +                      /* 宿舍楼号 */
                                "name VARCHAR(20));";                 /* 宿舍名称 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                   /* 如果表不存在，创建宿舍信息表 */

                sql = "CREATE TABLE IF NOT EXISTS institute(" +     /* 建表语句 */
                                "num INTEGER," +                    /* 学院序号 */
                                "name VARCHAR(30));";               /* 学院名称 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                   /* 如果表不存在，创建学院信息表 */
                cmdQ.Dispose();                           /* 释放资源 */
                #endregion
                InitBuildingList();
                InitInstituteList();
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
        private void InitBuildingList()
        {
            int index = 0;
            List<BuildingInfo> buildingListTemp = new List<BuildingInfo>();
            SQLiteConnection conn = new SQLiteConnection(dbPath); /* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            buildingList.Clear();                                 /* 清空宿舍列表 */
            try
            {
                #region 初始化宿舍列表
                conn.Open();                                      /* 打开数据库，若文件不存在会自动创建 */
                string sql = "SELECT * FROM building";
                cmdQ = new SQLiteCommand(sql, conn);

                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    buildingInfo.num = (UInt32)reader.GetInt32(0);  /* 宿舍楼号 */
                    buildingInfo.name = reader.GetString(1);        /* 宿舍名称 */
                    buildingListTemp.Add(buildingInfo);
                }
                reader.Dispose();                                   /* 释放资源 */

                int buildingCount = buildingListTemp.Count;
                for (int idx = 0; idx < buildingCount; idx++)
                {
                    int floorCount = getDataCount(buildingListTemp[idx].name);
                    if (floorCount < 0) { continue; }
                    buildingInfo.floor = new string[floorCount];
                    buildingInfo.num = buildingListTemp[idx].num;
                    buildingInfo.name = buildingListTemp[idx].name;

                    sql = "SELECT * FROM " + buildingListTemp[idx].name;
                    cmdQ = new SQLiteCommand(sql, conn);

                    reader = cmdQ.ExecuteReader();
                    index = 0;
                    while (reader.Read())
                    {
                        buildingInfo.floor[index++] = reader.GetString(0); /* 单元名称 */
                    }
                    reader.Dispose();                                      /* 释放资源 */
                    buildingList.Add(buildingInfo);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose();   /* 释放资源 */
            conn.Close();
        }
        /// <summary>
        /// 将数据库中的数据读取到列表中
        /// </summary>
        private void InitInstituteList()
        {
            int index = 0;
            List<InstituteInfo> instituteListTemp = new List<InstituteInfo>();
            SQLiteConnection conn = new SQLiteConnection(dbPath); /* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            instituteList.Clear();                                 /* 清空宿舍列表 */
            try
            {
                #region 初始化宿舍列表
                conn.Open();                                      /* 打开数据库，若文件不存在会自动创建 */
                string sql = "SELECT * FROM institute";
                cmdQ = new SQLiteCommand(sql, conn);

                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    instituteInfo.num = (UInt32)reader.GetInt32(0);  /* 学院序号 */
                    instituteInfo.name = reader.GetString(1);        /* 学院名称 */
                    instituteListTemp.Add(instituteInfo);
                }
                reader.Dispose();                                   /* 释放资源 */

                int buildingCount = instituteListTemp.Count;
                for (int idx = 0; idx < buildingCount; idx++)
                {
                    int floorCount = getDataCount(instituteListTemp[idx].name);
                    if (floorCount < 0) { continue; }
                    instituteInfo.major = new string[floorCount];
                    instituteInfo.num = instituteListTemp[idx].num;
                    instituteInfo.name = instituteListTemp[idx].name;

                    sql = "SELECT * FROM " + instituteListTemp[idx].name;
                    cmdQ = new SQLiteCommand(sql, conn);

                    reader = cmdQ.ExecuteReader();
                    index = 0;
                    while (reader.Read())
                    {
                        instituteInfo.major[index++] = reader.GetString(0); /* 专业名称 */
                    }
                    reader.Dispose();                                      /* 释放资源 */
                    instituteList.Add(instituteInfo);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose();   /* 释放资源 */
            conn.Close();
        }
        public bool AddBuliding(BuildingInfo buildingInfo)
        {
            if ((buildingInfo.floor == null) || (buildingInfo.floor.Length == 0))
            {
                MessageBox.Show("单元为空！");
                return false;
            }

            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "insert into building values(@num, @name)";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                        new SQLiteParameter("@num", buildingInfo.num),
                                        new SQLiteParameter("@name", buildingInfo.name)
                                        });
                cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                   /* 提交 */
                cmdQ.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */

                string sql = "CREATE TABLE IF NOT EXISTS " + buildingInfo.name + "(" + /* 建表语句 */
                                "name VARCHAR(30));";            /* 单元名称 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                          /* 如果表不存在，创建单元表 */
                for (int idx = 0; idx < buildingInfo.floor.Length; idx++)
                {
                    tran = conn.BeginTransaction();
                    cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                    cmdQ.Transaction = tran;
                    cmdQ.CommandText = "insert into " + buildingInfo.name + " values(@name)";/* 设置带参SQL语句 */
                    cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                        new SQLiteParameter("@name", buildingInfo.floor[idx])
                                        });
                    cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                    tran.Commit();                                   /* 提交 */
                    cmdQ.Dispose();                                  /* 释放资源 */
                    tran.Dispose();                                  /* 释放资源 */
                    tran.Dispose();                                  /* 释放资源 */
                }
                InitBuildingList();                              /* 更新列表 */
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
        public bool AddInstitute(InstituteInfo instituteInfo)
        {
            if ((instituteInfo.major == null) || (instituteInfo.major.Length == 0))
            {
                MessageBox.Show("专业为空！");
                return false;
            }

            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "insert into institute values(@num, @name)";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                        new SQLiteParameter("@num", instituteInfo.num),
                                        new SQLiteParameter("@name", instituteInfo.name)
                                        });
                cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                   /* 提交 */
                cmdQ.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */
                tran.Dispose();                                  /* 释放资源 */

                string sql = "CREATE TABLE IF NOT EXISTS " + instituteInfo.name + "(" + /* 建表语句 */
                                "name VARCHAR(30));";            /* 学院名称 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                          /* 如果表不存在，创建单元表 */
                for (int idx = 0; idx < instituteInfo.major.Length; idx++)
                {
                    tran = conn.BeginTransaction();
                    cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                    cmdQ.Transaction = tran;
                    cmdQ.CommandText = "insert into " + instituteInfo.name + " values(@name)";/* 设置带参SQL语句 */
                    cmdQ.Parameters.AddRange(new[] {                 /* 添加参数 */
                                        new SQLiteParameter("@name", instituteInfo.major[idx])
                                        });
                    cmdQ.ExecuteNonQuery();                          /* 执行查询 */
                    tran.Commit();                                   /* 提交 */
                    cmdQ.Dispose();                                  /* 释放资源 */
                    tran.Dispose();                                  /* 释放资源 */
                    tran.Dispose();                                  /* 释放资源 */
                }
                InitInstituteList();                              /* 更新列表 */
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
        public bool DelBuliding(BuildingInfo buildingInfo)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "DELETE FROM building where num = @num and name = @name";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@num", buildingInfo.num),
                                        new SQLiteParameter("@name", buildingInfo.name)
                                        });
                cmdQ.ExecuteNonQuery();                           /* 执行查询 */
                tran.Commit();                                    /* 提交 */
                tran.Dispose();                                   /* 释放资源 */

                string sql = "DROP TABLE " + buildingInfo.name;   /* 建表语句 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                           /* 删除表 */
                InitBuildingList();                               /* 更新列表 */
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
        public bool DelInstitute(InstituteInfo instituteInfo)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "DELETE FROM institute where num = @num and name = @name";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@num", instituteInfo.num),
                                        new SQLiteParameter("@name", instituteInfo.name)
                                        });
                cmdQ.ExecuteNonQuery();                           /* 执行查询 */
                tran.Commit();                                    /* 提交 */
                tran.Dispose();                                   /* 释放资源 */

                string sql = "DROP TABLE " + instituteInfo.name;  /* 建表语句 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();                           /* 删除表 */
                InitBuildingList();                               /* 更新列表 */
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
        /// 获取表内数据条数
        /// </summary>
        /// <returns></returns>
        private int getDataCount(string sheet)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath); /* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            int RowCount = 0;
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM " + sheet;
                cmdQ = new SQLiteCommand(sql, conn);
                RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   /* 获取数据条数 */
            }
            catch (Exception ex)
            {
                RowCount = -1;
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose();   /* 释放资源 */
            conn.Close();
            conn.Dispose();   /* 释放资源 */
            return RowCount;
        }
    }
}
