using Lab_rab5_ImamovaAR_BPI_23_02.Helper;
using Lab_rab5_ImamovaAR_BPI_23_02.Model;
using Lab_rab5_ImamovaAR_BPI_23_02.View;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq;

namespace Lab_rab5_ImamovaAR_BPI_23_02.ViewModel
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
        public int NewRoleId { get; set; }
        public string NewRoleName { get; set; }

        string path = @"DataModels\RoleData.json";

        public RoleViewModel()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>>(json);
            }
            else
            {
                ListRole = new ObservableCollection<Role>();
            }
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(ListRole, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private RelayCommand _addRole;
        public RelayCommand AddRole => _addRole ?? (_addRole = new RelayCommand(obj =>
        {
            NewRoleId = ListRole.Count > 0 ? ListRole.Max(r => r.Id) + 1 : 1;
            NewRoleName = "";

            WindowNewRole win = new WindowNewRole { DataContext = this };
            if (win.ShowDialog() == true)
            {
                Role role = new Role { Id = NewRoleId, NameRole = NewRoleName };
                ListRole.Add(role);
                Save();
            }
        }));

        private RelayCommand _editRole;
        public RelayCommand EditRole => _editRole ?? (_editRole = new RelayCommand(obj =>
        {
            NewRoleId = SelectedRole.Id;
            NewRoleName = SelectedRole.NameRole;

            WindowNewRole win = new WindowNewRole { DataContext = this };
            if (win.ShowDialog() == true)
            {
                int index = ListRole.IndexOf(SelectedRole);
                ListRole[index] = new Role { Id = NewRoleId, NameRole = NewRoleName };
                Save();
            }
        }, (obj) => SelectedRole != null));

        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole => _deleteRole ?? (_deleteRole = new RelayCommand(obj =>
        {
            ListRole.Remove(SelectedRole);
            Save();
        }, (obj) => SelectedRole != null));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}