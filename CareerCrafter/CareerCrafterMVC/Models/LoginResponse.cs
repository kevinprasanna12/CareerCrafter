namespace CareerCrafterMVC.Models
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
