using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous =
        new ClaimsPrincipal(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_anonymous));
    }

    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, username) },
            "custom");

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(
                new ClaimsPrincipal(identity))));
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_anonymous)));
    }
}
