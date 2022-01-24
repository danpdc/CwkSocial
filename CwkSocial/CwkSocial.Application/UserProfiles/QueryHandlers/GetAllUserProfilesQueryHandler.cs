using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.Dal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfilesQueryHandler 
        : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _ctx;
        public GetAllUserProfilesQueryHandler(DataContext ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, 
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UserProfile>>();
            result.Payload =  await _ctx.UserProfiles.ToListAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
