using IdentityModel;
using identityserver.Storage;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identityserver.Services
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRepository _userRepository;

        public CustomResourceOwnerPasswordValidator(IRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

            var user = _userRepository.Single<CustomUser>(_ => _.UserName == context.UserName);
   
            if (user != null)
            {
                if (user.Password.Equals(context.Password))
                {

                    context.Result = new GrantValidationResult(user.SubjectId, OidcConstants.AuthenticationMethods.Password);

                }
            }

            return Task.FromResult(0);
        }
    }
}
