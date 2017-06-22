namespace AccessControlSystem
{
    partial class FormPersonnelManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPersonnelManagement));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新增toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.cmsdgvPerson = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmstbLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清除日志信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbHeadcount = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbSex = new System.Windows.Forms.Label();
            this.lbBirthday = new System.Windows.Forms.Label();
            this.lbStudentID = new System.Windows.Forms.Label();
            this.lbQQ = new System.Windows.Forms.Label();
            this.lbTel = new System.Windows.Forms.Label();
            this.lbAuthority = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lbDomitory = new System.Windows.Forms.Label();
            this.tbStudentID = new System.Windows.Forms.TextBox();
            this.tbDomitory = new System.Windows.Forms.TextBox();
            this.tbTel = new System.Windows.Forms.TextBox();
            this.tbQQ = new System.Windows.Forms.TextBox();
            this.gbPhoto = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.pbPortrait = new System.Windows.Forms.PictureBox();
            this.gbIDandEigen = new System.Windows.Forms.GroupBox();
            this.tbcEigen = new System.Windows.Forms.TabControl();
            this.tbpEigen1 = new System.Windows.Forms.TabPage();
            this.tbEigen1 = new System.Windows.Forms.TextBox();
            this.tbpEigen2 = new System.Windows.Forms.TabPage();
            this.tbEigen2 = new System.Windows.Forms.TextBox();
            this.tbpEigen3 = new System.Windows.Forms.TabPage();
            this.tbEigen3 = new System.Windows.Forms.TextBox();
            this.tbpEigen4 = new System.Windows.Forms.TabPage();
            this.tbEigen4 = new System.Windows.Forms.TextBox();
            this.tbpEigen5 = new System.Windows.Forms.TabPage();
            this.tbEigen5 = new System.Windows.Forms.TextBox();
            this.lbID = new System.Windows.Forms.Label();
            this.lbEigen = new System.Windows.Forms.Label();
            this.tbRFID = new System.Windows.Forms.TextBox();
            this.cbSex = new System.Windows.Forms.ComboBox();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.lbUID = new System.Windows.Forms.Label();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.cbBuilding = new System.Windows.Forms.ComboBox();
            this.cbFloor = new System.Windows.Forms.ComboBox();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.btnDownloadAll = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.cbDeviceList = new System.Windows.Forms.ComboBox();
            this.lbSelectDevice = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvPerson = new System.Windows.Forms.DataGridView();
            this.cbAuthority = new PresentationControls.CheckBoxComboBox();
            this.cbActiveState = new System.Windows.Forms.CheckBox();
            this.dtpLimitTime = new System.Windows.Forms.DateTimePicker();
            this.lbLimitTime = new System.Windows.Forms.Label();
            this.cbIsLimitTime = new System.Windows.Forms.CheckBox();
            this.tbWeiXin = new System.Windows.Forms.TextBox();
            this.lbWeiXin = new System.Windows.Forms.Label();
            this.lbMajor = new System.Windows.Forms.Label();
            this.cbMajor = new System.Windows.Forms.ComboBox();
            this.cbInstitute = new System.Windows.Forms.ComboBox();
            this.lbInstitute = new System.Windows.Forms.Label();
            this.UID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeState = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLimitTime = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LimitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.institute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.major = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.birthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recodeData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weiXin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dormitory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigen1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigen2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigen3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigen4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigen5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eigenNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.cmsdgvPerson.SuspendLayout();
            this.cmstbLog.SuspendLayout();
            this.gbPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortrait)).BeginInit();
            this.gbIDandEigen.SuspendLayout();
            this.tbcEigen.SuspendLayout();
            this.tbpEigen1.SuspendLayout();
            this.tbpEigen2.SuspendLayout();
            this.tbpEigen3.SuspendLayout();
            this.tbpEigen4.SuspendLayout();
            this.tbpEigen5.SuspendLayout();
            this.gbLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerson)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增toolStripMenuItem1,
            this.编辑ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.tbSearch,
            this.查找ToolStripMenuItem,
            this.导入ToolStripMenuItem,
            this.导出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1144, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新增toolStripMenuItem1
            // 
            this.新增toolStripMenuItem1.Name = "新增toolStripMenuItem1";
            this.新增toolStripMenuItem1.Size = new System.Drawing.Size(44, 23);
            this.新增toolStripMenuItem1.Text = "新增";
            this.新增toolStripMenuItem1.Click += new System.EventHandler(this.新增toolStripMenuItem1_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.编辑ToolStripMenuItem.Text = "编辑";
            this.编辑ToolStripMenuItem.Click += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(100, 23);
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.查找ToolStripMenuItem.Text = "查找";
            this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItem_Click);
            // 
            // 导入ToolStripMenuItem
            // 
            this.导入ToolStripMenuItem.Name = "导入ToolStripMenuItem";
            this.导入ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.导入ToolStripMenuItem.Text = "导入";
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.导出ToolStripMenuItem.Text = "导出";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 27);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 523);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // cmsdgvPerson
            // 
            this.cmsdgvPerson.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增ToolStripMenuItem,
            this.编辑ToolStripMenuItem1,
            this.保存ToolStripMenuItem1,
            this.删除ToolStripMenuItem1});
            this.cmsdgvPerson.Name = "contextMenuStrip";
            this.cmsdgvPerson.Size = new System.Drawing.Size(101, 92);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.新增toolStripMenuItem1_Click);
            // 
            // 编辑ToolStripMenuItem1
            // 
            this.编辑ToolStripMenuItem1.Name = "编辑ToolStripMenuItem1";
            this.编辑ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.编辑ToolStripMenuItem1.Text = "编辑";
            this.编辑ToolStripMenuItem1.Click += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem1
            // 
            this.保存ToolStripMenuItem1.Name = "保存ToolStripMenuItem1";
            this.保存ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.保存ToolStripMenuItem1.Text = "保存";
            this.保存ToolStripMenuItem1.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // cmstbLog
            // 
            this.cmstbLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清除日志信息ToolStripMenuItem});
            this.cmstbLog.Name = "cmstbLog";
            this.cmstbLog.Size = new System.Drawing.Size(149, 26);
            // 
            // 清除日志信息ToolStripMenuItem
            // 
            this.清除日志信息ToolStripMenuItem.Name = "清除日志信息ToolStripMenuItem";
            this.清除日志信息ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清除日志信息ToolStripMenuItem.Text = "清除日志信息";
            this.清除日志信息ToolStripMenuItem.Click += new System.EventHandler(this.清除日志信息ToolStripMenuItem_Click);
            // 
            // lbHeadcount
            // 
            this.lbHeadcount.AutoSize = true;
            this.lbHeadcount.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbHeadcount.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHeadcount.Location = new System.Drawing.Point(1005, 6);
            this.lbHeadcount.Name = "lbHeadcount";
            this.lbHeadcount.Size = new System.Drawing.Size(89, 20);
            this.lbHeadcount.TabIndex = 3;
            this.lbHeadcount.Text = "总人数：";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(27, 46);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(29, 12);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "姓名";
            // 
            // lbSex
            // 
            this.lbSex.AutoSize = true;
            this.lbSex.Location = new System.Drawing.Point(27, 76);
            this.lbSex.Name = "lbSex";
            this.lbSex.Size = new System.Drawing.Size(29, 12);
            this.lbSex.TabIndex = 1;
            this.lbSex.Text = "性别";
            // 
            // lbBirthday
            // 
            this.lbBirthday.AutoSize = true;
            this.lbBirthday.Location = new System.Drawing.Point(214, 164);
            this.lbBirthday.Name = "lbBirthday";
            this.lbBirthday.Size = new System.Drawing.Size(53, 12);
            this.lbBirthday.TabIndex = 2;
            this.lbBirthday.Text = "出生日期";
            // 
            // lbStudentID
            // 
            this.lbStudentID.AutoSize = true;
            this.lbStudentID.Location = new System.Drawing.Point(27, 105);
            this.lbStudentID.Name = "lbStudentID";
            this.lbStudentID.Size = new System.Drawing.Size(29, 12);
            this.lbStudentID.TabIndex = 3;
            this.lbStudentID.Text = "学号";
            // 
            // lbQQ
            // 
            this.lbQQ.AutoSize = true;
            this.lbQQ.Location = new System.Drawing.Point(226, 74);
            this.lbQQ.Name = "lbQQ";
            this.lbQQ.Size = new System.Drawing.Size(41, 12);
            this.lbQQ.TabIndex = 4;
            this.lbQQ.Text = "QQ号码";
            // 
            // lbTel
            // 
            this.lbTel.AutoSize = true;
            this.lbTel.Location = new System.Drawing.Point(214, 134);
            this.lbTel.Name = "lbTel";
            this.lbTel.Size = new System.Drawing.Size(53, 12);
            this.lbTel.TabIndex = 5;
            this.lbTel.Text = "电话号码";
            // 
            // lbAuthority
            // 
            this.lbAuthority.AutoSize = true;
            this.lbAuthority.Location = new System.Drawing.Point(27, 194);
            this.lbAuthority.Name = "lbAuthority";
            this.lbAuthority.Size = new System.Drawing.Size(29, 12);
            this.lbAuthority.TabIndex = 8;
            this.lbAuthority.Text = "权限";
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.SystemColors.Window;
            this.tbName.Location = new System.Drawing.Point(62, 42);
            this.tbName.MaxLength = 4;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(132, 21);
            this.tbName.TabIndex = 1;
            this.tbName.Text = "张进科";
            this.tbName.TextChanged += new System.EventHandler(this.TextChangedChinese);
            // 
            // lbDomitory
            // 
            this.lbDomitory.AutoSize = true;
            this.lbDomitory.Location = new System.Drawing.Point(27, 135);
            this.lbDomitory.Name = "lbDomitory";
            this.lbDomitory.Size = new System.Drawing.Size(29, 12);
            this.lbDomitory.TabIndex = 13;
            this.lbDomitory.Text = "寝室";
            // 
            // tbStudentID
            // 
            this.tbStudentID.BackColor = System.Drawing.SystemColors.Window;
            this.tbStudentID.Location = new System.Drawing.Point(62, 101);
            this.tbStudentID.MaxLength = 11;
            this.tbStudentID.Name = "tbStudentID";
            this.tbStudentID.Size = new System.Drawing.Size(132, 21);
            this.tbStudentID.TabIndex = 3;
            this.tbStudentID.Text = "11307030328";
            this.tbStudentID.TextChanged += new System.EventHandler(this.TextChangedNum);
            // 
            // tbDomitory
            // 
            this.tbDomitory.BackColor = System.Drawing.SystemColors.Window;
            this.tbDomitory.Location = new System.Drawing.Point(132, 160);
            this.tbDomitory.MaxLength = 3;
            this.tbDomitory.Name = "tbDomitory";
            this.tbDomitory.Size = new System.Drawing.Size(62, 21);
            this.tbDomitory.TabIndex = 6;
            this.tbDomitory.Text = "414";
            this.tbDomitory.TextChanged += new System.EventHandler(this.TextChangedNum);
            // 
            // tbTel
            // 
            this.tbTel.BackColor = System.Drawing.SystemColors.Window;
            this.tbTel.Location = new System.Drawing.Point(273, 130);
            this.tbTel.MaxLength = 11;
            this.tbTel.Name = "tbTel";
            this.tbTel.Size = new System.Drawing.Size(132, 21);
            this.tbTel.TabIndex = 10;
            this.tbTel.Text = "15825941073";
            this.tbTel.TextChanged += new System.EventHandler(this.TextChangedNum);
            // 
            // tbQQ
            // 
            this.tbQQ.BackColor = System.Drawing.SystemColors.Window;
            this.tbQQ.Location = new System.Drawing.Point(273, 70);
            this.tbQQ.MaxLength = 15;
            this.tbQQ.Name = "tbQQ";
            this.tbQQ.Size = new System.Drawing.Size(132, 21);
            this.tbQQ.TabIndex = 9;
            this.tbQQ.Text = "799548861";
            this.tbQQ.TextChanged += new System.EventHandler(this.TextChangedNum);
            // 
            // gbPhoto
            // 
            this.gbPhoto.Controls.Add(this.btnOpen);
            this.gbPhoto.Controls.Add(this.pbPortrait);
            this.gbPhoto.Location = new System.Drawing.Point(433, 12);
            this.gbPhoto.Name = "gbPhoto";
            this.gbPhoto.Size = new System.Drawing.Size(157, 224);
            this.gbPhoto.TabIndex = 22;
            this.gbPhoto.TabStop = false;
            this.gbPhoto.Text = "相片";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(19, 186);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "选择相片";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // pbPortrait
            // 
            this.pbPortrait.BackColor = System.Drawing.SystemColors.Window;
            this.pbPortrait.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPortrait.Image = ((System.Drawing.Image)(resources.GetObject("pbPortrait.Image")));
            this.pbPortrait.Location = new System.Drawing.Point(19, 20);
            this.pbPortrait.Name = "pbPortrait";
            this.pbPortrait.Size = new System.Drawing.Size(120, 160);
            this.pbPortrait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPortrait.TabIndex = 21;
            this.pbPortrait.TabStop = false;
            // 
            // gbIDandEigen
            // 
            this.gbIDandEigen.Controls.Add(this.tbcEigen);
            this.gbIDandEigen.Controls.Add(this.lbID);
            this.gbIDandEigen.Controls.Add(this.lbEigen);
            this.gbIDandEigen.Controls.Add(this.tbRFID);
            this.gbIDandEigen.Location = new System.Drawing.Point(596, 12);
            this.gbIDandEigen.Name = "gbIDandEigen";
            this.gbIDandEigen.Size = new System.Drawing.Size(200, 224);
            this.gbIDandEigen.TabIndex = 23;
            this.gbIDandEigen.TabStop = false;
            this.gbIDandEigen.Text = "指纹或卡登记";
            // 
            // tbcEigen
            // 
            this.tbcEigen.Controls.Add(this.tbpEigen1);
            this.tbcEigen.Controls.Add(this.tbpEigen2);
            this.tbcEigen.Controls.Add(this.tbpEigen3);
            this.tbcEigen.Controls.Add(this.tbpEigen4);
            this.tbcEigen.Controls.Add(this.tbpEigen5);
            this.tbcEigen.ItemSize = new System.Drawing.Size(25, 15);
            this.tbcEigen.Location = new System.Drawing.Point(52, 45);
            this.tbcEigen.Name = "tbcEigen";
            this.tbcEigen.SelectedIndex = 0;
            this.tbcEigen.Size = new System.Drawing.Size(129, 167);
            this.tbcEigen.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbcEigen.TabIndex = 2;
            // 
            // tbpEigen1
            // 
            this.tbpEigen1.Controls.Add(this.tbEigen1);
            this.tbpEigen1.Location = new System.Drawing.Point(4, 19);
            this.tbpEigen1.Name = "tbpEigen1";
            this.tbpEigen1.Size = new System.Drawing.Size(121, 144);
            this.tbpEigen1.TabIndex = 0;
            this.tbpEigen1.Text = "1";
            this.tbpEigen1.UseVisualStyleBackColor = true;
            // 
            // tbEigen1
            // 
            this.tbEigen1.BackColor = System.Drawing.SystemColors.Window;
            this.tbEigen1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEigen1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEigen1.Location = new System.Drawing.Point(0, 0);
            this.tbEigen1.MaxLength = 578;
            this.tbEigen1.Multiline = true;
            this.tbEigen1.Name = "tbEigen1";
            this.tbEigen1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEigen1.Size = new System.Drawing.Size(121, 144);
            this.tbEigen1.TabIndex = 14;
            this.tbEigen1.Text = resources.GetString("tbEigen1.Text");
            this.tbEigen1.TextChanged += new System.EventHandler(this.TextChangedHex);
            // 
            // tbpEigen2
            // 
            this.tbpEigen2.Controls.Add(this.tbEigen2);
            this.tbpEigen2.Location = new System.Drawing.Point(4, 19);
            this.tbpEigen2.Margin = new System.Windows.Forms.Padding(0);
            this.tbpEigen2.Name = "tbpEigen2";
            this.tbpEigen2.Size = new System.Drawing.Size(121, 144);
            this.tbpEigen2.TabIndex = 1;
            this.tbpEigen2.Text = "2";
            this.tbpEigen2.UseVisualStyleBackColor = true;
            // 
            // tbEigen2
            // 
            this.tbEigen2.BackColor = System.Drawing.SystemColors.Window;
            this.tbEigen2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEigen2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEigen2.Location = new System.Drawing.Point(0, 0);
            this.tbEigen2.MaxLength = 578;
            this.tbEigen2.Multiline = true;
            this.tbEigen2.Name = "tbEigen2";
            this.tbEigen2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEigen2.Size = new System.Drawing.Size(121, 144);
            this.tbEigen2.TabIndex = 15;
            // 
            // tbpEigen3
            // 
            this.tbpEigen3.Controls.Add(this.tbEigen3);
            this.tbpEigen3.Location = new System.Drawing.Point(4, 19);
            this.tbpEigen3.Name = "tbpEigen3";
            this.tbpEigen3.Size = new System.Drawing.Size(121, 144);
            this.tbpEigen3.TabIndex = 2;
            this.tbpEigen3.Text = "3";
            this.tbpEigen3.UseVisualStyleBackColor = true;
            // 
            // tbEigen3
            // 
            this.tbEigen3.BackColor = System.Drawing.SystemColors.Window;
            this.tbEigen3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEigen3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEigen3.Location = new System.Drawing.Point(0, 0);
            this.tbEigen3.MaxLength = 578;
            this.tbEigen3.Multiline = true;
            this.tbEigen3.Name = "tbEigen3";
            this.tbEigen3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEigen3.Size = new System.Drawing.Size(121, 144);
            this.tbEigen3.TabIndex = 16;
            // 
            // tbpEigen4
            // 
            this.tbpEigen4.Controls.Add(this.tbEigen4);
            this.tbpEigen4.Location = new System.Drawing.Point(4, 19);
            this.tbpEigen4.Name = "tbpEigen4";
            this.tbpEigen4.Size = new System.Drawing.Size(121, 144);
            this.tbpEigen4.TabIndex = 3;
            this.tbpEigen4.Text = "4";
            this.tbpEigen4.UseVisualStyleBackColor = true;
            // 
            // tbEigen4
            // 
            this.tbEigen4.BackColor = System.Drawing.SystemColors.Window;
            this.tbEigen4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEigen4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEigen4.Location = new System.Drawing.Point(0, 0);
            this.tbEigen4.MaxLength = 578;
            this.tbEigen4.Multiline = true;
            this.tbEigen4.Name = "tbEigen4";
            this.tbEigen4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEigen4.Size = new System.Drawing.Size(121, 144);
            this.tbEigen4.TabIndex = 17;
            // 
            // tbpEigen5
            // 
            this.tbpEigen5.Controls.Add(this.tbEigen5);
            this.tbpEigen5.Location = new System.Drawing.Point(4, 19);
            this.tbpEigen5.Name = "tbpEigen5";
            this.tbpEigen5.Size = new System.Drawing.Size(121, 144);
            this.tbpEigen5.TabIndex = 4;
            this.tbpEigen5.Text = "5";
            this.tbpEigen5.UseVisualStyleBackColor = true;
            // 
            // tbEigen5
            // 
            this.tbEigen5.BackColor = System.Drawing.SystemColors.Window;
            this.tbEigen5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbEigen5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEigen5.Location = new System.Drawing.Point(0, 0);
            this.tbEigen5.MaxLength = 578;
            this.tbEigen5.Multiline = true;
            this.tbEigen5.Name = "tbEigen5";
            this.tbEigen5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbEigen5.Size = new System.Drawing.Size(121, 144);
            this.tbEigen5.TabIndex = 18;
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(17, 20);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(29, 12);
            this.lbID.TabIndex = 6;
            this.lbID.Text = "卡号";
            // 
            // lbEigen
            // 
            this.lbEigen.Location = new System.Drawing.Point(29, 45);
            this.lbEigen.Name = "lbEigen";
            this.lbEigen.Size = new System.Drawing.Size(17, 113);
            this.lbEigen.TabIndex = 7;
            this.lbEigen.Text = "指纹特征值";
            // 
            // tbRFID
            // 
            this.tbRFID.BackColor = System.Drawing.SystemColors.Window;
            this.tbRFID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbRFID.Location = new System.Drawing.Point(52, 16);
            this.tbRFID.MaxLength = 8;
            this.tbRFID.Name = "tbRFID";
            this.tbRFID.Size = new System.Drawing.Size(129, 21);
            this.tbRFID.TabIndex = 13;
            this.tbRFID.Text = "C6FBD4DD";
            this.tbRFID.TextChanged += new System.EventHandler(this.TextChangedHex);
            // 
            // cbSex
            // 
            this.cbSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSex.FormattingEnabled = true;
            this.cbSex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.cbSex.Location = new System.Drawing.Point(62, 72);
            this.cbSex.Name = "cbSex";
            this.cbSex.Size = new System.Drawing.Size(132, 20);
            this.cbSex.TabIndex = 2;
            // 
            // dtpBirthday
            // 
            this.dtpBirthday.CustomFormat = "yyyy年MM月dd日";
            this.dtpBirthday.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBirthday.Location = new System.Drawing.Point(273, 160);
            this.dtpBirthday.Name = "dtpBirthday";
            this.dtpBirthday.Size = new System.Drawing.Size(132, 21);
            this.dtpBirthday.TabIndex = 11;
            this.dtpBirthday.Value = new System.DateTime(1995, 2, 23, 0, 0, 0, 0);
            // 
            // lbUID
            // 
            this.lbUID.AutoSize = true;
            this.lbUID.Location = new System.Drawing.Point(15, 16);
            this.lbUID.Name = "lbUID";
            this.lbUID.Size = new System.Drawing.Size(41, 12);
            this.lbUID.TabIndex = 26;
            this.lbUID.Text = "用户号";
            // 
            // tbUID
            // 
            this.tbUID.BackColor = System.Drawing.SystemColors.Window;
            this.tbUID.Location = new System.Drawing.Point(62, 12);
            this.tbUID.MaxLength = 4;
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(132, 21);
            this.tbUID.TabIndex = 0;
            this.tbUID.Text = "1";
            this.tbUID.TextChanged += new System.EventHandler(this.TextChangedNum);
            // 
            // cbBuilding
            // 
            this.cbBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuilding.FormattingEnabled = true;
            this.cbBuilding.Location = new System.Drawing.Point(62, 131);
            this.cbBuilding.Name = "cbBuilding";
            this.cbBuilding.Size = new System.Drawing.Size(132, 20);
            this.cbBuilding.TabIndex = 4;
            this.cbBuilding.TextChanged += new System.EventHandler(this.cbBuilding_TextChanged);
            // 
            // cbFloor
            // 
            this.cbFloor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFloor.FormattingEnabled = true;
            this.cbFloor.Location = new System.Drawing.Point(62, 161);
            this.cbFloor.MaxLength = 3;
            this.cbFloor.Name = "cbFloor";
            this.cbFloor.Size = new System.Drawing.Size(41, 20);
            this.cbFloor.TabIndex = 5;
            // 
            // gbLog
            // 
            this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLog.Controls.Add(this.btnDownloadAll);
            this.gbLog.Controls.Add(this.rtbLog);
            this.gbLog.Location = new System.Drawing.Point(802, 12);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(325, 189);
            this.gbLog.TabIndex = 31;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "日志信息";
            // 
            // btnDownloadAll
            // 
            this.btnDownloadAll.Location = new System.Drawing.Point(221, 160);
            this.btnDownloadAll.Name = "btnDownloadAll";
            this.btnDownloadAll.Size = new System.Drawing.Size(98, 23);
            this.btnDownloadAll.TabIndex = 34;
            this.btnDownloadAll.TabStop = false;
            this.btnDownloadAll.Text = "下传人员数据库";
            this.btnDownloadAll.UseVisualStyleBackColor = true;
            this.btnDownloadAll.Click += new System.EventHandler(this.btnDownloadAll_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.Info;
            this.rtbLog.ContextMenuStrip = this.cmstbLog;
            this.rtbLog.Location = new System.Drawing.Point(6, 20);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(313, 163);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.TabStop = false;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(1023, 207);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(104, 31);
            this.btnDownload.TabIndex = 22;
            this.btnDownload.TabStop = false;
            this.btnDownload.Text = "同步人员信息";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // cbDeviceList
            // 
            this.cbDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeviceList.FormattingEnabled = true;
            this.cbDeviceList.Location = new System.Drawing.Point(859, 213);
            this.cbDeviceList.Name = "cbDeviceList";
            this.cbDeviceList.Size = new System.Drawing.Size(158, 20);
            this.cbDeviceList.TabIndex = 1;
            // 
            // lbSelectDevice
            // 
            this.lbSelectDevice.AutoSize = true;
            this.lbSelectDevice.Location = new System.Drawing.Point(800, 217);
            this.lbSelectDevice.Name = "lbSelectDevice";
            this.lbSelectDevice.Size = new System.Drawing.Size(53, 12);
            this.lbSelectDevice.TabIndex = 32;
            this.lbSelectDevice.Text = "选择设备";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvPerson);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbAuthority);
            this.splitContainer1.Panel2.Controls.Add(this.cbActiveState);
            this.splitContainer1.Panel2.Controls.Add(this.dtpLimitTime);
            this.splitContainer1.Panel2.Controls.Add(this.lbLimitTime);
            this.splitContainer1.Panel2.Controls.Add(this.cbIsLimitTime);
            this.splitContainer1.Panel2.Controls.Add(this.tbWeiXin);
            this.splitContainer1.Panel2.Controls.Add(this.lbWeiXin);
            this.splitContainer1.Panel2.Controls.Add(this.lbMajor);
            this.splitContainer1.Panel2.Controls.Add(this.cbMajor);
            this.splitContainer1.Panel2.Controls.Add(this.cbInstitute);
            this.splitContainer1.Panel2.Controls.Add(this.lbInstitute);
            this.splitContainer1.Panel2.Controls.Add(this.gbIDandEigen);
            this.splitContainer1.Panel2.Controls.Add(this.lbSelectDevice);
            this.splitContainer1.Panel2.Controls.Add(this.cbDeviceList);
            this.splitContainer1.Panel2.Controls.Add(this.btnDownload);
            this.splitContainer1.Panel2.Controls.Add(this.cbFloor);
            this.splitContainer1.Panel2.Controls.Add(this.cbBuilding);
            this.splitContainer1.Panel2.Controls.Add(this.tbUID);
            this.splitContainer1.Panel2.Controls.Add(this.lbUID);
            this.splitContainer1.Panel2.Controls.Add(this.dtpBirthday);
            this.splitContainer1.Panel2.Controls.Add(this.cbSex);
            this.splitContainer1.Panel2.Controls.Add(this.gbPhoto);
            this.splitContainer1.Panel2.Controls.Add(this.tbQQ);
            this.splitContainer1.Panel2.Controls.Add(this.tbTel);
            this.splitContainer1.Panel2.Controls.Add(this.tbDomitory);
            this.splitContainer1.Panel2.Controls.Add(this.tbStudentID);
            this.splitContainer1.Panel2.Controls.Add(this.lbDomitory);
            this.splitContainer1.Panel2.Controls.Add(this.tbName);
            this.splitContainer1.Panel2.Controls.Add(this.lbAuthority);
            this.splitContainer1.Panel2.Controls.Add(this.lbTel);
            this.splitContainer1.Panel2.Controls.Add(this.lbQQ);
            this.splitContainer1.Panel2.Controls.Add(this.lbStudentID);
            this.splitContainer1.Panel2.Controls.Add(this.lbBirthday);
            this.splitContainer1.Panel2.Controls.Add(this.lbSex);
            this.splitContainer1.Panel2.Controls.Add(this.lbName);
            this.splitContainer1.Panel2.Controls.Add(this.gbLog);
            this.splitContainer1.Size = new System.Drawing.Size(1141, 523);
            this.splitContainer1.SplitterDistance = 268;
            this.splitContainer1.TabIndex = 2;
            // 
            // dgvPerson
            // 
            this.dgvPerson.AllowUserToAddRows = false;
            this.dgvPerson.AllowUserToDeleteRows = false;
            this.dgvPerson.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPerson.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPerson.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPerson.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UID,
            this.activeState,
            this.name,
            this.sex,
            this.authority,
            this.isLimitTime,
            this.LimitTime,
            this.StudentID,
            this.institute,
            this.major,
            this.age,
            this.birthday,
            this.recodeData,
            this.QQ,
            this.weiXin,
            this.tel,
            this.dormitory,
            this.ID,
            this.eigen1,
            this.eigen2,
            this.eigen3,
            this.eigen4,
            this.eigen5,
            this.eigenNum});
            this.dgvPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPerson.Location = new System.Drawing.Point(0, 0);
            this.dgvPerson.Name = "dgvPerson";
            this.dgvPerson.ReadOnly = true;
            this.dgvPerson.RowTemplate.Height = 23;
            this.dgvPerson.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPerson.Size = new System.Drawing.Size(1137, 264);
            this.dgvPerson.TabIndex = 1;
            this.dgvPerson.TabStop = false;
            this.dgvPerson.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPerson_CellMouseDown);
            this.dgvPerson.DoubleClick += new System.EventHandler(this.编辑ToolStripMenuItem_Click);
            // 
            // cbAuthority
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbAuthority.CheckBoxProperties = checkBoxProperties1;
            this.cbAuthority.DisplayMemberSingleItem = "";
            this.cbAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthority.FormattingEnabled = true;
            this.cbAuthority.Items.AddRange(new object[] {
            "超级管理员",
            "管理员",
            "407",
            "409",
            "410",
            "411",
            "412"});
            this.cbAuthority.Location = new System.Drawing.Point(62, 190);
            this.cbAuthority.Name = "cbAuthority";
            this.cbAuthority.Size = new System.Drawing.Size(132, 20);
            this.cbAuthority.TabIndex = 43;
            // 
            // cbActiveState
            // 
            this.cbActiveState.AutoSize = true;
            this.cbActiveState.Checked = true;
            this.cbActiveState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveState.Location = new System.Drawing.Point(29, 223);
            this.cbActiveState.Name = "cbActiveState";
            this.cbActiveState.Size = new System.Drawing.Size(48, 16);
            this.cbActiveState.TabIndex = 42;
            this.cbActiveState.Text = "激活";
            this.cbActiveState.UseVisualStyleBackColor = true;
            this.cbActiveState.CheckedChanged += new System.EventHandler(this.cbActiveState_CheckedChanged);
            // 
            // dtpLimitTime
            // 
            this.dtpLimitTime.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtpLimitTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLimitTime.Location = new System.Drawing.Point(252, 220);
            this.dtpLimitTime.Name = "dtpLimitTime";
            this.dtpLimitTime.Size = new System.Drawing.Size(177, 21);
            this.dtpLimitTime.TabIndex = 41;
            // 
            // lbLimitTime
            // 
            this.lbLimitTime.AutoSize = true;
            this.lbLimitTime.Location = new System.Drawing.Point(209, 224);
            this.lbLimitTime.Name = "lbLimitTime";
            this.lbLimitTime.Size = new System.Drawing.Size(41, 12);
            this.lbLimitTime.TabIndex = 40;
            this.lbLimitTime.Text = "有效期";
            // 
            // cbIsLimitTime
            // 
            this.cbIsLimitTime.AutoSize = true;
            this.cbIsLimitTime.Location = new System.Drawing.Point(214, 194);
            this.cbIsLimitTime.Name = "cbIsLimitTime";
            this.cbIsLimitTime.Size = new System.Drawing.Size(72, 16);
            this.cbIsLimitTime.TabIndex = 39;
            this.cbIsLimitTime.Text = "时间限制";
            this.cbIsLimitTime.UseVisualStyleBackColor = true;
            this.cbIsLimitTime.CheckedChanged += new System.EventHandler(this.cbIsLimitTime_CheckedChanged);
            // 
            // tbWeiXin
            // 
            this.tbWeiXin.BackColor = System.Drawing.SystemColors.Window;
            this.tbWeiXin.Location = new System.Drawing.Point(273, 100);
            this.tbWeiXin.MaxLength = 15;
            this.tbWeiXin.Name = "tbWeiXin";
            this.tbWeiXin.Size = new System.Drawing.Size(132, 21);
            this.tbWeiXin.TabIndex = 38;
            this.tbWeiXin.Text = "zhangjinke0220";
            // 
            // lbWeiXin
            // 
            this.lbWeiXin.AutoSize = true;
            this.lbWeiXin.Location = new System.Drawing.Point(238, 104);
            this.lbWeiXin.Name = "lbWeiXin";
            this.lbWeiXin.Size = new System.Drawing.Size(29, 12);
            this.lbWeiXin.TabIndex = 37;
            this.lbWeiXin.Text = "微信";
            // 
            // lbMajor
            // 
            this.lbMajor.AutoSize = true;
            this.lbMajor.Location = new System.Drawing.Point(238, 45);
            this.lbMajor.Name = "lbMajor";
            this.lbMajor.Size = new System.Drawing.Size(29, 12);
            this.lbMajor.TabIndex = 36;
            this.lbMajor.Text = "专业";
            // 
            // cbMajor
            // 
            this.cbMajor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMajor.FormattingEnabled = true;
            this.cbMajor.Location = new System.Drawing.Point(273, 41);
            this.cbMajor.MaxLength = 3;
            this.cbMajor.Name = "cbMajor";
            this.cbMajor.Size = new System.Drawing.Size(132, 20);
            this.cbMajor.TabIndex = 8;
            // 
            // cbInstitute
            // 
            this.cbInstitute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstitute.FormattingEnabled = true;
            this.cbInstitute.Location = new System.Drawing.Point(273, 12);
            this.cbInstitute.Name = "cbInstitute";
            this.cbInstitute.Size = new System.Drawing.Size(132, 20);
            this.cbInstitute.TabIndex = 7;
            this.cbInstitute.TextChanged += new System.EventHandler(this.cbInstitute_TextChanged);
            // 
            // lbInstitute
            // 
            this.lbInstitute.AutoSize = true;
            this.lbInstitute.Location = new System.Drawing.Point(238, 16);
            this.lbInstitute.Name = "lbInstitute";
            this.lbInstitute.Size = new System.Drawing.Size(29, 12);
            this.lbInstitute.TabIndex = 33;
            this.lbInstitute.Text = "学院";
            // 
            // UID
            // 
            this.UID.HeaderText = "用户号";
            this.UID.Name = "UID";
            this.UID.ReadOnly = true;
            this.UID.Width = 64;
            // 
            // activeState
            // 
            this.activeState.HeaderText = "激活状态";
            this.activeState.Name = "activeState";
            this.activeState.ReadOnly = true;
            this.activeState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activeState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // name
            // 
            this.name.HeaderText = "姓名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 69;
            // 
            // sex
            // 
            this.sex.HeaderText = "性别";
            this.sex.Name = "sex";
            this.sex.ReadOnly = true;
            this.sex.Width = 61;
            // 
            // authority
            // 
            this.authority.HeaderText = "权限";
            this.authority.Name = "authority";
            this.authority.ReadOnly = true;
            this.authority.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.authority.Width = 53;
            // 
            // isLimitTime
            // 
            this.isLimitTime.HeaderText = "时间限制";
            this.isLimitTime.Name = "isLimitTime";
            this.isLimitTime.ReadOnly = true;
            this.isLimitTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isLimitTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LimitTime
            // 
            this.LimitTime.HeaderText = "有效期";
            this.LimitTime.Name = "LimitTime";
            this.LimitTime.ReadOnly = true;
            // 
            // StudentID
            // 
            this.StudentID.HeaderText = "学号";
            this.StudentID.Name = "StudentID";
            this.StudentID.ReadOnly = true;
            this.StudentID.Width = 89;
            // 
            // institute
            // 
            this.institute.HeaderText = "学院";
            this.institute.Name = "institute";
            this.institute.ReadOnly = true;
            // 
            // major
            // 
            this.major.HeaderText = "专业";
            this.major.Name = "major";
            this.major.ReadOnly = true;
            // 
            // age
            // 
            this.age.HeaderText = "年龄";
            this.age.Name = "age";
            this.age.ReadOnly = true;
            this.age.Width = 54;
            // 
            // birthday
            // 
            this.birthday.HeaderText = "出生日期";
            this.birthday.Name = "birthday";
            this.birthday.ReadOnly = true;
            // 
            // recodeData
            // 
            this.recodeData.HeaderText = "录入时间";
            this.recodeData.Name = "recodeData";
            this.recodeData.ReadOnly = true;
            this.recodeData.Width = 109;
            // 
            // QQ
            // 
            this.QQ.HeaderText = "QQ";
            this.QQ.Name = "QQ";
            this.QQ.ReadOnly = true;
            this.QQ.Width = 77;
            // 
            // weiXin
            // 
            this.weiXin.HeaderText = "微信";
            this.weiXin.Name = "weiXin";
            this.weiXin.ReadOnly = true;
            // 
            // tel
            // 
            this.tel.HeaderText = "电话号码";
            this.tel.Name = "tel";
            this.tel.ReadOnly = true;
            this.tel.Width = 93;
            // 
            // dormitory
            // 
            this.dormitory.HeaderText = "寝室";
            this.dormitory.Name = "dormitory";
            this.dormitory.ReadOnly = true;
            this.dormitory.Width = 133;
            // 
            // ID
            // 
            this.ID.HeaderText = "卡号";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 77;
            // 
            // eigen1
            // 
            this.eigen1.HeaderText = "指纹1";
            this.eigen1.Name = "eigen1";
            this.eigen1.ReadOnly = true;
            this.eigen1.Width = 115;
            // 
            // eigen2
            // 
            this.eigen2.HeaderText = "指纹2";
            this.eigen2.Name = "eigen2";
            this.eigen2.ReadOnly = true;
            // 
            // eigen3
            // 
            this.eigen3.HeaderText = "指纹3";
            this.eigen3.Name = "eigen3";
            this.eigen3.ReadOnly = true;
            // 
            // eigen4
            // 
            this.eigen4.HeaderText = "指纹4";
            this.eigen4.Name = "eigen4";
            this.eigen4.ReadOnly = true;
            // 
            // eigen5
            // 
            this.eigen5.HeaderText = "指纹5";
            this.eigen5.Name = "eigen5";
            this.eigen5.ReadOnly = true;
            // 
            // eigenNum
            // 
            this.eigenNum.HeaderText = "指纹号";
            this.eigenNum.Name = "eigenNum";
            this.eigenNum.ReadOnly = true;
            this.eigenNum.Width = 150;
            // 
            // FormPersonnelManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 550);
            this.Controls.Add(this.lbHeadcount);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPersonnelManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "人员维护";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPersonnelManagement_FormClosing);
            this.Load += new System.EventHandler(this.FormPersonnelManagement_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.cmsdgvPerson.ResumeLayout(false);
            this.cmstbLog.ResumeLayout(false);
            this.gbPhoto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPortrait)).EndInit();
            this.gbIDandEigen.ResumeLayout(false);
            this.gbIDandEigen.PerformLayout();
            this.tbcEigen.ResumeLayout(false);
            this.tbpEigen1.ResumeLayout(false);
            this.tbpEigen1.PerformLayout();
            this.tbpEigen2.ResumeLayout(false);
            this.tbpEigen2.PerformLayout();
            this.tbpEigen3.ResumeLayout(false);
            this.tbpEigen3.PerformLayout();
            this.tbpEigen4.ResumeLayout(false);
            this.tbpEigen4.PerformLayout();
            this.tbpEigen5.ResumeLayout(false);
            this.tbpEigen5.PerformLayout();
            this.gbLog.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerson)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox tbSearch;
        private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入ToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增toolStripMenuItem1;
        private System.Windows.Forms.Label lbHeadcount;
        private System.Windows.Forms.ContextMenuStrip cmsdgvPerson;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip cmstbLog;
        private System.Windows.Forms.ToolStripMenuItem 清除日志信息ToolStripMenuItem;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbSex;
        private System.Windows.Forms.Label lbBirthday;
        private System.Windows.Forms.Label lbStudentID;
        private System.Windows.Forms.Label lbQQ;
        private System.Windows.Forms.Label lbTel;
        private System.Windows.Forms.Label lbAuthority;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lbDomitory;
        private System.Windows.Forms.TextBox tbStudentID;
        private System.Windows.Forms.TextBox tbDomitory;
        private System.Windows.Forms.TextBox tbTel;
        private System.Windows.Forms.TextBox tbQQ;
        private System.Windows.Forms.GroupBox gbPhoto;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PictureBox pbPortrait;
        private System.Windows.Forms.GroupBox gbIDandEigen;
        private System.Windows.Forms.Label lbID;
        public System.Windows.Forms.TextBox tbEigen1;
        private System.Windows.Forms.Label lbEigen;
        private System.Windows.Forms.TextBox tbRFID;
        private System.Windows.Forms.ComboBox cbSex;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
        private System.Windows.Forms.Label lbUID;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.ComboBox cbBuilding;
        private System.Windows.Forms.ComboBox cbFloor;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.Button btnDownload;
        public System.Windows.Forms.ComboBox cbDeviceList;
        private System.Windows.Forms.Label lbSelectDevice;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvPerson;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnDownloadAll;
        private System.Windows.Forms.TabControl tbcEigen;
        private System.Windows.Forms.TabPage tbpEigen1;
        private System.Windows.Forms.TabPage tbpEigen2;
        public System.Windows.Forms.TextBox tbEigen2;
        private System.Windows.Forms.TabPage tbpEigen3;
        public System.Windows.Forms.TextBox tbEigen3;
        private System.Windows.Forms.TabPage tbpEigen4;
        public System.Windows.Forms.TextBox tbEigen4;
        private System.Windows.Forms.TabPage tbpEigen5;
        public System.Windows.Forms.TextBox tbEigen5;
        private System.Windows.Forms.Label lbMajor;
        private System.Windows.Forms.ComboBox cbMajor;
        private System.Windows.Forms.ComboBox cbInstitute;
        private System.Windows.Forms.Label lbInstitute;
        private System.Windows.Forms.TextBox tbWeiXin;
        private System.Windows.Forms.Label lbWeiXin;
        private System.Windows.Forms.DateTimePicker dtpLimitTime;
        private System.Windows.Forms.Label lbLimitTime;
        private System.Windows.Forms.CheckBox cbIsLimitTime;
        private System.Windows.Forms.CheckBox cbActiveState;
        private PresentationControls.CheckBoxComboBox cbAuthority;
        private System.Windows.Forms.DataGridViewTextBoxColumn UID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeState;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sex;
        private System.Windows.Forms.DataGridViewTextBoxColumn authority;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLimitTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LimitTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn institute;
        private System.Windows.Forms.DataGridViewTextBoxColumn major;
        private System.Windows.Forms.DataGridViewTextBoxColumn age;
        private System.Windows.Forms.DataGridViewTextBoxColumn birthday;
        private System.Windows.Forms.DataGridViewTextBoxColumn recodeData;
        private System.Windows.Forms.DataGridViewTextBoxColumn QQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn weiXin;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dormitory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigen1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigen2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigen3;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigen4;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigen5;
        private System.Windows.Forms.DataGridViewTextBoxColumn eigenNum;
    }
}