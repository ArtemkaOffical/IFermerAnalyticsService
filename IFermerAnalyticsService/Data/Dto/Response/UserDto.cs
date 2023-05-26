namespace IFermerAnalyticsService.Data.Dto.Response
{
    public class Users
    {
        public List<UserDto> UsersList;
    }

    public class UserDto
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public string[] Roles { get; set; }
        public long DateRegistration { get; set; }

    }
public enum Roles {ADMIN,USER, FARMER}
}
