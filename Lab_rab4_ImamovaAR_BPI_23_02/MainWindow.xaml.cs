using Lab_rab5_ImamovaAR_BPI_23_02.View;
using System.Windows;
using System.Windows.Controls;

namespace Lab_rab4_ImamovaAR_BPI_23_02
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenEmployee_Click(object sender, RoutedEventArgs e)
        {
            WindowEmployee win = new WindowEmployee();
            win.Show();
        }
        private void OpenRole_Click(object sender, RoutedEventArgs e)
        {
            WindowRole win = new WindowRole();
            win.Show();
        }
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string themeFile = btn.Tag.ToString();
                ((App)Application.Current).ChangeTheme(themeFile);
            }
        }
    }
}