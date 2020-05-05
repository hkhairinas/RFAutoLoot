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


namespace RFAutoLoot
{
    public partial class AutoLoot : Form
    {
        #region variables

        uint
            WM_KEYUP = 0x00000101,
            SW_SHOW = 0x0005,
            SW_SHOWMINIMIZED = 2,
            WM_SYSKEYDOWN = 0x104,
            WM_SYSKEYUP = 0x105,
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
            num_delay.Minimum = 0.1m;
            num_delay.Maximum = 1;
            num_delay.DecimalPlaces = 1;
            num_delay.Increment = 0.1m;
            t1.Interval = Convert.ToInt32(num_delay.Value * (decimal)1000);
            t1.Enabled = false;
            t1.Tick += (s, e) => { dowork(RFWindowHandle); };
            t2.Interval = 1000;
            t2.Tick += (s, e) => { checkRFWindow(); };
        }

        private void SelectClick(object sender, MouseEventArgs e)
        {
            cB.Items.Clear();
            Process[] MyProcess = Process.GetProcessesByName("RF_Online.bin");
            for (int i = 0; i < MyProcess.Length; i++)
                cB.Items.Add(MyProcess[i].Id); /**/
        }

        private void AutoLoot_Load(object sender, EventArgs e)
        {

        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X)
            {
                if (t1.Enabled)
                {
                    t1.Enabled = false;
                    t1.Stop();
                    btnStart.Text = "Looting Stop";
                }
                else
                {
                    t1.Enabled = true;
                    t1.Start();
                    btnStart.Text = "Looting Start";
                }
            }
        }

        void dowork(IntPtr hwnd)
        {
            if (RFWindowHandle != IntPtr.Zero)
            {
                DllImports.SendMessage(hwnd, WM_KEYDOWN, (uint)Keys.X, 0);
            }
            else
            {
                t1.Enabled = false;
                t1.Stop();
                btnStart.Text = "RF Berhenti!";
            }
        }

        void checkRFWindow()
        {
            try
            {
                int cb = int.Parse(cB.SelectedItem.ToString());
                /*RFWindowHandle = DllImports.FindWindow(cl, cl1);*/
                RFWindowHandle = Process.GetProcessById(cb).MainWindowHandle;
            }
            catch
            {
                RFWindowHandle = IntPtr.Zero;
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string message =
                "---RF Auto Loot---\n" +
                "Cara Pakai :\n" +
                "1. Pilih Targetnya\n" +
                "2. Set Waktunya Bebas\n" +
                "3. Klik Start\n" +
                "4. Pencet Tombol X 1x\n" +
                "5. Buka Kembali RF yg di Minimize\n" +
                "6. Minimize Kembali dengan meng-klik icon RF di TaskBar\n" +
                "7. Autoloot berjalan\n\n" +
                "Happy Farming!\n" +
                "Thanks to Gaara Hokage a.k.a Tommy Hawk";
            MessageBox.Show(message, "Informasi", MessageBoxButtons.OK);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            String cb = cB.Text;
            textBox1.Text = cb;
            checkRFWindow();
            if (RFWindowHandle != IntPtr.Zero)
            {
                DllImports.ShowWindow(RFWindowHandle, SW_SHOWMINIMIZED);
            }
            hook = Hook.GlobalEvents();
            hook.KeyDown += hook_KeyDown;
            t2.Enabled = true;
            t2.Start();
        }
    }
}

