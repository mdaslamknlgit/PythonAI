using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

public class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;

        var token = context?.Session.GetString("access_token");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            //Nav.NavigateTo("/logn", forceLoad: true);
        }

            return await base.SendAsync(request, cancellationToken);
    }
}
