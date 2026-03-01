using Lab_rab5_ImamovaAR_BPI_23_02.Helper;
using Lab_rab5_ImamovaAR_BPI_23_02.Model;
using Lab_rab5_ImamovaAR_BPI_23_02.View;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using System.Linq;

namespace Lab_rab5_ImamovaAR_BPI_23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PersonDPO> ListPersonDPO { get; set; }
        public ObservableCollection<Role> ListRole { get; set; }

        private PersonDPO _selectedPersonDPO;
        public PersonDPO SelectedPersonDPO
        {
            get => _selectedPersonDPO;
            set
            {
                _selectedPersonDPO = value;
                OnPropertyChanged("SelectedPersonDPO");
            }
        }

        public int NewId { get; set; }
        public string NewLastName { get; set; }
        public string NewFirstName { get; set; }
        public DateTime NewBirthday { get; set; } = DateTime.Now;
        public Role NewRole { get; set; }

        string pathP = @"DataModels\PersonData.json";
        string pathR = @"DataModels\RoleData.json";

        public PersonViewModel()
        {
            ListPersonDPO = new ObservableCollection<PersonDPO>();
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(pathP) && File.Exists(pathR))
            {
                var persons = JsonConvert.DeserializeObject<ObservableCollection<Person>>(File.ReadAllText(pathP));
                var roles = JsonConvert.DeserializeObject<ObservableCollection<Role>>(File.ReadAllText(pathR));
                ListRole = roles;

                foreach (var p in persons)
                {
                    var roleName = roles.FirstOrDefault(r => r.Id == p.RoleId)?.NameRole ?? "Не задано";
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

        private void Save()
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

            string json = JsonConvert.SerializeObject(personsToSave, Formatting.Indented);
            File.WriteAllText(pathP, json);
        }

        private RelayCommand _addPerson;
        public RelayCommand AddPerson => _addPerson ?? (_addPerson = new RelayCommand(obj =>
        {
            NewId = ListPersonDPO.Count > 0 ? ListPersonDPO.Max(p => p.Id) + 1 : 1;
            NewLastName = "";
            NewFirstName = "";
            NewBirthday = DateTime.Now;
            NewRole = null;

            WindowNewEmployee win = new WindowNewEmployee();
            win.DataContext = this;

            if (win.ShowDialog() == true)
            {
                PersonDPO newPerson = new PersonDPO
                {
                    Id = NewId,
                    LastName = NewLastName,
                    FirstName = NewFirstName,
                    Birthday = NewBirthday,
                    Role = NewRole?.NameRole ?? "Не задано"
                };

                ListPersonDPO.Add(newPerson);
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

            WindowNewEmployee win = new WindowNewEmployee();
            win.DataContext = this;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}