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
        FormDeviceManagement formDeviceManagement = new FormDeviceManagement();
        FormAttendanceInfo formAttendanceInfo = new FormAttendanceInfo();
        FormPersonnelManagement formPersonnelManagement = new FormPersonnelManagement();
        Thread child_thread;
        UInt32 is_child_thread_stop = 0; /* 线程是否结束 0:结束 1:未结束 */

        private void btnSendData_Click(object sender, EventArgs e)
        {
            //byte[] xx = new byte[259];
            //uart.sendNByte(xx, 0x56, 0x00, 259, 500);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Stm32Sync stm32_sync = new Stm32Sync();
            PersonnelManagement personnelManagement = new PersonnelManagement();
            PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);
            MacAddr mac = new MacAddr();
            //stm32_sync.user_add(mac.Mac, personnelManagement.PersonList[2]);
            //byte[] name1 = {0x30, 0x5f, 0xd5, 0xc5, 0xbd, 0xf8, 0xbf, 
            //                0xc6, 0x2e, 0x62, 0x69, 0x6e, 0x00, 0x33, 
            //                0x20, 0x31, 0x33, 0x30, 0x37, 0x35, 0x35, 
            //                0x37, 0x34, 0x30, 0x33, 0x36, 0x20, 0x30, 
            //                0x20, 0x30, 0x20, 0x31, 0x20, 0x33};
            //string name_str1 = Encoding.Default.GetString(name1);
            //byte[] name = Encoding.Default.GetBytes(name_str1);

            //AttendanceInfo attendanceInfo = new AttendanceInfo();
            //AttendanceInfo.AttInfo attInfo = new AttendanceInfo.AttInfo();

            //attInfo.deviceID = 4;                     /* 设备ID */
            //attInfo.deviceName = "411";               /* 设备名称 */
            //attInfo.mac = "00:00:00:00:00:04";        /* 设备mac地址 */
            //attInfo.infoID = 0;                       /* 记录ID 可重复,只作为参考 */
            //attInfo.uID = 1;                          /* 用户号 */
            //attInfo.studentID = "11307030328";        /* 学号 */
            //attInfo.name = "张进科";                  /* 姓名 */
            //attInfo.state = (char)1;                  /* 状态 0:出门 1:进门 */
            //attInfo.time = "2017年03月18日 22:34:59"; /* 时间 */
            //attendanceInfo.AddInfo(attInfo);
            //attendanceInfo.DelInfo(attInfo);


            //PersonnelManagement personnelManagement = new PersonnelManagement();
            //PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);

            //person.uID         = 0;
            //person.cardID      = 0xFFFFFFFF;
            //person.activeState = 0;
            //person.studentID   = "11307030328";
            //person.dormitory   = "柏轩 B414";
            //person.major       = "机械工程学院 测控技术与仪器";
            //person.name        = "张进科";
            //person.sex         = "男";
            //person.birthday    = "1995年02月23日";
            //person.tel         = "15825941073";
            //person.QQ          = "799658861";
            //person.weiXin      = "zhangjinke0220";
            //person.authority   = "authority";
            //person.isLimitTime = 0;
            //person.limitTime   = "2099年12月31日23时59分59秒";
            //person.eigenNum[0] = 0;
            //person.eigenNum[1] = 1;
            //person.eigenNum[2] = 2;
            //person.eigenNum[3] = 3;
            //person.eigenNum[4] = 4;
            //person.eigen[0]    = "eigen0";
            //person.eigen[1]    = "eigen1";
            //person.eigen[2]    = "eigen2";
            //person.eigen[3]    = "eigen3";
            //person.eigen[4]    = "eigen4";
            //person.recodeDate = "2017年2月3日17时49分26秒";
            //person.resv0       = 0;
            //person.resv1       = 0;
            //person.resv2       = "";
            //person.resv3       = "";
            //person.resv4       = "";
            //personnelManagement.AddPerson(person);
            //personnelManagement.DelPerson(person);

            //school_info_init();

            Control.CheckForIllegalCrossThreadCalls = false;        //不检查跨线程的调用是否合法（不好的解决方案）
            if (!Directory.Exists(Environment.CurrentDirectory + @"\dataBase"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\dataBase");//目录不存在，建立目录
            }
            if (!Directory.Exists(Environment.CurrentDirectory + @"\picture"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\picture");//目录不存在，建立目录
            }

            DeviceManagement deviceManagement = new DeviceManagement();
            dgvDeviceInfo.Rows.Clear();                    /* 清空表格 */
            deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 设备数量 */
            for (int idx = 0; idx < count; idx++)          /* 将设备添加到表格中 */
            {
                int index = dgvDeviceInfo.Rows.Add();
                dgvDeviceInfo.Rows[index].Cells[0].Value = deviceManagement.DeviceList[idx].name; /* 设备名称 */
                dgvDeviceInfo.Rows[index].Cells[1].Value = "未连接";                              /* 状态 */
                dgvDeviceInfo.Rows[index].Cells[2].Value = deviceManagement.DeviceList[idx].mac;  /* mac地址 */
                dgvDeviceInfo.Rows[index].Cells[3].Value = "";                                    /* 人员数 */
            }
        }

        private void 人员维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerConnectAndGet.Enabled = false;//停止获取设备考勤记录
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
            formPersonnelManagement.cbDeviceList.Items.Clear();
            formPersonnelManagement.cbDeviceList.Items.Add("全部设备");
            formPersonnelManagement.cbDeviceList.SelectedIndex = 0;
            formPersonnelManagement.ShowDialog();
        }
        private void 设备管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //formDeviceManagement = new FormDeviceManagement();
            formDeviceManagement.ShowDialog();
        }

        private void 连接设备toolStripButton_Click(object sender, EventArgs e)
        {
            if (is_child_thread_stop != 0)
            {
                MessageBox.Show("任务正在执行！");
                return;
            }

            child_thread = new Thread(connectDevice);
            child_thread.Priority = ThreadPriority.Highest;
            child_thread.Start();
            is_child_thread_stop = 1;
        }
        private void connectDevice()
        {
            Stm32Sync stm32_sync = new Stm32Sync();
            stm32_sync.output_dlg = output;
            MacAddr dst_addr;

            int row = dgvDeviceInfo.SelectedRows.Count;    /* 获取选中总行数 */
            int rowIndex = dgvDeviceInfo.CurrentRow.Index; /* 获取选择行号 */

            for (int i = 0; i < row; i++)
            {
                string addr_str = dgvDeviceInfo.SelectedRows[i].Cells[2].Value.ToString();
                dst_addr = new MacAddr(addr_str);
                if (stm32_sync.connect(dst_addr.Mac))
                {
                    dgvDeviceInfo.SelectedRows[i].Cells[1].Value = "已连接";
                }
            }

            is_child_thread_stop = 0;
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
            PersonnelManagement personnelManagement = new PersonnelManagement();
            PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);
            Stm32Sync stm32_sync = new Stm32Sync();
            stm32_sync.output_dlg = output;
            MacAddr dst_addr;

            string addr_str = dgvDeviceInfo.SelectedRows[0].Cells[2].Value.ToString();
            dst_addr = new MacAddr(addr_str);

            /* 添加用户 */
            for (int i = 0; i < personnelManagement.PersonList.Count; i++)
            { 
                stm32_sync.user_add(dst_addr.Mac, personnelManagement.PersonList[i]);
            }

            /* 删除用户 */
            //stm32_sync.user_del(dst_addr.Mac, (UInt32)1);

            /* 重新加载卡号-用户号，指纹号-用户号列表 */
            stm32_sync.list_reload(dst_addr.Mac);
            return;

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
            if (is_child_thread_stop != 0)
            {
                MessageBox.Show("任务正在执行！");
                return;
            }
            child_thread = new Thread(SyncTime);
            child_thread.Priority = ThreadPriority.Highest;
            child_thread.Start();
            is_child_thread_stop = 1;
        }

        /// <summary>
        /// 同步所有已连接设备的时间
        /// </summary>
        private void SyncTime()
        {
            Stm32Sync stm32_sync = new Stm32Sync();
            stm32_sync.output_dlg = output;
            MacAddr dst_addr;

            int row = dgvDeviceInfo.SelectedRows.Count;    /* 获取选中总行数 */
            int rowIndex = dgvDeviceInfo.CurrentRow.Index; /* 获取选择行号 */

            for (int i = 0; i < row; i++)
            {
                if (dgvDeviceInfo.SelectedRows[i].Cells[1].Value.ToString() == "已连接")
                { 
                    string addr_str = dgvDeviceInfo.SelectedRows[i].Cells[2].Value.ToString();
                    dst_addr = new MacAddr(addr_str);
                    stm32_sync.time_sync(dst_addr.Mac);
                }
            }

            is_child_thread_stop = 0;
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
            formAttendanceInfo.Show();
        }

        private void 获取人员进出记录toolStripButton_Click(object sender, EventArgs e)
        {
            if (is_child_thread_stop != 0)
            {
                MessageBox.Show("任务正在执行！");
                return;
            }
            child_thread = new Thread(SyncUser);
            child_thread.Priority = ThreadPriority.Highest;
            child_thread.Start();
            is_child_thread_stop = 1;
        }
        /// <summary>
        /// 同步所有已连接设备的考勤记录
        /// </summary>
        private void SyncATT()
        {
            output("获取人员进出记录");
            DeviceManagement deviceManagement = new DeviceManagement();
            AttendanceInfo attendanceInfo = new AttendanceInfo();
            AttendanceInfo.AttInfo attInfo = new AttendanceInfo.AttInfo();
            Stm32Sync stm32_sync = new Stm32Sync();
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            MacAddr dst_addr;
            UInt32 total;
            UInt16 count;
            byte[] att_data;
            byte[] student_id = new byte[16];
            byte[] name = new byte[16];

            stm32_sync.output_dlg = output;

            int row = dgvDeviceInfo.SelectedRows.Count;    /* 获取选中总行数 */
            int rowIndex = dgvDeviceInfo.CurrentRow.Index; /* 获取选择行号 */

            for (int j = 0; j < row; j++)
            {
                if (dgvDeviceInfo.SelectedRows[j].Cells[1].Value.ToString() == "已连接")
                {
                    string addr_str = dgvDeviceInfo.SelectedRows[j].Cells[2].Value.ToString();
                    dst_addr = new MacAddr(addr_str);

                    while (true)
                    {
                        /* 获取考勤记录 */
                        if (stm32_sync.att_get(dst_addr.Mac, (UInt16)10, out total, out count, out att_data) != true)
                        {
                            break;
                        }

                        for (int i = 0; i < count; i++)
                        {
                            crc = stm32_crc.block_crc_calc(att_data, (UInt32)(i * 53), 53 - 4);
                            if (crc != (UInt32)((att_data[i * 53 + 49] << 0) +
                                       (att_data[i * 53 + 50] << 8) +
                                       (att_data[i * 53 + 51] << 16) +
                                       (att_data[i * 53 + 52] << 24)))
                            {
                                output("crc failed");
                                continue;
                            }

                            Buffer.BlockCopy(att_data, i * 53 + 2, student_id, 0, student_id.Length);
                            Buffer.BlockCopy(att_data, i * 53 + 18, name, 0, name.Length);

                            attInfo.deviceID = att_data[i * 53 + 34]; /* 设备ID */
                            attInfo.deviceName = deviceManagement.device_name_get(attInfo.deviceID); /* 设备名称 */
                            attInfo.mac = att_data[i * 53 + 35].ToString("x2") + ":" +
                                          att_data[i * 53 + 36].ToString("x2") + ":" +
                                          att_data[i * 53 + 37].ToString("x2") + ":" +
                                          att_data[i * 53 + 38].ToString("x2") + ":" +
                                          att_data[i * 53 + 39].ToString("x2") + ":" +
                                          att_data[i * 53 + 40].ToString("x2");        /* 设备mac地址 */
                            attInfo.uID = (UInt32)(att_data[i * 53 + 0] + (att_data[i * 53 + 1] << 8));                          /* 用户号 */
                            attInfo.studentID = System.Text.Encoding.Default.GetString(student_id).Replace("\0", "");        /* 学号 */
                            attInfo.name = System.Text.Encoding.UTF8.GetString(name).Replace("\0", "");                  /* 姓名 */
                            attInfo.state = (char)att_data[i * 53 + 41];                  /* 状态 0:出门 1:进门 */
                            attInfo.time =
                                (att_data[i * 53 + 42] + (att_data[i * 53 + 43] << 8)).ToString("D4") + "年" +
                                att_data[i * 53 + 44].ToString("D2") + "月" +
                                att_data[i * 53 + 45].ToString("D2") + "日 " +
                                att_data[i * 53 + 46].ToString("D2") + ":" +
                                att_data[i * 53 + 47].ToString("D2") + ":" +
                                att_data[i * 53 + 48].ToString("D2"); /* 时间 */
                            attendanceInfo.AddInfo(attInfo);
                        }

                        /* 删除考勤记录 */
                        if (count != 0)
                        {
                            if (stm32_sync.att_del(dst_addr.Mac, count) != true)
                            {
                                break;
                            }
                        }

                        if (total == 0)
                        {
                            output("已获取完成所有考勤记录");
                            break;
                        }
                    }
                }
            }

            is_child_thread_stop = 0;
        }
        /// <summary>
        /// 同步所有已连接设备的用户信息
        /// </summary>
        private void SyncUser()
        {
            output("同步用户信息");
            DeviceManagement deviceManagement = new DeviceManagement();
            PersonnelManagement personnelManagement = new PersonnelManagement();
            PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);
            Stm32Sync stm32_sync = new Stm32Sync();
            Stm32_crc stm32_crc = new Stm32_crc();
            UInt32 crc = 0;
            MacAddr dst_addr;
            UInt16 total;
            UInt16 count;
            UInt16 total_get = 0;
            byte[] user_crc_byte;
            UInt32[] user_crc = new UInt32[0];
            bool err = true;
            bool is_change = false;

            stm32_sync.output_dlg = output;

            int row = dgvDeviceInfo.SelectedRows.Count;    /* 获取选中总行数 */
            int rowIndex = dgvDeviceInfo.CurrentRow.Index; /* 获取选择行号 */

            for (int j = 0; j < row; j++)
            {
                if (dgvDeviceInfo.SelectedRows[j].Cells[1].Value.ToString() == "已连接")
                {
                    string addr_str = dgvDeviceInfo.SelectedRows[j].Cells[2].Value.ToString();
                    dst_addr = new MacAddr(addr_str);

                    /* 首先获取设备内所有用户信息的crc */
                    while (true)
                    {
                        /* 获取用户crc */
                        if (stm32_sync.user_crc_get(dst_addr.Mac, (UInt16)(total_get + 1), (UInt16)200, out total, out count, out user_crc_byte) != true)
                        {
                            err = false;
                            break;
                        }

                        if (total_get == 0) {
                            user_crc = new UInt32[total];
                        }

                        for (int i = 0; i < count; i++)
                        {
                            crc = (UInt32)((user_crc_byte[i * 4 + 0] << 0) +
                                           (user_crc_byte[i * 4 + 1] << 8) +
                                           (user_crc_byte[i * 4 + 2] << 16) +
                                           (user_crc_byte[i * 4 + 3] << 24));
                            user_crc[i + total_get] = crc;
                        }

                        total_get += count;

                        if (total_get == total)
                        {
                            output("已获取完成所有用户crc");
                            err = true;
                            break;
                        }
                    }
                    if (err == false)
                    {
                        output("crc获取错误");
                        continue;
                    }

                    /* 同步用户信息 */
                    for (UInt32 uID = 1; uID <= user_crc.Length; uID++)
                    {
                        /* 获取用户信息 */
                        person = personnelManagement.user_get(uID);

                        /* 用户不存在上位机，删除用户 */
                        if ((person.uID != uID) && (user_crc[uID - 1] != 0))
                        {
                            is_change = true;
                            if (stm32_sync.user_del(dst_addr.Mac, uID) == false)
                            {
                                output("删除" + uID.ToString() + "号用户失败");
                                err = false;
                                break;
                            }
                            continue;
                        }

                        if (person.uID != 0) {
                            /* 计算用户crc */
                            crc = stm32_sync.user_crc_calc(person);

                            /* crc不同，证明用户信息改变，添加用户 */
                            if (crc != user_crc[uID - 1])
                            {
                                is_change = true;
                                if (stm32_sync.user_add(dst_addr.Mac, person) == false)
                                {
                                    output("添加" + uID.ToString() + "号用户失败");
                                    err = false;
                                    break;
                                }
                            }
                        }
                    }

                    UInt32 user_id_max_device = (UInt32)user_crc.Length;
                    UInt32 user_id_max_db = personnelManagement.max_user_id_get();
                    if (user_id_max_device < user_id_max_db)
                    {
                        for (UInt32 uID = (UInt32)(user_id_max_device + 1); uID <= user_id_max_db; uID++)
                        {
                            /* 获取用户信息 */
                            person = personnelManagement.user_get(uID);

                            if (person.uID == uID)
                            {
                                is_change = true;
                                if (stm32_sync.user_add(dst_addr.Mac, person) == false)
                                {
                                    output("添加" + uID.ToString() + "号用户失败");
                                    err = false;
                                }
                            }
                        }
                    }

                    if (is_change == true)
                    { 
                        /* 重新加载卡号-用户号，指纹号-用户号列表 */
                        stm32_sync.list_reload(dst_addr.Mac);
                    }

                    if (err == false)
                    {
                        output("同步用户信息出错");
                        continue;
                    }
                    else
                    {
                        output("同步用户信息成功");
                    }

                }
            }

            is_child_thread_stop = 0;
        }

        private void school_info_init()
        {
            SchoolInfo schoolInfo = new SchoolInfo();
            SchoolInfo.BuildingInfo buildingInfo = new SchoolInfo.BuildingInfo();
            SchoolInfo.InstituteInfo instituteInfo = new SchoolInfo.InstituteInfo();

            string[] floor = new string[3];
            buildingInfo.num = 1;
            buildingInfo.name = "竹轩（一园区）";
            floor[0] = "A";
            floor[1] = "B";
            floor[2] = "C";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 2;
            buildingInfo.name = "兰轩（二园区）";
            floor[0] = "D";
            floor[1] = "E";
            floor[2] = "F";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[4];
            buildingInfo.num = 3;
            buildingInfo.name = "梅轩（三园区）";
            floor[0] = "G";
            floor[1] = "H";
            floor[2] = "I";
            floor[3] = "J";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 4;
            buildingInfo.name = "菊轩（四园区）";
            floor[0] = "K";
            floor[1] = "L";
            floor[2] = "M";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[2];
            buildingInfo.num = 5;
            buildingInfo.name = "松轩（五园区）";
            floor[0] = "N";
            floor[1] = "O";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 6;
            buildingInfo.name = "荷轩（六园区）";
            floor[0] = "P";
            floor[1] = "Q";
            floor[2] = "R";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 7;
            buildingInfo.name = "榕轩（七园区）";
            floor[0] = "A";
            floor[1] = "B";
            floor[2] = "C";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 8;
            buildingInfo.name = "柏轩（八园区）";
            floor[0] = "A";
            floor[1] = "B";
            floor[2] = "C";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[3];
            buildingInfo.num = 9;
            buildingInfo.name = "桂轩（九园区）";
            floor[0] = "A";
            floor[1] = "B";
            floor[2] = "C";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            floor = new string[2];
            buildingInfo.num = 10;
            buildingInfo.name = "柳轩（十园区）";
            floor[0] = "A";
            floor[1] = "B";
            buildingInfo.floor = floor;
            schoolInfo.AddBuliding(buildingInfo);

            string[] major = new string[8];
            instituteInfo.num = 1;
            instituteInfo.name = "机械工程学院";
            major[0] = "机械制造及其自动化";
            major[1] = "机械电子工程";
            major[2] = "机械设计及理论";
            major[3] = "工业工程";
            major[4] = "精密仪器及机械";
            major[5] = "测试计量技术及仪器";
            major[6] = "武器探测与精确制导";
            major[7] = "机械工程领域";
            instituteInfo.major = major;
            schoolInfo.AddInstitute(instituteInfo);

            major = new string[6];
            instituteInfo.num = 1;
            instituteInfo.name = "电气与电子工程学院";
            major[0] = "光电信息科学与工程";
            major[1] = "电子信息科学与技术";
            major[2] = "通信工程";
            major[3] = "自动化";
            major[4] = "电子信息工程";
            major[5] = "电气工程及其自动化";
            instituteInfo.major = major;
            schoolInfo.AddInstitute(instituteInfo);
        }
    }
}


