using Microsoft.EntityFrameworkCore;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Academic.PublicApi;
using NextUni.Modules.Users.Application.Abstractions.Data;

namespace NextUni.Modules.Users.Application.Staffs.GetStaffAccountByUniversity;

public abstract class GetStaffAccountByUniversity
{
    public record Query(Guid UniversityId) : IQuery<Response>;
    public record Response(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        Guid UniversityId);

    internal sealed class Handler(IUserDbContext dbContext, IUniversityApi publicApi) : IQueryHandler<Query, Response>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var staffId = await  publicApi.GetStaffIdByUniversityIdAsync(request.UniversityId, cancellationToken);

           var staff =  await  dbContext.Users
                .SingleAsync(x => x.Id ==  staffId);

           return new Response(staff.Id, staff.Email, staff.FirstName, staff.LastName, staff.PhoneNumber,
               request.UniversityId);
        }
    }
}