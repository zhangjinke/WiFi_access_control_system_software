using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using AccessControlSystem.Model;
using AccessControlSystem.Lib;

namespace AccessControlSystem
{
    public partial class FormPersonnelManagement : Form
    {
        PersonnelManagement personnelManagement = new PersonnelManagement();
        PersonnelManagement.PersonInfo person = new PersonnelManagement.PersonInfo(5);

        string fPath;   //头像文件路径

        public FormPersonnelManagement()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体构造函数，检测数据库并给表格添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPersonnelManagement_Load(object sender, EventArgs e)
        {
            CreateStm32Database createStm32Database = new CreateStm32Database();
            createStm32Database.CreateMemberBin();

            reLoadDataGridView();//重新给表格添加数据
            cleanPicture();
            cbSex.SelectedIndex = 0;
            //cbBuilding.SelectedIndex = 7;
            //cbFloor.SelectedIndex = 1;
            //cbInstitute.SelectedIndex = 0;
            //cbMajor.SelectedIndex = 0;
            cbActiveState_CheckedChanged(sender, e);
            SchoolInfoInit();
            新增toolStripMenuItem1_Click(sender, e);//初始化文本框数据
        }

        private void SchoolInfoInit()
        {
            SchoolInfo schoolInfo = new SchoolInfo();
            SchoolInfo.BuildingInfo buildingInfo = new SchoolInfo.BuildingInfo();
            SchoolInfo.InstituteInfo instituteInfo = new SchoolInfo.InstituteInfo();

            cbBuilding.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFloor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbInstitute.Items.Clear();
            cbBuilding.Items.Clear();
            for (int i = 0; i < schoolInfo.InstituteList.Count; i++)
            {
                instituteInfo = schoolInfo.InstituteList[i];
                cbInstitute.Items.Add(instituteInfo.name);
            }
            for (int i = 0; i < schoolInfo.BuildingList.Count; i++)
            {
                buildingInfo = schoolInfo.BuildingList[i];
                cbBuilding.Items.Add(buildingInfo.name);
            }
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
            dgvPerson.Rows.Clear();             //清空表格
            int RowCount = personnelManagement.PersonList.Count;   //获取总人数
            lbHeadcount.Text = "总人数：" + RowCount.ToString();
            for (int idx = 0; idx < RowCount; idx++)
            {
                int index = dgvPerson.Rows.Add();

                DateTime DTbirthDay = DateTime.Parse(personnelManagement.PersonList[idx].birthday);
                TimeSpan TSbirthDay = new TimeSpan(DTbirthDay.Ticks);
                TimeSpan Now = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan TSage = Now - TSbirthDay;

                int age = (int)(TSage.TotalDays / 365);           //计算年龄
                dgvPerson.Rows[index].Cells[0].Value = personnelManagement.PersonList[idx].uID;  //用户号
                dgvPerson.Rows[index].Cells[1].Value = personnelManagement.PersonList[idx].activeState; //激活状态
                dgvPerson.Rows[index].Cells[2].Value = personnelManagement.PersonList[idx].name; //姓名
                dgvPerson.Rows[index].Cells[3].Value = personnelManagement.PersonList[idx].sex; //性别
                dgvPerson.Rows[index].Cells[4].Value = personnelManagement.PersonList[idx].authority;  //权限
                dgvPerson.Rows[index].Cells[5].Value = personnelManagement.PersonList[idx].isLimitTime;  //时间限制
                dgvPerson.Rows[index].Cells[6].Value = personnelManagement.PersonList[idx].limitTime;  //有效期
                dgvPerson.Rows[index].Cells[7].Value = personnelManagement.PersonList[idx].studentID; //学号
                dgvPerson.Rows[index].Cells[8].Value = personnelManagement.PersonList[idx].major; //学院
                dgvPerson.Rows[index].Cells[9].Value = personnelManagement.PersonList[idx].major; //专业
                dgvPerson.Rows[index].Cells[10].Value = age; //年龄
                dgvPerson.Rows[index].Cells[11].Value = personnelManagement.PersonList[idx].birthday; //出生日期
                dgvPerson.Rows[index].Cells[12].Value = personnelManagement.PersonList[idx].recodeDate; //录入日期
                dgvPerson.Rows[index].Cells[13].Value = personnelManagement.PersonList[idx].QQ; //QQ
                dgvPerson.Rows[index].Cells[14].Value = personnelManagement.PersonList[idx].weiXin; //微信
                dgvPerson.Rows[index].Cells[15].Value = personnelManagement.PersonList[idx].tel; //电话号码
                dgvPerson.Rows[index].Cells[16].Value = personnelManagement.PersonList[idx].dormitory; //寝室
                dgvPerson.Rows[index].Cells[17].Value = personnelManagement.PersonList[idx].cardID.ToString("X8");    //卡号
                dgvPerson.Rows[index].Cells[18].Value = personnelManagement.PersonList[idx].eigen[0];   //指纹1
                dgvPerson.Rows[index].Cells[19].Value = personnelManagement.PersonList[idx].eigen[1];   //指纹1
                dgvPerson.Rows[index].Cells[20].Value = personnelManagement.PersonList[idx].eigen[2];   //指纹1
                dgvPerson.Rows[index].Cells[21].Value = personnelManagement.PersonList[idx].eigen[3];   //指纹1
                dgvPerson.Rows[index].Cells[22].Value = personnelManagement.PersonList[idx].eigen[4];   //指纹1
                dgvPerson.Rows[index].Cells[23].Value = personnelManagement.PersonList[idx].eigenNum[0].ToString() + "-" +
                                                        personnelManagement.PersonList[idx].eigenNum[1].ToString() + "-" +
                                                        personnelManagement.PersonList[idx].eigenNum[2].ToString() + "-" +
                                                        personnelManagement.PersonList[idx].eigenNum[3].ToString() + "-" +
                                                        personnelManagement.PersonList[idx].eigenNum[4].ToString();   //指纹号
            }
        }
        private void cleanPicture()
        {
            try
            {
                string path = Environment.CurrentDirectory + @"\picture";
                DirectoryInfo TheFolder = new DirectoryInfo(path);
                foreach (FileInfo NextFile in TheFolder.GetFiles())//遍历文件
                {
                    string fileName = (NextFile.Name);
                    if (fileName == "defaultAvatar.jpg") { continue; }

                    string[] s = fileName.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        NextFile.Delete();
                        continue;
                    }
                    UInt32 uID = (UInt32)Convert.ToInt32(s[0]);//用户号
                    string userName = s[1].Replace(".jpg", "");//姓名
                    int index = personnelManagement.uIDtoIndex(uID);

                    if ((index < 0) || (personnelManagement.PersonList[index].name != userName))
                    {
                        NextFile.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
        }
        /// <summary>
        /// 清空所有文本框的数据，方便下次添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新增toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SchoolInfoInit();
            string cellString = "";
            int rowsCount = dgvPerson.Rows.Count;//获取总人数
            if (rowsCount == 0)
            {
                tbUID.Text = 1.ToString();
            }
            else
            { 
                cellString = dgvPerson.Rows[rowsCount-1].Cells[0].Value.ToString().Trim();//初始化为最后一行的用户号
            }
            tbUID.Text = (rowsCount + 1).ToString();
            tbName.Text = null;
            cbSex.Text = null;
            tbStudentID.Text = null;
            tbQQ.Text = null;
            tbTel.Text = null;
            cbBuilding.Text = null; cbFloor.Text = null; tbDomitory.Text = null;
            cbAuthority = CheckBoxComboBoxOP.checkItem(cbAuthority, "");
            cbInstitute.Text = null;
            cbMajor.Text = null;
            tbWeiXin.Text = null;
            tbRFID.Text = null;
            tbEigen1.Text = null;
            tbEigen2.Text = null;
            tbEigen3.Text = null;
            tbEigen4.Text = null;
            tbEigen5.Text = null;
            dtpBirthday.Value = DateTime.Now;
            pbPortrait.Image = null;
            fPath = Environment.CurrentDirectory + @"\picture\defaultAvatar.jpg";
            Image pic = Image.FromFile(fPath);
            pbPortrait.Image = pic;

            if (cellString != "")
            { 
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
        }
        /// <summary>
        /// 将指定人员的数据从数据库读取并显示，方便编辑人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SchoolInfoInit();
            PersonnelManagement personnelManagement = new PersonnelManagement();
            int rowIndex = dgvPerson.CurrentRow.Index;     //获取选择行号
            tbUID.Text = dgvPerson.Rows[rowIndex].Cells[0].Value.ToString();
            uint uID = (UInt32)Convert.ToInt32(tbUID.Text);
            int index = personnelManagement.uIDtoIndex(uID);
            if (index < 0)
            {
                output("数据库中没有指定ID用户");
                return;
            }
            tbName.Text = personnelManagement.PersonList[index].name;
            cbSex.Text = personnelManagement.PersonList[index].sex;
            tbStudentID.Text = personnelManagement.PersonList[index].studentID;

            string domitory = personnelManagement.PersonList[index].dormitory;
            byte one = (byte)domitory.LastIndexOf("）");
            string Building = domitory.Substring(0, one + 1);
            string Floor = domitory.Substring(one + 1, 1);
            tbDomitory.Text = domitory.Substring(one + 2, 3);

            if (cbBuilding.Items.Contains(Building))
            {
                cbBuilding.DropDownStyle = ComboBoxStyle.DropDownList;
                cbBuilding.Text = Building;
            }
            else
            {
                cbBuilding.DropDownStyle = ComboBoxStyle.Simple;
                cbBuilding.Text = Building;
            }

            if (cbFloor.Items.Contains(Floor))
            {
                cbFloor.DropDownStyle = ComboBoxStyle.DropDownList;
                cbFloor.Text = Floor;
            }
            else
            {
                cbFloor.DropDownStyle = ComboBoxStyle.Simple;
                cbFloor.Text = Floor;
            }

            cbActiveState.Checked = personnelManagement.PersonList[index].activeState == 0 ? false : true;

            string authority = personnelManagement.PersonList[index].authority;
            cbAuthority = CheckBoxComboBoxOP.checkItem(cbAuthority, authority);

            string major = personnelManagement.PersonList[index].major;
            one = (byte)major.LastIndexOf(" ");
            string Institute = major.Substring(0, one);
            string Major = major.Substring(one + 1, major.Length - one - 1);

            if (cbInstitute.Items.Contains(Institute))
            {
                cbInstitute.DropDownStyle = ComboBoxStyle.DropDownList;
                cbInstitute.Text = Institute;
            }
            else
            {
                cbInstitute.DropDownStyle = ComboBoxStyle.Simple;
                cbInstitute.Text = Institute;
            }

            if (cbMajor.Items.Contains(Major))
            {
                cbMajor.DropDownStyle = ComboBoxStyle.DropDownList;
                cbMajor.Text = Major;
            }
            else
            {
                cbMajor.DropDownStyle = ComboBoxStyle.Simple;
                cbMajor.Text = Major;
            }

            //cbInstitute.Text = major.Substring(0, one);
            //cbMajor.Text = major.Substring(one + 1, major.Length - one - 1);

            tbQQ.Text = personnelManagement.PersonList[index].QQ;
            tbWeiXin.Text = personnelManagement.PersonList[index].weiXin;
            tbTel.Text = personnelManagement.PersonList[index].tel;
            dtpBirthday.Value = DateTime.Parse(personnelManagement.PersonList[index].birthday);
            cbIsLimitTime.Checked = personnelManagement.PersonList[index].isLimitTime == 0 ? false : true;
            dtpLimitTime.Value = DateTime.Parse(personnelManagement.PersonList[index].limitTime);

            tbRFID.Text = personnelManagement.PersonList[index].cardID.ToString("X8"); ;
            tbEigen1.Text = personnelManagement.PersonList[index].eigen[0];
            tbEigen2.Text = personnelManagement.PersonList[index].eigen[1];
            tbEigen3.Text = personnelManagement.PersonList[index].eigen[2];
            tbEigen4.Text = personnelManagement.PersonList[index].eigen[3];
            tbEigen5.Text = personnelManagement.PersonList[index].eigen[4];
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
            if (cbBuilding.Text == "") { MessageBox.Show("寝室楼" + "不能为空"); return; }
            if (cbFloor.Text == "") { MessageBox.Show("楼号" + "不能为空"); return; }
            if (tbDomitory.Text == "") { MessageBox.Show("寝室门牌号" + "不能为空"); return; }
            if (cbAuthority.Text == "") { MessageBox.Show(lbAuthority.Text + "不能为空"); return; }

            if (cbInstitute.Text == "") { MessageBox.Show(lbInstitute.Text + "不能为空"); return; }
            if (cbMajor.Text == "") { MessageBox.Show(lbMajor.Text + "不能为空"); return; }
            if (tbQQ.Text == "") { MessageBox.Show(lbQQ.Text + "不能为空"); return; }
            if (tbWeiXin.Text == "") { MessageBox.Show(lbWeiXin.Text + "不能为空"); return; }
            if (tbTel.Text == "") { MessageBox.Show(lbTel.Text + "不能为空"); return; }
            if (dtpBirthday.Text == "") { MessageBox.Show(lbBirthday.Text + "不能为空"); return; }
            if (dtpLimitTime.Text == "") { MessageBox.Show(lbLimitTime.Text + "不能为空"); return; }

            if (tbRFID.Text == "") { MessageBox.Show(lbID.Text + "不能为空"); return; }
            if (tbRFID.Text.Replace(" ", "").Length != 8) { MessageBox.Show(lbID.Text + "长度错误"); return; }
            if (fPath == null) { MessageBox.Show("请选择相片"); btnOpen_Click(sender, e); return; }
            try
            {            
                person.uID         = (UInt32)Convert.ToInt32(tbUID.Text.Replace(" ", ""), 10);
                person.cardID      = (UInt32)Convert.ToInt32(tbRFID.Text.Replace(" ", ""), 16);
                person.activeState = (UInt32)(cbActiveState.Checked ? 1 : 0);
                person.studentID   = tbStudentID.Text.Replace(" ", "");
                person.dormitory   = cbBuilding.Text + cbFloor.Text + tbDomitory.Text.Replace(" ", "");
                person.major       = cbInstitute.Text.Replace(" ", "") + " " + cbMajor.Text.Replace(" ", "");
                person.name        = tbName.Text.Replace(" ", "");
                person.sex         = cbSex.Text;
                person.birthday    = dtpBirthday.Text;
                person.tel         = tbTel.Text.Replace(" ", "");
                person.QQ          = tbQQ.Text.Replace(" ", "");
                person.weiXin      = tbWeiXin.Text.Replace(" ", "");
                person.authority   = cbAuthority.Text;
                person.isLimitTime = (UInt32)(cbIsLimitTime.Checked ? 1 : 0);
                person.limitTime   = person.isLimitTime == 0 ?
                                     DateTime.Parse("2099年12月31日 23:59:59").
                                     ToString("yyyy年MM月dd日 HH:mm:ss") :
                                     dtpLimitTime.Value.ToString("yyyy年MM月dd日 HH:mm:ss");

                /* 分配指纹号 */
                int i = 0;
                foreach (TabPage page in tbcEigen.TabPages)
                {
                    foreach (Control control in page.Controls)
                    {
                        if (control is TextBox)
                        {
                            person.eigenNum[i] = 0;
                            string eigen = ((TextBox)control).Text.Replace(" ","");

                            if (eigen == "") /* 指纹特征值为空 */
                            {
                                person.eigenNum[i] = 0;
                                person.eigen[i] = "";
                            }
                            else /* 指纹特征值不为空，分配指纹号 */
                            {
                                UInt32 j;

                                if (eigen.Length != 193 * 2) 
                                {
                                    MessageBox.Show("指纹" + (i + 1).ToString() + "长度错误"); 
                                    return; 
                                }

                                /* 遍历每个指纹号，查看是否使用 */
                                for (j = 1; j <= 4000; j++)
                                {
                                    bool is_this_use = false;

                                    /* 查看当前指纹是否使用 */
                                    if (personnelManagement.finger_id_check(j) == false)
                                    {

                                        /* 查看本次添加是否使用，如果本次已经使用，继续寻找下一个未使用的指纹号 */
                                        for (int k = 0; k < person.eigenNum.Length; k++)
                                        {
                                            if (person.eigenNum[k] == j)
                                            {
                                                is_this_use = true;
                                                break;
                                            }
                                        }
                                        if (is_this_use)
                                        {
                                            continue;
                                        }

                                        /* 找到未使用的指纹号 */
                                        person.eigenNum[i] = j;
                                        person.eigen[i] = ((TextBox)control).Text;
                                        break;
                                    }
                                }
                                if (person.eigenNum[i] != j)
                                {
                                    MessageBox.Show("指纹容量满！"); 
                                    return;
                                }
                            }
                        }
                    }
                    i++;
                }

                person.recodeDate  = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
                person.resv0       = 0;
                person.resv1       = 0;
                person.resv2       = "";
                person.resv3       = "";
                person.resv4       = "";

                /* 检查用户是否存在 */
                if (personnelManagement.user_id_check(person.uID))
                {
                    DialogResult dr = MessageBox.Show("号用户已存在，是否覆盖", "确认覆盖", MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr != DialogResult.Yes)
                    {
                        return; /* 不点击确认按钮直接返回 */
                    }
                    else
                    {
                        if (personnelManagement.UpdatePerson(person))
                        {
                            output("修改" + tbUID.Text + "号用户" + tbName.Text + "成功");
                        }
                        else
                        {
                            output("修改" + tbUID.Text + "号用户" + tbName.Text + "失败");
                        }
                    }
                }
                else
                { 
                    if (personnelManagement.AddPerson(person))
                    {
                        output("添加" + tbUID.Text + "号用户" + tbName.Text + "成功");
                    }
                    else
                    {
                        output("添加" + tbUID.Text + "号用户" + tbName.Text + "失败");
                    }
                }
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            reLoadDataGridView();//重新给表格添加数据
            String sourcePath = fPath;
            String targetPath = Environment.CurrentDirectory + @"\picture\" + tbUID.Text.PadLeft(4,'0') + "_" + tbName.Text + ".jpg";
            if (sourcePath != fPath)
            { 
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                try { System.IO.File.Copy(sourcePath, targetPath, isrewrite); }
                catch (Exception ex)
                {
                    output(ex.Message);
                }
            }
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
            if (dr != DialogResult.Yes) { return; }//不点击确认按钮直接返回
              
            int rows = dgvPerson.SelectedRows.Count;//获取选中总行数
            try
            {
                for (int j = 0; j < rows; j++)
                {
                    UInt32 uId = (UInt32)int.Parse(dgvPerson.SelectedRows[j].Cells[0].Value.ToString());//获取用户号
                    int index = personnelManagement.uIDtoIndex(uId);//获取列表索引
                    personnelManagement.DelPerson(personnelManagement.PersonList[index]);
                    String uID = dgvPerson.SelectedRows[j].Cells[0].Value.ToString();
                    String name = dgvPerson.SelectedRows[j].Cells[2].Value.ToString();
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
        private void cbActiveState_CheckedChanged(object sender, EventArgs e)
        {
            if (cbActiveState.Checked)
            {
                tbUID.Enabled = true;
                tbName.Enabled = true;
                cbSex.Enabled = true;
                tbStudentID.Enabled = true;
                cbBuilding.Enabled = true;
                cbFloor.Enabled = true;
                tbDomitory.Enabled = true;
                cbAuthority.Enabled = true;
                cbInstitute.Enabled = true;
                cbMajor.Enabled = true;
                tbQQ.Enabled = true;
                tbWeiXin.Enabled = true;
                tbTel.Enabled = true;
                dtpBirthday.Enabled = true;
                tbRFID.Enabled = true;
                cbIsLimitTime.Enabled = true;
                btnOpen.Enabled = true;
                tbcEigen.Enabled = true;
                cbIsLimitTime_CheckedChanged(sender, e);
            }
            else
            {
                tbUID.Enabled = false;
                tbName.Enabled = false;
                cbSex.Enabled = false;
                tbStudentID.Enabled = false;
                cbBuilding.Enabled = false;
                cbFloor.Enabled = false;
                tbDomitory.Enabled = false;
                cbAuthority.Enabled = false;
                cbInstitute.Enabled = false;
                cbMajor.Enabled = false;
                tbQQ.Enabled = false;
                tbWeiXin.Enabled = false;
                tbTel.Enabled = false;
                dtpBirthday.Enabled = false;
                tbRFID.Enabled = false;
                cbIsLimitTime.Enabled = false;
                btnOpen.Enabled = false;
                tbcEigen.Enabled = false;
                dtpLimitTime.Enabled = false;
            }
        }
        private void cbIsLimitTime_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsLimitTime.Checked)
            {
                dtpLimitTime.Enabled = true;
                dtpLimitTime.Value = DateTime.Now;
            }
            else
            {
                dtpLimitTime.Enabled = false;
                dtpLimitTime.Value = DateTime.Parse("2099年12月31日 23:59:59");
            }
        }
        private void btnDownloadAll_Click(object sender, EventArgs e)
        {
            //Thread downloadAll = new Thread(DownloadAll);
            //downloadAll.Priority = ThreadPriority.Highest;
            //downloadAll.Start();
        }
        /// <summary>
        /// 创建下载函数的线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            //Thread download = new Thread(Download);
            //download.Priority = ThreadPriority.Highest;
            //download.Start();
        }

        private void cbInstitute_TextChanged(object sender, EventArgs e)
        {
            SchoolInfo schoolInfo = new SchoolInfo();
            SchoolInfo.InstituteInfo instituteInfo = new SchoolInfo.InstituteInfo();

            cbMajor.Items.Clear(); /* 清除原有项 */
            for (int i = 0; i < schoolInfo.InstituteList.Count; i++)
            {
                instituteInfo = schoolInfo.InstituteList[i];
                if (instituteInfo.name == cbInstitute.Text)
                {
                    for (int j = 0; j < instituteInfo.major.Length; j++)
                    { 
                        cbMajor.Items.Add(instituteInfo.major[j]);
                    }
                }
            }
        }
        /// <summary>
        /// 由选择的寝室楼切换相应的楼号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBuilding_TextChanged(object sender, EventArgs e)
        {
            SchoolInfo schoolInfo = new SchoolInfo();
            SchoolInfo.BuildingInfo buildingInfo = new SchoolInfo.BuildingInfo();

            cbFloor.Items.Clear();      //清除已有的项
            for (int i = 0; i < schoolInfo.BuildingList.Count; i++)
            {
                buildingInfo = schoolInfo.BuildingList[i];
                if (buildingInfo.name == cbBuilding.Text)
                {
                    for (int j = 0; j < buildingInfo.floor.Length; j++)
                    {
                        cbFloor.Items.Add(buildingInfo.floor[j]);
                    }
                }
            }
        }

        private void FormPersonnelManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; //取消关闭事件
        }
    }
}
