using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.IdentityServer.ViewModels;

namespace WebShop.IdentityServer.Data
{
    public class AppDbContext : IdentityDbContext // Клиенты и сотрудники
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Товары
        public DbSet<Product> Products { get; set; }

        // Категории товаров
        public DbSet<Category> Categories { get; set; }

        // Заказы
        public DbSet<Order> Orders { get; set; }

        // Корзина товаров
        public DbSet<Basket> Baskets { get; set; }

        // Доставка
        public DbSet<Delivery> Deliveries { get; set; }

    }
}
