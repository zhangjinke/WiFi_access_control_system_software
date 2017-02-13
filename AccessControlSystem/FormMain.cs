using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;
using System.IO;
using AccessControlSystem.Lib;
using AccessControlSystem.Model;

namespace AccessControlSystem
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            //byte[] xx = new byte[259];
            //uart.sendNByte(xx, 0x56, 0x00, 259, 500);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PersonnelManagement personnelManagement = new PersonnelManagement();
            PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);

            person.uID         = 0;
            person.cardID      = 0xFFFFFFFF;
            person.avtiveState = 0;
            person.studentID   = "11307030328";
            person.dormitory   = "柏轩 B414";
            person.major       = "机械工程学院 测控技术与仪器";
            person.name        = "张进科";
            person.sex         = "男";
            person.birthday    = "1995年02月23日";
            person.tel         = "15825941073";
            person.QQ          = "799658861";
            person.weiXin      = "zhangjinke0220";
            person.authority   = "authority";
            person.isLimitTime = 0;
            person.limitTime   = "2099年12月31日23时59分59秒";
            person.eigenNum[0] = 0;
            person.eigenNum[1] = 1;
            person.eigenNum[2] = 2;
            person.eigenNum[3] = 3;
            person.eigenNum[4] = 4;
            person.eigen[0]    = "eigen0";
            person.eigen[1]    = "eigen1";
            person.eigen[2]    = "eigen2";
            person.eigen[3]    = "eigen3";
            person.eigen[4]    = "eigen4";
            person.recodeDate = "2017年2月3日17时49分26秒";
            person.resv0       = 0;
            person.resv1       = 0;
            person.resv2       = "";
            person.resv3       = "";
            person.resv4       = "";
            personnelManagement.AddPerson(person);
            personnelManagement.DelPerson(person);

            Control.CheckForIllegalCrossThreadCalls = false;        //不检查跨线程的调用是否合法（不好的解决方案）
            if (!Directory.Exists(Environment.CurrentDirectory + @"\dataBase"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\dataBase");//目录不存在，建立目录
            }
            if (!Directory.Exists(Environment.CurrentDirectory + @"\picture"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\picture");//目录不存在，建立目录
            }
            createDataBase();//创建数据库

            DeviceManagement deviceManagement = new DeviceManagement();
            dgvDeviceInfo.Rows.Clear();                      /* 清空表格 */
            deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 设备数量 */
            for (int idx = 0; idx < count; idx++)          /* 将设备添加到表格中 */
            {
                int index = dgvDeviceInfo.Rows.Add();
                dgvDeviceInfo.Rows[index].Cells[0].Value = deviceManagement.DeviceList[idx].name;  //设备名称
                dgvDeviceInfo.Rows[index].Cells[1].Value = "未连接";  //状态
                dgvDeviceInfo.Rows[index].Cells[2].Value = deviceManagement.DeviceList[idx].mac;  //机器号
                dgvDeviceInfo.Rows[index].Cells[3].Value = "";  //人员数
            }
        }

        private void createDataBase()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "CREATE TABLE IF NOT EXISTS student(" +    //建表语句 
                                 "uID INTEGER PRIMARY KEY," +             //用户号(主键约束)
                                 "name VARCHAR(6)," +                    //姓名
                                 "sex VARCHAR(2)," +                     //性别
                                 "studentID VARCHAR(20)," +              //学号
                                 "birthday VARCHAR(20)," +               //生日
                                 "recodeDate VARCHAR(20)," +             //录入日期
                                 "qq VARCHAR(20)," +                     //QQ号
                                 "tel VARCHAR(20)," +                    //电话号码
                                 "dormitory VARCHAR(20)," +              //寝室
                                 "authority TINYINT," +                  //权限
                                 "id VARCHAR(20)," +                     //卡号
                                 "eigen VARCHAR(200));";                 //指纹特征值
                SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建人员信息表  

                sql = "CREATE TABLE IF NOT EXISTS studentTemp(" +    //建表语句 
                                "uID INTEGER," +                     //用户号
                                "id VARCHAR(20)," +                  //卡号
                                "studentID VARCHAR(20)," +           //学号
                                "name VARCHAR(6)," +                 //姓名
                                "authority TINYINT);";               //权限
                cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建下位机人员信息缓存表 
                conn.Close();

                dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";
                conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
                conn.Open();//打开数据库，若文件不存在会自动创建  

                sql = "CREATE TABLE IF NOT EXISTS attendanceRecord(" +    //建表语句 
                                "deviceName VARCHAR(14)," +              //设备名称
                                "uID INTEGER," +                         //用户号
                                "studentID VARCHAR(20)," +               //学号
                                "userName VARCHAR(14)," +                //姓名
                                "state VARCHAR(20)," +                   //状态
                                "time VARCHAR(14));";                    //时间
                cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建串口配置信息表  
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }
        private void 人员维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerConnectAndGet.Enabled = false;//停止获取设备考勤记录
            FormPersonnelManagement formPersonnelManagement = new FormPersonnelManagement();
            int rowsCount = dgvDeviceInfo.Rows.Count;
            for(int i=0;i<rowsCount;i++)
            {
                if (dgvDeviceInfo.Rows[i].Cells[1].Value.ToString() == "已连接")
                {
                    string deviceName = dgvDeviceInfo.Rows[i].Cells[0].Value.ToString();  //设备名称
                    string address = dgvDeviceInfo.Rows[i].Cells[2].Value.ToString();  //机器号
                    formPersonnelManagement.cbDeviceList.Items.Add(deviceName + " 机器号：" + address);              
                }
            }
            formPersonnelManagement.cbDeviceList.Items.Add("全部设备");
            formPersonnelManagement.cbDeviceList.SelectedIndex = 0;
            formPersonnelManagement.ShowDialog();
        }

        private void 设备管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDeviceManagement formDeviceManagement = new FormDeviceManagement();
            formDeviceManagement.ShowDialog();
        }

        private void 连接设备toolStripButton_Click(object sender, EventArgs e)
        {
            Thread ConnectDevice = new Thread(connectDevice);
            ConnectDevice.Priority = ThreadPriority.Highest;
            ConnectDevice.Start();        
        }
        private void connectDevice()
        {
            //连接设备toolStripButton.Enabled = false;
            //if (uart.serialPort.IsOpen == false)
            //{
            //    MessageBox.Show("请打开串口");
            //    连接设备toolStripButton.Enabled = true;
            //    return;
            //}
            //try
            //{
            //    int rows = dgvDeviceInfo.SelectedRows.Count;//获取选中总行数
            //    for (int j = 0; j < rows; j++)
            //    {                                     
            //        for (byte i = 0; i < 1; i++)
            //        {
            //            byte address = Convert.ToByte(dgvDeviceInfo.SelectedRows[j].Cells[2].Value.ToString());
            //            string deviceName = dgvDeviceInfo.SelectedRows[j].Cells[0].Value.ToString();
            //            byte[] none = new byte[1];
            //            output("正在连接设备：" + deviceName);
            //            byte state = uart.sendNByte(none, 0x03, address, 1, 500);
            //            uart.cleanReceiveData();//清除接收器
            //            switch (state)
            //            {
            //                case 0: { output("发送命令成功，等待设备应答···"); } break;
            //                case 1: { output("发送命令超时!!!"); } break;
            //                case 2: { output("超重发次数!!!"); } break;
            //                case 3: { output("通信地址校验错误!!!"); } break;
            //            }
            //            if (state != 0) { dgvDeviceInfo.SelectedRows[j].Cells[1].Value = "未连接"; break; }
            //            int outTimeX = 5000;
            //            while ((uart.receive_len != 21) && (outTimeX != 0))//等待应答数据
            //            {
            //                outTimeX--;
            //                Thread.Sleep(1);
            //            }
            //            uart.exchangeOrder(6, 7);
            //            uart.exchangeOrder(9, 12);
            //            uart.exchangeOrder(13, 16);
            //            uart.exchangeOrder(17, 20);	//采用高位在前发送，所以需要将高低位交换
            //            uart.transformDataShort();
            //            if (outTimeX == 0) { output("应答超时!!!"); dgvDeviceInfo.SelectedRows[j].Cells[1].Value = "未连接"; break; }//如果超时，中断发送
            //            if ((uart.HEAD != uart.Z_HEAD)
            //                || (uart.ADDRESS != address)
            //                || (uart.CMD != 0x03)
            //                || (uart.TOTAL != 0xFFFFFFFF))	//判断应答是否正确
            //            { 
            //                output("应答校验错误!!!");
            //                dgvDeviceInfo.SelectedRows[j].Cells[1].Value = "未连接"; 
            //                break; 
            //            }
            //            else
            //            {
            //                output("连接设备成功!!!");
            //                dgvDeviceInfo.SelectedRows[j].Cells[1].Value = "已连接";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //连接设备toolStripButton.Enabled = true;
        }
        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="log"></param>
        private void output(string log)
        {
            if (tbLog.Text != "") { tbLog.Text += "\r\n"; }
            tbLog.Text += DateTime.Now.ToString("HH:mm:ss ") + log;
            tbLog.Select(tbLog.Text.Length, 0);//将光标设置到最末尾
            tbLog.ScrollToCaret();  //将滚动条设置到光标处
        }
        /// <summary>
        /// 清除日志信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清除日志信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbLog.Text = "";
        }

        /// <summary>
        /// 刷新主窗体中的表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Activated(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;

            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\DeviceInfo.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.Open();//打开数据库，若文件不存在会自动创建  
            try
            {
                string sql = "SELECT * FROM device";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                dgvDeviceInfo.Rows.Clear();//清空表格
                while (reader.Read())
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    int index = dgvDeviceInfo.Rows.Add();
                    dgvDeviceInfo.Rows[index].Cells[0].Value = reader.GetString(1);  //设备名称
                    dgvDeviceInfo.Rows[index].Cells[1].Value = "未连接";  //状态
                    dgvDeviceInfo.Rows[index].Cells[2].Value = reader.GetInt32(0).ToString();  //机器号
                    dgvDeviceInfo.Rows[index].Cells[3].Value = "";  //人员数
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }
        /// <summary>
        /// 断开选择的设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 断开设备toolStripButton_Click(object sender, EventArgs e)
        {
            int rows = dgvDeviceInfo.SelectedRows.Count;//获取选中总行数
            for (int j = 0; j < rows; j++)
            {
                if (dgvDeviceInfo.SelectedRows[j].Cells[1].Value.ToString() == "未连接")
                {
                    output("设备" + dgvDeviceInfo.SelectedRows[j].Cells[0].Value.ToString() + "未连接");
                }
                else
                { 
                   dgvDeviceInfo.SelectedRows[j].Cells[1].Value = "未连接";               
                }
            }
        }
        /// <summary>
        /// 创建同步所有设备时间的任务线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同步设备时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread syncTime = new Thread(SyncTime);
            syncTime.Priority = ThreadPriority.Highest;
            syncTime.Start();
        }

        /// <summary>
        /// 同步所有已连接设备的时间
        /// </summary>
        private void SyncTime()
        {
            //同步设备时间ToolStripMenuItem.Enabled = false;
            //同步设备时间toolStripButton.Enabled = false;
            //if (uart.serialPort.IsOpen == false)
            //{
            //    MessageBox.Show("请打开串口");
            //    同步设备时间ToolStripMenuItem.Enabled = true;
            //    同步设备时间toolStripButton.Enabled = true;
            //    return;
            //}
            //try
            //{
            //    //获取需要更新的设备数
            //    int deviceNum = dgvDeviceInfo.RowCount;
            //    for (int j = 0; j < deviceNum; j++)
            //    {
            //        byte address = Convert.ToByte(dgvDeviceInfo.Rows[j].Cells[2].Value);//设备地址
            //        string deviceName = dgvDeviceInfo.Rows[j].Cells[0].Value.ToString();//设备名称
            //        //将设备中的人员信息存到数据库中
            //        if (dgvDeviceInfo.Rows[j].Cells[1].Value.ToString() != "已连接")
            //        {
            //            output("设备：" + deviceName + "未连接");
            //            continue;
            //        }
            //        syncDeviceTime(address);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //同步设备时间ToolStripMenuItem.Enabled = true;
            //同步设备时间toolStripButton.Enabled = true;
        }

        /// <summary>
        /// 同步指定设备时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte syncDeviceTime(byte address)
        {
            //try
            //{
            //    string sendDataString = string.Empty;   //要发送的字符串
            //    string eigenString = string.Empty;      //指纹特征值字符串
            //    byte[] sendData = new byte[14];        //串口发送的数据

            //    sendDataString += DateTime.Now.Year.ToString().PadLeft(4, '0');//年（4字节）
            //    sendDataString += DateTime.Now.Month.ToString().PadLeft(2, '0');//月（2字节）
            //    sendDataString += DateTime.Now.Day.ToString().PadLeft(2, '0');//日（2字节）
            //    sendDataString += DateTime.Now.Hour.ToString().PadLeft(2, '0'); //时（2字节）          
            //    sendDataString += DateTime.Now.Minute.ToString().PadLeft(2, '0');//分（2字节）
            //    sendDataString += DateTime.Now.Second.ToString().PadLeft(2, '0');//秒（2字节）

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
            //    output("正在与机器号为：" + address.ToString() + "的设备同步时间");
            //    byte state = uart.sendNByte(sendData, 0x07, address, 14, 500);//发送数据至下位机
            //    uart.cleanReceiveData();//清除接收器
            //    switch (state)
            //    {
            //        case 0: { output("下传时间数据成功!!!"); } break;
            //        case 1: { output("下传时间数据超时!!!"); } break;
            //        case 2: { output("下传时间数据超重发次数!!!"); } break;
            //        case 3: { output("下传时间数据通信错误!!!"); } break;
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
            //    if (outTimeX == 0) { output("下传时间数据应答超时!!!"); return 0xff; }//如果超时，中断发送
            //    if ((uart.HEAD != uart.Z_HEAD)
            //        || (uart.ADDRESS != address)
            //        || (uart.CMD != 0x07)
            //        || (uart.BYTE != 0x00)
            //        || (uart.TOTAL != 0xFFFFFFFF))	//判断应答是否正确
            //    { output("同步时间失败!!!"); return 0xff; }
            //    output("同步时间成功!!!");
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
        /// 定时查看设备是否在线并获取考勤信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerConnectAndGet_Tick(object sender, EventArgs e)
        {
            //if (uart.serialPort.IsOpen == true)
            //{
            //    if ((连接设备toolStripButton.Enabled != false) || (同步设备时间ToolStripMenuItem.Enabled != false))
            //    {
            //        连接设备toolStripButton.Enabled = false;
            //        断开设备toolStripButton.Enabled = false;
            //        同步设备时间ToolStripMenuItem.Enabled = false;
            //        人员维护toolStripButton.Enabled = false;
            //        人员维护ToolStripMenuItem.Enabled = false;
            //        Thread getAttendanceInfo = new Thread(getAllAttendanceInfo);
            //        getAttendanceInfo.Priority = ThreadPriority.Highest;
            //        getAttendanceInfo.Start();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("请打开串口");
            //}
        }
        /// <summary>
        /// 获取所有已连接的设备的考勤记录
        /// </summary>
        private void getAllAttendanceInfo()
        {
            //connectDevice();
            int rowsCount = dgvDeviceInfo.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                if (dgvDeviceInfo.Rows[i].Cells[1].Value.ToString() == "已连接")
                {
                    byte address = Convert.ToByte(dgvDeviceInfo.Rows[i].Cells[2].Value.ToString());
                    getOneAttendanceInfo(address);
                }
            }
            连接设备toolStripButton.Enabled = true;
            断开设备toolStripButton.Enabled = true;
            同步设备时间ToolStripMenuItem.Enabled = true;
            人员维护toolStripButton.Enabled = true;
            人员维护ToolStripMenuItem.Enabled = true;
        }
        /// <summary>
        /// 获取指定设备的考勤记录
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private byte getOneAttendanceInfo(byte address)
        {
            //SQLiteConnection conn = null;
            //string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";
            //conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            //try
            //{
            //    conn.Open();//打开数据库，若文件不存在会自动创建
            //    while(true)
            //    { 
            //        byte[] sendData = new byte[1];
            //        output("正在获取设备内考勤信息!");
            //        byte state = uart.sendNByte(sendData, 0x08, address, 1, 500);//获取下位机考勤信息
            //        uart.cleanReceiveData();//清除接收器
            //        switch (state)
            //        {
            //            case 0: { output("获取设备考勤信息指令发送成功!"); } break;
            //            case 1: { output("获取设备考勤信息指令发送超时!"); } break;
            //            case 2: { output("获取设备考勤信息指令超重发次数!"); } break;
            //            case 3: { output("获取设备考勤信息指令通信错误!"); } break;
            //        }
            //        if (state != 0) 
            //        { return 0xff; }
            //        int outTimeX = 6000;
            //        while ((uart.receiveNByte_ok != 1) && (outTimeX != 0))//等待应答数据
            //        {
            //            outTimeX--;
            //            Thread.Sleep(1);
            //        }
            //        if (outTimeX == 0) { output("获取设备考勤信息指令应答超时!"); return 0xff; }//如果超时，中断发送
            //        else
            //        {
            //            uint totalCount = Convert.ToUInt32(new string(uart.data, 0, 8));
            //            uint thisCount = Convert.ToUInt32(new string(uart.data, 8, 8));
            //            if (thisCount == 0) { output("没有需要获取的信息。"); break; }
            //            output("设备" + getDeviceName(address) + "内信息总条数：" + totalCount);

            //            SQLiteTransaction tran = conn.BeginTransaction();//实例化事务
            //            for (int i = 0; i < thisCount; i++)
            //            {
            //                string uID = new string(uart.data, 16 + i * 36 + 0, 4);//用户号
            //                string studentID = new string(uart.data, 16 + i * 36 +4, 11);//用户号

            //                //将GBK编码的汉字转换为字符串
            //                byte[] bs = new byte[2];
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +15]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +16]).ToString("X"), 16);
            //                string userName = Encoding.GetEncoding("GBK").GetString(bs);
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +17]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +18]).ToString("X"), 16);
            //                userName += Encoding.GetEncoding("GBK").GetString(bs);
            //                bs[0] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +19]).ToString("X"), 16);
            //                bs[1] = (byte)Convert.ToByte(((int)uart.data[16 + i * 36 +20]).ToString("X"), 16);
            //                userName += Encoding.GetEncoding("GBK").GetString(bs);
            //                string doorState = new string(uart.data, 16 + i * 36 +21, 1);//进出门状态
            //                string year = new string(uart.data, 16 + i * 36 +22, 4);//年
            //                string month = new string(uart.data, 16 + i * 36 +26, 2);//月
            //                string day = new string(uart.data, 16 + i * 36 +28, 2);//日
            //                string hour = new string(uart.data, 16 + i * 36 +30, 2);//时
            //                string minutes = new string(uart.data, 16 + i * 36 +32, 2);//分
            //                string second = new string(uart.data, 16 + i * 36 +34, 2);//秒
            //                //string time = (year + "年" + month + "月" + day + "日" + hour + "时" + minutes + "分" + second + "秒");
            //                string time = (year + "年" + month + "月" + day + "日 " + hour + ":" + minutes + ":" + second);
            //                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
            //                cmd.Transaction = tran;
            //                cmd.CommandText = "insert into attendanceRecord values(@deviceName, @uID, @studentID, @userName, @state,@time)";//设置带参SQL语句  
            //                cmd.Parameters.AddRange(new[] {//添加参数  
            //                                        new SQLiteParameter("@deviceName", getDeviceName(address)),//设备名称
            //                                        new SQLiteParameter("@uID", uID),//用户号
            //                                        new SQLiteParameter("@studentID", studentID),//学号
            //                                        new SQLiteParameter("@userName", userName),  //姓名
            //                                        new SQLiteParameter("@state", doorState),//进出门状态
            //                                        new SQLiteParameter("@time", time),//时间
            //                                        });
            //                cmd.ExecuteNonQuery();//执行查询 

            //                output("获取：" + uID + "号用户：" + userName + " " + (doorState == "0" ? "进门" : "出门") + " 信息成功");
            //            }
            //            tran.Commit();//提交
            //            tran.Dispose();//释放tran使用的资源，防止database is lock异常产生
            //            if (thisCount == totalCount)
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    output(ex.Message);
            //    return 0xff;
            //}
            //conn.Close();
            //return 0x00;
            return 0x00;
        }
        /// <summary>
        /// 由机器号获取设备名称
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string getDeviceName(byte address)
        {
            int rowsCount = dgvDeviceInfo.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                if (dgvDeviceInfo.Rows[i].Cells[2].Value.ToString() == address.ToString())
                {
                    return dgvDeviceInfo.Rows[i].Cells[0].Value.ToString();
                }
            }
            return "无机器号为：" + address.ToString() + " 的设备";
        }

        private void 考勤信息统计toolStripButton_Click(object sender, EventArgs e)
        {
            FormAttendanceInfo formAttendanceInfo = new FormAttendanceInfo();
            formAttendanceInfo.Show();
        }

        private void 获取人员进出记录toolStripButton_Click(object sender, EventArgs e)
        {
            timerConnectAndGet_Tick(sender, e);
        }
    }
}


