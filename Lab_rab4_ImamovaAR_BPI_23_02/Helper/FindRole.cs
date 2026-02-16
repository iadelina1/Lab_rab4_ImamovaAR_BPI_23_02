namespace Lab_rab4_ImamovaAR_BPI_23_02.Helper
{
    public class FindRole
    {
        int id;
        public FindRole(int id)
        {
            this.id = id;
        }

        public bool RolePredicate(Model.Role role)
        {
            return role.Id == id;
        }
    }
}