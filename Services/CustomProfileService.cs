using identityserver.Storage;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace identityserver.Services
{
    public class CustomProfileService : IProfileService
    {
        protected IRepository _repository;
        protected readonly ILogger Logger;

        public CustomProfileService(IRepository repository)
        {
            _repository = repository;
        }
        public  Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            Logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = _repository.Single<CustomUser>(x => x.SubjectId == context.Subject.GetSubjectId());

            var claims = new List<Claim>
            {
                new Claim("role", "dataEventRecords.admin"),
                new Claim("role", "dataEventRecords.user"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email)
            };

            context.IssuedClaims = claims;
            return Task.CompletedTask;
        }

        public  Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _repository.Single<CustomUser>(x => x.SubjectId == context.Subject.GetSubjectId());
            context.IsActive = user != null;
            return Task.CompletedTask;
        }
    }
}
