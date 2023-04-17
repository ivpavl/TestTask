using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddSingleton(builder.Configuration);


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
            options.Cookie.HttpOnly = true; // Set HttpOnly flag on session cookie
            options.Cookie.IsEssential = true; // Mark session cookie as essential
        });


builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnectionSQLite")
        );
    // options.UseMySql(
    //     builder.Configuration.GetConnectionString("DefaultConnectionMySQL"), 
    //     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnectionMySQL"))
    //     );
});


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
app.UseSession();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
