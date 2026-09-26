// Surface 画面輝度調整ツール (WMI: WmiMonitorBrightnessMethods を使用)
using System;
using System.Windows.Forms;

namespace Brightness
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int? current = BrightnessForm.GetBrightness();
            if (current == null)
            {
                MessageBox.Show("このディスプレイは輝度制御(WMI)に対応していません。\n外部モニターでは動作しない場合があります。", "画面輝度");
                //return;
            }
            Application.Run(new BrightnessForm(current ?? 100));
        }
    }
}
