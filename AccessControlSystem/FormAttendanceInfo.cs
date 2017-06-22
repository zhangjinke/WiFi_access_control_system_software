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
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace AccessControlSystem
{
    public partial class FormAttendanceInfo : Form
    {
        public FormAttendanceInfo()
        {
            InitializeComponent();
        }
        private void addOneAttendanceInfo(string deviceName, string uID, string studentID, string userName, string doorState, string time)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建
                SQLiteTransaction tran = conn.BeginTransaction();//实例化事务
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
                cmd.Transaction = tran;
                cmd.CommandText = "insert into attendanceRecord values(@deviceName, @uID, @studentID, @userName, @state,@time)";//设置带参SQL语句  
                cmd.Parameters.AddRange(new[] {//添加参数  
                                        new SQLiteParameter("@deviceName", deviceName),//设备名称
                                        new SQLiteParameter("@uID", uID),//用户号
                                        new SQLiteParameter("@studentID", studentID),//学号
                                        new SQLiteParameter("@userName", userName),  //姓名
                                        new SQLiteParameter("@state", doorState),//进出门状态
                                        new SQLiteParameter("@time", time),//时间
                                        });
                cmd.ExecuteNonQuery();//执行查询 
                tran.Commit();//提交
                tran.Dispose();//释放tran使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }
        private void fixTime()
        {
            string deviceName = cbDeviceName.Text.ToString().Replace(" ", "");
            string fromTime = dtpFrom.Text;
            string toTime = dtpTo.Text;
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet2.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                int rowsCount = dgvAttendanceInfo.Rows.Count;//表格总行数
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令
                cmd.CommandText = "SELECT * FROM attendanceRecord";//设置带参SQL语句  
                SQLiteDataReader reader = cmd.ExecuteReader();
                //DateTime DTfirst = DateTime.Parse("2009年12月29日 06:50:54");
                //TimeSpan TPfirst = new TimeSpan(DTfirst.Ticks);
                //DateTime DTsecond = DateTime.Parse("2016年01月03日 15:54:54");
                //TimeSpan TPsecond = new TimeSpan(DTsecond.Ticks);
                //TimeSpan timeDifference = TPsecond - TPfirst;
                while (reader.Read())
                {
                    string DeviceName = reader.GetString(0);
                    string UID = reader.GetInt32(1).ToString();
                    string StudentID = reader.GetString(2);
                    string UserName = reader.GetString(3);
                    string STATE = reader.GetString(4);
                    string TIME = reader.GetString(5);
                    //DateTime DTtime = DateTime.Parse(TIME);
                    //TimeSpan TPtime = new TimeSpan(DTtime.Ticks);
                    //TPtime += timeDifference;
                    //DateTime dt = new DateTime(TPtime.Ticks);
                    //TIME = dt.ToString("yyyy年MM月dd日 HH:mm:ss");
                    addOneAttendanceInfo(DeviceName,UID,StudentID,UserName,STATE,TIME);
                } 
                cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string name = cbName.Text.Replace(" ", "");
            string studentID = cbStudentID.Text.ToString().Replace(" ", "");
            string deviceName = cbDeviceName.Text.ToString().Replace(" ", "");
            string thisDeviceName = deviceName;
            int deviceNum = 1;
            if (deviceName == "所有")
            {
                deviceNum = cbDeviceName.Items.Count - 1;
            }
            string fromTime = dtpFrom.Text;
            string toTime = dtpTo.Text;

            queryDesignee(name, studentID);
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                int rowsCount = dgvAttendanceInfo.Rows.Count;//表格总行数
                for (int i = 0; i < rowsCount; i++)
                {
                    uint reallyInAndOutTimes = 0;//总进出次数
                    uint exceptionRecord = 0;//异常记录条数
                    TimeSpan TPallTime = new TimeSpan(DateTime.Now.Ticks); ;//总时间
                    TPallTime -= TPallTime;
                    for (int j = 0; j < deviceNum;j++ )
                    {
                        if (deviceName == "所有")
                        {
                            thisDeviceName = cbDeviceName.Items[j].ToString();
                        }
                        string userName = dgvAttendanceInfo.Rows[i].Cells[1].Value.ToString();//姓名
                        if (userName.Length == 2)
                        {
                            userName = userName.Insert(1, "  ");
                        }
                        string userStudentID = dgvAttendanceInfo.Rows[i].Cells[3].Value.ToString().PadLeft(11, '0'); //学号
                        SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令
                        cmd.CommandText = "SELECT * FROM attendanceRecord WHERE studentID = @studentID AND name = @name AND deviceName like @deviceName AND time >= @fromTime AND time <= @toTime ORDER BY time";//设置带参SQL语句  
                        cmd.Parameters.AddRange(new[] {//添加参数  
                                                new SQLiteParameter("@studentID", userStudentID),  
                                                new SQLiteParameter("@name", userName),  
                                                new SQLiteParameter("@deviceName","%" + thisDeviceName + "%"),  
                                                new SQLiteParameter("@fromTime", fromTime),  
                                                new SQLiteParameter("@toTime", toTime),  
                                                });
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        DateTime DTfirst;
                        TimeSpan TPfirst = TimeSpan.Zero;
                        DateTime DTsecond;
                        TimeSpan TPsecond = TimeSpan.Zero;
                        TimeSpan TPoneDelay = TimeSpan.Zero;
                        TimeSpan TPthisDelay = TimeSpan.Zero;
                        uint InAndOutTimes = 0;
                        int state = 0;
                        string time;
                        while (reader.Read())
                        {
                            reallyInAndOutTimes++;
                            state = reader.GetInt32(6);
                            if ((state != 1) && (InAndOutTimes == 0))//从第一个进门时间开始计算
                            {
                                continue;
                            }
                            else if (Convert.ToBoolean(InAndOutTimes % 2))
                            {
                                if (state == 1) { continue; }//如果进出门次数是奇数应该是进门
                            }
                            else
                            {
                                if (state == 0) { continue; }//否则应该是出门
                            }
                            InAndOutTimes++;    //记录有效进出次数
                            time = reader.GetString(7);//读取时间
                            switch (state)//判断状态
                            {
                                case 1://进门
                                    {
                                        DTfirst = DateTime.Parse(time);
                                        TPfirst = new TimeSpan(DTfirst.Ticks);
                                        break;
                                    }
                                case 0://出门
                                    {
                                        DTsecond = DateTime.Parse(time);
                                        TPsecond = new TimeSpan(DTsecond.Ticks);
                                        TPoneDelay = TPsecond - TPfirst;
                                        if (TPoneDelay.TotalHours <= 24)
                                        {
                                            TPthisDelay += TPoneDelay;
                                        }
                                        else //如果一次记录时间超过1天，不计入总时间
                                        {
                                            exceptionRecord++;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (InAndOutTimes != 0)
                        {
                            TPallTime += TPthisDelay;
                        }
                        cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                        reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
                    }
                    string timeIn = TPallTime.Days.ToString().PadLeft(2, '0') + "天" + TPallTime.Hours.ToString().PadLeft(2, '0') + "小时" + TPallTime.Minutes.ToString().PadLeft(2, '0') + "分钟" + TPallTime.Seconds.ToString().PadLeft(2, '0') + "秒";
                    dgvAttendanceInfo.Rows[i].Cells[9].Value = reallyInAndOutTimes;
                    if (reallyInAndOutTimes != 0)
                    {
                        dgvAttendanceInfo.Rows[i].Cells[10].Value = timeIn;
                    }
                    else
                    {
                        dgvAttendanceInfo.Rows[i].Cells[10].Value = "0";
                    }
                    dgvAttendanceInfo.Rows[i].Cells[11].Value = exceptionRecord;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        /// <summary>
        /// 指定学号和姓名查询用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="studentID"></param>
        private void queryDesignee(string name, string studentID)
        {
            name = name.Replace(" ", "");
            studentID = studentID.Replace(" ", "");
            if (name == "所有") { name = ""; }
            if (studentID == "所有") { studentID = ""; }
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            dgvAttendanceInfo.Rows.Clear();             //清空表格
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  

                string sql = "SELECT * FROM personInfo WHERE studentID like '%" + studentID + "%' AND name like '%" + name + "%'";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    int index = dgvAttendanceInfo.Rows.Add();

                    DateTime DTbirthDay = DateTime.Parse(reader.GetString(8));
                    TimeSpan TSbirthDay = new TimeSpan(DTbirthDay.Ticks);
                    TimeSpan Now = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan TSage = Now - TSbirthDay;

                    int age = (int)(TSage.TotalDays / 365);           //计算年龄
                    dgvAttendanceInfo.Rows[index].Cells[0].Value = reader.GetInt32(0);  //用户号
                    dgvAttendanceInfo.Rows[index].Cells[1].Value = reader.GetString(6); //姓名
                    dgvAttendanceInfo.Rows[index].Cells[2].Value = reader.GetString(7); //性别
                    dgvAttendanceInfo.Rows[index].Cells[3].Value = reader.GetString(3); //学号
                    dgvAttendanceInfo.Rows[index].Cells[4].Value = age; //年龄
                    dgvAttendanceInfo.Rows[index].Cells[5].Value = reader.GetString(25); //录入日期
                    dgvAttendanceInfo.Rows[index].Cells[6].Value = reader.GetString(10); //QQ
                    dgvAttendanceInfo.Rows[index].Cells[7].Value = reader.GetString(9); //电话号码
                    dgvAttendanceInfo.Rows[index].Cells[8].Value = reader.GetString(12);  //权限
                }
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
        private void cbName_DropDown(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            cbName.Items.Clear();             //清空表格
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM personInfo";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM personInfo";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    cbName.Items.Add(reader.GetString(6)); //姓名
                }
                cbName.Items.Add("所有"); //姓名
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        private void cbStudentID_DropDown(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\personalInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            cbStudentID.Items.Clear();             //清空表格
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT COUNT(*) FROM personInfo";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总人数
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM personInfo";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    cbStudentID.Items.Add(reader.GetString(3)); //学号
                }
                cbStudentID.Items.Add("所有"); //学号
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        private void dgvAttendanceInfo_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvAttendanceInfo.Rows[e.RowIndex].Selected == false)
                    {
                        dgvAttendanceInfo.ClearSelection();
                        dgvAttendanceInfo.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvAttendanceInfo.SelectedRows.Count == 1)
                    {
                        dgvAttendanceInfo.CurrentCell = dgvAttendanceInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    cmsdgvAttendanceInfo.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }

        private void 查看详细记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fromTime = dtpFrom.Text;
            string toTime = dtpTo.Text;

            FormMinuteAttendanceInfo formMinuteAttendanceInfo = new FormMinuteAttendanceInfo();
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\attendanceSheet.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  

            int rows = dgvAttendanceInfo.SelectedRows.Count;//获取选中总行数
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                for (int j = 0; j < rows; j++)
                {
                    String userStudentID = dgvAttendanceInfo.SelectedRows[j].Cells[3].Value.ToString();
                    String userName = dgvAttendanceInfo.SelectedRows[j].Cells[1].Value.ToString();
                    if (userName.Length == 2)
                    {
                        userName = userName.Insert(1, "  ");
                    }
                    SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令
                    cmd.CommandText = "SELECT * FROM attendanceRecord WHERE studentID = @studentID AND name = @name AND time >= @fromTime AND time <= @toTime ORDER BY time";//设置带参SQL语句  
                    cmd.Parameters.AddRange(new[] {//添加参数  
                                            new SQLiteParameter("@studentID", userStudentID),  
                                            new SQLiteParameter("@name", userName),  
                                            new SQLiteParameter("@fromTime", fromTime),  
                                            new SQLiteParameter("@toTime", toTime),  
                                            });
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int index = formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows.Add();
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[0].Value = reader.GetString(5);//姓名
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[1].Value = reader.GetString(1);//设备名称
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[2].Value = reader.GetInt32(3);//用户号
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[3].Value = reader.GetString(4);//学号
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[4].Value = reader.GetInt32(6) == 1 ? "进门" : "出门";//状态
                        formMinuteAttendanceInfo.dgvMinuteAttendanceInfo.Rows[index].Cells[5].Value = reader.GetString(7);//时间
                    }
                }
                formMinuteAttendanceInfo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbDeviceName_DropDown(object sender, EventArgs e)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\DeviceInfo.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            cbDeviceName.Items.Clear();
            try
            {
                conn.Open();//打开数据库，若文件不存在会自动创建  
                string sql = "SELECT * FROM device";
                SQLiteCommand cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    cbDeviceName.Items.Add(reader.GetString(2)); //设备名称
                }
                cbDeviceName.Items.Add("所有"); //设备名称
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void FormAttendanceInfo_Load(object sender, EventArgs e)
        {
            cbTime.Text = "所有";
            cbDeviceName_DropDown(sender, e);
            dtpFrom.Value = DateTimePicker.MinimumDateTime;
            dtpTo.Value = DateTimePicker.MaximumDateTime;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            DataGridViewToExcel(dgvAttendanceInfo);
        }
        /// <summary>   
        ///方法，导出DataGridView中的数据到Excel文件   
        /// </summary>   
        /// <remarks>  
        /// add com "Microsoft Excel 11.0 Object Library"  
        /// using Excel=Microsoft.Office.Interop.Excel;  
        /// using System.Reflection;  
        /// </remarks>  
        /// <param name= "dgv"> DataGridView </param>   
        public void DataGridViewToExcel(DataGridView dgv)
        {
            #region   验证可操作性

            string fromTime = dtpFrom.Text;
            string toTime = dtpTo.Text;
            //申明保存对话框
            SaveFileDialog dlg = new SaveFileDialog();
            //默认文件名
            dlg.FileName = fromTime + "-" + toTime;
            //默认文件后缀   
            dlg.DefaultExt = "xlsx";
            //文件后缀列表   
            dlg.Filter = "EXCEL文件(*.xlsx)|*.xlsx";
            //默然路径是系统当前路径   
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            //定义表格内数据的行数和列数   
            int rowscount = dgv.Rows.Count;
            int colscount = dgv.Columns.Count;
            //行数必须大于0   
            if (rowscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数必须大于0   
            if (colscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //行数不可以大于65536   
            if (rowscount > 65536)
            {
                MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数不可以大于255   
            if (colscount > 255)
            {
                MessageBox.Show("数据记录行数太多，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //打开保存对话框   
            if (dlg.ShowDialog() == DialogResult.Cancel) return;
            //返回文件路径   
            string fileNameString = dlg.FileName;
            //验证strFileName是否为空或值无效   
            if (fileNameString.Trim() == " ")
            { return; }
            //验证以fileNameString命名的文件是否存在，如果存在删除它   
            FileInfo file = new FileInfo(fileNameString);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "删除失败 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            #endregion
            Excel.Application objExcel = null;
            Excel.Workbook objWorkbook = null;
            Excel.Worksheet objsheet = null;
            try
            {
                //申明对象   
                objExcel = new Microsoft.Office.Interop.Excel.Application();
                objWorkbook = objExcel.Workbooks.Add(Missing.Value);
                objsheet = (Excel.Worksheet)objWorkbook.ActiveSheet;
                //设置EXCEL不可见   
                objExcel.Visible = false;

                //向Excel中写入表格的表头   
                int displayColumnsCount = 1;
                for (int i = 0; i <= dgv.ColumnCount - 1; i++)
                {
                    if (dgv.Columns[i].Visible == true)
                    {
                        objExcel.Cells[1, displayColumnsCount] = dgv.Columns[i].HeaderText.Trim();
                        displayColumnsCount++;
                    }
                }
                //设置进度条   
                tempProgressBar.Refresh();
                tempProgressBar.Visible = true;
                tempProgressBar.Minimum = 1;
                tempProgressBar.Maximum = dgv.RowCount;
                tempProgressBar.Step = 1;   
                //向Excel中逐行逐列写入表格中的数据   
                for (int row = 0; row <= dgv.RowCount - 1; row++)
                {
                    tempProgressBar.PerformStep();   
                    displayColumnsCount = 1;
                    for (int col = 0; col < colscount; col++)
                    {
                        if (dgv.Columns[col].Visible == true)
                        {
                            try
                            {
                                objExcel.Cells[row + 2, displayColumnsCount] = dgv.Rows[row].Cells[col].Value.ToString().Trim();
                                displayColumnsCount++;
                            }
                            catch (Exception)
                            {

                            }

                        }
                    }
                }
                objExcel.Cells.Columns.AutoFit();
                //隐藏进度条   
                tempProgressBar.Visible = false;   
                //保存文件   
                objWorkbook.SaveAs(fileNameString, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "警告 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            finally
            {
                //关闭Excel应用   
                if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
                if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
                if (objExcel != null) objExcel.Quit();

                objsheet = null;
                objWorkbook = null;
                objExcel = null;
            }
            MessageBox.Show(fileNameString + "\r\n导出完毕! ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            switch (cbTime.Text)
            {
                case "最近1天":
                    {
                        dtpFrom.Value = DateTime.Now.AddDays(-1);
                        dtpTo.Value = DateTime.Now;
                        break;
                    }
                case "最近1周":
                    {
                        dtpFrom.Value = DateTime.Now.AddDays(-7);
                        dtpTo.Value = DateTime.Now;
                        break;
                    }
                case "最近1月":
                    {
                        dtpFrom.Value = DateTime.Now.AddDays(-30);
                        dtpTo.Value = DateTime.Now;
                        break;
                    }
                case "最近1年":
                    {
                        dtpFrom.Value = DateTime.Now.AddDays(-365);
                        dtpTo.Value = DateTime.Now;
                        break;
                    }
                case "所有":
                    {
                        dtpFrom.Value = DateTimePicker.MinimumDateTime;
                        dtpTo.Value = DateTimePicker.MaximumDateTime;
                        break;
                    }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FormAttendanceInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; //取消关闭事件
        }
    }
}
