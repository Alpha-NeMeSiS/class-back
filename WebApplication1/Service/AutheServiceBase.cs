using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AutheServiceBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
    }
}