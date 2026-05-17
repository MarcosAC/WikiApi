namespace WikiApi.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";

    public User(string userName, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentNullException("O nome de usuário não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException("A senha não pode ser nula ou vazia.");
      
        UserName = userName;
        Password = password;
        Role = string.IsNullOrWhiteSpace(role) ? "User" : role;
    }

    public User() { }

    public void UpdatePassword(string newPassword)
    {        
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentNullException("O hash da senha não pode ser nulo ou vazio.");

        Password = newPassword;        
    }
}
