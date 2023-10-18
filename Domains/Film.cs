﻿using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public class Film
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
