using Lab_rab4_ImamovaAR_BPI_23_02.Helper;
using Lab_rab4_ImamovaAR_BPI_23_02.Model;
using Lab_rab4_ImamovaAR_BPI_23_02.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Lab_rab4_ImamovaAR_BPI_23_02.ViewModel
{
    public class RoleViewModel
    {
        public ObservableCollection<Role> ListRole { get; set; }
        public Role SelectedRole { get; set; }

        public ICommand AddRoleCommand { get; }
        public ICommand EditRoleCommand { get; }
        public ICommand DeleteRoleCommand { get; }

        public RoleViewModel()
        {
            ListRole = new ObservableCollection<Role>();
            ListRole.Add(new Role(1, "Директор"));
            ListRole.Add(new Role(2, "Бухгалтер"));

            DeleteRoleCommand = new RelayCommand(o =>
            {
                if (SelectedRole != null)
                {
                    ListRole.Remove(SelectedRole);
                }
                else
                {
                    MessageBox.Show("Выберите должность для удаления");
                }
            });

            AddRoleCommand = new RelayCommand(o =>
            {
                WindowNewRole win = new WindowNewRole();
                win.Title = "Новая должность";

                if (win.ShowDialog() == true)
                {
                    int newId = ListRole.Count > 0 ? ListRole[ListRole.Count - 1].Id + 1 : 1;
                    ListRole.Add(new Role(newId, win.tbRoleName.Text));
                }
            });

            EditRoleCommand = new RelayCommand(o => {
                if (SelectedRole != null)
                {
                    WindowNewRole win = new WindowNewRole();
                    win.tbRoleName.Text = SelectedRole.NameRole;

                    if (win.ShowDialog() == true)
                    {
                        int index = ListRole.IndexOf(SelectedRole);
                        ListRole[index] = new Role(SelectedRole.Id, win.tbRoleName.Text);
                    }
                }

                else
                {
                    MessageBox.Show("Выберите должность!");
                }
                
            });
        }
    }
}