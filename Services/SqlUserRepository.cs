using Microsoft.Data.SqlClient;
public class SqlUserRespository : IUserRepository
{
    public string _connectionString = "";

    public SqlUserRespository(string connectionString) 
    {
        _connectionString = connectionString;
    }

    public void InsertUser(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO [User] (Name, Age, IsActive) VALUES (@Name, @Age, @IsActive)"; 
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Age", user.Age);
                command.Parameters.AddWithValue("@IsActive", user.IsActive);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public List<User> GetAllUsers()
    {
        throw new NotImplementedException();
    }
}
