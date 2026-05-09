using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using BLL.Services;
using BLL.Mapping;
using DAL;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PL 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLogging();

            // 2. Налаштовуємо Entity Framework (DAL)
            services.AddDbContext<FinanceDbContext>(options =>
                options.UseSqlite("Data Source=finance.db"));

            // 3. Реєструємо Unit of Work (DAL -> Domain)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 4. Налаштовуємо AutoMapper (BLL)
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // 5. Реєструємо Бізнес-сервіси (BLL)
            services.AddScoped<IFinanceService, FinanceService>();

            // 6. Створюємо провайдер, який видасть нам готовий сервіс
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
                context.Database.EnsureCreated(); // Створює таблиці на основі твоїх Entity
            }

            var financeService = serviceProvider.GetRequiredService<IFinanceService>();

            

            // ЗАПУСК МЕНЮ
            var menu = new ConsoleMenu(financeService);
            menu.Show();
        }
    }
}
