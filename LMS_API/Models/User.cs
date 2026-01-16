namespace LMS_API.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string UserName { get; set; } = "";

        public string Email { get; set; } = "";

        public string MobileNumber { get; set; } = "";

        public string Address { get; set; } = "";

        public short? RoleID { get; set; }

        public bool Status { get; set; }
        public string? Password { get; set; } = "";
    }
}
