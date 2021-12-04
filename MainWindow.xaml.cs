using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EasyAdd
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void toolBar_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey registryKey1;

            ColorAnimation colorAnim = new ColorAnimation();
            colorAnim.From = Colors.White;
            colorAnim.To = Colors.OrangeRed;
            colorAnim.Duration = new Duration(TimeSpan.FromSeconds(1));
            colorAnim.AutoReverse = true;
            colorAnim.RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(30));

            string pathToExe = PathToEXE.Text.Trim().Replace('\\', '/');
            string nameInContent = NameInContext.Text.Trim();

            if (PathToEXE.Text.Length <= 0 || pathToExe.IndexOf(".exe") == -1) {
                PathToEXE.BorderBrush = new SolidColorBrush(Colors.White);
                PathToEXE.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
                return;
            }
            if (NameInContext.Text.Length <= 0)
            {
                
                NameInContext.BorderBrush = new SolidColorBrush(Colors.White);
                NameInContext.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
                return;
            }



            string mode;

            if ((string)((ComboBoxItem)ComboBox1.SelectedItem).Content == "For files")
                mode = "*";
            else
                mode = "Directory";

            
            registryKey1 = Registry.ClassesRoot.OpenSubKey(mode, true).OpenSubKey("shell", true);
            RegistryKey EasyAdd = registryKey1.CreateSubKey(nameInContent, true);
            EasyAdd.SetValue(null, $"Open with {nameInContent}");
            RegistryKey command1 = EasyAdd.CreateSubKey("command", true);
            command1.SetValue(null, pathToExe);
            EasyAdd.SetValue("icon", pathToExe);

            SucMessage.Visibility = Visibility.Visible;
            DispatcherTimer dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
            dispatcherTimer.Start();
            dispatcherTimer.Tick += new EventHandler((object c, EventArgs eventArgs) =>
            {
                SucMessage.Visibility = Visibility.Hidden;
                ((DispatcherTimer)c).Stop();
            });
        }

    }
}
