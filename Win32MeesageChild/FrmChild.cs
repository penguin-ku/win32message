using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32HwndMessage;

namespace Win32MeesageChild
{
    public partial class FrmChild : Form
    {
        private IntPtr SendToHandle;//这个变量用于保存要发送窗口的句柄
                                    //自定义的消息


        public FrmChild()
        {
            InitializeComponent();
        }

        public FrmChild(Form p_frm)
        {
            InitializeComponent();
            SendToHandle = p_frm.Handle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HwndMessageHelper.SendHwndMessage(SendToHandle, HwndMessageType.CustomMessage, textBox1.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SendToHandle = HwndMessageHelper.FindWindow(null, "FrmMain");
            HwndMessageHelper.SendHwndMessage(SendToHandle, HwndMessageType.CustomMessage, textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HwndMessageHelper.RegistCustomWndProc(Handle, message =>
            {
                richTextBox1.Text = message.lpData;
            });
        }
    }
}
