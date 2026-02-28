using Lab_rab4_ImamovaAR_BPI_23_02.Helper;
using Lab_rab4_ImamovaAR_BPI_23_02.Model;
using Lab_rab4_ImamovaAR_BPI_23_02.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Lab_rab4_ImamovaAR_BPI_23_02.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PersonDPO> ListPersonDPO { get; set; }

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
        public string NewLastName { get; set; }
        public string NewFirstName { get; set; }
        public DateTime NewBirthday { get; set; } = DateTime.Now;
        
        

        public PersonViewModel()
        {
            ListPersonDPO = new ObservableCollection<PersonDPO>();
            ListPersonDPO.Add(new PersonDPO { Id = 1, LastName = "Иванов", FirstName = "Иван", Role = "Директор" });
            ListPersonDPO.Add(new PersonDPO { Id = 2, LastName = "Петров", FirstName = "Пётр", Role = "Менеджер" });
        }

        private RelayCommand _addPerson;
        public RelayCommand AddPerson => _addPerson ?? (_addPerson = new RelayCommand(obj =>
        {
            NewLastName = ""; NewFirstName = ""; NewBirthday = DateTime.Now;

            WindowNewEmployee win = new WindowNewEmployee();
            win.DataContext = this;

            if (win.ShowDialog() == true)
            {
                PersonDPO newPerson = new PersonDPO
                {
                    Id = ListPersonDPO.Count + 1,
                    LastName = NewLastName,
                    FirstName = NewFirstName,
                    Birthday = NewBirthday,
                    Role = "Не назначена"
                };

                ListPersonDPO.Add(newPerson);
            }
        }));

        private RelayCommand _editPerson;
        public RelayCommand EditPerson => _editPerson ?? (_editPerson = new RelayCommand(obj =>
        {
            NewLastName = SelectedPersonDPO.LastName;
            NewFirstName = SelectedPersonDPO.FirstName;
            NewBirthday = SelectedPersonDPO.Birthday;

            WindowNewEmployee win = new WindowNewEmployee();
            win.DataContext = this;

            if (win.ShowDialog() == true)
            {
                int index = ListPersonDPO.IndexOf(SelectedPersonDPO);
                ListPersonDPO[index] = new PersonDPO
                {
                    Id = SelectedPersonDPO.Id,
                    LastName = NewLastName,
                    FirstName = NewFirstName,
                    Birthday = NewBirthday,
                    Role = SelectedPersonDPO.Role
                };
            }
        }, (obj) => SelectedPersonDPO != null));

        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson => _deletePerson ?? (_deletePerson = new RelayCommand(obj =>
        {
            ListPersonDPO.Remove(SelectedPersonDPO);
        }, (obj) => SelectedPersonDPO != null));
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}