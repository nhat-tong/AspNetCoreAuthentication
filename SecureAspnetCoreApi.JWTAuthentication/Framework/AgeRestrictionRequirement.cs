using Microsoft.AspNetCore.Authorization;

namespace SecureAspnetCoreApi.JWTAuthentication.Framework
{
    public class AgeRestrictionRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; private set; }

        public AgeRestrictionRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
