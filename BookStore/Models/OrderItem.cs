using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        // הקשר להזמנה הכללית (לאיזו קבלה זה שייך)
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // 1. הקשר לספר הספציפי
        public int BookId { get; set; }
        public Book Book { get; set; }

        // 3. כמות
        public int Quantity { get; set; }

        // 4. מחיר (המחיר ליחידה בעת המכירה)
        public decimal UnitPrice { get; set; }

    }
}