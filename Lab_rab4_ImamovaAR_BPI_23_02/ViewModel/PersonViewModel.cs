using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Lab_rab4_ImamovaAR_BPI_23_02.Model;
using Lab_rab4_ImamovaAR_BPI_23_02.Helper;
using Lab_rab4_ImamovaAR_BPI_23_02.View;

namespace Lab_rab4_ImamovaAR_BPI_23_02.ViewModel
{
    public class PersonViewModel
    {
        public ObservableCollection<PersonDPO> ListPersonDpo { get; set; }
        public PersonDPO SelectedPerson { get; set; }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public PersonViewModel()
        {
            ListPersonDpo = new ObservableCollection<PersonDPO>();
            ListPersonDpo.Add(new PersonDPO { Id = 1, FirstName = "Иван", LastName = "Иванов", Role = "Менеджер", Birthday = new DateTime(1990, 5, 10) });

            DeleteCommand = new RelayCommand(o => {
                if (SelectedPerson != null)
                {
                    ListPersonDpo.Remove(SelectedPerson);
                }
                else
                {
                    MessageBox.Show("Выберите сотрудника для удаления");
                }
            });

            AddCommand = new RelayCommand(o => {
                WindowNewEmployee win = new WindowNewEmployee();
                if (win.ShowDialog() == true)
                {
                    PersonDPO newPerson = new PersonDPO
                    {
                        Id = ListPersonDpo.Count + 1,
                        LastName = win.tbLastName.Text,
                        FirstName = win.tbFirstName.Text,
                        Birthday = win.dpBirthday.SelectedDate ?? DateTime.Now,
                        Role = "Менеджер"
                    };

                    ListPersonDpo.Add(newPerson);
                }
            });
            EditCommand = new RelayCommand(o => {
                if (SelectedPerson != null)
                {
                    WindowNewEmployee win = new WindowNewEmployee();

                    win.tbLastName.Text = SelectedPerson.LastName;
                    win.tbFirstName.Text = SelectedPerson.FirstName;
                    win.dpBirthday.SelectedDate = SelectedPerson.Birthday;

                    if (win.ShowDialog() == true)
                    {
                        PersonDPO tempPerson = new PersonDPO
                        {
                            Id = SelectedPerson.Id,
                            LastName = win.tbLastName.Text,
                            FirstName = win.tbFirstName.Text,
                            Birthday = win.dpBirthday.SelectedDate ?? DateTime.Now,
                            Role = SelectedPerson.Role
                        };

                        int index = ListPersonDpo.IndexOf(SelectedPerson);
                        ListPersonDpo[index] = tempPerson;
                    }
                }
                else
                {
                    MessageBox.Show("Выберите сотрудника для редактирования!");
                }
            });
        }
    }
}