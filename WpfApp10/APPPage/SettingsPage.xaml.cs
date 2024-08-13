using StarLight_Core.Utilities;
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
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage
    {
        public static string GameVersion { get; set; }
        public static string JavaPath { get; set; }
        public static int LoginMode { get; set; }
        public static string PlayerName { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            GetGameVer();
            GetJavaPath();
            LoginModeBox.SelectedIndex = 0;
        }
        void GetGameVer()
        {
            GameVersionBox.DisplayMemberPath = "Id";
            GameVersionBox.SelectedValuePath = "Id";
            GameVersionBox.ItemsSource = GameCoreUtil.GetGameCores();
        }

        public class Item
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        async Task GetJavaPath()
        {
            List<Item> items = new List<Item>();
            foreach (var item in JavaUtil.GetJavas())
            {
                items.Add(new Item
                {
                    Name = $"Java {item.JavaVersion.Substring(0,6)} 路径: {item.JavaLibraryPath}",
                    Path = item.JavaPath
                });
            }
            JavaPathBox.DisplayMemberPath = "Name";
            JavaPathBox.SelectedValuePath = "Path";
            JavaPathBox.ItemsSource = items;
        }
        private void LoginModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoginMode = LoginModeBox.SelectedIndex;
            if (LoginModeBox.SelectedIndex == 0)
            {
                UserNameBoxText.Visibility = Visibility.Hidden;
                UserNameBox.Visibility = Visibility.Hidden;
            }
            else
            {
                UserNameBoxText.Visibility = Visibility.Visible;
                UserNameBox.Visibility = Visibility.Visible;
            }
        }

        private void GameVersionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GameVersion = GameVersionBox.SelectedItem.ToString();
        }

        private void JavaPathBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JavaPath = JavaPathBox.SelectedItem.ToString();
        }

        private void UserNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlayerName = UserNameBox.Text;
        }
    }
}
