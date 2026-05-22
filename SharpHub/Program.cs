using Microsoft.AspNetCore.Authentication.Cookies;
using SharpHub.Models.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Home");
            return Task.CompletedTask;
        };
        options.LogoutPath = "/Home/Index";
        options.ExpireTimeSpan = TimeSpan.FromDays(5);
        options.Cookie.Name = "Super_non_obvious_cookiename_SharpHub";
        options.AccessDeniedPath = "/Home/Index";
    });

MongoManipulator.Initialize(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapBlazorHub();
app.Run();
