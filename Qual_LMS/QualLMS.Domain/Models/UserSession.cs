namespace QualLMS.Domain.Models
{
    public record UserSession(string? Id, string? Name, string? Email, string? Role);

    public enum Roles
    {
        SuperAdmin = 0,
        Admin = 1,
        Students = 2,
        Teachers = 3,
        Parents = 4,
        None = -1
    }
}
