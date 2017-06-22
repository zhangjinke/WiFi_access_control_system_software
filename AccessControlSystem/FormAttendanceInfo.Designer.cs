namespace AccessControlSystem
{
    partial class FormAttendanceInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbCondition = new System.Windows.Forms.GroupBox();
            this.tempProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnExport = new System.Windows.Forms.Button();
            this.cbDeviceName = new System.Windows.Forms.ComboBox();
            this.lbDeviceName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbStudentID = new System.Windows.Forms.ComboBox();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.lbStudentID = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.gbTime = new System.Windows.Forms.GroupBox();
            this.cbTime = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lbRange = new System.Windows.Forms.Label();
            this.lbTo = new System.Windows.Forms.Label();
            this.lbFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dgvAttendanceInfo = new System.Windows.Forms.DataGridView();
            this.UID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recodeData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InAndOutTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exceptionRecord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsdgvAttendanceInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看详细记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbCondition.SuspendLayout();
            this.gbTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceInfo)).BeginInit();
            this.cmsdgvAttendanceInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCondition
            // 
            this.gbCondition.Controls.Add(this.tempProgressBar);
            this.gbCondition.Controls.Add(this.btnExport);
            this.gbCondition.Controls.Add(this.cbDeviceName);
            this.gbCondition.Controls.Add(this.lbDeviceName);
            this.gbCondition.Controls.Add(this.btnSearch);
            this.gbCondition.Controls.Add(this.cbStudentID);
            this.gbCondition.Controls.Add(this.cbName);
            this.gbCondition.Controls.Add(this.lbStudentID);
            this.gbCondition.Controls.Add(this.lbName);
            this.gbCondition.Controls.Add(this.gbTime);
            this.gbCondition.Location = new System.Drawing.Point(12, 12);
            this.gbCondition.Name = "gbCondition";
            this.gbCondition.Size = new System.Drawing.Size(801, 162);
            this.gbCondition.TabIndex = 0;
            this.gbCondition.TabStop = false;
            this.gbCondition.Text = "条件";
            // 
            // tempProgressBar
            // 
            this.tempProgressBar.Location = new System.Drawing.Point(599, 120);
            this.tempProgressBar.Name = "tempProgressBar";
            this.tempProgressBar.Size = new System.Drawing.Size(170, 31);
            this.tempProgressBar.TabIndex = 3;
            this.tempProgressBar.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(599, 120);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(73, 30);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "导出Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // cbDeviceName
            // 
            this.cbDeviceName.FormattingEnabled = true;
            this.cbDeviceName.Location = new System.Drawing.Point(81, 117);
            this.cbDeviceName.Name = "cbDeviceName";
            this.cbDeviceName.Size = new System.Drawing.Size(121, 20);
            this.cbDeviceName.TabIndex = 8;
            this.cbDeviceName.Text = "所有";
            this.cbDeviceName.DropDown += new System.EventHandler(this.cbDeviceName_DropDown);
            // 
            // lbDeviceName
            // 
            this.lbDeviceName.AutoSize = true;
            this.lbDeviceName.Location = new System.Drawing.Point(12, 120);
            this.lbDeviceName.Name = "lbDeviceName";
            this.lbDeviceName.Size = new System.Drawing.Size(53, 12);
            this.lbDeviceName.TabIndex = 7;
            this.lbDeviceName.Text = "设备名称";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(599, 32);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(170, 82);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbStudentID
            // 
            this.cbStudentID.FormattingEnabled = true;
            this.cbStudentID.Location = new System.Drawing.Point(81, 73);
            this.cbStudentID.Name = "cbStudentID";
            this.cbStudentID.Size = new System.Drawing.Size(121, 20);
            this.cbStudentID.TabIndex = 5;
            this.cbStudentID.Text = "所有";
            this.cbStudentID.DropDown += new System.EventHandler(this.cbStudentID_DropDown);
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(81, 24);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(121, 20);
            this.cbName.TabIndex = 4;
            this.cbName.Text = "所有";
            this.cbName.DropDown += new System.EventHandler(this.cbName_DropDown);
            // 
            // lbStudentID
            // 
            this.lbStudentID.AutoSize = true;
            this.lbStudentID.Location = new System.Drawing.Point(36, 76);
            this.lbStudentID.Name = "lbStudentID";
            this.lbStudentID.Size = new System.Drawing.Size(29, 12);
            this.lbStudentID.TabIndex = 3;
            this.lbStudentID.Text = "学号";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(36, 27);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(29, 12);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "姓名";
            // 
            // gbTime
            // 
            this.gbTime.Controls.Add(this.cbTime);
            this.gbTime.Controls.Add(this.dtpTo);
            this.gbTime.Controls.Add(this.lbRange);
            this.gbTime.Controls.Add(this.lbTo);
            this.gbTime.Controls.Add(this.lbFrom);
            this.gbTime.Controls.Add(this.dtpFrom);
            this.gbTime.Location = new System.Drawing.Point(246, 20);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(310, 126);
            this.gbTime.TabIndex = 1;
            this.gbTime.TabStop = false;
            this.gbTime.Text = "起止时间";
            // 
            // cbTime
            // 
            this.cbTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTime.FormattingEnabled = true;
            this.cbTime.Items.AddRange(new object[] {
            "最近1天",
            "最近1周",
            "最近1月",
            "最近1年",
            "所有"});
            this.cbTime.Location = new System.Drawing.Point(82, 94);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(121, 20);
            this.cbTime.TabIndex = 10;
            this.cbTime.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(82, 66);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 21);
            this.dtpTo.TabIndex = 7;
            // 
            // lbRange
            // 
            this.lbRange.AutoSize = true;
            this.lbRange.Location = new System.Drawing.Point(24, 97);
            this.lbRange.Name = "lbRange";
            this.lbRange.Size = new System.Drawing.Size(53, 12);
            this.lbRange.TabIndex = 9;
            this.lbRange.Text = "查询范围";
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(24, 66);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(17, 12);
            this.lbTo.TabIndex = 2;
            this.lbTo.Text = "至";
            // 
            // lbFrom
            // 
            this.lbFrom.AutoSize = true;
            this.lbFrom.Location = new System.Drawing.Point(24, 36);
            this.lbFrom.Name = "lbFrom";
            this.lbFrom.Size = new System.Drawing.Size(17, 12);
            this.lbFrom.TabIndex = 3;
            this.lbFrom.Text = "从";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(82, 30);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 21);
            this.dtpFrom.TabIndex = 6;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // dgvAttendanceInfo
            // 
            this.dgvAttendanceInfo.AllowUserToAddRows = false;
            this.dgvAttendanceInfo.AllowUserToDeleteRows = false;
            this.dgvAttendanceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttendanceInfo.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAttendanceInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAttendanceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttendanceInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UID,
            this.name,
            this.sex,
            this.StudentID,
            this.age,
            this.recodeData,
            this.QQ,
            this.tel,
            this.authority,
            this.InAndOutTimes,
            this.time,
            this.exceptionRecord});
            this.dgvAttendanceInfo.Location = new System.Drawing.Point(12, 189);
            this.dgvAttendanceInfo.Name = "dgvAttendanceInfo";
            this.dgvAttendanceInfo.ReadOnly = true;
            this.dgvAttendanceInfo.RowTemplate.Height = 23;
            this.dgvAttendanceInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttendanceInfo.Size = new System.Drawing.Size(945, 244);
            this.dgvAttendanceInfo.TabIndex = 2;
            this.dgvAttendanceInfo.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAttendanceInfo_CellMouseDown);
            // 
            // UID
            // 
            this.UID.HeaderText = "用户号";
            this.UID.Name = "UID";
            this.UID.ReadOnly = true;
            this.UID.Width = 64;
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
            // StudentID
            // 
            this.StudentID.HeaderText = "学号";
            this.StudentID.Name = "StudentID";
            this.StudentID.ReadOnly = true;
            this.StudentID.Width = 89;
            // 
            // age
            // 
            this.age.HeaderText = "年龄";
            this.age.Name = "age";
            this.age.ReadOnly = true;
            this.age.Width = 54;
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
            // tel
            // 
            this.tel.HeaderText = "电话号码";
            this.tel.Name = "tel";
            this.tel.ReadOnly = true;
            this.tel.Width = 93;
            // 
            // authority
            // 
            this.authority.HeaderText = "权限";
            this.authority.Name = "authority";
            this.authority.ReadOnly = true;
            this.authority.Width = 53;
            // 
            // InAndOutTimes
            // 
            this.InAndOutTimes.HeaderText = "进出门次数";
            this.InAndOutTimes.Name = "InAndOutTimes";
            this.InAndOutTimes.ReadOnly = true;
            this.InAndOutTimes.Width = 90;
            // 
            // time
            // 
            this.time.HeaderText = "时间";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 150;
            // 
            // exceptionRecord
            // 
            this.exceptionRecord.HeaderText = "异常记录条数";
            this.exceptionRecord.Name = "exceptionRecord";
            this.exceptionRecord.ReadOnly = true;
            this.exceptionRecord.Width = 110;
            // 
            // cmsdgvAttendanceInfo
            // 
            this.cmsdgvAttendanceInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看详细记录ToolStripMenuItem});
            this.cmsdgvAttendanceInfo.Name = "contextMenuStrip";
            this.cmsdgvAttendanceInfo.Size = new System.Drawing.Size(149, 26);
            // 
            // 查看详细记录ToolStripMenuItem
            // 
            this.查看详细记录ToolStripMenuItem.Name = "查看详细记录ToolStripMenuItem";
            this.查看详细记录ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.查看详细记录ToolStripMenuItem.Text = "查看详细记录";
            this.查看详细记录ToolStripMenuItem.Click += new System.EventHandler(this.查看详细记录ToolStripMenuItem_Click);
            // 
            // FormAttendanceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 445);
            this.Controls.Add(this.dgvAttendanceInfo);
            this.Controls.Add(this.gbCondition);
            this.Name = "FormAttendanceInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "考勤信息统计";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAttendanceInfo_FormClosing);
            this.Load += new System.EventHandler(this.FormAttendanceInfo_Load);
            this.gbCondition.ResumeLayout(false);
            this.gbCondition.PerformLayout();
            this.gbTime.ResumeLayout(false);
            this.gbTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendanceInfo)).EndInit();
            this.cmsdgvAttendanceInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCondition;
        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.Label lbStudentID;
        private System.Windows.Forms.GroupBox gbTime;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.Label lbFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvAttendanceInfo;
        private System.Windows.Forms.ContextMenuStrip cmsdgvAttendanceInfo;
        private System.Windows.Forms.ToolStripMenuItem 查看详细记录ToolStripMenuItem;
        private System.Windows.Forms.Label lbDeviceName;
        private System.Windows.Forms.ComboBox cbDeviceName;
        private System.Windows.Forms.ComboBox cbStudentID;
        private System.Windows.Forms.ComboBox cbTime;
        private System.Windows.Forms.Label lbRange;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ProgressBar tempProgressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn UID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sex;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn age;
        private System.Windows.Forms.DataGridViewTextBoxColumn recodeData;
        private System.Windows.Forms.DataGridViewTextBoxColumn QQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn tel;
        private System.Windows.Forms.DataGridViewTextBoxColumn authority;
        private System.Windows.Forms.DataGridViewTextBoxColumn InAndOutTimes;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn exceptionRecord;

    }
}