using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } // מזהה ההזמנה אוטומטי

        public DateTime OrderDate { get; set; } = DateTime.Now; // 3. תאריך (מקבל את התאריך של עכשיו כברירת מחדל)

        public string CustomerName { get; set; } // 4. שם קונה

        public PayType PaymentMethod { get; set; } // 5. דרך תשלום (אשראי, מזומן, ביט)

        public decimal TotalAmount { get; set; } // 2. מחיר סופי ששולם

        public bool HasReceipt { get; set; } = false;

        // 1. רשימה של 'רכישת ספר בודד'
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}