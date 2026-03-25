using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class PurchaseOrderItem
    {
        [Key]
        public int Id { get; set; }

        // הקשר להזמנת הרכש הכללית
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        // הקשר לספר הספציפי שממנו מזמינים מלאי
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; } // כמה עותקים הזמנו הפעם?

        // עלות הקנייה ליחידה מהספק בעת ביצוע ההזמנה
        public decimal UnitCost { get; set; }
    }
}