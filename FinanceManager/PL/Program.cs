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

            services.AddDbContext<FinanceDbContext>(options =>
                options.UseSqlite("Data Source=finance.db"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            services.AddScoped<IFinanceService, FinanceService>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
                context.Database.EnsureCreated();
            }

            var financeService = serviceProvider.GetRequiredService<IFinanceService>();

            
            var menu = new ConsoleMenu(financeService);
            menu.Show();
        }
    }
}
