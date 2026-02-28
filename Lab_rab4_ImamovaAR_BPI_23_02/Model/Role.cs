using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_rab4_ImamovaAR_BPI_23_02.Model
{
    public class Role : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string nameRole;
        public string NameRole
        {
            get => nameRole;
            set { nameRole = value; OnPropertyChanged(); }
        }

        public Role() { }
        public Role(int id, string nameRole) { Id = id; NameRole = nameRole; }

        public Role ShallowCopy() => (Role)this.MemberwiseClone();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}