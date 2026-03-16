using Lab_rab5_ImamovaAR_BPI_23_02.View;
using Lab_rab5_ImamovaAR_BPI_23_02.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Lab_rab5_ImamovaAR_BPI_23_02
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = PersonViewModel.Instance;
        }
        
    }
}