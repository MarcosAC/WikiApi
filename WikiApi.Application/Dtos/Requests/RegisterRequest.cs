namespace WikiApi.Application.Dtos.Requests
{
    public record RegisterRequest(string UserName, string Password, string Role);
}
