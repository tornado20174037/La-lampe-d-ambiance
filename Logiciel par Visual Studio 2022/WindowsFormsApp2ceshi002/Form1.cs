//By LI Xiangjun
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
using System.Threading;
namespace WindowsFormsApp2ceshi002
{
    public partial class Form1 : Form
    {
        SerialPort sp = new SerialPort(); //定义一个SerialPort实现串口传输支持
        byte[] sendBytes = new byte[5]; //定义一个字节数组，包含传输的RGB值
        //int sendInts = 0;
        public Form1()
        {
            InitializeComponent();
            sp.DataReceived += Sp_DataReceived;
            CheckForIllegalCrossThreadCalls = false;
            sendBytes[0] = 0x01;
            sendBytes[1] = 0x00;
            sendBytes[2] = 0x00;
            sendBytes[3] = 0x00;
            sendBytes[4] = 0x02;
            SetTextColorValue();
            //comboBox2.SelectedIndex = 5;
            //添加波特率列表
            string[] baud = { "9600", "115200" };
            comboBox2.Items.AddRange(baud);

            //设置选项默认值
            comboBox2.Text = "9600";
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //textBox4.AppendText(sp.ReadLine() + Environment.NewLine);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个串口进行通信！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!sp.IsOpen)
            {

                SetTextColorValue();
                sp.PortName = comboBox1.SelectedItem.ToString();  //用户选择串口输入
                sp.BaudRate = Convert.ToInt32(comboBox2.SelectedItem.ToString());  //波特率设置
                sp.Open();
                Send();
                button1.Text = "Close the Port";
            }
            else
            {
                sp.Close();
                button1.Text = "Open the Port";
                sendBytes[0] = 0x00;
                sendBytes[1] = 0x00;
                sendBytes[2] = 0x00;
                sendBytes[3] = 0x00;
                sendBytes[4] = 0x00;
                Send();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                //首先判断串口是否开启
                if (sp.IsOpen)
                {
                //串口处于开启状态，将发送区文本发送
                //以ASCII模式发送

                    sendBytes[0] = 0x02; 
                    sendBytes[4] = 0x03;
                    //sendBytes[1] = 0x02;
                    sendBytes[3] = Convert.ToByte(textBox4.Text);
                   // sendInts = Convert.ToInt32(textBox4.Text);
                    Send();
                    
                }
            
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sp.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x02;
                sendBytes[1] = colorDialog1.Color.R;  //字节数组1,2,3位是RGB色值 0-255
                sendBytes[2] = colorDialog1.Color.G;
                sendBytes[3] = colorDialog1.Color.B;
                Send();
                //trackBar1.Value = colorDialog1.Color.R;
                //trackBar2.Value = colorDialog1.Color.G;
                //trackBar3.Value = colorDialog1.Color.B;
                trackBar1.Value = sendBytes[1];
                trackBar2.Value = sendBytes[2];
                trackBar3.Value = sendBytes[3];
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                SetTextColorValue();
                SetPicColor();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            sendBytes[0] = 0x01;
            sendBytes[4] = 0x02;
            sendBytes[1] = Convert.ToByte(trackBar1.Value);
            trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
            trackBar4.Value = 255;
            textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
            SetPicColor();
            SetTextColorValue();
            Send();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            sendBytes[0] = 0x01;
            sendBytes[4] = 0x02;
            sendBytes[2] = Convert.ToByte(trackBar2.Value);
            trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
            trackBar4.Value = 255;
            textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
            SetPicColor();
            SetTextColorValue();
            Send();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            sendBytes[0] = 0x01;
            sendBytes[4] = 0x02;
            sendBytes[3] = Convert.ToByte(trackBar3.Value);
            trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
            trackBar4.Value = 255;
            textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
            SetPicColor();
            SetTextColorValue();
            Send();
        }
        private void Send()
        {
            if (sp.IsOpen)
            { 
                sp.Write(sendBytes, 0, 5);
                //sp.Write(sendInts);
            }
        }
        //private void Send_1()
        //{
        //    if (sp.IsOpen)
        //    {

        //        serialPort1.Write("sendchar");
        //    }
        //}
        private void SetPicColor()
        {
            Color c = Color.FromArgb(0xff, trackBar1.Value, trackBar2.Value, trackBar3.Value);
            pictureBox1.BackColor = c;
        }

        private void SetTextColorValue()
        {
            textBox3.Text = string.Format("{0:D3}, {1:D3}, {2:D3}", trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void bling(int q)
        {

            int a = trackBar1.Value;
            int b = trackBar2.Value;
            int c = trackBar3.Value;
            for (int i = 0; i <= sendBytes[0]; i++)
            {
                trackBar1.Value = a;
                trackBar2.Value = b;
                trackBar3.Value = c;
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                SetPicColor();
                SetTextColorValue();
                Thread.Sleep(q);
                trackBar1.Value = 0;
                trackBar2.Value = 0;
                trackBar3.Value = 0;
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                SetPicColor();
                SetTextColorValue();
                //爆闪
                Thread.Sleep(q);//延时
            }
        }
       
        //private void button2_Click_1(object sender, EventArgs e)
        //{
        //    if (!sp.IsOpen)
        //    {
        //        return;
        //    }
        //    sendBytes[4] = 0x03;
        //    Send();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断串口是否开启
                if (sp.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    //以ASCII模式发送
                    sendBytes[0] = 0x01;

                    sendBytes[4] = 0x04;
                    trackBar4.Value = 120;
                    sendBytes[1] = Convert.ToByte(trackBar4.Value);
                    sendBytes[2] = Convert.ToByte(trackBar4.Value);
                    sendBytes[3] = Convert.ToByte(trackBar4.Value);
                    trackBar1.Value = trackBar4.Value;
                    trackBar2.Value = trackBar4.Value;
                    trackBar3.Value = trackBar4.Value;
                    
                    textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
                    trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                    trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                    trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                    SetTextColorValue();
                    SetPicColor();
                    Send();
                    //sendBytes[4] = 0x02;
                }
            }
            catch (Exception ex)
            {
                sp.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                sp = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断串口是否开启
                if (sp.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    //以ASCII模式发送
                    sendBytes[0] = 0x01;
                    sendBytes[4] = 0x05;
                    trackBar4.Value = 0;
                    sendBytes[1] = Convert.ToByte(trackBar4.Value);
                    sendBytes[2] = Convert.ToByte(trackBar4.Value);
                    sendBytes[3] = Convert.ToByte(trackBar4.Value);
                    trackBar1.Value = 0;
                    trackBar2.Value = 0;
                    trackBar3.Value = 0;
                    trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                    trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                    trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                    SetPicColor();
                    SetTextColorValue();
                    textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
                    Send();
                }
            }
            catch (Exception ex)
            {
                sp.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                sp = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                //串口处于开启状态，将发送区文本发送
                //以ASCII模式发送
                //sendBytes[0] = 0x02;
                //sendBytes[4] = 0x03;
                //sendBytes[1] = 0x00;
                
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x05;
                Send();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = Convert.ToByte(textBox8.Text);
                sendBytes[4] = 0x06;
                sendBytes[1] = Convert.ToByte(trackBar1.Value);
                sendBytes[2] = Convert.ToByte(trackBar2.Value);
                sendBytes[3] = Convert.ToByte(trackBar3.Value);
                Send();
                bling(100);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = Convert.ToByte(textBox8.Text);
                sendBytes[4] = 0x07;
                sendBytes[1] = Convert.ToByte(trackBar1.Value);
                sendBytes[2] = Convert.ToByte(trackBar2.Value);
                sendBytes[3] = Convert.ToByte(trackBar3.Value);
                Send();
                bling(500);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = Convert.ToByte(textBox8.Text);
                sendBytes[4] = 0x08;
                sendBytes[1] = Convert.ToByte(trackBar1.Value);
                sendBytes[2] = Convert.ToByte(trackBar2.Value);
                sendBytes[3] = Convert.ToByte(trackBar3.Value);
                Send();
                bling(1000);
                
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x09;
                sendBytes[2] = Convert.ToByte(textBox9.Text);
                trackBar1.Value = 255;
                trackBar2.Value = 0;
                trackBar3.Value = 0;
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                SetPicColor();
                SetTextColorValue();
                Send();
                breath_red();
            }
        }
        private void breath_red()
        {
            for (int i = 0; i < sendBytes[2]; i++)
            {
                for (int j = 0; j <= 255; j += 3)//为了同步
                {
                    trackBar4.Value = j;
                    trackBar4.BackColor = Color.FromArgb(0xff, trackBar4.Value, 0, 0);
                    textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
                    Thread.Sleep(5);
                }
                for (int j = 255; j >= 0; j -= 3)
                {
                    trackBar4.Value = j;
                    trackBar4.BackColor = Color.FromArgb(0xff, trackBar4.Value, 0, 0);
                    textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
                    Thread.Sleep(5);
                }
                Thread.Sleep(800);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x10;
                sendBytes[1] = Convert.ToByte(trackBar1.Value);
                sendBytes[2] = Convert.ToByte(trackBar2.Value);
                sendBytes[3] = Convert.ToByte(trackBar3.Value);
                Send();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x11;
                sendBytes[2] = Convert.ToByte(textBox9.Text);
                trackBar1.Value = 0;
                trackBar2.Value = 255;
                trackBar3.Value = 0;
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
                SetPicColor();
                SetTextColorValue();
                Send();
                for (int i = 0; i < sendBytes[2]; i++)
                {
                    for (int j = 0; j <= 255; j += 3)//为了同步
                    {
                        trackBar4.Value = j;
                        trackBar4.BackColor = Color.FromArgb(0xff, 0, trackBar4.Value, 0);
                        textBox10.Text = string.Format("{0:D3}", j);
                        Thread.Sleep(5);
                    }
                    for (int j = 255; j >= 0; j -= 3)
                    {
                        
                        trackBar4.Value = j;
                        trackBar4.BackColor = Color.FromArgb(0xff, 0, trackBar4.Value, 0);
                        textBox10.Text = string.Format("{0:D3}", j);
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(800);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x12;
                sendBytes[2] = Convert.ToByte(textBox9.Text);
                trackBar1.Value = 0;
                trackBar2.Value = 0;
                trackBar3.Value = 255;
                trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
                trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
                trackBar3.BackColor = Color.FromArgb(0xff,  0, 0, trackBar3.Value);
                SetPicColor();
                SetTextColorValue();
                Send();
                for (int i = 0; i < sendBytes[2]; i++)
                {
                    for (int j = 0; j <= 255; j += 3)//为了同步
                    {
                        trackBar4.Value = j;
                        trackBar4.BackColor = Color.FromArgb(0xff, 0, 0, trackBar4.Value);
                        textBox10.Text = string.Format("{0:D3}", j);
                        Thread.Sleep(5);
                    }
                    for (int j = 255; j >= 0; j -= 3)
                    {

                        trackBar4.Value = j;
                        trackBar4.BackColor = Color.FromArgb(0xff, 0, 0, trackBar4.Value);
                        textBox10.Text = string.Format("{0:D3}", j);
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(800);
                }
            }
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {

            sendBytes[1] = Convert.ToByte(trackBar4.Value);
            sendBytes[2] = Convert.ToByte(trackBar4.Value);
            sendBytes[3] = Convert.ToByte(trackBar4.Value);

            textBox10.Text = string.Format("{0:D3}", trackBar4.Value);
            trackBar1.Value = trackBar4.Value;
            trackBar2.Value = trackBar4.Value;
            trackBar3.Value = trackBar4.Value;
            trackBar1.BackColor = Color.FromArgb(0xff, trackBar1.Value, 0, 0);
            trackBar2.BackColor = Color.FromArgb(0xff, 0, trackBar2.Value, 0);
            trackBar3.BackColor = Color.FromArgb(0xff, 0, 0, trackBar3.Value);
            SetPicColor();
            SetTextColorValue();
            Send();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x13;
                Send();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sendBytes[0] = 0x01;
                sendBytes[4] = 0x14;
                Send();
            }
        }
    }
}