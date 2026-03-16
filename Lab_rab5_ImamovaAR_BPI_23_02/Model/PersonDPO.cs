using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Lab_rab5_ImamovaAR_BPI_23_02.Model
{
    public class PersonDPO : INotifyPropertyChanged
        {
            private int _id;
            private string _lastName;
            private string _firstName;
            private DateTime _birthday;
            private string _role;

            public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
            public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
            public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
            public DateTime Birthday { get => _birthday; set { _birthday = value; OnPropertyChanged(); } }
            public string Role { get => _role; set { _role = value; OnPropertyChanged(); } }

            public string Error => null;
            public string this[string columnName]
            {
                get
                {
                    string error = string.Empty;
                    switch (columnName)
                    {
                        case "LastName":
                            if (string.IsNullOrWhiteSpace(LastName))
                                error = "Введите фамилию";
                            else if (!Regex.IsMatch(LastName, @"^[a-zA-Zа-яА-ЯёЁ]+$"))
                                error = "Только буквы";
                            break;
                        case "FirstName":
                            if (string.IsNullOrWhiteSpace(FirstName))
                                error = "Введите имя";
                            else if (!Regex.IsMatch(FirstName, @"^[a-zA-Zа-яА-ЯёЁ]+$"))
                                error = "Только буквы";
                            break;
                    }
                    return error;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
