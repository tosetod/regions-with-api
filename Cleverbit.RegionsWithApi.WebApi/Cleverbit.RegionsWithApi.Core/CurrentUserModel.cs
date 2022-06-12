namespace Cleverbit.RegionsWithApi.Core
{
    public class CurrentUserModel
    {
        public int Id { get; private set; }
        public string Email { get; private set; }

        public CurrentUserModel()
        {
        }

        public CurrentUserModel(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
