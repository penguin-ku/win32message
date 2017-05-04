using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32HwndMessage
{
    /// <summary>
    /// 窗体消息助手
    /// </summary>
    public static class HwndMessageHelper
    {
        public const int WM_COPYDATA = 0x004A;
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate IntPtr CallBack(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        // 文档：https://msdn.microsoft.com/en-us/library/windows/desktop/ms633499(v=vs.85).aspx
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow
        (
            string lpClassName,                         // 窗口类
            string lpWindowName                         // 窗口主题
        );

        // 文档：https://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage
        (
            IntPtr hWnd,                        // 信息发住的窗口的句柄
            int Msg,                            // 消息ID
            int wParam,                      // 参数1
            ref HwndMessage lParam              // 参数2
        );

        // 文档：https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591(v=vs.85).aspx
        private const int GWL_EXSTYLE = -20;
        private const int GWL_HINSTANCE = -6;
        private const int GWL_ID = -12;
        private const int GWL_STYLE = -16;
        private const int GWL_USERDATA = -21;
        private const int GWL_WNDPROC = -4;
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong
        (
            IntPtr hWnd,                        // 窗口句柄
            int nIndex,                         // 需要设置的类别，此处取值强制为-4
            CallBack wndProc                    // 窗口消息处理的函数
        );

        // 文档：https://msdn.microsoft.com/en-us/library/windows/desktop/ms633571(v=vs.85).aspx
        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc
        (
            IntPtr wndProc,                     // 前消息处理函数
            IntPtr hWnd,                        // 窗口句柄
            int msg,                            // 消息类型
            IntPtr wParam,                      // 消息参数1
            IntPtr lParam                       // 消息参数2
        );

        /// <summary>
        /// 发送窗体消息
        /// </summary>
        /// <param name="p_hWnd">窗口句柄</param>
        /// <param name="p_type">消息类型</param>
        /// <param name="p_strMessage">消息内容</param>
        /// <returns></returns>
        public static int SendHwndMessage(IntPtr p_hWnd, HwndMessageType p_type, string p_strMessage)
        {
            HwndMessage message = new HwndMessage()
            {
                dwData = (IntPtr)100,
                lpData = p_strMessage,
                length = System.Text.Encoding.Default.GetByteCount(p_strMessage) + 1
            };
            return SendMessage(p_hWnd, WM_COPYDATA, 0, ref message);
        }

        /// <summary>
        /// 注册自定义的消息处理函数
        /// </summary>
        /// <param name="p_hWnd"></param>
        /// <param name="p_winProc"></param>
        public static void RegistCustomWndProc(IntPtr p_hWnd, Action<HwndMessage> p_winProc)
        {
            IntPtr oldWndProc = IntPtr.Zero;
            CallBack hwndProc = (hwnd, msg, wParam, lParam) =>
            {
                if (msg == WM_COPYDATA)
                {
                    p_winProc(ParseMessage(lParam));
                }
                return CallWindowProc(oldWndProc, hwnd, msg, wParam, lParam);
            };
            oldWndProc = SetWindowLong(p_hWnd, GWL_WNDPROC, hwndProc);
        }

        private static HwndMessage ParseMessage(IntPtr pData)
        {
            HwndMessage pControllerInfo = new HwndMessage();
            byte[] pByte = new byte[512];
            Marshal.Copy(pData, pByte, 0, Marshal.SizeOf(pControllerInfo));
            HwndMessage message = (HwndMessage)BytesToStruct(pByte, pControllerInfo.GetType());
            return message;
        }

        private static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
