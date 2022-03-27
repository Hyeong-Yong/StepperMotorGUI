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

namespace Motor
{
    public partial class Motor : Form
    {
        public Thread serialThread;
        public string rx_data;



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

        }
        private void LoadConfigurationSetting()
        {
            txtPortname.Text = ConfigurationManager.AppSettings["comname"];
            txtBaudrate.Text = ConfigurationManager.AppSettings["combaudrate"];
        }


        private void SerialThread()
        {
            try
            {
                serialThread = new Thread(SerialMethod);
                serialThread.Start();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Read Serial thread." + exc.Message);

            }
        }

        private void SerialMethod()
        {
            while (serialPortIn.IsOpen) //SerialThread 객체에서 SerialMethod 메소드를 사용하여 계속 데이터를 취득함.
            {
                rx_data = serialPortIn.ReadExisting();
                showRxData(rx_data);
                Thread.Sleep(500);
            }
        }


        public delegate void ShowSerialDataDelegate(string r);
        private void showRxData(string rx_data) //Serial 스레드에서 UI 스레드 접근 시, Delegate로 접근
        {
            if (txtRead.InvokeRequired)
            {
                ShowSerialDataDelegate SSDD = showRxData;
                Invoke(SSDD, rx_data);
            }//기다림
            else
            {//기다림 후 실행
                txtRead.AppendText(Environment.NewLine + rx_data);
                txtRead.ScrollToCaret();
            }

        }



        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPortIn.IsOpen)
            {
                serialPortIn.WriteLine(txtWrite.Text);
            }
        }
















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
       
    }
}
