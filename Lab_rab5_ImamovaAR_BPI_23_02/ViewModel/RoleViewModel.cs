using Lab_rab5_ImamovaAR_BPI_23_02.Helper;
using Lab_rab5_ImamovaAR_BPI_23_02.Model;
using Lab_rab5_ImamovaAR_BPI_23_02.View;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

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
                OnPropertyChanged();
            }
        }

        private int _newRoleId;
        public int NewRoleId
        {
            get => _newRoleId;
            set { _newRoleId = value; OnPropertyChanged(); }
        }

        private string _newRoleName;
        public string NewRoleName
        {
            get => _newRoleName;
            set
            {
                string filtered = Regex.Replace(value ?? "", @"[^a-zA-Zа-яА-ЯёЁ ]", "");

                if (_newRoleName != filtered)
                {
                    _newRoleName = filtered;
                    OnPropertyChanged();
                }
            }
        }

        private string actualPath => GetSafePath("RoleData.json");

        public RoleViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            ListRole = new ObservableCollection<Role>();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(actualPath))
                {
                    string json = File.ReadAllText(actualPath);
                    var roles = JsonConvert.DeserializeObject<ObservableCollection<Role>>(json);

                    ListRole.Clear();
                    if (roles != null)
                    {
                        foreach (var r in roles)
                            ListRole.Add(r);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке ролей: " + ex.Message);
            }
        }

        private void Save()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(actualPath);
                if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();

                string json = JsonConvert.SerializeObject(ListRole, Formatting.Indented);
                File.WriteAllText(actualPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении ролей: " + ex.Message);
            }
        }

        private RelayCommand _addRole;
        public RelayCommand AddRole => _addRole ?? (_addRole = new RelayCommand(obj =>
        {
            NewRoleId = ListRole.Count > 0 ? ListRole.Max(r => r.Id) + 1 : 1;
            NewRoleName = "";

            WindowNewRole win = new WindowNewRole { DataContext = this };
            if (win.ShowDialog() == true)
            {
                ListRole.Add(new Role { Id = NewRoleId, NameRole = NewRoleName });
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

        private RelayCommand _acceptCommand;
        public RelayCommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(obj =>
        {
            if (obj is Window window)
            {
                window.DialogResult = true;
            }
        }));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private static string GetSafePath(string fileName)
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(currentDir);

            for (int i = 0; i < 5; i++)
            {
                if (directory == null) break;
                string potentialPath = Path.Combine(directory.FullName, "DataModels", fileName);
                if (File.Exists(potentialPath)) return potentialPath;
                directory = directory.Parent;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataModels", fileName);
        }
    }
}