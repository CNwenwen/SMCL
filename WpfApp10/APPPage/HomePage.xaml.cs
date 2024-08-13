using iNKORE.UI.WPF.Modern.Controls;
using StarLight_Core.Authentication;
using StarLight_Core.Enum;
using StarLight_Core.Launch;
using StarLight_Core.Models.Authentication;
using StarLight_Core.Models.Launch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
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
using WpfApp10.APPPage;

namespace WpfApp10.Page
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            VVV.Value = 0;
            BaseAccount account;
            if (SettingsPage.LoginMode == 0)
            {
                var auth = new MicrosoftAuthentication("e1e383f9-59d9-4aa2-bf5e-73fe83b15ba0");
                var code = await auth.RetrieveDeviceCodeInfo();
                Clipboard.Clear();
                Clipboard.SetText(code.UserCode);
                iNKORE.UI.WPF.Modern.Controls.MessageBox.Show("请在稍后的输入框粘贴此串代码：" + code.UserCode, "MSA验证");
                Process.Start(new ProcessStartInfo(code.VerificationUri)
                {
                    UseShellExecute = true,
                    Verb = "open"
                });
                var token = await auth.GetTokenResponse(code);
                account = await auth.MicrosoftAuthAsync(token, x =>
                {
                    Progress.Content = x;
                });
            }
            else
            {
                account = new OfflineAuthentication(SettingsPage.PlayerName).OfflineAuth();
            }
            LaunchConfig args = new() // 配置启动参数
            {
                Account = new()
                {
                    BaseAccount = account // 账户
                },
                GameCoreConfig = new()
                {
                    Root = ".minecraft", // 游戏根目录(可以是绝对的也可以是相对的,自动判断)
                    Version = SettingsPage.GameVersion, // 启动的版本
                    IsVersionIsolation = true, //版本隔离
                    //Nide8authPath = ".minecraft\\nide8auth.jar", // 只有统一通行证需要
                    //UnifiedPassServerId = "xxxxxxxxxxxxxxxxxx" // 同上
                },
                JavaConfig = new()
                {
                    JavaPath = SettingsPage.JavaPath, // Java 路径(绝对路径)
                    MaxMemory = 16384,
                    MinMemory = 1000
                }
            };
            var launch = new MinecraftLauncher(args); // 实例化启动器
            var la = await launch.LaunchAsync(ReportProgress); // 启动

            // 日志输出
            la.ErrorReceived += (output) => Console.WriteLine($"{output}");
            la.OutputReceived += (output) => Console.WriteLine($"{output}");

            if (la.Status == Status.Succeeded)
            {
                Progress.Content = "启动成功";
            }
            else
            {
                iNKORE.UI.WPF.Modern.Controls.MessageBox.Show("游戏启动时崩溃。请完整截图本窗口和日志，以下为日志：" + la.Exception.ToString() ,"啊哦，错误啦...");
            }
        }

        private void ReportProgress(ProgressReport report)
        {
            Progress.Content = report.Description;
            VVV.Value = report.Percentage;
        }
    }
}
