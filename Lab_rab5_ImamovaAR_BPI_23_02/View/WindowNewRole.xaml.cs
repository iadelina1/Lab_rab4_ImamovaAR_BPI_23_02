using Lab_rab5_ImamovaAR_BPI_23_02.ViewModel;
using System.Windows;

namespace Lab_rab5_ImamovaAR_BPI_23_02.View
{
    public partial class WindowNewRole : Window
    {
        public WindowNewRole()
        {
            InitializeComponent();
            this.DataContext = new RoleViewModel();
        }
    }
}