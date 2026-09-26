using System;
using System.IO;
using System.Windows;

namespace Brightness
{
    public partial class App : Application
    {
        // 予期しない例外は実行ファイルと同じフォルダーの error.log に記録する
        static readonly string ErrorLogFile = Path.Combine(AppContext.BaseDirectory, "error.log");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += (s, args) =>
            {
                WriteErrorLog(args.Exception);
                MessageBox.Show(args.Exception.Message + "\n\n詳細: " + ErrorLogFile, "Brightness - エラー");
                args.Handled = true;
            };
            AppDomain.CurrentDomain.UnhandledException += (s, args) => WriteErrorLog(args.ExceptionObject);

            int? current = DisplayBrightness.Get();
            if (current == null)
            {
                MessageBox.Show("このディスプレイは輝度制御(WMI)に対応していません。\n外部モニターでは動作しない場合があります。", "Brightness");
                //Shutdown(); return;
            }
            new MainWindow(current ?? 100).Show();
        }

        static void WriteErrorLog(object exception)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ErrorLogFile)!);
                File.AppendAllText(ErrorLogFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{exception}\n\n");
            }
            catch { }
        }
    }
}
