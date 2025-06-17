using NextUni.Common.Domain;

namespace NextUni.Modules.Users.Domain.Users;

public class User : Entity
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string IdentityId { get; set; } = null!;
    
    public ICollection<Role> Roles { get; set; }  = new List<Role>();
}