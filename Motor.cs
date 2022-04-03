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

namespace Motor
{
    public partial class Motor : Form
    {
        //public Thread serialThread;
        public int rx_data;

        

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

            txtMotorAngle.Text = "17734";
            txtMotorSpeed.Text = "17734";

        }
        private void LoadConfigurationSetting()
        {
            txtPortname.Text = ConfigurationManager.AppSettings["comname"];
            txtBaudrate.Text = ConfigurationManager.AppSettings["combaudrate"];
        }

        #region Receive data
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
                
                txtRead.AppendText(Convert.ToChar(rx_data)+Environment.NewLine);
                txtRead.ScrollToCaret();

            }
        }


        /// <summary>
        /// instruction ------ 0x01 : set speed, 0x02 : set angle, 0x03 : clock (1) or anticlock (-1)
        /// </summary>
        private struct PKT
        {
            public int length;
            public int inst;
            public int param;
        }
        PKT rxPKT = new PKT();
        byte[] rxbuf;
        int state = PKT_STATE.HEADER;

        private void msgParse(byte rx_data)
        {
            Queue<byte> rxMessage = new Queue<byte>();
            /////Add : Time out 걸어서 header state로 이동
            // 상태머신으로 parsing
            switch (state) 
            { 
                case PKT_STATE.HEADER:
                    if (rx_data == 0xFF)
                    {
                        rxMessage.Enqueue(rx_data);
                        state = PKT_STATE.LENGTH;
                    }
                break;

                case PKT_STATE.LENGTH:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.INST;
                    break;

                case PKT_STATE.INST:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.PARAM_1;
                    break;

                case PKT_STATE.PARAM_1:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.PARAM_2;
                    break;


                case PKT_STATE.PARAM_2:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.CHECK;
                    break;

                case PKT_STATE.CHECK:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.HEADER;
                    break;
            }

            //체크섬 확인
            if (ChecksumPAK(rxbuf, rxbuf[PKT_INDEX.CHECK]))
            { 
                for(int i=0; i<rxMessage.Count();i++)
                {
                    rxbuf[i] = rxMessage.Dequeue();
                }
                //패킷메세지를 전역변수 구조체에 넘겨주기.
                rxPKT.length = rxbuf[PKT_INDEX.LENGTH];
                rxPKT.inst = rxbuf[PKT_INDEX.INST];
                rxPKT.param = rxbuf[PKT_INDEX.PARAM_1] << 0;
                rxPKT.param |= rxbuf[PKT_INDEX.PARAM_1] << 8; 
            }
            

        }






        #endregion



        #region Transmit data 

        private int motorSpeedSet()
        {
            int error = 0; // 0 : 연결잘됨, 1 : serial 연결문제, 2 : Text 공백
            if (serialPortIn.IsOpen)
            {
                if (txtMotorSpeed.Text != "")
                { 
                byte[] txByte = new byte[6];
                txByte[PKT_INDEX.HEADER] = Constants.STX;
                txByte[PKT_INDEX.LENGTH] = 0x04;  // INST+PARAM[2]+CHECK
                txByte[PKT_INDEX.INST] = Instructions.speedInst;
                txByte[PKT_INDEX.PARAM_1] = Convert.ToByte(0xFF & (uint.Parse(txtMotorSpeed.Text) >> 0));
                txByte[PKT_INDEX.PARAM_2] = Convert.ToByte(0xFF & (uint.Parse(txtMotorSpeed.Text) >> 8));
                txByte[PKT_INDEX.CHECK] = Convert.ToByte(sendChecksum(txByte));
                serialPortIn.Write(txByte, 0, txByte.Length);
                error = 0;
                }
                else { return error = 2; }
            }
            else
            {
                return error =1;
            }
            return error;
        }

        private int motorAngleSet()
        {
            int error = 0;
            if (serialPortIn.IsOpen)
            {
                if (txtMotorSpeed.Text != "")
                {
                    byte[] txByte = new byte[6];
                    txByte[PKT_INDEX.HEADER] = Constants.STX;
                    txByte[PKT_INDEX.LENGTH] = 0x04;  // INST+PARAM[2]+CHECK
                    txByte[PKT_INDEX.INST] = Instructions.angleInst;
                    txByte[PKT_INDEX.PARAM_1] = Convert.ToByte(0xFF & (uint.Parse(txtMotorSpeed.Text) >> 0));
                    txByte[PKT_INDEX.PARAM_2] = Convert.ToByte(0xFF & (uint.Parse(txtMotorSpeed.Text) >> 8));
                    txByte[PKT_INDEX.CHECK] = Convert.ToByte(sendChecksum(txByte));
                    serialPortIn.Write(txByte, 0, txByte.Length);
                    error = 0;
                }
                else { return error = 2; }
            }
            else
            {
                return error = 1;
            }
            return error;
        }


        #endregion


        #region Button click
        private void btnMotorSet_Click(object sender, EventArgs e)
        {
            if (motorSpeedSet() == 1)
            {
                MessageBox.Show("Please connect port.");
            }
            if (motorSpeedSet() == 2)
            {
                MessageBox.Show("모터 스피드를 설정하세요");
            }
        }
        private void btnMotorAct_Click(object sender, EventArgs e)
        {
            if (motorAngleSet() == 1)
            {
                MessageBox.Show("Please connect port.");
            }
            if (motorAngleSet() == 2)
            {
                MessageBox.Show("모터 각도를 설정하세요");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
   
        }

        #endregion



        private byte sendChecksum(byte[] txData)
        {
            uint sum = 0;
            for (int i = 0; i < txData.Length - 1; i++)
            {
                sum += Convert.ToUInt32(txData[i]);
            }
            sum = sum & 0xFF;
            sum = (~sum + 1) & 0xFF;

            return ((byte)sum);
            //            total += sum;
            //            return total;
        }




        private bool ChecksumPAK(byte[] txData, byte Checkbyte)
        {
            bool ret = false;
            uint sum = 0, total = 0;
            for (int i = 0; i < txData.Length - 1; i++)
            {
                sum += Convert.ToUInt32(txData[i]);
            }
            total = sum;
            total = total & 0xFF;
            total = (~total + 1) & 0xFF;
            total += sum;
            total = total & 0xFF;
            if (total == 0)
            {
                return ret = true;
            }
            return ret;
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
