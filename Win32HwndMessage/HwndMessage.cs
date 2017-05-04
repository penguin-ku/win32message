using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32HwndMessage
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct HwndMessage
    {
        /// <summary>
        /// not used  
        /// </summary>
        public IntPtr dwData;

        /// <summary>
        /// 信息的长度
        /// </summary>
        public int length;

        /// <summary>
        /// 要发送的信息
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
