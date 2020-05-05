using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace RFAutoLoot
{
    public class Utilities
    {
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;

            public override string ToString()
            {
                return $"[Virtual Key Code: {vkCode} | Scan Code: {scanCode} | Flags: {flags.ToString()}]";
            }
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_NONE = 0x00,
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80
        }

        public static Process GetProcess(string process_name)
        {
            Process[] pname = Process.GetProcessesByName(process_name);
            if (pname.Length == 0) return null;
            else return pname[0];
        }
    }
}
