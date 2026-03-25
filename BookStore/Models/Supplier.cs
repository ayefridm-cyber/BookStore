using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // שם הספק

        public string? Phone { get; set; } // טלפון איש קשר

        public string? Email { get; set; } // אימייל להזמנות

        public string? Notes { get; set; }

        // מאפייני ניווט: מאפשרים לנו לשלוף בקלות את כל הספרים של הספק הזה
        public List<Book> Books { get; set; } = new List<Book>();

        // מאפשר לשלוף את כל היסטוריית ההזמנות שעשינו מהספק הזה
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}