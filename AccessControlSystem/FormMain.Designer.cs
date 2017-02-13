namespace AccessControlSystem
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lbport = new System.Windows.Forms.Label();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.初始化系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.备份数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.导入考勤数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出考勤数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.维护设置OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.人员维护ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设备管理DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.同步设备时间ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.dgvDeviceInfo = new System.Windows.Forms.DataGridView();
            this.tspLnkTools = new System.Windows.Forms.ToolStrip();
            this.考勤信息统计toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.人员维护toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.设备管理toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.连接设备toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.断开设备toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.同步设备时间toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.获取人员进出记录toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.cmstbLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清除日志信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvAttendanceSheet = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.timerConnectAndGet = new System.Windows.Forms.Timer(this.components);
            this.deviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personnelNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceInfo)).BeginInit();
            this.tspLnkTools.SuspendLayout();
            this.cmstbLog.SuspendLayout();
            this.gbLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbport
            // 
            this.lbport.AutoSize = true;
            this.lbport.Location = new System.Drawing.Point(645, 290);
            this.lbport.Name = "lbport";
            this.lbport.Size = new System.Drawing.Size(0, 12);
            this.lbport.TabIndex = 4;
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.维护设置OToolStripMenuItem,
            this.设备管理DToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(745, 25);
            this.menuMain.TabIndex = 5;
            this.menuMain.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.初始化系统ToolStripMenuItem,
            this.备份数据库ToolStripMenuItem,
            this.toolStripSeparator1,
            this.导入考勤数据ToolStripMenuItem,
            this.导出考勤数据ToolStripMenuItem,
            this.toolStripSeparator2,
            this.退出系统ToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "数据(&F)";
            // 
            // 初始化系统ToolStripMenuItem
            // 
            this.初始化系统ToolStripMenuItem.Name = "初始化系统ToolStripMenuItem";
            this.初始化系统ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.初始化系统ToolStripMenuItem.Text = "初始化系统(&I)";
            // 
            // 备份数据库ToolStripMenuItem
            // 
            this.备份数据库ToolStripMenuItem.Name = "备份数据库ToolStripMenuItem";
            this.备份数据库ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.备份数据库ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.备份数据库ToolStripMenuItem.Text = "备份数据库";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // 导入考勤数据ToolStripMenuItem
            // 
            this.导入考勤数据ToolStripMenuItem.Name = "导入考勤数据ToolStripMenuItem";
            this.导入考勤数据ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.导入考勤数据ToolStripMenuItem.Text = "导入考勤数据";
            // 
            // 导出考勤数据ToolStripMenuItem
            // 
            this.导出考勤数据ToolStripMenuItem.Name = "导出考勤数据ToolStripMenuItem";
            this.导出考勤数据ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.导出考勤数据ToolStripMenuItem.Text = "导出考勤数据";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.退出系统ToolStripMenuItem.Text = "退出系统";
            // 
            // 维护设置OToolStripMenuItem
            // 
            this.维护设置OToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.人员维护ToolStripMenuItem,
            this.toolStripSeparator3,
            this.系统设置ToolStripMenuItem});
            this.维护设置OToolStripMenuItem.Name = "维护设置OToolStripMenuItem";
            this.维护设置OToolStripMenuItem.Size = new System.Drawing.Size(91, 21);
            this.维护设置OToolStripMenuItem.Text = "维护/设置(&O)";
            // 
            // 人员维护ToolStripMenuItem
            // 
            this.人员维护ToolStripMenuItem.Name = "人员维护ToolStripMenuItem";
            this.人员维护ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.人员维护ToolStripMenuItem.Text = "人员维护";
            this.人员维护ToolStripMenuItem.Click += new System.EventHandler(this.人员维护ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(121, 6);
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            // 
            // 设备管理DToolStripMenuItem
            // 
            this.设备管理DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接设备ToolStripMenuItem,
            this.toolStripSeparator4,
            this.同步设备时间ToolStripMenuItem});
            this.设备管理DToolStripMenuItem.Name = "设备管理DToolStripMenuItem";
            this.设备管理DToolStripMenuItem.Size = new System.Drawing.Size(85, 21);
            this.设备管理DToolStripMenuItem.Text = "设备管理(&D)";
            // 
            // 连接设备ToolStripMenuItem
            // 
            this.连接设备ToolStripMenuItem.Name = "连接设备ToolStripMenuItem";
            this.连接设备ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.连接设备ToolStripMenuItem.Text = "设备管理";
            this.连接设备ToolStripMenuItem.Click += new System.EventHandler(this.设备管理ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(145, 6);
            // 
            // 同步设备时间ToolStripMenuItem
            // 
            this.同步设备时间ToolStripMenuItem.Name = "同步设备时间ToolStripMenuItem";
            this.同步设备时间ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.同步设备时间ToolStripMenuItem.Text = "同步设备时间";
            this.同步设备时间ToolStripMenuItem.Click += new System.EventHandler(this.同步设备时间ToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // dgvDeviceInfo
            // 
            this.dgvDeviceInfo.AllowUserToAddRows = false;
            this.dgvDeviceInfo.AllowUserToDeleteRows = false;
            this.dgvDeviceInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDeviceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeviceInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deviceName,
            this.deviceState,
            this.deviceAddress,
            this.personnelNumber});
            this.dgvDeviceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeviceInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvDeviceInfo.Name = "dgvDeviceInfo";
            this.dgvDeviceInfo.ReadOnly = true;
            this.dgvDeviceInfo.RowTemplate.Height = 23;
            this.dgvDeviceInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeviceInfo.Size = new System.Drawing.Size(443, 223);
            this.dgvDeviceInfo.TabIndex = 6;
            // 
            // tspLnkTools
            // 
            this.tspLnkTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.考勤信息统计toolStripButton,
            this.人员维护toolStripButton,
            this.设备管理toolStripButton,
            this.连接设备toolStripButton,
            this.断开设备toolStripButton,
            this.同步设备时间toolStripButton,
            this.获取人员进出记录toolStripButton});
            this.tspLnkTools.Location = new System.Drawing.Point(0, 25);
            this.tspLnkTools.Name = "tspLnkTools";
            this.tspLnkTools.Size = new System.Drawing.Size(745, 25);
            this.tspLnkTools.TabIndex = 7;
            this.tspLnkTools.Text = "toolStrip1";
            // 
            // 考勤信息统计toolStripButton
            // 
            this.考勤信息统计toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("考勤信息统计toolStripButton.Image")));
            this.考勤信息统计toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.考勤信息统计toolStripButton.Name = "考勤信息统计toolStripButton";
            this.考勤信息统计toolStripButton.Size = new System.Drawing.Size(100, 22);
            this.考勤信息统计toolStripButton.Text = "考勤信息统计";
            this.考勤信息统计toolStripButton.Click += new System.EventHandler(this.考勤信息统计toolStripButton_Click);
            // 
            // 人员维护toolStripButton
            // 
            this.人员维护toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("人员维护toolStripButton.Image")));
            this.人员维护toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.人员维护toolStripButton.Name = "人员维护toolStripButton";
            this.人员维护toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.人员维护toolStripButton.Text = "人员维护";
            this.人员维护toolStripButton.Click += new System.EventHandler(this.人员维护ToolStripMenuItem_Click);
            // 
            // 设备管理toolStripButton
            // 
            this.设备管理toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("设备管理toolStripButton.Image")));
            this.设备管理toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.设备管理toolStripButton.Name = "设备管理toolStripButton";
            this.设备管理toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.设备管理toolStripButton.Text = "设备管理";
            this.设备管理toolStripButton.Click += new System.EventHandler(this.设备管理ToolStripMenuItem_Click);
            // 
            // 连接设备toolStripButton
            // 
            this.连接设备toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("连接设备toolStripButton.Image")));
            this.连接设备toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.连接设备toolStripButton.Name = "连接设备toolStripButton";
            this.连接设备toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.连接设备toolStripButton.Text = "连接设备";
            this.连接设备toolStripButton.Click += new System.EventHandler(this.连接设备toolStripButton_Click);
            // 
            // 断开设备toolStripButton
            // 
            this.断开设备toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("断开设备toolStripButton.Image")));
            this.断开设备toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.断开设备toolStripButton.Name = "断开设备toolStripButton";
            this.断开设备toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.断开设备toolStripButton.Text = "断开设备";
            this.断开设备toolStripButton.Click += new System.EventHandler(this.断开设备toolStripButton_Click);
            // 
            // 同步设备时间toolStripButton
            // 
            this.同步设备时间toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("同步设备时间toolStripButton.Image")));
            this.同步设备时间toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.同步设备时间toolStripButton.Name = "同步设备时间toolStripButton";
            this.同步设备时间toolStripButton.Size = new System.Drawing.Size(100, 22);
            this.同步设备时间toolStripButton.Text = "同步设备时间";
            this.同步设备时间toolStripButton.Click += new System.EventHandler(this.同步设备时间ToolStripMenuItem_Click);
            // 
            // 获取人员进出记录toolStripButton
            // 
            this.获取人员进出记录toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("获取人员进出记录toolStripButton.Image")));
            this.获取人员进出记录toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.获取人员进出记录toolStripButton.Name = "获取人员进出记录toolStripButton";
            this.获取人员进出记录toolStripButton.Size = new System.Drawing.Size(124, 22);
            this.获取人员进出记录toolStripButton.Text = "获取人员进出记录";
            this.获取人员进出记录toolStripButton.Click += new System.EventHandler(this.获取人员进出记录toolStripButton_Click);
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.Info;
            this.tbLog.ContextMenuStrip = this.cmstbLog;
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 17);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(284, 203);
            this.tbLog.TabIndex = 32;
            this.tbLog.WordWrap = false;
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
            // gbLog
            // 
            this.gbLog.AutoSize = true;
            this.gbLog.Controls.Add(this.tbLog);
            this.gbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLog.Location = new System.Drawing.Point(0, 0);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(290, 223);
            this.gbLog.TabIndex = 33;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "日志信息";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvAttendanceSheet);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(745, 452);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.TabIndex = 34;
            // 
            // dgvAttendanceSheet
            // 
            this.dgvAttendanceSheet.AllowUserToAddRows = false;
            this.dgvAttendanceSheet.AllowUserToDeleteRows = false;
            this.dgvAttendanceSheet.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvAttendanceSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendanceSheet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.device,
            this.uID,
            this.name,
            this.state});
            this.dgvAttendanceSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttendanceSheet.Location = new System.Drawing.Point(0, 0);
            this.dgvAttendanceSheet.Name = "dgvAttendanceSheet";
            this.dgvAttendanceSheet.ReadOnly = true;
            this.dgvAttendanceSheet.RowTemplate.Height = 23;
            this.dgvAttendanceSheet.Size = new System.Drawing.Size(741, 217);
            this.dgvAttendanceSheet.TabIndex = 0;
            // 
            // time
            // 
            this.time.HeaderText = "时间";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            // 
            // device
            // 
            this.device.HeaderText = "设备名称";
            this.device.Name = "device";
            this.device.ReadOnly = true;
            // 
            // uID
            // 
            this.uID.HeaderText = "用户号";
            this.uID.Name = "uID";
            this.uID.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "姓名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // state
            // 
            this.state.HeaderText = "状态";
            this.state.Name = "state";
            this.state.ReadOnly = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvDeviceInfo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gbLog);
            this.splitContainer2.Size = new System.Drawing.Size(745, 227);
            this.splitContainer2.SplitterDistance = 447;
            this.splitContainer2.TabIndex = 0;
            // 
            // timerConnectAndGet
            // 
            this.timerConnectAndGet.Interval = 30000;
            this.timerConnectAndGet.Tick += new System.EventHandler(this.timerConnectAndGet_Tick);
            // 
            // deviceName
            // 
            this.deviceName.HeaderText = "设备名称";
            this.deviceName.Name = "deviceName";
            this.deviceName.ReadOnly = true;
            // 
            // deviceState
            // 
            this.deviceState.HeaderText = "状态";
            this.deviceState.Name = "deviceState";
            this.deviceState.ReadOnly = true;
            // 
            // deviceAddress
            // 
            this.deviceAddress.HeaderText = "机器号";
            this.deviceAddress.Name = "deviceAddress";
            this.deviceAddress.ReadOnly = true;
            this.deviceAddress.Width = 120;
            // 
            // personnelNumber
            // 
            this.personnelNumber.HeaderText = "人员数";
            this.personnelNumber.Name = "personnelNumber";
            this.personnelNumber.ReadOnly = true;
            this.personnelNumber.Width = 80;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 502);
            this.Controls.Add(this.tspLnkTools);
            this.Controls.Add(this.lbport);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuMain;
            this.MaximumSize = new System.Drawing.Size(761, 1000);
            this.MinimumSize = new System.Drawing.Size(761, 39);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重庆理工大学创新实验室门禁考勤系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceInfo)).EndInit();
            this.tspLnkTools.ResumeLayout(false);
            this.tspLnkTools.PerformLayout();
            this.cmstbLog.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceSheet)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.Label lbport;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始化系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 备份数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 导入考勤数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出考勤数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 维护设置OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 人员维护ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设备管理DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连接设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 同步设备时间ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.DataGridView dgvDeviceInfo;
        private System.Windows.Forms.ToolStrip tspLnkTools;
        private System.Windows.Forms.ToolStripButton 人员维护toolStripButton;
        private System.Windows.Forms.ToolStripButton 设备管理toolStripButton;
        private System.Windows.Forms.ToolStripButton 连接设备toolStripButton;
        private System.Windows.Forms.ToolStripButton 断开设备toolStripButton;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.ContextMenuStrip cmstbLog;
        private System.Windows.Forms.ToolStripMenuItem 清除日志信息ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvAttendanceSheet;
        private System.Windows.Forms.Timer timerConnectAndGet;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn device;
        private System.Windows.Forms.DataGridViewTextBoxColumn uID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.ToolStripButton 同步设备时间toolStripButton;
        private System.Windows.Forms.ToolStripButton 考勤信息统计toolStripButton;
        private System.Windows.Forms.ToolStripButton 获取人员进出记录toolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceState;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn personnelNumber;
    }
}

