public interface IUserRepository 
{
    public void InsertUser(User user);

    public List<User> GetAllUsers();
}