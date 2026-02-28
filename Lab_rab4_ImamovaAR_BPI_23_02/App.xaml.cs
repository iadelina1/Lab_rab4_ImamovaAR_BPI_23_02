using System;
using System.Windows;

namespace Lab_rab4_ImamovaAR_BPI_23_02
{
    public partial class App : Application
    {
        public void ChangeTheme(string themeFile)
        {
            try
            {
                ResourceDictionary newTheme = new ResourceDictionary();
                newTheme.Source = new Uri(themeFile, UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при смене темы: {ex.Message}");
            }
        }
    }
}