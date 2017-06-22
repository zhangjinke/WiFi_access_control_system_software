namespace AccessControlSystem
{
    partial class FormDeviceManagement
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
            this.gbDeviceList = new System.Windows.Forms.GroupBox();
            this.dgvMyDevice = new System.Windows.Forms.DataGridView();
            this.myDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbDeviceManagement = new System.Windows.Forms.GroupBox();
            this.btnMesh = new System.Windows.Forms.Button();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lbID = new System.Windows.Forms.Label();
            this.tbDeviceAddress = new System.Windows.Forms.TextBox();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnADD = new System.Windows.Forms.Button();
            this.lbDeviceAddress = new System.Windows.Forms.Label();
            this.lbDeviceName = new System.Windows.Forms.Label();
            this.gbTcpServer = new System.Windows.Forms.GroupBox();
            this.netTCPServer = new LeafSoft.Units.NetTCPServer();
            this.gbDeviceList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyDevice)).BeginInit();
            this.gbDeviceManagement.SuspendLayout();
            this.gbTcpServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDeviceList
            // 
            this.gbDeviceList.Controls.Add(this.dgvMyDevice);
            this.gbDeviceList.Location = new System.Drawing.Point(12, 12);
            this.gbDeviceList.Name = "gbDeviceList";
            this.gbDeviceList.Size = new System.Drawing.Size(159, 258);
            this.gbDeviceList.TabIndex = 0;
            this.gbDeviceList.TabStop = false;
            this.gbDeviceList.Text = "设备列表";
            // 
            // dgvMyDevice
            // 
            this.dgvMyDevice.AllowUserToAddRows = false;
            this.dgvMyDevice.AllowUserToDeleteRows = false;
            this.dgvMyDevice.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvMyDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.myDevice});
            this.dgvMyDevice.Location = new System.Drawing.Point(6, 20);
            this.dgvMyDevice.Name = "dgvMyDevice";
            this.dgvMyDevice.ReadOnly = true;
            this.dgvMyDevice.RowTemplate.Height = 23;
            this.dgvMyDevice.Size = new System.Drawing.Size(147, 232);
            this.dgvMyDevice.TabIndex = 0;
            this.dgvMyDevice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMyDevice_CellClick);
            // 
            // myDevice
            // 
            this.myDevice.HeaderText = "我的设备";
            this.myDevice.Name = "myDevice";
            this.myDevice.ReadOnly = true;
            // 
            // gbDeviceManagement
            // 
            this.gbDeviceManagement.Controls.Add(this.btnMesh);
            this.gbDeviceManagement.Controls.Add(this.tbID);
            this.gbDeviceManagement.Controls.Add(this.lbID);
            this.gbDeviceManagement.Controls.Add(this.tbDeviceAddress);
            this.gbDeviceManagement.Controls.Add(this.tbDeviceName);
            this.gbDeviceManagement.Controls.Add(this.btnDelete);
            this.gbDeviceManagement.Controls.Add(this.btnADD);
            this.gbDeviceManagement.Controls.Add(this.lbDeviceAddress);
            this.gbDeviceManagement.Controls.Add(this.lbDeviceName);
            this.gbDeviceManagement.Location = new System.Drawing.Point(177, 12);
            this.gbDeviceManagement.Name = "gbDeviceManagement";
            this.gbDeviceManagement.Size = new System.Drawing.Size(216, 258);
            this.gbDeviceManagement.TabIndex = 1;
            this.gbDeviceManagement.TabStop = false;
            this.gbDeviceManagement.Text = "设备维护";
            // 
            // btnMesh
            // 
            this.btnMesh.Location = new System.Drawing.Point(19, 175);
            this.btnMesh.Name = "btnMesh";
            this.btnMesh.Size = new System.Drawing.Size(89, 23);
            this.btnMesh.TabIndex = 8;
            this.btnMesh.Text = "查询Mesh";
            this.btnMesh.UseVisualStyleBackColor = true;
            this.btnMesh.Click += new System.EventHandler(this.btnMesh_Click);
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(76, 36);
            this.tbID.MaxLength = 20;
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(113, 21);
            this.tbID.TabIndex = 7;
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(29, 40);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(41, 12);
            this.lbID.TabIndex = 6;
            this.lbID.Text = "设备ID";
            // 
            // tbDeviceAddress
            // 
            this.tbDeviceAddress.Location = new System.Drawing.Point(76, 104);
            this.tbDeviceAddress.MaxLength = 255;
            this.tbDeviceAddress.Name = "tbDeviceAddress";
            this.tbDeviceAddress.Size = new System.Drawing.Size(113, 21);
            this.tbDeviceAddress.TabIndex = 5;
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Location = new System.Drawing.Point(76, 70);
            this.tbDeviceName.MaxLength = 20;
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.Size = new System.Drawing.Size(113, 21);
            this.tbDeviceName.TabIndex = 4;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(114, 142);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(89, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除设备";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnADD
            // 
            this.btnADD.Location = new System.Drawing.Point(19, 142);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(89, 23);
            this.btnADD.TabIndex = 2;
            this.btnADD.Text = "添加设备";
            this.btnADD.UseVisualStyleBackColor = true;
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // lbDeviceAddress
            // 
            this.lbDeviceAddress.AutoSize = true;
            this.lbDeviceAddress.Location = new System.Drawing.Point(23, 108);
            this.lbDeviceAddress.Name = "lbDeviceAddress";
            this.lbDeviceAddress.Size = new System.Drawing.Size(47, 12);
            this.lbDeviceAddress.TabIndex = 1;
            this.lbDeviceAddress.Text = "MAC地址";
            // 
            // lbDeviceName
            // 
            this.lbDeviceName.AutoSize = true;
            this.lbDeviceName.Location = new System.Drawing.Point(17, 74);
            this.lbDeviceName.Name = "lbDeviceName";
            this.lbDeviceName.Size = new System.Drawing.Size(53, 12);
            this.lbDeviceName.TabIndex = 0;
            this.lbDeviceName.Text = "设备名称";
            // 
            // gbTcpServer
            // 
            this.gbTcpServer.Controls.Add(this.netTCPServer);
            this.gbTcpServer.Location = new System.Drawing.Point(399, 12);
            this.gbTcpServer.Name = "gbTcpServer";
            this.gbTcpServer.Size = new System.Drawing.Size(200, 258);
            this.gbTcpServer.TabIndex = 6;
            this.gbTcpServer.TabStop = false;
            this.gbTcpServer.Text = "TCP服务器";
            // 
            // netTCPServer
            // 
            this.netTCPServer.Location = new System.Drawing.Point(6, 20);
            this.netTCPServer.Name = "netTCPServer";
            this.netTCPServer.Size = new System.Drawing.Size(188, 232);
            this.netTCPServer.TabIndex = 10;
            this.netTCPServer.DataReceived += new LeafSoft.Lib.LeafEvent.DataReceivedHandler(this.netTCPServer1_DataReceived);
            // 
            // FormDeviceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 296);
            this.Controls.Add(this.gbTcpServer);
            this.Controls.Add(this.gbDeviceManagement);
            this.Controls.Add(this.gbDeviceList);
            this.Name = "FormDeviceManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设备管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDeviceManagement_FormClosing);
            this.Load += new System.EventHandler(this.FormDeviceManagement_Load);
            this.gbDeviceList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyDevice)).EndInit();
            this.gbDeviceManagement.ResumeLayout(false);
            this.gbDeviceManagement.PerformLayout();
            this.gbTcpServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDeviceList;
        private System.Windows.Forms.DataGridView dgvMyDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn myDevice;
        private System.Windows.Forms.GroupBox gbDeviceManagement;
        private System.Windows.Forms.TextBox tbDeviceAddress;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnADD;
        private System.Windows.Forms.Label lbDeviceAddress;
        private System.Windows.Forms.Label lbDeviceName;
        private LeafSoft.Units.NetTCPServer netTCPServer;
        private System.Windows.Forms.GroupBox gbTcpServer;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Button btnMesh;
    }
}