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
using System.Windows.Input;

namespace Lab_rab5_ImamovaAR_BPI_23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private static PersonViewModel _instance;
        public static PersonViewModel Instance => _instance ?? (_instance = new PersonViewModel());

        private readonly string _pathP;
        private readonly string _pathR;

        public ObservableCollection<PersonDPO> ListPersonDPO { get; set; }
        public ObservableCollection<Role> ListRole { get; set; }

        private PersonDPO _selectedPersonDPO;
        public PersonDPO SelectedPersonDPO
        {
            get => _selectedPersonDPO;
            set { _selectedPersonDPO = value; OnPropertyChanged(); }
        }
        private string _newLastName;
        public string NewLastName
        {
            get => _newLastName;
            set
            {
                _newLastName = Regex.Replace(value ?? "", @"[^a-zA-Zа-яА-ЯёЁ]", "");
                OnPropertyChanged();
            }
        }

        private string _newFirstName;
        public string NewFirstName
        {
            get => _newFirstName;
            set
            {
                _newFirstName = Regex.Replace(value ?? "", @"[^a-zA-Zа-яА-ЯёЁ]", "");
                OnPropertyChanged();
            }
        }

        private Role _newRole;
        public Role NewRole
        {
            get => _newRole;
            set { _newRole = value; OnPropertyChanged(); }
        }

        public int NewId { get; set; }
        public DateTime NewBirthday { get; set; } = DateTime.Now;

        public PersonViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            ListPersonDPO = new ObservableCollection<PersonDPO>();
            ListRole = new ObservableCollection<Role>();

            _pathP = GetSafePath("PersonData.json");
            _pathR = GetSafePath("RoleData.json");

            LoadData();
        }


        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(NewLastName) &&
                   !string.IsNullOrWhiteSpace(NewFirstName) &&
                   NewRole != null;
        }

        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-ЯёЁ]+$");
        }
        private void LoadData()
        {
            try
            {
                if (File.Exists(_pathR))
                {
                    var roles = JsonConvert.DeserializeObject<ObservableCollection<Role>>(File.ReadAllText(_pathR));
                    ListRole.Clear();
                    if (roles != null) foreach (var r in roles) ListRole.Add(r);
                }

                if (File.Exists(_pathP))
                {
                    var persons = JsonConvert.DeserializeObject<ObservableCollection<Person>>(File.ReadAllText(_pathP));
                    ListPersonDPO.Clear();
                    if (persons != null)
                    {
                        foreach (var p in persons)
                        {
                            var roleName = ListRole.FirstOrDefault(r => r.Id == p.RoleId)?.NameRole ?? "Не задано";
                            ListPersonDPO.Add(new PersonDPO
                            {
                                Id = p.Id,
                                LastName = p.LastName,
                                FirstName = p.FirstName,
                                Birthday = p.Birthday,
                                Role = roleName
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка загрузки: " + ex.Message); }
        }

        private void Save()
        {
            try
            {
                var personsToSave = new ObservableCollection<Person>();
                foreach (var dpo in ListPersonDPO)
                {
                    personsToSave.Add(new Person
                    {
                        Id = dpo.Id,
                        LastName = dpo.LastName,
                        FirstName = dpo.FirstName,
                        Birthday = dpo.Birthday,
                        RoleId = ListRole.FirstOrDefault(r => r.NameRole == dpo.Role)?.Id ?? 0
                    });
                }
                File.WriteAllText(_pathP, JsonConvert.SerializeObject(personsToSave, Formatting.Indented));
            }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения: " + ex.Message); }
        }

        private RelayCommand _acceptCommand;
        public RelayCommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(obj =>
        {
            if (obj is Window window) window.DialogResult = true;
        }, (obj) => IsValid()));

        private RelayCommand _addPerson;
        public RelayCommand AddPerson => _addPerson ?? (_addPerson = new RelayCommand(obj =>
        {
            NewId = ListPersonDPO.Count > 0 ? ListPersonDPO.Max(p => p.Id) + 1 : 1;
            NewLastName = ""; NewFirstName = ""; NewBirthday = DateTime.Now; NewRole = null;

            WindowNewEmployee win = new WindowNewEmployee { DataContext = this };
            if (win.ShowDialog() == true)
            {
                ListPersonDPO.Add(new PersonDPO
                {
                    Id = NewId,
                    LastName = NewLastName,
                    FirstName = NewFirstName,
                    Birthday = NewBirthday,
                    Role = NewRole?.NameRole ?? "Не задано"
                });
                Save();
            }
        }));

        private RelayCommand _editPerson;
        public RelayCommand EditPerson => _editPerson ?? (_editPerson = new RelayCommand(obj =>
        {
            NewId = SelectedPersonDPO.Id;
            NewLastName = SelectedPersonDPO.LastName; 
            NewFirstName = SelectedPersonDPO.FirstName;
            NewBirthday = SelectedPersonDPO.Birthday;
            NewRole = ListRole.FirstOrDefault(r => r.NameRole == SelectedPersonDPO.Role);

            WindowNewEmployee win = new WindowNewEmployee { DataContext = this };

            if (win.ShowDialog() == true)
            {
                int index = ListPersonDPO.IndexOf(SelectedPersonDPO);
                ListPersonDPO[index] = new PersonDPO
                {
                    Id = NewId,
                    LastName = NewLastName,
                    FirstName = NewFirstName,
                    Birthday = NewBirthday,
                    Role = NewRole?.NameRole ?? "Не задано"
                };
                Save();
            }
        }, (obj) => SelectedPersonDPO != null));
        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson => _deletePerson ?? (_deletePerson = new RelayCommand(obj =>
        {
            ListPersonDPO.Remove(SelectedPersonDPO);
            Save();
        }, (obj) => SelectedPersonDPO != null));

        private RelayCommand _openEmployee;
        public RelayCommand OpenEmployeeCommand => _openEmployee ?? (_openEmployee = new RelayCommand(obj => { new WindowEmployee().Show(); }));

        private RelayCommand _openRole;
        public RelayCommand OpenRoleCommand => _openRole ?? (_openRole = new RelayCommand(obj => {
            WindowRole win = new WindowRole();
            win.ShowDialog();
            LoadData();
        }));

        private RelayCommand _changeTheme;
        public RelayCommand ChangeThemeCommand => _changeTheme ?? (_changeTheme = new RelayCommand(obj => {
            if (obj is string themeFile) ((App)Application.Current).ChangeTheme(themeFile);
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