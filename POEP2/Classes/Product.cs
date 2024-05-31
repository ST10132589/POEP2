using System;
using System.ComponentModel.DataAnnotations;

namespace POEP2.Classes
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime prodDate { get; set; }

    }
}
