using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NextUni.Modules.Users.Application.Abstractions.Identity;

namespace NextUni.Modules.Users.Infrastructure.Identity;

public class KeyCloakClient(HttpClient httpClient, ILogger<KeyCloakClient> logger)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return identityId;
    }
    
    public async Task<IIdentityProviderService.TokenResponse> LoginUserAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var url = "/realms/nextuni/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "nextuni-public-client",
            ["scope"] = "email openid",
            ["username"] = username,
            ["password"] = password
        };

        var content = new FormUrlEncodedContent(formData);

        
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var token = await JsonSerializer.DeserializeAsync<IIdentityProviderService.TokenResponse>(responseStream, cancellationToken: cancellationToken);

        if (token is null)
        {
            throw new InvalidOperationException("Login failed: response is null.");
        }

        return token;
    }
}