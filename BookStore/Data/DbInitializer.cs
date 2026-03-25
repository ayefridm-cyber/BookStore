using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // 1. מחיקת מסד הנתונים הקיים ויצירתו מחדש (זהירות: מוחק הכל!)
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // אם כבר יש ספקים, סימן שיש נתונים ואין צורך לאתחל
        if (context.Suppliers.Any())
        {
            return;
        }

        var random = new Random();

        // 2. יצירת 5 ספקים
        var suppliers = new List<Supplier>
        {
            new Supplier { Name = "הוצאת כנרת זמורה-ביתן", Phone = "03-5551234", Email = "info@kinneret.co.il", Notes = "חלוקה בימי שני" },
            new Supplier { Name = "ידיעות ספרים", Phone = "03-6669874", Email = "orders@yedioth.co.il", Notes = "הנחה של 10% על הזמנות מעל 1000 שח" },
            new Supplier { Name = "עם עובד", Phone = "03-7774561", Email = "sales@am-oved.co.il", Notes = "אין החזרות אחרי חודש" },
            new Supplier { Name = "הוצאת שוקן", Phone = "03-8881239", Email = "contact@schocken.co.il", Notes = "" },
            new Supplier { Name = "התרבות החדשה", Phone = "02-9993333", Email = "tarbut@newculture.com", Notes = "חלוקה בימי שלישי וחמישי" }
        };

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        // 3. יצירת 50 ספרים (10 לכל ספק)
        var books = new List<Book>();
        string[] adjectives = { "הסוד של", "המדריך המלא ל", "עולמו של", "הקסם של", "תעלומת", "מעבר ל", "החיים של", "סיפורו של" };
        string[] nouns = { "הזמן", "הקוסם", "הים הגדול", "ההרים", "המלך", "המחשבים", "ההיסטוריה", "הנפש", "העתיד" };
        string[] authors = { "דוד כהן", "מיכל לוי", "יוסי ישראלי", "יעל שפירא", "רוני אברהם" };

        foreach (var supplier in suppliers)
        {
            for (int i = 0; i < 10; i++)
            {
                string bookName = $"{adjectives[random.Next(adjectives.Length)]} {nouns[random.Next(nouns.Length)]}";
                decimal cost = random.Next(30, 80);

                books.Add(new Book
                {
                    Name = bookName,
                    SupplierId = supplier.Id,
                    CostPrice = cost,
                    SellingPrice = cost + random.Next(20, 50), // רווח של 20-50 שקלים
                    StockQuantity = random.Next(5, 30),
                    IsDiscontinued = false
                });
            }
        }

        context.Books.AddRange(books);
        context.SaveChanges();

        // 4. יצירת 200 הזמנות (מכירות ללקוחות) פרושות על פני 365 הימים האחרונים
        var orders = new List<Order>();
        string[] customerNames = { "יוסי", "אנה", "דניאל", "נועה", "איתי", "שרה", "לקוח מזדמן", "לקוח מזדמן", "לקוח מזדמן" };
        var payTypes = Enum.GetValues(typeof(PayType)).Cast<PayType>().ToArray();

        for (int i = 0; i < 200; i++)
        {
            // הגרלת תאריך רנדומלי בשנה האחרונה
            DateTime randomDate = DateTime.Now.AddDays(-random.Next(1, 365)).AddHours(-random.Next(1, 10));
            var order = new Order
            {
                CustomerName = customerNames[random.Next(customerNames.Length)],
                OrderDate = randomDate,
                PaymentMethod = payTypes[random.Next(payTypes.Length)],
                HasReceipt = random.Next(100) > 20, // 80% מהלקוחות קיבלו קבלה
                OrderItems = new List<OrderItem>()
            };

            // הגרלת 1 עד 4 ספרים בכל הזמנה
            int itemsCount = random.Next(1, 4);
            decimal orderTotal = 0;

            for (int j = 0; j < itemsCount; j++)
            {
                var randomBook = books[random.Next(books.Count)];
                int qty = random.Next(1, 3);
                decimal price = randomBook.SellingPrice;

                // מונע כפילויות של אותו ספר באותה הזמנה
                if (!order.OrderItems.Any(oi => oi.BookId == randomBook.Id))
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        BookId = randomBook.Id,
                        Quantity = qty,
                        UnitPrice = price
                    });
                    orderTotal += (price * qty);
                }
            }

            // הפעלת הנחה רנדומלית לחלק מההזמנות
            decimal discount = random.Next(100) > 80 ? random.Next(5, 20) : 0;
            order.TotalAmount = Math.Max(0, orderTotal - discount);

            orders.Add(order);
        }

        context.Orders.AddRange(orders);

        // 5. יצירת 50 הזמנות רכש מספקים (הוצאות)
        var purchaseOrders = new List<PurchaseOrder>();
        for (int i = 0; i < 50; i++)
        {
            DateTime randomDate = DateTime.Now.AddDays(-random.Next(1, 365));
            var supplier = suppliers[random.Next(suppliers.Count)];

            var po = new PurchaseOrder
            {
                SupplierId = supplier.Id,
                OrderDate = randomDate,
                IsReceived = true, // כבר הגיעו לחנות
                IsReturn = random.Next(100) > 90, // 10% סיכוי שזה זיכוי/החזרה לספק
                Items = new List<PurchaseOrderItem>()
            };

            int itemsCount = random.Next(3, 8);
            decimal poTotal = 0;
            var supplierBooks = books.Where(b => b.SupplierId == supplier.Id).ToList();

            if (supplierBooks.Any())
            {
                for (int j = 0; j < itemsCount; j++)
                {
                    var randomBook = supplierBooks[random.Next(supplierBooks.Count)];
                    int qty = random.Next(5, 20);

                    if (!po.Items.Any(pi => pi.BookId == randomBook.Id))
                    {
                        po.Items.Add(new PurchaseOrderItem
                        {
                            BookId = randomBook.Id,
                            Quantity = qty,
                            UnitCost = randomBook.CostPrice
                        });
                        poTotal += (randomBook.CostPrice * qty);
                    }
                }
            }

            po.TotalAmount = poTotal + random.Next(0, 50); // תוספת משלוח רנדומלית
            purchaseOrders.Add(po);
        }

        context.PurchaseOrders.AddRange(purchaseOrders);
        context.SaveChanges();
    }
}