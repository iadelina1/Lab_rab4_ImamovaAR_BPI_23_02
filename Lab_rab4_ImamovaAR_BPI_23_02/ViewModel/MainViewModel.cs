using System.Windows.Input;
using Lab_rab4_ImamovaAR_BPI_23_02.Helper;
using Lab_rab4_ImamovaAR_BPI_23_02.View;

namespace Lab_rab4_ImamovaAR_BPI_23_02.ViewModel
{
    public class MainViewModel
    {
        public ICommand OpenEmployeeWindowCommand { get; }
        public ICommand OpenRoleWindowCommand { get; }

        public MainViewModel()
        {
            OpenEmployeeWindowCommand = new RelayCommand(o => {
                new WindowEmployee().Show();
            });

            OpenRoleWindowCommand = new RelayCommand(o => {
                new WindowRole().Show();
            });
        }
    }
}