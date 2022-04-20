using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections;

namespace Motor
{
    public partial class Motor : Form
    {

        SerialComm serialComm = new SerialComm();

        public Motor()
        {
            InitializeComponent();
        }

        private void Motor_Load(object sender, EventArgs e)
        {
            this.Text = "Read serial Data using thread";
            LoadConfigurationSetting();
            gbConnect.Enabled = false;
            btnSave.Enabled = false;
            btnDisconnect.Enabled = false;
            btnSend.Enabled = false;

            txtMotorAngle.Text = "0";
            txtMotorSpeed.Text = "0";
        }
        private void LoadConfigurationSetting()
        {
            txtPortname.Text = ConfigurationManager.AppSettings["comname"];
            txtBaudrate.Text = ConfigurationManager.AppSettings["combaudrate"];
        }

        public int rx_data;
        private void SerialThread()
        {
            try
            {
                Thread serialThread = new Thread(SerialMethod); //이와 같음 : Thread serialThread = new Thread(new ThreadStart(SerialMethod));
                serialThread.Start();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Read Serial thread." + exc.Message);

            }
        }
        private void SerialMethod()
        {
            try
            {
                while (serialPortIn.IsOpen) //SerialThread 객체에서 SerialMethod 메소드를 사용하여 계속 데이터를 취득함.
                {
                    rx_data = serialPortIn.ReadByte();
                    showRxData(rx_data);
                    Thread.Sleep(10);
                }
            }
            catch 
            {
            }
        }



        private bool rxLogFlag = true;

        public delegate void ShowSerialDataDelegate(int r);
        private void showRxData(int rx_data) //Serial 스레드에서 UI 스레드 접근 시, Delegate로 접근
        {
            if (txtRead.InvokeRequired)
            {
                ShowSerialDataDelegate SSDD = showRxData;
                Invoke(SSDD, rx_data);
            }//기다림
            else
            {//기다림 후 실행
                
                if (serialComm.msgParse(((byte)rx_data), 100) == true & rxLogFlag == true)
                {
                    txtRead.Text += serialComm.PrintData;
                    txtRead.Text += Environment.NewLine + "------------------" + Environment.NewLine;
                    txtRead.Text += "Packet Length : " + serialComm.rxPKT.Length.ToString() + Environment.NewLine;
                    txtRead.Text += "Instruction : " + serialComm.rxPKT.Inst.ToString() + Environment.NewLine;
                    txtRead.Text += "Parameter :" + serialComm.rxPKT.Param.ToString() + Environment.NewLine;
                    txtRead.ScrollToCaret();

                }
            }
        }


        #region Button click
        private void btnMotorSet_Click(object sender, EventArgs e)
        {
            if (serialPortIn.IsOpen)
            {
                if (txtMotorSpeed.Text != "")
                {
                    byte[] txByte = serialComm.motorSpeedSet(txtMotorSpeed.Text);
                    serialPortIn.Write(txByte, 0, txByte.Length);                    
                }
                else { MessageBox.Show("모터 스피드를 설정하세요"); }
            }
            else
            { MessageBox.Show("Please connect port."); }
        }

        private void btnMotorAct_Click(object sender, EventArgs e)
        {

            if (serialPortIn.IsOpen)
            {
                if (txtMotorAngle.Text != "")
                {
                    byte[] txByte = serialComm.motorAngleSet(txtMotorAngle.Text);
                    serialPortIn.Write(txByte, 0, txByte.Length);
                }
                else { MessageBox.Show("모터 각도를 설정하세요"); }
            }
            else
            { MessageBox.Show("Please connect port."); }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
   
        }

        #endregion


        private void btnEdit_Click(object sender, EventArgs e)
        {
            gbConnect.Enabled = true;
            btnSave.Enabled = true;
            btnEdit.Enabled = false;
            btnConnect.Enabled = false;
        }
        private void txtPortname_DropDown(object sender, EventArgs e)
        {
            txtPortname.Items.Clear();
            string[] COMs = SerialPort.GetPortNames();
                foreach (var item in COMs)
                {
                    txtPortname.Items.Add(item);
                }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            serialPortIn.PortName = txtPortname.Text;
            serialPortIn.BaudRate = int.Parse(txtBaudrate.Text);
            serialPortIn.DataBits = 8;
            serialPortIn.StopBits = StopBits.One;
            serialPortIn.Parity = Parity.None;

            serialPortIn.Open();
            if (serialPortIn.IsOpen)
            {
                SerialThread();
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
                btnSend.Enabled = true;
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("comname");
                config.AppSettings.Settings.Remove("combaudrate");

                config.AppSettings.Settings.Add("comname", txtPortname.Text);
                config.AppSettings.Settings.Add("combaudrate", txtBaudrate.Text);

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                btnConnect.Enabled = true;
                btnEdit.Enabled = true;
                btnSave.Enabled = false;
                gbConnect.Enabled = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show("saving Error," + exc.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRead.Text = "";

        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortIn.IsOpen)
                {
                    serialPortIn.Close();
                    btnDisconnect.Enabled = false;
                    btnConnect.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = true;
                    btnSend.Enabled = false;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Disconected error." + exc.Message);
                
            }
        }
        private void txtMotorSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(Keys.Delete)))
            {
                e.Handled = true;
            }
        }
        private void txtMotorAngle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(Keys.Delete)))
            {
                e.Handled = true;
            }
        }


        
    }
}
