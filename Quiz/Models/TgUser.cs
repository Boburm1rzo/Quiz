namespace Quiz.Models
{
    internal class TgUser
    {
        public long TgId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Role UserRole { get; set; }
    }
}
enum Role
{
    ADMIN,
    USER
}
