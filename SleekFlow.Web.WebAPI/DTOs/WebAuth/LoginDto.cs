namespace SleekFlow.Web.WebAPI.DTOs.Login
{
    public class LoginDto
    {
        /// <summary>
        /// username
        /// </summary>
        /// <example>admin</example>
        public string Username { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <example>P@ssw0rd</example>
        public string Password { get; set; }
    }
}
