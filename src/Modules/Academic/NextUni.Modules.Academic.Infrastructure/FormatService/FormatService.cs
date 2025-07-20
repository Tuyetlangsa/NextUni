using NextUni.Modules.Academic.Application.Abstractions.FormatService;
using NextUni.Modules.Academic.Domain.Majors;

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
        Guid universityId,
        string universityName,
        List<string> majorNames)
    {
        if (string.IsNullOrWhiteSpace(universityName))
            throw new ArgumentException("University name cannot be null or empty", nameof(universityName));

        if (majorNames == null || majorNames.Count == 0)
            throw new ArgumentException("Major names list cannot be null or empty", nameof(majorNames));

        string formattedMajors;
        if (majorNames.Count == 1)
        {
            formattedMajors = majorNames[0];
        }
        else if (majorNames.Count == 2)
        {
            formattedMajors = $"{majorNames[0]} và {majorNames[1]}";
        }
        else
        {
            formattedMajors = string.Join(", ", majorNames.Take(majorNames.Count - 1)) +
                              $" và {majorNames.Last()}";
        }

        return $"Trường Đại học {universityName} có đào tạo các ngành {formattedMajors}.";
    }

    public Task<string> FormatAdmissionScoresAsync(string universityName, List<Major> majors, int year)
    {
        throw new NotImplementedException();
    }
}