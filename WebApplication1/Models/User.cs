using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
        public class User : IdentityUser
        {
            public string Nom { get; set; }
            public string Prenom { get; set; }
        }
}
