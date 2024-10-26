using Bank_Web_App.Services;
using BankApplication_Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GitSecrets.json"), optional: true);
string? connectionString = builder.Configuration.GetConnectionString("BankApplication") ?? builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<BankContext>(options => options.UseMySQL(connectionString));

builder.Services.AddScoped<BankService, BankService>();
builder.Services.AddScoped<UserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
