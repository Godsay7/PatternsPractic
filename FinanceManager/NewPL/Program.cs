using BLL.Mapping;
using BLL.Services;
using DAL;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

//app.Run();

// Підключення БД
builder.Services.AddDbContext<FinanceDbContext>(options => options.UseSqlite("Data Source=finance.db"));

// Реєстрація DI для патерну Unit of Work та Репозиторіїв (DAL)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Реєстрація DI для бізнес-логіки (BLL)
builder.Services.AddScoped<IFinanceService, FinanceService>();

// Реєстрація AutoMapper (якщо він у тебе використовується)
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());