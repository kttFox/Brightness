using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace Brightness
{
    public partial class MainWindow : Window
    {
        readonly WinForms.NotifyIcon notifyIcon;

        // デザイナー用
        public MainWindow() : this(100) { }

        public MainWindow(int current)
        {
            InitializeComponent();
            notifyIcon = CreateNotifyIcon();
            HighlightButton(current);
            RestoreSettings();
        }

        // 前回のウィンドウ位置・サイズと「固定する」「最前面に表示」の状態を %LOCALAPPDATA%\Brightness\location.txt に保存 (形式: X,Y,固定,最前面,幅,高さ)
        static readonly string SettingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Brightness", "location.txt");

        void RestoreSettings()
        {
            try
            {
                var parts = File.ReadAllText(SettingsFile).Split(',');
                if (parts.Length > 2)
                    SetLocked(parts[2].Trim() == "1");
                if (parts.Length > 3)
                    SetTopmost(parts[3].Trim() == "1");
                if (parts.Length > 5)
                {
                    Width = double.Parse(parts[4]);
                    Height = double.Parse(parts[5]);
                }
                var bounds = new Rect(double.Parse(parts[0]), double.Parse(parts[1]), Width, Height);
                // 画面外(モニター構成の変更など)なら中央に表示
                var screen = new Rect(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop,
                    SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
                if (screen.IntersectsWith(bounds))
                {
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    Left = bounds.X;
                    Top = bounds.Y;
                    return;
                }
            }
            catch { }
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        void SaveSettings()
        {
            try
            {
                var bounds = WindowState == WindowState.Normal ? new Rect(Left, Top, Width, Height) : RestoreBounds;
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile)!);
                File.WriteAllText(SettingsFile, string.Join(",",
                    (int)bounds.X, (int)bounds.Y, lockMenuItem.IsChecked ? 1 : 0, Topmost ? 1 : 0, (int)bounds.Width, (int)bounds.Height));
            }
            catch { }
        }

        // タスクトレイ: 閉じても終了せず隠すだけ。メニューの「終了」で終了する
        bool exiting;

        WinForms.NotifyIcon CreateNotifyIcon()
        {
            var menu = new WinForms.ContextMenuStrip();
            var show = new WinForms.ToolStripMenuItem("表示", null, (s, e) => ShowWindow());
            show.Font = new System.Drawing.Font(show.Font, System.Drawing.FontStyle.Bold);
            menu.Items.Add(show);
            menu.Items.Add(new WinForms.ToolStripMenuItem("終了", null, (s, e) => Exit()));

            System.Drawing.Icon? icon = null;
            try { icon = System.Drawing.Icon.ExtractAssociatedIcon(Environment.ProcessPath!); } catch { }

            var tray = new WinForms.NotifyIcon
            {
                Text = "Brightness",
                Icon = icon ?? System.Drawing.SystemIcons.Application,
                ContextMenuStrip = menu,
                Visible = true,
            };
            tray.MouseClick += (s, e) =>
            {
                if (e.Button != WinForms.MouseButtons.Left) return;
                if (IsVisible) Hide(); else ShowWindow();
            };
            return tray;
        }

        void ShowWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        void Exit()
        {
            exiting = true;
            Close();
        }

        void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!exiting)
            {
                e.Cancel = true;
                Hide();
                return;
            }
            base.OnClosing(e);
            SaveSettings();
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        // 右クリックメニュー
        void topMostMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SetTopmost(topMostMenuItem.IsChecked);
        }

        void lockMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SetLocked(lockMenuItem.IsChecked);
        }

        void SetTopmost(bool value)
        {
            Topmost = value;
            topMostMenuItem.IsChecked = value;
        }

        // 固定中は移動もサイズ変更もしない
        void SetLocked(bool value)
        {
            lockMenuItem.IsChecked = value;
            ResizeMode = value ? ResizeMode.NoResize : ResizeMode.CanResize;
        }

        // ボタン上のドラッグでウィンドウを移動 (少し動かしただけではクリック扱い)
        Point dragStart;

        void buttonGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragStart = e.GetPosition(this);
        }

        void buttonGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || lockMenuItem.IsChecked) return;
            var delta = e.GetPosition(this) - dragStart;
            if (Math.Abs(delta.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(delta.Y) < SystemParameters.MinimumVerticalDragDistance) return;
            // ボタンのマウスキャプチャを外してクリックを取り消してから移動
            Mouse.Capture(null);
            // タッチ操作などで左ボタンが離れていると DragMove が例外を出すため無視する
            try { DragMove(); } catch (InvalidOperationException) { }
        }

        // 各プリセットボタンの Tag に輝度値を設定
        void presetButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not Button button) return;
            int level = Convert.ToInt32(button.Tag);
            DisplayBrightness.Set(level);
            HighlightButton(level);
        }

        // 現在の輝度と一致するボタンを強調表示
        void HighlightButton(int level)
        {
            foreach (Button button in buttonGrid.Children)
            {
                if (Convert.ToInt32(button.Tag) == level)
                {
                    button.Background = SystemColors.HighlightBrush;
                    button.Foreground = SystemColors.HighlightTextBrush;
                }
                else
                {
                    button.ClearValue(BackgroundProperty);
                    button.ClearValue(ForegroundProperty);
                }
            }
        }
    }
}
