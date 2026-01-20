using Microsoft.AspNetCore.Authentication.Cookies;
using Python.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authentication & Authorization (Microsoft way)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/logn";
        options.AccessDeniedPath = "/logn";
    });


builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Auth middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Razor Components routing
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
