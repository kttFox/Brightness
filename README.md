# Brightness

Surface などのノート PC 内蔵ディスプレイの輝度を、ワンクリックで切り替える Windows 用の小さなツールです。

![スクリーンショット](docs/screenshot.png)

## 機能

- 0% / 25% / 50% / 75% / 100% のボタンで輝度を切り替え
- 現在の輝度に最も近いボタンを強調表示
- タイトルバーの右クリックメニューから「最前面に表示」を ON/OFF

## 動作環境

- Windows 10 / 11
- [.NET 10 デスクトップ ランタイム](https://dotnet.microsoft.com/download/dotnet/10.0)
- WMI(`WmiMonitorBrightnessMethods`)で輝度を制御できるディスプレイ
  - ノート PC やタブレットの内蔵ディスプレイが対象です。  
  外部モニターでは多くの場合動作しません。
