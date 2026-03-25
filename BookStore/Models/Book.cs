using System.ComponentModel.DataAnnotations; //check

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // מזהה ייחודי אוטומטי

        [Required]
        public string Name { get; set; } // 1. שם

        public int StockQuantity { get; set; } // 3. מלאי

        public decimal CostPrice { get; set; } // 4. עלות ספר בודד (מהספק)

        public decimal SellingPrice { get; set; } // 5. מחיר מכירה נוכחי

        public bool IsDiscontinued { get; set; } = false;

        // 6. מחיר מעודכן/מבצע - אפשר להשתמש ב-nullable decimal
        public decimal? DiscountPrice { get; set; }

        // 9. ספק (נוסיף את הקשר לספק כפי שדיברנו)
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}