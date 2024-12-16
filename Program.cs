using HairSaloonScheduler.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and authentication services
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Mysql")));

// Correct authentication scheme setup
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(3);
    });


builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
	options.AddPolicy("UserOnly", policy => policy.RequireClaim("Role", "User"));
});

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Saloon}/{action=Index}");

app.Run();
