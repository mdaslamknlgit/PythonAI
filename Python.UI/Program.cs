using Microsoft.AspNetCore.Authentication.Cookies;
using Python.UI.Components;

var builder = WebApplication.CreateBuilder(args);

// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// REQUIRED for session + handler
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

// Session (to store access_token)
builder.Services.AddSession();

// Authentication & Authorization
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/logn";
        options.AccessDeniedPath = "/logn";
    });

builder.Services.AddAuthorization();

// DelegatingHandler
builder.Services.AddTransient<ApiAuthorizationHandler>();

// HttpClient with handler (FastAPI client)
builder.Services.AddHttpClient("FastApiClient", client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:8000/");
})
.AddHttpMessageHandler<ApiAuthorizationHandler>();

// Controllers (AuthController)
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
