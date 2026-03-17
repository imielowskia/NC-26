using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NC_26.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NC_26Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NC_26Context") ?? throw new InvalidOperationException("Connection string 'NC_26Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
