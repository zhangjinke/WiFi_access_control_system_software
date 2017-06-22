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
using AccessControlSystem.Lib;

namespace AccessControlSystem
{
    public partial class FormDeviceManagement : Form
    {
        DeviceManagement deviceManagement;

        public FormDeviceManagement()
        {
            InitializeComponent();
            Global.netTCPServer = this.netTCPServer;
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
            device.ID = (UInt32)Convert.ToInt32(tbID.Text);
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
            device.ID = (UInt32)Convert.ToInt32(tbID.Text);
            device.name = tbDeviceName.Text;
            device.mac = tbDeviceAddress.Text;
            deviceManagement.DelDevice(device); /* 删除设备 */
            InitDataGridView();                 /* 刷新表格 */
        }
        private void FormDeviceManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; //取消关闭事件
        }
        
        private void dgvMyDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMyDevice.CurrentRow == null) { return; }
            int index = dgvMyDevice.CurrentRow.Index; /* 获取选择行号 */
            string name = dgvMyDevice.Rows[index].Cells[0].Value.ToString();

            deviceManagement = new DeviceManagement();
            int count = deviceManagement.DeviceList.Count; /* 获取设备数量 */
            for (int idx = 0; idx < count; idx++)          /* 获取设备名称与MAC */
            {
                if (deviceManagement.DeviceList[idx].name == name)
                {
                    tbDeviceName.Text = name;
                    tbID.Text = deviceManagement.DeviceList[idx].ID.ToString();
                    tbDeviceAddress.Text = deviceManagement.DeviceList[idx].mac;
                }
            }
        }
        /// <summary>
        /// TCP服务器数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void netTCPServer1_DataReceived(object sender, byte[] data)
        {
            if (Global.data_recv == null) {
                Global.data_recv = data;
            }
        }

        private void btnMesh_Click(object sender, EventArgs e)
        {
            byte[] dst_mac = String_Byte.strToHexByte(tbDeviceAddress.Text.Replace(":", ""));
            byte[] user_data = Mesh.create_topo_req(dst_mac);
            if (false == Global.netTCPServer.SendData(user_data))
            {
                return;
            }

            if (Global.wait_data(1000) == false)
            {
                MessageBox.Show("连接超时");
                return;
            }
            Mesh.mesh_header_format header;
            header = (Mesh.mesh_header_format)Mesh.espconn_mesh_analysis_packet(Global.data_recv);

            if (header.option != null)
            {
                for (int i = 0; i < header.option[0].olist.Length; i++)
                {
                    if (header.option[0].olist[0].otype == (byte)Mesh.mesh_option_type.M_O_TOPO_RESP)
                    {
                        string mac_list = "=====mac list info=====\r\n";
                        int mac_num = (header.option[0].olist[0].olen - 2) / 6;
                        byte[][] mac = new byte[mac_num][];
                        for (int j = 0; j < mac_num; j++)
                        {
                            mac[j] = new byte[6];
                            Array.Copy(header.option[0].olist[0].ovalue, j * 6, mac[j], 0, 6);
                        }
                        Global.device_list = mac;
                        for (int j = 0; j < mac_num; j++)
                        {
                            if (j == 0)
                            {
                                mac_list += "root: " + String_Byte.byteToHexStr(mac[j], ":") + "\r\n";
                            }
                            else
                            {
                                mac_list += "idx:" + (j - 1).ToString() + ", " + String_Byte.byteToHexStr(mac[j], ":") + "\r\n";
                            }
                        }
                        MessageBox.Show(mac_list);
                    }
                }
            }
            Global.data_recv = null;
        }
    }
}
