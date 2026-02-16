namespace Lab_rab4_ImamovaAR_BPI_23_02.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string NameRole { get; set; }

        public Role() { }
        public Role(int id, string name)
        {
            this.Id = id;
            this.NameRole = name;
        }
    }
}