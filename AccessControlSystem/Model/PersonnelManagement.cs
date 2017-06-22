using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using PresentationControls;

namespace AccessControlSystem.Model
{
    class PersonnelManagement
    {
        private string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
        private PersonInfo personInfo = new PersonInfo(5);
        private List<PersonInfo> personList = new List<PersonInfo>();
        /// <summary>
        /// 设备列表
        /// </summary>
        public List<PersonInfo> PersonList
        {
            get { return personList; }
            set { personList = value; }
        }
        public struct PersonInfo
        {
            public UInt32   uID;            /* 用户号 1-5000 */
            public UInt32   cardID;         /* RFID卡号 */
            public UInt32   activeState;    /* 激活状态 0:失效 1:有效 */
            public string   studentID;      /* 学号 */
            public string   dormitory;      /* 寝室 */
            public string   major;          /* 学院 专业 */
            public string   name;           /* 姓名 GBK编码字符串(右补0x00) */
            public string   sex;            /* 性别 男/女 */
            public string   birthday;       /* 生日 年月日 */
            public string   tel;            /* 电话 */
            public string   QQ;             /* QQ号 */
            public string   weiXin;         /* 微信号 */
            public string   authority;      /* 权限 16字节，每一位对应一个门(最多128个门) */
            public UInt32   isLimitTime;    /* 是否有时间限制 0:无限制 1:有限制 */
            public string   limitTime;      /* 有效期 年月日时分秒 */
            public UInt32[] eigenNum;       /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
            public string[] eigen;          /* 指纹特征值 最多5个指纹 */
            public string   recodeDate;     /* 登记时间 年月日时分秒 */
            public Int32    resv0;          /* 保留字段 */
            public Int32    resv1;          /* 保留字段 */
            public string   resv2;          /* 保留字段 */
            public string   resv3;          /* 保留字段 */
            public string   resv4;          /* 保留字段 */
            public PersonInfo(byte eigenLenth)
            {
                uID = 0;
                cardID = 0;
                activeState = 0;
                studentID = "";
                dormitory = "";
                major = "";
                name = "";
                sex = "";
                birthday = "";
                tel = "";
                QQ = "";
                weiXin = "";
                authority = "";
                isLimitTime = 0;
                limitTime = "";
                eigenNum = new UInt32[eigenLenth];
                eigen = new string[eigenLenth];
                recodeDate = "";
                resv0 = 0;
                resv1 = 0;
                resv2 = "";
                resv3 = "";
                resv4 = "";      
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonnelManagement()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + @"\dataBase"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\dataBase");/* 目录不存在，建立目录 */
            }
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                #region 初始化数据库文件并建表
                conn.Open();/* 打开数据库，若文件不存在会自动创建 */
                string sql = "CREATE TABLE IF NOT EXISTS personInfo(" + /* 建表语句 */
                             "uID INTEGER PRIMARY KEY," +        /* 用户号 1-5000 */
                             "cardID INTEGER," +                 /* RFID卡号 */
                             "avtiveState INTEGER," +            /* 激活状态 0:失效 1:有效 */
                             "studentID VARCHAR(20)," +          /* 学号 */
                             "dormitory VARCHAR(25)," +          /* 寝室 */
                             "major VARCHAR(30)," +              /* 学院 专业 */
                             "name VARCHAR(8)," +                /* 姓名 GBK编码字符串(右补0x00) */
                             "sex VARCHAR(2)," +                 /* 性别 男/女 */
                             "birthday VARCHAR(14)," +           /* 生日 年月日 */
                             "tel VARCHAR(11)," +                /* 电话 */
                             "QQ VARCHAR(25)," +                 /* QQ号 */
                             "weiXin VARCHAR(25)," +             /* 微信号 */
                             "authority VARCHAR(32)," +          /* 权限 16字节，每一位对应一个门(最多128个门) */
                             "isLimitTime INTEGER," +            /* 是否有时间限制 0:无限制 1:有限制 */
                             "limitTime VARCHAR(14)," +          /* 有效期 年月日时分秒 */
                             "eigenNum0 INTEGER," +              /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
                             "eigenNum1 INTEGER," +              /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
                             "eigenNum2 INTEGER," +              /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
                             "eigenNum3 INTEGER," +              /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
                             "eigenNum4 INTEGER," +              /* 指纹号 指纹特征值分别在指纹模块中的用户号 */
                             "eigen0 VARCHAR(579)," +            /* 指纹特征值 最多5个指纹 */
                             "eigen1 VARCHAR(579)," +            /* 指纹特征值 最多5个指纹 */
                             "eigen2 VARCHAR(579)," +            /* 指纹特征值 最多5个指纹 */
                             "eigen3 VARCHAR(579)," +            /* 指纹特征值 最多5个指纹 */
                             "eigen4 VARCHAR(579)," +            /* 指纹特征值 最多5个指纹 */
                             "recodeDate VARCHAR(25)," +         /* 登记时间 年月日时分秒 */
                             "resv0 INTEGER," +                  /* 保留字段 */
                             "resv1 INTEGER," +                  /* 保留字段 */
                             "resv2 VARCHAR(128)," +             /* 保留字段 */
                             "resv3 VARCHAR(256)," +             /* 保留字段 */
                             "resv4 VARCHAR(512));";             /* 保留字段 */
                cmdQ = new SQLiteCommand(sql, conn);
                cmdQ.ExecuteNonQuery();/* 如果表不存在，创建人员信息表 */
                conn.Close();
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
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            personList.Clear();                                  /* 清空列表 */
            try
            {
                #region 初始化人员列表
                conn.Open();/* 打开数据库，若文件不存在会自动创建 */
                string sql = "SELECT * FROM personInfo";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    UInt32[] eigenNum = new UInt32[personInfo.eigenNum.Length];
                    string[] eigen = new string[personInfo.eigen.Length];

                    personInfo.uID = (UInt32)reader.GetInt32(0);
                    personInfo.cardID      = (UInt32)reader.GetInt32(1);
                    personInfo.activeState = (UInt32)reader.GetInt32(2);
                    personInfo.studentID   = reader.GetString(3);
                    personInfo.dormitory   = reader.GetString(4);
                    personInfo.major       = reader.GetString(5);
                    personInfo.name        = reader.GetString(6);
                    personInfo.sex         = reader.GetString(7);
                    personInfo.birthday    = reader.GetString(8);
                    personInfo.tel         = reader.GetString(9);
                    personInfo.QQ          = reader.GetString(10);
                    personInfo.weiXin      = reader.GetString(11);
                    personInfo.authority   = reader.GetString(12);
                    personInfo.isLimitTime = (UInt32)reader.GetInt32(13);
                    personInfo.limitTime   = reader.GetString(14);
                    eigenNum[0]            = (UInt32)reader.GetInt32(15);
                    eigenNum[1]            = (UInt32)reader.GetInt32(16);
                    eigenNum[2]            = (UInt32)reader.GetInt32(17);
                    eigenNum[3]            = (UInt32)reader.GetInt32(18);
                    eigenNum[4]            = (UInt32)reader.GetInt32(19);
                    eigen[0]               = reader.GetString(20);
                    eigen[1]               = reader.GetString(21);
                    eigen[2]               = reader.GetString(22);
                    eigen[3]               = reader.GetString(23);
                    eigen[4]               = reader.GetString(24);
                    personInfo.eigenNum    = eigenNum;
                    personInfo.eigen       = eigen;
                    personInfo.recodeDate  = reader.GetString(25);
                    personInfo.resv0       = reader.GetInt32(26);
                    personInfo.resv1       = reader.GetInt32(27);
                    personInfo.resv2       = reader.GetString(28);
                    personInfo.resv3       = reader.GetString(29);
                    personInfo.resv4       = reader.GetString(30);
                    personList.Add(personInfo);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmdQ.Dispose(); /* 释放资源 */
            conn.Close();
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool AddPerson(PersonInfo person)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmd = new SQLiteCommand();             /* 实例化SQL命令 */
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmd = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmd.Transaction = tran;
                cmd.CommandText = "insert into personInfo values(@uID, @cardID, @avtiveState, @studentID, @dormitory, " +
                                  "@major, @name, @sex, @birthday, @tel, @QQ, @weiXin, @authority, @isLimitTime, @limitTime, " +
                                  "@eigenNum0, @eigenNum1, @eigenNum2, @eigenNum3, @eigenNum4, @eigen0, @eigen1, @eigen2, " +
                                  "@eigen3, @eigen4, @recodeDate, @resv0, @resv1, @resv2, @resv3, @resv4)";/* 设置带参SQL语句 */
                cmd.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@uID"        , person.uID        ),
                                        new SQLiteParameter("@cardID"     , person.cardID     ),
                                        new SQLiteParameter("@avtiveState", person.activeState),
                                        new SQLiteParameter("@studentID"  , person.studentID  ),
                                        new SQLiteParameter("@dormitory"  , person.dormitory  ),
                                        new SQLiteParameter("@major"      , person.major      ),
                                        new SQLiteParameter("@name"       , person.name       ),
                                        new SQLiteParameter("@sex"        , person.sex        ),
                                        new SQLiteParameter("@birthday"   , person.birthday   ),
                                        new SQLiteParameter("@tel"        , person.tel        ),
                                        new SQLiteParameter("@QQ"         , person.QQ         ),
                                        new SQLiteParameter("@weiXin"     , person.weiXin     ),
                                        new SQLiteParameter("@authority"  , person.authority  ),
                                        new SQLiteParameter("@isLimitTime", person.isLimitTime),
                                        new SQLiteParameter("@limitTime"  , person.limitTime  ),
                                        new SQLiteParameter("@eigenNum0"  , person.eigenNum[0]),
                                        new SQLiteParameter("@eigenNum1"  , person.eigenNum[1]),
                                        new SQLiteParameter("@eigenNum2"  , person.eigenNum[2]),
                                        new SQLiteParameter("@eigenNum3"  , person.eigenNum[3]),
                                        new SQLiteParameter("@eigenNum4"  , person.eigenNum[4]),
                                        new SQLiteParameter("@eigen0"     , person.eigen[0]   ),
                                        new SQLiteParameter("@eigen1"     , person.eigen[1]   ),
                                        new SQLiteParameter("@eigen2"     , person.eigen[2]   ),
                                        new SQLiteParameter("@eigen3"     , person.eigen[3]   ),
                                        new SQLiteParameter("@eigen4"     , person.eigen[4]   ),
                                        new SQLiteParameter("@recodeDate" , person.recodeDate ),
                                        new SQLiteParameter("@resv0"      , person.resv0      ),
                                        new SQLiteParameter("@resv1"      , person.resv1      ),
                                        new SQLiteParameter("@resv2"      , person.resv2      ),
                                        new SQLiteParameter("@resv3"      , person.resv3      ),
                                        new SQLiteParameter("@resv4"      , person.resv4      )
                                        });
                cmd.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                  /* 提交 */
                tran.Dispose();                                 /* 释放资源 */
                InitList();                                     /* 更新列表 */
            }
            catch (Exception ex)
            {
                if (ex.Message == "constraint failed\r\nUNIQUE constraint failed: personInfo.uID")
                {
                    MessageBox.Show("用户号不能与其它用户相同！！！");
                }
                else
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cmd.Dispose();                                  /* 释放资源 */
                conn.Close();
                return false;
            }
            cmd.Dispose();                                  /* 释放资源 */
            conn.Close();
            return true;
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool UpdatePerson(PersonInfo person)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmd = new SQLiteCommand();             /* 实例化SQL命令 */
            try
            {
                conn.Open();                                     /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmd = new SQLiteCommand(conn);     /* 实例化SQL命令 */
                cmd.Transaction = tran;
                cmd.CommandText = "UPDATE personInfo SET " +
                                  "cardID      = @cardID      ," +
                                  "avtiveState = @avtiveState ," +
                                  "studentID   = @studentID   ," +
                                  "dormitory   = @dormitory   ," +
                                  "major       = @major       ," +
                                  "name        = @name        ," +
                                  "sex         = @sex         ," +
                                  "birthday    = @birthday    ," +
                                  "tel         = @tel         ," +
                                  "QQ          = @QQ          ," +
                                  "weiXin      = @weiXin      ," +
                                  "authority   = @authority   ," +
                                  "isLimitTime = @isLimitTime ," +
                                  "limitTime   = @limitTime   ," +
                                  "eigenNum0   = @eigenNum0   ," +
                                  "eigenNum1   = @eigenNum1   ," +
                                  "eigenNum2   = @eigenNum2   ," +
                                  "eigenNum3   = @eigenNum3   ," +
                                  "eigenNum4   = @eigenNum4   ," +
                                  "eigen0      = @eigen0      ," +
                                  "eigen1      = @eigen1      ," +
                                  "eigen2      = @eigen2      ," +
                                  "eigen3      = @eigen3      ," +
                                  "eigen4      = @eigen4      ," +
                                  "resv0       = @resv0       ," +
                                  "resv1       = @resv1       ," +
                                  "resv2       = @resv2       ," +
                                  "resv3       = @resv3       ," +
                                  "resv4       = @resv4        " +
                                  "where uID = @uID";/* 设置带参SQL语句 */
                cmd.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@uID"        , person.uID        ),
                                        new SQLiteParameter("@cardID"     , person.cardID     ),
                                        new SQLiteParameter("@avtiveState", person.activeState),
                                        new SQLiteParameter("@studentID"  , person.studentID  ),
                                        new SQLiteParameter("@dormitory"  , person.dormitory  ),
                                        new SQLiteParameter("@major"      , person.major      ),
                                        new SQLiteParameter("@name"       , person.name       ),
                                        new SQLiteParameter("@sex"        , person.sex        ),
                                        new SQLiteParameter("@birthday"   , person.birthday   ),
                                        new SQLiteParameter("@tel"        , person.tel        ),
                                        new SQLiteParameter("@QQ"         , person.QQ         ),
                                        new SQLiteParameter("@weiXin"     , person.weiXin     ),
                                        new SQLiteParameter("@authority"  , person.authority  ),
                                        new SQLiteParameter("@isLimitTime", person.isLimitTime),
                                        new SQLiteParameter("@limitTime"  , person.limitTime  ),
                                        new SQLiteParameter("@eigenNum0"  , person.eigenNum[0]),
                                        new SQLiteParameter("@eigenNum1"  , person.eigenNum[1]),
                                        new SQLiteParameter("@eigenNum2"  , person.eigenNum[2]),
                                        new SQLiteParameter("@eigenNum3"  , person.eigenNum[3]),
                                        new SQLiteParameter("@eigenNum4"  , person.eigenNum[4]),
                                        new SQLiteParameter("@eigen0"     , person.eigen[0]   ),
                                        new SQLiteParameter("@eigen1"     , person.eigen[1]   ),
                                        new SQLiteParameter("@eigen2"     , person.eigen[2]   ),
                                        new SQLiteParameter("@eigen3"     , person.eigen[3]   ),
                                        new SQLiteParameter("@eigen4"     , person.eigen[4]   ),
                                        new SQLiteParameter("@resv0"      , person.resv0      ),
                                        new SQLiteParameter("@resv1"      , person.resv1      ),
                                        new SQLiteParameter("@resv2"      , person.resv2      ),
                                        new SQLiteParameter("@resv3"      , person.resv3      ),
                                        new SQLiteParameter("@resv4"      , person.resv4      )
                                        });
                cmd.ExecuteNonQuery();                          /* 执行查询 */
                tran.Commit();                                  /* 提交 */
                tran.Dispose();                                 /* 释放资源 */
                InitList();                                     /* 更新列表 */
            }
            catch (Exception ex)
            {
                if (ex.Message == "constraint failed\r\nUNIQUE constraint failed: personInfo.uID")
                {
                    MessageBox.Show("用户号不能与其它用户相同！！！");
                }
                else
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cmd.Dispose();                                  /* 释放资源 */
                conn.Close();
                return false;
            }
            cmd.Dispose();                                  /* 释放资源 */
            conn.Close();
            return true;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool DelPerson(PersonInfo person)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);/* 创建数据库实例，指定文件位置 */
            SQLiteCommand cmdQ = new SQLiteCommand();
            try
            {
                conn.Open();                                      /* 打开数据库，若文件不存在会自动创建 */
                SQLiteTransaction tran = conn.BeginTransaction();
                cmdQ = new SQLiteCommand(conn);                   /* 实例化SQL命令 */
                cmdQ.Transaction = tran;
                cmdQ.CommandText = "DELETE FROM personInfo where uID = @uID and name = @name";/* 设置带参SQL语句 */
                cmdQ.Parameters.AddRange(new[] {                  /* 添加参数 */
                                        new SQLiteParameter("@uID" , person.uID),
                                        new SQLiteParameter("@name", person.name),
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
        /// 获得用户ID在列表中的索引，成功返回索引，失败返回-1
        /// </summary>
        /// <param name="uID"></param>
        /// <returns></returns>
        public Int32 uIDtoIndex(UInt32 uID)
        {
            for (int idx = 0; idx < personList.Count; idx++)
            {
                if (personList[idx].uID == uID) { return idx; }
            }
            return -1;
        }
        /// <summary>
        /// 获取最大用户号
        /// </summary>
        /// <returns></returns>
        public UInt32 max_user_id_get()
        {
            UInt32 max = 0;

            for (int i = 0; i < personList.Count; i++)
            {
                if (personList[i].uID > max)
                {
                    max = personList[i].uID;
                }
            }

            return max;
        }
        /// <summary>
        /// 搜索指纹号是否已经使用
        /// </summary>
        /// <param name="finger_id"></param>
        /// <returns></returns>
        public bool finger_id_check(UInt32 finger_id)
        {
            for (int i = 0; i < personList.Count; i++)
            {
                for (int j = 0; j < personList[i].eigenNum.Length; j++)
                {
                    if (personList[i].eigenNum[j] == finger_id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 搜索用户号是否已经使用
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public bool user_id_check(UInt32 user_id)
        {
            for (int i = 0; i < personList.Count; i++)
            {
                if (personList[i].uID == user_id)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 获取指定用户号的用户信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public PersonInfo user_get(UInt32 user_id)
        {
            for (int i = 0; i < personList.Count; i++)
            {
                if (personList[i].uID == user_id)
                {
                    return personList[i];
                }
            }

            return new PersonInfo(5);;
        }
    }
}
