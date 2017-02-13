using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace AccessControlSystem
{
    public partial class FormPersonnelManagement : Form
    {
        string fPath;   //头像文件路径

        public FormPersonnelManagement()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "JPG|*.jpg|所有文件|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                fPath = open.FileName;
                Image pic = Image.FromFile(fPath);
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值 
                pbPortrait.Image = pic;
            }
        }
        private void reLoadDataGridView()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            dgvPerson.Rows.Clear();             //清空表格
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM student";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
                lbHeadcount.Text = "总人数：" + RowCount.ToString();
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM student";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    int index = dgvPerson.Rows.Add();

                    DateTime DTbirthDay = DateTime.Parse(reader.GetString(4));
                    TimeSpan TSbirthDay = new TimeSpan(DTbirthDay.Ticks);
                    TimeSpan Now = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan TSage = Now - TSbirthDay;

                    int age = (int)(TSage.TotalDays/365);           //计算年龄
                    dgvPerson.Rows[index].Cells[0].Value = reader.GetInt32(0);  //用户号
                    dgvPerson.Rows[index].Cells[1].Value = reader.GetString(1); //姓名
                    dgvPerson.Rows[index].Cells[2].Value = reader.GetString(2); //性别
                    dgvPerson.Rows[index].Cells[3].Value = reader.GetString(3); //学号
                    dgvPerson.Rows[index].Cells[4].Value = age; //年龄
                    dgvPerson.Rows[index].Cells[5].Value = reader.GetString(4); //出生日期
                    dgvPerson.Rows[index].Cells[6].Value = reader.GetString(5); //录入日期
                    dgvPerson.Rows[index].Cells[7].Value = reader.GetString(6); //QQ
                    dgvPerson.Rows[index].Cells[8].Value = reader.GetString(7); //电话号码
                    dgvPerson.Rows[index].Cells[9].Value = reader.GetString(8); //寝室
                    dgvPerson.Rows[index].Cells[10].Value = reader.GetInt32(9).ToString("X");  //权限
                    dgvPerson.Rows[index].Cells[11].Value = reader.GetString(10);    //卡号
                    dgvPerson.Rows[index].Cells[12].Value = reader.GetString(11);   //特征值
                }
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
                //CreateMember();
                CreateMemberBin();
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
        /// <summary>
        /// 窗体构造函数，检测数据库并给表格添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPersonnelManagement_Load(object sender, EventArgs e)
        {
            reLoadDataGridView();//重新给表格添加数据
            新增toolStripMenuItem1_Click(sender, e);//初始化文本框数据
            cleanPicture();
        }

        private void cleanPicture()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置 

            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建
                string path = Environment.CurrentDirectory + @"\picture";
                DirectoryInfo TheFolder = new DirectoryInfo(path);
                foreach (FileInfo NextFile in TheFolder.GetFiles())//遍历文件
                {
                    string fileName = (NextFile.Name);
                    int uID = Convert.ToInt32(fileName.Substring(0, 4));//用户号
                    string userName = fileName.Length == 12 ? fileName.Substring(5, 3) : fileName.Substring(5, 2);//姓名
                    SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令
                    cmd.CommandText = "SELECT * FROM student WHERE uID = @uID AND name = @name";//设置带参SQL语句  
                    cmd.Parameters.AddRange(new[] {//添加参数  
                                                new SQLiteParameter("@uID", uID),  
                                                new SQLiteParameter("@name", userName),  
                                                });
                    SQLiteDataReader readerStudent = cmd.ExecuteReader();
                    if(readerStudent.Read()!=true)
                    {
                        NextFile.Delete();
                    }
                    readerStudent.Dispose();
                    cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                }
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
        /// <summary>
        /// 清空所有文本框的数据，方便下次添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新增toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int rowsCount = dgvPerson.Rows.Count;//获取总人数
            if (rowsCount == 0) { return; }
            string cellString = dgvPerson.Rows[rowsCount-1].Cells[0].Value.ToString().Trim();//初始化为最后一行的用户号
            tbUID.Text = (rowsCount + 1).ToString();
            tbName.Text = null;
            cbSex.Text = null;
            tbStudentID.Text = null;
            tbQQ.Text = null;
            tbTel.Text = null;
            cbBuilding.Text = null; cbFloor.Text = null; tbDomitory.Text = null;
            cbAuthority.Text = null;
            tbID.Text = null;
            tbEigen.Text = null;
            dtpBirthday.Value = DateTime.Now;
            pbPortrait.Image = null;
            fPath = Environment.CurrentDirectory + @"\defaultAvatar.jpg";
            Image pic = Image.FromFile(fPath);
            pbPortrait.Image = pic;

            if (rowsCount != int.Parse(cellString))//如果最后一行用户号不等于用户数，则有空缺的用户号
            {
                bool isSearch = false;
                for (int i = 1; i < rowsCount; i++)//从1号一直搜索到rowsCount - 1号用户
                {
                    isSearch = false;
                    for (int j = 0; j < rowsCount; j++)//搜索有无第i号用户
                    {
                        cellString = dgvPerson.Rows[j].Cells[0].Value.ToString().Trim();//得到用户号
                        if (i == int.Parse(cellString))
                        {
                            isSearch = true;//如果搜索到有第i号用户，跳出循环，搜索下一用户
                            break;
                        }
                    }
                    if (isSearch == false)//如果没有搜索到i号用户，证明第i号用户空缺
                    {
                        tbUID.Text = i.ToString();
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 将指定人员的数据从数据库读取并显示，方便编辑人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dgvPerson.CurrentRow.Index;     //获取选择行号
            tbUID.Text = dgvPerson.Rows[index].Cells[0].Value.ToString();
            tbName.Text = dgvPerson.Rows[index].Cells[1].Value.ToString();
            cbSex.Text = dgvPerson.Rows[index].Cells[2].Value.ToString();
            tbStudentID.Text = dgvPerson.Rows[index].Cells[3].Value.ToString();
            dtpBirthday.Value = DateTime.Parse(dgvPerson.Rows[index].Cells[5].Value.ToString());
            tbQQ.Text = dgvPerson.Rows[index].Cells[7].Value.ToString();
            tbTel.Text = dgvPerson.Rows[index].Cells[8].Value.ToString();
            string domitory = dgvPerson.Rows[index].Cells[9].Value.ToString();
            byte one = (byte)domitory.LastIndexOf("）");
            cbBuilding.Text = domitory.Substring(0, one+1);
            cbFloor.Text = domitory.Substring(one+1, 1);
            tbDomitory.Text = domitory.Substring(one + 2, 3);
            cbAuthority.Text = dgvPerson.Rows[index].Cells[10].Value.ToString();
            tbID.Text = dgvPerson.Rows[index].Cells[11].Value.ToString();
            tbEigen.Text = dgvPerson.Rows[index].Cells[12].Value.ToString();
            fPath = Environment.CurrentDirectory + @"\picture\" + tbUID.Text.PadLeft(4, '0') + "_" + tbName.Text + ".jpg";
            try
            {
                if (File.Exists(fPath) != true) { output("头像不存在！"); pbPortrait.Image = null; return; }
                pbPortrait.Image = Image.FromFile(Environment.CurrentDirectory + @"\picture\" + tbUID.Text.PadLeft(4, '0') + "_" + tbName.Text + ".jpg");
                output("读取信息成功！");
            }            
            catch (Exception ex)
            {
                output(ex.Message);
            }
        }
        /// <summary>
        /// 表格的右键菜单相应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPerson_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            编辑ToolStripMenuItem_Click(sender, e);
        }
        /// <summary>
        /// 将待添加人员的信息保存到数据库并刷新表格的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tbUID.Text == "") { MessageBox.Show(lbUID.Text + "不能为空"); return; }
            if (tbName.Text == "") { MessageBox.Show(lbName.Text + "不能为空"); return; }
            if (cbSex.Text == "") { MessageBox.Show(lbSex.Text + "不能为空"); return; }
            if (tbStudentID.Text == "") { MessageBox.Show(lbStudentID.Text + "不能为空"); return; }
            if (dtpBirthday.Text == "") { MessageBox.Show(lbBirthday.Text + "不能为空"); return; }
            if (tbQQ.Text == "") { MessageBox.Show(lbQQ.Text + "不能为空"); return; }
            if (tbTel.Text == "") { MessageBox.Show(lbTel.Text + "不能为空"); return; }
            if (cbBuilding.Text == "") { MessageBox.Show("寝室楼" + "不能为空"); return; }
            if (cbFloor.Text == "") { MessageBox.Show("楼号" + "不能为空"); return; }
            if (tbDomitory.Text == "") { MessageBox.Show("寝室门牌号" + "不能为空"); return; }
            if (cbAuthority.Text == "") { MessageBox.Show(lbAuthority.Text + "不能为空"); return; }
            if (tbID.Text == "") { MessageBox.Show(lbID.Text + "不能为空"); return; }
            if (tbID.Text.Replace(" ", "").Length != 8) { MessageBox.Show(lbID.Text + "长度错误"); return; }
            if (tbEigen.Text == "") { MessageBox.Show(lbEigen.Text + "不能为空"); return; }
            if (tbEigen.Text.Replace(" ", "").Length != 193*2) { MessageBox.Show(lbEigen.Text + "长度错误"); return; }
            if (fPath == null) { MessageBox.Show("请选择相片"); btnOpen_Click(sender, e); return; }
            SQLiteConnection conn = null;

            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.Open();//打开数据库，若文件不存在会自动创建  
            try
            {
                SQLiteTransaction tran = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
                cmd.Transaction = tran;
                cmd.CommandText = "insert into student values(@uID, @name, @sex, @studentID, @birthday, @recodeDate, @qq, @tel, @dormitory, @authority, @id, @eigen)";//设置带参SQL语句  
                cmd.Parameters.AddRange(new[] {//添加参数  
                                        new SQLiteParameter("@uID", int.Parse(tbUID.Text.Replace(" ", ""))),  
                                        new SQLiteParameter("@name", tbName.Text.Replace(" ", "")),  
                                        new SQLiteParameter("@sex", cbSex.Text),
                                        new SQLiteParameter("@studentID", tbStudentID.Text.Replace(" ", "")),
                                        new SQLiteParameter("@birthday", dtpBirthday.Text),
                                        new SQLiteParameter("@recodeDate", DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分")),
                                        new SQLiteParameter("@qq", tbQQ.Text.Replace(" ", "")),
                                        new SQLiteParameter("@tel", tbTel.Text.Replace(" ", "")),
                                        new SQLiteParameter("@dormitory", cbBuilding.Text + cbFloor.Text + tbDomitory.Text.Replace(" ", "")),
                                        new SQLiteParameter("@authority", Convert.ToInt32(cbAuthority.Text,16)),
                                        new SQLiteParameter("@id", tbID.Text.Replace(" ", "")),
                                        new SQLiteParameter("@eigen", tbEigen.Text)
                                        });
                cmd.ExecuteNonQuery();//执行查询
                tran.Commit();//提交
                cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                tran.Dispose();//释放reader使用的资源，防止database is lock异常产生
                output("添加" + tbUID.Text + "号用户" + tbName.Text + "成功");
            }
            catch (Exception ex)
            {
                if (ex.Message == "constraint failed\r\nUNIQUE constraint failed: student.uID")
                {
                    output("用户号不能与其它用户相同！！！");
                }
                else
                {
                    output(ex.Message);
                }
                
            }
            reLoadDataGridView();//重新给表格添加数据
            String sourcePath = fPath;
            String targetPath = Environment.CurrentDirectory + @"\picture\" + tbUID.Text.PadLeft(4,'0') + "_" + tbName.Text + ".jpg";
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            try { System.IO.File.Copy(sourcePath, targetPath, isrewrite); }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
        /// <summary>
        /// 从数据库中删除指定用户号的用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认删除人员？？？", "确认删除", MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.Yes) { return;}//不电击确认按钮直接返回
            
            SQLiteConnection conn = null;

            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.Open();//打开数据库，若文件不存在会自动创建
  
            int rows = dgvPerson.SelectedRows.Count;//获取选中总行数
            try
            {
                for (int j = 0; j < rows; j++)
                {
                    SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
                    cmd.CommandText = "DELETE FROM student where uID = @uID";//设置带参SQL语句  
                    cmd.Parameters.AddRange(new[] { new SQLiteParameter("@uID", int.Parse(dgvPerson.SelectedRows[j].Cells[0].Value.ToString())) });//添加参数
                    cmd.ExecuteNonQuery();//执行查询
                    cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生

                    String uID = dgvPerson.SelectedRows[j].Cells[0].Value.ToString();
                    String name = dgvPerson.SelectedRows[j].Cells[1].Value.ToString();
                    output("删除" + uID + "号用户" + name + "成功");
                    if (pbPortrait.Image != null)
                    {
                        pbPortrait.Image.Dispose(); //解除pictureBox对图片文件的占用
                    }
                    File.Delete(Environment.CurrentDirectory + @"\picture\" + uID + "_" + name + ".jpg");
                }
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            reLoadDataGridView();//重新给表格添加数据

            conn.Close(); 
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        /// <summary>
        /// 创建下载函数的线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            Thread download = new Thread(Download);
            download.Priority = ThreadPriority.Highest;
            download.Start();
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
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置 

            try
            { 
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM student";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM student";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader readerStudent = cmdQ.ExecuteReader();//读取上位机数据库内人员信息
                while (readerStudent.Read())
                {
                    int sourceUID = readerStudent.GetInt32(0);  //用户号
                    string sourceId = readerStudent.GetString(10);//卡号
                    string sourceStudentID = readerStudent.GetString(3); //学号
                    string sourceName = readerStudent.GetString(1); //姓名
                    sourceName = sourceName.Replace(" ", ""); //删除字符串中的空格
                    byte sourceAuthority = readerStudent.GetByte(9);  //权限
                    string sourceEigen = readerStudent.GetString(11);   //特征值
                    string sqlStudentTemp = "SELECT * FROM studentTemp WHERE uID=" + sourceUID.ToString();
                    SQLiteCommand cmdStudentTemp = new SQLiteCommand(sqlStudentTemp, conn);
                    SQLiteDataReader readerStudentTemp = cmdStudentTemp.ExecuteReader();//在下位机数据库中搜索指定用户号的用户
                    if (readerStudentTemp.Read() == true)
                    {
                        int tempUID = readerStudentTemp.GetInt32(0);//用户号
                        string tempId = readerStudentTemp.GetString(1);//卡号
                        string tempStudentID = readerStudentTemp.GetString(2);//学号
                        string tempName = readerStudentTemp.GetString(3);//姓名
                        tempName = tempName.Replace(" ", ""); //删除字符串中的空格
                        byte tempAuthority = readerStudentTemp.GetByte(4);//权限
                        //while (readerStudentTemp.Read())
                        //如果下位机中存在此用户号用户，且其它信息与上位机内数据不用，删除此用户重新添加
                        if ((tempId != sourceId) || (tempStudentID != sourceStudentID) || (tempName != sourceName) || (tempAuthority != sourceAuthority))
                        {
                            if (deleteOnePerson(tempUID.ToString(), tempName, address) != 0x00) { break; }
                            if (addUser(sourceUID.ToString(), sourceId, sourceStudentID, sourceName,
                                        sourceAuthority.ToString(), "0", sourceEigen, RowCount, address) != 0x00) { break; }
                        }
                        
                    }
                    else
                    { 
                        if (addUser(sourceUID.ToString(), sourceId, sourceStudentID, sourceName,
                                    sourceAuthority.ToString(), "0", sourceEigen, RowCount, address) != 0x00) { break; }                  
                    }
                    readerStudentTemp.Dispose();
                }
                readerStudent.Dispose();
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM studentTemp";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader readerStudentTempReRead = cmdQ.ExecuteReader();//读取下位机数据库内人员信息
                while (readerStudentTempReRead.Read())
                {
                    int tempUID = readerStudentTempReRead.GetInt32(0);  //用户号
                    string tempId = readerStudentTempReRead.GetString(1);//卡号
                    string tempStudentID = readerStudentTempReRead.GetString(2); //学号
                    string tempName = readerStudentTempReRead.GetString(3); //姓名
                    byte tempAuthority = readerStudentTempReRead.GetByte(4);  //权限
                    string sqlStudentTemp = "SELECT * FROM student WHERE uID=" + tempUID.ToString();
                    SQLiteCommand cmdStudentTemp = new SQLiteCommand(sqlStudentTemp, conn);
                    SQLiteDataReader readerStudentReRead = cmdStudentTemp.ExecuteReader();//在上位机数据库中搜索指定用户号的用户
                    if (readerStudentReRead.Read() == false)//若在上位机数据库中没搜索到指定用户，代表此用户已被删除
                    {
                        deleteOnePerson(tempUID.ToString(), tempName, address);
                    }
                    readerStudentReRead.Dispose();
                }
                readerStudentTempReRead.Dispose();
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生
                output("人员信息同步完成。");
            }
            catch (Exception ex)
            {
                output(ex.Message);
                return 0xff;
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
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
        /// 计算数据库内总人数
        /// </summary>
        /// <returns></returns>
        private int calculatePersonNum()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            int RowCount = 0;
            try
            { 
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM student";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
            return RowCount;
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
                             string eigen, int personNum,byte address)
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
        private byte deleteOnePerson(string uID, string name,byte address)
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
        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="log"></param>
        private void output(string log)
        {
            if (rtbLog.Text != "") { rtbLog.Text += "\r\n"; }
            rtbLog.Text += DateTime.Now.ToString("HH:mm:ss ") + log;
            rtbLog.Select(rtbLog.Text.Length, 0);//将光标设置到最末尾
            rtbLog.ScrollToCaret();  //将滚动条设置到光标处
        }
        /// <summary>
        /// 清除日志信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清除日志信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbLog.Text = "";
        }

        /// <summary>
        /// 限制TextBox只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedNum(object sender, EventArgs e)
        {
            TextBox tbInput = (TextBox)sender;
            var reg = new Regex("^[0-9]*$");
            var str = tbInput.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                tbInput.Text = sb.ToString();
                tbInput.SelectionStart = tbInput.Text.Length;    //定义输入焦点在最后一个字符
            }
        }

        /// <summary>
        /// 限制TextBox只能输入数字和a-f、A-F及空格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedHex(object sender, EventArgs e)
        {
            TextBox tbInput = (TextBox)sender;
            var reg = new Regex("^[0-9A-Fa-f ]*$");
            var str = tbInput.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                tbInput.Text = sb.ToString();
                tbInput.SelectionStart = tbInput.Text.Length;    //定义输入焦点在最后一个字符
            }
        }
        /// <summary>
        /// 限制TextBox只能输入中日韩文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedChinese(object sender, EventArgs e)
        {
            TextBox tbInput = (TextBox)sender;
            var reg = new Regex("^[\u2E80-\u9FFF]*$");
            var str = tbInput.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                tbInput.Text = sb.ToString();
                tbInput.SelectionStart = tbInput.Text.Length;    //定义输入焦点在最后一个字符
            }
        }

        /// <summary>
        /// 由选择的寝室楼切换相应的楼号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBuilding_TextChanged(object sender, EventArgs e)
        {
            byte index = (byte)(cbBuilding.SelectedIndex + 1);
            cbFloor.Items.Clear();      //清除已有的项
            switch (index)
            {
                case 1: { cbFloor.Items.Add("A"); cbFloor.Items.Add("B"); cbFloor.Items.Add("C"); } break;
                case 2: { cbFloor.Items.Add("D"); cbFloor.Items.Add("E"); cbFloor.Items.Add("F"); } break;
                case 3: { cbFloor.Items.Add("G"); cbFloor.Items.Add("H"); cbFloor.Items.Add("I"); cbFloor.Items.Add("J"); } break;
                case 4: { cbFloor.Items.Add("K"); cbFloor.Items.Add("L"); cbFloor.Items.Add("M"); } break;
                case 5: { cbFloor.Items.Add("N"); cbFloor.Items.Add("O"); } break;
                case 6: { cbFloor.Items.Add("P"); cbFloor.Items.Add("Q"); cbFloor.Items.Add("R"); } break;
                case 7: { cbFloor.Items.Add("A"); cbFloor.Items.Add("B"); cbFloor.Items.Add("C"); } break;
                case 8: { cbFloor.Items.Add("A"); cbFloor.Items.Add("B"); cbFloor.Items.Add("C"); } break;
                case 9: { cbFloor.Items.Add("A"); cbFloor.Items.Add("B"); cbFloor.Items.Add("C"); } break;
                case 10: { cbFloor.Items.Add("A"); cbFloor.Items.Add("B"); } break;
            }
        }

        private void dgvPerson_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvPerson.Rows[e.RowIndex].Selected == false)
                    {
                        dgvPerson.ClearSelection();
                        dgvPerson.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvPerson.SelectedRows.Count == 1)
                    {
                        dgvPerson.CurrentCell = dgvPerson.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    cmsdgvPerson.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void btnDownloadAll_Click(object sender, EventArgs e)
        {
            Thread downloadAll = new Thread(DownloadAll);
            downloadAll.Priority = ThreadPriority.Highest;
            downloadAll.Start();

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
        private byte getPictureState(byte address,string uID,string name)
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
        private byte writeInfo(string uID, string ID, string studentID,
                             string name, string authority, string status,
                             string eigen)
        {
            string path = Environment.CurrentDirectory + @"\dataBase\Member.txt";
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);

            string sendDataString = string.Empty;   //要发送的字符串
            string eigenString = string.Empty;      //指纹特征值字符串

            sendDataString += uID.PadLeft(4, '0') + "\t";//添加用户号（4字节）
            sendDataString += ID.PadLeft(8, '0') + "\t";//添加ID卡号（8字节）
            sendDataString += studentID.PadLeft(11, '0') + "\t";//添加学号（11字节）
            if (name.Length == 2) { name = name.Insert(1, "  "); }
            sendDataString += name.PadLeft(3) + "\t"; //姓名（6字节）          
            sendDataString += authority.PadLeft(1, '0') + "\t";//权限（1字节）
            sendDataString += status.PadLeft(1, '0') + "\t";//状态（1字节）
            sendDataString += eigen.Replace(" ", "").PadRight(193 * 2, '0') + "\r\n"; //添加指纹特征值
            sw.Write(sendDataString);
            sw.Close();//关闭流
            return 0;
        }
        private byte writeInfoBin(string uID, string ID, string effective,
                                string studentID, string name, string authority, 
                                string status,string effectiveTime, string[] eigen)
        {
            string path = Environment.CurrentDirectory + @"\dataBase\member.bin";
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);

            string sendDataString = string.Empty;   //要发送的字符串
            string eigenString = string.Empty;      //指纹特征值字符串

            sendDataString += uID.PadLeft(4, '0') + "\t";//添加用户号（4字节）
            sendDataString += ID.PadLeft(8, '0') + "\t";//添加ID卡号（8字节）
            sendDataString += studentID.PadLeft(11, '0') + "\t";//添加学号（11字节）
            if (name.Length == 2) { name = name.Insert(1, "  "); }
            sendDataString += name.PadLeft(3) + "\t"; //姓名（6字节）          
            sendDataString += authority.PadLeft(1, '0') + "\t";//权限（1字节）
            sendDataString += status.PadLeft(1, '0') + "\t";//状态（1字节）
            sendDataString += eigen[0].Replace(" ", "").PadRight(193 * 2, '0') + "\r\n"; //添加指纹特征值
            sw.Write(sendDataString);
            sw.Close();//关闭流
            return 0;
        }
        private void CreateMember()
        {
            string path = Environment.CurrentDirectory + @"\dataBase\Member.txt";
            FileStream fs = new FileStream(path, FileMode.Create);
            int rowsCount = dgvPerson.Rows.Count;
            byte[] data = System.Text.Encoding.Default.GetBytes("用户号\t卡号\t学号\t姓名\t权限\t状态\t指纹\t总人数\t" + rowsCount.ToString().PadLeft(4, '0') + "\r\n");
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
            for (int i = 0; i < rowsCount; i++)
            {
                string UID = dgvPerson.Rows[i].Cells[0].Value.ToString();
                string ID = dgvPerson.Rows[i].Cells[11].Value.ToString();
                string StudentID = dgvPerson.Rows[i].Cells[3].Value.ToString();
                string Name = dgvPerson.Rows[i].Cells[1].Value.ToString();
                int AuthorityInt = Convert.ToInt32(dgvPerson.Rows[i].Cells[10].Value.ToString(), 16);
                string Authority = AuthorityInt.ToString("X");
                string Eigen = dgvPerson.Rows[i].Cells[12].Value.ToString();
                writeInfo(UID, ID, StudentID, Name, Authority, "1", Eigen);
            }
            StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
            sw.Write("OVER\r\n");
            sw.Flush();
            sw.Close();
        }
        uint nReg;//CRC寄存器

        //复位CRC
        void CRC_Reset()
        {
            nReg = 0xFFFFFFFF;
        }

        //软件CRC
        uint[] Crc32Table = new uint[256]
        {
                0x00000000,0x04C11DB7,0x09823B6E,0x0D4326D9,0x130476DC,0x17C56B6B,0x1A864DB2,0x1E475005,
                0x2608EDB8,0x22C9F00F,0x2F8AD6D6,0x2B4BCB61,0x350C9B64,0x31CD86D3,0x3C8EA00A,0x384FBDBD,
                0x4C11DB70,0x48D0C6C7,0x4593E01E,0x4152FDA9,0x5F15ADAC,0x5BD4B01B,0x569796C2,0x52568B75,
                0x6A1936C8,0x6ED82B7F,0x639B0DA6,0x675A1011,0x791D4014,0x7DDC5DA3,0x709F7B7A,0x745E66CD,
                0x9823B6E0,0x9CE2AB57,0x91A18D8E,0x95609039,0x8B27C03C,0x8FE6DD8B,0x82A5FB52,0x8664E6E5,
                0xBE2B5B58,0xBAEA46EF,0xB7A96036,0xB3687D81,0xAD2F2D84,0xA9EE3033,0xA4AD16EA,0xA06C0B5D,
                0xD4326D90,0xD0F37027,0xDDB056FE,0xD9714B49,0xC7361B4C,0xC3F706FB,0xCEB42022,0xCA753D95,
                0xF23A8028,0xF6FB9D9F,0xFBB8BB46,0xFF79A6F1,0xE13EF6F4,0xE5FFEB43,0xE8BCCD9A,0xEC7DD02D,
                0x34867077,0x30476DC0,0x3D044B19,0x39C556AE,0x278206AB,0x23431B1C,0x2E003DC5,0x2AC12072,
                0x128E9DCF,0x164F8078,0x1B0CA6A1,0x1FCDBB16,0x018AEB13,0x054BF6A4,0x0808D07D,0x0CC9CDCA,
                0x7897AB07,0x7C56B6B0,0x71159069,0x75D48DDE,0x6B93DDDB,0x6F52C06C,0x6211E6B5,0x66D0FB02,
                0x5E9F46BF,0x5A5E5B08,0x571D7DD1,0x53DC6066,0x4D9B3063,0x495A2DD4,0x44190B0D,0x40D816BA,
                0xACA5C697,0xA864DB20,0xA527FDF9,0xA1E6E04E,0xBFA1B04B,0xBB60ADFC,0xB6238B25,0xB2E29692,
                0x8AAD2B2F,0x8E6C3698,0x832F1041,0x87EE0DF6,0x99A95DF3,0x9D684044,0x902B669D,0x94EA7B2A,
                0xE0B41DE7,0xE4750050,0xE9362689,0xEDF73B3E,0xF3B06B3B,0xF771768C,0xFA325055,0xFEF34DE2,
                0xC6BCF05F,0xC27DEDE8,0xCF3ECB31,0xCBFFD686,0xD5B88683,0xD1799B34,0xDC3ABDED,0xD8FBA05A,
                0x690CE0EE,0x6DCDFD59,0x608EDB80,0x644FC637,0x7A089632,0x7EC98B85,0x738AAD5C,0x774BB0EB,
                0x4F040D56,0x4BC510E1,0x46863638,0x42472B8F,0x5C007B8A,0x58C1663D,0x558240E4,0x51435D53,
                0x251D3B9E,0x21DC2629,0x2C9F00F0,0x285E1D47,0x36194D42,0x32D850F5,0x3F9B762C,0x3B5A6B9B,
                0x0315D626,0x07D4CB91,0x0A97ED48,0x0E56F0FF,0x1011A0FA,0x14D0BD4D,0x19939B94,0x1D528623,
                0xF12F560E,0xF5EE4BB9,0xF8AD6D60,0xFC6C70D7,0xE22B20D2,0xE6EA3D65,0xEBA91BBC,0xEF68060B,
                0xD727BBB6,0xD3E6A601,0xDEA580D8,0xDA649D6F,0xC423CD6A,0xC0E2D0DD,0xCDA1F604,0xC960EBB3,
                0xBD3E8D7E,0xB9FF90C9,0xB4BCB610,0xB07DABA7,0xAE3AFBA2,0xAAFBE615,0xA7B8C0CC,0xA379DD7B,
                0x9B3660C6,0x9FF77D71,0x92B45BA8,0x9675461F,0x8832161A,0x8CF30BAD,0x81B02D74,0x857130C3,
                0x5D8A9099,0x594B8D2E,0x5408ABF7,0x50C9B640,0x4E8EE645,0x4A4FFBF2,0x470CDD2B,0x43CDC09C,
                0x7B827D21,0x7F436096,0x7200464F,0x76C15BF8,0x68860BFD,0x6C47164A,0x61043093,0x65C52D24,
                0x119B4BE9,0x155A565E,0x18197087,0x1CD86D30,0x029F3D35,0x065E2082,0x0B1D065B,0x0FDC1BEC,
                0x3793A651,0x3352BBE6,0x3E119D3F,0x3AD08088,0x2497D08D,0x2056CD3A,0x2D15EBE3,0x29D4F654,
                0xC5A92679,0xC1683BCE,0xCC2B1D17,0xC8EA00A0,0xD6AD50A5,0xD26C4D12,0xDF2F6BCB,0xDBEE767C,
                0xE3A1CBC1,0xE760D676,0xEA23F0AF,0xEEE2ED18,0xF0A5BD1D,0xF464A0AA,0xF9278673,0xFDE69BC4,
                0x89B8FD09,0x8D79E0BE,0x803AC667,0x84FBDBD0,0x9ABC8BD5,0x9E7D9662,0x933EB0BB,0x97FFAD0C,
                0xAFB010B1,0xAB710D06,0xA6322BDF,0xA2F33668,0xBCB4666D,0xB8757BDA,0xB5365D03,0xB1F740B4
        };
        uint CRC_CalcCRC(uint Data)
        {
            uint nTemp = 0;

            nReg ^= (uint)Data;
            for (ushort i = 0; i < 4; i++)
            {
                nTemp = Crc32Table[(byte)((nReg >> 24) & 0xff)]; //取一个字节，查表
                nReg <<= 8;                        //丢掉计算过的头一个BYTE
                nReg ^= nTemp;                //与前一个BYTE的计算结果异或 
            }
            return nReg;
        }
        private void CreateMemberBin()
        {
            string path = Environment.CurrentDirectory + @"\dataBase\member.bin";
            FileStream fs = new FileStream(path, FileMode.Create);
            ushort rowsCount = (ushort)dgvPerson.Rows.Count;//获取行数，即用户数量
            if (rowsCount == 0) { return; }
            ushort user_num_max = (ushort)Convert.ToInt32(dgvPerson.Rows[rowsCount - 1].Cells[0].Value.ToString().Trim());//初始化为最后一行的用户号，即最大用户号
            uint one_user_lenth = 1040;

            byte[] data = new byte[4 + rowsCount * one_user_lenth];
            data[0] = (byte)(rowsCount & 0xff);
            data[1] = (byte)((rowsCount & 0xff00) >> 8);
            data[2] = (byte)(user_num_max & 0xff);
            data[3] = (byte)((user_num_max & 0xff00) >> 8);
            for (ushort i = 0; i < rowsCount; i++)
            {
                ushort UID = (ushort)Convert.ToInt32(dgvPerson.Rows[i].Cells[0].Value.ToString());
                uint ID = (uint)Convert.ToInt32(dgvPerson.Rows[i].Cells[11].Value.ToString(),16);
                byte effective = 1;
                byte[] StudentID = System.Text.Encoding.Default.GetBytes(dgvPerson.Rows[i].Cells[3].Value.ToString());
                byte[] name = Encoding.GetEncoding("gbk").GetBytes(dgvPerson.Rows[i].Cells[1].Value.ToString());
                byte[] Authority = Enumerable.Repeat((byte)0xFF, 16).ToArray();
                byte[] state = Enumerable.Repeat((byte)0x00, 16).ToArray();
                byte is_time_limit = 0;
                string Eigen = dgvPerson.Rows[i].Cells[12].Value.ToString().Replace(" ", "");
                byte[] data_temp = Enumerable.Repeat((byte)0xFF, 4096).ToArray();
                /* 将用户号拷贝进数组 */
                data[(UID - 1) * one_user_lenth + 4 + 0] = (byte)((UID & 0x00ff) >> 0);
                data[(UID - 1) * one_user_lenth + 4 + 1] = (byte)((UID & 0xff00) >> 8);
                /* 将卡号拷贝进数组 */
                data[(UID - 1) * one_user_lenth + 4 + 2] = (byte)((ID & 0x000000ff) >> 0);
                data[(UID - 1) * one_user_lenth + 4 + 3] = (byte)((ID & 0x0000ff00) >> 8);
                data[(UID - 1) * one_user_lenth + 4 + 4] = (byte)((ID & 0x00ff0000) >> 16);
                data[(UID - 1) * one_user_lenth + 4 + 5] = (byte)((ID & 0xff000000) >> 24);
                /* 将有效位拷贝进数组 */
                data[(UID - 1) * one_user_lenth + 4 + 6] = effective;
                /* 将学号拷贝进数组 */
                for (int j = 0; j < StudentID.Length; j++)
                {
                    data[(UID - 1) * one_user_lenth + 4 + 7 + j] = StudentID[j];
                }
                /* 将姓名拷贝进数组 */
                for (int j = 0; j < name.Length; j++)
                {
                    data[(UID - 1) * one_user_lenth + 4 + 23 + j] = name[j];
                }
                /* 将权限拷贝进数组 */
                for (int j = 0; j < Authority.Length; j++)
                {
                    data[(UID - 1) * one_user_lenth + 4 + 31 + j] = Authority[j];
                }
                /* 将状态拷贝进数组 */
                for (int j = 0; j < state.Length; j++)
                {
                    data[(UID - 1) * one_user_lenth + 4 + 47 + j] = state[j];
                }
                /* 将是否有时间限制拷贝进数组 */
                data[(UID - 1) * one_user_lenth + 4 + 63] = is_time_limit;
                /* 将指纹特征值拷贝进数组 */
                for (int k = 0; k < 5; k++)
                {
                    for (int j = 0; j < 193; j++)
                    {
                        string str = Eigen.Substring(j * 2, 2);
                        data[((UID - 1) * one_user_lenth + 4 + 71 + j) + (k * 193)] = Convert.ToByte(str, 16);
                    }
                }
                /* 计算CRC */
                CRC_Reset();
                uint crc_temp = 0;
                for (int j = 0; j < 1026; j++)
                {
                    crc_temp = CRC_CalcCRC(data[(UID - 1) * one_user_lenth + 4 + j]);
                }
                /* 将CRC拷贝进数组 */
                data[(UID - 1) * one_user_lenth + 4 + 1036] = (byte)((crc_temp & 0x000000ff) >> 0);
                data[(UID - 1) * one_user_lenth + 4 + 1037] = (byte)((crc_temp & 0x0000ff00) >> 8);
                data[(UID - 1) * one_user_lenth + 4 + 1038] = (byte)((crc_temp & 0x00ff0000) >> 16);
                data[(UID - 1) * one_user_lenth + 4 + 1039] = (byte)((crc_temp & 0xff000000) >> 24);
            }
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        
        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dgvPerson.Rows.Count;//得到总行数 
            int cell = dgvPerson.Rows[1].Cells.Count;//得到总列数
            int rowIndex = dgvPerson.CurrentRow.Index;//获取选择行号
            string searchString = "";
            string cellString = "";


            for (int i = rowIndex + 1; i < row; i++)//得到总行数并在之内循环 
            {
                for (int j = 0; j < cell - 1; j++)//得到总列数并在之内循环 
                {
                    searchString = tbSearch.Text.Trim();
                    cellString = dgvPerson.Rows[i].Cells[j].Value.ToString().Trim();
                    //精确查找定位
                    if (cellString.IndexOf(searchString) != -1)
                    {
                        //对比TexBox中的值是否与dataGridView中的值相同（上面这句） 
                        dgvPerson.CurrentCell = dgvPerson[j, i];//定位到相同的单元格 
                        dgvPerson.Rows[i].Selected = true;//定位到行 
                        return;//返回
                    }
                }
            }
            for (int i = 0; i < rowIndex + 1; i++)//得到总行数并在之内循环 
            {
                for (int j = 0; j < cell - 1; j++)//得到总列数并在之内循环 
                {
                    searchString = tbSearch.Text.Trim();
                    cellString = dgvPerson.Rows[i].Cells[j].Value.ToString().Trim();
                    //精确查找定位
                    if (cellString.IndexOf(searchString) != -1)
                    {
                        //对比TexBox中的值是否与dataGridView中的值相同（上面这句） 
                        dgvPerson.CurrentCell = dgvPerson[j, i];//定位到相同的单元格 
                        dgvPerson.Rows[i].Selected = true;//定位到行 
                        return;//返回
                    }
                }
            }
            MessageBox.Show("找不到“" + searchString + "”", "人员维护");
        }
    }
}
