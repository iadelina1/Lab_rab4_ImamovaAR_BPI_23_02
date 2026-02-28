using Lab_rab4_ImamovaAR_BPI_23_02.Helper;
using Lab_rab4_ImamovaAR_BPI_23_02.Model;
using Lab_rab4_ImamovaAR_BPI_23_02.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Lab_rab4_ImamovaAR_BPI_23_02.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Role> ListRole { get; set; }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged("SelectedRole");
            }
        }

        public RoleViewModel()
        {
            ListRole = new ObservableCollection<Role>
            {
                new Role { Id = 1, NameRole = "Директор" },
                new Role { Id = 2, NameRole = "Менеджер" },
                new Role { Id = 3, NameRole = "Программист" }
            };
        }

        public string NewRoleName { get; set; }

        private RelayCommand _addRole;
        public RelayCommand AddRole => _addRole ?? (_addRole = new RelayCommand(obj =>
        {
            NewRoleName = "";
            WindowNewRole win = new WindowNewRole();
            win.DataContext = this;

            if (win.ShowDialog() == true)
            {
                ListRole.Add(new Role
                {
                    Id = ListRole.Count + 1,
                    NameRole = NewRoleName
                });
            }
        }));

        private RelayCommand _editRole;
        public RelayCommand EditRole => _editRole ?? (_editRole = new RelayCommand(obj =>
        {
            NewRoleName = SelectedRole.NameRole;

            WindowNewRole win = new WindowNewRole();
            win.DataContext = this;

            if (win.ShowDialog() == true)
            {
                int index = ListRole.IndexOf(SelectedRole);
                ListRole[index] = new Role
                {
                    Id = SelectedRole.Id,
                    NameRole = NewRoleName
                };
            }
        }, (obj) => SelectedRole != null));

        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole => _deleteRole ?? (_deleteRole = new RelayCommand(obj =>
        {
            ListRole.Remove(SelectedRole);
        }, (obj) => SelectedRole != null));
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}