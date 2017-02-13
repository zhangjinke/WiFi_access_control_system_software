namespace AccessControlSystem
{
    partial class FormMinuteAttendanceInfo
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
            this.dgvMinuteAttendanceInfo = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.studentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMinuteAttendanceInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMinuteAttendanceInfo
            // 
            this.dgvMinuteAttendanceInfo.AllowUserToAddRows = false;
            this.dgvMinuteAttendanceInfo.AllowUserToDeleteRows = false;
            this.dgvMinuteAttendanceInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvMinuteAttendanceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMinuteAttendanceInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.deviceName,
            this.uID,
            this.studentID,
            this.state,
            this.time});
            this.dgvMinuteAttendanceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMinuteAttendanceInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvMinuteAttendanceInfo.Name = "dgvMinuteAttendanceInfo";
            this.dgvMinuteAttendanceInfo.ReadOnly = true;
            this.dgvMinuteAttendanceInfo.RowTemplate.Height = 23;
            this.dgvMinuteAttendanceInfo.Size = new System.Drawing.Size(734, 288);
            this.dgvMinuteAttendanceInfo.TabIndex = 0;
            // 
            // name
            // 
            this.name.HeaderText = "姓名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // deviceName
            // 
            this.deviceName.HeaderText = "设备名称";
            this.deviceName.Name = "deviceName";
            this.deviceName.ReadOnly = true;
            // 
            // uID
            // 
            this.uID.HeaderText = "用户号";
            this.uID.Name = "uID";
            this.uID.ReadOnly = true;
            // 
            // studentID
            // 
            this.studentID.HeaderText = "学号";
            this.studentID.Name = "studentID";
            this.studentID.ReadOnly = true;
            // 
            // state
            // 
            this.state.HeaderText = "状态";
            this.state.Name = "state";
            this.state.ReadOnly = true;
            // 
            // time
            // 
            this.time.HeaderText = "时间";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 190;
            // 
            // FormMinuteAttendanceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 288);
            this.Controls.Add(this.dgvMinuteAttendanceInfo);
            this.MaximumSize = new System.Drawing.Size(750, 1080);
            this.Name = "FormMinuteAttendanceInfo";
            this.Text = "FormMinuteAttendanceInfo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMinuteAttendanceInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvMinuteAttendanceInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn uID;
        private System.Windows.Forms.DataGridViewTextBoxColumn studentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
    }
}