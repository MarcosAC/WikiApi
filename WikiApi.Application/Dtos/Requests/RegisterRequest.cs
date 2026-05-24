namespace WikiApi.Domain.Dtos.Requests
{
    public record RegisterRequest(string UserName, string Password, string Role);
}
