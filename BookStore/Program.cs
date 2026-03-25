using BookStore.Components;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //כאן אנחנו אומרים לאפליקציה: "תכירי, יש לנו בסיס נתונים מסוג SQLite, והנה ההגדרות שלו
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // --- קוד אתחול הנתונים למערכת ---
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    // קריאה לפונקציה שיצרנו שממלאת את הנתונים
                    BookStore.Data.DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    // התעלמות משגיאות בהרצה ראשונית
                    Console.WriteLine(ex.Message);
                }
            }
            // --------------------------------

            app.Run();
        }
    }
}
