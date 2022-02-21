namespace Models.ViewModels
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(UserVM user, string token)
        {
            Id = user.UUID;
            FullName = user.fullName;
            Email = user.email;
            Token = token;
        }
    }
}