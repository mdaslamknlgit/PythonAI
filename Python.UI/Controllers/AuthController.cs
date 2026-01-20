using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.Text.Json;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("login")]
    public async Task<IActionResult> Login(
      [FromForm] string username,
      [FromForm] string password)
    {
        var client = _httpClientFactory.CreateClient();

        var url =
            $"http://127.0.0.1:8000/auth/login?username={username}&password={password}";

        var response = await client.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
            return Unauthorized();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var accessToken = doc.RootElement
            .GetProperty("access_token")
            .GetString();

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        // store JWT (for API calls later)
        HttpContext.Session.SetString("access_token", accessToken);

        // COOKIE AUTH (THIS IS WHAT MATTERS FOR BLAZOR)
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, username)
    };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        // IMPORTANT: no Redirect here
        return Ok();
    }


    [Microsoft.AspNetCore.Mvc.HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/logn");
    }



}
