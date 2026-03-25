using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now; // תאריך ביצוע ההזמנה מהספק

        public decimal TotalAmount { get; set; } // סך הכל עלות ההזמנה
        public bool IsReceived { get; set; } = false;

        public decimal ShippingCost { get; set; } = 0;

        public bool IsReturn { get; set; } = false; /*האם זה החזרה או רכישה*/

        // הקשר לספק (ממי הזמנו?)
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } // מאפיין ניווט שיביא את פרטי הספק

        // רשימת הספרים הספציפיים שהוזמנו בהזמנה זו
        public List<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}