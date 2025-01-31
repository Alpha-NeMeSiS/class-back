using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
