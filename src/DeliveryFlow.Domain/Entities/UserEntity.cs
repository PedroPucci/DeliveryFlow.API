using Microsoft.AspNetCore.Identity;

namespace DeliveryFlow.Domain.Entities
{
    public class UserEntity : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
        public override string? UserName { get; set; } = null;
    }
}