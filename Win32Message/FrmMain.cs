using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32HwndMessage;

namespace Win32Message
{
    public partial class FrmMain : Form
    {

        private IntPtr SendToHandle;//这个变量用于保存要发送窗口的句柄

        public FrmMain()
        {

            //FrmChild child = new FrmChild(this);
            //child.Show();
            //SendToHandle = child.Handle;
            InitializeComponent();

            //Process process = Process.Start(@"F:\dpjia.penguin.work\Dpjia.WoodenCustomization\test demo\Win32Message\Win32MeesageChild\bin\Debug\Win32MeesageChild.exe");
            //process.WaitForInputIdle();
            //SendToHandle = HwndMessageHelper.FindWindow(null, "FrmChild");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendToHandle = HwndMessageHelper.FindWindow(null, "FrmChild");
            HwndMessageHelper.SendHwndMessage(SendToHandle, HwndMessageType.CustomMessage, textBox1.Text);//发送自定义消息给句柄为SendToHandle 的窗口,
                                                                  //本例为创建本窗口的窗口句,创建时,传递给本窗口的构造函数
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            HwndMessageHelper.RegistCustomWndProc(Handle, message =>
            {
                richTextBox1.Text = message.lpData; //显示收到的自定义信息
            });
        }


    }
}
