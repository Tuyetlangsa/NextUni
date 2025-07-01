using NextUni.Modules.Academic.Application.Abstractions.FormatService;

namespace NextUni.Modules.Academic.Infrastructure.FormatService;

public class FormatService : IFormatService
{
    public async Task<string> FormateUniversityAsync(string name, string region, string universityType, string address, string email,
        string websiteUrl, string facebookUrl)
    {
        return
            $"{name} la 1 trường đại học {universityType} tại {region}, địa chỉ {address}, email {email}, website {websiteUrl}, facebook {facebookUrl}";
    }
}