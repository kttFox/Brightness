using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Brightness
{
    public partial class BrightnessForm : Form
    {
        internal static int? GetBrightness()
        {
            try
            {
                using (var s = new ManagementObjectSearcher(@"root\WMI", "SELECT CurrentBrightness FROM WmiMonitorBrightness"))
                    foreach (ManagementObject o in s.Get())
                        return Convert.ToInt32(o["CurrentBrightness"]);
            }
            catch { }
            return null;
        }

        static void SetBrightness(int level)
        {
            try
            {
                using (var s = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM WmiMonitorBrightnessMethods"))
                    foreach (ManagementObject o in s.Get())
                        o.InvokeMethod("WmiSetBrightness", new object[] { (uint)0, (byte)level });
            }
            catch { }
        }

        // デザイナー用
        public BrightnessForm() : this(100) { }

        public BrightnessForm(int current)
        {
            InitializeComponent();
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            HighlightButton(current);
            RestoreLocation();
        }

        // 前回のウィンドウ位置を %LOCALAPPDATA%\Brightness\location.txt に保存
        static readonly string LocationFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Brightness", "location.txt");

        void RestoreLocation()
        {
            try
            {
                var parts = File.ReadAllText(LocationFile).Split(',');
                var pt = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
                // 画面外(モニター構成の変更など)なら既定位置のまま
                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.WorkingArea.IntersectsWith(new Rectangle(pt, Size)))
                    {
                        Location = pt;
                        return;
                    }
                }
            }
            catch { }
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                var pt = WindowState == FormWindowState.Normal ? Location : RestoreBounds.Location;
                Directory.CreateDirectory(Path.GetDirectoryName(LocationFile)!);
                File.WriteAllText(LocationFile, pt.X + "," + pt.Y);
            }
            catch { }
        }

        // 現在の輝度に最も近いボタンを強調表示
        void HighlightButton(int level)
        {
            Button nearest = button0;
            foreach (var btn in new[] { button0, button25, button50, button75, button100 })
            {
                // 通常のボタンは標準の見た目に戻す
                btn.FlatStyle = FlatStyle.Standard;
                btn.BackColor = SystemColors.Control;
                btn.ForeColor = SystemColors.ControlText;
                btn.UseVisualStyleBackColor = true;
                if (Math.Abs(Convert.ToInt32(btn.Tag) - level) < Math.Abs(Convert.ToInt32(nearest.Tag) - level))
                    nearest = btn;
            }
            nearest.FlatStyle = FlatStyle.Flat;
            nearest.BackColor = SystemColors.Highlight;
            nearest.ForeColor = SystemColors.HighlightText;
        }

        // タイトルバーの右クリックメニュー(システムメニュー)に「最前面に表示」を追加
        const int WM_SYSCOMMAND = 0x0112;
        const int MF_STRING = 0x0000, MF_SEPARATOR = 0x0800, MF_CHECKED = 0x0008, MF_UNCHECKED = 0x0000;
        const int SC_TOPMOST = 0x0010; // 独自コマンドID (下位4ビットはシステム予約のため0にする)

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string? lpNewItem);
        [DllImport("user32.dll")]
        static extern int CheckMenuItem(IntPtr hMenu, int uIDCheckItem, int uCheck);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            IntPtr menu = GetSystemMenu(Handle, false);
            AppendMenu(menu, MF_SEPARATOR, 0, null);
            AppendMenu(menu, MF_STRING | (TopMost ? MF_CHECKED : MF_UNCHECKED), SC_TOPMOST, "最前面に表示");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND && ((int)m.WParam & 0xFFF0) == SC_TOPMOST)
            {
                TopMost = !TopMost;
                CheckMenuItem(GetSystemMenu(Handle, false), SC_TOPMOST, TopMost ? MF_CHECKED : MF_UNCHECKED);
                return;
            }
            base.WndProc(ref m);
        }

        // 各プリセットボタンの Tag に輝度値を設定
        void presetButton_Click(object sender, EventArgs e)
        {
            int level = Convert.ToInt32(((Button)sender).Tag);
            SetBrightness(level);
            HighlightButton(level);
        }
    }
}
