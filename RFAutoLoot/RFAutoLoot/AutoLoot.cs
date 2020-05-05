using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Gma.System.MouseKeyHook;
using Microsoft.Win32;
using System.Windows.Input;



namespace RFAutoLoot
{
    public partial class AutoLoot : Form
    {
        #region variables
        [DllImport("user32.dll")]
        static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;
        const int KEYEVENTF_UNICODE = 0x0004;
        const int KEYEVENTF_SCANCODE = 0x0008;

        public enum KeyCode : ushort
        {
            KEY_X = 0x58,
        }
        uint 
            WM_KEYUP = 0x00000101,
            WM_KEYDOWN = 0x00000100;

        Timer t1 = new Timer();
        Timer t2 = new Timer();
        IntPtr RFWindowHandle = IntPtr.Zero;
        IKeyboardMouseEvents hook;
        #endregion variables
        public AutoLoot()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
        }

        private void SelectClick(object sender, MouseEventArgs e)
        {
            cB.Items.Clear();
            Process[] MyProcess = Process.GetProcessesByName("RF_Online.bin");
            for (int i = 0; i < MyProcess.Length; i++)
                cB.Items.Add(MyProcess[i].ProcessName); /*+ "-" + MyProcess[i].Id*/
        }

        void dowork(IntPtr hwnd)
        {
            SendKeyDown(KeyCode.KEY_X);
            /*DllImports.SendMessage(hwnd, WM_KEYDOWN, (uint)Keys.X, 0);*/
        }

        void checkRFWindow()
        {
            IntPtr hWindow = DllImports.FindWindow("D3D Window", "Genesis 50");
            IntPtr hWindow2 = DllImports.FindWindowEx(hWindow, null, "D3D Window", null);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string message =
                "---RF Auto Loot---\n" +
                "Cara Pakai :\n" +
                "1. Pilih Window / Tab RF_Online.bin\n" +
                "2. Set Waktunya (Default 0.2)\n" +
                "3. X = On/Off Looting\n" +
                "Happy Farming!\n\n" +
                "Thanks to Gaara Hokage a.k.a Tommy Hawk";
            MessageBox.Show(message, "Informasi", MessageBoxButtons.OK);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            checkRFWindow();

        }

        public void Send_Key(short Keycode, int KeyUporDown)
        {
            INPUT[] InputData = new INPUT[1];

            InputData[0].type = 1;
            InputData[0].ki.wScan = Keycode;
            InputData[0].ki.dwFlags = KeyUporDown;
            InputData[0].ki.time = 0;
            InputData[0].ki.dwExtraInfo = IntPtr.Zero;

            SendInput(1, InputData, Marshal.SizeOf(typeof(INPUT)));
        }

    }
}
