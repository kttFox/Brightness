// Surface 画面輝度調整ツール (WMI: WmiMonitorBrightnessMethods を使用)
using System;
using System.Management;

namespace Brightness
{
    static class DisplayBrightness
    {
        public static int? Get()
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

        public static void Set(int level)
        {
            try
            {
                using (var s = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM WmiMonitorBrightnessMethods"))
                    foreach (ManagementObject o in s.Get())
                        o.InvokeMethod("WmiSetBrightness", new object[] { (uint)0, (byte)level });
            }
            catch { }
        }
    }
}
