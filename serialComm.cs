using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Motor
{
    class serialComm 
    {
        private System.IO.Ports.SerialPort serialPortIn;

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

                txtRead.AppendText(Convert.ToChar(rx_data) + Environment.NewLine);
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
                for (int i = 0; i < rxMessage.Count(); i++)
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
                return error = 1;
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

    }
}
