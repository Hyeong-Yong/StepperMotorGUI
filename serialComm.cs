using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace LaserWritingGUI
{
    class Constants
    {
        public const byte STX = 0xFF;
    }

    class Instructions
    {
        public const byte SET_ANGLE = 0x01;
        public const byte SET_RPM = 0x02;
        public const byte SET_DIRECTION = 0x03;
    }
    class PKT_INDEX
    {
        public const int HEADER = 0;
        public const int LENGTH = 1;
        public const int INST = 2;
        public const int PARAM_1 = 3;
        public const int PARAM_2 = 4;
        public const int PARAM_3 = 5;
        public const int PARAM_4 = 6;
        public const int CHECK = 7;
    }

    class PKT_STATE
    {
        public const int HEADER = 0;
        public const int LENGTH = 1;
        public const int INST = 2;
        public const int PARAM_1 = 3;
        public const int PARAM_2 = 4;
        public const int PARAM_3 = 5;
        public const int PARAM_4 = 6;
        public const int CHECK = 7;
    }
    class PKT
    {
        private int length;
        private int inst;
        private int param;
        public int Length { get => length; set => length = value; }
        public int Inst { get => inst; set => inst = value; }
        public int Param { get => param; set => param = value; }
    }

    class SerialComm
    {
        public PKT rxPKT = new PKT();
        private int state = PKT_STATE.HEADER;
        //구조체 생성자로 초기화 설정할수 있는데 필요하 나?
        // public PKT rxPKT = new PKT (a,b,c,d);....
        private string printData;
        public string PrintData { get => printData; set => printData = value; }

        public bool checksumFlag;
        DateTime preTime = DateTime.Now;

        byte[] rxbuf = new byte[PKT_INDEX.CHECK + 1];
        Queue<byte> rxMessage = new Queue<byte>();


        public bool msgParse(byte rx_data, int timeout)
        {
            checksumFlag = false;

            if ((DateTime.Now - preTime).TotalMilliseconds > timeout)
            {
                state = PKT_STATE.HEADER;
                preTime = DateTime.Now;
            }
            /////Add : Time out 걸어서 header state로 이동
            // 상태머신으로 parsing
            switch (state)
            {
                case PKT_STATE.HEADER:
                    if (rx_data == 0xFF)
                    {
                        rxMessage.Clear();
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
                    state = PKT_STATE.PARAM_3;
                    break;

                case PKT_STATE.PARAM_3:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.PARAM_4;
                    break;
                case PKT_STATE.PARAM_4:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.CHECK;
                    break;

                case PKT_STATE.CHECK:
                    rxMessage.Enqueue(rx_data);
                    int i = 0;
                    while (rxMessage.Count > 0)
                    { rxbuf[i++] = rxMessage.Dequeue(); }
                    checksumFlag = ChecksumPAK(rxbuf, rxbuf[PKT_INDEX.CHECK]); //체크섬 확인
                    if (checksumFlag == true)
                    {
                        //패킷메세지를 전역변수 구조체에 넘겨주기.
                        rxPKT.Length = rxbuf[PKT_INDEX.LENGTH];
                        rxPKT.Inst = rxbuf[PKT_INDEX.INST];
                        rxPKT.Param = rxbuf[PKT_INDEX.PARAM_1] << 0;
                        rxPKT.Param |= rxbuf[PKT_INDEX.PARAM_2] << 8;
                        rxPKT.Param |= rxbuf[PKT_INDEX.PARAM_2] << 16;
                        rxPKT.Param |= rxbuf[PKT_INDEX.PARAM_2] << 24;
                    }
                    state = PKT_STATE.HEADER;
                    break;
            }
            PrintData = BitConverter.ToString(rxbuf);
            PrintData = PrintData.Replace('-', ' ');

            return checksumFlag;
/*            if (checksumFlag == true & rxLogFlag == true)
            {
                string hexString = BitConverter.ToString(rxbuf);
                hexString = hexString.Replace('-', ' ');
                return hexString;
*//*              txtRead.Text += hexString;
                txtRead.Text += Environment.NewLine + "------------------" + Environment.NewLine;
                txtRead.Text += "Length : " + rxPKT.length.ToString() + Environment.NewLine;
                txtRead.Text += "Instruction : " + rxPKT.inst.ToString() + Environment.NewLine;
                txtRead.Text += "Parameter :" + rxPKT.param.ToString() + Environment.NewLine;*//*
            }
            else { 
            return "";
            }*/
        }

        private byte ChecksumByte(byte[] txData)
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

        public byte[] LaserWritingGUISpeedSet(string speed)
        {
            //// 0 : 연결잘됨, 1 : serial 연결문제, 2 : Text 공백
                    byte[] txByte = new byte[PKT_INDEX.CHECK+1];
                    txByte[PKT_INDEX.HEADER] = Constants.STX;
                    txByte[PKT_INDEX.LENGTH] = 0x06;  // INST+PARAM[4]+CHECK
                    txByte[PKT_INDEX.INST] = Instructions.SET_RPM;
                    txByte[PKT_INDEX.PARAM_1] = Convert.ToByte(0xFF & (uint.Parse(speed) >> 0));
                    txByte[PKT_INDEX.PARAM_2] = Convert.ToByte(0xFF & (uint.Parse(speed) >> 8));
                    txByte[PKT_INDEX.PARAM_3] = Convert.ToByte(0xFF & (uint.Parse(speed) >> 16));
                    txByte[PKT_INDEX.PARAM_4] = Convert.ToByte(0xFF & (uint.Parse(speed) >> 24));
                    txByte[PKT_INDEX.CHECK] = Convert.ToByte(ChecksumByte(txByte));
            return txByte;
        }
        public byte[] LaserWritingGUIAngleSet(string angle)
        {
                    byte[] txByte = new byte[PKT_INDEX.CHECK + 1];
                    txByte[PKT_INDEX.HEADER] = Constants.STX;
                    txByte[PKT_INDEX.LENGTH] = 0x06;  // INST+PARAM[4]+CHECK
                    txByte[PKT_INDEX.INST] = Instructions.SET_ANGLE;
                    txByte[PKT_INDEX.PARAM_1] = Convert.ToByte(0xFF & (uint.Parse(angle) >> 0));
                    txByte[PKT_INDEX.PARAM_2] = Convert.ToByte(0xFF & (uint.Parse(angle) >> 8));
                    txByte[PKT_INDEX.PARAM_3] = Convert.ToByte(0xFF & (uint.Parse(angle) >> 16));
                    txByte[PKT_INDEX.PARAM_4] = Convert.ToByte(0xFF & (uint.Parse(angle) >> 24));
                    txByte[PKT_INDEX.CHECK] = Convert.ToByte(ChecksumByte(txByte));
            return txByte;
        }


    }
}
