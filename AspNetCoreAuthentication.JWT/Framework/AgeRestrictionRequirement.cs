using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreAuthentication.JWT.Framework
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
