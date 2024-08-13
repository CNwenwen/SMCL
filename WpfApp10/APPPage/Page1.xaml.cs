using iNKORE.UI.WPF.Modern.Controls;
using StarLight_Core.Installer;
using StarLight_Core.Models.Downloader;
using StarLight_Core.Models.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp10.APPPage
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1
    {
        public Page1()
        {
            InitializeComponent();
            DownloaderConfig.MaxThreads = 64;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            VVV.Value = 0;
            MinecraftInstaller installer = new MinecraftInstaller(VersionName.Text, ".minecraft");
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken cancellationToken = cts.Token;
            installer.OnProgressChanged += (status, progress) =>
            {
                if (status.Contains("失败"))
                {
                    iNKORE.UI.WPF.Modern.Controls.MessageBox.Show("下载错误。请完整截图本窗口和日志，以下为日志：" + status,"啊哦，错误啦...");
                }
                VVV.Value = progress;
            };
            
            await installer.InstallAsync(VersionName.Text, true, cancellationToken);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(idkBox.SelectedIndex == 1)
            {
                DownloadAPIs.SwitchDownloadSource(StarLight_Core.Enum.DownloadSource.Mojang);
            }
            else
            {
                DownloadAPIs.SwitchDownloadSource(StarLight_Core.Enum.DownloadSource.BmclApi);
            }
        }
    }
}
