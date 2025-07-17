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


    public async Task<string> FormatMajorAsync(
        string universityName,
        string majorName,
        string majorCode)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(universityName))
            throw new ArgumentException("University name cannot be null or empty", nameof(universityName));
    
        if (string.IsNullOrWhiteSpace(majorName))
            throw new ArgumentException("Major name cannot be null or empty", nameof(majorName));
    
        // Simulate async operation if needed
        await Task.Delay(1);
    
        // Format with major code if provided
        if (!string.IsNullOrWhiteSpace(majorCode))
        {
            return $"Trường Đại học {universityName} có đào tạo ngành {majorName} ({majorCode})";
        }
    
        return $"Trường Đại học {universityName} có đào tạo ngành {majorName}";
    }
}