namespace NextUni.Modules.Academic.Application.Abstractions.FormatService;

public interface IFormatService
{
    Task<string> FormateUniversityAsync(
        string name,
        string region,
        string universityType,
        string address,
        string email,
        string websiteUrl,
        string facebookUrl);
    
    Task<string> FormatMajorAsync(
        Guid universityId,
        string universityName,
        List<string> majorNames
        );

}