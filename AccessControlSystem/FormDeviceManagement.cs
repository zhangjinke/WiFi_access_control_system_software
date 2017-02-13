using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using AccessControlSystem.Model;

namespace AccessControlSystem
{
    public partial class FormDeviceManagement : Form
    {
        DeviceManagement deviceManagement;

        public FormDeviceManagement()
        {
            InitializeComponent();
        }

        public void FormDeviceManagement_Load(object sender, EventArgs e)
        {
            InitDataGridView();
            dgvMyDevice_CellClick(null, null);
        }

        private void InitDataGridView()
        {
            dgvMyDevice.Rows.Clear();                      /* 清空表格 */
            deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 设备数量 */
            for (int idx = 0; idx < count; idx++)          /* 将设备添加到表格中 */
            {
                int index = dgvMyDevice.Rows.Add();
                dgvMyDevice.Rows[index].Cells[0].Value = deviceManagement.DeviceList[idx].name;
            }
        }
        private void btnADD_Click(object sender, EventArgs e)
        {
            if (tbDeviceName.Text == "") { MessageBox.Show(lbDeviceName.Text + "不能为空"); return; }
            if (tbDeviceAddress.Text == "") { MessageBox.Show(lbDeviceAddress.Text + "不能为空"); return; }
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\DeviceInfo.db";
            DeviceManagement.DeviceInfo device = new DeviceManagement.DeviceInfo();
            device.name = tbDeviceName.Text;
            device.mac = tbDeviceAddress.Text;
            deviceManagement.AddDevice(device); /* 添加设备 */
            InitDataGridView();                 /* 刷新表格 */
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbDeviceName.Text == "") { MessageBox.Show(lbDeviceName.Text + "不能为空"); return; }
            if (tbDeviceAddress.Text == "") { MessageBox.Show(lbDeviceAddress.Text + "不能为空"); return; }
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\DeviceInfo.db";
            DeviceManagement.DeviceInfo device = new DeviceManagement.DeviceInfo();
            device.name = tbDeviceName.Text;
            device.mac = tbDeviceAddress.Text;
            deviceManagement.DelDevice(device); /* 删除设备 */
            InitDataGridView();                 /* 刷新表格 */
        }
        private void FormDeviceManagement_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        
        private void dgvMyDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvMyDevice.CurrentRow.Index; /* 获取选择行号 */
            string name = dgvMyDevice.Rows[index].Cells[0].Value.ToString();

            deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 获取设备数量 */
            for (int idx = 0; idx < count; idx++)          /* 获取设备名称与MAC */
            {
                if (deviceManagement.DeviceList[idx].name == name)
                {
                    tbDeviceName.Text = name;
                    tbDeviceAddress.Text = deviceManagement.DeviceList[idx].mac;
                }
            }
        }
    }
}
