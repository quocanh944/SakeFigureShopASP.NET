using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SakeFigureShop.Data;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace SakeFigureShop.Domains
{
    public class User : IdentityUser
    {
        public string AvatarUrl { get; set; }

        [StringLength(250, ErrorMessage = "Name is limited to 250 characters in length.", MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
