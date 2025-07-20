using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Infrastructure.Database;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);
        
        builder.HasData(
            Permission.CreateUniversity,
            Permission.GetAdministrativeUniversities,
            Permission.ModifyUniversity,
            Permission.CreateMajor,
            Permission.GetAdministrativeMajors,
            Permission.ModifyMajor,
            Permission.CreateMajorGroup,
            Permission.CreateSubject,
            Permission.GetAdministrativeSubjects,
            Permission.ModifySubject,
            Permission.CreateSubjectGroup,
            Permission.GetAdministrativeSubjectGroups,
            Permission.ModifySubjectGroup,
            Permission.CreateMasterArticle,
            Permission.GetAdministrativeMasterArticles,
            Permission.ModifyMasterArticle,
            Permission.CreateUniversityArticle,
            Permission.GetAdministrativeUniversityArticles,
            Permission.ModifyUniversityArticle,
            Permission.GetStaffUniversityArticles,
            Permission.HideArticle,
            Permission.ProcessUniversityArticle,
            Permission.CreateEvent,
            Permission.GetStaffEvent,
            Permission.GetAdministrativeEvents,
            Permission.ProcessEvents,
            Permission.RegisterEvent,
            Permission.CancelEventRegistration,
            Permission.CreateStaffAccount,
            Permission.DeleteStaffAccount,
            Permission.GetStaffAccountByUniversity,
            Permission.CreateAdmissionScoreByYear
        );

        
        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                // joinBuilder.HasData(
                //     CreateRolePermission(Role.Administrator, Permission.CreateMasterArticle),
                //     CreateRolePermission(Role.Staff, Permission.CreateEvent)
                //     );
                //
                joinBuilder.HasData(
                    CreateRolePermission(Role.Administrator, Permission.CreateUniversity),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeUniversities),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUniversity),
                    CreateRolePermission(Role.Administrator, Permission.CreateSubject),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeSubjects),
                    CreateRolePermission(Role.Administrator, Permission.ModifySubject),
                    CreateRolePermission(Role.Administrator, Permission.CreateSubjectGroup),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeSubjectGroups),
                    CreateRolePermission(Role.Administrator, Permission.ModifySubjectGroup),
                    CreateRolePermission(Role.Administrator, Permission.CreateMajor),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeMajors),
                    CreateRolePermission(Role.Administrator, Permission.ModifyMajor),
                    CreateRolePermission(Role.Administrator, Permission.CreateMajorGroup),
                    CreateRolePermission(Role.Administrator, Permission.CreateAdmissionScoreByYear),
                    CreateRolePermission(Role.Administrator, Permission.CreateMasterArticle),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeMasterArticles),
                    CreateRolePermission(Role.Administrator, Permission.ModifyMasterArticle),
                    CreateRolePermission(Role.Administrator, Permission.ProcessUniversityArticle),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeUniversityArticles),
                    CreateRolePermission(Role.Administrator, Permission.HideArticle),
                    CreateRolePermission(Role.Administrator, Permission.ProcessEvents),
                    CreateRolePermission(Role.Administrator, Permission.GetAdministrativeEvents),
                    CreateRolePermission(Role.Administrator, Permission.CreateStaffAccount),
                    CreateRolePermission(Role.Administrator, Permission.DeleteStaffAccount),
                    CreateRolePermission(Role.Administrator, Permission.GetStaffAccountByUniversity),
                    CreateRolePermission(Role.Staff, Permission.ModifyUniversityArticle),
                    CreateRolePermission(Role.Staff, Permission.CreateUniversityArticle),
                    CreateRolePermission(Role.Staff, Permission.GetStaffUniversityArticles),
                    CreateRolePermission(Role.Staff, Permission.HideArticle),
                    CreateRolePermission(Role.Staff, Permission.CreateEvent),
                    CreateRolePermission(Role.Staff, Permission.ProcessEvents),
                    CreateRolePermission(Role.Staff, Permission.GetStaffEvent),
                    CreateRolePermission(Role.Student, Permission.RegisterEvent),
                    CreateRolePermission(Role.Student, Permission.CancelEventRegistration)
                );
                
            });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
