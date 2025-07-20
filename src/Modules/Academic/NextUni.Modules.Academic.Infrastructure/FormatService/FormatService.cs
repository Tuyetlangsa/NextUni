using System.Text;
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

    public async Task<string> FormatAdmissionScoresAsync(string universityName, List<Major> majors, int year)
    {
        if (string.IsNullOrWhiteSpace(universityName))
            throw new ArgumentException("University name cannot be null or empty", nameof(universityName));

        if (majors == null || majors.Count == 0)
            throw new ArgumentException("Majors list cannot be null or empty", nameof(majors));

        var sb = new StringBuilder();
        sb.AppendLine($"Admission scores of {universityName} in {year}:");
        
        foreach (var major in majors)
        {
            var admissionScore = major.AdmissionScore.FirstOrDefault(s => s.Year.Year == year);
            if (admissionScore != null)
            {
                sb.AppendLine($"- {major.Name} (Code: {major.Code}): GPA Score = {admissionScore.GpaScore}, Exam Score = {admissionScore.ExamScore}");
            }
        }

        return sb.ToString();
    }
}