namespace WikiApi.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";

    public User(string userName, string password, string role)
    {
        UserName = userName;
        Password = password;
        Role = role;
    }

    public void UpdatePassword(string newPassword)
    {        
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentNullException("O hash da senha não pode ser nulo ou vazio.");

        Password = newPassword;        
    }
}
