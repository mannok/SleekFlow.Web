namespace SleekFlow.Web.WebAPI.DTOs.Login
{
    public class LoginDto
    {
        /// <summary>
        /// username
        /// </summary>
        /// <example>user1</example>
        public string UserName { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <example>P@ssw0rd</example>
        public string Password { get; set; }
    }
}
