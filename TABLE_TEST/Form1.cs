using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text;



namespace TABLE_TEST
{

    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        public Form1()
        {
            InitializeComponent();

            foreach (string com in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(com);
            }


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string selectedPort = comboBox1.SelectedItem as string;
            try
            {

                serialPort = new SerialPort(selectedPort, 115200);
                serialPort.DataReceived += SerialPortDataReceived;

                serialPort.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show("連線失敗：" + ex.Message);
            }



            if (selectedPort != null)
            {
                try
                {
                    MessageBox.Show("已連線成功");

                }
                catch (Exception ex)
                {

                    MessageBox.Show("連線失敗：" + ex.Message);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }



        private StringBuilder _receivedData = new StringBuilder();

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                while (serialPort.BytesToRead == 11) // Continue reading while there are enough bytes to read for a full message
                {
                    //FF 07 86 B2 B1 B2 7F 7F 7F 87 F0
                    int bytes = 11; // We know that the complete message is 11 bytes
                    byte[] Get_Data = new byte[bytes];
                    serialPort.Read(Get_Data, 0, bytes);

                    // Check that the first byte is 255 (0xFF) and the last byte is 240 (0xF0)
                    if (Get_Data[0] == 255 && Get_Data[10] == 240)
                    {
                        if (Get_Data[3] >= 128) 
                        {
                            Get_Data[3] -= 128;
                        }
                        if (Get_Data[4] >= 128)
                        {
                            Get_Data[4] -= 128;
                        }
                        if (Get_Data[5] >= 128)
                        {
                            Get_Data[5] -= 128;
                        }
                        txtBack.Text = Get_Data[3].ToString();
                        txtline.Text = Get_Data[4].ToString();
                        txtbuck.Text = Get_Data[5].ToString();
                        serialPort.DiscardInBuffer();

                    }
                    else
                    {
                        // Clear the input buffer if the received data is not in correct format
                        serialPort.DiscardInBuffer();
                    }
                }
            }));
        


        }
        static string ToBinaryString(byte[] bytes, int[] indices)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int index in indices)
            {
                sb.Append(Convert.ToString(bytes[index], 2).PadLeft(8, '0'));
                sb.Append(" ");
            }
            return sb.ToString().TrimEnd();
        }
        private string GetBinaryAtIndex(string binary, int index)
        {
            string[] binaryArray = binary.Split(' ');
            if (index >= 0 && index < binaryArray.Length)
            {
                return binaryArray[index];
            }
            return string.Empty;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (serialPort.IsOpen)
                serialPort.Close();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private int inputValue;
        private System.Windows.Forms.Timer timer;

        private void button1_Click(object sender, EventArgs e)
        {
            
            string text = textBox4.Text;

            if (!int.TryParse(text, out inputValue))
            {
                MessageBox.Show("請輸入有效的數值");
                textBox4.Text = ""; // 清除無效輸入
            }
            else
            {
                // 輸入有效，已經成功存入 inputValue 變數
                // 在此處添加其他需要執行的操作

                // 建立一個新的Timer物件並設定其間隔為inputValue毫秒
                timer = new System.Windows.Forms.Timer();
                timer.Interval = inputValue;
                timer.Tick += Timer_Tick;
                timer.Start();
                
            }
        }

        public static readonly byte[] Get_Status = { 0xFF, 0x01, 0x80, 0x81, 0xF0 };

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                serialPort.Write(Get_Status, 0, Get_Status.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("發送數據時發生錯誤: " + ex.Message);
            }
        }

        private void txtBack_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
